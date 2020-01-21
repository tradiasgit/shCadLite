using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public class LayerStatesLocker : IDisposable
    {
        internal LayerStatesLocker(Transaction tr, ObjectId layerid)
        {
            _layerid = layerid;
            _tr = tr;
            UnlockAndOpen();
        }

        private void UnlockAndOpen()
        {
            if (_layerid != ObjectId.Null)
            {
                _ltr = _tr.GetObject(_layerid, OpenMode.ForWrite) as LayerTableRecord;
                isoff = _ltr.IsOff;
                isfrozen = _ltr.IsFrozen;
                islock = _ltr.IsLocked;
                if (isfrozen || islock)
                {
                    _ltr.IsFrozen = false;
                    _ltr.IsLocked = false;
                }
            }
        }
        private Transaction _tr;
        private ObjectId _layerid;
        private LayerTableRecord _ltr;


        private bool isfrozen;
        private bool islock;
        private bool isoff;

        public void Dispose()
        {
            if (_ltr != null)
            {
                try
                {
                    if(_ltr.ObjectId!=Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database.Clayer)
                    {
                        _ltr.IsFrozen = isfrozen;
                    }
                    _ltr.IsLocked = islock;
                    _ltr.IsOff = isoff;
                }
                catch (Exception ex) { throw ex; }
                finally
                {
                    _ltr.Dispose();
                }
            }
        }
    }
}
