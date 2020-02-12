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
    public class EntityInfo : IEntityInfo
    {
        public EntityInfo() { }
        public EntityInfo(Entity ent, Transaction tr)
        {
            Data = new DataManager().ReadDictionary(ent, tr);
            EntityTypeName = ent.GetType().Name;
            EntityHandle = ent.Handle.ToString();
            ColorIndex = ent.ColorIndex;
            LayerName = ent.Layer;
            Area = ent.GetArea();
            Length = ent.GetLength();
        }


        protected bool Compare_Base(Entity ent, Transaction tr)
        {
            if (EntityTypeName != ent.GetType().Name) return false;
            if (LayerName != ent.Layer) return false;
            return true;
        }
        protected virtual bool Compare_Property(Entity ent, Transaction tr)
        {
            return true;
        }
        protected bool Compare_Data(Entity ent, Transaction tr)
        {
            //if (Data != info.Data) return false;
            return true;
        }
        public virtual bool Compare(Entity ent, Transaction tr)
        {
            return
                Compare_Base(ent, tr) &&
                Compare_Property(ent, tr) &&
                Compare_Data(ent, tr);
        }


        public virtual void Draw() { }



        public string EntityTypeName { get; set; }

        public int ColorIndex { get; set; } = 256;

        public string LayerName { get; set; }

        public Dictionary<string, string> Data { get; set; }


        public static EntityInfo Get(ObjectId objectid, Transaction tran)
        {
            return EntityCache.Get(objectid, tran, (oid, tr) =>
            {
                var ent = tr.GetObject(oid, OpenMode.ForRead) as Entity;
                if (ent == null) return null;
                else if (ent is BlockReference) return new BlockInfo(ent as BlockReference, tr);
                else if (ent is Hatch) return new HatchInfo(ent as Hatch, tr);
                else if (ent is Polyline) return new PolylineInfo(ent as Polyline, tr);
                else return new EntityInfo(ent, tr);
            });
        }



        public static IEntityInfo Get(FileInfo file)
        {
            if (file == null || !file.Exists || file.Extension.ToLower() != ".enf") return null;
            var text = File.ReadAllText(file.FullName);
            var info = JsonConvert.DeserializeObject<IEntityInfo>(text, new Json.EntityInfoJsonConverter());
            return info;
        }

        [JsonIgnore]
        public string EntityHandle { get; private set; }

        [JsonIgnore]
        public double? Area { get; private set; }

        [JsonIgnore]
        public double? Length { get; private set; }



        #region Brush

        public void Brush()
        {
            while (DoBrush()) ;
        }
        //private string BlockTableRecordName { get; set; } = "*MODEL_SPACE";


        protected void BrushEntity_Base(Entity ent, Transaction tr)
        {
            ent.ColorIndex = ColorIndex;
            if (!string.IsNullOrWhiteSpace(LayerName))
            {
                LayerManager.SetLayer(ent, LayerName, tr);
            }
        }
        protected void BrushEntity_Data(Entity ent, Transaction tr)
        {
            if (Data != null)
            {
                DataManager.WriteDictionary(ent, Data, tr);
            }
        }
        protected virtual void BrushEntity_Property(Entity ent, Transaction tr)
        {
        }

        public void BrushImplied()
        {
            var doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            var ed = doc.Editor;
            var db = HostApplicationServices.WorkingDatabase;
            using (var l = doc.LockDocument())
            {
                var result = ed.SelectImplied();
                if (result.Status == Autodesk.AutoCAD.EditorInput.PromptStatus.OK)
                {
                    using (var tr = db.TransactionManager.StartTransaction())
                    {
                        foreach (var oid in result.Value.GetObjectIds())
                        {
                            Entity ent = tr.GetObject(oid, OpenMode.ForWrite) as Entity;
                            BrushEntity_Base(ent, tr);
                            BrushEntity_Property(ent, tr);
                            BrushEntity_Data(ent, tr);
                            tr.Commit();
                        }
                    }
                }
            }
        }



        private bool DoBrush()
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
                        BrushEntity_Base(ent, tr);
                        BrushEntity_Property(ent, tr);
                        BrushEntity_Data(ent, tr);
                        tr.Commit();
                    }
                    ed.WriteMessage(Environment.NewLine);
                    return true;
                }
                else if (r.Status == PromptStatus.Keyword)
                {
                    return true;
                }
            }
            return false;
        }



        #endregion


        public virtual void SaveAs()
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
            op_file.Filter = "图形配置文件-json格式 (*.enf)|*.enf";
            var result_file = ed.GetFileNameForSave(op_file);
            if (result_file.Status == PromptStatus.OK)
            {
                File.WriteAllText(result_file.StringResult, JsonConvert.SerializeObject(this));
            }
        }


    }
}
