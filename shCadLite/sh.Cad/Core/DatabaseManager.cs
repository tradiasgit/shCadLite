using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public class DatabaseManager
    {
        private Database _db;
        protected Database Database { get { if (_db != null) return _db; else return HostApplicationServices.WorkingDatabase; } set { _db = value; } }

        public static ObjectId GetObjectIdByHandle(string handle)
        {
            var db = Application.DocumentManager.MdiActiveDocument.Database;
            var obj = ObjectId.Null;
            try
            {
                var h = new Handle(long.Parse(handle, System.Globalization.NumberStyles.AllowHexSpecifier));
                if (db.TryGetObjectId(h, out obj))
                {
                    if (!obj.IsErased)
                    {
                        return obj;
                    }
                }
            }
            catch
            {
                return ObjectId.Null;
            }
            return ObjectId.Null;
        }



        public static void ExportBlock(FileInfo targetDwgfile, string blockName)
        {

            //var doc = Application.DocumentManager.MdiActiveDocument;

            var sourceDb = HostApplicationServices.WorkingDatabase;
            var blockIds = new ObjectIdCollection();
            using (var tr = sourceDb.TransactionManager.StartTransaction())
            {
                using (var bt = (BlockTable)tr.GetObject(sourceDb.BlockTableId, OpenMode.ForRead, false))
                {
                    if (bt.Has(blockName))
                    {
                        blockIds.Add(bt[blockName]);
                    }
                }
            }
            if (blockIds.Count != 0)
            {
                using (var targetdb = new Database(true, true))
                {
                    sourceDb.WblockCloneObjects(blockIds, targetdb.BlockTableId, new IdMapping(), DuplicateRecordCloning.Ignore, false);
                    targetdb.SaveAs(targetDwgfile.FullName, DwgVersion.Current);
                }

            }
        }
        public static void PutBlockTo0(string blockName)
        {

            var doc = Application.DocumentManager.MdiActiveDocument;
            using (doc.LockDocument())
            {
                var sourceDb = HostApplicationServices.WorkingDatabase;
                var blockIds = new ObjectIdCollection();
                using (var tr = sourceDb.TransactionManager.StartTransaction())
                {
                    using (var bt = (BlockTable)tr.GetObject(sourceDb.BlockTableId, OpenMode.ForRead, false))
                    {
                        if (bt.Has(blockName))
                        {
                            var btr = tr.GetObject(bt[blockName], OpenMode.ForRead) as BlockTableRecord;
                            foreach (var oid in btr)
                            {
                                var ent = tr.GetObject(oid, OpenMode.ForWrite) as Entity;
                                if (ent != null) ent.Layer = "0";
                            }
                            tr.Commit();
                        }
                    }
                }
            }
        }
        public static void SetBlockBasePoint(ObjectId brid)
        {
            var sourceDb = HostApplicationServices.WorkingDatabase;
            var doc = Application.DocumentManager.MdiActiveDocument;


            using (doc.LockDocument())
            {
                using (var tr = sourceDb.TransactionManager.StartTransaction())
                {
                    var br = tr.GetObject(brid, OpenMode.ForWrite) as BlockReference;
                    if (br.Rotation != 0 || br.ScaleFactors != new Scale3d(1, 1, 1)) throw new Exception("块参照比例不为1或角度不为0，无法变换");

                    if (br == null) return;

                    var r = doc.Editor.GetPoint("选择新的基点");
                    if (r.Status == PromptStatus.OK)
                    {
                        var newpoint = r.Value;
                        var oldpoint = br.Position;
                        var blockIds = new ObjectIdCollection();
                        using (var bt = (BlockTable)tr.GetObject(sourceDb.BlockTableId, OpenMode.ForRead, false))
                        {
                            if (bt.Has(br.Name))
                            {
                                var btr = tr.GetObject(bt[br.Name], OpenMode.ForRead) as BlockTableRecord;

                                foreach (var oid in btr)
                                {
                                    var ent = tr.GetObject(oid, OpenMode.ForWrite) as Entity;
                                    if (ent != null) ent.TransformBy(Matrix3d.Displacement(oldpoint - newpoint));
                                }

                                var coll = btr.GetBlockReferenceIds(false, false);
                                foreach (ObjectId oid in coll)
                                {
                                    var ent = tr.GetObject(oid, OpenMode.ForWrite) as Entity;
                                    if (ent != null) ent.TransformBy(Matrix3d.Displacement(newpoint - oldpoint));
                                }
                                tr.Commit();
                            }
                        }
                    }
                }
            }
        }

        public static bool HasBlock(string blockName)
        {
            Database db = HostApplicationServices.WorkingDatabase;  //当前文档数据库
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            using (DocumentLock acLock = acDoc.LockDocument())
            {
                return HasBlock(blockName, db);
            }
        }

        private static bool HasBlock(string blockName, Database db)
        {
            using (Transaction transaction = db.TransactionManager.StartTransaction())
            {
                //已读的形势打开块表
                var table = (BlockTable)db.BlockTableId.GetObject(OpenMode.ForRead);
                return table.Has(blockName);
            }
        }

        public static bool HasBlock(FileInfo sourcefile, string blockName)
        {
            using (Database db = OpenDataBase(sourcefile))
            {
                return HasBlock(blockName, db);
            }
        }

        private static Database OpenDataBase(FileInfo file)
        {
            if (!file.Exists)
            {
                throw new Exception("该文件不存在：" + file);
            }
            else
            {
                //创建新数据库对象 源数据库
                var sourceDb = new Database(false, true);
                //读取文件
                sourceDb.ReadDwgFile(file.FullName, FileShare.Read, true, "");
                return sourceDb;
            }

        }



        public static void ImportBlock(FileInfo sourceDwgfile, string sourceBlockName)
        {

            var document = Application.DocumentManager.MdiActiveDocument;

            if (sourceDwgfile == null || !sourceDwgfile.Exists)
            {
                throw new Exception("该文件不存在：" + sourceDwgfile);
            }
            else
            {
                //创建新数据库对象 源数据库
                using (var sourceDb = new Database(false, true))
                {
                    //读取文件
                    sourceDb.ReadDwgFile(sourceDwgfile.FullName, FileShare.Read, true, "");
                    //集合
                    var blockIds = new ObjectIdCollection();
                    using (var tran = sourceDb.TransactionManager.StartTransaction())
                    {
                        //读取源数据库
                        using (var bt = (BlockTable)tran.GetObject(sourceDb.BlockTableId, OpenMode.ForRead, false))
                        {
                            if (bt.Has(sourceBlockName))
                            {
                                blockIds.Add(bt[sourceBlockName]);
                            }
                        }
                    }
                    if (blockIds.Count != 0)
                    {
                        using (var doclock = Application.DocumentManager.MdiActiveDocument.LockDocument())
                        {
                            //从源数据库复制到当先数据库
                            sourceDb.WblockCloneObjects(blockIds, document.Database.BlockTableId, new IdMapping(), DuplicateRecordCloning.Ignore, false);



                        }
                    }
                }
            }
        }


        public static ObjectId CreateBlankBlock(string name)
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
        public static void RemoveBlankBlock(string name)
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


        public static void RenameBlock(string oldName, string newName)
        {
            if (oldName == newName) return;
            var document = Application.DocumentManager.MdiActiveDocument;
            using (document.LockDocument())
            {
                using (var tran = document.Database.TransactionManager.StartTransaction())
                {
                    //读取源数据库
                    using (var bt = (BlockTable)tran.GetObject(document.Database.BlockTableId, OpenMode.ForRead, false))
                    {
                        if (bt.Has(oldName) && !bt.Has(newName))
                        {
                            var btr = tran.GetObject(bt[oldName], OpenMode.ForWrite) as BlockTableRecord;
                            btr.Name = newName;
                            tran.Commit();
                        }
                        else
                        {
                            throw new Exception("【异常】块重命名失败，块重名或没有找到");
                        }
                    }
                }
            }
        }

        public static void ForceExpold(BlockReference ent)
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

        public static void ExplodBlockReference(BlockReference ent)
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

        public static void ImportModelSpaceToBlock(FileInfo file, string blockName)
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
        }





        public static void DrawHatch(HatchInfo info)
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = HostApplicationServices.WorkingDatabase;
            var ed = doc.Editor;
            using (doc.LockDocument())
            {
                var op = new PromptPointOptions(Environment.NewLine + "请在要填充的区域内部选择一点");
                op.AllowNone = true;
                var result = ed.GetPoint(op);
                if (result.Status == PromptStatus.OK)
                {
                    var lines = ed.TraceBoundary(result.Value, true).Cast<Polyline>();
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        var space = (BlockTableRecord)tr.GetObject(doc.Database.CurrentSpaceId, OpenMode.ForWrite);
                        using (var ent = new Hatch())
                        {
                            ent.SetDatabaseDefaults();
                            ent.SetHatchPattern((HatchPatternType)Enum.Parse(typeof(HatchPatternType), info.PatternType), info.PatternName);
                            ent.PatternScale = info.PatternScale;
                            ent.PatternAngle = info.PatternAngle * (Math.PI / 180);  //角度转弧度 度数 * (π / 180） = 弧度
                            ent.PatternSpace = info.PatternSpace;
                            ent.PatternDouble = info.PatternDouble;
                            ent.HatchStyle = (HatchStyle)Enum.Parse(typeof(HatchStyle), info.HatchStyle);
                            ent.ColorIndex = info.ColorIndex;
                            ent.Associative = info.Associative;
                            LayerManager.SetLayer(ent, info.LayerName, tr);
                            foreach (var one in lines)
                            {
                                var id = space.AppendEntity(one);
                                tr.AddNewlyCreatedDBObject(one, true);
                                ent.AppendLoop(HatchLoopTypes.Outermost, new ObjectIdCollection() { id });
                            }
                            ent.EvaluateHatch(true);
                            space.AppendEntity(ent);
                            tr.AddNewlyCreatedDBObject(ent, true);

                            foreach (var one in lines)
                            {
                                one.Erase();
                            }

                            tr.Commit();
                            DataManager.WriteDictionary(ent.Id, info.Data);
                        }
                    }
                }
            }
        }


        public static Point3d? CopyAllEntity(FileInfo file, Point3d basepoint)
        {
            Point3d? result = null;
            if (!file.Exists) throw new FileNotFoundException("找不到文件", file.FullName);
            var db = HostApplicationServices.WorkingDatabase;
            var doc = Application.DocumentManager.MdiActiveDocument;
            using (doc.LockDocument())
            {
                var blockName = Guid.NewGuid().ToString();
                var importObjectIds = new ObjectIdCollection();
                var layoutIds = new ObjectIdCollection();
                using (var sourceDb = new Database(false, true))
                {
                    if (file.Extension.ToLower() == ".dwg")
                        sourceDb.ReadDwgFile(file.FullName, FileShare.Read, true, "");
                    else if (file.Extension.ToLower() == ".dxf")
                        sourceDb.DxfIn(file.FullName, $@"{Environment.CurrentDirectory}\logs\importdxf\{file.Name}.log");
                    else throw new Exception("不能识别的文件类型：" + file.FullName);
                    using (var tran = sourceDb.TransactionManager.StartTransaction())
                    {
                        //读取源数据库
                        using (var bt = (BlockTable)tran.GetObject(sourceDb.BlockTableId, OpenMode.ForRead, false))
                        {

                            BlockTableRecord btr = (BlockTableRecord)tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead, false);
                            foreach (ObjectId id in btr)
                            {
                                var ent = tran.GetObject(id, OpenMode.ForRead) as Entity;
                                if (ent == null) continue;
                                importObjectIds.Add(id);
                            }
                        }
                    }
                    using (var tr = db.TransactionManager.StartTransaction())
                    {
                        using (var bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForWrite, false))
                        {
                            using (var btr = new BlockTableRecord())
                            {
                                btr.Name = blockName;
                                btr.Explodable = true;
                                bt.Add(btr);
                                tr.AddNewlyCreatedDBObject(btr, true);                                
                                sourceDb.WblockCloneObjects(importObjectIds, btr.Id, new IdMapping(), DuplicateRecordCloning.Replace, false);
                                foreach (var oid in btr)
                                {
                                    var ent = tr.GetObject(oid, OpenMode.ForWrite) as Entity;
                                    if (ent != null)
                                    {
                                        var mtx = Matrix3d.Displacement(Point3d.Origin- basepoint);
                                        ent.TransformBy(mtx);
                                    }
                                }
                                tr.Commit();
                            }
                        }
                    }
                }
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    using (var ent = new BlockReference(Point3d.Origin, bt[blockName]))
                    {
                        var space = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                        space.AppendEntity(ent);
                        tr.AddNewlyCreatedDBObject(ent, true);
                        var jig = new Jig.BlockReferenceJig(ent);
                        if (jig.Run())
                        {
                            ent.ExplodeToOwnerSpace();
                            var e = tr.GetObject(ent.Id, OpenMode.ForWrite);
                            ent.Erase();
                            var b = tr.GetObject(bt[blockName], OpenMode.ForWrite);
                            b.Erase();
                            tr.Commit();
                            result = ent.Position;
                        }
                    }
                }

            }
            return result;
        }

        public static List<string> GetLayoutNames(FileInfo file)
        {
            var result = new List<string>();
            if (file == null && !file.Exists || file.Extension.ToLower() != ".dwg") return result;
            Database sourceDb = new Database(false, true);
            sourceDb.ReadDwgFile(file.FullName, FileOpenMode.OpenForReadAndAllShare, true, "");
            using (Transaction tr = sourceDb.TransactionManager.StartTransaction())
            {
                DBDictionary lays = tr.GetObject(sourceDb.LayoutDictionaryId, OpenMode.ForRead) as DBDictionary;
                foreach (DBDictionaryEntry item in lays)
                {
                    result.Add(item.Key);
                }
                tr.Abort();
            }
            return result;
        }













    }
}
