using Newtonsoft.Json;
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
        public string EntityType { get; set; }

        public List<EntityPropertyConfig> PropertyConfigs { get; set; }
    }

    public class EntityPropertyConfig
    {
        public string Title { get; set; }
        public string ProperyName { get; set; }

        public string ViewModelName { get; set; }
    }
}
