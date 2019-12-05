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
using System.Xml;

namespace sh.Creator.ViewModels
{
    public class VM_SingleSelectBox : ViewModelBase, sh.Cad.IEntitySelectionListener
    {

        public string AreaText { get { return GetValue<string>(); } set { SetValue(value); } }
        public string LengthText { get { return GetValue<string>(); } set { SetValue(value); } }
        public string LayerName { get { return GetValue<string>(); } set { SetValue(value); } }

        public string TypeName { get { return GetValue<string>(); } set { SetValue(value); } }

        public string ValueTypeText { get { return GetValue<string>(); } set { SetValue(value); } }

        public Dictionary<string, string> EntityData { get; set; }

        public HacthConfig HacthStyle { get; set; }

        public static string ConvertEntityType(string dxf)
        {
            switch (dxf)
            {
                default: return dxf;
                case "LWPOLYLINE": return "多段线";
                case "LINE": return "直线";
                case "CIRCLE": return "圆";
                case "INSERT": return "块参照";
                case "HATCH": return "填充";
                case "DIMENSION": return "标注";
                case "MTEXT": return "多行文字";
                case "MLINE": return "多行";
                case "ARC": return "圆弧";
                case "LEADER": return "引线";
                case "TEXT": return "文字";
                case "ATTDEF": return "属性定义";
            }
        }



        public bool IsVisible { get { return GetValue<bool>(); } set { SetValue(value); } }

        public void OnSelectionChanged(EntitySelection selection)
        {
            IsVisible = false;
            if (selection != null && selection.Count == 1)
            {
                var ent = selection.GetEntity();
                if (ent != null)
                {
                    AreaText = string.Format("{0:f2}平米", 0.000001 * ent.GetArea());
                    LengthText = string.Format("{0:f2}米", 0.001 * ent.GetLength());
                    LayerName = ent.LayerName;
                    EntityData = ent.GetData();
                    TypeName = ConvertEntityType(ent.DxfName);
                    if (ent.IsHatch) HacthStyle = ent.GetHatch();
                    IsVisible = true;
                }
            }
        }






        public ICommand Cmd_SaveAs
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
                    var type = "Count"; ;
                    switch (ValueTypeText)
                    {
                        case "长度": type = "SumLength"; break;
                        case "面积": type = "SumArea"; break;
                    }
                    var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                    var db_source = HostApplicationServices.WorkingDatabase;
                    var dir = new FileInfo(db_source.Filename).Directory;
                    dir = new DirectoryInfo($@"{dir.FullName}\support");
                    dir.Create();
                    var op_file = new PromptSaveFileOptions("选择目标文件" + Environment.NewLine);
                    op_file.InitialDirectory = $@"{dir.FullName}"; ;
                    op_file.Filter = "图形配置文件-XML格式 (*.ecx)|*.ecx";

                    var result_file = ed.GetFileNameForSave(op_file);
                    if (result_file.Status == PromptStatus.OK)
                    {
                        var doc = new XmlDocument();
                        var root = doc.CreateElement("EntityConfig");
                        root.SetAttribute("ValueType", type);
                        root.SetAttribute("ColorIndex", "256");
                        root.SetAttribute("LayerName", LayerName);
                        switch (type)
                        {
                            default:
                                {
                                    root.SetAttribute("ValueFormat", "{0}个");
                                    root.SetAttribute("ValueRatio", "1");
                                    break;
                                }
                            case "SumLength":
                                {
                                    root.SetAttribute("ValueFormat", "{0:f2}米");
                                    root.SetAttribute("ValueRatio", "0.001");
                                    break;
                                }
                            case "SumArea":
                                {
                                    root.SetAttribute("ValueFormat", "{0:f2}平米");
                                    root.SetAttribute("ValueRatio", "0.000001");
                                    break;
                                }
                        }
                        doc.AppendChild(root);
                          
                        if (EntityData != null)
                        {
                            foreach (var data in EntityData)
                            {
                                var dnode = doc.CreateElement("Data");
                                dnode.SetAttribute("Key", data.Key);
                                dnode.SetAttribute("Value", data.Value);
                                root.AppendChild(dnode);
                            }
                        }
                        if (HacthStyle != null)
                        {
                            var ele = HacthStyle.ToXml(doc);
                            if (ele != null) root.AppendChild(ele);
                        }
                        doc.AppendChild(root);
                        doc.Save(result_file.StringResult);
                    }
                });
            }
        }


    }
}
