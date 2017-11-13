using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.TextFixes
{
    public class InstrTxtCleanupPlugin
    {
        [ElementProcessing]
        public void ProcessElement(IElementContext context, Paragraph paragraph)
        {
            var ti = 0;
            foreach(var r in paragraph.Elements<Run>())
            {
                int l = r.InnerText.Length;
                if (r.LastChild is FieldCode f)
                {
                    context.AddMutation(new TextDeletion
                    {
                        Count = l,
                        Position = ti
                    });
                }
                ti += l;
            }
        }
    }
}
