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

        public double NumberingIndex { get; set; }
        public string NumberingContainerTag { get; set; }
        public string NumberingContainerCls { get; set; }
        public int NumberingContainerZ { get; set; }
        public string NumberingNumberTag { get; set; }
        public string NumberingNumberCls { get; set; }
        public int NumberingNumberZ { get; set; }

        public NumberingProcessorPlugin(WordprocessingDocument wpDoc)
        {
            _wpDoc = wpDoc;
            NumberingIndex = double.MinValue + double.Epsilon;
            NumberingContainerTag = "div";
            NumberingContainerCls = "numbering-container";
            NumberingContainerZ = 900;
            NumberingNumberTag = "span";
            NumberingNumberCls = "numbering-number";
            NumberingNumberZ = 899;
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
                context.AddNode(BuildNumberingContainer());
                context.AddNode(BuildNumberingNumber());
            }
        }

        private HtmlNode BuildNumberingContainer()
        {
            var node = new HtmlNode
            {
                Start = NumberingIndex,
                End = NumberingIndex,
                Tag = NumberingContainerTag,
                Z = NumberingContainerZ,
            };
            node.AddClass(NumberingContainerCls);
            return node;
        }

        private HtmlNode BuildNumberingNumber()
        {
            var node = new HtmlNode
            {
                Start = NumberingIndex,
                End = NumberingIndex,
                Tag = NumberingNumberTag,
                Z = NumberingNumberZ,
            };
            node.AddClass(NumberingNumberCls);
            return node;
        }
    }
}
