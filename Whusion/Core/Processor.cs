using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whusion.Core
{
    public class Processor
    {
        public List<Action<IGlobalContext>> PreRenderingActions { get; }
        public List<Action<IGlobalContext>> PostRenderingActions { get; }
        public Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>> ElementRenderingActions { get; internal set; }

        public Processor()
        {
            PreRenderingActions = new List<Action<IGlobalContext>>();
            PostRenderingActions = new List<Action<IGlobalContext>>();
            ElementRenderingActions = new Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>>();
        }

        public Processor(params Processor[] plugins) : this()
        {
            foreach (var plugin in plugins) Combine(plugin);
        }

        public void PreRender(IGlobalContext context)
        {
            foreach (var action in PreRenderingActions)
                action(context);
        }

        public void PostRender(IGlobalContext context)
        {
            foreach (var action in PostRenderingActions)
                action(context);
        }

        public void ElementRender(IElementContext context, OpenXmlElement element)
        {
            List<Action<IElementContext, OpenXmlElement>> processorActions;
            if (ElementRenderingActions.TryGetValue(element.GetType(), out processorActions))
            {
                foreach(var action in processorActions)
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
                List<Action<IElementContext, OpenXmlElement>> current;
                if (ElementRenderingActions.TryGetValue(type, out current))
                    current.AddRange(processor.ElementRenderingActions[type]);
                else
                    ElementRenderingActions.Add(type, processor.ElementRenderingActions[type]);
            }
        }
    }
}
