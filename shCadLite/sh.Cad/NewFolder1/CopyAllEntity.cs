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


namespace sh.XmlResourcesParsing.Models
{
    public class CopyAllEntity : DrawBlockReference
    {

        public CopyAllEntity(XmlElement ele) : base(ele)
        {
        }

        public string SourceFileName { get; set; }


        private static ObjectId CreateBlankBlock(string name)
        {
            var db = HostApplicationServices.WorkingDatabase;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                using (var bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForWrite, false))
                {
                    var btr = new BlockTableRecord();
                    btr.Name = name;
                    btr.Explodable = true;
                    bt.Add(btr);
                    tr.AddNewlyCreatedDBObject(btr, true);
                    tr.Commit();
                    return btr.Id;
                }
            }
        }
        private static void RemoveBlankBlock(string name)
        {
            var db = HostApplicationServices.WorkingDatabase;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                using (var bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForWrite, false))
                {
                    if (bt.Has(name))
                    {
                        var b = tr.GetObject(bt[name], OpenMode.ForWrite);
                        b.Erase();
                        tr.Commit();
                    }
                }
            }
        }

        private static void ForceExpold(BlockReference ent)
        {

            var doc = Application.DocumentManager.MdiActiveDocument;
            using (var doclock = doc.LockDocument())
            {
                var db = HostApplicationServices.WorkingDatabase;
                using (var tr = db.TransactionManager.StartTransaction())
                {

                    using (var coll = new DBObjectCollection())
                    {
                        ent.Explode(coll);
                        BlockTableRecord table = (BlockTableRecord)tr.GetObject(ent.OwnerId, OpenMode.ForWrite);
                        foreach (DBObject obj in coll)
                        {
                            if (obj is Entity)
                            {
                                table.AppendEntity((Entity)obj);
                                tr.AddNewlyCreatedDBObject(obj, true);
                            }
                        }
                    }
                    var e = tr.GetObject(ent.Id, OpenMode.ForWrite);
                    ent.Erase();
                    tr.Commit();
                }
            }


        }

        private static void ExplodBlockReference(BlockReference ent)
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            using (var doclock = doc.LockDocument())
            {
                var db = HostApplicationServices.WorkingDatabase;
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    ent.ExplodeToOwnerSpace();

                    var e = tr.GetObject(ent.Id, OpenMode.ForWrite);
                    ent.Erase();
                    tr.Commit();
                }
            }

        }

        protected override Entity CreateEntity()
        {
            if (string.IsNullOrWhiteSpace(BlockName)) BlockName = Guid.NewGuid().ToString();
            var file = FindSupportFile(SourceFileName);
            if (file.Exists)
            {
                import(file, BlockName);
            }
            return base.CreateEntity();
        }

        protected override void OnExecuted(Entity ent)
        {
            ExplodBlockReference(ent as BlockReference);
            RemoveBlankBlock(BlockName);
        }

        protected override void OnCommited(Entity ent)
        {
        }


        private static bool import(FileInfo file, string blockName)
        {
            // 返回插入对象
            var resultObjectIdList = new List<ObjectId>();
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;

            try
            {

                var importObjectIds = new ObjectIdCollection();//18345044618
                // 导入方法参数之一，导入前与导入后对象关系
                IdMapping mapping = new IdMapping();
                #region 读取文件fileName图纸，并且导入
                using (var sourceDb = new Database(false, true))
                {
                    if (file.Extension.ToLower() == ".dwg")
                        sourceDb.ReadDwgFile(file.FullName, FileShare.Read, true, "");
                    else if (file.Extension.ToLower() == ".dxf")
                        sourceDb.DxfIn(file.FullName, $@"{Environment.CurrentDirectory}\logs\importdxf\{file.Name}.log");
                    else throw new Exception("不能识别的文件类型：" + file.FullName);

                    #region 获取导入实体
                    using (var tran = sourceDb.TransactionManager.StartTransaction())
                    {
                        //读取源数据库
                        using (var bt = (BlockTable)tran.GetObject(sourceDb.BlockTableId, OpenMode.ForRead, false))
                        {
                            var entitys = new List<Entity>();
                            BlockTableRecord btr = (BlockTableRecord)tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead, false);
                            foreach (ObjectId id in btr)
                            {
                                var ent = tran.GetObject(id, OpenMode.ForRead) as Entity;
                                if (ent == null) continue;
                                importObjectIds.Add(id);
                                entitys.Add(ent);
                            }
                        }
                        //tran.Commit();
                    }
                    #endregion
                    using (var doclock = Application.DocumentManager.MdiActiveDocument.LockDocument())
                    {
                        var targetid = CreateBlankBlock(blockName);
                        //HostApplicationServices.WorkingDatabase.CurrentSpaceId
                        // 复制
                        sourceDb.WblockCloneObjects(importObjectIds, targetid, mapping, DuplicateRecordCloning.Replace, false);
                    }

                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                ed.WriteMessage("\n导入出现错误，" + ex.Message);
                resultObjectIdList = null;
            }
            return false;
        }
    }
}
