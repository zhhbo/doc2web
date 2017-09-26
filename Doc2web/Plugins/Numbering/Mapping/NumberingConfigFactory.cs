using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering
{
    public class NumberingConfigFactory : INumberingConfigFactory
    {
        /// <summary>
        /// Create a numbering config from a numbering instance using an other numbering config as template.
        /// </summary>
        /// <param name="abstractNumConfig">NumberingConfig used as template.</param>
        /// <param name="numberingInstance">NumberingInstance overriding the template.</param>
        /// <returns>New NumberingConfig combining the two parameters.</returns>
        public NumberingConfig CreateFromNumbering(NumberingConfig abstractNumConfig, NumberingInstance numberingInstance)
        {
            var numberingConfig = new NumberingConfig();
            numberingConfig.AbstractNumberingId = abstractNumConfig.AbstractNumberingId;
            numberingConfig.NumberingId = numberingInstance.NumberID;

            var levelOverrides = numberingInstance.Descendants<LevelOverride>();

            var abstractToBeOverride =
               abstractNumConfig
               .SelectMany(x => FindMatchingLevelOverride(x, levelOverrides));

            var abstractNotToBeOverride =
                abstractNumConfig.Except(abstractToBeOverride.Select(x => x.Item1));

            var levelOverrideWithoutAbstract =
                levelOverrides
                .Except(abstractToBeOverride.Select(x => x.Item2));

            var levels =
              abstractNotToBeOverride
              .Concat(abstractToBeOverride.Select(x => CreateFromOverride(x.Item2, x.Item1)))
              .Concat(levelOverrideWithoutAbstract.Select(x => CreateFromOverride(x)))
              .OrderBy(x => x.LevelIndex);

            foreach (var level in levels) numberingConfig.AddLevel(level);

            return numberingConfig;
        }

        private IEnumerable<(IIndentationConfig, LevelOverride)> FindMatchingLevelOverride(IIndentationConfig identationConfig, IEnumerable<LevelOverride> levelOverrides)
        {
            var level = levelOverrides.Where(y => y.LevelIndex.Value == identationConfig.LevelIndex).SingleOrDefault();
            if (level != null)
                return Enumerable.Repeat((identationConfig, level), 1);
            else
                return Enumerable.Empty<(IIndentationConfig, LevelOverride)>();
        }

        private IIndentationConfig CreateFromOverride(LevelOverride lvlOverride, IIndentationConfig template = null)
        {
            var level = lvlOverride.Level;
            //if (level == null && template == null) throw new ArgumentException("Cannot create level from scratch");

            var result = template?.Clone() ?? new IndentationConfig();

            result.LevelIndex = lvlOverride.LevelIndex.Value;
            result.IsFromAbstract = false;
            result.NumberId = (lvlOverride.Parent as NumberingInstance).NumberID.Value;
            result.StartValue = lvlOverride.StartOverrideNumberingValue?.Val?.Value ?? result.StartValue;

            if (level != null)
            {
                result.NumberingFormat = level.NumberingFormat?.Val?.Value ?? result.NumberingFormat;
                result.ParagraphStyleId = level.ParagraphStyleIdInLevel?.Val?.Value ?? result.ParagraphStyleId;
                result.Text = level.LevelText?.Val?.Value ?? result.Text;
            }

            return result;
        }

        /// <summary>
        /// Create a NumberingConfig from an AbstractNum.
        /// </summary>
        /// <param name="abstractNum">AbstractNum used as template for the numbering config.</param>
        /// <returns>NumeringConfig created from the AbstractNum.</returns>
        public NumberingConfig CreateFromAbstractNumbering(AbstractNum abstractNum)
        {
            var numberingConfig = new NumberingConfig();
            numberingConfig.AbstractNumberingId = abstractNum.AbstractNumberId.Value;

            var levels = abstractNum.Descendants<Level>();
            if (levels.Count() == 0 && abstractNum.Descendants<NumberingStyleLink>().SingleOrDefault() != null)
            {
                var numberingStyleLink = abstractNum.Descendants<NumberingStyleLink>().SingleOrDefault();
                throw new LinkedStyleNumberingException(numberingStyleLink.Val.Value);
            }
            foreach (var level in levels)
            {
                var indentation = CreateIndentationConfigForAbstract(numberingConfig.AbstractNumberingId, level);
                numberingConfig.AddLevel(indentation);
            }

            return numberingConfig;
        }

        private IndentationConfig CreateIndentationConfigForAbstract(int abstractNumbId, Level level)
        {
            var indentationConfig = CreateIndentationConfigFromLevel(level);
            indentationConfig.IsFromAbstract = true;
            return indentationConfig;
        }

        private IndentationConfig CreateIndentationConfigFromLevel(Level arg)
        {
            var identationConfig = new IndentationConfig()
            {
                IsFromAbstract = true,
                LevelIndex = arg.LevelIndex.Value,
                ParagraphStyleId = arg.ParagraphStyleIdInLevel?.Val?.Value,
                NumberingFormat = arg.NumberingFormat.Val.Value,
                Text = arg.LevelText?.Val?.Value ?? "",
                LevelNode = arg,
                ForceNumbericRendering = arg.IsLegalNumberingStyle != null
            };
            var startValue = arg.StartNumberingValue?.Val?.HasValue;
            if (startValue.HasValue && startValue.Value)
            {
                identationConfig.StartValue = arg.StartNumberingValue.Val;
            }
            else
            {
                identationConfig.StartValue = 1;
            }

            return identationConfig;
        }
    }
}
