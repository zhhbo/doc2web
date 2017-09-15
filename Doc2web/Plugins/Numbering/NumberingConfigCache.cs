using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Concurrent;

namespace Doc2web.Plugins.Numbering
{
    public class NumberingConfigCache
    {
        private readonly INumberingConfigFactory _numberingConfigFactory;
        private readonly DocumentFormat.OpenXml.Wordprocessing.Numbering _numbering;
        private readonly ConcurrentDictionary<int, NumberingConfig> _cache = new ConcurrentDictionary<int, NumberingConfig>();
        private Styles _styles;

        public NumberingConfigCache(
          DocumentFormat.OpenXml.Wordprocessing.Numbering numbering,
          DocumentFormat.OpenXml.Wordprocessing.Styles styles,
          INumberingConfigFactory numberingConfigFac)
        {
            _numbering = numbering;
            _styles = styles;
            _numberingConfigFactory = numberingConfigFac;

        }
        public NumberingConfig Get(int numberingId)
        {
            NumberingConfig cachedValue;
            if (_cache.TryGetValue(numberingId, out cachedValue))
                return cachedValue;
            else
                return GetNumberingConfigFromNumberingInstance(numberingId);
        }

        private NumberingConfig GetNumberingConfigFromNumberingInstance(int numberingId)
        {
            NumberingInstance numberingInstance = FindNumberingInstancFromId(numberingId);
            NumberingConfig abstractNumConfig = GetNumberingConfigFromAbstract(numberingInstance);

            var result = _numberingConfigFactory.CreateFromNumbering(abstractNumConfig, numberingInstance);

            _cache.TryAdd(numberingId, result);
            return result;
        }

        private NumberingInstance FindNumberingInstancFromId(int numberingId)
        {
            return _numbering
              .Elements<NumberingInstance>()
              .Single(x => x.NumberID.Value == numberingId);
        }

        private NumberingConfig GetNumberingConfigFromAbstract(NumberingInstance numberingInstance)
        {
            NumberingConfig result = null;
            var abstractNumberingId = numberingInstance.AbstractNumId.Val.Value;
            try
            {
                result = GetNumberingFromAbstractId(abstractNumberingId);
            }
            catch (LinkedStyleNumberingException ex)
            {
                var numberingId = GetNumberingFromStyleId(ex.LinkedStyleId);
                if (numberingId == numberingInstance.NumberID.Value)
                    throw new CircularNumberingException();
                else return Get(numberingId);
            }

            return result;
        }

        private int GetNumberingFromStyleId(string linkedStyleId) =>
          _styles.Elements<DocumentFormat.OpenXml.Wordprocessing.Style>()
          .Single(x => x.StyleId.Value == linkedStyleId)
          .Descendants<NumberingId>()
          .Single()
          .Val.Value;

        private NumberingConfig GetNumberingFromAbstractId(int abstractNumberingId)
        {
            NumberingConfig result;
            AbstractNum abtractNum = FindAbstractNumFromId(abstractNumberingId);
            result = _numberingConfigFactory.CreateFromAbstractNumbering(abtractNum);
            return result;
        }

        private AbstractNum FindAbstractNumFromId(int abstractNumberingId)
        {
            return _numbering
              .Elements<AbstractNum>()
              .Single(x => x.AbstractNumberId.Value == abstractNumberingId);
        }
    }
}
