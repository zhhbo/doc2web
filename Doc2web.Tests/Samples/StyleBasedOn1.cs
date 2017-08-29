using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;
using Style = DocumentFormat.OpenXml.Wordprocessing.Style;

namespace Doc2web.Tests.Samples
{
    public static class StyleBasedOn1
    {
        // Adds child parts and generates content of the specified part.
        public static Styles CreateStyles() => GeneratePartContent();

        // Generates content of part.
        private static Styles GeneratePartContent()
        {
            Styles styles = new Styles() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15 w16se w16cid" } };
            styles.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            styles.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            styles.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            styles.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
            styles.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");
            styles.AddNamespaceDeclaration("w16cid", "http://schemas.microsoft.com/office/word/2016/wordml/cid");
            styles.AddNamespaceDeclaration("w16se", "http://schemas.microsoft.com/office/word/2015/wordml/symex");

            DocDefaults docDefaults1 = new DocDefaults();

            RunPropertiesDefault runPropertiesDefault1 = new RunPropertiesDefault();

            RunPropertiesBaseStyle runPropertiesBaseStyle1 = new RunPropertiesBaseStyle();
            RunFonts runFonts1 = new RunFonts() { AsciiTheme = ThemeFontValues.MinorHighAnsi, HighAnsiTheme = ThemeFontValues.MinorHighAnsi, EastAsiaTheme = ThemeFontValues.MinorHighAnsi, ComplexScriptTheme = ThemeFontValues.MinorBidi };
            FontSize fontSize1 = new FontSize() { Val = "22" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "22" };
            Languages languages1 = new Languages() { Val = "en-CA", EastAsia = "en-US", Bidi = "ar-SA" };

            runPropertiesBaseStyle1.Append(runFonts1);
            runPropertiesBaseStyle1.Append(fontSize1);
            runPropertiesBaseStyle1.Append(fontSizeComplexScript1);
            runPropertiesBaseStyle1.Append(languages1);

            runPropertiesDefault1.Append(runPropertiesBaseStyle1);

            ParagraphPropertiesDefault paragraphPropertiesDefault1 = new ParagraphPropertiesDefault();

            ParagraphPropertiesBaseStyle paragraphPropertiesBaseStyle1 = new ParagraphPropertiesBaseStyle();
            SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { After = "160", Line = "259", LineRule = LineSpacingRuleValues.Auto };

            paragraphPropertiesBaseStyle1.Append(spacingBetweenLines1);

            paragraphPropertiesDefault1.Append(paragraphPropertiesBaseStyle1);

            docDefaults1.Append(runPropertiesDefault1);
            docDefaults1.Append(paragraphPropertiesDefault1);

            Style style1 = new Style() { Type = StyleValues.Paragraph, StyleId = "Normal", Default = true };
            StyleName styleName1 = new StyleName() { Val = "Normal" };
            PrimaryStyle primaryStyle1 = new PrimaryStyle();

            style1.Append(styleName1);
            style1.Append(primaryStyle1);

            Style style2 = new Style() { Type = StyleValues.Character, StyleId = "DefaultParagraphFont", Default = true };
            StyleName styleName2 = new StyleName() { Val = "Default Paragraph Font" };
            UIPriority uIPriority1 = new UIPriority() { Val = 1 };
            SemiHidden semiHidden1 = new SemiHidden();
            UnhideWhenUsed unhideWhenUsed1 = new UnhideWhenUsed();

            style2.Append(styleName2);
            style2.Append(uIPriority1);
            style2.Append(semiHidden1);
            style2.Append(unhideWhenUsed1);

            Style style3 = new Style() { Type = StyleValues.Table, StyleId = "TableNormal", Default = true };
            StyleName styleName3 = new StyleName() { Val = "Normal Table" };
            UIPriority uIPriority2 = new UIPriority() { Val = 99 };
            SemiHidden semiHidden2 = new SemiHidden();
            UnhideWhenUsed unhideWhenUsed2 = new UnhideWhenUsed();

            StyleTableProperties styleTableProperties1 = new StyleTableProperties();
            TableIndentation tableIndentation1 = new TableIndentation() { Width = 0, Type = TableWidthUnitValues.Dxa };

            TableCellMarginDefault tableCellMarginDefault1 = new TableCellMarginDefault();
            TopMargin topMargin1 = new TopMargin() { Width = "0", Type = TableWidthUnitValues.Dxa };
            TableCellLeftMargin tableCellLeftMargin1 = new TableCellLeftMargin() { Width = 108, Type = TableWidthValues.Dxa };
            BottomMargin bottomMargin1 = new BottomMargin() { Width = "0", Type = TableWidthUnitValues.Dxa };
            TableCellRightMargin tableCellRightMargin1 = new TableCellRightMargin() { Width = 108, Type = TableWidthValues.Dxa };

            tableCellMarginDefault1.Append(topMargin1);
            tableCellMarginDefault1.Append(tableCellLeftMargin1);
            tableCellMarginDefault1.Append(bottomMargin1);
            tableCellMarginDefault1.Append(tableCellRightMargin1);

            styleTableProperties1.Append(tableIndentation1);
            styleTableProperties1.Append(tableCellMarginDefault1);

            style3.Append(styleName3);
            style3.Append(uIPriority2);
            style3.Append(semiHidden2);
            style3.Append(unhideWhenUsed2);
            style3.Append(styleTableProperties1);

            Style style4 = new Style() { Type = StyleValues.Numbering, StyleId = "NoList", Default = true };
            StyleName styleName4 = new StyleName() { Val = "No List" };
            UIPriority uIPriority3 = new UIPriority() { Val = 99 };
            SemiHidden semiHidden3 = new SemiHidden();
            UnhideWhenUsed unhideWhenUsed3 = new UnhideWhenUsed();

            style4.Append(styleName4);
            style4.Append(uIPriority3);
            style4.Append(semiHidden3);
            style4.Append(unhideWhenUsed3);

            Style style5 = new Style() { Type = StyleValues.Character, StyleId = "RunStyle1", CustomStyle = true };
            StyleName styleName5 = new StyleName() { Val = "RunStyle1" };
            BasedOn basedOn1 = new BasedOn() { Val = "DefaultParagraphFont" };
            UIPriority uIPriority4 = new UIPriority() { Val = 1 };
            PrimaryStyle primaryStyle2 = new PrimaryStyle();
            Rsid rsid1 = new Rsid() { Val = "00856DAD" };

            StyleRunProperties styleRunProperties1 = new StyleRunProperties();
            Color color1 = new Color() { Val = "4472C4", ThemeColor = ThemeColorValues.Accent1 };

            styleRunProperties1.Append(color1);

            style5.Append(styleName5);
            style5.Append(basedOn1);
            style5.Append(uIPriority4);
            style5.Append(primaryStyle2);
            style5.Append(rsid1);
            style5.Append(styleRunProperties1);

            Style style6 = new Style() { Type = StyleValues.Character, StyleId = "RunStyle2", CustomStyle = true };
            StyleName styleName6 = new StyleName() { Val = "RunStyle2" };
            BasedOn basedOn2 = new BasedOn() { Val = "RunStyle1" };
            UIPriority uIPriority5 = new UIPriority() { Val = 1 };
            PrimaryStyle primaryStyle3 = new PrimaryStyle();
            Rsid rsid2 = new Rsid() { Val = "00856DAD" };

            StyleRunProperties styleRunProperties2 = new StyleRunProperties();
            Bold bold1 = new Bold();
            Color color2 = new Color() { Val = "4472C4", ThemeColor = ThemeColorValues.Accent1 };
            Languages languages2 = new Languages() { Val = "en-US" };

            styleRunProperties2.Append(bold1);
            styleRunProperties2.Append(color2);
            styleRunProperties2.Append(languages2);

            style6.Append(styleName6);
            style6.Append(basedOn2);
            style6.Append(uIPriority5);
            style6.Append(primaryStyle3);
            style6.Append(rsid2);
            style6.Append(styleRunProperties2);

            Style style7 = new Style() { Type = StyleValues.Paragraph, StyleId = "ParagraphStyle1", CustomStyle = true };
            StyleName styleName7 = new StyleName() { Val = "ParagraphStyle1" };
            BasedOn basedOn3 = new BasedOn() { Val = "Normal" };
            PrimaryStyle primaryStyle4 = new PrimaryStyle();
            Rsid rsid3 = new Rsid() { Val = "00856DAD" };

            StyleParagraphProperties styleParagraphProperties1 = new StyleParagraphProperties();

            ParagraphBorders paragraphBorders1 = new ParagraphBorders();
            TopBorder topBorder1 = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)1U };
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)4U };
            BottomBorder bottomBorder1 = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)1U };
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)4U };

            paragraphBorders1.Append(topBorder1);
            paragraphBorders1.Append(leftBorder1);
            paragraphBorders1.Append(bottomBorder1);
            paragraphBorders1.Append(rightBorder1);

            styleParagraphProperties1.Append(paragraphBorders1);

            StyleRunProperties styleRunProperties3 = new StyleRunProperties();
            Color color3 = new Color() { Val = "4472C4", ThemeColor = ThemeColorValues.Accent1 };
            Languages languages3 = new Languages() { Val = "en-US" };

            styleRunProperties3.Append(color3);
            styleRunProperties3.Append(languages3);

            style7.Append(styleName7);
            style7.Append(basedOn3);
            style7.Append(primaryStyle4);
            style7.Append(rsid3);
            style7.Append(styleParagraphProperties1);
            style7.Append(styleRunProperties3);

            Style style8 = new Style() { Type = StyleValues.Paragraph, StyleId = "ParagraphStyle2", CustomStyle = true };
            StyleName styleName8 = new StyleName() { Val = "ParagraphStyle2" };
            BasedOn basedOn4 = new BasedOn() { Val = "ParagraphStyle1" };
            PrimaryStyle primaryStyle5 = new PrimaryStyle();
            Rsid rsid4 = new Rsid() { Val = "00856DAD" };

            StyleParagraphProperties styleParagraphProperties2 = new StyleParagraphProperties();

            ParagraphBorders paragraphBorders2 = new ParagraphBorders();
            TopBorder topBorder2 = new TopBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            LeftBorder leftBorder2 = new LeftBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder2 = new BottomBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            RightBorder rightBorder2 = new RightBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };

            paragraphBorders2.Append(topBorder2);
            paragraphBorders2.Append(leftBorder2);
            paragraphBorders2.Append(bottomBorder2);
            paragraphBorders2.Append(rightBorder2);

            styleParagraphProperties2.Append(paragraphBorders2);

            StyleRunProperties styleRunProperties4 = new StyleRunProperties();
            Bold bold2 = new Bold();
            Color color4 = new Color() { Val = "auto" };

            styleRunProperties4.Append(bold2);
            styleRunProperties4.Append(color4);

            style8.Append(styleName8);
            style8.Append(basedOn4);
            style8.Append(primaryStyle5);
            style8.Append(rsid4);
            style8.Append(styleParagraphProperties2);
            style8.Append(styleRunProperties4);

            styles.Append(docDefaults1);
            styles.Append(style1);
            styles.Append(style2);
            styles.Append(style3);
            styles.Append(style4);
            styles.Append(style5);
            styles.Append(style6);
            styles.Append(style7);
            styles.Append(style8);

            return styles;
        }

    }
}
