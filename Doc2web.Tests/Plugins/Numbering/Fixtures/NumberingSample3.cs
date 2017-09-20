using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Doc2web.Tests.Plugins.Numbering.Fixtures
{
    public static class NumberingSample3
    {
        public static WordprocessingDocument BuildDoc()
        {
            var wpDoc = WordprocessingDocument.Create(new MemoryStream(), DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
            wpDoc.AddMainDocumentPart();
            var mainPart = wpDoc.MainDocumentPart;
            mainPart.AddNewPart<NumberingDefinitionsPart>();
            mainPart.AddNewPart<StyleDefinitionsPart>();

            mainPart.NumberingDefinitionsPart.Numbering = new DocumentFormat.OpenXml.Wordprocessing.Numbering(
                BuildAbstractNum(1),
                BuildAbstractNumStyleLink(2, "num2style"),
                BuildAbstractNumStyleLink(3, "num3style"),
                BuildAbstractNumStyleLink(4, "num4style"),
                BuildNumberingInstance(10, 1),
                BuildNumberingInstance(20, 2),
                BuildNumberingInstance(30, 3)
            );

            mainPart.StyleDefinitionsPart.Styles = new DocumentFormat.OpenXml.Wordprocessing.Styles(
                BuildNumberingStyle("num2style", 1),
                BuildNumberingStyle("num3style", 4),
                BuildNumberingStyle("num4style", 3)
            );

            return wpDoc;
        }

        public static NumberingInstance BuildNumberingInstance(int numId, int abstractNum)
        {
            LevelOverride levelOverride = new LevelOverride
            {
                LevelIndex = 0,
                Level = new Level {  LevelIndex = 0 }
            };
            var numberingInstance = new NumberingInstance(levelOverride)
            {
                NumberID = numId,
                AbstractNumId = new AbstractNumId() {  Val = abstractNum }
            };

            return numberingInstance;
        }

        public static AbstractNum BuildAbstractNum(int anumId)
        {
            var abstractNum = new AbstractNum(
                new Level { LevelIndex = 0 },
                new Level { LevelIndex = 1 },
                new Level { LevelIndex = 2 }
            )
            {
                AbstractNumberId = anumId,
            };

            return abstractNum;
        }

        public static AbstractNum BuildAbstractNumStyleLink(int anumId, string styleId)
        {
            var abstractNum = new AbstractNum
            {
                AbstractNumberId = anumId,
                StyleLink = new StyleLink { Val = styleId }
            };

            return abstractNum;
        }

        public static DocumentFormat.OpenXml.Wordprocessing.Style BuildNumberingStyle(string styleId, int abstractNum)
        {
            var style = new DocumentFormat.OpenXml.Wordprocessing.Style
            {
                StyleId = styleId,
                Type = StyleValues.Numbering,
                StyleParagraphProperties = new StyleParagraphProperties
                {
                    NumberingProperties = new NumberingProperties
                    {
                        NumberingId = new NumberingId { Val = abstractNum }
                    }
                }
            };
            return style;
        }

    }
}
