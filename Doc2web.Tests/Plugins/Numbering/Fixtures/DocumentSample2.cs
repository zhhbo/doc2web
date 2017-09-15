using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Tests.Plugins.Numbering.Fixtures
{
    public static class DocumentSample2
    {
        public static Body GenerateBody() =>
          new Body(
            GenerateParagraph(0, "1 0"),
            GenerateParagraph(0, "1 1"),
            GenerateParagraph(1, "2 0"),
            GenerateParagraph(1, "2 1"),
            GenerateParagraph(2, "3 0"),
            GenerateParagraph(2, "3 1"),
            GenerateParagraph(1, "2 2"),
            GenerateParagraph(1, "2 3"),
            GenerateParagraph(2, "3 0"),
            GenerateParagraph(2, "3 1"),
            GenerateParagraph(0, "1 2")
          );

        // Creates an Paragraph instance and adds its children.
        public static Paragraph GenerateParagraph(int numberingLevel, string innerText, bool skip = false)
        {
            Paragraph paragraph1 = new Paragraph() { RsidParagraphAddition = "00D14869", RsidParagraphProperties = "00D14869", RsidRunAdditionDefault = "00D14869" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId() { Val = "ListParagraph" };

            NumberingProperties numberingProperties1 = new NumberingProperties();
            NumberingLevelReference numberingLevelReference1 = new NumberingLevelReference() { Val = numberingLevel };
            NumberingId numberingId1 = new NumberingId() { Val = (skip) ? 0 : 1 };

            numberingProperties1.Append(numberingLevelReference1);
            numberingProperties1.Append(numberingId1);

            paragraphProperties1.Append(paragraphStyleId1);
            paragraphProperties1.Append(numberingProperties1);

            Run run1 = new Run();
            Text text1 = new Text();
            text1.Text = innerText;

            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);
            return paragraph1;
        }
    }
}
