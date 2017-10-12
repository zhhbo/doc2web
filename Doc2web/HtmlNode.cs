using System;
using System.Collections.Generic;
using System.Linq;

namespace Doc2web
{
    /// <summary>
    /// Virtual html node used to decribe the conversion output.
    /// This is used by the plugin to affect the final result.
    /// </summary>
    public class HtmlNode
    {
        private Dictionary<string, string> _attributes = new Dictionary<string, string>();

        public HtmlNode()
        {
            Tag = "div";
            Z = int.MinValue;
        }

        /// <summary>
        /// Position where the opening tag should be placed.
        /// </summary>
        public double Start { get; set; }

        /// <summary>
        /// Position where the closing tag should be placed.
        /// </summary>
        public double End { get; set; }

        /// <summary>
        /// Name of the DOM Node (ex: p, a, span, div).
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Position in the third dimension that will be used to calculate colisions and prevent
        /// invalid html.
        /// </summary>
        public int Z { get; set; }

        /// <summary>
        /// Text that will be inserted right after the opening tag.
        /// </summary>
        public string TextPrefix { get; set; }

        /// <summary>
        /// Text that will be inserted right before the opening tag.
        /// </summary>
        public string TextSuffix { get; set; }

        /// <summary>
        /// Css classes that have been added to this DOM element.
        /// </summary>
        public IReadOnlyCollection<string> Classes
        {
            get
            {
                if (_attributes.TryGetValue("class", out string val))
                    return val.Split(' ');
                else
                    return new string[] { };
            }
        }

        /// <summary>
        /// Html attributes that have been added to this DOM element.
        /// The first value of the tupple is the name of the attribute and the 
        /// seccond value is the value of the attribute.
        /// </summary>
        public IReadOnlyDictionary<string, string> Attributes =>
            _attributes;

        /// <summary>
        /// Css properties that are registred in the style attribute.
        /// The first value of the tupple is the name of the css property and the
        /// seccond value is the value of the attribute.
        /// </summary>
        public IReadOnlyDictionary<string, string> Style
        {
            get
            {
                if (_attributes.TryGetValue("style", out string style))
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

        /// <summary>
        /// Add multuiple css classes to the DOM node.
        /// </summary>
        /// <param name="names">Css classes to be added.</param>
        public void AddClasses(params string[] names)
        {
            for(int i =0; i< names.Length; i++)
            {
                string name = names[i];
                if (name == null || name.Length == 0) continue;
                AddClass(name);
            }
        }

        /// <summary>
        /// Add a single css class to the DOM node.
        /// </summary>
        /// <param name="name">Css class to be addded.</param>
        private void AddClass(string name)
        {
            _attributes.TryGetValue("class", out string defaultVal);
            _attributes["class"] = (defaultVal + " " + name).Trim();
        }

        /// <summary>
        /// Set an inline css property that will be in the "style" attribute.
        /// </summary>
        /// <param name="name">Name of the css property (ex: color).</param>
        /// <param name="value">Value of the css property (ex: red).</param>
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

        /// <summary>
        /// Set the value of one of the DOM element attribute.
        /// </summary>
        /// <param name="name">Name of the attribute.</param>
        /// <param name="value">Value of the attribute.</param>
        public void SetAttribute(string name, string value)
        {
            if (name == "style" || name == "class")
                throw new ArgumentException("Please use the SetStyle or AddClass method.");

            _attributes[name] = value;
        }

        /// <summary>
        /// Compare the properties the other object if it's a HtmlNode.
        /// Compare the reference if the object is not an HtmlNode.
        /// </summary>
        /// <param name="obj">Object to compare with.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var other = obj as HtmlNode;
            if (other == null) return base.Equals(obj);

            return
                other.Tag == Tag &&
                other.Start == Start &&
                other.End == End &&
                other.Z == Z &&
                other._attributes.SequenceEqual(_attributes);
        }

        /// <summary>
        /// Returns a deep copy of the HtmlNode.
        /// </summary>
        /// <returns>Deep copy of the HtmlNode.</returns>
        public HtmlNode Clone() =>
            new HtmlNode
            {
                Tag = Tag,
                Start = Start,
                End = End,
                Z = Z,
                _attributes = new Dictionary<string, string>(_attributes)
            };

        /// <summary>
        /// Returns TRUE if the other HtmlNode is overlapsing this current HtmlNode.
        /// </summary>
        /// <param name="other">Other HtmlNode that could have an intersection.</param>
        /// <returns></returns>
        public bool HasIntersection(HtmlNode other) =>
            (Start < other.Start && other.Start < End) ||
            (Start < other.End && other.End < End) ||
            (other.Start < Start && Start < other.End) ||
            (other.Start < End && End < other.End);

        public override int GetHashCode()
        {
            var hashCode = -1757873924;
            hashCode = hashCode * -1521134295 + Start.GetHashCode();
            hashCode = hashCode * -1521134295 + End.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Tag);
            return hashCode;
        }
    }
}
