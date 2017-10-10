using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WStyle = DocumentFormat.OpenXml.Wordprocessing.Style;

namespace Doc2web.Plugins.Style
{
    public class DefaultsProvider : IDefaultsProvider
    {
        private Styles _styles;
        private ICssPropertiesFactory _pPropsFac;
        private ICssPropertiesFactory _rPropsFac;
        private CssPropertiesSet _rCache;
        private CssPropertiesSet _pCache;
        private bool _rDefaultStyleInitialized = false;
        private bool _pDefaultStyleInitialized = false;
        private string _pDefaultStyle;
        private string _rDefaultStyle;

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
            var pDocDefaults = 
                _styles.DocDefaults?.ParagraphPropertiesDefault?.ParagraphPropertiesBaseStyle;

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


        public void Init()
        {
            InitPDefaultStyle();
            InitRDefaultStyle();
        }

        private void InitRDefaultStyle()
        {
            _rDefaultStyle =
                _styles
                .Elements<WStyle>()
                .FirstOrDefault(x =>
                    x.Type.Value == StyleValues.Character && x.Default?.Value == true)?
                .StyleId;
            _rDefaultStyleInitialized = true;
        }

        private void InitPDefaultStyle()
        {
            _pDefaultStyle =
                _styles
                .Elements<WStyle>()
                .FirstOrDefault(x =>
                    x.Type.Value == StyleValues.Paragraph && x.Default?.Value == true)?
                .StyleId;
            _pDefaultStyleInitialized = true;
        }

        public CssPropertiesSet Run
        {
            get
            {
                if (_rCache == null) InitRCache();
                return _rCache;
            }
        }

        public string DefaultParagraphStyle
        {
            get
            {
                if (!_pDefaultStyleInitialized) InitPDefaultStyle();
                return _pDefaultStyle;
            }
        }

        public string DefaultRunStyle
        {
            get
            {
                if (!_rDefaultStyleInitialized) InitRDefaultStyle();
                return _rDefaultStyle;
            }
        }

        private void InitRCache()
        {
            var rDocDefaults = _styles.DocDefaults?.RunPropertiesDefault?.RunPropertiesBaseStyle;
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
