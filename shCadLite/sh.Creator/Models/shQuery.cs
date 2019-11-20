using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.Models
{
    public class shQuery
    {
        /// <summary>
        /// 查询名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据过滤器
        /// </summary>
        public Dictionary<string,string> Data { get; set; }

        /// <summary>
        /// 图层过滤器
        /// </summary>
        public List<string> LayerNames { get; set; }

        /// <summary>
        /// Entity类型过滤器
        /// 使用DxfName
        /// </summary>
        public List<string> EntityTypeNames { get; set; }

        /// <summary>
        /// 笔刷图层名
        /// </summary>
        public string BrushLayerName { get; set; }

        /// <summary>
        /// 值类型
        /// Count=数量
        /// Length=长度
        /// Area=面积
        /// </summary>
        public string ValueType { get; set; }
        
                
    }
}
