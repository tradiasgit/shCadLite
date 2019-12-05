using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace sh.Cad
{
    public class LayerManager
    {
        public LayerStatesLocker OpenLayer(Transaction tr, ObjectId layerid)
        {
            LayerStatesLocker result = new LayerStatesLocker(tr,layerid);
            return result;
        }


        public ObjectId GetAndSaveLayerID(Database db, string layername, short? color = null, double? weight = null, string linetype = null)
        {
            if (db == null)
                db = HostApplicationServices.WorkingDatabase;
            ObjectId layerId = ObjectId.Null;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);
                if (lt.Has(layername))
                {
                    return lt[layername];
                }
                else
                {
                    LayerTableRecord ltr = new LayerTableRecord();
                    ltr.Name = layername;
                    ltr.IsHidden = false;
                    if (color != null) ltr.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.ByAci, color.Value);
                    lt.UpgradeOpen();
                    layerId = lt.Add(ltr);
                    tr.AddNewlyCreatedDBObject(ltr, true);
                    tr.Commit();
                }
            }
            return layerId;
        }

        public void SaveLayer(string layername, short? color = null, double? weight = null, string linetype = null)
        {
            GetAndSaveLayerID(null, layername, color, weight, linetype);
        }



        public static void SetLayer(Database db, Entity ent, string layerName, bool autoCreate = false)
        {
            if (string.IsNullOrWhiteSpace(layerName)) return;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                using (LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForWrite))
                {
                    if (!lt.Has(layerName))
                    {
                        if (autoCreate)
                        {
                            using (LayerTableRecord ltr = new LayerTableRecord())
                            {
                                ltr.Name = layerName;
                                FileInfo layerfile = null;// FindSupportFile($@"{layerName}.xml", "Layers");
                                string des = null;
                                if (layerfile != null)
                                {
                                    //try
                                    //{
                                    //    XmlDocument doc = new XmlDocument();
                                    //    doc.Load(layerfile.FullName);
                                    //    var node = doc.SelectSingleNode("/Layer");
                                    //    foreach (XmlAttribute att in node.Attributes)
                                    //    {

                                    //        switch (att.Name)
                                    //        {
                                    //            case "ColorIndex": ltr.Color = Color.FromColorIndex(ColorMethod.ByAci, short.Parse(att.Value)); break;
                                    //            case "Description": des = att.Value; break;
                                    //            case "IsPlottable": ltr.IsPlottable = ParseString(att.Value, true); break;
                                    //            case "ViewportVisibilityDefault": ltr.ViewportVisibilityDefault = ParseString(att.Value, false); break;
                                    //            case "LineWeight": ltr.LineWeight = ParseString(att.Value, LineWeight.ByLineWeightDefault); break;
                                    //            case "Linetype": ltr.LinetypeObjectId = SetLinetype(att.Value, "Continuous"); break;
                                    //            case "IsFrozen": ltr.IsFrozen = ParseString(att.Value, false); break;
                                    //            case "IsHidden": ltr.IsHidden = ParseString(att.Value, false); break;
                                    //            case "IsOff": ltr.IsOff = ParseString(att.Value, false); break;
                                    //            case "IsLocked": ltr.IsLocked = ParseString(att.Value, false); break;
                                    //                //case "IsReconciled": ltr.IsReconciled = ParseString(att.Value, true); break;//不能在此设置
                                    //        }
                                    //    }
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //    WriteLine($"读取图层设置文件失败，图层：{layerName}；文件：{layerfile.FullName}；错误：{ex.Message}");
                                    //}
                                }
                                lt.Add(ltr);
                                ltr.Description = des;//此字段必须在add之后写入
                                tr.AddNewlyCreatedDBObject(ltr, true);
                                tr.Commit();
                            }
                        }
                        else throw new Exception("不存在图层" + layerName);
                    }
                    else
                    {

                    }
                }
            }
            ent.Layer = layerName;
        }







    }
}
