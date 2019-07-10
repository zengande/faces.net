using CoreHtmlToImage;
using Faces.Net.Models;
using System;
using System.Drawing;
using System.IO;
using System.Xml;

namespace Faces.Net
{
    /// <summary>
    /// facesjs implemented with c#
    /// </summary>
    public class FaceGenerator
    {
        private readonly Color[] _skinColors = new[] {
            Color.FromArgb(242,214,203),
            Color.FromArgb(221,183,160),
            Color.FromArgb(206,150,125),
            Color.FromArgb(187,135,111),
            Color.FromArgb(170,129,111),
            Color.FromArgb(166,115,88),
            Color.FromArgb(173,100,83),
            Color.FromArgb(116,69,61),
            Color.FromArgb(92,57,55)
        };
        public byte[] Generate(int width = 400, int height = 600)
        {
            var face = InitFace();

            var svg = GenerateSVG(face, width, height);

            var converter = new HtmlConverter();
            var html = $"<div style='width:{width}px;height:{height}px'>{svg.InnerXml}</div>";
            var buffer = converter.FromHtmlString(html, width + 20);
            return buffer;
        }

        private int GetId(int length = 1) => (int)Math.Floor(new Random().NextDouble() * length);

        private XmlDocument GenerateSVG(Face face, int width, int height)
        {
            var doc = new XmlDocument();

            var paper = doc.CreateElement("svg", "http://www.w3.org/2000/svg");
            paper.SetAttribute("version", "1.2");
            paper.SetAttribute("baseProfile", "tiny");
            paper.SetAttribute("width", "100%");
            paper.SetAttribute("height", "100%");
            paper.SetAttribute("viewBox", $"0 0 {width} {height}");
            paper.SetAttribute("preserveAspectRatio", "xMinYMin meet");

            doc.AppendChild(paper);

            face.Head.Render(doc, paper, width, height);
            face.Eyebrows[0].Render(doc, paper, width, height);
            face.Eyebrows[1].Render(doc, paper, width, height);

            face.Eyes[0].Render(doc, paper, width, height);
            face.Eyes[1].Render(doc, paper, width, height);

            face.Nose.Render(doc, paper, width, height);
            face.Mouth.Render(doc, paper, width, height);
            face.Hair.Render(doc, paper, width, height);

            return doc;
        }

        private Face InitFace()
        {
            var random = new Random();
            var face = new Face();



            face.Head.Id = GetId(Head.MaxId);
            face.Head.Fatness = random.NextDouble();
            face.Head.SkinColor = _skinColors[random.Next(0, _skinColors.Length)];

            var ebId = GetId(Eyebrow.MaxId);
            face.Eyebrows[0] = new Eyebrow { Id = ebId, LR = LR.Left, CX = 135, CY = 250 };
            face.Eyebrows[1] = new Eyebrow { Id = ebId, LR = LR.Right, CX = 265, CY = 250 };

            var angle = random.NextDouble() * 50 - 20;
            var eId = GetId(Eye.MaxId);
            face.Eyes[0] = new Eye { Id = eId, LR = LR.Left, CX = 135, CY = 280, Angle = angle };
            face.Eyes[1] = new Eye { Id = eId, LR = LR.Right, CX = 265, CY = 280, Angle = angle };

            var flip = random.NextDouble() > 0.5 ? true : false;
            face.SetNose(GetId(Nose.MaxId), LR.Left, 200d, 330d, random.NextDouble(), flip);

            face.SetMouth(GetId(Mouth.MaxId), 200d, 400d);

            face.Hair.Id = GetId(Hair.MaxId);

            return face;
        }
    }
}