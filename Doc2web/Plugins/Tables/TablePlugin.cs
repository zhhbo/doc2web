using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Tables
{
    public class TablePlugin
    {
        private TableConfig _tableConfig;

        public TablePlugin(TableConfig tableConfig)
        {
            _tableConfig = tableConfig;
        }

        public TablePlugin() : this(new TableConfig()) { }

        [ElementProcessing]
        public void ProcessTable(IElementContext context, Table table)
        {
            DeleteAllText(context, table);
            AddWarningNode(context);
        }

        private static void DeleteAllText(IElementContext context, Table table)
        {
            context.AddMutation(new TextDeletion
            {
                Count = table.InnerText.Length,
                Position = 0
            });
        }

        private void AddWarningNode(IElementContext context)
        {
            var node = new HtmlNode
            {
                Start = 0,
                End = 0,
                TextPrefix = _tableConfig.WarningMessage
            };
            node.AddClasses(_tableConfig.WarningCssClass);
            context.AddNode(node);
        }

        [PostProcessing]
        public void PostProcessing(IGlobalContext context)
        {
            context.AddCss(CSS(_tableConfig.WarningCssClass));
        }

        public static string CSS(string tableWarningCssClass) =>
            $".{tableWarningCssClass} " +
            "{ background-color: #fcc3b8; color: #f23b18; border: 1px solid #f23b18; " +
            "padding: 10px; margin: 25px; text-align: center; font-size: 11pt; " +
            "font-family: courier new; }";
    }
}
