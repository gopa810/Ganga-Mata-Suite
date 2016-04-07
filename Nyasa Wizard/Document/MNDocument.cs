using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Xml;

namespace Nyasa_Wizard
{
    [Serializable()]
    public class MNDocument
    {
        public List<MNReferencedImage> Images = new List<MNReferencedImage>();
        public List<MNReferencedMantra> Mantras = new List<MNReferencedMantra>();
        public List<MNPage> Pages = new List<MNPage>();

        public static readonly int PO_PORTAIT = 0;
        public static readonly int PO_LANDSCAPE = 1;
        public static readonly int PS_A4 = 0;
        public static readonly int PS_LETTER = 1;

        public static int DotPerMM = 12;

        public string Title { get; set; }
        public int PageOrientation { get; set; }
        public int PageSize { get; set; }

        public float DocumentHeaderFontSize
        {
            get
            {
                return DocumentHeaderSize;
            }
            set
            {
                DocumentHeaderSize = value;
                DocumentHeaderFont = MNDocument.FontOfSize(DocumentHeaderSize);
            }
        }

        public float PageHeaderFontSize
        {
            get
            {
                return PageHeaderSize;
            }
            set
            {
                PageHeaderSize = value;
                PageHeaderFont = MNDocument.FontOfSize(PageHeaderSize);
            }
        }

        public float LabelFontSize
        {
            get
            {
                return LabelSize;
            }
            set
            {
                LabelSize = value;
                LabelFont = FontOfSize(LabelSize);
            }
        }

        public float SubLabelFontSize
        {
            get
            {
                return SubLabelSize;
            }
            set
            {
                SubLabelSize = value;
                SubLabelFont = FontOfSize(SubLabelSize);
            }
        }


        public float DocumentHeaderSize;
        public float PageHeaderSize;
        public float LabelSize = 60;
        public float SubLabelSize = 30;

        public Font DocumentHeaderFont { get; set; }
        public Font PageHeaderFont { get; set; }
        public Font LabelFont { get; set; }
        public Font SubLabelFont { get; set; }

        public static FontFamily TextFontFamily { get; set; }

        public MNDocument()
        {
            PageOrientation = PO_LANDSCAPE;
            PageSize = PS_A4;

            CreateNewPage();

            if (TextFontFamily == null)
                TextFontFamily = new FontFamily("Arial");

            DocumentHeaderFontSize = 40;
            PageHeaderFontSize = 30;
            LabelFontSize = 60;
            SubLabelFontSize = 30;
        }

        private static Dictionary<float, Font> fontsBank = new Dictionary<float, Font>();

        public static Font FontOfSize(float size)
        {
            if (fontsBank.ContainsKey(size))
                return fontsBank[size];

            Font f = new Font(TextFontFamily, size);
            fontsBank.Add(size, f);
            return f;
        }

        /// <summary>
        /// Creates new page
        /// </summary>
        /// <returns></returns>
        public MNPage CreateNewPage()
        {
            MNPage page = new MNPage(this);
            page.Title = string.Format("<Page Title>");
            page.PageIndex = Pages.Count + 1;
            page.Document = this;
            Pages.Add(page);

            return page;
        }

        public MNPage InsertPageAfter(MNPage cp)
        {
            int index = Pages.IndexOf(cp);

            MNPage page = new MNPage(this);
            page.Title = string.Format("<Page Title>");
            page.Document = this;
            if (index >= 0)
            {
                Pages.Insert(index + 1, page);
            }
            else
            {
                Pages.Add(page);
            }

            RecalculatePageIndices();

            return page;
        }

        private void RecalculatePageIndices()
        {
            for (int i = 0; i < Pages.Count; i++)
            {
                Pages[i].PageIndex = i + 1;
            }
        }

        public void Save(string CurrentFileName)
        {
            XmlDocument doc = new XmlDocument();
            Save(doc);
            doc.Save(CurrentFileName);
        }

        public static MNDocument Load(string fileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                MNDocument document = new MNDocument();
                document.Load(doc);
                return document;
            }
            catch
            {
                return null;
            }

        }

        public bool HasContent()
        {
            if (Images.Count > 0)
                return true;

            int count = 0;
            foreach (MNPage page in Pages)
            {
                count += page.Objects.Count;
            }

            if (count > 0)
                return true;

            return false;
        }

        public int PageWidth
        {
            get
            {
                if (PageOrientation == PO_LANDSCAPE)
                {
                    if (PageSize == PS_A4)
                    {
                        return DotPerMM * 297;
                    }
                    else if (PageSize == PS_LETTER)
                    {
                        return DotPerMM * 279;
                    }
                }
                else if (PageOrientation == PO_PORTAIT)
                {
                    if (PageSize == PS_A4)
                    {
                        return DotPerMM * 210;
                    }
                    else if (PageSize == PS_LETTER)
                    {
                        return DotPerMM * 216;
                    }
                }

                return 200 * DotPerMM;
            }
        }

        public int PageHeight
        {
            get
            {
                if (PageOrientation == PO_PORTAIT)
                {
                    if (PageSize == PS_A4)
                    {
                        return DotPerMM * 297;
                    }
                    else if (PageSize == PS_LETTER)
                    {
                        return DotPerMM * 279;
                    }
                }
                else if (PageOrientation == PO_LANDSCAPE)
                {
                    if (PageSize == PS_A4)
                    {
                        return DotPerMM * 210;
                    }
                    else if (PageSize == PS_LETTER)
                    {
                        return DotPerMM * 216;
                    }
                }

                return 200 * DotPerMM;
            }
        }


        public XmlElement Save(XmlDocument doc)
        {
            XmlElement e1 = doc.CreateElement("gms");
            doc.AppendChild(e1);

            foreach (MNReferencedImage ri in Images)
            {
                e1.AppendChild(ri.Save(doc));
            }

            foreach (MNReferencedMantra rm in Mantras)
            {
                e1.AppendChild(rm.Save(doc));
            }

            foreach (MNPage p in Pages)
            {
                e1.AppendChild(p.Save(doc));
            }


            e1.AppendChild(SetXmlNode(doc, "Title", Title));
            e1.AppendChild(SetXmlNode(doc, "PageSize", PageSize.ToString()));
            e1.AppendChild(SetXmlNode(doc, "PageOrientation", PageOrientation.ToString()));

            e1.AppendChild(SetXmlNode(doc, "HeaderFontSize", DocumentHeaderFontSize.ToString()));
            e1.AppendChild(SetXmlNode(doc, "PageHeaderSize", PageHeaderFontSize.ToString()));
            e1.AppendChild(SetXmlNode(doc, "LabelSize", LabelFontSize.ToString()));
            e1.AppendChild(SetXmlNode(doc, "SubLabelSize", SubLabelFontSize.ToString()));

            return e1;
        }

        public void Load(XmlDocument doc)
        {
            Images.Clear();
            Mantras.Clear();
            Pages.Clear();
            foreach (XmlElement e1 in doc.ChildNodes)
            {
                if (e1.Name.Equals("gms"))
                {
                    foreach (XmlElement e2 in e1.ChildNodes)
                    {
                        switch (e2.Name)
                        {
                            case "image":
                                MNReferencedImage image = new MNReferencedImage();
                                image.Load(e2);
                                Images.Add(image);
                                break;
                            case "mantra":
                                MNReferencedMantra mantra = new MNReferencedMantra();
                                mantra.Load(e2);
                                Mantras.Add(mantra);
                                break;
                            case "page":
                                MNPage page = new MNPage(this);
                                page.Load(e2);
                                Pages.Add(page);
                                break;
                            case "Title": Title = e2.InnerText; break;
                            case "PageSize": PageSize = int.Parse(e2.InnerText); break;
                            case "PageOrientation": PageOrientation = int.Parse(e2.InnerText); break;
                            case "HeaderFontSize": DocumentHeaderFontSize = float.Parse(e2.InnerText); break;
                            case "PageHeaderSize": PageHeaderFontSize = float.Parse(e2.InnerText); break;
                            case "LabelSize": LabelFontSize = float.Parse(e2.InnerText); break;
                            case "SubLabelSize": SubLabelFontSize = float.Parse(e2.InnerText); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public static XmlElement SetXmlNode(XmlDocument doc, string name, string value)
        {
            XmlElement e1 = doc.CreateElement(name);
            e1.InnerText = value;
            return e1;
        }

        public MNReferencedImage FindImage(string p)
        {
            foreach (MNReferencedImage img in Images)
            {
                if (img.Title.Equals(p))
                    return img;
            }
            return null;
        }
    }
}
