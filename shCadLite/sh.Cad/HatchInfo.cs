using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public class HatchInfo : EntityInfo
    {
        public HatchInfo() { }

        public HatchInfo(Hatch ent, Transaction tr) : base(ent, tr)
        {
            var h = ent as Hatch;
            PatternAngle = h.PatternAngle;
            PatternDouble = h.PatternDouble;
            PatternName = h.PatternName;
            PatternScale = h.PatternScale;
            PatternSpace = h.PatternSpace;
            PatternType = h.PatternType.ToString();
            Associative = h.Associative;
            HatchStyle = h.HatchStyle.ToString();
        }


        public double PatternScale { get; set; } = 1;

        /// <summary>
        /// 填充角度
        /// </summary>
        public double PatternAngle { get; set; }

        /// <summary>
        /// CAD预设样式名称
        /// </summary>
        public string PatternName { get; set; } = "SOLID";

        public string HatchStyle { get; set; } = "Normal";

        public bool Associative { get; set; } = false;

        /// <summary>
        /// 类型
        /// </summary>
        public string PatternType { get; set; } = "PreDefined";

        /// <summary>
        /// 间隙
        /// </summary>
        public double PatternSpace { get; set; } = 1;

        /// <summary>
        /// 双向
        /// </summary>
        public bool PatternDouble { get; set; }

        //public Point2d Origin { get; set; }

        public override string ToString()
        {
            if (PatternType == "UserDefined") return $"用户定义:{PatternSpace}" + (PatternDouble ? "-双向" : "");
            else if (PatternType == "CustomDefined") return $"自定义:{PatternName}";
            else if (PatternType == "PreDefined") return $"预定义:{PatternName}";
            else return "未知";
        }


        protected override bool Compare_Property(Entity ent, Transaction tr)
        {
            var h = ent as Hatch;
            if (h == null) return false;
            if (PatternType != h.PatternType.ToString()) return false;
            if (h.PatternType == HatchPatternType.UserDefined && PatternSpace == h.PatternSpace) return true;
            if (PatternScale != h.PatternScale) return false;
            if (PatternName != h.PatternName) return false;
            return true;
        }
        protected override void BrushEntity_Property(Entity ent, Transaction tr)
        {
            var h = ent as Hatch;
            if (h == null) return;
            HatchPatternType type;
            Enum.TryParse(PatternType, out type);
            h.SetHatchPattern(type, PatternName);
            h.PatternScale = PatternScale;
            h.PatternAngle = PatternAngle;
            h.HatchStyle = Autodesk.AutoCAD.DatabaseServices.HatchStyle.Normal;
            if (h.PatternType == HatchPatternType.UserDefined)
            {
                h.PatternSpace = PatternSpace;
                h.PatternDouble = PatternDouble;
            }
        }


        public override void Draw()
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
                            ent.SetHatchPattern((HatchPatternType)Enum.Parse(typeof(HatchPatternType), PatternType), PatternName);
                            ent.PatternScale = PatternScale;
                            ent.PatternAngle = PatternAngle * (Math.PI / 180);  //角度转弧度 度数 * (π / 180） = 弧度
                            ent.PatternSpace = PatternSpace;
                            ent.PatternDouble = PatternDouble;
                            ent.HatchStyle = (HatchStyle)Enum.Parse(typeof(HatchStyle), HatchStyle);
                            ent.ColorIndex = ColorIndex;
                            ent.Associative = Associative;
                            LayerManager.SetLayer(ent, LayerName, tr);
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
                            DataManager.WriteDictionary(ent.Id, Data);
                        }
                    }
                }
            }
        }


    }
}
