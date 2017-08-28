using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;
using Autofac;
using Doc2web.Plugins.Style.Properties;

namespace Doc2web.Plugins.Style
{
    public class CssPropertiesFactory : ICssPropertiesFactory
    {
        private ILifetimeScope _lifetimeScope;

        public CssPropertiesFactory(ILifetimeScope lifeTimeScope)
        {
            _lifetimeScope = lifeTimeScope;
        }

        public ICssProperty[] Build(OpenXmlElement element)
        {
            List<ICssProperty> props = new List<ICssProperty>();
            TryAddCssProperty(props, element);

            foreach(var childElement in element.ChildElements)
                TryAddCssProperty(props, childElement);
            
            return props.ToArray();
        }

        private void TryAddCssProperty(List<ICssProperty> container, OpenXmlElement elem)
        {
            var expectedType = typeof(BaseCssProperty<>).MakeGenericType(elem.GetType());
            if (_lifetimeScope.TryResolve(expectedType, out object prop)) {
                var cssProp = (ICssProperty)prop;
                cssProp.OpenXmlElement = elem;
                container.Add(cssProp);
            }
        }
    }
}
