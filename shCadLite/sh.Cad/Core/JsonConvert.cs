using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad.Json
{
    
    internal abstract class JsonCreationConverter<T> : JsonConverter
    {
        protected abstract T Create(Type objectType, JObject jsonObject);
        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var target = Create(objectType, jsonObject);
            serializer.Populate(jsonObject.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    internal class EntityInfoJsonConverter : JsonCreationConverter<IEntityInfo>
    {
        protected override IEntityInfo Create(Type objectType, JObject jsonObject)
        {
            var typeName = jsonObject["EntityTypeName"].ToString();
            switch (typeName)
            {
                case "BlockReference": return new BlockInfo();
                case "Hatch": return new HatchInfo();
                case "Polyline": return new HatchInfo();
                case "Layout": return new LayoutInfo();
                case "EntityGroup": return new EntityGroupInfo();
                default: return new EntityInfo();
            }
        }
    }
}
