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


        public Processor BuildSingle(object processorConfig)
        {
            var preProcessingActions = GetPrePostProcessingActions<PreProcessingAttribute>(processorConfig);
            var postProcessingActions = GetPrePostProcessingActions<PostProcessingAttribute>(processorConfig);
            var elementProcessingActions = BuildElementsActions(processorConfig);

            var plugin = new Processor();
            plugin.PreRenderingActions.AddRange(preProcessingActions);
            plugin.PostRenderingActions.AddRange(postProcessingActions);
            plugin.ElementRenderingActions = elementProcessingActions;

            return plugin;
        }

        private Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>> BuildElementsActions(object processorConfig)
        {
            var elementProcessingAction = new Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>>();
            foreach (var (type, action) in GetElemActionFromAttributes(processorConfig))
                AddElementProcessingAction(elementProcessingAction, type, action);

            return elementProcessingAction;
        }

        private void AddElementProcessingAction(Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>> container, Type type, Action<IElementContext, OpenXmlElement> action)
        {
            if (container.ContainsKey(type))
                container[type].Add(action);
            else
            {
                var list = new List<Action<IElementContext, OpenXmlElement>>();
                list.Add(action);
                container.Add(type, list);
            }
        }

        private Action<IGlobalContext>[] GetPrePostProcessingActions<AttributeType>(object processorConfig)
            where AttributeType : Attribute =>
            processorConfig.GetType().GetMethods()
            .Where(IsValidPrePostProcessingMethod<AttributeType>)
            .Select(x => new Action<IGlobalContext>(y => x.Invoke(processorConfig, new object[] { y })))
            .ToArray();

        private bool IsValidPrePostProcessingMethod<T>(MethodInfo m) 
            where T : Attribute =>
            (HasAttribute<T>(m))? HasValidPrePostProcessingParameters(m) : false;

        private (Type, Action<IElementContext, OpenXmlElement>)[] GetElemActionFromAttributes(object processorConfig)
        {
            var m = processorConfig.GetType().GetMethods().ToArray();
            return 
                m
                .Where(IsValidElementProcessingMethod)
                .Select(x =>
                {
                    var elementType = x.GetParameters().Last().ParameterType;
                    var action =
                        new Action<IElementContext, OpenXmlElement>(
                            (context, elem) => x.Invoke(processorConfig, new object[] { context, elem }));
                    return (elementType, action);
                })
                .ToArray();
        }

        private bool IsValidElementProcessingMethod(MethodInfo methodInfo) =>
            HasAttribute<ElementProcessingAttribute>(methodInfo) && HasValidElementProcessingParameters(methodInfo);

        private bool HasAttribute<T>(MethodInfo methodInfo)
            where T : Attribute =>
            methodInfo.GetCustomAttributes<T>(true).Any();

        private bool HasValidPrePostProcessingParameters(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 1 &&
                parameters[0].ParameterType == typeof(IGlobalContext))
                return true;

            throw new ArgumentException($"{methodInfo.Name} has a Pre/Post Processing attribute with invalid parameters");
        }


        private bool HasValidElementProcessingParameters(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 2 &&
                parameters[0].ParameterType == typeof(IElementContext) &&
                typeof(OpenXmlElement).IsAssignableFrom(parameters[1].ParameterType))
                return true;

            throw new ArgumentException($"The {methodInfo.Name} an ElementProcessing attribute with invalid parameters.");
        }
    }
}
