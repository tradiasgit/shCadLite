using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sh.UI.Common
{

    public class FontIcon : Control
    {
        static FontIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FontIcon), new FrameworkPropertyMetadata(typeof(FontIcon)));
        }





        public Icons Icon
        {
            get { return (Icons)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Icons), typeof(FontIcon), new PropertyMetadata(Icons.Twitter, (sender, e) =>
             {
                 try
                 {
                     var current = (FontIcon)sender;
                     var value = (Icons)e.NewValue;
                     var charactor = typeof(Icons).GetField(value.ToString()).GetCustomAttributes(typeof(CharAttribute), true).FirstOrDefault() as CharAttribute;
                     current.Text = charactor.Value.ToString();
                 }
                 catch (Exception ex) { }
             }));




        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(FontIcon), new PropertyMetadata("Null"));




    }
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class CharAttribute : Attribute
    {
        public string Value { get; private set; }

        public CharAttribute(string value)
        {
            this.Value = value;
        }
    }

    public enum Icons
    {
        /// <summary>
        /// twitter小鸟
        /// </summary>
        [Char("\xf099")]
        Twitter,

        /// <summary>
        /// 实心圈i
        /// </summary>
        [Char("\xf129")]
        Info,

        /// <summary>
        /// 实心三角向上
        /// </summary>
        [Char("\xf0d8")]
        CaretUp,

        /// <summary>
        /// 实心三角向下
        /// </summary>
        [Char("\xf0d7")]
        CaretDown,

        /// <summary>
        /// 实心三角向左
        /// </summary>
        [Char("\xf0d9")]
        CaretLeft,

        /// <summary>
        /// 实心三角向右
        /// </summary>
        [Char("\xf0da")]
        CaretRight,

        /// <summary>
        /// 关闭的文件夹
        /// </summary>
        [Char("\xf07b")]
        FolderClose,

        /// <summary>
        /// 打开的文件夹
        /// </summary>
        [Char("\xf07c")]
        FolderOpen,

        /// <summary>
        /// 单个对号
        /// </summary>
        [Char("\xf00c")]
        OK,

        /// <summary>
        /// 刷新
        /// </summary>
        [Char("\xf2f1")]
        Refresh,

        /// <summary>
        /// 磁铁
        /// </summary>
        [Char("\xf076")]
        Magnet,

        /// <summary>
        /// 云下载
        /// </summary>
        [Char("\xf381")]
        CloudDownload,

        /// <summary>
        /// 打开新链接
        /// </summary>
        [Char("\xf35d")]
        ExternalLink,

        /// <summary>
        /// 垃圾桶
        /// </summary>
        [Char("\xf2ed")]
        Trash,

        /// <summary>
        /// 上传
        /// </summary>
        [Char("\xf093")]
        Upload,

        /// <summary>
        /// 星
        /// </summary>
        [Char("\xf069")]
        Asterisk,

        /// <summary>
        /// 暂停
        /// </summary>
        [Char("\xf04c")]
        PauseCircle,

        /// <summary>
        /// 开始
        /// </summary>
        [Char("\xf04b")]
        PlayCircle,

        /// <summary>
        /// 加号
        /// </summary>
        [Char("\xf067")]
        Plus,

        /// <summary>
        /// 锁
        /// </summary>
        [Char("\xf023")]
        Lock,
        /// <summary>
        /// 注销
        /// </summary>
        [Char("\xf2f5")]
        SignOutAlt,
        /// <summary>
        /// 搜索
        /// </summary>
        [Char("\xf002")]
        Search,
        /// <summary>
        /// 叉
        /// </summary>
        [Char("\xf00d")]
        Times,
        /// <summary>
        /// 排序
        /// </summary>
        [Char("\xf0dc")]
        Sort,

        /// <summary>
        /// 撤销，左旋转
        /// </summary>
        [Char("\xf0e2")]
        Undo,

        /// <summary>
        /// 重做，右旋转
        /// </summary>
        [Char("\xf01e")]
        Redo,
        /// <summary>
        /// 水平双向箭头
        /// </summary>
        [Char("\xf337")]
        ArrowsH,
        /// <summary>
        /// 垂直双向箭头
        /// </summary>
        [Char("\xf338")]
        ArrowsV,

        /// <summary>
        /// 水平向右（双箭头）书名号
        /// </summary>
        [Char("\xf101")]
        AngleDoubleRight,

        /// <summary>
        /// 复制
        /// </summary>
        [Char("\xf0c5")]
        Copy,

        /// <summary>
        /// 复制
        /// </summary>
        [Char("\xf24d")]
        Clone,

        /// <summary>
        /// 降级
        /// </summary>
        [Char("\xf3be")]
        LevelDown,
        


        /// <summary>
        /// 手形向上
        /// </summary>
        [Char("\xf0a6")]
        HandPointUp,
        /// <summary>
        /// 吸管（不好使）
        /// </summary>
        [Char("\xf1fb")]
        EyeDropper,

        /// <summary>
        /// Window（）
        /// </summary>
        [Char("\xf2d2")]
        WindowAlt,
        
        /// <summary>
        /// 油漆滚子
        /// </summary>
        [Char("\xf5aa")]
        PaintRoller,
        /// <summary>
        /// 查找定额
        /// </summary>
        [Char("\xf688")]
        SearchDollar,


        /// <summary>
        /// 用户锁定
        /// </summary>
        [Char("\xf502")]
        UserLock,

        /// <summary>
        /// 编辑
        /// </summary>
        [Char("\xf044")]
        Edit,

        /// <summary>
        ///文件
        /// </summary>
        [Char("\xf15b")]
        File,
        /// <summary>
        /// 带线文件
        /// </summary>
        [Char("\xf15c")]
        FileAlt,
        
        /// <summary>
        /// 油漆桶填充
        /// </summary>
        [Char("\xf576")]
        FillDrip,
        
         
        /// <summary>
        /// 代码
        /// </summary>
        [Char("\xf121")]
        Code,

        /// <summary>
        /// 计算器
        /// </summary>
        [Char("\xf1ec")]
        Calculator,

        /// <summary>
        /// 保存 Save
        /// </summary>
        [Char("\xf0c7")]
        FaFloppyO, 
        /// <summary>
        /// 刷子
        /// </summary>
        [Char("\xf55d")]
        Brush,

        /// <summary>
        /// 过滤器
        /// </summary>
        [Char("\xf0b0")]
        Filter,
    }
}
