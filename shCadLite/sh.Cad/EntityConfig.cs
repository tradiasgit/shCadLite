using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace sh.Cad
{
    public class EntityConfig
    {
        public string EntityTypeName { get; protected set; }

        public int ColorIndex { get; set; } = 256;

        public string LayerName { get; set; }

        public Dictionary<string, string> Data { get; protected set; }

        public HacthInfo Hatch { get; protected set; }

        public string BlockName { get; protected set; }


       


        //public virtual string GetValueText() { return string.Format(ValueFormat, GetValue() * ValueRatio); }



        //public double GetValue()
        //{
        //    var result = Query();
        //    switch (ValueType)
        //    {
        //        default: return 0;
        //        case "SumArea": return SumArea(result);
        //        case "SumLength": return SumLength(result);
        //        case "Count": return Count(result);
        //    }
        //}

        //protected IEnumerable<ObjectId> Query()
        //{
        //    try
        //    {
        //        var db = HostApplicationServices.WorkingDatabase;
        //        var result = new List<ObjectId>();
        //        using (var tr = db.TransactionManager.StartOpenCloseTransaction())
        //        {
        //            using (BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false, true))
        //            {
        //                if (bt.Has(BlockTableRecord))
        //                {
        //                    using (BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord], OpenMode.ForRead, false, true))
        //                    {
        //                        foreach (var oid in btr)
        //                        {
        //                            if (HitLayerFilter(oid, tr) && HitDataFilter(oid, tr))
        //                                result.Add(oid);
        //                        }
        //                    }
        //                }
        //            }
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($"执行失败，{ex.Message}{Environment.NewLine}");
        //    }
        //    return null;
        //}

        //#region 计算
        //public int Count(IEnumerable<ObjectId> selection)
        //{
        //    if (selection == null) return 0;
        //    var value = selection.Count();
        //    //WriteLine($"{value}个匹配对象{Environment.NewLine}");
        //    return selection.Count();
        //}

        //protected double GetMlineLength(Entity ent)
        //{
        //    var ml = ent as Mline;
        //    double length = 0;
        //    if (ml == null) return length;
        //    for (int i = 0; i < ml.NumberOfVertices; i++)
        //    {
        //        Point3d pointS = ml.VertexAt(i);
        //        if (i < ml.NumberOfVertices - 1)
        //        {
        //            var pointE = ml.VertexAt(i + 1);
        //            length += pointS.DistanceTo(pointE);
        //        }
        //        else if (ml.IsClosed)
        //        {
        //            var pointE = ml.VertexAt(0);
        //            length += pointS.DistanceTo(pointE);
        //        }
        //    }
        //    return length;
        //}


        //public double SumLength(IEnumerable<ObjectId> selection)
        //{
        //    if (selection == null) return 0;
        //    var value = 0.0;
        //    ForEach(selection, ent =>
        //    {
        //        if (ent is Mline) value += GetMlineLength(ent);
        //        else
        //        {
        //            var prop = ent.GetType().GetProperty("Length");
        //            if (prop != null)
        //            {
        //                value += (double)prop.GetValue(ent);
        //            }
        //        }

        //    });
        //    //WriteLine($"总长度:{value}{Environment.NewLine}");
        //    return value;
        //}
        //public double SumArea(IEnumerable<ObjectId> selection)
        //{
        //    if (selection == null) return 0;
        //    var value = 0.0;
        //    ForEach(selection, ent =>
        //    {
        //        var prop = ent.GetType().GetProperty("Area");
        //        if (prop != null)
        //        {
        //            value += (double)prop.GetValue(ent);
        //        }
        //    });
        //    //WriteLine($"总面积:{value}{Environment.NewLine}");
        //    return value;
        //}

        //private void ForEach(IEnumerable<ObjectId> selection, Action<Entity> fun, Func<Exception, bool> exFun = null)
        //{
        //    if (fun != null)
        //    {
        //        var db = HostApplicationServices.WorkingDatabase;
        //        using (var tr = db.TransactionManager.StartOpenCloseTransaction())
        //        {
        //            foreach (var oid in selection)
        //            {
        //                try
        //                {
        //                    var ent = tr.GetObject(oid, OpenMode.ForRead, false, true) as Entity;
        //                    if (ent != null)
        //                    {
        //                        fun(ent);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    if (ex == null)
        //                    {
        //                        Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.Message);
        //                    }
        //                    else
        //                    {
        //                        exFun(ex);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //#endregion

        //#region 命中

        //public bool HitLayerFilter(ObjectId oid, Transaction tr)
        //{
        //    if (LayerName == null) return true;
        //    Entity ent = tr.GetObject(oid, OpenMode.ForRead, false, true) as Entity;
        //    return ent != null && ent.Layer == LayerName;

        //}
        //public bool HitDataFilter(ObjectId oid, Transaction tr)
        //{
        //    if (Data == null) return true;
        //    bool dataFlag = true;
        //    if (Data.Any())
        //    {
        //        dataFlag = false;
        //        var obj = tr.GetObject(oid, OpenMode.ForRead);
        //        if (obj != null)
        //        {
        //            if (!obj.ExtensionDictionary.IsNull && obj.ExtensionDictionary.IsValid)
        //            {
        //                using (DBDictionary dbextdic = tr.GetObject(obj.ExtensionDictionary, OpenMode.ForRead) as DBDictionary)
        //                {
        //                    dataFlag = Data.All(kv => IfEntityContainsKeyValue(dbextdic, kv, tr));
        //                }
        //            }
        //        }
        //    }
        //    return dataFlag;
        //}
        //private bool IfEntityContainsKeyValue(DBDictionary dbextdic, KeyValuePair<string, string> kv, Transaction tr)
        //{
        //    var r = false;
        //    if (dbextdic.Contains(kv.Key))
        //    {
        //        if (string.IsNullOrWhiteSpace(kv.Value) || kv.Value == "*") return true;
        //        else
        //        {
        //            var one = dbextdic.GetAt(kv.Key);
        //            using (var xRec = tr.GetObject(one, OpenMode.ForRead) as Xrecord)
        //            {
        //                if (xRec != null && xRec.Data != null)
        //                {
        //                    var array = xRec.Data.AsArray();
        //                    if (array.Any())
        //                    {
        //                        var value = array[0].Value.ToString();
        //                        return value == kv.Value;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return r;
        //}

        //#endregion


    }
}
