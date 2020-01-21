using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public class BlockInfo : EntityInfo
    {

        public BlockInfo() { }

        public BlockInfo(BlockReference ent, Transaction tr) : base(ent, tr)
        {
            IsDynamicBlock = ent.IsDynamicBlock;
            if (ent.IsDynamicBlock)
            {
                var btr = tr.GetObject(ent.DynamicBlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
                foreach (DynamicBlockReferenceProperty p in ent.DynamicBlockReferencePropertyCollection)
                {
                    if (p.PropertyTypeCode == 5 && p.Value is string)
                    {
                        BlockVisibilityName = (string)p.Value;
                        break;
                    }
                }
                BlockName = btr.Name;
            }
            else
                BlockName = ent.Name;
        }


        public string BlockName { get; set; }

        public string BlockVisibilityName { get; set; }

        public bool IsDynamicBlock { get; set; }


        protected override bool Compare_Property(Entity ent, Transaction tr)
        {
            var br = ent as BlockReference;
            if (br.IsDynamicBlock)
            {
                var dyblock = tr.GetObject(br.DynamicBlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
                if (BlockName != dyblock.Name) return false;
                if (!string.IsNullOrWhiteSpace(BlockVisibilityName))
                {
                    foreach (DynamicBlockReferenceProperty p in br.DynamicBlockReferencePropertyCollection)
                    {
                        if (p.PropertyTypeCode == 5 && p.Value is string && (string)p.Value == BlockVisibilityName) return true;
                    }
                    return false;
                }
            }
            else if (BlockName != br.Name) return false;
            return true;
        }




        public override void SaveAs()
        {
            var dwgtitled = Application.GetSystemVariable("DWGTITLED");
            if (Convert.ToInt16(dwgtitled) == 0)
            {
                throw new Exception("图纸为临时图纸，不能保存");
            }

            var ed = Application.DocumentManager.MdiActiveDocument.Editor;
            var db_source = HostApplicationServices.WorkingDatabase;
            var dir = new FileInfo(db_source.OriginalFileName).Directory;
            dir = new DirectoryInfo($@"{dir.FullName}\support");
            dir.Create();
            var op_file = new PromptSaveFileOptions("选择目标文件" + Environment.NewLine);
            op_file.InitialDirectory = $@"{dir.FullName}";

            op_file.InitialFileName = $"{BlockName}.enf";
            op_file.Filter = "图形配置文件-json格式 (*.enf)|*.enf";

            var result_file = ed.GetFileNameForSave(op_file);
            if (result_file.Status == PromptStatus.OK)
            {
                if (BlockName != null)
                {
                    var file = new FileInfo(result_file.StringResult.Replace(".enf", "_block.dwg"));
                    var doc = Application.DocumentManager.MdiActiveDocument;
                    using (doc.LockDocument())
                    {
                        var selection = ed.SelectImplied();
                        var ids = selection.Value.GetObjectIds();
                        using (var tr_source = db_source.TransactionManager.StartTransaction())
                        {
                            var br = tr_source.GetObject(selection.Value[0].ObjectId, OpenMode.ForRead) as BlockReference;
                            var mtx = Matrix3d.Displacement(Point3d.Origin - br.Position);

                            var targetObjectIds = new ObjectIdCollection(ids);
                            IdMapping mapping = new IdMapping();
                            using (var db_target = new Database(true, true))
                            {
                                using (var tr_target = db_target.TransactionManager.StartTransaction())
                                {
                                    var targetid = ((BlockTable)tr_target.GetObject(db_target.BlockTableId, OpenMode.ForRead))[BlockTableRecord.ModelSpace];
                                    db_source.WblockCloneObjects(targetObjectIds, targetid, mapping, DuplicateRecordCloning.Replace, false);

                                    var btr = tr_target.GetObject(targetid, OpenMode.ForRead) as BlockTableRecord;
                                    foreach (var oid in btr)
                                    {
                                        var ent = tr_target.GetObject(oid, OpenMode.ForWrite) as Entity;
                                        if (ent != null)
                                        {
                                            ent.TransformBy(mtx);
                                        }
                                    }

                                    tr_target.Commit();
                                }
                                if (file.FullName.ToLower().EndsWith("dxf"))
                                    db_target.DxfOut(file.FullName, 16, DwgVersion.Current);
                                else
                                    db_target.SaveAs(file.FullName, DwgVersion.Current);
                            }
                            tr_source.Abort();

                        }
                    }

                }
                File.WriteAllText(result_file.StringResult, JsonConvert.SerializeObject(this));
            }
        }

        public override void Draw()
        {
            using (var l = Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                var db = HostApplicationServices.WorkingDatabase;
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    if (!bt.Has(BlockName)) return;
                    using (var ent = new BlockReference(Point3d.Origin, bt[BlockName]))
                    {
                        if (LayerName != null)
                            LayerManager.SetLayer(db, ent, LayerName, true);

                        var space = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                        space.AppendEntity(ent);
                        tr.AddNewlyCreatedDBObject(ent, true);
                        var jig = new Jig.BlockReferenceJig(ent as BlockReference);
                        if (jig.Run())
                        {
                            tr.Commit();
                            DataManager.WriteDictionary(ent.ObjectId, Data);
                        }
                    }
                }
            }
        }







    }
}
