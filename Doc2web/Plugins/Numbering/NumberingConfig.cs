﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering
{
    public class NumberingConfig : IEnumerable<IIndentationConfig>
    {

        private static readonly Regex _textParser = new Regex(@"\%(?<I>\d)", RegexOptions.Compiled);

        private readonly List<IIndentationConfig> _identations = new List<IIndentationConfig>();

        public int AbstractNumberingId { get; set; }

        public int? NumberingId { get; set; }

        public int LevelCount => _identations.Count;

        public void AddLevel(IIndentationConfig identationConfig)
        {
            _identations.Add(identationConfig);
        }

        public IEnumerator<IIndentationConfig> GetEnumerator() => _identations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _identations.GetEnumerator();

        public IIndentationConfig this[int index]
        {
            get => _identations[index];
        }

        public string Render(IEnumerable<int> indentationVector)
        {
            var count = indentationVector.Count();
            if (count > LevelCount)
                throw new ArgumentException("The indentation vector is longer that the one supported by this configuration.");

            var targetedLevel = this[count - 1];

            return RenderText(targetedLevel, indentationVector.ToArray());
        }

        private string RenderText(IIndentationConfig targetedLevel, int[] indentationVector)
        {
            var text = targetedLevel.Text;
            foreach (Match m in _textParser.Matches(targetedLevel.Text))
            {
                var indentationLevel = int.Parse(m.Groups["I"].Value) - 1;
                var indentationConfig = this[indentationLevel];
                var identationValue = indentationVector[indentationLevel] + indentationConfig.StartValue;
                var forceNumeric = targetedLevel.ForceNumbericRendering && indentationLevel != targetedLevel.LevelIndex;
                var formattedLevel = (forceNumeric) ? identationValue.ToString() : indentationConfig.RenderNumber(identationValue);

                text = text.Replace("%" + (indentationLevel + 1), formattedLevel);
            }

            return text;
        }
    }
}
