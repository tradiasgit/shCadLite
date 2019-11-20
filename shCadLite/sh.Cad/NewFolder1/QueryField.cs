using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace sh.XmlResourcesParsing.Fields
{
    public class QueryField : Field
    {


        public string ResultType { get; set; }

        public string BlockTableRecord { get; set; } = "*MODEL_SPACE";

        public override double GetValue()
        {
            var result = Query();
            switch (ResultType)
            {
                default: return 0;
                case "SumArea": return SumArea(result);
                case "SumLength": return SumLength(result);
                case "Count": return Count(result);
            }
        }



        #region 计算
        public int Count(IEnumerable<ObjectId> selection)
        {
            if (selection == null) return 0;
            var value = selection.Count();
            //WriteLine($"{value}个匹配对象{Environment.NewLine}");
            return selection.Count();
        }

        protected double GetMlineLength(Entity ent)
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


        public double SumLength(IEnumerable<ObjectId> selection)
        {
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
        public double SumArea(IEnumerable<ObjectId> selection)
        {
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

        private void WriteLine(string message)
        {
            Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(Environment.NewLine + message + Environment.NewLine);
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
                                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.Message);
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

        class Filter { public string Type { get; set; } public string Name { get; set; } }

        List<Filter> _filters;

        public QueryField(XmlElement ele) : base(ele)
        {
        }

        List<Filter> Filters { get { if (_filters == null) _filters = GetFilters(); return _filters; } }
        private List<Filter> GetFilters()
        {
            var result = new List<Filter>();
            var nodes = Element.SelectNodes("Filter");
            if (nodes != null && nodes.Count > 0)
            {

                foreach (var n in nodes)
                {
                    var attType = (n as XmlElement).Attributes["Type"]?.Value;
                    var attName = (n as XmlElement).Attributes["Name"]?.Value;
                    result.Add(new Filter { Type = attType, Name = attName });
                }
            }
            return result;
        }

        public bool HasTypeFilter { get { return Filters != null && Filters.Any(p => p.Type == "EntityType"); } }
        public bool HasLayerFilter { get { return Filters != null && Filters.Any(p => p.Type == "Layer"); } }


        public bool HitTypeFilter(ObjectId oid)
        {
            bool typeFlag = true;
            if (HasTypeFilter)
            {
                typeFlag = Filters.Exists(p => p.Type == "EntityType" && p.Name == oid.ObjectClass.DxfName);
            }
            return typeFlag;
        }
        public bool HitLayerFilter(ObjectId oid, Transaction tr)
        {
            bool layerFlag = true;
            if (HasLayerFilter)
            {
                Entity ent = tr.GetObject(oid, OpenMode.ForRead, false, true) as Entity;
                if (ent != null)
                {
                    layerFlag = Filters.Exists(p => p.Type == "Layer" && p.Name == ent.Layer);// LayerFilter.Contains(ent.Layer);
                }
            }
            return layerFlag;

        }
        public bool HitDataFilter(ObjectId oid, Transaction tr)
        {
            bool dataFlag = true;
            var data = GetData();
            if (data.Any())
            {
                dataFlag = false;
                var obj = tr.GetObject(oid, OpenMode.ForRead);
                if (obj != null)
                {
                    if (!obj.ExtensionDictionary.IsNull && obj.ExtensionDictionary.IsValid)
                    {
                        using (DBDictionary dbextdic = tr.GetObject(obj.ExtensionDictionary, OpenMode.ForRead) as DBDictionary)
                        {

                            dataFlag = data.All(kv => IfEntityContainsKeyValue(dbextdic, kv, tr));
                        }
                    }
                }
            }
            return dataFlag;
        }
        private bool IfEntityContainsKeyValue(DBDictionary dbextdic, KeyValuePair<string, string> kv, Transaction tr)
        {
            var r = false;
            if (dbextdic.Contains(kv.Key))
            {
                if (string.IsNullOrWhiteSpace(kv.Value) || kv.Value == "*") return true;
                else
                {
                    var one = dbextdic.GetAt(kv.Key);
                    using (var xRec = tr.GetObject(one, OpenMode.ForRead) as Xrecord)
                    {
                        if (xRec != null && xRec.Data != null)
                        {
                            var array = xRec.Data.AsArray();
                            if (array.Any())
                            {
                                var value = array[0].Value.ToString();
                                return value == kv.Value;
                            }
                        }
                    }
                }
            }
            return r;
        }

        #endregion



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

                        if (bt.Has(BlockTableRecord))
                        {
                            using (BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord], OpenMode.ForRead, false, true))
                            {
                                foreach (var oid in btr)
                                {
                                    if (HitTypeFilter(oid) && HitLayerFilter(oid, tr) && HitDataFilter(oid, tr))
                                        result.Add(oid);
                                }
                            }
                        }
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($"执行失败，{ex.Message}{Environment.NewLine}");
            }
            return null;
        }


    }

}
