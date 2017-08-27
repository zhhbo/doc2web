using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Doc2web.Core
{
    public class ProcessorFactory
    {
        public Processor BuildMultiple(params object[] inputObject) =>
            new Processor(inputObject.Select(BuildSingle).ToArray());


        public Processor BuildSingle(object plugin)
        {
            var initEngineActions = GetInitializeEngineActions(plugin);
            var initProcessActions = GetInitializeProcessingActions(plugin);
            var preProcessingActions = GetPreProcessingActions(plugin);
            var postProcessingActions = GetPostProcessingActions(plugin);
            var elementProcessingActions = GetElementProcessingActions(plugin);

            var processor = new Processor();
            processor.InitEngineActions.AddRange(initEngineActions);
            processor.InitProcessActions.AddRange(initProcessActions);
            processor.PreProcessActions.AddRange(preProcessingActions);
            processor.PostProcessActions.AddRange(postProcessingActions);
            processor.ElementRenderingActions = elementProcessingActions;

            return processor;
        }

        #region Initialize engine/processing

        private Action<ContainerBuilder>[] GetInitializeEngineActions(object plugin) =>
            plugin.GetType().GetMethods()
            .Where(IsValidInitEngineMethod)
            .Select(x => BuildInitializeAction(plugin, x))
            .ToArray();

        private Action<ContainerBuilder>[] GetInitializeProcessingActions(object plugin) =>
            plugin.GetType().GetMethods()
            .Where(IsValidInitProcessingMethod)
            .Select(x => BuildInitializeAction(plugin, x))
            .ToArray();

        private bool IsValidInitEngineMethod(MethodInfo m) =>
            (HasAttribute<InitializeEngineAttribute>(m)) ? HasValidInitProcessingParameters(m) : false;

        private bool IsValidInitProcessingMethod(MethodInfo m) =>
            (HasAttribute<InitializeProcessingAttribute>(m)) ? HasValidInitProcessingParameters(m) : false;

        private bool HasValidInitProcessingParameters(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 1 &&
                parameters[0].ParameterType == typeof(ContainerBuilder))
                return true;

            throw new ArgumentException($"The {methodInfo.Name} an InitializeProcessing attribute with invalid parameters.");
        }

        private static Action<ContainerBuilder> BuildInitializeAction(object processorConfig, MethodInfo x) =>
             new Action<ContainerBuilder>(
                (containerBuilder) => x.Invoke(processorConfig, new object[] { containerBuilder }));


        #endregion

        #region Pre/Post processing

        private Action<IGlobalContext>[] GetPreProcessingActions(object plugin) =>
            plugin.GetType().GetMethods()
            .Where(IsValidPreProcessingMethod)
            .Select(x => BuildProcessingAction(plugin, x))
            .ToArray();

        private Action<IGlobalContext>[] GetPostProcessingActions(object plugin) =>
            plugin.GetType().GetMethods()
            .Where(IsValidPostProcessingMethod)
            .Select(x => BuildProcessingAction(plugin, x))
            .ToArray();

        private bool IsValidPreProcessingMethod(MethodInfo m) =>
            (HasAttribute<PreProcessingAttribute>(m)) ? HasValidPreProcessingParameters(m) : false;

        private bool IsValidPostProcessingMethod(MethodInfo m) =>
            (HasAttribute<PostProcessingAttribute>(m)) ? HasValidPostProcessingParameters(m) : false;


        private bool HasValidPreProcessingParameters(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 1 &&
                parameters[0].ParameterType == typeof(IGlobalContext))
                return true;

            throw new ArgumentException($"The {methodInfo.Name} an ElementProcessing attribute with invalid parameters.");
        }

        private bool HasValidPostProcessingParameters(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 1 &&
                parameters[0].ParameterType == typeof(IGlobalContext))
                return true;

            throw new ArgumentException($"{methodInfo.Name} has a Pre/Post Processing attribute with invalid parameters");
        }

        private static Action<IGlobalContext> BuildProcessingAction(object processorConfig, MethodInfo x) =>
             new Action<IGlobalContext>(
                (context) => x.Invoke(processorConfig, new object[] { context }));

        #endregion

        #region Element processing

        private Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>> GetElementProcessingActions(object plugin)
        {
            var elementProcessingAction = new Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>>();
            foreach (var (type, action) in BuildElementProcessingActions(plugin))
                AddElementProcessingAction(elementProcessingAction, type, action);

            return elementProcessingAction;
        }

        private (Type, Action<IElementContext, OpenXmlElement>)[] BuildElementProcessingActions(object plugin) =>
            plugin.GetType().GetMethods()
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

        private bool IsValidElementProcessingMethod(MethodInfo methodInfo) =>
            HasAttribute<ElementProcessingAttribute>(methodInfo) && HasValidElementProcessingParameters(methodInfo);

        private bool HasValidElementProcessingParameters(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 2 &&
                parameters[0].ParameterType == typeof(IElementContext) &&
                typeof(OpenXmlElement).IsAssignableFrom(parameters[1].ParameterType))
                return true;

            throw new ArgumentException($"The {methodInfo.Name} an ElementProcessing attribute with invalid parameters.");
        }

        private void AddElementProcessingAction(Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>> container, Type type, Action<IElementContext, OpenXmlElement> action)
        {
            if (container.ContainsKey(type))
                container[type].Add(action);
            else
            {
                var list = new List<Action<IElementContext, OpenXmlElement>>
                {
                    action
                };
                container.Add(type, list);
            }
        }

        #endregion

        private bool HasAttribute<T>(MethodInfo methodInfo) where T : Attribute =>
            methodInfo.GetCustomAttributes<T>(true).Any();


    }
}
