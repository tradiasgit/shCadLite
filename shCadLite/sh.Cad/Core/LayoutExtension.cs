using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autodesk.AutoCAD.DatabaseServices
{
    public static class LayoutExtension
    {

        public static ObjectIdCollection ExGetObejctIds(this Layout layout,Transaction tr)
        {
            BlockTableRecord blkBlkRecEx = tr.GetObject(layout.BlockTableRecordId, OpenMode.ForRead) as BlockTableRecord;
            // Get the objects from the block associated with the layout
            ObjectIdCollection idCol = new ObjectIdCollection();
            foreach (ObjectId id in blkBlkRecEx)
            {
                idCol.Add(id);
            }
            return idCol;
        }
       
    }
}
