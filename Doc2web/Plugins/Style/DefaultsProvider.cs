using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class DefaultsProvider : IDefaultsProvider
    {
        private Styles _styles;
        private ICssPropertiesFactory _pPropsFac;
        private ICssPropertiesFactory _rPropsFac;
        private CssPropertiesSet _rCache;
        private CssPropertiesSet _pCache;

        public DefaultsProvider(
            Func<CssPropertySource, ICssPropertiesFactory> facBuiler,
            Styles styles)
        {
            _pPropsFac = facBuiler(CssPropertySource.Paragraph);
            _rPropsFac = facBuiler(CssPropertySource.Run);
            _styles = styles;
        }

        public CssPropertiesSet Paragraph
        {
            get
            {
                if (_pCache == null) InitPCache();
                return _pCache;
            }
        }

        private void InitPCache()
        {
            var pDocDefaults = _styles.DocDefaults?.ParagraphPropertiesDefault;
            if (pDocDefaults != null)
            {
                var props = _pPropsFac.Build(pDocDefaults);
                var set = new CssPropertiesSet();
                set.AddMany(props);
                _pCache = set;
            } else
            {
                _pCache = new CssPropertiesSet();
            }
        }

    

        public CssPropertiesSet Run
        {
            get
            {
                if (_rCache == null) InitRCache();
                return _rCache;
            }
        }

        private void InitRCache()
        {
            var rDocDefaults = _styles.DocDefaults?.RunPropertiesDefault;
            if (rDocDefaults != null)
            {
                var props = _rPropsFac.Build(rDocDefaults);
                var set = new CssPropertiesSet();
                set.AddMany(props);
                _rCache = set;
            } else
            {
                _rCache = new CssPropertiesSet();
            }
        }
    }
}
