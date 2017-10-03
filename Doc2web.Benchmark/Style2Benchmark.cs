using Autofac;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Css;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Benchmark
{
    [ClrJob]
    public class Style2Benchmark
    {
        private WordprocessingDocument _wpDoc;
        private StylePlugin _plugin;
        private ContainerBuilder _containerBuilder;
        private IContainer _container;
        private ICssRegistrator2 _registrator;
        private Style[] _styles;
        private ParagraphProperties[] _pPropsStyles;
        private ParagraphProperties[] _pPropsInlines;
        private RunProperties[] _rPropsStyles;
        private RunProperties[] _rPropsInlines;

        [GlobalSetup]
        public void Setup()
        {
            _wpDoc = WordprocessingDocument.Open(Utils.GetAssetPath("shareholders.docx"), false);
            _plugin = new StylePlugin(_wpDoc);

            _styles =
                _wpDoc
                .MainDocumentPart
                .StyleDefinitionsPart
                .Styles
                .Elements<Style>()
                .Where(x => x.StyleId?.Value != null)
                .Distinct()
                .ToArray();

            _pPropsStyles =
                _styles
                .Where(x => x.Type?.Value == StyleValues.Paragraph)
                .Select(x => new ParagraphProperties
                {
                    ParagraphStyleId = new ParagraphStyleId()
                    {
                        Val = x.StyleId.Value
                    }
                })
                .ToArray();

            _rPropsStyles =
                _styles
                .Where(x => x.Type?.Value == StyleValues.Character)
                .Select(x => new RunProperties
                {
                    RunStyle = new RunStyle()
                    {
                        Val = x.StyleId.Value
                    }
                })
                .ToArray();

            _pPropsInlines =
                _wpDoc.MainDocumentPart.Document.Body
                .Descendants<ParagraphProperties>()
                .Where(x => x.ChildElements?.Count > 2)
                .Take(100)
                .ToArray();

            _rPropsInlines =
                _wpDoc.MainDocumentPart.Document.Body
                .Descendants<RunProperties>()
                .Where(x => x.ChildElements?.Count > 2)
                .Take(100)
                .ToArray();

            Console.WriteLine($"Paragraph style count: {_pPropsStyles.Length}\tRun style count: {_rPropsStyles.Length}");
        }

        [IterationSetup]
        public void ResetContainer()
        {
            _containerBuilder = new ContainerBuilder();
            _plugin.InitEngine(_containerBuilder);
            _container = _containerBuilder.Build();
            _registrator = _container.Resolve<ICssRegistrator2>();
        }

        //[Benchmark]
        public void RenderAllParagraphStyles()
        {
            for (int i = 0; i < _pPropsStyles.Length; i++)
                _registrator.RegisterParagraph(_pPropsStyles[i], null);
        }

        //[Benchmark]
        public void RenderAllRunStyles()
        {
            for (int i = 0; i < _pPropsStyles.Length; i++)
                for (int j = 0; j < _rPropsStyles.Length; j++)
                    _registrator.RegisterRun(_pPropsStyles[i], _rPropsStyles[j], null);
        }

        //[Benchmark]
        public void Render100ParagraphInlines()
        {
            for (int i = 0; i < _pPropsInlines.Length; i++)
                _registrator.RegisterParagraph(_pPropsInlines[i], null);
        }

        [Benchmark]
        public void Render100RunInlines()
        {
            for (int i = 0; i < _rPropsInlines.Length; i++)
                _registrator.RegisterRun(null, _rPropsInlines[i], null);
        }
    }
}
