using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core
{
    public class Processor : IProcessor
    {
        public List<Action<IGlobalContext, ContainerBuilder>> PreRenderingActions { get; }
        public List<Action<IGlobalContext>> PostRenderingActions { get; }
        public Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>> ElementRenderingActions { get; internal set; }

        public Processor()
        {
            PreRenderingActions = new List<Action<IGlobalContext, ContainerBuilder>>();
            PostRenderingActions = new List<Action<IGlobalContext>>();
            ElementRenderingActions = new Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>>();
        }

        public Processor(params Processor[] plugins) : this()
        {
            foreach (var plugin in plugins) Combine(plugin);
        }

        public void PreProcess(IGlobalContext context, ContainerBuilder containerBuilder)
        {
            foreach (var action in PreRenderingActions)
                action(context, containerBuilder);
        }

        public void PostProcess(IGlobalContext context)
        {
            foreach (var action in PostRenderingActions)
                action(context);
        }

        public void ProcessElement(IElementContext context, OpenXmlElement element)
        {
            if (ElementRenderingActions.TryGetValue(element.GetType(), 
                out List<Action<IElementContext, OpenXmlElement>> processorActions))
            {
                foreach (var action in processorActions)
                    action(context, element);
            }
        }

        private void Combine(Processor other)
        {
            AddElementProcessingActionFromProcessor(other);
            AddPrePostRenderingActionsFromProcessor(other);
        }

        private void AddPrePostRenderingActionsFromProcessor(Processor processor)
        {
            PreRenderingActions.AddRange(processor.PreRenderingActions);
            PostRenderingActions.AddRange(processor.PostRenderingActions);
        }

        private void AddElementProcessingActionFromProcessor(Processor processor)
        {
            foreach (var type in processor.ElementRenderingActions.Keys)
            {
                if (ElementRenderingActions.TryGetValue(type, 
                    out List<Action<IElementContext, OpenXmlElement>> current))
                    current.AddRange(processor.ElementRenderingActions[type]);
                else
                    ElementRenderingActions.Add(type, processor.ElementRenderingActions[type]);
            }
        }
    }
}
