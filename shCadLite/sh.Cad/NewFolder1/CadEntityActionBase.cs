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
    public abstract class CadEntityActionBase :ActionNode
    {
        public CadEntityActionBase(XmlElement ele) : base(ele)
        {
        }
        

        protected FileInfo CopyToSupportDirectory(FileInfo file)
        {
            if (file.Exists)
            {
                foreach (var dir in GetSupportPaths())
                {
                    var target = new FileInfo($@"{dir}\{file.Name}");
                    try
                    {
                        File.Copy(file.FullName, target.FullName, true);
                        return target;
                    }
                    catch (UnauthorizedAccessException e)//访问被拒绝，UAC没有以管理员模式运行
                    {
                        continue;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }
            }
            throw new Exception("所有Support路径都没有权限操作，请添加非C盘Support路径");
        }
        
        public string LayerName { get; set; }

        /// <summary>
        /// 查找文件
        /// 优先级：图纸目录>图纸目录\扩展名目录>图纸目录\support>图纸目录\support\扩展名目录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="extname"></param>
        /// <returns></returns>
        public static FileInfo FindSupportFile(string file, string subpath = null)
        {
            if (string.IsNullOrWhiteSpace(file)) return null;
            var dbfile = new FileInfo(HostApplicationServices.WorkingDatabase.OriginalFileName);
            List<string> locations = new List<string>();
            //locations.Add($@"{dbfile.DirectoryName}");
            locations.Add($@"{dbfile.DirectoryName}\support");

            var dotindex = file.LastIndexOf('.');
            if (dotindex > 0 && dotindex < file.Length)
            {
                var ext = file.Substring(dotindex + 1);
                locations.Add($@"{dbfile.DirectoryName}\{ext}");
                locations.Add($@"{dbfile.DirectoryName}\support\{ext}");

            }

            if (!string.IsNullOrWhiteSpace(subpath))
            {
                locations.Add($@"{dbfile.DirectoryName}\{subpath}");
                locations.Add($@"{dbfile.DirectoryName}\support\{subpath}");
            }
            foreach (var f in locations)
            {
                var result = new FileInfo($@"{f}\{file}");
                if (result.Exists) return result;
            }
            return null;
        }

        public static FileInfo FindSupportFile(IEnumerable<string> files)
        {
            foreach (var f in files)
            {
                var r = FindSupportFile(f);
                if (r != null && r.Exists) return r;
            }
            return null;
        }

        protected List<DirectoryInfo> GetSupportPaths()
        {
            dynamic acadObject = Application.AcadApplication;
            dynamic prefences = acadObject.Preferences;
            dynamic files = prefences.Files;
            var path = files.SupportPath;
            return ((string)path).Split(';').Select(p => new DirectoryInfo(p)).ToList();

        }


        private void addSupportPath(string path)
        {
            dynamic acadObject = Application.AcadApplication;
            dynamic prefences = acadObject.Preferences;
            dynamic files = prefences.Files;
            if (files.SupportPath.Contains(path)) return;
            files.SupportPath += $";{path}";
        }

        protected static T ParseString<T>(string value, T defaultvalue = default(T))
        {
            object v = defaultvalue;
            if (typeof(T) == typeof(string))
                v = (string)value;
            else if (typeof(T) == typeof(Guid))
            {
                Guid vv;
                if (Guid.TryParse(value, out vv)) v = vv;
            }
            else if (typeof(T) == typeof(int))
            {
                int vv;
                if (int.TryParse(value, out vv)) v = vv;
            }
            else if (typeof(T) == typeof(short))
            {
                short vv;
                if (short.TryParse(value, out vv)) v = vv;
            }
            else if (typeof(T) == typeof(double))
            {
                double vv;
                if (double.TryParse(value, out vv)) v = vv;
            }
            else if (typeof(T) == typeof(bool))
            {
                bool vv;
                if (bool.TryParse(value, out vv)) v = vv;
            }
            else if (typeof(T) == typeof(LineWeight))
            {
                LineWeight vv;
                if (Enum.TryParse(value, out vv)) v = vv;
            }
            return (T)v;
        }

        protected static void SetLayer(Database db, Entity ent, string layerName, bool autoCreate = false)
        {
            if (string.IsNullOrWhiteSpace(layerName)) return;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                using (LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForWrite))
                {
                    if (!lt.Has(layerName))
                    {
                        if (autoCreate)
                        {
                            using (LayerTableRecord ltr = new LayerTableRecord())
                            {
                                ltr.Name = layerName;
                                var layerfile = FindSupportFile($@"{layerName}.xml", "Layers");
                                string des = null;
                                if (layerfile!=null)
                                {
                                    try
                                    {
                                        XmlDocument doc = new XmlDocument();
                                        doc.Load(layerfile.FullName);
                                        var node = doc.SelectSingleNode("/Layer");
                                        foreach (XmlAttribute att in node.Attributes)
                                        {

                                            switch (att.Name)
                                            {
                                                case "ColorIndex": ltr.Color = Color.FromColorIndex(ColorMethod.ByAci, short.Parse(att.Value)); break;
                                                case "Description": des = att.Value; break;
                                                case "IsPlottable": ltr.IsPlottable = ParseString(att.Value, true); break;
                                                case "ViewportVisibilityDefault": ltr.ViewportVisibilityDefault = ParseString(att.Value, false); break;
                                                case "LineWeight": ltr.LineWeight = ParseString(att.Value, LineWeight.ByLineWeightDefault); break;
                                                case "Linetype": ltr.LinetypeObjectId = SetLinetype(att.Value, "Continuous"); break;
                                                case "IsFrozen": ltr.IsFrozen = ParseString(att.Value, false); break;
                                                case "IsHidden": ltr.IsHidden = ParseString(att.Value, false); break;
                                                case "IsOff": ltr.IsOff = ParseString(att.Value, false); break;
                                                case "IsLocked": ltr.IsLocked = ParseString(att.Value, false); break;
                                                    //case "IsReconciled": ltr.IsReconciled = ParseString(att.Value, true); break;//不能在此设置
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        WriteLine($"读取图层设置文件失败，图层：{layerName}；文件：{layerfile.FullName}；错误：{ex.Message}");
                                    }
                                }
                                lt.Add(ltr);
                                ltr.Description = des;//此字段必须在add之后写入
                                tr.AddNewlyCreatedDBObject(ltr, true);
                                tr.Commit();
                            }
                        }
                        else throw new Exception("不存在图层" + layerName);
                    }
                    else
                    {

                    }
                }
            }
            ent.Layer = layerName;
        }


        protected static ObjectId SetLinetype(string typeName, string defaultValue = "ByLayer")
        {
            var db = HostApplicationServices.WorkingDatabase;
            //打开线型表
            var lt = (LinetypeTable)db.LinetypeTableId.GetObject(OpenMode.ForRead);
            if (lt.Has(typeName)) return lt[typeName]; //返回加载的线型的ObjectId
            try
            {
                var path = HostApplicationServices.Current.FindFile("acad.lin", db, FindFileHint.Default);
                db.LoadLineTypeFile(typeName, path);
                return lt[typeName];
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                return lt[defaultValue];
            }
        }

        protected static void Write(string message)
        {
            Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(message);
        }
        protected static void WriteLine(string message)
        {
            Write(Environment.NewLine + message + Environment.NewLine);
        }

        protected static void RemoveBlock(string name)
        {
            var db = HostApplicationServices.WorkingDatabase;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                using (var bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForWrite, false))
                {
                    if (bt.Has(name))
                    {
                        var btr = (BlockTableRecord)tr.GetObject(bt[name], OpenMode.ForWrite);
                        //TODO:需要删除所有块参照，所有相关引用，如果是动态快，还需要删除匿名块，临时手动保证
                        //var referenceIds = btr.GetBlockReferenceIds(true,true);
                        //if (btr.IsDynamicBlock)
                        //{
                        //    ObjectIdCollection anonymousIds = btr.GetAnonymousBlockIds();
                        //    ObjectIdCollection dynBlockRefs = new ObjectIdCollection();
                        //    foreach (ObjectId anonymousBtrId in anonymousIds)
                        //    {
                        //        BlockTableRecord anonymousBtr = (BlockTableRecord)tr.GetObject(anonymousBtrId, OpenMode.ForRead);
                        //        ObjectIdCollection blockRefIds = anonymousBtr.GetBlockReferenceIds(true, true);
                        //        foreach (ObjectId id in blockRefIds)
                        //        {
                        //            dynBlockRefs.Add(id);
                        //        }
                        //    }
                        //}
                        btr.Erase();
                        tr.Commit();
                    }
                }
            }
        }
        protected static void ExplodeBlockReference(BlockReference ent)
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


        protected static bool ImportToBlock(FileInfo file, string blockName, string sourceblockname = null)
        {
            //var resultObjectIdList = new List<ObjectId>();
            //var logfile = new FileInfo($@"{file.DirectoryName}\logs\importdxf\{file.Name}.log");
            var logfile = new FileInfo($@"{Environment.CurrentDirectory}\logs\importdxf\{file.Name}.log");
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                var importObjectIds = new ObjectIdCollection();
                IdMapping mapping = new IdMapping();
                using (var sourceDb = new Database(false, true))
                {
                    if (file.Extension.ToLower() == ".dwg")
                    {
                        sourceDb.ReadDwgFile(file.FullName, FileShare.Read, true, "");
                    }
                    else if (file.Extension.ToLower() == ".dxf")
                    {
                        logfile.Directory.Create();
                        sourceDb.DxfIn(file.FullName, logfile.FullName);
                    }
                    else throw new Exception("不能识别的文件类型：" + file.FullName);
                    using (var tr = sourceDb.TransactionManager.StartTransaction())
                    {
                        //读取源数据库
                        using (var bt = (BlockTable)tr.GetObject(sourceDb.BlockTableId, OpenMode.ForRead, false))
                        {
                            if (string.IsNullOrWhiteSpace(sourceblockname)) sourceblockname = BlockTableRecord.ModelSpace;
                            if (!bt.Has(sourceblockname)) throw new Exception($"文件不含有块：{sourceblockname}({file.FullName})");
                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[sourceblockname], OpenMode.ForRead, false);
                            foreach (ObjectId id in btr)
                            {
                                var ent = tr.GetObject(id, OpenMode.ForRead) as Entity;
                                if (ent == null) continue;
                                importObjectIds.Add(id);
                            }
                        }
                    }
                    var db = HostApplicationServices.WorkingDatabase;
                    var targetid = ObjectId.Null;
                    using (var doclock = Application.DocumentManager.MdiActiveDocument.LockDocument())
                    {
                        using (var tr = db.TransactionManager.StartTransaction())
                        {
                            //读取源数据库
                            using (var bt = (BlockTable)tr.GetObject(sourceDb.BlockTableId, OpenMode.ForRead, false))
                            {
                                if (bt.Has(blockName))
                                {
                                    throw new Exception("块已存在，无法导入");
                                }
                                else
                                {
                                    var btr = new BlockTableRecord();
                                    btr.Name = blockName;
                                    btr.Explodable = true;
                                    bt.Add(btr);
                                    tr.AddNewlyCreatedDBObject(btr, true);
                                    tr.Commit();
                                    targetid=btr.Id;
                                }
                            }
                        }
                        sourceDb.WblockCloneObjects(importObjectIds, targetid, mapping, DuplicateRecordCloning.Replace, false);
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                ed.WriteMessage("\n导入出现错误，" + ex.Message);
            }
            return false;
        }

        private static bool HasBlock(string blockName, Database db=null)
        {
            if (db == null) db = HostApplicationServices.WorkingDatabase;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId,OpenMode.ForRead);
                return bt.Has(blockName);
            }
        }

        protected static void WriteDictionary(ObjectId oid, Dictionary<string, string> data, bool clear = false)
        {
            if (data != null && data.Any())
            {
                using (var locker = Application.DocumentManager.MdiActiveDocument.LockDocument())
                {
                    using (var tr = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction())
                    {
                        var obj = tr.GetObject(oid, OpenMode.ForWrite);
                        if (!obj.ExtensionDictionary.IsValid)
                        {
                            //创建扩展字典
                            obj.CreateExtensionDictionary();
                        }
                        using (var objdict = (DBDictionary)tr.GetObject(obj.ExtensionDictionary, OpenMode.ForWrite, false))
                        {
                            if (clear)
                            {
                                foreach (var k in objdict)
                                {
                                    objdict.Remove(k.Key);
                                }
                            }

                            foreach (var kv in data)
                            {
                                Xrecord xrecord = new Xrecord() { Data = new ResultBuffer(new TypedValue((int)DxfCode.Text, kv.Value)) };
                                objdict.SetAt(kv.Key, xrecord);
                                tr.AddNewlyCreatedDBObject(xrecord, true);
                                xrecord.Dispose();
                                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($@"【{kv.Key}】{kv.Value}{Environment.NewLine}");
                            }
                        }
                        tr.Commit();
                    }
                }
            }
        }
    }
}
