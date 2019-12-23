using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace sh.Cad
{
    public class EntitySelection
    {
        public EntitySelection(PromptSelectionResult result)
        {
            if (result.Status == PromptStatus.OK && result.Value != null)
            {
                ObjectIds = result.Value.GetObjectIds().ToList();
                Count = ObjectIds.Count;
            }
        }

        public EntitySelection()
        {
        }

        public int Count { get; set; }


        private List<ObjectId> ObjectIds { get; set; } = new List<ObjectId>();

        public EntityInfo GetEntity()
        {
            return GetEntityies()?[0];
        }
        public T GetEntity<T>()where T:EntityInfo
        {
            return GetEntity() as T;
        }

            List<EntityInfo> _entitys;
        public List<EntityInfo> GetEntityies()
        {
            if (_entitys == null && ObjectIds.Count > 0)
            {
                _entitys = new List<EntityInfo>();
                var db = HostApplicationServices.WorkingDatabase;
                _entitys.Clear();
                using (var tr = db.TransactionManager.StartOpenCloseTransaction())
                {
                    foreach (var oid in ObjectIds)
                    {
                        var ent = EntityInfo.Get(oid, tr);// tr.GetObject(oid, OpenMode.ForRead) as Entity);
                        _entitys.Add(ent);
                    }
                }
            }
            return _entitys;
        }

    }



}
