using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad.Jig
{
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
