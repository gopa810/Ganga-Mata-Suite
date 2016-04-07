using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.IO;

namespace Nyasa_Wizard
{
    [Serializable()]
    public class MNReferencedImage
    {
        public string Title { get; set; }
        public string FilePath { get; set; }
        public Image ImageData { get; set; }

        public override string ToString()
        {
            return Title;
        }


        public XmlElement Save(XmlDocument doc)
        {
            XmlElement e = doc.CreateElement("image");

            e.SetAttribute("title", Title);
            e.InnerText = FilePath;
            return e;
        }

        public void Load(XmlElement e)
        {
            Title = e.GetAttribute("title");
            FilePath = e.InnerText;
            if (File.Exists(FilePath))
                ImageData = Image.FromFile(FilePath);
        }

    }
}
