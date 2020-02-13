using Autodesk.AutoCAD.ApplicationServices;
using sh.Creator.Cad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator
{
    class EventManager
    {
        public static event EventHandler CadDocumentChanged;

        public static void RaiseCadDocumentChanged(object sender, EventArgs e)
        {
            CadDocumentChanged?.Invoke(sender, e);
        }


        public static event EventHandler CadDocumentSelectionChanged;

        public static void RaiseCadDocumentSelectionChanged(object sender, EventArgs e)
        {
            CadDocumentSelectionChanged?.Invoke(sender, e);
        }

        public static event EventHandler<TreeItemEventArgs> RepositorySelectedItemChanged;
        public static void RaiseRepositorySelectedItemChanged(object sender, TreeItemEventArgs e)
        {
            RepositorySelectedItemChanged?.Invoke(sender, e);
        }

    }
    class TreeItemEventArgs : EventArgs
    {
        public TreeItemEventArgs(ViewModels.Repository.VM_TreeItem item)
        {
            SelectedItem = item;
        }
        public ViewModels.Repository.VM_TreeItem SelectedItem { get; private set; }
    }
}
