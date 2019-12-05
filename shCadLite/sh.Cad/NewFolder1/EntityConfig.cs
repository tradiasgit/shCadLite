using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
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
        public EntityConfig(FileInfo file)
        {
            if (file.Exists)
            {
                var doc = new XmlDocument();
                doc.Load(file.FullName);
                SetFromXml(doc.DocumentElement);
            }
        }

        private void SetFromXml(XmlElement ele)
        {
            Data = GetData(ele);
            SetPropertyFromAttribute(this, ele, "ColorIndex");
            SetPropertyFromAttribute(this, ele, "LayerName");
            SetPropertyFromAttribute(this, ele, "ValueFormat");
            SetPropertyFromAttribute(this, ele, "ValueRatio");
            SetPropertyFromAttribute(this, ele, "ValueType");
            Hacth = GetHatch(ele);
        }

        private Dictionary<string, string> GetData(XmlElement ele)
        {
            var result = new Dictionary<string, string>();
            if (ele != null)
            {
                var data = ele.SelectNodes("Data");
                if (data != null && data.Count > 0)
                {
                    foreach (XmlNode d in data)
                    {
                        var remove = d.Attributes["RemoveKey"];
                        if (remove != null)
                        {
                            result.Remove(remove.Value);
                        }
                        else
                        {
                            var k = d.Attributes["Key"].Value;
                            var v = d.Attributes["Value"].Value;
                            if (result.ContainsKey(k))
                                result[k] = v;
                            else
                                result.Add(k, v);
                        }
                    }
                }
            }
            return result;
        }

        private HacthConfig GetHatch(XmlElement ele)
        {
            if (ele != null)
            {
                var hatchele = ele.SelectSingleNode("//Hatch") as XmlElement;
                if (hatchele != null)
                    return new HacthConfig(hatchele);
            }
            return null;
        }

        protected static void SetPropertyFromAttribute(object target, XmlElement ele, string name)
        {
            var att = ele.Attributes[name];
            if (att != null)
            {
                var prop = target.GetType().GetProperty(name);
                if (prop != null && prop.SetMethod != null)
                {
                    string value = att.Value;

                    object obj = null;
                    if (prop.PropertyType == typeof(string))
                        obj = value;
                    else if (prop.PropertyType == typeof(Guid))
                    {
                        Guid v = Guid.Empty;
                        if (Guid.TryParse(value, out v))
                            obj = v;
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        int v = 0;
                        if (int.TryParse(value, out v))
                            obj = v;
                    }
                    else if (prop.PropertyType == typeof(short))
                    {
                        short v = 0;
                        if (short.TryParse(value, out v))
                            obj = v;
                    }
                    else if (prop.PropertyType == typeof(double))
                    {
                        double v = 0;
                        if (double.TryParse(value, out v))
                            obj = v;
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        bool v = false;
                        if (bool.TryParse(value, out v))
                            obj = v;
                    }
                    if (obj != null)
                        prop.SetValue(target, obj);
                }

            }
        }



        public short ColorIndex { get; set; } = 256;

        public string LayerName { get; set; }

        public HacthConfig Hacth { get; set; }

        public double Value { get; set; }

        public string ValueFormat { get; set; } = "";

        public double ValueRatio { get; set; } = 1;

        public string ValueType { get; set; }

        private string BlockTableRecord { get; set; } = "*MODEL_SPACE";

        public Dictionary<string, string> Data { get; set; }

        public void Brush()
        {
            try
            {
                while (DoBrush()) ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private bool DoBrush()
        {
            try
            {
                var oid = ObjectId.Null;
                var doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                var db = HostApplicationServices.WorkingDatabase;
                var ed = doc.Editor;
                using (var l = doc.LockDocument())
                {
                    PromptEntityResult r;

                    r = ed.DoPrompt(new PromptEntityOptions("选择目标对象或[设置(S)]", "s") { AllowObjectOnLockedLayer = true }) as PromptEntityResult;
                    if (r.Status == PromptStatus.OK)
                    {
                        oid = r.ObjectId;
                        using (var tr = db.TransactionManager.StartTransaction())
                        {
                            Entity ent = tr.GetObject(r.ObjectId, OpenMode.ForWrite) as Entity;
                            ent.ColorIndex = ColorIndex;
                            if (!string.IsNullOrWhiteSpace(LayerName))
                            {
                                LayerManager.SetLayer(db, ent, LayerName, true);
                            }


                            if (ent is Hatch && Hacth != null)
                            {
                                var h = ent as Hatch;
                                Hacth.SetHatch(h);
                            }
                            tr.Commit();
                            if (Data != null)
                                DataManager.WriteDictionary(ent.Id, Data);
                        }
                        ed.WriteMessage(Environment.NewLine);
                        return true;
                    }
                    else if (r.Status == PromptStatus.Keyword)
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($"执行失败，{ex.Message}{Environment.NewLine}");
            }
            return false;
        }



        public virtual string GetValueText() { return string.Format(ValueFormat, GetValue() * ValueRatio); }



        public double GetValue()
        {
            var result = Query();
            switch (ValueType)
            {
                default: return 0;
                case "SumArea": return SumArea(result);
                case "SumLength": return SumLength(result);
                case "Count": return Count(result);
            }
        }

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
                                    if (HitLayerFilter(oid, tr) && HitDataFilter(oid, tr))
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
                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($"执行失败，{ex.Message}{Environment.NewLine}");
            }
            return null;
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

        public bool HitLayerFilter(ObjectId oid, Transaction tr)
        {
            if (LayerName == null) return true;
            Entity ent = tr.GetObject(oid, OpenMode.ForRead, false, true) as Entity;
            return ent != null && ent.Layer == LayerName;

        }
        public bool HitDataFilter(ObjectId oid, Transaction tr)
        {
            if (Data == null) return true;
            bool dataFlag = true;
            if (Data.Any())
            {
                dataFlag = false;
                var obj = tr.GetObject(oid, OpenMode.ForRead);
                if (obj != null)
                {
                    if (!obj.ExtensionDictionary.IsNull && obj.ExtensionDictionary.IsValid)
                    {
                        using (DBDictionary dbextdic = tr.GetObject(obj.ExtensionDictionary, OpenMode.ForRead) as DBDictionary)
                        {
                            dataFlag = Data.All(kv => IfEntityContainsKeyValue(dbextdic, kv, tr));
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


    }
}
