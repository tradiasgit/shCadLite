using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace sh.Creator.ViewModels
{
    public class VM_Field
    {
        public string Title { get; set; }

        public string Value { get; set; }

        public VM_Field(FileInfo f)
        {
            var doc = new XmlDocument();
            doc.Load(f.FullName);
            Query = new XmlResourcesParsing.Fields.QueryField(doc.DocumentElement);
            Title = f.Name;
            Refresh();

        }


        public void Refresh()
        {
            try
            {
                Value = Query.GetText();
            }
            catch (Exception e)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(e.Message + Environment.NewLine);
            }
        }


        public XmlResourcesParsing.Fields.QueryField Query { get; set; }
    }
}
