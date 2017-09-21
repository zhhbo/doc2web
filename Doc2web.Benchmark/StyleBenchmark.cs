using Autofac;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using Doc2web.Plugins.Style;
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
    public class StyleBenchmark
    {
        private WordprocessingDocument _wpDoc;
        private StyleProcessorPlugin _plugin;
        private ContainerBuilder _containerBuilder;
        private IContainer _container;
        private ILifetimeScope _lts;
        private ParagraphProperties[] _paragraphProperties;
        private RunProperties[] _runProperties;
        private Task[] _tasks;

        private Body Body => _wpDoc.MainDocumentPart.Document.Body;
        private ICssRegistrator Registrator => _container.Resolve<ICssRegistrator>();

        [GlobalSetup]
        public void Setup()
        {
            _wpDoc = WordprocessingDocument.Open(Utils.GetAssetPath("transaction-formatted.docx"), false);
            _plugin = new StyleProcessorPlugin(_wpDoc);
            _containerBuilder = new ContainerBuilder();
            _plugin.InitEngine(_containerBuilder);
            _plugin.InitProcessing(_containerBuilder);
            _container = _containerBuilder.Build();
        }

        [IterationSetup(Target = nameof(RegisterDynamics))]
        public void SetupDynamic()
        {
            _lts = _container.BeginLifetimeScope();
            _paragraphProperties = Body.Descendants<ParagraphProperties>().ToArray();
            _runProperties = Body.Descendants<RunProperties>().ToArray();
        }

        [Benchmark]
        public void RegisterDynamics()
        {
            var t1 =
                _paragraphProperties
                .Select(x => Task.Factory.StartNew(() => Registrator.RegisterParagraphProperties(x)))
                .ToArray();

            var t2 =
                _runProperties
                .Select(x => Task.Factory.StartNew(() => Registrator.RegisterRunProperties(x)))
                .ToArray();

            Task.WaitAll(t1);
            Task.WaitAll(t2);
        }

        [GlobalSetup(Target = nameof(RenderStyles))]
        public void AddAllStyles ()
        {
            var styles =
                _wpDoc
                .MainDocumentPart
                .StyleDefinitionsPart
                .Styles
                .Elements<DocumentFormat.OpenXml.Wordprocessing.Style>()
                .Where(x => x.StyleId?.Value != null)
                .Select(x => x.StyleId.Value)
                .Distinct()
                .ToArray();

            for (int i = 0; i < styles.Length; i++)
                Registrator.RegisterStyle(styles[i]);

            (Registrator as CssRegistrator).RenderInto(new StringBuilder()); // make sure everything is cached...
        }

        [Benchmark]
        public void RenderStyles()
        {
            (Registrator as CssRegistrator).RenderInto(new StringBuilder());
        }
    }
}
