using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Samples
{
    public static class TOCSample1
    {
        public static Paragraph GenerateParagraph()
        {
            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00D36A28", RsidParagraphAddition = "00D36A28", RsidRunAdditionDefault = "005478FD", ParagraphId = "16FCFC06", TextId = "77777777" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId() { Val = "TOC1" };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial", EastAsiaTheme = ThemeFontValues.MinorEastAsia };
            Caps caps1 = new Caps() { Val = false };
            NoProof noProof1 = new NoProof();
            FontSize fontSize1 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "20" };
            Languages languages1 = new Languages() { EastAsia = "en-CA" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(caps1);
            paragraphMarkRunProperties1.Append(noProof1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);
            paragraphMarkRunProperties1.Append(languages1);

            paragraphProperties1.Append(paragraphStyleId1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run() { RsidRunProperties = "004D1F3B" };

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize2 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "20" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            FieldChar fieldChar1 = new FieldChar() { FieldCharType = FieldCharValues.Begin };

            run1.Append(runProperties1);
            run1.Append(fieldChar1);
            BookmarkEnd bookmarkEnd1 = new BookmarkEnd() { Id = "3" };
            BookmarkEnd bookmarkEnd2 = new BookmarkEnd() { Id = "4" };
            BookmarkEnd bookmarkEnd3 = new BookmarkEnd() { Id = "5" };

            Run run2 = new Run() { RsidRunProperties = "004D1F3B" };

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts3 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize3 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "20" };

            runProperties2.Append(runFonts3);
            runProperties2.Append(fontSize3);
            runProperties2.Append(fontSizeComplexScript3);
            FieldCode fieldCode1 = new FieldCode() { Space = SpaceProcessingModeValues.Preserve };
            fieldCode1.Text = " TOC \\o \"1-2\" \\h \\z \\u ";

            run2.Append(runProperties2);
            run2.Append(fieldCode1);

            Run run3 = new Run() { RsidRunProperties = "004D1F3B" };

            RunProperties runProperties3 = new RunProperties();
            RunFonts runFonts4 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize4 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "20" };

            runProperties3.Append(runFonts4);
            runProperties3.Append(fontSize4);
            runProperties3.Append(fontSizeComplexScript4);
            FieldChar fieldChar2 = new FieldChar() { FieldCharType = FieldCharValues.Separate };

            run3.Append(runProperties3);
            run3.Append(fieldChar2);

            Hyperlink hyperlink1 = new Hyperlink() { History = true, Anchor = "_Toc459701534" };

            Run run4 = new Run() { RsidRunProperties = "00D36A28", RsidRunAddition = "00D36A28" };

            RunProperties runProperties4 = new RunProperties();
            RunStyle runStyle1 = new RunStyle() { Val = "Hyperlink" };
            RunFonts runFonts5 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            NoProof noProof2 = new NoProof();
            FontSize fontSize5 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript() { Val = "20" };

            runProperties4.Append(runStyle1);
            runProperties4.Append(runFonts5);
            runProperties4.Append(noProof2);
            runProperties4.Append(fontSize5);
            runProperties4.Append(fontSizeComplexScript5);
            Text text1 = new Text();
            text1.Text = "Article 1 definitions and iNTERPRETATION";

            run4.Append(runProperties4);
            run4.Append(text1);

            Run run5 = new Run() { RsidRunProperties = "00D36A28", RsidRunAddition = "00D36A28" };

            RunProperties runProperties5 = new RunProperties();
            RunFonts runFonts6 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            NoProof noProof3 = new NoProof();
            WebHidden webHidden1 = new WebHidden();
            FontSize fontSize6 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "20" };

            runProperties5.Append(runFonts6);
            runProperties5.Append(noProof3);
            runProperties5.Append(webHidden1);
            runProperties5.Append(fontSize6);
            runProperties5.Append(fontSizeComplexScript6);
            TabChar tabChar1 = new TabChar();

            run5.Append(runProperties5);
            run5.Append(tabChar1);

            Run run6 = new Run() { RsidRunProperties = "00D36A28", RsidRunAddition = "00D36A28" };

            RunProperties runProperties6 = new RunProperties();
            RunFonts runFonts7 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            NoProof noProof4 = new NoProof();
            WebHidden webHidden2 = new WebHidden();
            FontSize fontSize7 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "20" };

            runProperties6.Append(runFonts7);
            runProperties6.Append(noProof4);
            runProperties6.Append(webHidden2);
            runProperties6.Append(fontSize7);
            runProperties6.Append(fontSizeComplexScript7);
            FieldChar fieldChar3 = new FieldChar() { FieldCharType = FieldCharValues.Begin };

            run6.Append(runProperties6);
            run6.Append(fieldChar3);

            Run run7 = new Run() { RsidRunProperties = "00D36A28", RsidRunAddition = "00D36A28" };

            RunProperties runProperties7 = new RunProperties();
            RunFonts runFonts8 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            NoProof noProof5 = new NoProof();
            WebHidden webHidden3 = new WebHidden();
            FontSize fontSize8 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript8 = new FontSizeComplexScript() { Val = "20" };

            runProperties7.Append(runFonts8);
            runProperties7.Append(noProof5);
            runProperties7.Append(webHidden3);
            runProperties7.Append(fontSize8);
            runProperties7.Append(fontSizeComplexScript8);
            FieldCode fieldCode2 = new FieldCode() { Space = SpaceProcessingModeValues.Preserve };
            fieldCode2.Text = " PAGEREF _Toc459701534 \\h ";

            run7.Append(runProperties7);
            run7.Append(fieldCode2);

            Run run8 = new Run() { RsidRunProperties = "00D36A28", RsidRunAddition = "00D36A28" };

            RunProperties runProperties8 = new RunProperties();
            RunFonts runFonts9 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            NoProof noProof6 = new NoProof();
            WebHidden webHidden4 = new WebHidden();
            FontSize fontSize9 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript9 = new FontSizeComplexScript() { Val = "20" };

            runProperties8.Append(runFonts9);
            runProperties8.Append(noProof6);
            runProperties8.Append(webHidden4);
            runProperties8.Append(fontSize9);
            runProperties8.Append(fontSizeComplexScript9);

            run8.Append(runProperties8);

            Run run9 = new Run() { RsidRunProperties = "00D36A28", RsidRunAddition = "00D36A28" };

            RunProperties runProperties9 = new RunProperties();
            RunFonts runFonts10 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            NoProof noProof7 = new NoProof();
            WebHidden webHidden5 = new WebHidden();
            FontSize fontSize10 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript10 = new FontSizeComplexScript() { Val = "20" };

            runProperties9.Append(runFonts10);
            runProperties9.Append(noProof7);
            runProperties9.Append(webHidden5);
            runProperties9.Append(fontSize10);
            runProperties9.Append(fontSizeComplexScript10);
            FieldChar fieldChar4 = new FieldChar() { FieldCharType = FieldCharValues.Separate };

            run9.Append(runProperties9);
            run9.Append(fieldChar4);

            Run run10 = new Run() { RsidRunProperties = "00D36A28", RsidRunAddition = "00D36A28" };

            RunProperties runProperties10 = new RunProperties();
            RunFonts runFonts11 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            NoProof noProof8 = new NoProof();
            WebHidden webHidden6 = new WebHidden();
            FontSize fontSize11 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript11 = new FontSizeComplexScript() { Val = "20" };

            runProperties10.Append(runFonts11);
            runProperties10.Append(noProof8);
            runProperties10.Append(webHidden6);
            runProperties10.Append(fontSize11);
            runProperties10.Append(fontSizeComplexScript11);
            Text text2 = new Text();
            text2.Text = "2";

            run10.Append(runProperties10);
            run10.Append(text2);

            Run run11 = new Run() { RsidRunProperties = "00D36A28", RsidRunAddition = "00D36A28" };

            RunProperties runProperties11 = new RunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            NoProof noProof9 = new NoProof();
            WebHidden webHidden7 = new WebHidden();
            FontSize fontSize12 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript12 = new FontSizeComplexScript() { Val = "20" };

            runProperties11.Append(runFonts12);
            runProperties11.Append(noProof9);
            runProperties11.Append(webHidden7);
            runProperties11.Append(fontSize12);
            runProperties11.Append(fontSizeComplexScript12);
            FieldChar fieldChar5 = new FieldChar() { FieldCharType = FieldCharValues.End };

            run11.Append(runProperties11);
            run11.Append(fieldChar5);

            hyperlink1.Append(run4);
            hyperlink1.Append(run5);
            hyperlink1.Append(run6);
            hyperlink1.Append(run7);
            hyperlink1.Append(run8);
            hyperlink1.Append(run9);
            hyperlink1.Append(run10);
            hyperlink1.Append(run11);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);
            paragraph1.Append(bookmarkEnd1);
            paragraph1.Append(bookmarkEnd2);
            paragraph1.Append(bookmarkEnd3);
            paragraph1.Append(run2);
            paragraph1.Append(run3);
            paragraph1.Append(hyperlink1);
            return paragraph1;
        }

        public static (IElementContext, Hyperlink) GenerateProcessingArgs()
        {
            var context = Substitute.For<IElementContext>();
            var p = GenerateParagraph();
            context.RootElement.Returns(p);
            var hyperlink = p.LastChild as Hyperlink;
            context.Element.Returns(hyperlink);
            context.TextIndex
                .Returns(
                    p.Elements().Reverse().Skip(1)
                    .Select(x => x.InnerText.Length).Sum());
            return (context, hyperlink);
        }

    }
}
