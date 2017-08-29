using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style
{
    public class BaseMockProp : ICssProperty
    {
        private CssData _cssData = new CssData();

        public BaseMockProp() { }
        public (string, string, string) Out
        {
            set
            {
                _cssData.AddAttribute(value.Item1, value.Item2, value.Item3);
            }
        }
        public string Selector { get; set; }
        public OpenXmlElement OpenXmlElement { get; set; }

        public CssData AsCss() => _cssData;

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }

    public class MockProp1 : BaseMockProp { }
    public class MockProp2 : BaseMockProp { }
    public class MockProp3 : BaseMockProp { }
    public class MockProp4 : BaseMockProp { }

}
