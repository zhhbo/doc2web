using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Numbering
{
    public class NumberingMapper
    {
        private Body _body;
        private DocumentFormat.OpenXml.Wordprocessing.Numbering _numbering;
        private Styles _styles;
        private ConfigCache _nconfCache;
        private ParagraphStateCache _pstateCache;

        public NumberingMapper(WordprocessingDocument wpDoc)
        {
            _body = wpDoc.MainDocumentPart.Document.Body;
            _numbering = wpDoc.MainDocumentPart.NumberingDefinitionsPart?.Numbering;
            _styles = wpDoc.MainDocumentPart.StyleDefinitionsPart.Styles;

            if (_numbering != null)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            try
            {
                _nconfCache = new ConfigCache(_numbering, _styles, new ConfigFactory());
                _pstateCache = new ParagraphStateCacheFactory(_styles, _body).Create();
            }
            catch { }
        }

        public bool IsValid => _nconfCache != null && _pstateCache != null;

        public ParagraphData GetNumbering(Paragraph p)
        {
            try
            {
                return TryGetNumbering(p);
            }
            catch (CircularNumberingException)
            {
                return null;
            }
        }

        private ParagraphData TryGetNumbering(Paragraph p)
        {
            if (!IsValid) return null;

            var state = _pstateCache.Get(p);
            if (state == null) return null;

            var nconf = _nconfCache.Get(state.NumberingInstanceId);
            var numId = nconf.NumberingId.Value;
            var text = nconf.Render(state.Indentations);
            var levelIndex = state.Indentations.Count() - 1;
            var level = nconf[levelIndex].LevelNode;

            return new ParagraphData(nconf, state);
        }
    }
}
