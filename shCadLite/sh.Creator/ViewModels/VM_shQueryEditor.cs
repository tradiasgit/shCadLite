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
    public class VM_shQueryEditor : ViewModelBase
    {

        

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
                    dir = new DirectoryInfo($@"{dir.FullName}\support\field");
                    dir.Create();
                    var op_file = new PromptSaveFileOptions("选择目标文件" + Environment.NewLine);
                    op_file.InitialDirectory = $@"{dir.FullName}\field"; ;
                    op_file.Filter = "过滤器配置文件 (*.xml)|*.xml";

                    var result_file = ed.GetFileNameForSave(op_file);
                    if (result_file.Status == PromptStatus.OK)
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
                        //if (DataBox != null)
                        //{
                        //    foreach (var data in DataBox.Data)
                        //    {
                        //        var dnode = doc.CreateElement("Data");
                        //        dnode.SetAttribute("Key", data.Key);
                        //        dnode.SetAttribute("Value", data.Value);
                        //        root.AppendChild(dnode);
                        //    }
                        //}
                        //if (LayerBox != null)
                        //{
                        //    foreach (var l in LayerBox.Layers)
                        //    {
                        //        var node = doc.CreateElement("Filter");
                        //        node.SetAttribute("Type", "Layer");
                        //        node.SetAttribute("Name", l.LayerName);
                        //        root.AppendChild(node);
                        //    }
                        //}
                        doc.AppendChild(root);
                        doc.Save(result_file.StringResult);
                    }
                });
            }
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

                    //if (LayerName == null) System.Windows.MessageBox.Show("请选择笔刷图层");

                    var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                    var db_source = HostApplicationServices.WorkingDatabase;
                    var dir = new FileInfo(db_source.Filename).Directory;
                    dir = new DirectoryInfo($@"{dir.FullName}\support\brush");
                    dir.Create();
                    var op_file = new PromptSaveFileOptions("选择目标文件" + Environment.NewLine);
                    op_file.InitialDirectory = $@"{dir.FullName}";
                    op_file.Filter = "笔刷配置文件 (*.xml)|*.xml";

                    var result_file = ed.GetFileNameForSave(op_file);
                    if (result_file.Status == PromptStatus.OK)
                    {
                        var doc = new XmlDocument();
                        var root = doc.CreateElement("BrushAction");
                        root.SetAttribute("ColorIndex", "256");
                        //root.SetAttribute("LayerName", LayerName);
                        doc.AppendChild(root);
                        //if (DataBox != null)
                        //{
                        //    foreach (var data in DataBox.Data)
                        //    {
                        //        var dnode = doc.CreateElement("Data");
                        //        dnode.SetAttribute("Key", data.Key);
                        //        dnode.SetAttribute("Value", data.Value);
                        //        root.AppendChild(dnode);
                        //    }
                        //}
                        doc.AppendChild(root);
                        doc.Save(result_file.StringResult);
                    }

                });
            }
        }






    }
}
