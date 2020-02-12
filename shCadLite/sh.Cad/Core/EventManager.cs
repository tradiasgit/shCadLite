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
        private static List<IDocumentCollectionListener> _docListeners = new List<IDocumentCollectionListener>();
        private static List<IntPtr> _docs = new List<IntPtr>();
        private static bool _Started;
        //private static object locker;

        public static void Start()
        {
            if (_Started) return;
            try
            {
                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentCreated -= DocumentChanged;
                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentCreated += DocumentChanged;
                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentBecameCurrent -= DocumentChanged;
                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentBecameCurrent += DocumentChanged;
                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentActivated -= DocumentChanged;
                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentActivated += DocumentChanged;
                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentToBeDestroyed -= DocumentToBeDestroyed;
                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.DocumentToBeDestroyed += DocumentToBeDestroyed;
                _Started = true;
            }
            catch (Exception ex)
            {
                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument?.Editor.WriteMessage(Environment.NewLine + "【监听事件异常】" + ex.Message + Environment.NewLine);
                //System.Windows.MessageBox.Show(System.Environment.NewLine + "【监听事件异常】" + ex.Message + System.Environment.NewLine);
            }
        }


        private static void Show(string message)
        {
            message = Environment.NewLine + message + Environment.NewLine;
            var ed = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument?.Editor;
            if (ed != null) ed.WriteMessage(message);
            else System.Windows.MessageBox.Show(message);
        }


        private static IntPtr _lastDocumentHandle;

        public static void DocumentChanged(object sender, DocumentCollectionEventArgs e)
        {
            if (e.Document == null)
            {
                _lastDocumentHandle = default(IntPtr);
                var de = new DocumentEventArgs()
                {
                    DocumentFile = null
                };
                foreach (var l in _docListeners)
                {
                    try
                    {
                        l.OnDocumentChanged(de);
                    }
                    catch (Exception ex)
                    {
                        Show("【事件异常】" + ex.Message);
                    }
                }
            }
            else if (_lastDocumentHandle != e.Document.Window.Handle)
            {
                _lastDocumentHandle = e.Document.Window.Handle;
                var de = new DocumentEventArgs()
                {
                    DocumentFile = new System.IO.FileInfo(e.Document.Database.OriginalFileName)
                };
                foreach (var l in _docListeners)
                {
                    try
                    {
                        l.OnDocumentChanged(de);
                    }
                    catch (Exception ex)
                    {
                        Show("【事件异常】" + ex.Message);
                    }
                }
                ListenDocumentSelectionChanged(e.Document);
            }
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
            //Show("Doc_ImpliedSelectionChanged");
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



        /// <summary>
        /// 此方法可能有内存泄漏，所以每个类型只保留最后一次注册的对象的通知
        /// </summary>
        /// <param name="listener"></param>
        public static void RegisterDocumentCollectionListener(IDocumentCollectionListener listener)
        {

            var old = _docListeners.FirstOrDefault(p => p.GetType().FullName == listener.GetType().FullName);
            if (old != null)
            {
                _docListeners.Remove(old);
            }
            _docListeners.Add(listener);
        }
        public static void UnRegisterDocumentCollectionListener(IDocumentCollectionListener listener)
        {
            if (_docListeners.Contains(listener)) _docListeners.Remove(listener);
        }

    }


    public class CadEventListenException : Exception
    {

    }
}
