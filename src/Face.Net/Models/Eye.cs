using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Faces.Net.Models
{
    public class Eye : FaceBase
    {
        public static int MaxId = 4;

        public double CX { get; set; }
        public double CY { get; set; }
        public LR LR { get; set; }
        public double Angle { get; set; }

        public override void Render(XmlDocument document, XmlElement paper, int width, int height)
        {
            switch (Id)
            {
                case 1:
                    {
                        NormalEye(document, paper, width, height);
                        break;
                    }
                case 2:
                    {
                        DotEye(document, paper, width, height);
                        break;
                    }
                case 3:
                    {
                        ArcEyelidEye(document, paper, width, height);
                        break;
                    }
                default:
                    {
                        HorizontalEye(document, paper, width, height);
                        break;
                    }
            }
        }

        /// <summary>
        /// Horizontal
        /// </summary>
        /// <param name="document"></param>
        /// <param name="paper"></param>
        private void HorizontalEye(XmlDocument document, XmlElement paper, int width, int height)
        {
            var x = CX - 30;
            var y = CY;

            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M " + x + "," + y +
                           "h 60");
            e.SetAttribute("stroke", "#000");
            e.SetAttribute("stroke-width", "8");
            e.SetAttribute("fill", "none");
            FaceHelper.RotateCentered(e, (LR == LR.Left ? Angle : -Angle), width, height);
        }

        /// <summary>
        /// Normal (circle with a dot in it)
        /// </summary>
        /// <param name="document"></param>
        /// <param name="paper"></param>
        private void NormalEye(XmlDocument document, XmlElement paper, int width, int height)
        {
            var x = CX;
            var y = CY + 20;

            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M " + x + "," + y +
                          "a 30,20 0 1 1 0.1,0");
            e.SetAttribute("stroke", "#000");
            e.SetAttribute("stroke-width", "6");
            e.SetAttribute("fill", "#f0f0f0");
            FaceHelper.RotateCentered(e, (LR == LR.Left ? Angle : -Angle), width, height);

            var e2 = FaceHelper.NewPath(document, paper);
            e2.SetAttribute("d", "M " + x + "," + (y - 12) +
                           "a 12,8 0 1 1 0.1,0");
            FaceHelper.RotateCentered(e2, (LR == LR.Left ? Angle : -Angle), width, height);
        }

        /// <summary>
        /// Dot
        /// </summary>
        /// <param name="document"></param>
        /// <param name="paper"></param>
        private void DotEye(XmlDocument document, XmlElement paper, int width, int height)
        {
            var x = CX;
            var y = CY + 13;

            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M " + x + "," + y +
                           "a 20,15 0 1 1 0.1,0");
            FaceHelper.RotateCentered(e, (LR == LR.Left ? Angle : -Angle), width, height);
        }

        /// <summary>
        /// Arc eyelid
        /// </summary>
        /// <param name="document"></param>
        /// <param name="paper"></param>
        private void ArcEyelidEye(XmlDocument document, XmlElement paper, int width, int height)
        {
            var x = CX;
            var y = CY + 20;

            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M " + x + "," + y +
                           "a 17,17 0 1 1 0.1,0 z");
            FaceHelper.RotateCentered(e, (LR == LR.Left ? Angle : -Angle), width, height);

            var e2 = FaceHelper.NewPath(document, paper);
            e2.SetAttribute("d", "M " + (x - 40) + "," + (y - 14) +
                           "c 36,-44 87,-4 87,-4");
            e2.SetAttribute("stroke", "#000");
            e2.SetAttribute("stroke-width", "4");
            e2.SetAttribute("fill", "none");
            FaceHelper.RotateCentered(e, (LR == LR.Left ? Angle : -Angle), width, height);
        }
    }
}
