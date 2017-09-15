using Autofac;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Numbering
{
    public class NumberingProcessorPlugin
    {
        private WordprocessingDocument _wpDoc;

        public NumberingProcessorPlugin(WordprocessingDocument wpDoc)
        {
            _wpDoc = wpDoc;
        }


        [InitializeEngine]
        public void InitEngine(ContainerBuilder builder)
        {
            builder
                .Register(x => new NumberingMapper(_wpDoc))
                .SingleInstance();
        }

        [ElementProcessing]
        public void InsertNumbering(IElementContext context, Paragraph p)
        {
            var numberingMapper = context.GlobalContext.Container.Resolve<NumberingMapper>();

            var numbering = numberingMapper.GetNumbering(p);
            if (numbering != null)
            {
                //context.AddMutation(new TextInsertion
                //{
                //    Text = numbering.Verbose,
                //    Index = context.TextIndex
                //});
            }
        }
    }
}
