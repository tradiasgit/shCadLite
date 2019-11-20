
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using Autodesk.AutoCAD.DatabaseServices;
using System.Xml;
using Autodesk.AutoCAD.EditorInput;

namespace sh.XmlResourcesParsing.Models
{
    public class BrushAction : CadEntityActionBase
    {
        public BrushAction(XmlElement ele) : base(ele)
        {
        }

        public short ColorIndex { get; set; } = 256;

        public override void Execute()
        {
            List<Entity> result = new List<Entity>();
            try
            {
                while (Brush(result)) ;

                foreach (var ent in result)
                {
                    ent.Unhighlight();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        //protected void WriteData(Entity ent)
        //{
        //    var data = Element.SelectNodes("Data");
        //    if (data != null && data.Count > 0)
        //    {
        //        var dic = new Dictionary<string, string>();
        //        foreach (XmlNode d in data)
        //        {
        //            var k = d.Attributes["Key"].Value;
        //            var v = d.Attributes["Value"].Value;
        //            dic.Add(k, v);
        //        }
        //        WriteDictionary(ent.ObjectId, dic);
        //    }
        //}



        private bool Brush(List<Entity> result)
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
                                SetLayer(db, ent, LayerName, true);
                            }


                            var node = Element.SelectSingleNode("Hatch") as XmlElement;
                            if (ent is Hatch && node != null)
                            {
                                var h = ent as Hatch;
                                var nameNode = node.Attributes["PatternName"];
                                if (nameNode != null)
                                    h.SetHatchPattern(HatchPatternType.PreDefined, nameNode.Value);

                                var angelNode = node.Attributes["PatternAngle"];
                                if (angelNode != null)
                                    h.PatternAngle = double.Parse(angelNode.Value) * (Math.PI / 180);  //角度转弧度 度数 * (π / 180） = 弧度   

                                var scaleNode = node.Attributes["PatternScale"];
                                if (scaleNode != null)
                                    h.PatternScale = double.Parse(scaleNode.Value);
                            }
                            ent.Highlight();
                            result.Add(ent);
                            tr.Commit();
                            var data = base.GetData();
                            WriteDictionary(ent.Id, data);
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
                Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($"执行失败，{ex.Message}{Environment.NewLine}");
            }
            return false;
        }





    }
}
