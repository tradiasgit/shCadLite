using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using sh.Cad;
using System.Collections.ObjectModel;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using System.IO;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System.Xml;

namespace sh.Creator.ViewModels
{
    public class VM_shQueryEditor : ViewModelBase, IEntitySelectionListener
    {
        #region Constructor

        private static VM_shQueryEditor _Instance;
        public static VM_shQueryEditor Instance
        {
            get
            {
                if (_Instance == null) _Instance = new VM_shQueryEditor();
                return _Instance;
            }
        }
        private VM_shQueryEditor()
        {
            NewData = new VM_Data();
            Data = new ObservableCollection<VM_Data>();
            IsAutoPickUp = true;
        }

        #endregion

        #region 选择集

        public int Count { get { return GetValue<int>(); } set { SetValue(value); } }
        public ICommand Cmd_PickFromSelection
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var selection = SelectionManager.GetSelection();
                    OnSelectionChanged(selection);
                });
            }
        }


        public void OnSelectionChanged(EntitySelection selection)
        {
            try
            {
                if (selection != null && selection.Count > 0)
                {
                    Layers = new ObservableCollection<VM_Layer>(selection.Entitys.Select(ent => ent.LayerName).Distinct().Select(layer => new VM_Layer(layer)).OrderBy(vm => vm.LayerName));
                    Types = new ObservableCollection<VM_EntityType>(selection.Entitys.Select(ent => ent.DxfName).Distinct().Select(name => new VM_EntityType(name)).OrderBy(vm => vm.ToString()));
                    Count = selection.Count;
                    Data = new ObservableCollection<VM_Data>();

                    var kvlist = selection.Entitys.SelectMany(ent => ent.Data).Distinct().OrderBy(p => p.Key);

                    foreach (var kv in kvlist)
                    {
                        var vm = new VM_Data(kv.Key, kv.Value);
                        vm.IsCommon = selection.Entitys.All(ent => ent.Data.Contains(kv));
                        Data.Add(vm);
                    }
                    Sum();
                }
                else
                {
                    Layers?.Clear();
                    Types?.Clear();
                    Data?.Clear();
                    Count = 0;
                    SumAreaText = "无选择";
                    SumLengthText = "无选择";
                }
                
            }
            catch (System.Exception ex)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog(ex.Message);
            }
        }


        /// <summary>
        /// 是否自动拾取
        /// </summary>
        public bool IsAutoPickUp
        {
            get { return GetValue<bool>(); }
            set
            {
                if (value)
                {
                    SelectionManager.RegisterListener(this);
                }
                else
                {
                    SelectionManager.UnRegisterListener(this);
                }
                SetValue(value);
            }
        }

        #endregion


        #region Layers
        public ObservableCollection<VM_Layer> Layers { get { return GetValue<ObservableCollection<VM_Layer>>(); } set { SetValue(value); } }

        public VM_Layer SelectedLayer { get { return GetValue<VM_Layer>(); } set { SetValue(value); } }

        public ICommand Cmd_RemoveLayer
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var value = p as string;
                    var tx = Layers.FirstOrDefault(t => t.LayerName == value);
                    if (tx != null) Layers.Remove(tx);
                });
            }
        }

        #endregion

        #region Types


        public ICommand Cmd_RemoveEntityType
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var value = p as string;
                    var tx = Types.FirstOrDefault(t => t.DxfName == value);
                    if (tx != null) Types.Remove(tx);
                });
            }
        }


        public ObservableCollection<VM_EntityType> Types { get { return GetValue<ObservableCollection<VM_EntityType>>(); } set { SetValue(value); } }


        #endregion


        #region Data
        public ObservableCollection<VM_Data> Data { get { return GetValue<ObservableCollection<VM_Data>>(); } set { SetValue(value); } }

        public VM_Data NewData { get { return GetValue<VM_Data>(); } set { SetValue(value); } }

        public ICommand Cmd_AddData
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    NewData.IsNew = true;
                    Data.Insert(0, NewData);
                    NewData = new VM_Data();

                });
            }
        }

        public ICommand Cmd_RemoveData
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var data = p as VM_Data;
                    Data.Remove(data);
                });
            }
        }
        protected double GetMlineLength(Entity ent)
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
        private void ForEach(IEnumerable<ObjectId> selection, Action<Entity> fun, Func<Exception, bool> exFun = null)
        {
            if (fun != null)
            {
                var db = HostApplicationServices.WorkingDatabase;
                using (var tr = db.TransactionManager.StartOpenCloseTransaction())
                {
                    foreach (var oid in selection)
                    {
                        try
                        {
                            var ent = tr.GetObject(oid, OpenMode.ForRead, false, true) as Entity;
                            if (ent != null)
                            {
                                fun(ent);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex == null)
                            {
                                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.Message);
                            }
                            else
                            {
                                exFun(ex);
                            }
                        }
                    }
                }
            }
        }
        public double SumLength(IEnumerable<ObjectId> selection)
        {
            if (selection == null) return 0;
            var value = 0.0;
            ForEach(selection, ent =>
            {
                if (ent is Mline) value += GetMlineLength(ent);
                else
                {
                    var prop = ent.GetType().GetProperty("Length");
                    if (prop != null)
                    {
                        try
                        {
                            value += (double)prop.GetValue(ent);
                        }
                        catch (Exception ex)
                        {
                            Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($@"1个对象获取长度失败：{ent.GetType().Name},{ex.Message}");
                        }
                    }
                }

            });
            //WriteLine($"总长度:{value}{Environment.NewLine}");
            return value;
        }
        public double SumArea(IEnumerable<ObjectId> selection)
        {
            if (selection == null) return 0;
            var value = 0.0;
            ForEach(selection, ent =>
            {
                var prop = ent.GetType().GetProperty("Area");
                if (prop != null)
                {
                    try
                    {
                        value += (double)prop.GetValue(ent);
                    }
                    catch (Exception ex)
                    {
                        Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($@"1个对象获取面积失败：{ent.GetType().Name},{ex.Message}");
                    }
                }
            });
            //WriteLine($"总面积:{value}{Environment.NewLine}");
            return value;
        }


        public string SumAreaText { get { return GetValue<string>(); } set { SetValue(value); } }
        public string SumLengthText { get { return GetValue<string>(); } set { SetValue(value); } }

        public ICommand Cmd_Refresh
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    Sum();
                });
            }
        }

        private void Sum()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;
            var db_source = HostApplicationServices.WorkingDatabase;
            using (var l = doc.LockDocument())
            {
                var op_ent = new PromptSelectionOptions();
                var result_ent = ed.SelectImplied();


                if (result_ent.Status == PromptStatus.OK&&result_ent.Value.Count>0)
                {
                    SumAreaText = string.Format("{0:f2}平米", 0.000001 * SumArea(result_ent.Value.GetObjectIds()));
                    SumLengthText = string.Format("{0:f2}米", 0.001 * SumLength(result_ent.Value.GetObjectIds()));
                }
                //ed.SetImpliedSelection(result_ent.Value.GetObjectIds());
            }
        }

        public ICommand Cmd_SaveData
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var data = p as VM_Data;
                    if (data.IsNew)
                    {
                        var doc = Application.DocumentManager.MdiActiveDocument;
                        var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                        var db_source = HostApplicationServices.WorkingDatabase;
                        using (var l = doc.LockDocument())
                        {
                            var op_ent = new PromptSelectionOptions();
                            var result_ent = ed.GetSelection(op_ent);

                            if (result_ent.Status == PromptStatus.OK)
                            {
                                var ids = result_ent.Value.GetObjectIds();
                                foreach (var oid in ids)
                                {
                                    sh.Cad.DataManager.WriteDictionary(oid, new Dictionary<string, string> { { data.Key, data.Value } });
                                }
                            }
                            ed.SetImpliedSelection(result_ent.Value);
                        }
                    }
                    data.IsNew = false;
                });
            }
        }

        public ICommand Cmd_DeleteData
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var data = p as VM_Data;
                    var doc = Application.DocumentManager.MdiActiveDocument;
                    var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                    var db_source = HostApplicationServices.WorkingDatabase;
                    using (var l = doc.LockDocument())
                    {
                        var op_ent = new PromptSelectionOptions();
                        var result_ent = ed.GetSelection(op_ent);

                        if (result_ent.Status == PromptStatus.OK)
                        {
                            var ids = result_ent.Value.GetObjectIds();
                            foreach (var oid in ids)
                            {
                                sh.Cad.DataManager.RemoveKey(oid, data.Key);
                            }
                        }
                        ed.SetImpliedSelection(result_ent.Value);
                    }
                });
            }
        }

        #endregion

        public string Name { get { return GetValue<string>(); } set { SetValue(value); } }


        #region 保存Cad部件

        public ICommand Cmd_SaveAs
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    //var dbmod = Application.GetSystemVariable("DBMOD");
                    var dwgtitled = Application.GetSystemVariable("DWGTITLED");
                    if(Convert.ToInt16(dwgtitled)==0)
                    {
                        ShowMessage("请保存图纸。");
                        return;
                    }
                     
                    var doc = Application.DocumentManager.MdiActiveDocument;
                    var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                    var db_source = HostApplicationServices.WorkingDatabase;
                    var dir = new FileInfo(db_source.Filename).Directory;


                    dir = new DirectoryInfo($@"{dir.FullName}\support");
                    dir.Create();
                    using (var l = doc.LockDocument())
                    {
                        var op_ent = new PromptSelectionOptions();
                        var result_ent = ed.GetSelection(op_ent);
                        if (result_ent.Status == PromptStatus.OK)
                        {
                            var op_point = new PromptPointOptions("请指定基点" + Environment.NewLine);
                            var result_point = ed.GetPoint(op_point);
                            if (result_point.Status == PromptStatus.OK)
                            {
                                var op_file = new PromptSaveFileOptions("选择目标文件" + Environment.NewLine);
                                op_file.InitialDirectory = $@"{dir.FullName}\dwg"; ;
                                op_file.Filter = "Autocad 2010 图形 (*.dwg)|*.dwg";

                                var result_file = ed.GetFileNameForSave(op_file);
                                if (result_file.Status == PromptStatus.OK)
                                {
                                    var ids = result_ent.Value.GetObjectIds();
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
                            }
                        }
                    }
                });
            }
        }


        #endregion
     
        public ICommand Cmd_SaveAsQuery
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var dwgtitled = Application.GetSystemVariable("DWGTITLED");
                    if (Convert.ToInt16(dwgtitled) == 0)
                    {
                        ShowMessage("请保存图纸。");
                        return;
                    }
                    var type = (string)p;
                    var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                    var db_source = HostApplicationServices.WorkingDatabase;
                    var dir = new FileInfo(db_source.Filename).Directory;
                    dir = new DirectoryInfo($@"{dir.FullName}\support");
                    dir.Create();
                    var op_file = new PromptSaveFileOptions("选择目标文件" + Environment.NewLine);
                    op_file.InitialDirectory = $@"{dir.FullName}\field"; ;
                    op_file.Filter = "过滤器配置文件 (*.xml)|*.xml";

                    var result_file = ed.GetFileNameForSave(op_file);
                    if (result_file.Status == PromptStatus.OK)
                    {


                        var doc = GetQueryXmlDocument((string)p);
                        doc.Save(result_file.StringResult);
                    }
                });
            }
        }




        private XmlDocument GetQueryXmlDocument(string type)
        {
            var doc = new XmlDocument();
            var root = doc.CreateElement("QueryField");
            root.SetAttribute("ResultType", type);
            switch (type)
            {
                default:
                    {
                        root.SetAttribute("Format", "{0}个");
                        root.SetAttribute("Ratio", "1");
                        break;
                    }
                case "SumLength":
                    {
                        root.SetAttribute("Format", "{0:f2}米");
                        root.SetAttribute("Ratio", "0.001");
                        break;
                    }
                case "SumArea":
                    {
                        root.SetAttribute("Format", "{0:f2}平米");
                        root.SetAttribute("Ratio", "0.000001");
                        break;
                    }
            }

            
            doc.AppendChild(root);
            foreach (var data in Data)
            {
                var dnode = doc.CreateElement("Data");
                dnode.SetAttribute("Key", data.Key);
                dnode.SetAttribute("Value", data.Value);
                root.AppendChild(dnode);
            }
            foreach (var t in Types)
            {
                var node = doc.CreateElement("Filter");
                node.SetAttribute("Type", "EntityTpye");
                node.SetAttribute("Name", t.DxfName);
                //root.AppendChild(node);
            }
            foreach (var l in Layers)
            {
                var node = doc.CreateElement("Filter");
                node.SetAttribute("Type", "Layer");
                node.SetAttribute("Name", l.LayerName);
                root.AppendChild(node);
            }
            doc.AppendChild(root);
            return doc;
        }

       
        private XmlDocument GetBrushXmlDocument()
        {
            var doc = new XmlDocument();
            var root = doc.CreateElement("BrushAction");
            root.SetAttribute("ColorIndex", "256");
            root.SetAttribute("LayerName", SelectedLayer.LayerName);
            doc.AppendChild(root);
            foreach (var data in Data)
            {
                var dnode = doc.CreateElement("Data");
                dnode.SetAttribute("Key", data.Key);
                dnode.SetAttribute("Value", data.Value);
                root.AppendChild(dnode);
            }
            foreach (var t in Types)
            {
                var node = doc.CreateElement("Filter");
                node.SetAttribute("Type", "EntityTpye");
                node.SetAttribute("Name", t.DxfName);
                //root.AppendChild(node);
            }
            doc.AppendChild(root);
            return doc;
        }

        public ICommand Cmd_SaveAsBrush
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var dwgtitled = Application.GetSystemVariable("DWGTITLED");
                    if (Convert.ToInt16(dwgtitled) == 0)
                    {
                        ShowMessage("请保存图纸。");
                        return;
                    }
                    if (SelectedLayer == null)
                    {
                        ShowMessage("请选择笔刷图层");
                        return;
                    }
                    var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                    var db_source = HostApplicationServices.WorkingDatabase;
                    var dir = new FileInfo(db_source.Filename).Directory;
                    dir = new DirectoryInfo($@"{dir.FullName}\support\brush");
                    dir.Create();
                    var op_file = new PromptSaveFileOptions("选择目标文件" + Environment.NewLine);
                    op_file.InitialDirectory = $@"{dir.FullName}" ;
                    op_file.Filter = "笔刷配置文件 (*.xml)|*.xml";
                    var result_file = ed.GetFileNameForSave(op_file);
                    if (result_file.Status == PromptStatus.OK)
                    {
                        var doc = GetBrushXmlDocument();
                        doc.Save(result_file.StringResult);
                    }
                });
            }
        }
    }
}
