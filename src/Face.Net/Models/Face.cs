using Faces.Net.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml;

namespace Faces.Net
{
    public class Face
    {
        public Face()
        {
            Head = new Head();
            Hair = new Hair();
            Eyebrows = new Eyebrow[2];
            Eyes = new Eye[2];
            Nose = new Nose();
            Mouth = new Mouth();
        }

        public Head Head { get; set; }
        public Hair Hair { get; set; }
        public Eyebrow[] Eyebrows { get; set; }
        public Eye[] Eyes { get; set; }
        public Nose Nose { get; set; }
        public Mouth Mouth { get; set; }

        public void SetNose(int id, LR lr, double cx, double cy, double size, bool flip, double? posY = null)
        {
            Nose.Id = id;
            Nose.LR = lr;
            Nose.CX = cx;
            Nose.CY = cy;
            Nose.Size = size;
            Nose.Flip = flip;
            Nose.PosY = posY;
        }

        public void SetMouth(int id, double cx, double cy)
        {
            Mouth.Id = id;
            Mouth.CX = cx;
            Mouth.CY = cy;
        }
    }

    public abstract class FaceBase
    {
        public int Id { get; set; }

        public double Fatness { get; set; }
        public Color SkinColor { get; set; }

        public abstract void Render(XmlDocument document, XmlElement paper, int width, int height);
    }

    public enum LR
    {
        Left,
        Right
    }
}
