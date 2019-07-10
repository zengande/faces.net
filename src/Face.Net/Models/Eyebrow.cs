using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Faces.Net.Models
{
    public class Eyebrow : FaceBase
    {
        public static int MaxId = 1;

        public double CX { get; set; }
        public double CY { get; set; }
        public LR LR { get; set; }

        public override void Render(XmlDocument document, XmlElement paper,int width, int height)
            => DefaultEyebrow(document, paper);
        
        private void DefaultEyebrow(XmlDocument document, XmlElement paper)
        {
            var x = CX - 30;
            var y = CY;

            var e = FaceHelper.NewPath(document, paper);
            if (LR == LR.Left)
            {
                e.SetAttribute("d", "M " + x + "," + y +
                               "c 0,0 -3,-30 60,0");
            }
            else
            {
                e.SetAttribute("d", "M " + x + "," + y +
                               "c 0,0 63,-30 60,0");
            }
            e.SetAttribute("stroke", "#000");
            e.SetAttribute("stroke-width", "8");
            e.SetAttribute("fill", "none");
        }
    }
}
