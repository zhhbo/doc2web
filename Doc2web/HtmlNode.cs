using System;
using System.Collections.Generic;
using System.Linq;

namespace Doc2web
{
    public class HtmlNode
    {
        private Dictionary<string, string> _attributes = new Dictionary<string, string>();

        public HtmlNode()
        {
            Tag = "div";
            Z = int.MinValue;
        }

        public int Start { get; set; }

        public int End { get; set; }

        public string Tag { get; set; }

        public int Z { get; set; }

        public IReadOnlyCollection<string> Classes
        {
            get
            {
                string val = "";
                if (_attributes.TryGetValue("class", out val))
                    return val.Split(' ');
                else
                    return new string[] { };
            }
        }
        public IReadOnlyDictionary<string, string> Attributes =>
            _attributes;

        public IReadOnlyDictionary<string, string> Style
        {
            get
            {
                string style;
                if (_attributes.TryGetValue("style", out style))
                    return ParseStyleAttribute(style);
                else
                    return new Dictionary<string, string>();
            }
        }

        private static Dictionary<string, string> ParseStyleAttribute(string style)
        {
            var keyValuePair =
                style
                .Split(';')
                .Select(x =>
                {
                    var y = x.Split(':');
                    return (y[0].TrimStart(), y[1].TrimStart());
                });

            var result = new Dictionary<string, string>();
            foreach ((string name, string val) in keyValuePair)
                result.Add(name, val);

            return result;
        }

        public void AddClass(string name)
        {
            string defaultVal = "";
            _attributes.TryGetValue("class", out defaultVal);
            _attributes["class"] = (defaultVal + " " + name).Trim();
        }

        public void SetStyle(string name, string value)
        {
            if (name.Contains(':') || value.Contains(':'))
                throw new ArgumentException("The value or the name contains a \":\" character");
            if (name.Contains(';') || value.Contains(';'))
                throw new ArgumentException("The value or the name contains a \";\" character");
            
            var style = Style;

            if (style.Count == 0)
                SetSingleStyleValue(name, value);
            else if (style.ContainsKey(name))
                UpdateStyleValue(name, value, (IDictionary<string, string>)style);
            else
                AddStyleValue(name, value);
        }

        private void AddStyleValue(string name, string value)
        {
            _attributes["style"] =
                String.Format("{0}; {1}: {2}", _attributes["style"], name, value);
        }

        private void SetSingleStyleValue(string name, string value)
        {
            _attributes["style"] = String.Format("{0}: {1}", name, value);
        }

        private void UpdateStyleValue(string name, string value, IDictionary<string, string> style)
        {
            style[name] = value;
            _attributes["style"] =
                string.Join(
                    ";",
                    style.Select(x => String.Format("{0}: {1}", x.Key, x.Value))
                );
        }

        public void SetAttribute(string name, string value)
        {
            if (name == "style" || name == "class")
                throw new ArgumentException("Please use the SetStyle or AddClass method.");

            _attributes[name] = value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as HtmlNode;
            if (other == null) return false;

            return
                other.Tag == Tag &&
                other.Start == Start &&
                other.End == End &&
                other.Z == Z &&
                other._attributes.SequenceEqual(_attributes);
        }

        public HtmlNode Clone() =>
            new HtmlNode
            {
                Tag = Tag,
                Start = Start,
                End = End,
                Z = Z,
                _attributes = new Dictionary<string, string>(_attributes)
            };

        public bool HasIntersection(HtmlNode other) =>
            (Start < other.Start && other.Start < End) ||
            (Start < other.End && other.End < End) ||
            (other.Start < Start && Start < other.End) ||
            (other.Start < End && End < other.End);
    }
}
