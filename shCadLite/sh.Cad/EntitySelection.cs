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
            }
        }

        public int Count { get { return ObjectIds.Count; } }
       

        public List<ObjectId> ObjectIds { get; private set; } = new List<ObjectId>();


        List<EntityInfo> _entitys;
        public List<EntityInfo> Entitys
        {
            get
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
                            var ent =new EntityInfo( tr.GetObject(oid, OpenMode.ForRead) as Entity);
                            _entitys.Add(ent);
                        }
                    }
                }
                return _entitys;
            }
        }

    }



}
