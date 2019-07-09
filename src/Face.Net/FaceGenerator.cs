using CoreHtmlToImage;
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

            var svg = GenerateSVG(face);

            var converter = new HtmlConverter();
            var html = $"<div style='width:{width}px;height:{height}px'>{svg.InnerXml}</div>";
            var buffer = converter.FromHtmlString(html, width + 20);
            return buffer;
        }

        private int GetId(int length = 1) => (int)Math.Floor(new Random().NextDouble() * length);

        private XmlDocument GenerateSVG(Face face)
        {
            var doc = new XmlDocument();

            var paper = doc.CreateElement("svg", "http://www.w3.org/2000/svg");
            paper.SetAttribute("version", "1.2");
            paper.SetAttribute("baseProfile", "tiny");
            paper.SetAttribute("width", "100%");
            paper.SetAttribute("height", "100%");
            paper.SetAttribute("viewBox", $"0 0 {Width} {Height}");
            paper.SetAttribute("preserveAspectRatio", "xMinYMin meet");

            doc.AppendChild(paper);

            Head[face.Head.Id](doc, paper, face.Fatness, face.SkinColor);
            Eyebrow[face.Eyebrows[0].Id](doc, paper, face.Eyebrows[0].LR, face.Eyebrows[0].CX, face.Eyebrows[0].CY);
            Eyebrow[face.Eyebrows[1].Id](doc, paper, face.Eyebrows[1].LR, face.Eyebrows[1].CX, face.Eyebrows[1].CY);

            Eye[face.Eyes[0].Id](doc, paper, face.Eyes[0].LR, face.Eyes[0].CX, face.Eyes[0].CY, face.Eyes[0].Angle);
            Eye[face.Eyes[1].Id](doc, paper, face.Eyes[1].LR, face.Eyes[1].CX, face.Eyes[1].CY, face.Eyes[1].Angle);

            Nose[face.Nose.Id](doc, paper, face.Nose.CX, face.Nose.CY, face.Nose.Size, face.Nose.PosY, face.Nose.Flip);
            Mouth[face.Mouth.Id](doc, paper, face.Mouth.CX, face.Mouth.CY);
            Hair[face.Hair.Id](doc, paper, face.Fatness);

            return doc;
        }

        private Face InitFace()
        {
            var random = new Random();
            var face = new Face();

            face.Fatness = random.NextDouble();
            face.SkinColor = _skinColors[random.Next(0, _skinColors.Length)];

            face.Head.Id = GetId(Head.Length);

            var ebId = GetId(Eyebrow.Length);
            face.Eyebrows[0] = new Eyebrow { Id = ebId, LR = LR.Left, CX = 135, CY = 250 };
            face.Eyebrows[1] = new Eyebrow { Id = ebId, LR = LR.Right, CX = 265, CY = 250 };

            var angle = random.NextDouble() * 50 - 20;
            var eId = GetId(Eye.Length);
            face.Eyes[0] = new Eye { Id = eId, LR = LR.Left, CX = 135, CY = 280, Angle = angle };
            face.Eyes[1] = new Eye { Id = eId, LR = LR.Right, CX = 265, CY = 280, Angle = angle };

            var flip = random.NextDouble() > 0.5 ? true : false;
            face.SetNose(GetId(Nose.Length), LR.Left, 200d, 330d, random.NextDouble(), flip);

            face.SetMouth(GetId(Mouth.Length), 200d, 400d);

            face.Hair.Id = GetId(Hair.Length);

            return face;
        }

        private readonly Action<XmlDocument, XmlElement, double, Color>[] Head = new Action<XmlDocument, XmlElement, double, Color>[] {
            (document,paper, fatness, color) => {
                var e = NewPath(document,paper);
                e.SetAttribute("d", "M 200,100" +
                               "c 0,0 180,-10 180,200" +
                               "c 0,0 0,210 -180,200" +
                               "c 0,0 -180,10 -180,-200" +
                               "c 0,0 0,-210 180,-200");
                e.SetAttribute("fill", GetHtmlColor(color));
                ScaleCentered(e, FatScale(fatness), 1);
            }
        };

        private readonly Action<XmlDocument, XmlElement, LR, double, double>[] Eyebrow = new Action<XmlDocument, XmlElement, LR, double, double>[] {
            (document,paper, lr, cx, cy) => {
                var x = cx - 30;
                var y = cy;

                var e = NewPath(document,paper);
                if (lr == LR.Left) {
                    e.SetAttribute("d", "M " + x + "," + y +
                                   "c 0,0 -3,-30 60,0");
                } else {
                    e.SetAttribute("d", "M " + x + "," + y +
                                   "c 0,0 63,-30 60,0");
                }
                e.SetAttribute("stroke", "#000");
                e.SetAttribute("stroke-width", "8");
                e.SetAttribute("fill", "none");
            }
        };

        private readonly Action<XmlDocument, XmlElement, LR, double, double, double>[] Eye = new Action<XmlDocument, XmlElement, LR, double, double, double>[] {
            (document,paper, lr, cx, cy, angle) => {
                // Horizontal
                var x = cx - 30;
                var y = cy;

                var e = NewPath(document,paper);
                e.SetAttribute("d", "M " + x + "," + y +
                               "h 60");
                e.SetAttribute("stroke", "#000");
                e.SetAttribute("stroke-width", "8");
                e.SetAttribute("fill", "none");
                RotateCentered(e, (lr == LR.Left ? angle : -angle));
            },
            (document,paper, lr, cx, cy, angle) => {
                // Normal (circle with a dot in it)
                var x = cx;
                var y = cy + 20;

                var e = NewPath(document,paper);
                e.SetAttribute("d", "M " + x + "," + y +
                              "a 30,20 0 1 1 0.1,0");
                e.SetAttribute("stroke", "#000");
                e.SetAttribute("stroke-width", "6");
                e.SetAttribute("fill", "#f0f0f0");
                RotateCentered(e, (lr == LR.Left ? angle : -angle));

                var e2 = NewPath(document,paper);
                e2.SetAttribute("d", "M " + x + "," + (y - 12) +
                               "a 12,8 0 1 1 0.1,0");
                RotateCentered(e2, (lr == LR.Left ? angle : -angle));
            },
            (document,paper, lr, cx, cy, angle) => {
                // Dot
                var x = cx;
                var y = cy + 13;

                var e = NewPath(document,paper);
                e.SetAttribute("d", "M " + x + "," + y +
                               "a 20,15 0 1 1 0.1,0");
                RotateCentered(e, (lr == LR.Left ? angle : -angle));
            },
            (document,paper, lr, cx, cy, angle) => {
                // Arc eyelid
                var x = cx;
                var y = cy + 20;

                var e = NewPath(document,paper);
                e.SetAttribute("d", "M " + x + "," + y +
                               "a 17,17 0 1 1 0.1,0 z");
                RotateCentered(e, (lr == LR.Left ? angle : -angle));

                var e2 = NewPath(document,paper);
                e2.SetAttribute("d", "M " + (x - 40) + "," + (y - 14) +
                               "c 36,-44 87,-4 87,-4");
                e2.SetAttribute("stroke", "#000");
                e2.SetAttribute("stroke-width", "4");
                e2.SetAttribute("fill", "none");
                RotateCentered(e, (lr == LR.Left ? angle : -angle));
            }
        };

        private readonly Action<XmlDocument, XmlElement, double, double, double, double?, bool>[] Nose = new Action<XmlDocument, XmlElement, double, double, double, double?, bool>[]
        {
            (document,paper, cx, cy, size, posY, flip) => {
                // Pinnochio
                var scale = size + 0.5;
                var x = cx;
                var y = cy - 10;

                var e = NewPath(document,paper);
                e.SetAttribute("d", "M " + (flip ? x - 48 : x) + "," + y +
                               "c 0,0 50,-30 0,30");
                e.SetAttribute("stroke", "#000");
                e.SetAttribute("stroke-width", "8");
                e.SetAttribute("fill", "none");
                if (flip) {
                    ScaleCentered(e, -scale, scale);
                } else {
                    ScaleCentered(e, scale, scale);
                }
            },
            (document,paper, cx, cy, size, posY, flip) => {
                // Big single
                var scale = size + 0.5;
                var x = cx - 9;
                var y = cy - 25;

                var e = NewPath(document,paper);
                e.SetAttribute("d", "M " + x + "," + y +
                               "c 0,0 -20,60 9,55" +
                               "c 0,0 29,5 9,-55");
                e.SetAttribute("stroke", "#000");
                e.SetAttribute("stroke-width", "8");
                e.SetAttribute("fill", "none");
                ScaleCentered(e, scale, scale);
            }
        };

        private readonly Action<XmlDocument, XmlElement, double, double>[] Mouth = new Action<XmlDocument, XmlElement, double, double>[]
        {
            (document,paper, cx, cy) => {
                // Thin smile
                var x = cx - 75;
                var y = cy - 15;

                var e = NewPath(document,paper);
                e.SetAttribute("d", "M " + x + "," + y +
                               "c 0,0 75,60 150,0");
                e.SetAttribute("stroke", "#000");
                e.SetAttribute("stroke-width", "8");
                e.SetAttribute("fill", "none");
            },
            (document,paper, cx, cy) => {
                // Thin flat
                var x = cx - 55;
                var y = cy;

                var e = NewPath(document,paper);
                e.SetAttribute("d", "M " + x + "," + y +
                               "h 110");
                e.SetAttribute("stroke", "#000");
                e.SetAttribute("stroke-width", "8");
                e.SetAttribute("fill", "none");
            },
            (document,paper, cx, cy) => {
                // Open-mouthed smile, top teeth
                var x = cx - 75;
                var y = cy - 15;

                var e = NewPath(document,paper);
                e.SetAttribute("d", "M " + x + "," + y +
                               "c 0,0 75,100 150,0" +
                               "h -150");

                var e2 = NewPath(document,paper);
                e2.SetAttribute("d", "M " + (x + 16) + "," + (y + 8) +
                               "l 16,16" +
                               "h 86" +
                               "l 16,-16" +
                               "h -118");
                e2.SetAttribute("fill", "#f0f0f0");
            },
            (document,paper, cx, cy) => {
                // Generic open mouth
                var x = cx - 55;
                var y = cy;

                var e = NewPath(document,paper);
                e.SetAttribute("d", "M " + x + "," + y +
                               "a 54,10 0 1 1 110,0" +
                               "a 54,20 0 1 1 -110,0");
            },
            (document,paper, cx, cy) => {
                // Thin smile with ends
                var x = cx - 75;
                var y = cy - 15;

                var e = NewPath(document,paper);
                e.SetAttribute("d", "M " + x + "," + y +
                               "c 0,0 75,60 150,0");
                e.SetAttribute("stroke", "#000");
                e.SetAttribute("stroke-width", "8");
                e.SetAttribute("fill", "none");

                var e2 = NewPath(document,paper);
                e2.SetAttribute("d", "M " + (x + 145) + "," + (y + 19) +
                               "c 15.15229,-18.18274 3.03046,-32.32488 3.03046,-32.32488");
                e2.SetAttribute("stroke", "#000");
                e2.SetAttribute("stroke-width", "8");
                e2.SetAttribute("fill", "none");

                var e3 = NewPath(document,paper);
                e3.SetAttribute("d", "M " + (x + 5) + "," + (y + 19) +
                               "c -15.15229,-18.18274 -3.03046,-32.32488 -3.03046,-32.32488");
                e3.SetAttribute("stroke", "#000");
                e3.SetAttribute("stroke-width", "8");
                e3.SetAttribute("fill", "none");
            }
        };

        private readonly Action<XmlDocument, XmlElement, double>[] Hair = new Action<XmlDocument, XmlElement, double>[]
        {
            (document, paper, fatness) => {
                // Normal short
                var e = NewPath(document,paper);
                e.SetAttribute("d", "M 200,100" +
                               "c 0,0 180,-10 176,150" +
                               "c 0,0 -180,-150 -352,0" +
                               "c 0,0 0,-160 176,-150");
                ScaleCentered(e, FatScale(fatness), 1);
            },
            (document,paper, fatness) => {
                // Flat top
                var e = NewPath(document,paper);
                e.SetAttribute("d", "M 25,60" +
                               "h 352" +
                               "v 190" +
                               "c 0,0 -180,-150 -352,0" +
                               "v -190");
                ScaleCentered(e, FatScale(fatness), 1);
            },
            (document,paper, fatness) => {
                // Afro
                var e = NewPath(document,paper);
                e.SetAttribute("d", "M 25,250" +
                               "a 210,150 0 1 1 352,0" +
                               "c 0,0 -180,-150 -352,0");
                ScaleCentered(e, FatScale(fatness), 1);
            },
            (document,paper, fatness) => {
                // Cornrows
                var e = NewPath(document,paper);
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
                ScaleCentered(e, FatScale(fatness), 1);
            },
            (_,__, ___) => {
                // Intentionally left blank (bald)
            }
        };


        private static double Width = 400;
        private static double Height = 600;

        private static XmlElement NewPath(XmlDocument document, XmlElement paper)
        {
            var e = document.CreateElement("path", "http://www.w3.org/2000/svg");
            paper.AppendChild(e);
            return e;
        }

        private static string GetHtmlColor(Color color) => $"rgba({color.R}, {color.G},{color.B},{color.A})";

        // Scale relative to the center of bounding box of element e, like in Raphael.
        // Set x and y to 1 and this does nothing. Higher = bigger, lower = smaller.
        private static void ScaleCentered(XmlElement e, double x, double y)
        {
            var cx = Width / 2;
            var cy = Height / 2;
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
        private static double FatScale(double fatness) => 0.75 + 0.25 * fatness;

        // Rotate around center of bounding box of element e, like in Raphael.
        private static void RotateCentered(XmlElement e, double angle)
        {
            var cx = Width / 2;
            var cy = Height / 2;
            e.SetAttribute("transform", "rotate(" + angle + " " + cx + " " + cy + ")");
        }
    }
}