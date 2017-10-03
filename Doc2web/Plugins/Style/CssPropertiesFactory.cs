using Autofac;
using Autofac.Features.Metadata;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class CssPropertiesFactory : ICssPropertiesFactory
    {
        private Dictionary<Type, Func<ICssProperty>> _constructorsDic;

        public CssPropertiesFactory(
            IEnumerable<Meta<Func<ICssProperty>>> propsConstrutors, 
            CssPropertySource source)
        {
            _constructorsDic = new Dictionary<Type, Func<ICssProperty>>();
            Initialize(propsConstrutors, source);
        }

        private void Initialize(IEnumerable<Meta<Func<ICssProperty>>> propsConstrutors, CssPropertySource source)
        {
            var sourceAttributeName =
                nameof(BaseCssPropertyAttribute.Source);
            var typeAttributeName =
                nameof(BaseCssPropertyAttribute.TargetedType);
            foreach (var meta in propsConstrutors)
            {
                var metaSource = 
                    (CssPropertySource)meta.Metadata[sourceAttributeName];
                if (metaSource == source)
                {
                    _constructorsDic.Add(
                        (Type)meta.Metadata[typeAttributeName],
                        meta.Value
                    );
                }
            }
        }

        public CssPropertiesSet Build(OpenXmlElement elem)
        {
            return new CssPropertiesSet(BuildMultiple(elem.ChildElements));
            //if (_constructorsDic.TryGetValue(
            //    elem.GetType(), 
            //    out Func<ICssProperty> cons))
            //{
            //    var prop = cons();
            //    prop.OpenXmlElement = elem;
            //    return new ICssProperty[1] { prop };
            //}
            //else
            //{
            //    return BuildMultiple(elem.ChildElements);
            //}
        }

        private ICssProperty[] BuildMultiple(OpenXmlElementList childElements)
        {
            var result = new List<ICssProperty>(childElements.Count);
            foreach (var child in childElements)
            {
                var prop = BuildSingle(child);
                if (prop != null) result.Add(prop);
            }
            return result.ToArray();
        }

        private ICssProperty BuildSingle(OpenXmlElement elem)
        {
            if (_constructorsDic.TryGetValue(
                elem.GetType(),
                out Func<ICssProperty> cons))
            {
                var prop = cons();
                prop.OpenXmlElement = elem;
                return prop;
            }
            else return null;
        }
    }
}
