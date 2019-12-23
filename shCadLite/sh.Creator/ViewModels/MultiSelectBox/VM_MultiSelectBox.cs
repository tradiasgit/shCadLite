using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using sh.Cad;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels
{
    public class VM_MultiSelectBox : ViewModelBase, sh.Cad.IEntitySelectionListener
    {
        public int Count { get { return GetValue<int>(); } set { SetValue(value); } }
        public string AreaText { get { return GetValue<string>(); } set { SetValue(value); } }
        public string LengthText { get { return GetValue<string>(); } set { SetValue(value); } }

        public bool IsVisible { get { return GetValue<bool>(); } set { SetValue(value); } }

        public void OnSelectionChanged(EntitySelection selection)
        {
            IsVisible = false;
            if (selection != null && selection.Count > 1)
            {
                var ents = selection.GetEntityies();
                AreaText = string.Format("{0:f2}平米", 0.000001 * (ents.Sum(i => i.Area)));
                LengthText = string.Format("{0:f2}米", 0.001 * (ents.Sum(i => i.Length)));
                Count = selection.Count;
                IsVisible = true;
            }
        }


        #region 保存Cad部件

        public ICommand Cmd_SaveAs
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    sh.Cad.EventManager.IsListening = false;

                    //var dbmod = Application.GetSystemVariable("DBMOD");
                    var dwgtitled = Application.GetSystemVariable("DWGTITLED");
                    if (Convert.ToInt16(dwgtitled) == 0)
                    {
                        ShowMessage("请保存图纸。");
                        return;
                    }

                    var doc = Application.DocumentManager.MdiActiveDocument;
                    var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                    var db_source = HostApplicationServices.WorkingDatabase;
                    var dir = new FileInfo(db_source.OriginalFileName).Directory;


                    dir = new DirectoryInfo($@"{dir.FullName}\support");
                    dir.Create();
                    using (var l = doc.LockDocument())
                    {
                        var selection = ed.SelectImplied();
                        var op_point = new PromptPointOptions("请指定基点" + Environment.NewLine);
                        var result_point = ed.GetPoint(op_point);
                        if (result_point.Status == PromptStatus.OK)
                        {
                            var op_file = new PromptSaveFileOptions("选择目标文件" + Environment.NewLine);
                            op_file.InitialDirectory = $@"{dir.FullName}\dwg"; ;
                            op_file.Filter = "Autocad 2016 图形 (*.dwg)|*.dwg";

                            var result_file = ed.GetFileNameForSave(op_file);
                            var ids = selection.Value.GetObjectIds();
                            using (var tr_source = db_source.TransactionManager.StartTransaction())
                            {
                                var mtx = Matrix3d.Displacement(Point3d.Origin - result_point.Value);
                                foreach (var oid in ids)
                                {
                                    var ent = tr_source.GetObject(oid, OpenMode.ForWrite) as Entity;
                                    if (ent != null)
                                    {
                                        ent.TransformBy(mtx);
                                    }
                                }
                                var targetObjectIds = new ObjectIdCollection(ids);
                                IdMapping mapping = new IdMapping();
                                using (var db_target = new Database(true, true))
                                {
                                    using (var tr_target = db_target.TransactionManager.StartTransaction())
                                    {
                                        var targetid = ((BlockTable)tr_target.GetObject(db_target.BlockTableId, OpenMode.ForRead))[BlockTableRecord.ModelSpace];
                                        db_source.WblockCloneObjects(targetObjectIds, targetid, mapping, DuplicateRecordCloning.Replace, false);
                                        tr_target.Commit();
                                    }
                                    if (result_file.StringResult.ToLower().EndsWith("dxf"))
                                        db_target.DxfOut(result_file.StringResult, 16, DwgVersion.Current);
                                    else
                                        db_target.SaveAs(result_file.StringResult, DwgVersion.Current);
                                }
                                tr_source.Abort();
                              
                            }

                        }
                        ed.SetImpliedSelection(selection.Value.GetObjectIds());
                    }
                    sh.Cad.EventManager.IsListening = true;
                });
            }
        }


        #endregion
    }
}
