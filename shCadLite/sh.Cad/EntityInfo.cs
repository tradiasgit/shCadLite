using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace sh.Cad
{
  

    public class EntityInfo
    {
        public string EntityTypeName { get;  set; }

        public int ColorIndex { get; set; } = 256;

        public string LayerName { get; set; }

        public Dictionary<string, string> Data { get;  set; }

        public HacthInfo Hatch { get;  set; }

        public string BlockName { get;  set; }


        public EntityInfo() { }







        public static EntityInfo Get(Entity ent)
        {
            if (ent == null) return null;
            EntityInfo result = new EntityInfo();
            result.Data = new DataManager().ReadDictionary(ent);
            result.EntityTypeName = ent.GetType().Name;
            result.EntityHandle = ent.Handle.ToString();
            result.ColorIndex = ent.ColorIndex;
            result.LayerName = ent.Layer;
            result.Area = GetArea(ent);
            result.Length = GetLength(ent);
            if (ent is Hatch)
            {
                var h = ent as Hatch;
                result.Hatch = new HacthInfo
                {
                    PatternAngle = h.PatternAngle,
                    PatternDouble = h.PatternDouble,
                    PatternName = h.PatternName,
                    PatternScale = h.PatternScale,
                    PatternSpace = h.PatternSpace,
                    PatternType = h.PatternType.ToString(),
                    Associative = h.Associative,
                    HatchStyle = h.HatchStyle.ToString(),

                };
            }
            else if (ent is BlockReference)
            {
                var br = ent as BlockReference;
                result.BlockName = br.Name;
            }
            return result;
        }

        [JsonIgnore]
        public string EntityHandle { get;private set; }             

        [JsonIgnore]
        public double? Area { get; private set; }

        [JsonIgnore]
        public double? Length { get; private set; }

        private static double? GetArea(Entity ent)
        {
            if (ent == null) return null;
            var prop = ent.GetType().GetProperty("Area");
            if (prop != null)
            {
                try
                {
                    return (double)prop.GetValue(ent);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        private static  double? GetLength(Entity ent)
        {
            if (ent == null) return null;
            if (ent is Mline)
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
                        return null;
                    }
                }
            }
            return null;
        }












        protected void OnExection(Exception ex)
        {
            Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($"执行失败，{ex.Message}{Environment.NewLine}");

        }




        public void Brush()
        {
            try
            {
                while (DoBrush()) ;
            }
            catch (Exception ex)
            {
                OnExection(ex);
            }
        }
        private string BlockTableRecord { get; set; } = "*MODEL_SPACE";

        private bool DoBrush()
        {
            try
            {
                var oid = ObjectId.Null;
                var doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                var db = HostApplicationServices.WorkingDatabase;
                var ed = doc.Editor;
                using (var l = doc.LockDocument())
                {
                    PromptEntityResult r;

                    r = ed.DoPrompt(new PromptEntityOptions("选择目标对象或[设置(S)]", "s") { AllowObjectOnLockedLayer = true }) as PromptEntityResult;
                    if (r.Status == PromptStatus.OK)
                    {
                        oid = r.ObjectId;
                        using (var tr = db.TransactionManager.StartTransaction())
                        {
                            Entity ent = tr.GetObject(r.ObjectId, OpenMode.ForWrite) as Entity;
                            ent.ColorIndex = ColorIndex;
                            if (!string.IsNullOrWhiteSpace(LayerName))
                            {
                                LayerManager.SetLayer(db, ent, LayerName, true);
                            }


                            if (ent is Hatch && Hatch != null)
                            {
                                var h = ent as Hatch;
                                Hatch.SetHatch(h);
                            }
                            tr.Commit();
                            if (Data != null)
                                DataManager.WriteDictionary(ent.Id, Data);
                        }
                        ed.WriteMessage(Environment.NewLine);
                        return true;
                    }
                    else if (r.Status == PromptStatus.Keyword)
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                OnExection(ex);
            }
            return false;
        }


    }
}
