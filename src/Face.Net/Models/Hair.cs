using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Faces.Net.Models
{
    public class Hair : FaceBase
    {
        public static int MaxId = 5;

        public override void Render(XmlDocument document, XmlElement paper, int width, int height)
        {
            switch (Id)
            {
                case 1:
                    NormalShortHair(document, paper, width, height);
                    break;
                case 2:
                    FlatTopHair(document, paper, width, height);
                    break;
                case 3:
                    AfroHair(document, paper, width, height);
                    break;
                case 4:
                    CornrowsHair(document, paper, width, height);
                    break;
                default:
                    // Intentionally left blank (bald)
                    break;
            }
        }

        private void NormalShortHair(XmlDocument document, XmlElement paper, int width, int height)
        {
            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M 200,100" +
                           "c 0,0 180,-10 176,150" +
                           "c 0,0 -180,-150 -352,0" +
                           "c 0,0 0,-160 176,-150");
            FaceHelper.ScaleCentered(e, FaceHelper.FatScale(Fatness), 1, width, height);
        }
        private void FlatTopHair(XmlDocument document, XmlElement paper, int width, int height)
        {
            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M 25,60" +
                           "h 352" +
                           "v 190" +
                           "c 0,0 -180,-150 -352,0" +
                           "v -190");
            FaceHelper.ScaleCentered(e, FaceHelper.FatScale(Fatness), 1, width, height);
        }
        private void AfroHair(XmlDocument document, XmlElement paper, int width, int height)
        {
            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M 25,250" +
                           "a 210,150 0 1 1 352,0" +
                           "c 0,0 -180,-150 -352,0");
            FaceHelper.ScaleCentered(e, FaceHelper.FatScale(Fatness), 1, width, height);
        }
        private void CornrowsHair(XmlDocument document, XmlElement paper, int width, int height)
        {
            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M 36,229" +
                           "v -10" +
                           "m 40,-10" +
                           "v -60" +
                           "m 50,37" +
                           "v -75" +
                           "m 50,65" +
                           "v -76" +
                           "m 50,76" +
                           "v -76" +
                           "m 50,93" +
                           "v -75" +
                           "m 50,92" +
                           "v -60" +
                           "m 40,80" +
                           "v -10");
            e.SetAttribute("stroke", "#000");
            e.SetAttribute("stroke-linecap", "round");
            e.SetAttribute("stroke-width", "22");
            FaceHelper.ScaleCentered(e, FaceHelper.FatScale(Fatness), 1, width, height);
        }
    }
}
