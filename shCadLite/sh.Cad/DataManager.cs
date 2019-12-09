using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public class DataManager
    {


        public Dictionary<string, string> ReadDictionary(Entity ent)
        {
            if (ent == null) throw new Exception("读取数据字典失败，ent为null");
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (ent.ExtensionDictionary.IsNull) return result;
            using (var tr = ent.Database.TransactionManager.StartTransaction())
            {
                DBDictionary dbextdic = tr.GetObject(ent.ExtensionDictionary, OpenMode.ForRead) as DBDictionary;
                foreach (var one in dbextdic)
                {
                    var dbext = tr.GetObject(one.Value, OpenMode.ForRead);
                    Xrecord xRec = dbext as Xrecord;
                    if (dbext == null) continue;//throw new EntityDictionaryException(this,"没有找到扩展属性记录");
                    else if (xRec != null)
                    {
                        var value = xRec.Data.AsArray()[0].Value.ToString();
                        result.Add(one.Key, value);
                    }
                }
            }
            return result;
        }
        public static void WriteDictionary(string handle, Dictionary<string, string> data)
        {
            var oid = DatabaseManager.GetObjectIdByHandle(handle);
            if (oid != ObjectId.Null)
                WriteDictionary(oid, data);
        }

        public void WriteDictionary(Entity ent, Dictionary<string, string> data)
        {
            using (var locker = Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                using (var tr = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction())
                {
                    var obj = tr.GetObject(ent.ObjectId, OpenMode.ForWrite);
                    if (!obj.ExtensionDictionary.IsValid)
                    {
                        //创建扩展字典
                        obj.CreateExtensionDictionary();
                    }
                    using (var objdict = (DBDictionary)tr.GetObject(obj.ExtensionDictionary, OpenMode.ForWrite, false))
                    {
                        foreach (var kv in data)
                        {
                            Xrecord xrecord = new Xrecord() { Data = new ResultBuffer(new TypedValue((int)DxfCode.Text, kv.Value)) };
                            objdict.SetAt(kv.Key, xrecord);
                            tr.AddNewlyCreatedDBObject(xrecord, true);
                            xrecord.Dispose();
                        }
                    }
                    tr.Commit();
                }
            }
        }
        public static void WriteDictionary(ObjectId oid, Dictionary<string, string> data, bool clear = false)
        {
            using (var locker = Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                using (var tr = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction())
                {
                    var obj = tr.GetObject(oid, OpenMode.ForWrite);
                    if (!obj.ExtensionDictionary.IsValid)
                    {
                        //创建扩展字典
                        obj.CreateExtensionDictionary();
                    }
                    using (var objdict = (DBDictionary)tr.GetObject(obj.ExtensionDictionary, OpenMode.ForWrite, false))
                    {
                        if (clear)
                        {
                            foreach (var k in objdict)
                            {
                                objdict.Remove(k.Key);
                            }
                        }

                        foreach (var kv in data)
                        {
                            Xrecord xrecord = new Xrecord() { Data = new ResultBuffer(new TypedValue((int)DxfCode.Text, kv.Value)) };
                            objdict.SetAt(kv.Key, xrecord);
                            tr.AddNewlyCreatedDBObject(xrecord, true);
                            xrecord.Dispose();
                        }
                    }
                    tr.Commit();
                }
            }
        }

        public static void RemoveKey(string handle, string key)
        {
            var oid = DatabaseManager.GetObjectIdByHandle(handle);
            if (oid != ObjectId.Null)
                RemoveKey(oid,key);
        }

        public static void RemoveKey(ObjectId oid, string key)
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            using (var l = doc.LockDocument())
            {
                var layermanager = new LayerManager();
                using (var tr = oid.Database.TransactionManager.StartTransaction())
                {
                    var ent = tr.GetObject(oid, OpenMode.ForRead) as Entity;
                    var layerid = ent.LayerId;
                    using (var layerlocker = layermanager.OpenLayer(tr, layerid))
                    {
                        var obj = tr.GetObject(oid, OpenMode.ForWrite);
                        if (!obj.ExtensionDictionary.IsValid)
                        {
                            //创建扩展字典
                            return;
                        }


                        using (var objdict = (DBDictionary)tr.GetObject(obj.ExtensionDictionary, OpenMode.ForWrite, false))
                        {
                            //if(objdict.Contains(key))
                            objdict.Remove(key);
                        }
                    }
                    tr.Commit();
                }
            }
        }


    }
}
