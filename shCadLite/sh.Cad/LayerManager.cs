using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


      
    }
}
