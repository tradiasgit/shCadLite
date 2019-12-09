using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Newtonsoft.Json;
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
    public class VM_SingleSelectBox : ViewModelBase<EntityInfo>, sh.Cad.IEntitySelectionListener
    {

        public string AreaText { get { return GetValue<string>(); } set { SetValue(value); } }
        public string LengthText { get { return GetValue<string>(); } set { SetValue(value); } }
        public string EntityTypeText { get { return GetValue<string>(); } set { SetValue(value); } }

        public string ValueTypeText { get { return GetValue<string>(); } set { SetValue(value); } }


        public Dictionary<string, string> EntityData { get; set; }

        public ObservableCollection<VM_Data> Data { get { return GetValue<ObservableCollection<VM_Data>>(); } set { SetValue(value); } }

        public HacthInfo HacthStyle { get; set; }

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
                Model = ent;
                if (ent != null)
                {
                    AreaText =ent.Area!=null? string.Format("{0:f2}平米", 0.000001 * ent.Area) :"/";
                    LengthText = ent.Length != null ? string.Format("{0:f2}米", 0.001 * ent.Length) : "/";
                    EntityTypeText = ConvertEntityType(ent.EntityTypeName);
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
                    var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                    var db_source = HostApplicationServices.WorkingDatabase;
                    var dir = new FileInfo(db_source.Filename).Directory;
                    dir = new DirectoryInfo($@"{dir.FullName}\support");
                    dir.Create();
                    var op_file = new PromptSaveFileOptions("选择目标文件" + Environment.NewLine);
                    op_file.InitialDirectory = $@"{dir.FullName}"; ;
                    op_file.Filter = "图形配置文件-json格式 (*.ecj)|*.ecj";

                    var result_file = ed.GetFileNameForSave(op_file);
                    if (result_file.Status == PromptStatus.OK)
                    {
                         File.WriteAllText(result_file.StringResult,JsonConvert.SerializeObject(Model));
                    }
                });
            }
        }

    }
}
