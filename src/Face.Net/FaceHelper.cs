using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml;

namespace Faces.Net
{
    public static class FaceHelper
    {
        public static XmlElement NewPath(XmlDocument document, XmlElement paper)
        {
            var e = document.CreateElement("path", "http://www.w3.org/2000/svg");
            paper.AppendChild(e);
            return e;
        }

        public static string GetHtmlColor(Color color) => $"rgba({color.R}, {color.G},{color.B},{color.A})";

        // Scale relative to the center of bounding box of element e, like in Raphael.
        // Set x and y to 1 and this does nothing. Higher = bigger, lower = smaller.
        public static void ScaleCentered(XmlElement e, double x, double y, int width, int height)
        {
            var cx = width / 2;
            var cy = height / 2;
            var tx = (cx * (1 - x)) / x;
            var ty = (cy * (1 - y)) / y;

            e.SetAttribute("transform", "scale(" + x + " " + y + "), translate(" + tx + " " + ty + ")");

            // Keep apparent stroke width constant, similar to how Raphael does it (I think)
            var strokeWidth = e.GetAttribute("stroke-width");
            if (double.TryParse(strokeWidth, out var sw))
            {
                e.SetAttribute("stroke-width", (sw / Math.Abs(x)).ToString());
            }
        }

        // Defines the range of fat/skinny, relative to the original width of the default head.
        public static double FatScale(double fatness) => 0.75 + 0.25 * fatness;

        // Rotate around center of bounding box of element e, like in Raphael.
        public static void RotateCentered(XmlElement e, double angle, int width, int height)
        {
            var cx = width / 2;
            var cy = height / 2;
            e.SetAttribute("transform", "rotate(" + angle + " " + cx + " " + cy + ")");
        }
    }
}
