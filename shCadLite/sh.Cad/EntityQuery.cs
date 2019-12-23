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
        public static List<EntityQuery> Compute(List<EntityInfo> infos)
        {
            var result = infos.Select(p => new EntityQuery(p)).ToList();
            try
            {
                var db = HostApplicationServices.WorkingDatabase;
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    using (BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false, true))
                    {
                        using (BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead, false, true))
                        {
                            foreach (var oid in btr)
                            {
                                var ent = tr.GetObject(oid, OpenMode.ForRead) as Entity;
                                if (ent != null)
                                {
                                    result.FirstOrDefault(q => q.info.Compare(ent, tr))?.Add(ent);
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

        public static EntityQuery Compute(EntityInfo info)
        {
            var result = new EntityQuery(info);
            var db = HostApplicationServices.WorkingDatabase;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                using (BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false, true))
                {
                    using (BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead, false, true))
                    {
                        foreach (var oid in btr)
                        {
                            var ent = tr.GetObject(oid, OpenMode.ForRead) as Entity;
                            if (ent != null&&info.Compare(ent, tr))
                            {
                                result.Add(ent);
                            }
                        }
                    }
                }
                return result;
            }
        }


        private void Add(Entity ent)
        {
            Count++;
            var l = ent.GetLength(); if (l != null) SumLength += l.Value;
            var a = ent.GetArea(); if (a != null) SumArea += a.Value;
            selection.Add(ent.ObjectId);
        }


        protected EntityQuery(EntityInfo ent)
        {
            info = ent;
        }
        private EntityInfo info;

        List<ObjectId> selection = new List<ObjectId>();


        public int Count { get; private set; }

        public double SumArea { get; private set; }

        public double SumLength { get; private set; }

        public void Select()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            using (doc.LockDocument())
            {
                doc.Editor.SetImpliedSelection(selection.ToArray());
            }
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



    }
}
