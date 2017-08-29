using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class CssData 
    {
        private SortedDictionary<string, SortedDictionary<string, string>> _data;

        public CssData()
        {
            _data = new SortedDictionary<string, SortedDictionary<string, string>>();
        }

        public string[] Selectors => _data.Keys.ToArray();

        public IDictionary<string, string> this[string selector] => _data[selector];

        public void AddAttribute(string selector, string name, string value)
        {
            if (_data.TryGetValue(selector, out SortedDictionary<string, string> props))
                props[name] = value;
            else
                _data[selector] = new SortedDictionary<string, string> { { name, value } };

        }

        public void AddRange(CssData other)
        {
            foreach(var kv in other._data)
                foreach (var prop in kv.Value)
                    AddAttribute(kv.Key, prop.Key, prop.Value);
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
            if (_data.TryGetValue(selector, out SortedDictionary<string, string> thisAttributes))
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
