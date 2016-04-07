using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Nyasa_Wizard
{
    [Serializable()]
    public class MNReferencedMantra
    {
        public string Number { get; set; }
        public string MantraText { get; set; }
        public string TouchedPartText { get; set; }
        public string HandGestureText { get; set; }

        public XmlElement Save(XmlDocument doc)
        {
            XmlElement e = doc.CreateElement("mantra");

            e.SetAttribute("number", Number);
            e.InnerText = MantraText;
            e.SetAttribute("tp", TouchedPartText);
            e.SetAttribute("hg", HandGestureText);
            return e;
        }

        public void Load(XmlElement e)
        {
            Number = e.GetAttribute("number");
            MantraText = e.InnerText;
            TouchedPartText = e.GetAttribute("tp");
            HandGestureText = e.GetAttribute("hg");
        }

    }
}
