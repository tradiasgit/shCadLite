using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.Cad
{
    class EntityInfoFactory
    {
        public static EntityInfo GetFromSelection()
        {
            try
            {
                using (var tr = HostApplicationServices.WorkingDatabase.TransactionManager.StartOpenCloseTransaction())
                {
                    var sel = Application.DocumentManager.MdiActiveDocument.Editor.SelectImplied();
                    if (sel.Value == null) return null;
                    if (sel.Value.Count == 1)
                    {
                        return Get(sel.Value[0].ObjectId, tr);
                    }
                    else return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public static EntityInfo Get(ObjectId oid, Transaction tr)
        {

            var result = new EntityInfo_Cad(oid, tr);
            return result;
        }


        public static EntityInfo Get(FileInfo file)
        {
            if (file == null || !file.Exists || file.Extension.ToLower() != ".enf") return null;
            var text = File.ReadAllText(file.FullName);
            return JsonConvert.DeserializeObject<EntityInfo_File>(text);
        }
    }
}
