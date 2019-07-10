using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Faces.Net.Models
{
    public class Nose : FaceBase
    {
        public static int MaxId = 2;
        
        public double CX { get; set; }
        public double CY { get; set; }
        public LR LR { get; set; }
        public double Size { get; set; }
        public double? PosY { get; set; }
        public bool Flip { get; set; }

        public override void Render(XmlDocument document, XmlElement paper,int width, int height)
        {
            if (Id == 1)
            {
                BigSingleNose(document, paper, width, height);
            }
            else
            {
                PinnochioNose(document, paper, width, height);
            }
        }

        /// <summary>
        /// Pinnochio
        /// </summary>
        /// <param name="document"></param>
        /// <param name="paper"></param>
        private void PinnochioNose(XmlDocument document, XmlElement paper, int width, int height)
        {
            var scale = Size + 0.5;
            var x = CX;
            var y = CY - 10;

            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M " + (Flip ? x - 48 : x) + "," + y +
                           "c 0,0 50,-30 0,30");
            e.SetAttribute("stroke", "#000");
            e.SetAttribute("stroke-width", "8");
            e.SetAttribute("fill", "none");
            if (Flip)
            {
                FaceHelper.ScaleCentered(e, -scale, scale, width, height);
            }
            else
            {
                FaceHelper.ScaleCentered(e, scale, scale, width, height);
            }
        }

        /// <summary>
        /// Big single
        /// </summary>
        /// <param name="document"></param>
        /// <param name="paper"></param>
        /// <param name="CX"></param>
        /// <param name="CY"></param>
        /// <param name="Size"></param>
        /// <param name="Flip"></param>
        private void BigSingleNose(XmlDocument document, XmlElement paper, int width, int height)
        {
            var scale = Size + 0.5;
            var x = CX - 9;
            var y = CY - 25;

            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M " + x + "," + y +
                           "c 0,0 -20,60 9,55" +
                           "c 0,0 29,5 9,-55");
            e.SetAttribute("stroke", "#000");
            e.SetAttribute("stroke-width", "8");
            e.SetAttribute("fill", "none");
            FaceHelper.ScaleCentered(e, scale, scale, width, height);
        }
    }
}
