using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering
{
    public class ParagraphStateCacheFactory
    {
        private Styles _styles;
        private Body _body;

        private class NumberList
        {
            public int NumberingInstanceId { get; set; }
            public IEnumerable<NumberItem> Items { get; set; }
        }

        private class NumberItem
        {
            public Paragraph Paragraph { get; set; }
            public int IndentationLevel { get; set; }
        }

        public ParagraphStateCacheFactory(Styles styles, Body body)
        {
            _styles = styles;
            _body = body;
        }

        public ParagraphStateCache Create()
        {
            var cache = new ParagraphStateCache();
            CacheNumberLists(cache);
            return cache;
        }

        private void CacheNumberLists(ParagraphStateCache cache)
        {
            foreach (var numberList in NumberLists)
            {
                //  Parallel.ForEach(NumberLists, numberList =>
                //{
                var vector = new List<int>();

                foreach (var item in numberList.Items)
                    CacheItem(numberList.NumberingInstanceId, cache, vector, item);
                //});
            }
        }

        private static void CacheItem(
          int numberingInstanceId,
          ParagraphStateCache cache,
          List<int> vector,
          NumberItem item)
        {
            var targetedIdentationLevel = item.IndentationLevel;
            var currentIndentationLevel = vector.Count - 1;
            if (targetedIdentationLevel <= currentIndentationLevel)
            {
                vector[targetedIdentationLevel]++;
                if (targetedIdentationLevel < currentIndentationLevel)
                    vector.RemoveRange(targetedIdentationLevel + 1, vector.Count - targetedIdentationLevel - 1);
            }
            else
            {
                while (vector.Count <= item.IndentationLevel)
                    vector.Add(0);
            }

            var itemVector = new int[vector.Count];
            vector.CopyTo(itemVector);

            cache.Add(item.Paragraph, new ParagraphState
            {
                Indentations = itemVector,
                NumberingInstanceId = numberingInstanceId
            });
        }

        private IEnumerable<NumberList> NumberLists =>
           _body
            .Elements<Paragraph>()
            .Select(p => (p, FindAssociatedNumberingInstance(p)))
            .Where(t => t.Item2.HasValue)
            .Select(t => (t.Item1, t.Item2.Value, FindAssociatedNumberingLevel(t.Item1)))
            .GroupBy(t => t.Item2)
            .Select(t => new NumberList
            {
                NumberingInstanceId = t.Key,
                Items = t.Select(x => new NumberItem
                {
                    Paragraph = x.Item1,
                    IndentationLevel = x.Item3
                })
            });

        public int? FindAssociatedNumberingInstance(Paragraph p)
        {
            var pProps =
              p.Elements<ParagraphProperties>()
              .SingleOrDefault();

            if (pProps == null) return null;

            var numId = FindNumInstanceId(pProps);
            if (numId.HasValue && numId == 0) return null;
            return numId;
        }

        private int? FindNumInstanceId(ParagraphProperties pProps)
        {
            var numberingId =
              pProps
              .Descendants<NumberingId>()
              .SingleOrDefault()?.Val?.Value;

            if (numberingId.HasValue) return numberingId;

            var paragraphStyleId =
              pProps
              .Elements<ParagraphStyleId>()
              .SingleOrDefault()?
              .Val?.Value;

            return TryFindNumberingIdUsingStyleId(paragraphStyleId);
        }

        private int? TryFindNumberingIdUsingStyleId(string styleId)
        {
            while (styleId != null)
            {
                var style =
                    _styles
                    .Elements<DocumentFormat.OpenXml.Wordprocessing.Style>()
                    .Single(x => x.StyleId?.Value == styleId);

                var result =
                    style
                    .Descendants<NumberingId>()
                    .SingleOrDefault()?.Val?.Value;

                if (result.HasValue) return result;

                styleId =
                    style
                    .Descendants<BasedOn>()
                    .SingleOrDefault()?.Val?.Value;
            }

            return null;
        }

        public int FindAssociatedNumberingLevel(Paragraph p)
        {
            var pProps =
              p.Elements<ParagraphProperties>()
              .SingleOrDefault();

            if (pProps == null) return 0;

            var level =
                pProps
                .Descendants<NumberingLevelReference>()
                .SingleOrDefault()?
                .Val?.Value;

            if (level.HasValue) return level.Value;

            string styleId =
                pProps
                .Elements<ParagraphStyleId>()
                .Single()?
                .Val?.Value;

            return FindNubmeringLevelUsingStyleId(styleId);
        }

        private int FindNubmeringLevelUsingStyleId(string styleId)
        {
            int? result = null;

            while (styleId != null)
            {
                var style =
                  _styles
                  .Elements<DocumentFormat.OpenXml.Wordprocessing.Style>()
                  .Single(x => x.StyleId?.Value == styleId);

                result =
                  style
                  .Descendants<NumberingLevelReference>()
                  .SingleOrDefault()?.Val?.Value;

                if (result.HasValue) return result.Value;

                styleId =
                  style
                  .Descendants<BasedOn>()
                  .SingleOrDefault()?
                  .Val?.Value;
            }

            return 0;
        }

    }
}
