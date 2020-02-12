using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public interface IDocumentCollectionListener
    {
        void OnDocumentChanged(DocumentEventArgs e);
    }

    public class DocumentEventArgs : EventArgs
    {
        public FileInfo DocumentFile { get; set; }


    }
}
