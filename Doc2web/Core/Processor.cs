using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core
{
    public class Processor : IProcessor
    {
        public List<Action<ContainerBuilder>> InitEngineActions { get; private set; }
        public List<Action<ContainerBuilder>> InitProcessActions { get; private set; }
        public List<Action<IGlobalContext>> PreProcessActions { get; }
        public List<Action<IGlobalContext>> PostProcessActions { get; }
        public Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>> ElementRenderingActions { get; internal set; }

        public Processor()
        {
            InitEngineActions = new List<Action<ContainerBuilder>>(); 
            InitProcessActions = new List<Action<ContainerBuilder>>();
            PreProcessActions = new List<Action<IGlobalContext>>();
            PostProcessActions = new List<Action<IGlobalContext>>();
            ElementRenderingActions = new Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>>();
        }

        public Processor(params Processor[] plugins) : this()
        {
            foreach (var plugin in plugins) Combine(plugin);
        }

        public void InitEngine(ContainerBuilder containerBuilder)
        {
            foreach (var action in InitEngineActions)
                action(containerBuilder);
        }

        public void InitProcess(ContainerBuilder containerBuilder)
        {
            foreach (var action in InitProcessActions)
                action(containerBuilder);
        }

        public void PreProcess(IGlobalContext context)
        {
            foreach (var action in PreProcessActions)
                action(context);
        }

        public void PostProcess(IGlobalContext context)
        {
            foreach (var action in PostProcessActions)
                action(context);
        }

        public void ProcessElement(Doc2web.IElementContext context, OpenXmlElement element)
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
            AddInitPrePostActionsFromProcessor(other);
        }

        private void AddInitPrePostActionsFromProcessor(Processor processor)
        {
            InitEngineActions.AddRange(processor.InitEngineActions);
            InitProcessActions.AddRange(processor.InitProcessActions);
            PreProcessActions.AddRange(processor.PreProcessActions);
            PostProcessActions.AddRange(processor.PostProcessActions);
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
