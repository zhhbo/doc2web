using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using BenchmarkDotNet.Attributes;
using Doc2web.Core.Rendering.Step1;
using Doc2web.Core.Rendering.Step2;
using Doc2web.Core.Rendering;
using Doc2web.Core.Rendering.Step3;
using BenchmarkDotNet.Attributes.Jobs;

namespace Doc2web.Benchmark
{
    [CoreJob]
    public class RenderingBenchmark
    {
        const int NodeCount = 500;
        const int TagCount = NodeCount * 2;

        private WordprocessingDocument _wpDoc;
        private Body _body;
        private List<HtmlNode> _nodes;
        private StringBuilder _stringBuilder;
        private List<HtmlNode> _tempNodes;
        private ITag[] _tags;
        private StringBuilder _tempStringBuilder;
        private HtmlNode[] _tempNodesAr;
        private ITag[] _tempsTags;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _wpDoc = WordprocessingDocument.Open(Utils.GetAssetPath("transaction-formatted.docx"), false);
            _body = _wpDoc.MainDocumentPart.Document.Body;
            BuildNodes();
            _tags = TagsFactory.Build(_nodes.ToArray());

            _tempNodes = new List<HtmlNode>(NodeCount);
            _tempNodes.AddRange(_nodes.Take(NodeCount).Select(x => x.Clone()));
            _tempNodesAr = _tempNodes.ToArray();

            _tempsTags = new ITag[TagCount];
            Array.Copy(_tags, _tempsTags, TagCount);

            Console.WriteLine($"Total node count: {_nodes.Count}, targeting {NodeCount} for this benchmark");
            Console.WriteLine($"Running at {Math.Round(100 * SizeRatio(), 3)}% of the document");
        }

        private double SizeRatio() => ((double)NodeCount / _nodes.Count);

        [IterationSetup(Target = nameof(FlatternNodes))]
        public void CopyNodes()
        {
            _tempNodes = new List<HtmlNode>(NodeCount);
            _tempNodes.AddRange(_nodes.Take(NodeCount).Select(x => x.Clone()));
        }

        [IterationSetup(Target = nameof(RenderTags))]
        public void ResetStringBuilder()
        {
            var maxI = _tempsTags[_tempsTags.Length - 1].Index + 1;
            _tempStringBuilder = new StringBuilder(maxI);
            for (int i = 1; i < maxI; i++) _tempStringBuilder.Append(i);
        }

        [Benchmark]
        public void FlatternNodes () => FlatternHtmlNodes.Apply(_tempNodes);

        [Benchmark]
        public ITag[] BuildTags() => TagsFactory.Build(_tempNodesAr);

        [Benchmark]
        public void RenderTags() => TagsRenderer.Render(_tempsTags, _tempStringBuilder);

        public void BuildNodes()
        {
            _nodes = new List<HtmlNode>();
            _stringBuilder = new StringBuilder(_body.InnerText.Length);
            var bodyNode = new HtmlNode { Z = 100, Start = 0, End = 0, Tag="body" };
            _nodes.Add(bodyNode);

            foreach(var p in _body.Elements<Paragraph>())
                bodyNode.End = AddParagraphNodes(bodyNode.End, p);
        }

        private int AddParagraphNodes(int i, Paragraph p)
        {
            _nodes.Add(new HtmlNode
            {
                Z = 50,
                Tag = "p",
                Start = i,
                End = i + p.InnerText.Length
            });

            int j = 0;
            foreach (var r in p.Elements<Run>())
                j = AddRunNodes(i, j, r);

            i += p.InnerText.Length;
            _stringBuilder.Append(p.InnerText);
            return i;
        }

        private int AddRunNodes(int i, int j, Run r)
        {
            _nodes.Add(new HtmlNode
            {
                Z = 0,
                Tag = "span",
                Start = i + j,
                End = i + j + r.InnerText.Length
            });

            j += r.InnerText.Length;
            return j;
        }
    }
}
