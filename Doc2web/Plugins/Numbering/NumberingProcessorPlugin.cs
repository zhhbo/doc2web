using Autofac;
using Doc2web.Plugins.Style;
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
        private NumberingProcessorPluginConfig _config;

        public NumberingProcessorPlugin(WordprocessingDocument wpDoc) : this(wpDoc, new NumberingProcessorPluginConfig())
        {

        }

        public NumberingProcessorPlugin(WordprocessingDocument wpDoc, NumberingProcessorPluginConfig config)
        {
            _wpDoc = wpDoc;
            _config = config;
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
                var cssRegistrator = context.GlobalContext.Container.Resolve<ICssRegistrator>();
                var cssClass = cssRegistrator.Register(numbering.NumberingId, numbering.LevelIndex);

                context.AddNode(BuildNumberingContainer(cssClass));
                context.AddNode(BuildNumberingNumber());
                context.AddMutation(BuildNumberingInsertion(numbering.Verbose));
            }
        }


        private HtmlNode BuildNumberingContainer(string cssClass)
        {
            var node = new HtmlNode
            {
                Start = _config.NumberingIndex,
                End = _config.NumberingIndex + _config.NumberingDelta * 4,
                Tag = _config.NumberingContainerTag,
                Z = _config.NumberingContainerZ,
            };
            node.AddClass(_config.NumberingContainerCls);
            node.AddClass(cssClass);
            return node;
        }

        private HtmlNode BuildNumberingNumber()
        {
            var node = new HtmlNode
            {
                Start = _config.NumberingIndex + _config.NumberingDelta,
                End = _config.NumberingIndex + _config.NumberingDelta * 3,
                Tag = _config.NumberingNumberTag,
                Z = _config.NumberingNumberZ,
            };
            node.AddClass(_config.NumberingNumberCls);
            return node;
        }

        private Mutation BuildNumberingInsertion(string verbose) => new TextInsertion
        {
            Position = _config.NumberingIndex + _config.NumberingDelta * 2,
            Text = verbose
        };
    }
}
