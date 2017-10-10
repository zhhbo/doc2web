using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Concurrent;
using System.Linq;

namespace Doc2web.Plugins.Style.Css
{
    public class CssRegistrator : ICssRegistrator
    {
        private IParagraphClassFactory _paragraphClassFactory;
        private IRunClassFactory _runClassFactory;
        private ConcurrentDictionary<CssPropertiesSet, CssClass> _registrations;

        public CssRegistrator(
            IParagraphClassFactory paragraphClassFactory,
            IRunClassFactory runClassFactory)
        {
            _paragraphClassFactory = paragraphClassFactory;
            _runClassFactory = runClassFactory;
            _registrations = new ConcurrentDictionary<CssPropertiesSet, CssClass>();
        }

        public IEnumerable<CssClass> Registrations => _registrations.Values;

        public CssClass RegisterParagraph(
            ParagraphProperties inlineProps,
            (int, int)? numbering=null)
        {
            var cssClass = _paragraphClassFactory.Build(new ParagraphClassParam
            {
                InlineProperties = inlineProps,
                StyleId = inlineProps?.ParagraphStyleId?.Val,
                NumberingId = numbering?.Item1,
                NumberingLevel = numbering?.Item2
            });

            return AddOrSet(cssClass);
        }

        public CssClass RegisterRun(
            ParagraphProperties parentProps, 
            OpenXmlElement inlineProps,
            (int, int)? numbering=null)
        {
            var cssClass = _runClassFactory.Build(new RunClassParam
            {
                ParagraphStyleId = parentProps?.ParagraphStyleId?.Val,
                RunStyleId = inlineProps?.Elements<RunStyle>().SingleOrDefault()?.Val,
                InlineProperties = inlineProps,
                NumberingId = numbering?.Item1,
                NumberingLevel = numbering?.Item2
            });

            return AddOrSet(cssClass);
        }

        private CssClass AddOrSet(CssClass cssClass)
        {
            return _registrations.GetOrAdd(cssClass.Props, cssClass);
        }

        public void InsertCss(CssData data)
        {
            foreach (var cls in Registrations)
                cls.InsertCss(data);
        }

    }
}
