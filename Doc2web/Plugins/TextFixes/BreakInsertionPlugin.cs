using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;

namespace Doc2web.Plugins.TextFixes
{
    public class BreakInsertionPlugin
    {
        private BreakInsertionConfig _config;

        public BreakInsertionPlugin() : this(new BreakInsertionConfig()) { }

        public BreakInsertionPlugin(BreakInsertionConfig config)
        {
            _config = config;
        }

        [ElementProcessing]
        public void BrInsertion(IElementContext context, Break br)
        {
            InsertBrAtCurrentPosition(context);
        }

        [ElementProcessing]
        public void CrInsertion(IElementContext context, CarriageReturn cr)
        {
            InsertBrAtCurrentPosition(context);
        }

        private void InsertBrAtCurrentPosition(IElementContext context)
        {
            if (context.TextIndex == 0)
                FixContainer(context);

            else
                context.AddNode(new HtmlNode
                {
                    Start = context.TextIndex,
                    Tag = "br"
                });
        }

        private void FixContainer(IElementContext context)
        {
            var container =
                context.Nodes
                .FirstOrDefault(IsContainer);
            if (container != null)
                container.AddClass(_config.BreakAtStartCls);
        }

        private bool IsContainer(HtmlNode n) =>
            n.Z == _config.ContainerZ &&
            n.Tag == _config.ContainerTag &&
            (_config.ContainerCls != "" ? n.Classes.Contains(_config.ContainerCls) : true);


        [PostProcessing]
        public void PostProcessing(IGlobalContext context)
        {
            context.AddCss(CSS);
        }

        private string BreakAtStartSelector =>
            $".{_config.ContainerCls}.{_config.BreakAtStartCls}";
        private string BreakAtStartLeftSpacerSelector =>
            $"{BreakAtStartSelector} .{_config.NumberingContainerCls}";

        private string BreakAtStartNumberingSelector =>
            $"{BreakAtStartSelector} .{_config.LeftSpacerCls}";

        private string CSS =>
            $"{BreakAtStartSelector} {{flex-direction: column;}}" +
            $"{BreakAtStartNumberingSelector}, {BreakAtStartLeftSpacerSelector} {{display: inline-block;}}" +
            $"{BreakAtStartNumberingSelector} * {{padding: 0!important;}}";

    }
}
