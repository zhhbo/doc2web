using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Concurrent;
using System.Linq;

namespace Doc2web.Plugins.Style.Css
{
    public class CssRegistrator2 : ICssRegistrator2, IEqualityComparer<CssClass2>
    {
        private IParagraphClassFactory _paragraphClassFactory;
        private IRunClassFactory _runClassFactory;
        private ConcurrentDictionary<CssClass2, CssClass2> _registrations;

        public CssRegistrator2(
            IParagraphClassFactory paragraphClassFactory,
            IRunClassFactory runClassFactory)
        {
            _paragraphClassFactory = paragraphClassFactory;
            _runClassFactory = runClassFactory;
            _registrations = new ConcurrentDictionary<CssClass2, CssClass2>(this);
        }

        public IEnumerable<CssClass2> Registrations => _registrations.Keys;

        public bool Equals(CssClass2 x, CssClass2 y) =>
            x.Props.SetEquals(y.Props);

        public int GetHashCode(CssClass2 obj) =>
            obj.Props.GetHashCode();

        public CssClass2 RegisterParagraph(
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

        public CssClass2 RegisterRun(
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


        private CssClass2 AddOrSet(CssClass2 cssClass)
        {
            if (_registrations.TryAdd(cssClass, cssClass))
                return cssClass;
            else
                return _registrations[cssClass];
        }
        public void InsertCss(CssData data)
        {
            foreach (var cls in Registrations)
                cls.InsertCss(data);
        }

    }
}
