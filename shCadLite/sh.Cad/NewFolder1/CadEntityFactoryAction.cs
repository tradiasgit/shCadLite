using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Colors;
using sh.XmlResourcesParsing.XmlNodeViewModels;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using System.Xml;

namespace sh.XmlResourcesParsing
{
    public abstract class CadEntityFactoryAction : CadEntityActionBase
    {
        public CadEntityFactoryAction(XmlElement ele) : base(ele)
        {
        }

        protected abstract Entity CreateEntity();

        protected virtual bool OnCommitting(Transaction tr, BlockTableRecord space, Entity ent) { return true; }
        protected virtual void OnCommited(Entity ent) { }
        
        public Dictionary<string,string> Data { get; set; }


        public override void Execute()
        {
            var db = HostApplicationServices.WorkingDatabase;
            var doc = Application.DocumentManager.MdiActiveDocument;
            using (var acLock = doc.LockDocument())
            {
                using (var ent = CreateEntity())
                {
                    if (LayerName != null)
                        SetLayer(db, ent, LayerName, true);
                    using (Transaction tr = doc.TransactionManager.StartTransaction())
                    {
                        try
                        {
                            var space = (BlockTableRecord)tr.GetObject(doc.Database.CurrentSpaceId, OpenMode.ForWrite);
                            space.AppendEntity(ent);
                            tr.AddNewlyCreatedDBObject(ent, true);
                            if (OnCommitting(tr, space, ent))
                            {
                                tr.Commit();
                                OnCommited(ent);
                                WriteDictionary(ent.ObjectId, Data);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (!ent.IsDisposed) ent.Dispose();
                            tr.Abort();
                            throw ex;
                        }
                    }
                    OnExecuted(ent);
                }
            }
        }

        protected virtual void OnExecuted(Entity ent) { }
    }
}
