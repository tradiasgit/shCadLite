using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public class DatabaseManager
    {
        private Database _db;
        protected Database Database { get { if (_db != null) return _db; else return HostApplicationServices.WorkingDatabase; } set { _db = value; } }


        public void UseTransaction(Action<Transaction> act, Transaction tr = null)
        {
            if (act != null)
            {
                if (tr == null)
                {
                    using (tr = Database.TransactionManager.StartTransaction())
                    {
                        act(tr);
                        //tr.Commit();
                    }
                }
                else
                {
                    act(tr);
                }
            }
        }

        public BlockTableRecord GetModelSpaceBlockTableRecordForWrite(Transaction tr)
        {
            BlockTable bt = (BlockTable)tr.GetObject(Database.BlockTableId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
            return btr;
        }

        public BlockTableRecord GetModelSpaceBlockTableRecordForWrite(Transaction tr,string space)
        {
            BlockTable bt = (BlockTable)tr.GetObject(Database.BlockTableId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[space], OpenMode.ForWrite);
            return btr;
        }


        public List<LayerTableRecord> ExGetAllLayer()
        {
            var oid = Database.LayerTableId;
            List<LayerTableRecord> ltrs = new List<LayerTableRecord>();
            using (var tr = Database.TransactionManager.StartTransaction())
            {
                LayerTable lt = tr.GetObject(oid,OpenMode.ForRead) as LayerTable;
                
                foreach (ObjectId id in lt)
                {
                    LayerTableRecord ltr = (LayerTableRecord)id.GetObject(OpenMode.ForRead);
                    ltrs.Add(ltr);
                }
            }
            return ltrs;
        }


    }
}
