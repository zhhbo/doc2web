using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Tests.Samples
{
    /// <summary>
    /// Sample extracted from Appendix-B Template for a collaborative
    /// </summary>
    public static class NumberingSample2
    {

        // Creates an Numbering instance and adds its children.
        public static DocumentFormat.OpenXml.Wordprocessing.Numbering GenerateNumbering()
        {
            DocumentFormat.OpenXml.Wordprocessing.Numbering numberingPart = new DocumentFormat.OpenXml.Wordprocessing.Numbering() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 wp14" } };
            numberingPart.AddNamespaceDeclaration("wpc", "http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas");
            numberingPart.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            numberingPart.AddNamespaceDeclaration("o", "urn:schemas-microsoft-com:office:office");
            numberingPart.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            numberingPart.AddNamespaceDeclaration("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
            numberingPart.AddNamespaceDeclaration("v", "urn:schemas-microsoft-com:vml");
            numberingPart.AddNamespaceDeclaration("wp14", "http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing");
            numberingPart.AddNamespaceDeclaration("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
            numberingPart.AddNamespaceDeclaration("w10", "urn:schemas-microsoft-com:office:word");
            numberingPart.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            numberingPart.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
            numberingPart.AddNamespaceDeclaration("wpg", "http://schemas.microsoft.com/office/word/2010/wordprocessingGroup");
            numberingPart.AddNamespaceDeclaration("wpi", "http://schemas.microsoft.com/office/word/2010/wordprocessingInk");
            numberingPart.AddNamespaceDeclaration("wne", "http://schemas.microsoft.com/office/word/2006/wordml");
            numberingPart.AddNamespaceDeclaration("wps", "http://schemas.microsoft.com/office/word/2010/wordprocessingShape");

            numberingPart.Append(GenerateAbstractNum10());
            numberingPart.Append(GenerateAbstractNum18());
            numberingPart.Append(GenerateNumberingInstance7());
            numberingPart.Append(GenerateNumberingInstance8());

            return numberingPart;
        }

        // Creates an AbstractNum instance and adds its children.
        public static AbstractNum GenerateAbstractNum10()
        {
            AbstractNum abstractNum1 = new AbstractNum() { AbstractNumberId = 10 };
            Nsid nsid1 = new Nsid() { Val = "056B0B64" };
            MultiLevelType multiLevelType1 = new MultiLevelType() { Val = MultiLevelValues.Multilevel };
            TemplateCode templateCode1 = new TemplateCode() { Val = "34D070E4" };
            StyleLink styleLink1 = new StyleLink() { Val = "ListBullets" };

            Level level1 = new Level() { LevelIndex = 0 };
            StartNumberingValue startNumberingValue1 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat1 = new NumberingFormat() { Val = NumberFormatValues.Bullet };
            ParagraphStyleIdInLevel paragraphStyleIdInLevel1 = new ParagraphStyleIdInLevel() { Val = "ListBullet1" };
            LevelText levelText1 = new LevelText() { Val = "§" };
            LevelJustification levelJustification1 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties1 = new PreviousParagraphProperties();

            Tabs tabs1 = new Tabs();
            TabStop tabStop1 = new TabStop() { Val = TabStopValues.Number, Position = 1267 };

            tabs1.Append(tabStop1);
            Indentation indentation1 = new Indentation() { Left = "1267", Hanging = "360" };

            previousParagraphProperties1.Append(tabs1);
            previousParagraphProperties1.Append(indentation1);

            NumberingSymbolRunProperties numberingSymbolRunProperties1 = new NumberingSymbolRunProperties();
            RunFonts runFonts1 = new RunFonts() { Hint = FontTypeHintValues.Default, Ascii = "Wingdings", HighAnsi = "Wingdings" };

            numberingSymbolRunProperties1.Append(runFonts1);

            level1.Append(startNumberingValue1);
            level1.Append(numberingFormat1);
            level1.Append(paragraphStyleIdInLevel1);
            level1.Append(levelText1);
            level1.Append(levelJustification1);
            level1.Append(previousParagraphProperties1);
            level1.Append(numberingSymbolRunProperties1);

            Level level2 = new Level() { LevelIndex = 1 };
            StartNumberingValue startNumberingValue2 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat2 = new NumberingFormat() { Val = NumberFormatValues.Bullet };
            ParagraphStyleIdInLevel paragraphStyleIdInLevel2 = new ParagraphStyleIdInLevel() { Val = "ListBullet2" };
            LevelText levelText2 = new LevelText() { Val = "o" };
            LevelJustification levelJustification2 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties2 = new PreviousParagraphProperties();

            Tabs tabs2 = new Tabs();
            TabStop tabStop2 = new TabStop() { Val = TabStopValues.Number, Position = 1627 };

            tabs2.Append(tabStop2);
            Indentation indentation2 = new Indentation() { Left = "1627", Hanging = "360" };

            previousParagraphProperties2.Append(tabs2);
            previousParagraphProperties2.Append(indentation2);

            NumberingSymbolRunProperties numberingSymbolRunProperties2 = new NumberingSymbolRunProperties();
            RunFonts runFonts2 = new RunFonts() { Hint = FontTypeHintValues.Default, Ascii = "Courier New", HighAnsi = "Courier New" };

            numberingSymbolRunProperties2.Append(runFonts2);

            level2.Append(startNumberingValue2);
            level2.Append(numberingFormat2);
            level2.Append(paragraphStyleIdInLevel2);
            level2.Append(levelText2);
            level2.Append(levelJustification2);
            level2.Append(previousParagraphProperties2);
            level2.Append(numberingSymbolRunProperties2);

            Level level3 = new Level() { LevelIndex = 2 };
            StartNumberingValue startNumberingValue3 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat3 = new NumberingFormat() { Val = NumberFormatValues.Bullet };
            LevelText levelText3 = new LevelText() { Val = "§" };
            LevelJustification levelJustification3 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties3 = new PreviousParagraphProperties();
            Indentation indentation3 = new Indentation() { Left = "2160", Hanging = "360" };

            previousParagraphProperties3.Append(indentation3);

            NumberingSymbolRunProperties numberingSymbolRunProperties3 = new NumberingSymbolRunProperties();
            RunFonts runFonts3 = new RunFonts() { Hint = FontTypeHintValues.Default, Ascii = "Wingdings", HighAnsi = "Wingdings" };

            numberingSymbolRunProperties3.Append(runFonts3);

            level3.Append(startNumberingValue3);
            level3.Append(numberingFormat3);
            level3.Append(levelText3);
            level3.Append(levelJustification3);
            level3.Append(previousParagraphProperties3);
            level3.Append(numberingSymbolRunProperties3);

            Level level4 = new Level() { LevelIndex = 3 };
            StartNumberingValue startNumberingValue4 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat4 = new NumberingFormat() { Val = NumberFormatValues.Bullet };
            LevelText levelText4 = new LevelText() { Val = "·" };
            LevelJustification levelJustification4 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties4 = new PreviousParagraphProperties();
            Indentation indentation4 = new Indentation() { Left = "2880", Hanging = "360" };

            previousParagraphProperties4.Append(indentation4);

            NumberingSymbolRunProperties numberingSymbolRunProperties4 = new NumberingSymbolRunProperties();
            RunFonts runFonts4 = new RunFonts() { Hint = FontTypeHintValues.Default, Ascii = "Symbol", HighAnsi = "Symbol" };

            numberingSymbolRunProperties4.Append(runFonts4);

            level4.Append(startNumberingValue4);
            level4.Append(numberingFormat4);
            level4.Append(levelText4);
            level4.Append(levelJustification4);
            level4.Append(previousParagraphProperties4);
            level4.Append(numberingSymbolRunProperties4);

            Level level5 = new Level() { LevelIndex = 4 };
            StartNumberingValue startNumberingValue5 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat5 = new NumberingFormat() { Val = NumberFormatValues.Bullet };
            LevelText levelText5 = new LevelText() { Val = "o" };
            LevelJustification levelJustification5 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties5 = new PreviousParagraphProperties();
            Indentation indentation5 = new Indentation() { Left = "3600", Hanging = "360" };

            previousParagraphProperties5.Append(indentation5);

            NumberingSymbolRunProperties numberingSymbolRunProperties5 = new NumberingSymbolRunProperties();
            RunFonts runFonts5 = new RunFonts() { Hint = FontTypeHintValues.Default, Ascii = "Courier New", HighAnsi = "Courier New" };

            numberingSymbolRunProperties5.Append(runFonts5);

            level5.Append(startNumberingValue5);
            level5.Append(numberingFormat5);
            level5.Append(levelText5);
            level5.Append(levelJustification5);
            level5.Append(previousParagraphProperties5);
            level5.Append(numberingSymbolRunProperties5);

            Level level6 = new Level() { LevelIndex = 5 };
            StartNumberingValue startNumberingValue6 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat6 = new NumberingFormat() { Val = NumberFormatValues.Bullet };
            LevelText levelText6 = new LevelText() { Val = "§" };
            LevelJustification levelJustification6 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties6 = new PreviousParagraphProperties();
            Indentation indentation6 = new Indentation() { Left = "4320", Hanging = "360" };

            previousParagraphProperties6.Append(indentation6);

            NumberingSymbolRunProperties numberingSymbolRunProperties6 = new NumberingSymbolRunProperties();
            RunFonts runFonts6 = new RunFonts() { Hint = FontTypeHintValues.Default, Ascii = "Wingdings", HighAnsi = "Wingdings" };

            numberingSymbolRunProperties6.Append(runFonts6);

            level6.Append(startNumberingValue6);
            level6.Append(numberingFormat6);
            level6.Append(levelText6);
            level6.Append(levelJustification6);
            level6.Append(previousParagraphProperties6);
            level6.Append(numberingSymbolRunProperties6);

            Level level7 = new Level() { LevelIndex = 6 };
            StartNumberingValue startNumberingValue7 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat7 = new NumberingFormat() { Val = NumberFormatValues.Bullet };
            LevelText levelText7 = new LevelText() { Val = "·" };
            LevelJustification levelJustification7 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties7 = new PreviousParagraphProperties();
            Indentation indentation7 = new Indentation() { Left = "5040", Hanging = "360" };

            previousParagraphProperties7.Append(indentation7);

            NumberingSymbolRunProperties numberingSymbolRunProperties7 = new NumberingSymbolRunProperties();
            RunFonts runFonts7 = new RunFonts() { Hint = FontTypeHintValues.Default, Ascii = "Symbol", HighAnsi = "Symbol" };

            numberingSymbolRunProperties7.Append(runFonts7);

            level7.Append(startNumberingValue7);
            level7.Append(numberingFormat7);
            level7.Append(levelText7);
            level7.Append(levelJustification7);
            level7.Append(previousParagraphProperties7);
            level7.Append(numberingSymbolRunProperties7);

            Level level8 = new Level() { LevelIndex = 7 };
            StartNumberingValue startNumberingValue8 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat8 = new NumberingFormat() { Val = NumberFormatValues.Bullet };
            LevelText levelText8 = new LevelText() { Val = "o" };
            LevelJustification levelJustification8 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties8 = new PreviousParagraphProperties();
            Indentation indentation8 = new Indentation() { Left = "5760", Hanging = "360" };

            previousParagraphProperties8.Append(indentation8);

            NumberingSymbolRunProperties numberingSymbolRunProperties8 = new NumberingSymbolRunProperties();
            RunFonts runFonts8 = new RunFonts() { Hint = FontTypeHintValues.Default, Ascii = "Courier New", HighAnsi = "Courier New" };

            numberingSymbolRunProperties8.Append(runFonts8);

            level8.Append(startNumberingValue8);
            level8.Append(numberingFormat8);
            level8.Append(levelText8);
            level8.Append(levelJustification8);
            level8.Append(previousParagraphProperties8);
            level8.Append(numberingSymbolRunProperties8);

            Level level9 = new Level() { LevelIndex = 8 };
            StartNumberingValue startNumberingValue9 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat9 = new NumberingFormat() { Val = NumberFormatValues.Bullet };
            LevelText levelText9 = new LevelText() { Val = "§" };
            LevelJustification levelJustification9 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties9 = new PreviousParagraphProperties();
            Indentation indentation9 = new Indentation() { Left = "6480", Hanging = "360" };

            previousParagraphProperties9.Append(indentation9);

            NumberingSymbolRunProperties numberingSymbolRunProperties9 = new NumberingSymbolRunProperties();
            RunFonts runFonts9 = new RunFonts() { Hint = FontTypeHintValues.Default, Ascii = "Wingdings", HighAnsi = "Wingdings" };

            numberingSymbolRunProperties9.Append(runFonts9);

            level9.Append(startNumberingValue9);
            level9.Append(numberingFormat9);
            level9.Append(levelText9);
            level9.Append(levelJustification9);
            level9.Append(previousParagraphProperties9);
            level9.Append(numberingSymbolRunProperties9);

            abstractNum1.Append(nsid1);
            abstractNum1.Append(multiLevelType1);
            abstractNum1.Append(templateCode1);
            abstractNum1.Append(styleLink1);
            abstractNum1.Append(level1);
            abstractNum1.Append(level2);
            abstractNum1.Append(level3);
            abstractNum1.Append(level4);
            abstractNum1.Append(level5);
            abstractNum1.Append(level6);
            abstractNum1.Append(level7);
            abstractNum1.Append(level8);
            abstractNum1.Append(level9);
            return abstractNum1;
        }

        // Creates an AbstractNum instance and adds its children.
        public static AbstractNum GenerateAbstractNum18()
        // Creates an AbstractNum instance and adds its children.
        {
            AbstractNum abstractNum1 = new AbstractNum() { AbstractNumberId = 18 };
            Nsid nsid1 = new Nsid() { Val = "78DA27B2" };
            MultiLevelType multiLevelType1 = new MultiLevelType() { Val = MultiLevelValues.Multilevel };
            TemplateCode templateCode1 = new TemplateCode() { Val = "34D070E4" };
            NumberingStyleLink numberingStyleLink1 = new NumberingStyleLink() { Val = "ListBullets" };

            abstractNum1.Append(nsid1);
            abstractNum1.Append(multiLevelType1);
            abstractNum1.Append(templateCode1);
            abstractNum1.Append(numberingStyleLink1);
            return abstractNum1;
        }

        // Creates an NumberingInstance instance and adds its children.
        public static NumberingInstance GenerateNumberingInstance8()
        {
            NumberingInstance numberingInstance1 = new NumberingInstance() { NumberID = 8 };
            AbstractNumId abstractNumId1 = new AbstractNumId() { Val = 18 };

            LevelOverride levelOverride1 = new LevelOverride() { LevelIndex = 0 };
            StartOverrideNumberingValue startOverrideNumberingValue1 = new StartOverrideNumberingValue() { Val = 1 };

            levelOverride1.Append(startOverrideNumberingValue1);

            LevelOverride levelOverride2 = new LevelOverride() { LevelIndex = 1 };
            StartOverrideNumberingValue startOverrideNumberingValue2 = new StartOverrideNumberingValue() { Val = 1 };

            levelOverride2.Append(startOverrideNumberingValue2);

            LevelOverride levelOverride3 = new LevelOverride() { LevelIndex = 2 };
            StartOverrideNumberingValue startOverrideNumberingValue3 = new StartOverrideNumberingValue() { Val = 1 };

            levelOverride3.Append(startOverrideNumberingValue3);

            LevelOverride levelOverride4 = new LevelOverride() { LevelIndex = 3 };
            StartOverrideNumberingValue startOverrideNumberingValue4 = new StartOverrideNumberingValue() { Val = 1 };

            levelOverride4.Append(startOverrideNumberingValue4);

            LevelOverride levelOverride5 = new LevelOverride() { LevelIndex = 4 };
            StartOverrideNumberingValue startOverrideNumberingValue5 = new StartOverrideNumberingValue() { Val = 1 };

            levelOverride5.Append(startOverrideNumberingValue5);

            LevelOverride levelOverride6 = new LevelOverride() { LevelIndex = 5 };
            StartOverrideNumberingValue startOverrideNumberingValue6 = new StartOverrideNumberingValue() { Val = 1 };

            levelOverride6.Append(startOverrideNumberingValue6);

            LevelOverride levelOverride7 = new LevelOverride() { LevelIndex = 6 };
            StartOverrideNumberingValue startOverrideNumberingValue7 = new StartOverrideNumberingValue() { Val = 1 };

            levelOverride7.Append(startOverrideNumberingValue7);

            LevelOverride levelOverride8 = new LevelOverride() { LevelIndex = 7 };
            StartOverrideNumberingValue startOverrideNumberingValue8 = new StartOverrideNumberingValue() { Val = 1 };

            levelOverride8.Append(startOverrideNumberingValue8);

            LevelOverride levelOverride9 = new LevelOverride() { LevelIndex = 8 };
            StartOverrideNumberingValue startOverrideNumberingValue9 = new StartOverrideNumberingValue() { Val = 1 };

            levelOverride9.Append(startOverrideNumberingValue9);

            numberingInstance1.Append(abstractNumId1);
            numberingInstance1.Append(levelOverride1);
            numberingInstance1.Append(levelOverride2);
            numberingInstance1.Append(levelOverride3);
            numberingInstance1.Append(levelOverride4);
            numberingInstance1.Append(levelOverride5);
            numberingInstance1.Append(levelOverride6);
            numberingInstance1.Append(levelOverride7);
            numberingInstance1.Append(levelOverride8);
            numberingInstance1.Append(levelOverride9);
            return numberingInstance1;
        }

        // Creates an NumberingInstance instance and adds its children.
        public static NumberingInstance GenerateNumberingInstance7()
        {
            NumberingInstance numberingInstance1 = new NumberingInstance() { NumberID = 7 };
            AbstractNumId abstractNumId1 = new AbstractNumId() { Val = 10 };

            numberingInstance1.Append(abstractNumId1);
            return numberingInstance1;
        }

        // Creates an Style instance and adds its children.
        public static DocumentFormat.OpenXml.Wordprocessing.Style GenerateListBulletsStyle()
        {
            DocumentFormat.OpenXml.Wordprocessing.Style style1 = new DocumentFormat.OpenXml.Wordprocessing.Style() { Type = StyleValues.Numbering, StyleId = "ListBullets", CustomStyle = true };
            StyleName styleName1 = new StyleName() { Val = "ListBullets" };
            Rsid rsid1 = new Rsid() { Val = "0042570B" };

            StyleParagraphProperties styleParagraphProperties1 = new StyleParagraphProperties();

            NumberingProperties numberingProperties1 = new NumberingProperties();
            NumberingId numberingId1 = new NumberingId() { Val = 7 };

            numberingProperties1.Append(numberingId1);

            styleParagraphProperties1.Append(numberingProperties1);

            style1.Append(styleName1);
            style1.Append(rsid1);
            style1.Append(styleParagraphProperties1);
            return style1;
        }

        // Creates an Styles instance and adds its children.
        public static Styles GenerateStyles()
        {
            Styles styles1 = new Styles() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14" } };
            styles1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            styles1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            styles1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            styles1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");

            styles1.Append(GenerateListBulletsStyle());

            return styles1;
        }

    }
}
