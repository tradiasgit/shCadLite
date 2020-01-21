using Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public class EventManager
    {

        private static List<IEntitySelectionListener> _listeners = new List<IEntitySelectionListener>();
        private static List<IntPtr> _docs = new List<IntPtr>();

        public static void Start()
        {
            try
            {
                //Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentCreated += DocumentCreated;
                //Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentBecameCurrent += DocumentBecameCurrent;
                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentActivated += DocumentActivated;
                //Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentToBeDestroyed += DocumentToBeDestroyed;
            }
            catch (Exception ex)
            {
                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument?.Editor.WriteMessage(System.Environment.NewLine + "【监听事件异常】" + ex.Message + System.Environment.NewLine);
                //System.Windows.MessageBox.Show(System.Environment.NewLine + "【监听事件异常】" + ex.Message + System.Environment.NewLine);
            }

        }





        private static void DocumentCreated(object sender, DocumentCollectionEventArgs e)
        {
            ListenDocumentSelectionChanged(e.Document);
        }
        private static void DocumentActivated(object sender, DocumentCollectionEventArgs e)
        {
            ListenDocumentSelectionChanged(e.Document);
        }
        private static void DocumentBecameCurrent(object sender, DocumentCollectionEventArgs e)
        {
            ListenDocumentSelectionChanged(e.Document);
        }
        private static void DocumentToBeDestroyed(object sender, DocumentCollectionEventArgs e)
        {
            if (e.Document != null)
            {
                try
                {
                    if (_docs.Contains(e.Document.Window.Handle))
                    {
                        e.Document.ImpliedSelectionChanged -= Doc_ImpliedSelectionChanged;
                        _docs.Remove(e.Document.Window.Handle);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(System.Environment.NewLine + "【解除监听事件异常】" + ex.Message + System.Environment.NewLine);
                }
            }
        }

        public static bool IsListening { get; set; } = true;

        private static void Doc_ImpliedSelectionChanged(object sender, EventArgs e)
        {
            var doc = (Document)sender;
            if (doc == null) return;
            if (!IsListening) return;
            if (_listeners == null || _listeners.Count <= 0) return;


            EntitySelection selection = new EntitySelection(doc.Editor.SelectImplied());
            foreach (var l in _listeners)
            {
                try
                {
                    l.OnSelectionChanged(selection);
                }
                catch (Exception ex)
                {
                    doc.Editor.WriteMessage(System.Environment.NewLine + "【选择集监听异常】" + ex.Message + System.Environment.NewLine);
                }
            }
        }




        private static void ListenDocumentSelectionChanged(Document doc)
        {
            try
            {
                if (doc != null && !_docs.Contains(doc.Window.Handle))
                {
                    doc.ImpliedSelectionChanged += Doc_ImpliedSelectionChanged;
                    _docs.Add(doc.Window.Handle);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(System.Environment.NewLine + "【监听事件异常】" + ex.Message + System.Environment.NewLine);
            }
        }

        /// <summary>
        /// 此方法可能有内存泄漏，所以每个类型只保留最后一次注册的对象的通知
        /// </summary>
        /// <param name="listener"></param>
        public static void RegisterSelectionListener(IEntitySelectionListener listener)
        {

            var old = _listeners.FirstOrDefault(p => p.GetType().FullName == listener.GetType().FullName);
            if (old != null)
            {
                _listeners.Remove(old);
            }
            _listeners.Add(listener);
        }
        public static void UnRegisterSelectionListener(IEntitySelectionListener listener)
        {
            if (_listeners.Contains(listener)) _listeners.Remove(listener);
        }


    }


    public class CadEventListenException : Exception
    {

    }
}
