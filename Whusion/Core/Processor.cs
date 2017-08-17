using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whusion.Core
{
    public class Processor
    {
        public List<Action<IGlobalContext>> PreRendering { get; set; }
        public List<Action<IGlobalContext>> PostRendering { get; set; }
        public Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>> ElementRendering { get; set; }

        public Processor()
        {
            PreRendering = new List<Action<IGlobalContext>>();
            PostRendering = new List<Action<IGlobalContext>>();
            ElementRendering = new Dictionary<Type, List<Action<IElementContext, OpenXmlElement>>>();
        }

        public Processor(params Processor[] plugins) : this()
        {
            foreach (var plugin in plugins) Combine(plugin);
        }

        public void PreRender(IGlobalContext context)
        {
            foreach (var action in PreRendering)
                action(context);
        }

        public void PostRender(IGlobalContext context)
        {
            foreach (var action in PostRendering)
                action(context);
        }

        public void ElementRender(IElementContext context, OpenXmlElement element)
        {
            List<Action<IElementContext, OpenXmlElement>> processorActions;
            if (ElementRendering.TryGetValue(element.GetType(), out processorActions))
            {
                foreach(var action in processorActions)
                    action(context, element);
            }
        }

        private void Combine(Processor other)
        {
            AddOtherElementRendering(other);
            AddOtherPrePostRendering(other);
        }

        private void AddOtherPrePostRendering(Processor other)
        {
            PreRendering.AddRange(other.PreRendering);
            PostRendering.AddRange(other.PostRendering);
        }

        private void AddOtherElementRendering(Processor other)
        {
            foreach (var type in other.ElementRendering.Keys)
            {
                List<Action<IElementContext, OpenXmlElement>> current;
                if (ElementRendering.TryGetValue(type, out current))
                {
                    current.AddRange(other.ElementRendering[type]);
                }
                else
                {
                    ElementRendering.Add(type, other.ElementRendering[type]);
                }
            }
        }
    }
}
