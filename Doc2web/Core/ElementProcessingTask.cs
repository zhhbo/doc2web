using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Core
{
    public class ElementProcessingTask : IContextNestingHandler
    {
        private List<INestableElementContext> _contexts = new List<INestableElementContext>();
        private IProcessor _processor;

        public ElementProcessingTask(INestableElementContext context, IProcessor processor)
        {
            context.NestingHandler = this;
            _processor = processor;
            _contexts.Add(context);
        }

        public void QueueElementProcessing(INestableElementContext context)
        {
            if (_contexts.Any(x => x.Element == context.Element)) return;
            _contexts.Add(context);
        }

        public void Execute()
        {
            int i = 0;
            while (i < _contexts.Count)
            {
                var current = _contexts[i];
                _processor.ProcessElement(current, current.Element);
                i++;
            }
        }

    }
}
