using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public class DatabaseManager
    {
        private Database _db;
        protected Database Database { get { if (_db != null) return _db; else return HostApplicationServices.WorkingDatabase; } set { _db = value; } }

        public static ObjectId GetObjectIdByHandle(string handle)
        {
            var db = Application.DocumentManager.MdiActiveDocument.Database;
            var obj = ObjectId.Null;           
            try
            {
                var h = new Handle(long.Parse(handle, System.Globalization.NumberStyles.AllowHexSpecifier));
                if (db.TryGetObjectId(h, out obj))
                {
                    if (!obj.IsErased)
                    {
                        return obj;
                    }
                }
            }
            catch
            {
                return ObjectId.Null;
            }
            return ObjectId.Null;
        }


    }
}
