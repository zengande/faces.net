using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Faces.Net.Models
{
    public class Mouth : FaceBase
    {
        public static int MaxId = 5;

        public double CX { get; set; }
        public double CY { get; set; }

        public override void Render(XmlDocument document, XmlElement paper,int width, int height)
        {
            switch (Id)
            {
                case 1:
                    ThinFlatMouth(document, paper);
                    break;
                case 2:
                    OpenMouthedSmileMouth(document, paper);
                    break;
                case 3:
                    GenericOpenMouth(document, paper);
                    break;
                case 4:
                    ThinSmileWithEndsMouth(document, paper);
                    break;
                default:
                    ThinSmileMouth(document, paper);
                    break;
            }
        }

        /// <summary>
        /// Thin smile
        /// </summary>
        /// <param name="document"></param>
        /// <param name="paper"></param>
        /// <param name="CX"></param>
        /// <param name="CY"></param>
        private void ThinSmileMouth(XmlDocument document, XmlElement paper)
        {
            var x = CX - 75;
            var y = CY - 15;

            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M " + x + "," + y +
                           "c 0,0 75,60 150,0");
            e.SetAttribute("stroke", "#000");
            e.SetAttribute("stroke-width", "8");
            e.SetAttribute("fill", "none");
        }

        /// <summary>
        /// Thin flat
        /// </summary>
        /// <param name="document"></param>
        /// <param name="paper"></param>
        /// <param name="CX"></param>
        /// <param name="CY"></param>
        private void ThinFlatMouth(XmlDocument document, XmlElement paper)
        {
            var x = CX - 55;
            var y = CY;

            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M " + x + "," + y +
                           "h 110");
            e.SetAttribute("stroke", "#000");
            e.SetAttribute("stroke-width", "8");
            e.SetAttribute("fill", "none");
        }

        /// <summary>
        /// Open-mouthed smile, top teeth
        /// </summary>
        /// <param name="document"></param>
        /// <param name="paper"></param>
        /// <param name="CX"></param>
        /// <param name="CY"></param>
        private void OpenMouthedSmileMouth(XmlDocument document, XmlElement paper)
        {
            var x = CX - 75;
            var y = CY - 15;

            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M " + x + "," + y +
                           "c 0,0 75,100 150,0" +
                           "h -150");

            var e2 = FaceHelper.NewPath(document, paper);
            e2.SetAttribute("d", "M " + (x + 16) + "," + (y + 8) +
                           "l 16,16" +
                           "h 86" +
                           "l 16,-16" +
                           "h -118");
            e2.SetAttribute("fill", "#f0f0f0");
        }

        /// <summary>
        /// Generic open mouth
        /// </summary>
        /// <param name="document"></param>
        /// <param name="paper"></param>
        /// <param name="CX"></param>
        /// <param name="CY"></param>
        private void GenericOpenMouth(XmlDocument document, XmlElement paper)
        {
            var x = CX - 55;
            var y = CY;

            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M " + x + "," + y +
                           "a 54,10 0 1 1 110,0" +
                           "a 54,20 0 1 1 -110,0");
        }

        /// <summary>
        /// Thin smile with ends
        /// </summary>
        /// <param name="document"></param>
        /// <param name="paper"></param>
        /// <param name="CX"></param>
        /// <param name="CY"></param>
        private void ThinSmileWithEndsMouth(XmlDocument document, XmlElement paper)
        {
            var x = CX - 75;
            var y = CY - 15;

            var e = FaceHelper.NewPath(document, paper);
            e.SetAttribute("d", "M " + x + "," + y +
                           "c 0,0 75,60 150,0");
            e.SetAttribute("stroke", "#000");
            e.SetAttribute("stroke-width", "8");
            e.SetAttribute("fill", "none");

            var e2 = FaceHelper.NewPath(document, paper);
            e2.SetAttribute("d", "M " + (x + 145) + "," + (y + 19) +
                           "c 15.15229,-18.18274 3.03046,-32.32488 3.03046,-32.32488");
            e2.SetAttribute("stroke", "#000");
            e2.SetAttribute("stroke-width", "8");
            e2.SetAttribute("fill", "none");

            var e3 = FaceHelper.NewPath(document, paper);
            e3.SetAttribute("d", "M " + (x + 5) + "," + (y + 19) +
                           "c -15.15229,-18.18274 -3.03046,-32.32488 -3.03046,-32.32488");
            e3.SetAttribute("stroke", "#000");
            e3.SetAttribute("stroke-width", "8");
            e3.SetAttribute("fill", "none");
        }
    }
}
