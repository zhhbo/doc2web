using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Whusion.Core
{
    public class ProcessorFactory
    {

        public Processor BuildMultiple(params object[] inputObject) =>
            new Processor(inputObject.Select(BuildSingle).ToArray());


        public Processor BuildSingle(object inputObject)
        {
            var preProcessingActions = GetPrePostActionsFromAttribute<PreProcessingAttribute, IGlobalContext>(inputObject);
            var postProcessingActions = GetPrePostActionsFromAttribute<PostProcessingAttribute, IGlobalContext>(inputObject);

            var elementProcessingAction = new Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>>();
            foreach (var (type, action) in GetElemActionFromAttribute(inputObject))
            {
                if (elementProcessingAction.ContainsKey(type))
                    elementProcessingAction[type].Add(action);
                else
                {
                    var list = new List<Action<IElementContext, OpenXmlElement>>();
                    list.Add(action);
                    elementProcessingAction.Add(type, list);
                }
            }

            var plugin = new Processor();
            plugin.PreRendering.AddRange(preProcessingActions);
            plugin.PostRendering.AddRange(postProcessingActions);
            plugin.ElementRendering = elementProcessingAction;

            return plugin;
        }


        private Action<ActionParams>[] GetPrePostActionsFromAttribute<AttributeType, ActionParams>(object plugin)
            where AttributeType : Attribute =>
            plugin.GetType().GetMethods()
            .Where(IsValidPrePostProcessingMethod<AttributeType>)
            .Select(x => new Action<ActionParams>(y => x.Invoke(plugin, new object[] { y })))
            .ToArray();

        private bool IsValidPrePostProcessingMethod<T>(MethodInfo m) 
            where T : Attribute =>
            (HasAttribute<T>(m))? HasValidPrePostProcessingParameters(m) : false;

        private (Type, Action<IElementContext, OpenXmlElement>)[] GetElemActionFromAttribute(object plugin)
        {
            var m = plugin.GetType().GetMethods().ToArray();
            return 
                m
                .Where(IsValidElementProcessingMethod)
                .Select(x =>
                {
                    var elementType = x.GetParameters().Last().ParameterType;
                    var action =
                        new Action<IElementContext, OpenXmlElement>(
                            (context, elem) => x.Invoke(plugin, new object[] { context, elem }));
                    return (elementType, action);
                })
                .ToArray();
        }

        private bool IsValidElementProcessingMethod(MethodInfo m) =>
            HasAttribute<ElementProcessingAttribute>(m) && HasValidElementProcessingParameters(m);

        private bool HasAttribute<T>(MethodInfo m)
            where T : Attribute =>
            m.GetCustomAttributes<T>(true).Any();

        private bool HasValidPrePostProcessingParameters(MethodInfo m)
        {
            var parameters = m.GetParameters();
            if (parameters.Length == 1 &&
                parameters[0].ParameterType == typeof(IGlobalContext))
                return true;

            throw new ArgumentException($"{m.Name} has a Pre/Post Processing attribute with invalid parameters");
        }


        private bool HasValidElementProcessingParameters(MethodInfo m)
        {
            var parameters = m.GetParameters();
            if (parameters.Length == 2 &&
                parameters[0].ParameterType == typeof(IElementContext) &&
                typeof(OpenXmlElement).IsAssignableFrom(parameters[1].ParameterType))
                return true;

            throw new ArgumentException($"The {m.Name} an ElementProcessing attribute with invalid parameters.");
        }
    }
}
