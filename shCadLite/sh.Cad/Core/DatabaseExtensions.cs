using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autodesk.AutoCAD.DatabaseServices
{
    public static class DatabaseExtensions
    {
        internal static bool ExHasLayout(this Database db, string layoutName) { using (var tr = db.TransactionManager.StartOpenCloseTransaction()) { return ExHasLayout(db, layoutName, tr); } }
        internal static bool ExHasLayout(this Database db, string layoutName, Transaction tr)
        {
            if (db == null || tr == null) return false;
            DBDictionary lays = tr.GetObject(db.LayoutDictionaryId, OpenMode.ForRead) as DBDictionary;
            foreach (DBDictionaryEntry item in lays)
            {
                if (item.Key == layoutName) return true;
            }
            return false;
        }

        internal static Layout ExGetLayout(this Database db, string layoutName, Transaction tr)
        {
            DBDictionary layoutsEx = tr.GetObject(db.LayoutDictionaryId, OpenMode.ForRead) as DBDictionary;
            // Check to see if the layout exists in the external drawing
            if (!layoutsEx.Contains(layoutName)) throw new Exception($"目标图纸中不含指定布局：{layoutName}");
            // Get the layout and block objects from the external drawing
            Layout layEx = layoutsEx.GetAt(layoutName).GetObject(OpenMode.ForRead) as Layout;
            return layEx;
        }

        public static List<string> ExGetLayoutNames(this Database db) { using (var tr = db.TransactionManager.StartOpenCloseTransaction()) { return ExGetLayoutNames(db, tr); } }
        internal static List<string> ExGetLayoutNames(this Database db, Transaction tr)
        {
            var result = new List<string>();
            DBDictionary lays = tr.GetObject(db.LayoutDictionaryId, OpenMode.ForRead) as DBDictionary;
            foreach (DBDictionaryEntry item in lays)
            {
                result.Add(item.Key);
            }
            return result;
        }

        internal static PlotSettings ExGetPlotSettings(this Database db, Transaction tr, string plotSettingsName)
        {
            DBDictionary plSetsEx = tr.GetObject(db.PlotSettingsDictionaryId, OpenMode.ForRead) as DBDictionary;
            if (!plSetsEx.Contains(plotSettingsName)) return null;
            return tr.GetObject(plSetsEx.GetAt(plotSettingsName), OpenMode.ForRead) as PlotSettings;
        }
        internal static void ExImportPlotSettings(this Database db, Transaction tr, PlotSettings plotSettings)
        {
            // Check to see if a named page setup was assigned to the layout,
            // if so then copy the page setup settings
            if (plotSettings == null || string.IsNullOrWhiteSpace(plotSettings.PlotSettingsName)) return;

            var plotSettingsName = plotSettings?.PlotSettingsName;
            DBDictionary plSets = tr.GetObject(db.PlotSettingsDictionaryId, OpenMode.ForRead) as DBDictionary;
            if (!plSets.Contains(plotSettingsName))
            {
                plSets.UpgradeOpen();
                using (PlotSettings plSet = new PlotSettings(plotSettings.ModelType))
                {
                    plSet.PlotSettingsName = plotSettingsName;
                    plSet.AddToPlotSettingsDictionary(db);
                    tr.AddNewlyCreatedDBObject(plSet, true);
                    plSet.CopyFrom(plotSettings);
                }
            }
        }


        public static void ExImportLayout(this Database targetDb,FileInfo file,string layoutName,Vector3d vector)
        {
            if (!file.Exists) throw new FileNotFoundException("找不到图纸文件", file.FullName);           
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) throw new Exception("没有打开的文档，无法执行命令");
            var targetlayoutName = layoutName;
            var ed = doc.Editor;
            var layoutNames = HostApplicationServices.WorkingDatabase.ExGetLayoutNames();
            while (layoutNames.Contains(targetlayoutName))
            {
                var prompt = ed.DoPrompt(new PromptStringOptions($"{Environment.NewLine}布局[{targetlayoutName}]已存在,请指定新的布局名称：{Environment.NewLine}"));
                if (prompt.Status != PromptStatus.OK) return ;
                targetlayoutName = prompt.StringResult;
            }
            Database sourceDb = new Database(false, true);
            sourceDb.ReadDwgFile(file.FullName, FileOpenMode.OpenForReadAndAllShare, true, "");
            using (Transaction sourceTr = sourceDb.TransactionManager.StartTransaction())
            {
                var sourceLayout = sourceDb.ExGetLayout(layoutName, sourceTr);
                // Create a transaction for the current drawing
                using (Transaction targetTr = targetDb.TransactionManager.StartTransaction())
                {
                    // Get the block table and create a new block
                    // then copy the objects between drawings
                    BlockTable blkTbl = targetTr.GetObject(targetDb.BlockTableId, OpenMode.ForWrite) as BlockTable;

                    using (BlockTableRecord newLayoutBtr = new BlockTableRecord())
                    {
                        newLayoutBtr.Name = $"*Paper_Space{layoutNames.Count-1}";
                        blkTbl.Add(newLayoutBtr);
                        targetTr.AddNewlyCreatedDBObject(newLayoutBtr, true);
                        targetDb.WblockCloneObjects(sourceLayout.ExGetObejctIds(sourceTr), newLayoutBtr.ObjectId, new IdMapping(), DuplicateRecordCloning.Ignore, false);

                        // Create a new layout and then copy properties between drawings
                        DBDictionary layouts = targetTr.GetObject(targetDb.LayoutDictionaryId, OpenMode.ForWrite) as DBDictionary;

                        using (Layout lay = new Layout())
                        {
                            lay.LayoutName = targetlayoutName;
                            lay.AddToLayoutDictionary(targetDb, newLayoutBtr.ObjectId);
                            targetTr.AddNewlyCreatedDBObject(lay, true);
                            lay.CopyFrom(sourceLayout);

                            var plotsettings = sourceDb.ExGetPlotSettings(sourceTr, sourceLayout.PlotSettingsName);
                            targetDb.ExImportPlotSettings(targetTr, plotsettings);
                        }
                        //所有视口重新定位到指定点
                        int vieportindex = 0;
                        foreach (var oid in newLayoutBtr)
                        {
                            var obj = targetTr.GetObject(oid, OpenMode.ForRead);
                            if (obj is Viewport)
                            {
                                if (vieportindex != 0)
                                {
                                    var vp = obj as Viewport;
                                    vp.UpgradeOpen();
                                    vp.ViewCenter += new Vector2d(vector.X,vector.Y);
                                }
                                vieportindex++;
                            }
                        }
                    }
                    targetTr.Commit();
                }
            }
        }
    }
}
