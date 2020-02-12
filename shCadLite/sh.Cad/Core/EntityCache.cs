using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public class EntityCache
    {
        private static Dictionary<ObjectId, EntityInfo> _cache = new Dictionary<ObjectId, EntityInfo>();

        public static void Push(ObjectId oid,EntityInfo info)
        {
            if (info == null || oid == ObjectId.Null || string.IsNullOrWhiteSpace(info.EntityHandle)) return;
            if (_cache.ContainsKey(oid)) _cache[oid] = info;
            else _cache.Add(oid, info);
        }

        public static EntityInfo Get(ObjectId oid, Transaction tr,Func<ObjectId,Transaction,EntityInfo> loader)
        {
            _cache.Clear();
            if (oid == ObjectId.Null) return null;
            if (!_cache.ContainsKey(oid))
            {
                var info = loader(oid,tr);
                Push(oid,info);
            }
            if (_cache.ContainsKey(oid)) return _cache[oid];
            else return null;
        }

    }


    public struct CacheItem<TKey, TValue>
    {
        public CacheItem(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Time = DateTime.Now.Ticks;
        }

        public TKey Key { get; private set; }
        public TValue Value { get; private set; }

        public long Time { get; set; }
    }
}
