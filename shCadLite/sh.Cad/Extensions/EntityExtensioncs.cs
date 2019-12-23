using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autodesk.AutoCAD.DatabaseServices
{
    public static class EntityExtensioncs
    {
        public static double? GetArea(this Entity ent)
        {
            var proparea = ent.GetType().GetProperty("Area");
            if (proparea == null) return null;
            try
            {
                return (double)proparea.GetValue(ent);
            }
            catch (Exception e)
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(Environment.NewLine + "【求面积异常】：" + ent.GetType().Name + "：" + e.Message);
                return null;
            }
        }

        public static double? GetLength(this Entity ent)
        {
            if (ent == null) return null;
            if (ent is Mline)
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
            else
            {
                var prop = ent.GetType().GetProperty("Length");
                if (prop != null)
                {
                    try
                    {
                        return (double)prop.GetValue(ent);
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }
            return null;
        }
    }
}
