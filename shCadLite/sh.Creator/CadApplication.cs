using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

[assembly: ExtensionApplication(typeof(sh.Creator.CadApplication))]//注册插件
namespace sh.Creator
{
    public class CadApplication : IExtensionApplication
    {


        public void Initialize()
        {
            try
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($"【山河软件】创造者插件已加载({Assembly.GetExecutingAssembly().GetName().Version.ToString()},Location:{Assembly.GetExecutingAssembly().Location})" + Environment.NewLine);

                try
                {
                    Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentCreated += OnDocumentChanged;
                    Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentBecameCurrent += OnDocumentChanged;
                    Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentActivated += OnDocumentChanged;
                }
                catch (System.Exception ex)
                {
                    Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument?.Editor.WriteMessage(Environment.NewLine + "【监听事件异常】" + ex.Message + Environment.NewLine);
                }

            }
            catch (System.Exception ex)
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("【山河异常】" + ex.Message + Environment.NewLine);
            }
            ToolBoxCommand.Query();
        }

        private static IntPtr _lastDocumentHandle;
        private static List<IntPtr> _docs = new List<IntPtr>();


        private static void OnDocumentChanged(object sender, DocumentCollectionEventArgs e)
        {
            try
            {
                var handle = IntPtr.Zero;
                if (e.Document != null) handle = e.Document.Window.Handle;
                if (handle == _lastDocumentHandle) return;
                if (!_docs.Contains(e.Document.Window.Handle))
                {
                    e.Document.ImpliedSelectionChanged += OnDocumentSelectionChanged;
                    _docs.Add(e.Document.Window.Handle);
                }

                EventManager.RaiseCadDocumentChanged(sender, e);
                _lastDocumentHandle = e.Document.Window.Handle;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(Environment.NewLine + "【监听事件异常】" + ex.Message + Environment.NewLine);
            }

        }
        private static void OnDocumentSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                EventManager.RaiseCadDocumentSelectionChanged(sender, e);
            }
            catch (System.Exception ex)
            {
                _ = System.Windows.MessageBox.Show(Environment.NewLine + "【监听事件异常】" + ex.Message + Environment.NewLine);
            }
        }

        public void Terminate() { }
    }
}
