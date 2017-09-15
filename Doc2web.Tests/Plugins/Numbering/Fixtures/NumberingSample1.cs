using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Tests.Plugins.Numbering.Fixtures
{
    static class NumberingSample1
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

            numberingPart.Append(NumberingSample1.GenerateAbstractNum());
            numberingPart.Append(NumberingSample1.GenerateNumberingInstance());

            return numberingPart;
        }

        static public AbstractNum GenerateAbstractNum()
        {
            AbstractNum abstractNum1 = new AbstractNum() { AbstractNumberId = 1 };
            Nsid nsid1 = new Nsid() { Val = "00000001" };
            MultiLevelType multiLevelType1 = new MultiLevelType() { Val = MultiLevelValues.Multilevel };
            TemplateCode templateCode1 = new TemplateCode() { Val = "00000000" };

            Level level1 = new Level() { LevelIndex = 0 };
            StartNumberingValue startNumberingValue1 = new StartNumberingValue() { Val = 0 };
            NumberingFormat numberingFormat1 = new NumberingFormat() { Val = NumberFormatValues.Bullet };
            ParagraphStyleIdInLevel paragraphStyleIdInLevel1 = new ParagraphStyleIdInLevel() { Val = "Legal1" };
            LevelText levelText1 = new LevelText() { Val = "%1-" };
            LevelJustification levelJustification1 = new LevelJustification() { Val = LevelJustificationValues.Right };

            PreviousParagraphProperties previousParagraphProperties1 = new PreviousParagraphProperties();

            Tabs tabs1 = new Tabs();
            TabStop tabStop1 = new TabStop() { Val = TabStopValues.Number, Position = 720 };

            tabs1.Append(tabStop1);

            previousParagraphProperties1.Append(tabs1);

            NumberingSymbolRunProperties numberingSymbolRunProperties1 = new NumberingSymbolRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Baskerville Old Face", HighAnsi = "Baskerville Old Face", ComplexScript = "Times New Roman" };
            FontSize fontSize1 = new FontSize() { Val = "20" };

            numberingSymbolRunProperties1.Append(runFonts1);
            numberingSymbolRunProperties1.Append(fontSize1);

            level1.Append(startNumberingValue1);
            level1.Append(numberingFormat1);
            level1.Append(paragraphStyleIdInLevel1);
            level1.Append(levelText1);
            level1.Append(levelJustification1);
            level1.Append(previousParagraphProperties1);
            level1.Append(numberingSymbolRunProperties1);

            Level level2 = new Level() { LevelIndex = 1 };
            StartNumberingValue startNumberingValue2 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat2 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            ParagraphStyleIdInLevel paragraphStyleIdInLevel2 = new ParagraphStyleIdInLevel() { Val = "Legal2" };
            LevelText levelText2 = new LevelText() { Val = "%1.%2." };
            LevelJustification levelJustification2 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties2 = new PreviousParagraphProperties();

            Tabs tabs2 = new Tabs();
            TabStop tabStop2 = new TabStop() { Val = TabStopValues.Number, Position = 1440 };

            tabs2.Append(tabStop2);
            Indentation indentation1 = new Indentation() { FirstLine = "720" };

            previousParagraphProperties2.Append(tabs2);
            previousParagraphProperties2.Append(indentation1);

            NumberingSymbolRunProperties numberingSymbolRunProperties2 = new NumberingSymbolRunProperties();
            RunFonts runFonts2 = new RunFonts() { ComplexScript = "Times New Roman" };

            numberingSymbolRunProperties2.Append(runFonts2);

            level2.Append(startNumberingValue2);
            level2.Append(numberingFormat2);
            level2.Append(paragraphStyleIdInLevel2);
            level2.Append(levelText2);
            level2.Append(levelJustification2);
            level2.Append(previousParagraphProperties2);
            level2.Append(numberingSymbolRunProperties2);

            Level level4 = new Level() { LevelIndex = 3 };
            StartNumberingValue startNumberingValue4 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat4 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            ParagraphStyleIdInLevel paragraphStyleIdInLevel4 = new ParagraphStyleIdInLevel() { Val = "Legal4" };
            LevelText levelText4 = new LevelText() { Val = "(%4)" };
            LevelJustification levelJustification4 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties4 = new PreviousParagraphProperties();

            Tabs tabs4 = new Tabs();
            TabStop tabStop4 = new TabStop() { Val = TabStopValues.Number, Position = 2880 };

            tabs4.Append(tabStop4);
            Indentation indentation3 = new Indentation() { Left = "1440", FirstLine = "720" };

            previousParagraphProperties4.Append(tabs4);
            previousParagraphProperties4.Append(indentation3);

            NumberingSymbolRunProperties numberingSymbolRunProperties4 = new NumberingSymbolRunProperties();
            RunFonts runFonts4 = new RunFonts() { ComplexScript = "Times New Roman" };

            numberingSymbolRunProperties4.Append(runFonts4);

            level4.Append(startNumberingValue4);
            level4.Append(numberingFormat4);
            level4.Append(paragraphStyleIdInLevel4);
            level4.Append(levelText4);
            level4.Append(levelJustification4);
            level4.Append(previousParagraphProperties4);
            level4.Append(numberingSymbolRunProperties4);

            Level level5 = new Level() { LevelIndex = 4 };
            StartNumberingValue startNumberingValue5 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat5 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            LevelText levelText5 = new LevelText() { Val = "%5" };
            LevelJustification levelJustification5 = new LevelJustification() { Val = LevelJustificationValues.Left };

            NumberingSymbolRunProperties numberingSymbolRunProperties5 = new NumberingSymbolRunProperties();
            RunFonts runFonts5 = new RunFonts() { ComplexScript = "Times New Roman" };

            numberingSymbolRunProperties5.Append(runFonts5);

            level5.Append(startNumberingValue5);
            level5.Append(numberingFormat5);
            level5.Append(levelText5);
            level5.Append(levelJustification5);
            level5.Append(numberingSymbolRunProperties5);

            Level level6 = new Level() { LevelIndex = 5 };
            StartNumberingValue startNumberingValue6 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat6 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            LevelText levelText6 = new LevelText() { Val = "%6" };
            LevelJustification levelJustification6 = new LevelJustification() { Val = LevelJustificationValues.Left };

            NumberingSymbolRunProperties numberingSymbolRunProperties6 = new NumberingSymbolRunProperties();
            RunFonts runFonts6 = new RunFonts() { ComplexScript = "Times New Roman" };

            numberingSymbolRunProperties6.Append(runFonts6);

            level6.Append(startNumberingValue6);
            level6.Append(numberingFormat6);
            level6.Append(levelText6);
            level6.Append(levelJustification6);
            level6.Append(numberingSymbolRunProperties6);

            Level level7 = new Level() { LevelIndex = 6 };
            StartNumberingValue startNumberingValue7 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat7 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            LevelText levelText7 = new LevelText() { Val = "%7" };
            LevelJustification levelJustification7 = new LevelJustification() { Val = LevelJustificationValues.Left };

            NumberingSymbolRunProperties numberingSymbolRunProperties7 = new NumberingSymbolRunProperties();
            RunFonts runFonts7 = new RunFonts() { ComplexScript = "Times New Roman" };

            numberingSymbolRunProperties7.Append(runFonts7);

            level7.Append(startNumberingValue7);
            level7.Append(numberingFormat7);
            level7.Append(levelText7);
            level7.Append(levelJustification7);
            level7.Append(numberingSymbolRunProperties7);

            Level level8 = new Level() { LevelIndex = 7 };
            StartNumberingValue startNumberingValue8 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat8 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            LevelText levelText8 = new LevelText() { Val = "%8" };
            LevelJustification levelJustification8 = new LevelJustification() { Val = LevelJustificationValues.Left };

            NumberingSymbolRunProperties numberingSymbolRunProperties8 = new NumberingSymbolRunProperties();
            RunFonts runFonts8 = new RunFonts() { ComplexScript = "Times New Roman" };

            numberingSymbolRunProperties8.Append(runFonts8);

            level8.Append(startNumberingValue8);
            level8.Append(numberingFormat8);
            level8.Append(levelText8);
            level8.Append(levelJustification8);
            level8.Append(numberingSymbolRunProperties8);

            Level level9 = new Level() { LevelIndex = 8 };
            var isLegalNumberingStyle = new IsLegalNumberingStyle();
            NumberingFormat numberingFormat9 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            LevelText levelText9 = new LevelText() { Val = "" };
            LevelJustification levelJustification9 = new LevelJustification() { Val = LevelJustificationValues.Left };

            NumberingSymbolRunProperties numberingSymbolRunProperties9 = new NumberingSymbolRunProperties();
            RunFonts runFonts9 = new RunFonts() { ComplexScript = "Times New Roman" };

            numberingSymbolRunProperties9.Append(runFonts9);

            level9.Append(numberingFormat9);
            level9.Append(levelText9);
            level9.Append(levelJustification9);
            level9.Append(numberingSymbolRunProperties9);
            //level9.Append(isLegalNumberingStyle);
            level9.IsLegalNumberingStyle = isLegalNumberingStyle;

            abstractNum1.Append(nsid1);
            abstractNum1.Append(multiLevelType1);
            abstractNum1.Append(templateCode1);
            abstractNum1.Append(level1);
            abstractNum1.Append(level2);
            //abstractNum1.Append(level3);
            abstractNum1.Append(level4);
            abstractNum1.Append(level5);
            abstractNum1.Append(level6);
            abstractNum1.Append(level7);
            abstractNum1.Append(level8);
            abstractNum1.Append(level9);
            return abstractNum1;
        }


        // Creates an NumberingInstance instance and adds its children.
        public static NumberingInstance GenerateNumberingInstance()
        {
            NumberingInstance numberingInstance1 = new NumberingInstance() { NumberID = 7 };
            AbstractNumId abstractNumId1 = new AbstractNumId() { Val = 1 };

            LevelOverride levelOverride1 = new LevelOverride() { LevelIndex = 0 };
            StartOverrideNumberingValue startOverrideNumberingValue1 = new StartOverrideNumberingValue() { Val = 1 };

            Level level1 = new Level() { LevelIndex = 0 };
            StartNumberingValue startNumberingValue1 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat1 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            ParagraphStyleIdInLevel paragraphStyleIdInLevel1 = new ParagraphStyleIdInLevel() { Val = "Legal1" };
            LevelText levelText1 = new LevelText() { Val = "%1." };
            LevelJustification levelJustification1 = new LevelJustification() { Val = LevelJustificationValues.Left };

            NumberingSymbolRunProperties numberingSymbolRunProperties1 = new NumberingSymbolRunProperties();
            RunFonts runFonts1 = new RunFonts() { ComplexScript = "Times New Roman" };

            numberingSymbolRunProperties1.Append(runFonts1);

            level1.Append(startNumberingValue1);
            level1.Append(numberingFormat1);
            level1.Append(paragraphStyleIdInLevel1);
            level1.Append(levelText1);
            level1.Append(levelJustification1);
            level1.Append(numberingSymbolRunProperties1);

            levelOverride1.Append(startOverrideNumberingValue1);
            levelOverride1.Append(level1);

            LevelOverride levelOverride2 = new LevelOverride() { LevelIndex = 1 };
            StartOverrideNumberingValue startOverrideNumberingValue2 = new StartOverrideNumberingValue() { Val = 1 };

            LevelOverride levelOverride3 = new LevelOverride() { LevelIndex = 2 };
            StartOverrideNumberingValue startOverrideNumberingValue3 = new StartOverrideNumberingValue() { Val = 1 };

            Level level3 = new Level() { LevelIndex = 2 };
            StartNumberingValue startNumberingValue3 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat3 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            ParagraphStyleIdInLevel paragraphStyleIdInLevel3 = new ParagraphStyleIdInLevel() { Val = "Legal3" };
            LevelText levelText3 = new LevelText() { Val = "(%3)" };
            LevelJustification levelJustification3 = new LevelJustification() { Val = LevelJustificationValues.Left };

            NumberingSymbolRunProperties numberingSymbolRunProperties3 = new NumberingSymbolRunProperties();
            RunFonts runFonts3 = new RunFonts() { ComplexScript = "Times New Roman" };

            numberingSymbolRunProperties3.Append(runFonts3);

            level3.Append(startNumberingValue3);
            level3.Append(numberingFormat3);
            level3.Append(paragraphStyleIdInLevel3);
            level3.Append(levelText3);
            level3.Append(levelJustification3);
            level3.Append(numberingSymbolRunProperties3);

            levelOverride3.Append(startOverrideNumberingValue3);
            levelOverride3.Append(level3);

            LevelOverride levelOverride4 = new LevelOverride() { LevelIndex = 3 };
            StartOverrideNumberingValue startOverrideNumberingValue4 = new StartOverrideNumberingValue() { Val = 1 };

            Level level4 = new Level() { LevelIndex = 3 };
            StartNumberingValue startNumberingValue4 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat4 = new NumberingFormat() { Val = NumberFormatValues.DecimalEnclosedCircle };
            ParagraphStyleIdInLevel paragraphStyleIdInLevel4 = new ParagraphStyleIdInLevel() { Val = "Legal4" };
            LevelText levelText4 = new LevelText() { Val = "(%4)" };
            LevelJustification levelJustification4 = new LevelJustification() { Val = LevelJustificationValues.Left };

            NumberingSymbolRunProperties numberingSymbolRunProperties4 = new NumberingSymbolRunProperties();
            RunFonts runFonts4 = new RunFonts() { ComplexScript = "Times New Roman" };

            numberingSymbolRunProperties4.Append(runFonts4);

            level4.Append(startNumberingValue4);
            level4.Append(numberingFormat4);
            level4.Append(paragraphStyleIdInLevel4);
            level4.Append(levelText4);
            level4.Append(levelJustification4);
            level4.Append(numberingSymbolRunProperties4);

            levelOverride4.Append(startOverrideNumberingValue4);
            levelOverride4.Append(level4);

            LevelOverride levelOverride5 = new LevelOverride() { LevelIndex = 4 };
            StartOverrideNumberingValue startOverrideNumberingValue5 = new StartOverrideNumberingValue() { Val = 1 };

            Level level5 = new Level() { LevelIndex = 4 };
            //StartNumberingValue startNumberingValue5 = new StartNumberingValue() { Val = 1 };
            //NumberingFormat numberingFormat5 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            //LevelText levelText5 = new LevelText() { Val = "%5" };
            //LevelJustification levelJustification5 = new LevelJustification() { Val = LevelJustificationValues.Left };

            //NumberingSymbolRunProperties numberingSymbolRunProperties5 = new NumberingSymbolRunProperties();
            //RunFonts runFonts5 = new RunFonts() { ComplexScript = "Times New Roman" };

            //numberingSymbolRunProperties5.Append(runFonts5);

            //level5.Append(startNumberingValue5);
            //level5.Append(numberingFormat5);
            //level5.Append(levelText5);
            //level5.Append(levelJustification5);
            //level5.Append(numberingSymbolRunProperties5);

            //levelOverride5.Append(startOverrideNumberingValue5);
            //levelOverride5.Append(level5);

            LevelOverride levelOverride6 = new LevelOverride() { LevelIndex = 5 };
            StartOverrideNumberingValue startOverrideNumberingValue6 = new StartOverrideNumberingValue() { Val = 1 };

            Level level6 = new Level() { LevelIndex = 5 };
            StartNumberingValue startNumberingValue6 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat6 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            LevelText levelText6 = new LevelText() { Val = "%6" };
            LevelJustification levelJustification6 = new LevelJustification() { Val = LevelJustificationValues.Left };

            NumberingSymbolRunProperties numberingSymbolRunProperties6 = new NumberingSymbolRunProperties();
            RunFonts runFonts6 = new RunFonts() { ComplexScript = "Times New Roman" };

            numberingSymbolRunProperties6.Append(runFonts6);

            level6.Append(startNumberingValue6);
            level6.Append(numberingFormat6);
            level6.Append(levelText6);
            level6.Append(levelJustification6);
            level6.Append(numberingSymbolRunProperties6);

            levelOverride6.Append(startOverrideNumberingValue6);
            levelOverride6.Append(level6);

            LevelOverride levelOverride7 = new LevelOverride() { LevelIndex = 6 };
            StartOverrideNumberingValue startOverrideNumberingValue7 = new StartOverrideNumberingValue() { Val = 1 };

            Level level7 = new Level() { LevelIndex = 6 };
            StartNumberingValue startNumberingValue7 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat7 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            LevelText levelText7 = new LevelText() { Val = "%7" };
            LevelJustification levelJustification7 = new LevelJustification() { Val = LevelJustificationValues.Left };

            NumberingSymbolRunProperties numberingSymbolRunProperties7 = new NumberingSymbolRunProperties();
            RunFonts runFonts7 = new RunFonts() { ComplexScript = "Times New Roman" };

            numberingSymbolRunProperties7.Append(runFonts7);

            level7.Append(startNumberingValue7);
            level7.Append(numberingFormat7);
            level7.Append(levelText7);
            level7.Append(levelJustification7);
            level7.Append(numberingSymbolRunProperties7);

            levelOverride7.Append(startOverrideNumberingValue7);
            levelOverride7.Append(level7);

            LevelOverride levelOverride8 = new LevelOverride() { LevelIndex = 7 };
            StartOverrideNumberingValue startOverrideNumberingValue8 = new StartOverrideNumberingValue() { Val = 1 };

            Level level8 = new Level() { LevelIndex = 7 };
            StartNumberingValue startNumberingValue8 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat8 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            LevelText levelText8 = new LevelText() { Val = "%8" };
            LevelJustification levelJustification8 = new LevelJustification() { Val = LevelJustificationValues.Left };

            NumberingSymbolRunProperties numberingSymbolRunProperties8 = new NumberingSymbolRunProperties();
            RunFonts runFonts8 = new RunFonts() { ComplexScript = "Times New Roman" };

            numberingSymbolRunProperties8.Append(runFonts8);

            level8.Append(startNumberingValue8);
            level8.Append(numberingFormat8);
            level8.Append(levelText8);
            level8.Append(levelJustification8);
            level8.Append(numberingSymbolRunProperties8);

            levelOverride8.Append(startOverrideNumberingValue8);
            levelOverride8.Append(level8);

            numberingInstance1.Append(abstractNumId1);
            numberingInstance1.Append(levelOverride1);
            //numberingInstance1.Append(levelOverride2);
            numberingInstance1.Append(levelOverride3);
            numberingInstance1.Append(levelOverride4);
            numberingInstance1.Append(levelOverride5);
            numberingInstance1.Append(levelOverride6);
            numberingInstance1.Append(levelOverride7);
            numberingInstance1.Append(levelOverride8);
            return numberingInstance1;
        }
    }
}
