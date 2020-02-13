using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.Cad
{
    class EntityInfo_Cad : EntityInfo
    {
        internal EntityInfo_Cad(ObjectId oid,Transaction tr)
        {
            _oid = oid;
            var ent = tr.GetObject(oid, OpenMode.ForRead) as Entity;
            EntityTypeName = ent.GetType().Name;
            EntityHandle = ent.Handle.ToString();
            ColorIndex = ent.ColorIndex;
            LayerName = ent.Layer;
            Data = ent.ExReadDictionary(tr);
            Area = ent.ExGetArea();
            Length = ent.ExGetLength();
            if (ent is BlockReference) FillBlock(this, ent as BlockReference, tr);
            if (ent is Hatch) FillHatch(this, ent as Hatch, tr);
        }

        private ObjectId _oid;

        private static void FillBlock(EntityInfo info, BlockReference ent, Transaction tr)
        {
            info.IsDynamicBlock = ent.IsDynamicBlock;
            if (ent.IsDynamicBlock)
            {
                var btr = tr.GetObject(ent.DynamicBlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
                foreach (DynamicBlockReferenceProperty p in ent.DynamicBlockReferencePropertyCollection)
                {
                    if (p.PropertyTypeCode == 5 && p.Value is string)
                    {
                        info.BlockVisibilityName = (string)p.Value;
                        break;
                    }
                }
                info.BlockName = btr.Name;
            }
            else
                info.BlockName = ent.Name;
        }
        private static void FillHatch(EntityInfo info, Hatch ent, Transaction tr)
        {
            info.PatternAngle = ent.PatternAngle;
            info.PatternDouble = ent.PatternDouble;
            info.PatternName = ent.PatternName;
            info.PatternScale = ent.PatternScale;
            info.PatternSpace = ent.PatternSpace;
            info.PatternType = ent.PatternType.ToString();
            info.Associative = ent.Associative;
            info.HatchStyle = ent.HatchStyle.ToString();
        }


        public override void SetDataValue(string key, string value)
        {
            using (var tr = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction())
            {
                var ent = tr.GetObject(_oid, OpenMode.ForWrite) as Entity;
                ent.ExWriteDictionary(key, value);
            }
        }
    }
}
