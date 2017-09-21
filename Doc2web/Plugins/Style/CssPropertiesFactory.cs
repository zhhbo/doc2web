using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;
using Autofac;
using Doc2web.Plugins.Style.Properties;
using System.Linq;

namespace Doc2web.Plugins.Style
{
    public class CssPropertiesFactory : ICssPropertiesFactory
    {
        private ILifetimeScope _lifetimeScope;

        public CssPropertiesFactory(ILifetimeScope lifeTimeScope)
        {
            _lifetimeScope = lifeTimeScope;
        }


        public ICssProperty[] BuildNumbering(OpenXmlElement element)
        {
            return Build<NumberingCssPropertyAttribute>(element);
        }

        public ICssProperty[] BuildParagraph(OpenXmlElement element)
        {
            return Build<ParagraphCssPropertyAttribute>(element);
        }

        public ICssProperty[] BuildRun(OpenXmlElement element)
        {
            return Build<RunCssPropertyAttribute>(element);
        }

        public ICssProperty[] Build<T>(OpenXmlElement element)
            where T : Attribute
        {
            List<ICssProperty> props = new List<ICssProperty>();
            TryAddCssProperty<T>(props, element);

            foreach(var childElement in element.ChildElements)
                TryAddCssProperty<T>(props, childElement);
            
            return props.ToArray();
        }

        private void TryAddCssProperty<T>(List<ICssProperty> container, OpenXmlElement elem)
            where T : Attribute
        {
            var prop = TryGetProp(container, elem.GetType(), typeof(T));
            if (prop == null) return;

            prop.OpenXmlElement = elem;
            container.Add(prop);
        }

        private ICssProperty TryGetProp(
            List<ICssProperty> container, 
            Type expectedXmlType,
            Type expectedAttributeType)
        {
            var expectedPropType = typeof(CssProperty<>).MakeGenericType(expectedXmlType);
            var expectedEnum = typeof(IEnumerable<>).MakeGenericType(expectedPropType);
            if (_lifetimeScope.TryResolve(expectedEnum, out object props))
                return GetPropWithAttribute(
                    (IEnumerable<ICssProperty>)props, 
                    expectedAttributeType);

            return null;
        }

        private static ICssProperty GetPropWithAttribute(IEnumerable<ICssProperty> props, Type t) =>
            props
            .FirstOrDefault(x => HasAttribute(x, t));

        private static bool HasAttribute(ICssProperty prop, Type t) =>
            prop
            .GetType()
            .GetCustomAttributes(true)
            .Any(x => x.GetType() == t);

    }
}
