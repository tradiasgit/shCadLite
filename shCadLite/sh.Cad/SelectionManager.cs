using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public class SelectionManager
    {
        public static EntitySelection GetSelection()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            return new EntitySelection(doc.Editor.SelectImplied());
        }



        private static List<Document> _docs = new List<Document>();


        private static void RegisterEvent_DocumentSelectionChanged(Document doc = null)
        {

            if (doc == null) doc = Application.DocumentManager.MdiActiveDocument;
            if (_docs.Contains(doc)) return;
            else
            {
                doc.ImpliedSelectionChanged += Doc_ImpliedSelectionChanged;
                _docs.Add(doc);
            }
        }

        private static void UnRegisterEvent_DocumentSelectionChanged(Document doc = null)
        {

            if (doc == null) doc = Application.DocumentManager.MdiActiveDocument;
            if (_docs.Contains(doc))
            {
                _docs.Remove(doc);
            }
            doc.ImpliedSelectionChanged -= Doc_ImpliedSelectionChanged;
        }

        private static void Doc_ImpliedSelectionChanged(object sender, EventArgs e)
        {
            var doc = (Document)sender;
            try
            {
                var selection = GetSelection();
                if (_listeners.Count > 0)
                {
                    foreach (var l in _listeners)
                    {
                        l.OnSelectionChanged(selection);
                    }
                }
            }
            catch (Exception ex)
            {
                doc.Editor.WriteMessage(System.Environment.NewLine + "【选择集监听异常】" + ex.Message + System.Environment.NewLine);
            }
        }

        private static List<IEntitySelectionListener> _listeners = new List<IEntitySelectionListener>();


        /// <summary>
        /// 此方法可能有内存泄漏，所以每个类型只保留最后一次注册的对象的通知
        /// </summary>
        /// <param name="listener"></param>
        public static void RegisterListener(IEntitySelectionListener listener)
        {

            var old = _listeners.FirstOrDefault(p => p.GetType().FullName == listener.GetType().FullName);
            if (old != null)
            {
                _listeners.Remove(old);
            }
            _listeners.Add(listener);
            RegisterEvent_DocumentSelectionChanged();
        }
        public static void UnRegisterListener(IEntitySelectionListener listener)
        {
            if (_listeners.Contains(listener)) _listeners.Remove(listener);
            UnRegisterEvent_DocumentSelectionChanged();
        }
    }

    public class DataWithKey
    {

        public DataWithKey(KeyValuePair<string, string> kv)
        {
            Key = kv.Key;
            Values.Add(kv.Value);
        }
        public string Key { get; set; }

        public string Value
        {
            get
            {
                return Values.FirstOrDefault();
            }
        }

        public List<string> Values { get; set; } = new List<string>();


        public void SetValue(string v)
        {
            if (!Values.Contains(v)) Values.Add(v);
        }
    }


}
