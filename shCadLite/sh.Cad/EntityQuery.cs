using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public class EntityQuery
    {

        public EntityQuery(EntityInfo ent)
        {
            info = ent;
        }

        private EntityInfo info;


        public virtual string GetValueText(double value, string format, double ratio) { return string.Format(format, value * ratio); }


        private string BlockName = Autodesk.AutoCAD.DatabaseServices.BlockTableRecord.ModelSpace;

        protected IEnumerable<ObjectId> Query()
        {
            try
            {
                var db = HostApplicationServices.WorkingDatabase;
                var result = new List<ObjectId>();
                using (var tr = db.TransactionManager.StartOpenCloseTransaction())
                {
                    using (BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false, true))
                    {
                        if (bt.Has(BlockName))
                        {
                            using (BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockName], OpenMode.ForRead, false, true))
                            {
                                foreach (var oid in btr)
                                {
                                    var ent = tr.GetObject(oid, OpenMode.ForRead) as Entity;
                                    if (ent!=null&&info.Compare(ent,tr)) result.Add(oid);
                                    //if (HitTypeFilter(oid, tr) && HitLayerFilter(oid, tr) && HitDataFilter(oid, tr))
                                        //result.Add(oid);
                                }
                            }
                        }
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($"执行失败，{ex.Message}{Environment.NewLine}");
            }
            return null;
        }

        IEnumerable<ObjectId> selection;

        public void Select()
        {
            var s = Query();
            var doc = Application.DocumentManager.MdiActiveDocument;
            using (doc.LockDocument())
            {
                doc.Editor.SetImpliedSelection(s.ToArray());
            }
        }


        #region 计算
        public int Count()
        {
            if (selection == null) selection = Query();
            if (selection == null) return 0;
            var value = selection.Count();
            //WriteLine($"{value}个匹配对象{Environment.NewLine}");
            return selection.Count();
        }

        protected static double GetMlineLength(Entity ent)
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


        public double SumLength()
        {
            if (selection == null) selection = Query();
            if (selection == null) return 0;
            var value = 0.0;
            ForEach(selection, ent =>
            {
                if (ent is Mline) value += GetMlineLength(ent);
                else
                {
                    var prop = ent.GetType().GetProperty("Length");
                    if (prop != null)
                    {
                        value += (double)prop.GetValue(ent);
                    }
                }

            });
            //WriteLine($"总长度:{value}{Environment.NewLine}");
            return value;
        }
        public double SumArea()
        {
            if (selection == null) selection = Query();
            if (selection == null) return 0;
            var value = 0.0;
            ForEach(selection, ent =>
            {
                var prop = ent.GetType().GetProperty("Area");
                if (prop != null)
                {
                    value += (double)prop.GetValue(ent);
                }
            });
            //WriteLine($"总面积:{value}{Environment.NewLine}");
            return value;
        }

        private void ForEach(IEnumerable<ObjectId> selection, Action<Entity> fun, Func<Exception, bool> exFun = null)
        {
            if (fun != null)
            {
                var db = HostApplicationServices.WorkingDatabase;
                using (var tr = db.TransactionManager.StartOpenCloseTransaction())
                {
                    foreach (var oid in selection)
                    {
                        try
                        {
                            var ent = tr.GetObject(oid, OpenMode.ForRead, false, true) as Entity;
                            if (ent != null)
                            {
                                fun(ent);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex == null)
                            {
                                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.Message);
                            }
                            else
                            {
                                exFun(ex);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 命中


        //private bool HitTypeFilter(ObjectId oid, Transaction tr)
        //{
        //    if (info.EntityTypeName == null) return true;
        //    Entity ent = tr.GetObject(oid, OpenMode.ForRead, false, true) as Entity;
        //    var entinfo = EntityInfo.Get(oid, tr);

        //    if (info.EntityTypeName == "BlockReference")
        //    {
        //        var br = ent as BlockReference;
        //        if (br == null) return false;
        //        else if (br.IsDynamicBlock)
        //        {
        //            var btr = tr.GetObject(br.DynamicBlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
        //            bool visibility = true;
        //            if (!string.IsNullOrWhiteSpace(info.BlockVisibilityName))
        //            {
        //                visibility = false;
        //                foreach (DynamicBlockReferenceProperty p in br.DynamicBlockReferencePropertyCollection)
        //                {
        //                    if (p.PropertyTypeCode==5 && p.Value is string&&(string)p.Value==info.BlockVisibilityName)
        //                    {
        //                        visibility = true;
        //                        break;
        //                    }
        //                }
        //            }
        //            return btr.Name == info.BlockName&& visibility;
        //        }
        //        else return br.Name == info.BlockName;
        //    }
        //    else return ent != null && ent.GetType().Name == info.EntityTypeName;

        //}
        //private bool HitLayerFilter(ObjectId oid, Transaction tr)
        //{
        //    if (info.LayerName == null) return true;
        //    Entity ent = tr.GetObject(oid, OpenMode.ForRead, false, true) as Entity;
        //    return ent != null && ent.Layer == info.LayerName;

        //}
        //private bool HitDataFilter(ObjectId oid, Transaction tr)
        //{
        //    if (info.Data == null) return true;
        //    bool dataFlag = true;
        //    if (info.Data.Any())
        //    {
        //        dataFlag = false;
        //        var obj = tr.GetObject(oid, OpenMode.ForRead);
        //        if (obj != null)
        //        {
        //            if (!obj.ExtensionDictionary.IsNull && obj.ExtensionDictionary.IsValid)
        //            {
        //                using (DBDictionary dbextdic = tr.GetObject(obj.ExtensionDictionary, OpenMode.ForRead) as DBDictionary)
        //                {
        //                    dataFlag = info.Data.All(kv => IfEntityContainsKeyValue(dbextdic, kv, tr));
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

        #endregion
    }
}
