using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace sh.Cad
{
    public class EntityInfo
    {
        private EntityInfo(ObjectId oid)
        {
            _oid = oid;
            var db = HostApplicationServices.WorkingDatabase;
            using (var tr = db.TransactionManager.StartOpenCloseTransaction())
            {
                ent = tr.GetObject(_oid, OpenMode.ForRead) as Entity;
            }
        }

        Entity ent;

        public EntityInfo(Entity ent)
        {
            _oid = ent.ObjectId;
            this.ent = ent;

        }
        private ObjectId _oid;

        public string LayerName { get { return ent.Layer; } }
        public string DxfName { get { return _oid.ObjectClass.DxfName; } }

        public string ClassName { get { return _oid.ObjectClass.Name; } }

        public string TypeName { get { return ent.GetType().Name; } }


        public Dictionary<string, string> GetData()
        {
            var datamanger = new DataManager();
            return datamanger.ReadDictionary(ent);
        }

        public double GetArea()
        {
            if (ent == null) return 0;
            var prop = ent.GetType().GetProperty("Area");
            if (prop != null)
            {
                try
                {
                    return (double)prop.GetValue(ent);
                }
                catch (Exception ex)
                {
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($@"1个对象获取面积失败：{ent.GetType().Name},{ex.Message}");
                }
            }
            //WriteLine($"总面积:{value}{Environment.NewLine}");
            return -1;
        }

        public double GetLength()
        {
            if (ent == null) return 0;

            if (ent is Mline) return GetMlineLength(ent);
            else
            {
                var prop = ent.GetType().GetProperty("Length");
                if (prop != null)
                {
                    try
                    {
                        return (double)prop.GetValue(ent);
                    }
                    catch (Exception ex)
                    {
                        Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($@"1个对象获取长度失败：{ent.GetType().Name},{ex.Message}");
                    }
                }
            }
            return -1;
        }
        private double GetMlineLength(Entity ent)
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


        public bool IsHatch { get { return ent.GetType() == typeof(Hatch); } }

        public HacthStyle GetHatch()
        {
            if (IsHatch)
            {
                var h = ent as Hatch;
                return new HacthStyle
                {
                    PatternAngle = h.PatternAngle,
                    PatternDouble = h.PatternDouble,
                    PatternName = h.PatternName,
                    PatternScale = h.PatternScale,
                    PatternSpace = h.PatternSpace,
                    PatternType = h.PatternType,
                    Associative = h.Associative,
                    HatchStyle = h.HatchStyle,
                    Origin = h.Origin
                };
            }
            return null;
        }



        public void WriteData(string key, string value)
        {
            sh.Cad.DataManager.WriteDictionary(_oid, new Dictionary<string, string> { { key, value } });
        }

        public void RemoveData(string key)
        {
            sh.Cad.DataManager.RemoveKey(_oid, key);
        }

    }
}
