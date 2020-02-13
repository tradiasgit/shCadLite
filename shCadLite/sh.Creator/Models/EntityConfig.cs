using Newtonsoft.Json;
using sh.Creator.Cad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.Models
{
    public class EntityConfig
    {

        public static EntityConfig Get(string entityTypeName = "Entity")
        {
            var entconfigfile = new FileInfo($@"{new FileInfo(Assembly.GetExecutingAssembly().Location).Directory}\configs\{entityTypeName}.json");
            if (!entconfigfile.Exists) return null;
            try
            {
                return JsonConvert.DeserializeObject<EntityConfig>(File.ReadAllText(entconfigfile.FullName));
            }
            catch { }
            return null;
        }

        public List<EntityPropertyConfig> PropertyConfigs { get; set; }

        public List<EntityFunctionConfig> EntityFunctions { get; set; }

        public List<EntityDataConfig> EntityData { get; set; }
    }

    public class EntityFunctionConfig
    {
        public string Title { get; set; }

        public string Command { get; set; }
        public List<string> EntityTypes { get; set; }

        public bool Visible(EntityInfo info)
        {

            if (string.IsNullOrWhiteSpace(Command)) return false;
            if (EntityTypes == null || EntityTypes.Count == 0) return true;
            else return EntityTypes.Contains(info.EntityTypeName);
        }

    }
    public class EntityDataConfig
    {
        public string Title { get; set; }
        public string Key { get; set; }

        public string Value { get; set; }

        public bool Visible(EntityInfo info)
        {
            return true;
        }

        }
    public class EntityPropertyConfig
    {
        public string Title { get; set; }
        public string ProperyName { get; set; }

        public string Category { get; set; } = "基本";

        public string ValueFormat { get; set; } = "{0:f2}";

        public double ValueRatio { get; set; } = 1;

        public string ViewModelName { get; set; }

        public bool IsEditable { get; set; }

        public List<string> EntityTypes { get; set; }
        public string GetValue(EntityInfo info)
        {
            if (info == null) return null;
            var value = info.GetType()?.GetProperty(ProperyName)?.GetValue(info);
            if (value is double)
            {
                double v = (double)value;
                return string.Format(ValueFormat, v * ValueRatio);
            }
            else return value?.ToString();
        }
        public bool Visible(EntityInfo info)
        {
            if (EntityTypes == null || EntityTypes.Count == 0) return true;
            else return EntityTypes.Contains(info.EntityTypeName);
        }

    }
}
