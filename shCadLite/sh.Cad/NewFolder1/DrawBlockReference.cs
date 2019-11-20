using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using System.IO;
using System.Xml;

namespace sh.XmlResourcesParsing.Models
{

    public class DrawBlockReference : CadEntityFactoryAction
    {
        public DrawBlockReference(XmlElement ele) : base(ele)
        {
        }

        public string BlockName { get; set; }
        public string SourceBlockName { get; set; }

        public string Mode { get; set; } = "Default";//Continuous


        private static bool HasBlock(string blockName, Database db)
        {
            using (var tran = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)db.BlockTableId.GetObject(OpenMode.ForRead);
                return bt.Has(blockName);
            }
        }

        protected override Entity CreateEntity()
        {
            var db = HostApplicationServices.WorkingDatabase;
            var doc = Application.DocumentManager.MdiActiveDocument;

            if (string.IsNullOrWhiteSpace(SourceBlockName)) SourceBlockName = BlockName;

            if (!HasBlock(SourceBlockName, db))
            {
                var file = FindSupportFile($@"{SourceBlockName}.dwg");
                if (!file.Exists)
                {
                    file = FindSupportFile($@"{SourceBlockName}.dxf");
                }

                if (file.Exists)
                {
                    using (var sourceDb = new Database(false, true))
                    {
                        if (file.Extension.ToLower() == ".dwg")
                            sourceDb.ReadDwgFile(file.FullName, FileShare.Read, true, "");
                        else if (file.Extension.ToLower() == ".dxf")
                            sourceDb.DxfIn(file.FullName, $@"{new FileInfo(db.OriginalFileName).DirectoryName}\logs\importdxf\{SourceBlockName}.log");
                        else throw new Exception("不能识别的文件类型：" + file.FullName);
                        var blockIds = new ObjectIdCollection();
                        using (var tr = sourceDb.TransactionManager.StartTransaction())
                        {
                            using (var bt = (BlockTable)tr.GetObject(sourceDb.BlockTableId, OpenMode.ForRead, false))
                            {
                                if (bt.Has(SourceBlockName))
                                {
                                    blockIds.Add(bt[SourceBlockName]);
                                }
                            }
                        }
                        if (blockIds.Count != 0)
                        {
                            using (var doclock = doc.LockDocument())
                            {
                                sourceDb.WblockCloneObjects(blockIds, db.BlockTableId, new IdMapping(), DuplicateRecordCloning.Ignore, false);
                            }
                        }
                    }
                }
                else
                    throw new Exception("找不到块或支持的块文件" + BlockName);
            }
            using (var tran = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)db.BlockTableId.GetObject(OpenMode.ForRead);

                if (bt.Has(BlockName))
                {
                    return new BlockReference(Point3d.Origin, bt[BlockName]);
                }
            }
            throw new Exception("找不到块" + BlockName);
        }


        protected override bool OnCommitting(Transaction tr, BlockTableRecord space, Entity ent)
        {
            return new BlockReferenceJig(ent as BlockReference).Run();
        }

        protected override void OnCommited(Entity ent)
        {
            base.OnCommited(ent);
            if (Mode == "Continuous")
                Execute();
        }

        internal class BlockReferenceJig : EntityJig
        {
            private Point3d _pos;
            //public bool ContinuousMode;
            private bool _pickingBasePoint;
            private Vector3d _basePointVector = new Vector3d();


            //private string ModeName { get { return ContinuousMode ? "连续" : "单件"; } }
            public BlockReferenceJig(BlockReference br) : base(br) { }
            protected override bool Update() { if (!_pickingBasePoint) ((BlockReference)Entity).Position = _pos + _basePointVector; return true; }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                var opts = new JigPromptPointOptions($"\n选择插入点():" + Environment.NewLine);
                opts.UserInputControls = UserInputControls.NullResponseAccepted;
                opts.BasePoint = Point3d.Origin;
                opts.Keywords.Add("a", "a", "左旋转90°(A)");
                opts.Keywords.Add("d", "d", "右旋转90°(D)");
                opts.Keywords.Add("q", "q", "垂直翻转(Q)");
                opts.Keywords.Add("e", "e", "水平翻转(E)");

                var ppr = prompts.AcquirePoint(opts);

                if (ppr.Status == PromptStatus.Cancel && ppr.Status == PromptStatus.Error)
                {
                    return SamplerStatus.Cancel;
                }
                else if (ppr.Status == PromptStatus.Keyword)
                {
                    switch (ppr.StringResult.ToUpper())
                    {
                        default: return SamplerStatus.NoChange;
                        case "A":
                            {
                                var btr = Entity as BlockReference;
                                btr.Rotation -= Math.PI / 2;
                                return SamplerStatus.OK;
                            }
                        case "D":
                            {
                                var btr = Entity as BlockReference;
                                btr.Rotation += Math.PI / 2;
                                return SamplerStatus.OK;
                            }
                        case "Q":
                            {
                                var btr = Entity as BlockReference;
                                var bp = ppr.Value;
                                btr.TransformBy(Matrix3d.Mirroring(new Line3d(bp, bp + Vector3d.XAxis)));
                                return SamplerStatus.OK;
                            }
                        case "E":
                            {
                                var btr = Entity as BlockReference;
                                var bp = ppr.Value;
                                btr.TransformBy(Matrix3d.Mirroring(new Line3d(bp, bp + Vector3d.YAxis)));
                                return SamplerStatus.OK;
                            }
                    }
                }
                else if (_pos == ppr.Value)
                {
                    return SamplerStatus.NoChange;
                }
                else
                {
                    _pos = ppr.Value;
                    return SamplerStatus.OK;
                }

            }


            public bool Run()
            {
                //while (true)
                //{
                var pr = Application.DocumentManager.MdiActiveDocument.Editor.Drag(this);
                switch (pr.Status)
                {
                    default: return false;
                    case PromptStatus.Keyword: break;
                    case PromptStatus.OK: return true;
                }
                return false;
                //}
            }
        }
    }

}
