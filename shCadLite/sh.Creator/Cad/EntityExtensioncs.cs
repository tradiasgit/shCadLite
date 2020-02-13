using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.Cad
{
    public static class EntityExtensioncs
    {
        public static double? ExGetArea(this Entity ent)
        {
            var proparea = ent.GetType().GetProperty("Area");
            if (proparea == null) return null;
            try
            {
                return (double)proparea.GetValue(ent);
            }
            catch (Exception e)
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(Environment.NewLine + "【求面积异常】：" + ent.GetType().Name + "：" + e.Message);
                return null;
            }
        }

        public static double? ExGetLength(this Entity ent)
        {
            if (ent == null) return null;
            if (ent is Mline)
            {
                var ml = ent as Mline;
                double length = 0;
                if (ml == null) return length;
                for (int i = 0; i < ml.NumberOfVertices; i++)
                {
                    Point3d pointS = ml.VertexAt(i);
                    if (i < ml.NumberOfVertices - 1)
                    {
                        var pointE = ml.VertexAt(i + 1);
                        length += pointS.DistanceTo(pointE);
                    }
                    else if (ml.IsClosed)
                    {
                        var pointE = ml.VertexAt(0);
                        length += pointS.DistanceTo(pointE);
                    }
                }
                return length;
            }
            else
            {
                var prop = ent.GetType().GetProperty("Length");
                if (prop != null)
                {
                    try
                    {
                        return (double)prop.GetValue(ent);
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }
            return null;
        }


        public static Dictionary<string, string> ExReadDictionary(this Entity ent, Transaction tr)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (ent.ExtensionDictionary.IsNull) return result;
            DBDictionary dbextdic = tr.GetObject(ent.ExtensionDictionary, OpenMode.ForRead) as DBDictionary;
            foreach (var one in dbextdic)
            {
                var dbext = tr.GetObject(one.Value, OpenMode.ForRead);
                Xrecord xRec = dbext as Xrecord;
                if (dbext == null) continue;
                else if (xRec != null)
                {
                    var value = xRec.Data.AsArray()[0].Value.ToString();
                    result.Add(one.Key, value);
                }
            }
            return result;
        }


        public static void ExWriteDictionary(this Entity ent, string key, string value)
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
                        using (Xrecord xrecord = new Xrecord() { Data = new ResultBuffer(new TypedValue((int)DxfCode.Text, value)) })
                        {
                            objdict.SetAt(key, xrecord);
                            tr.AddNewlyCreatedDBObject(xrecord, true);
                        }
                    }
                    tr.Commit();
                }
            }
        }
    }
}
