using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
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


        public static  bool HasBlock(string blockName)
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

            if (sourceDwgfile==null||!sourceDwgfile.Exists)
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
    }
}
