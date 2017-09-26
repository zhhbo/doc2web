using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Tests.Samples
{
    public static class NumberingCircularSample
    {
        /// Creates an NumberingInstance instance and adds its children.
        public static NumberingInstance GenerateNumberingInstance()
        {
            NumberingInstance numberingInstance1 = new NumberingInstance() { NumberID = 1 };
            AbstractNumId abstractNumId1 = new AbstractNumId() { Val = 1 };

            numberingInstance1.Append(abstractNumId1);
            return numberingInstance1;
        }

        // Creates an AbstractNum instance and adds its children.
        public static AbstractNum GenerateAbstractNum()
        {
            AbstractNum abstractNum1 = new AbstractNum() { AbstractNumberId = 1 };
            abstractNum1.SetAttribute(new OpenXmlAttribute("w15", "restartNumberingAfterBreak", "http://schemas.microsoft.com/office/word/2012/wordml", "0"));
            Nsid nsid1 = new Nsid() { Val = "00000001" };
            MultiLevelType multiLevelType1 = new MultiLevelType() { Val = MultiLevelValues.Multilevel };
            TemplateCode templateCode1 = new TemplateCode() { Val = "226AA648" };
            NumberingStyleLink numberingStyleLink1 = new NumberingStyleLink() { Val = "ImportedStyle1" };

            abstractNum1.Append(nsid1);
            abstractNum1.Append(multiLevelType1);
            abstractNum1.Append(templateCode1);
            abstractNum1.Append(numberingStyleLink1);
            return abstractNum1;
        }

        public static DocumentFormat.OpenXml.Wordprocessing.Numbering GenerateNumbering()
        {
            DocumentFormat.OpenXml.Wordprocessing.Numbering num = new DocumentFormat.OpenXml.Wordprocessing.Numbering();
            num.Append(GenerateAbstractNum());
            num.Append(GenerateNumberingInstance());
            return num;
        }

        // Creates an Style instance and adds its children.
        public static DocumentFormat.OpenXml.Wordprocessing.Style GenerateStyle()
        {
            DocumentFormat.OpenXml.Wordprocessing.Style style1 = new DocumentFormat.OpenXml.Wordprocessing.Style() { Type = StyleValues.Numbering, StyleId = "ImportedStyle1", CustomStyle = true };
            StyleName styleName1 = new StyleName() { Val = "Imported Style 1" };
            AutoRedefine autoRedefine1 = new AutoRedefine();

            StyleParagraphProperties styleParagraphProperties1 = new StyleParagraphProperties();

            NumberingProperties numberingProperties1 = new NumberingProperties();
            NumberingId numberingId1 = new NumberingId() { Val = 1 };

            numberingProperties1.Append(numberingId1);

            styleParagraphProperties1.Append(numberingProperties1);

            style1.Append(styleName1);
            style1.Append(autoRedefine1);
            style1.Append(styleParagraphProperties1);
            return style1;
        }

        public static Styles GenerateStyles()
        {
            var styles = new Styles();
            styles.AppendChild(GenerateStyle());
            return styles;
        }


    }
}
