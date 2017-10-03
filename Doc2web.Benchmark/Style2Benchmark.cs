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
    [ClrJob, CoreJob]
    public class Style2Benchmark
    {
        private WordprocessingDocument _wpDoc;
        private StylePlugin _plugin;
        private ContainerBuilder _containerBuilder;
        private IContainer _container;
        private ICssRegistrator2 _registrator;
        private Style[] _styles;
        private ParagraphProperties[] _pProps;
        private RunProperties[] _rProps;

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

            _pProps =
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

            _rProps =
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

            Console.WriteLine($"Paragraph style count: {_pProps.Length}\tRun style count: {_rProps.Length}");
        }

        [IterationSetup]
        public void ResetContainer()
        {
            _containerBuilder = new ContainerBuilder();
            _plugin.InitEngine(_containerBuilder);
            _container = _containerBuilder.Build();
            _registrator = _container.Resolve<ICssRegistrator2>();
        }

        [Benchmark]
        public void RenderAllParagraphStyles()
        {
            for (int i = 0; i < _pProps.Length; i++)
                _registrator.RegisterParagraph(_pProps[i], null);
        }

        [Benchmark]
        public void RenderAllRunStyles()
        {
            for (int i = 0; i < _pProps.Length; i++)
                for (int j = 0; j < _rProps.Length; j++)
                    _registrator.RegisterRun(_pProps[i], _rProps[j], null);
        }
    }
}
