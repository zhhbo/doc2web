using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;

namespace Doc2web.Plugins.Style
{
    public class NumberingProvider : INumberingProvider
    {
        private Styles _styles;
        private DocumentFormat.OpenXml.Wordprocessing.Numbering _numbering;

        private class Cursor
        {
            public int level;
            public int? anumID;
            public string styleId;
            public List<Level> results = new List<Level>();
            public HashSet<string> addedStyles = new HashSet<string>();
        }

        public NumberingProvider(
            DocumentFormat.OpenXml.Wordprocessing.Numbering numbering,
            Styles styles)
        {
            _styles = styles;
            _numbering = numbering;
        }

        public List<Level> Collect(int numberingId, int level)
        {
            if (_numbering == null) return new List<Level>();

            var num = _numbering
                .Elements<NumberingInstance>()
                .FirstOrDefault(x => x.NumberID?.Value == numberingId);
            if (num == null) return new List<Level>();

            var cursor = new Cursor { anumID = num.AbstractNumId?.Val, level = level };

            AddFromAnum(cursor);
            AddFromOverride(level, num, cursor);

            return cursor.results;
        }

        private static void AddFromOverride(int level, NumberingInstance num, Cursor cursor)
        {
            var numOverride =
                num
                .Elements<LevelOverride>()
                .FirstOrDefault(x => x.LevelIndex?.Value == level)
                ?.Level;

            if (numOverride != null)
                cursor.results.Add(numOverride);
        }

        private void AddFromAnum(Cursor cursor)
        {
            if (!cursor.anumID.HasValue) return;

            var anum = _numbering
                .Elements<AbstractNum>()
                .FirstOrDefault(x => x.AbstractNumberId?.Value == cursor.anumID.Value);

            if (anum == null) return;

            if (anum.StyleLink?.Val?.Value != null)
            {
                cursor.styleId = anum.StyleLink.Val.Value;
                AddFromStyleId(cursor);
            }

            var anumLevel =
                anum
                .Elements<Level>()
                .FirstOrDefault(x => x.LevelIndex?.Value == cursor.level);

            if (anumLevel != null) cursor.results.Add(anumLevel);
        }

        private void AddFromStyleId(Cursor cursor)
        {
            if (cursor.addedStyles.Contains(cursor.styleId))
                throw new InvalidOperationException();

            cursor.addedStyles.Add(cursor.styleId);
            int? anumId = FindAnumFromStyleId(cursor.styleId);

            if (anumId.HasValue)
            {
                cursor.anumID = anumId.Value;
                AddFromAnum(cursor);
            }
        }

        private int? FindAnumFromStyleId(string styleId) => _styles
                .Elements<DocumentFormat.OpenXml.Wordprocessing.Style>()
                .SingleOrDefault(x => x.StyleId == styleId)
                .StyleParagraphProperties?
                .NumberingProperties?
                .NumberingId?
                .Val?.
                Value;
    }
}
