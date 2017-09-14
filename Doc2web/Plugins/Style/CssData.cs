using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class CssData
    {
        private SortedDictionary<string, SortedDictionary<string, SortedDictionary<string, string>>> _data;

        public CssData()
        {
            _data = new SortedDictionary<string, SortedDictionary<string, SortedDictionary<string, string>>>();
        }

        public string[] Selectors => _data.Keys.ToArray();

        public SortedDictionary<string, SortedDictionary<string, string>> this[string mediaQuery] =>
            _data[mediaQuery];

        public IDictionary<string, string> GetMediaQuery(string mediaQuery, string selector)
        {
            throw new NotImplementedException();
        }

        public void AddAttribute(string selector, string name, string value)
        {
            AddAttribute("", selector, name, value);
        }

        public void AddAttribute(string mediaQuery, string selector, string name, string value)
        {
            if (_data.TryGetValue(mediaQuery, out SortedDictionary<string, SortedDictionary<string, string>> mq))
            {
                if (mq.TryGetValue(selector, out SortedDictionary<string, string> props))
                    props[name] = value;
                else
                    mq[selector] = new SortedDictionary<string, string> { { name, value } };
            }
            else
            {
                _data[mediaQuery] = new SortedDictionary<string, SortedDictionary<string, string>>
                {
                    { selector, new SortedDictionary<string, string> { { name, value } } }
                };
            }
        }

        public void AddRange(CssData other)
        {
            foreach (var mq in other._data)
                foreach (var selector in mq.Value)
                    foreach (var prop in selector.Value)
                        AddAttribute(mq.Key, selector.Key, prop.Key, prop.Value);
        }

        public override bool Equals(object obj)
        {
            var other = obj as CssData;
            if (other == null) return false;

            var otherData = other._data;

            if (otherData.Count != _data.Count) return false;

            foreach (var kv in otherData)
                if (!AreMediaQueryEquals(kv.Key, kv.Value)) return false;

            return true;
        }

        private bool AreMediaQueryEquals(
            string mq, 
            SortedDictionary<string, SortedDictionary<string, string>> attributes)
        {
            if (_data.TryGetValue(mq, out SortedDictionary<string, SortedDictionary<string, string>> thisAttributes))
            {
                if (!AreSelectorsEquals(thisAttributes, attributes))
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }

        private bool AreSelectorsEquals(
            SortedDictionary<string, SortedDictionary<string, string>> selectorsA,
            SortedDictionary<string, SortedDictionary<string, string>> selectorsB) {
            if (selectorsA.Keys.Count != selectorsB.Keys.Count) return false;
            
            foreach(var selector in selectorsA.Keys)
            {
                var aAttributes = selectorsA[selector];
                if (selectorsB.TryGetValue(selector, out SortedDictionary<string, string> bAttributes))
                {
                    if (!AreAttributesEqual(aAttributes, bAttributes))
                        return false;
                } else
                {
                    return false;
                }
            }

            return true;
        }

        private bool AreAttributesEqual(
            SortedDictionary<string, string> aAttributes, 
            SortedDictionary<string, string> bAttributes)
        {
            if (aAttributes.Count != bAttributes.Count) return false;
            
            foreach(var name in aAttributes.Keys)
            {
                var aVal = aAttributes[name];
                if (bAttributes.TryGetValue(name, out string bVal))
                {
                    if (bVal != aVal) return false;
                } else
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode() => _data.GetHashCode();

        public void RenderInto(StringBuilder sb)
        {
            foreach(var mq in _data)
            {
                if (mq.Key.Length > 0) sb.Append($"@media {mq.Key} {{ ");
                foreach (var selector in mq.Value)
                {
                    sb.Append(selector.Key);
                    sb.Append(" {");
                    RenderPropsInto(sb, selector.Value);
                    sb.Append("}");
                }
                if (mq.Key.Length > 0) sb.Append(" }");
            }
        }

        private static void RenderPropsInto(StringBuilder sb, IEnumerable<KeyValuePair<string, string>> props)
        {
            foreach (var prop in props)
            {
                sb.Append(prop.Key);
                sb.Append(": ");
                sb.Append(prop.Value);
                sb.Append(";");
            }
        }
    }
}
