using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Newtonsoft.Json;
using sh.Creator.Cad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.Functions
{
    class Brush : CadCommandBase
    {
        [CommandMethod("shbrush", CommandFlags.UsePickSet)]
        protected override void ExecuteBody(object parameter)
        {
            var enf = parameter as EntityInfo;
            if (enf == null)
            {
                var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                var db_source = HostApplicationServices.WorkingDatabase;
                var dir = new FileInfo(db_source.OriginalFileName).Directory;

                var op_file = new PromptOpenFileOptions("选择目标文件" + Environment.NewLine);
                op_file.InitialDirectory = $@"{dir.FullName}";
                op_file.Filter = "图形配置文件-json格式 (*.enf)|*.enf";
                var result_file = ed.GetFileNameForOpen(op_file);
                if (result_file.Status == PromptStatus.OK)
                {
                    enf = EntityInfoFactory.Get(new FileInfo(result_file.StringResult));
                }
                else return;
            }
            if (enf.IsFromDocument)
            {
                BrushImplied(enf);
            }
            else
            {
                
            }
            //var sel = new EntitySelection(doc.Editor.SelectImplied());
            //EventManager.ra
        }


        private void BrushImplied(EntityInfo info)
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var ed = doc.Editor;
            var db = HostApplicationServices.WorkingDatabase;
            var result = ed.SelectImplied();
            if (result.Status == PromptStatus.OK)
            {
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    foreach (var oid in result.Value.GetObjectIds())
                    {
                        Entity ent = tr.GetObject(oid, OpenMode.ForWrite) as Entity;
                        BrushEntity_Base(ent, tr,info);
                        BrushEntity_Property(ent, tr, info);
                        BrushEntity_Data(ent, tr, info);
                        tr.Commit();
                    }
                }
            }
        }


        private void BrushEntity_Base(Entity ent, Transaction tr,EntityInfo info)
        {
            ent.ColorIndex = info.ColorIndex;
            if (!string.IsNullOrWhiteSpace(info.LayerName))
            {
                //LayerManager.SetLayer(ent, info.LayerName, tr);
            }
        }
        private void BrushEntity_Data(Entity ent, Transaction tr, EntityInfo info)
        {
            //if (Data != null)
            //{
            //    DataManager.WriteDictionary(ent, Data, tr);
            //}
        }
        private void BrushEntity_Property(Entity ent, Transaction tr, EntityInfo info)
        {
        }


    }
}
