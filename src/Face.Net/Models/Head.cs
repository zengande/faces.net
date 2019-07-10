using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml;

namespace Faces.Net.Models
{
    public class Head : FaceBase
    {
        public static int MaxId = 1;

        public override void Render(XmlDocument document, XmlElement paper,int width, int height)
        {
            DefaultHead(document, paper, width, height);
        }

        private void DefaultHead(XmlDocument document, XmlElement paper, int width, int height)
        {
            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M 200,100" +
                           "c 0,0 180,-10 180,200" +
                           "c 0,0 0,210 -180,200" +
                           "c 0,0 -180,10 -180,-200" +
                           "c 0,0 0,-210 180,-200");
            e.SetAttribute("fill", FaceHelper.GetHtmlColor(SkinColor));
            FaceHelper.ScaleCentered(e, FaceHelper.FatScale(Fatness), 1, width, height);
        }
    }
}
