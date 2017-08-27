using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class CssData 
    {
        private Dictionary<string, Dictionary<string, string>> _data;

        public CssData()
        {
            _data = new Dictionary<string, Dictionary<string, string>>();
        }

        public string[] Selectors => _data.Keys.ToArray();

        public IDictionary<string, string> this[string selector] => _data[selector];

        public void AddAttribute(string selector, string name, string value)
        {
            if (_data.TryGetValue(selector, out Dictionary<string, string> props))
                props[name] = value;
            else
                _data[selector] = new Dictionary<string, string> { { name, value } };

        }

        public override bool Equals(object obj)
        {
            var other = obj as CssData;
            if (other == null) return false;
            var otherData = other._data;

            if (otherData.Count != _data.Count) return false;

            foreach(var kv in otherData)
                if (!AreAttributeEquals(kv.Key, kv.Value)) return false;

            return true;
        }

        private bool AreAttributeEquals(string selector, IDictionary<string, string> attributes)
        {
            if (_data.TryGetValue(selector, out Dictionary<string, string> thisAttributes))
            {
                if (!thisAttributes.SequenceEqual(attributes))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode() => _data.GetHashCode();
    }
}
