using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class CssPropertiesSet : ICollection<ICssProperty>
    {
        private Dictionary<Type, ICssProperty> _dict;

        public CssPropertiesSet()
        {
            _dict = new Dictionary<Type, ICssProperty>();
        }

        public CssPropertiesSet(ICssProperty[] props)
        {
            _dict = new Dictionary<Type, ICssProperty>(props.Length);
            for(int i = 0; i < props.Length; i++)
                _dict.Add(props[i].GetType(), props[i]);
        }

        public int Count => _dict.Count;

        public bool IsReadOnly => false;

        public void Add(ICssProperty item)
        {
            if (_dict.TryGetValue(item.GetType(), out ICssProperty current))
            {
                current.Extends(item);
            }
            else _dict[item.GetType()] = item;
        }

        public void AddMany(IEnumerable<ICssProperty> items)
        {
            foreach (var item in items) Add(item);
        }

        public void Clear() => _dict.Clear();

        public bool Contains(ICssProperty item)
        {
            if (_dict.TryGetValue(item.GetType(), out ICssProperty inside))
            {
                return inside.GetHashCode() == item.GetHashCode() &&
                    inside.Equals(item);
            }
            return false;
        }

        public void CopyTo(ICssProperty[] array, int arrayIndex)
        {
            var vals = _dict.Values.ToArray();
            vals.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ICssProperty> GetEnumerator() => _dict.Values.GetEnumerator();

        public bool Remove(ICssProperty item) => _dict.Remove(item.GetType());

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public string Selector { get; set; }

        public void InsertCss(CssData cssData)
        {
            foreach(var prop in this)
            {
                prop.Selector = Selector;
                prop.InsertCss(cssData);
            }
        }

        public bool SetEquals(CssPropertiesSet other)
        {
            if (other.Count != Count) return false;
            //foreach (var (a, b) in other.Zip(this, (a, b) => (a, b)))
            //{
            //    if (a.GetHashCode() != b.GetHashCode() || !a.Equals(b))
            //        return false;
            //}
            foreach(var a in _dict.Values)
            {
                if (other._dict.TryGetValue(a.GetType(), out ICssProperty b))
                {
                    if (a.GetHashCode() != b.GetHashCode() || !a.Equals(b))
                        return false;
                }
                else return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return this.Select(x => x.GetHashCode())
                .Sum();
            //return _dict.Keys.Count;
        }

        public CssPropertiesSet Clone()
        {
            var clone = new CssPropertiesSet();
            clone.AddMany(this.Select(x => x.Clone()));
            clone.Selector = Selector;
            return clone;
        }
    }
}
