using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace sh.Cad
{
    public class EntityInfo
    {
        public EntityInfo(ObjectId oid)
        {
            _oid = oid;
            DxfName = _oid.ObjectClass.DxfName;
            ClassName = _oid.ObjectClass.Name;
        }

        private ObjectId _oid;

        public string LayerName { get; private set; }
        public string DxfName { get; private set; }

        public string ClassName { get; private set; }

        public string TypeName { get; private set; }

        public Dictionary<string, string> Data { get; private set; }

        public void Fill(Transaction tr)
        {
            var ent = tr.GetObject(_oid, OpenMode.ForRead) as Entity;
            if (ent != null)
            {
                LayerName = ent.Layer;
                TypeName = ent.GetType().Name;
                var datamanger = new DataManager();
                Data = datamanger.ReadDictionary(ent);
            }
        }
        public void Fill()
        {
            var db = HostApplicationServices.WorkingDatabase;
            using (var tr = db.TransactionManager.StartOpenCloseTransaction())
            {
                Fill(tr);
            }
        }


    }
}
