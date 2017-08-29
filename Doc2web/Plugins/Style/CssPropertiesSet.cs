using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class CssPropertiesSet : ICollection<ICssProperty>, IComparer<ICssProperty>
    {
        private SortedSet<ICssProperty> _set;

        public CssPropertiesSet()
        {
            _set = new SortedSet<ICssProperty>(this as IComparer<ICssProperty>);
        }

        public int Count => _set.Count;

        public bool IsReadOnly => false;

        public void Add(ICssProperty item) =>_set.Add(item);

        public void Clear() => _set.Clear();

        public int Compare(ICssProperty x, ICssProperty y)
        {
            if (x.GetType().Equals(y.GetType())) return 0;
            return -1;
        }

        public bool Contains(ICssProperty item) => _set.Contains(item);

        public void CopyTo(ICssProperty[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ICssProperty> GetEnumerator() => _set.GetEnumerator();

        public bool Remove(ICssProperty item) => _set.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public string Selector { get; set; }

        public CssData AsCss()
        {
            var cssData = new CssData();
            foreach(var prop in this)
            {
                prop.Selector = Selector;
                cssData.AddRange(prop.AsCss());
            }
            return cssData;
        }
    }
}
