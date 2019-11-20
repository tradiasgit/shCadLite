using Autodesk.AutoCAD.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace sh.Cad
{
    public class PaletteSetManager
    {

        public void ShowPaletteSet()
        {
            if (PaletteSet == null)
            {

                PaletteSet = ShowPaletteSet(GetConfig());
            }
            else if (!PaletteSet.Visible) PaletteSet.Visible = true;
        }

        protected virtual PaletteSetConfig GetConfig()
        {
            var _config = new PaletteSetConfig();
            return _config;
        }
        

        public void HidePaletteSet()
        {
            if (PaletteSet == null && PaletteSet.Visible) PaletteSet.Visible = false;
        }

        public void ClosePaletteSet()
        {
            if (PaletteSet != null) PaletteSet.Close();
        }

        protected virtual void OnPaletteSetClosed() { }



        private PaletteSet PaletteSet { get; set; }

        private static Dictionary<string, Guid> _dic = new Dictionary<string, Guid>();
        public PaletteSet ShowPaletteSet(PaletteSetConfig config)
        {
            var ps = new PaletteSet(config.Name);
            ps.Text = config.Name;
            ps.Style = PaletteSetStyles.Snappable | PaletteSetStyles. Notify | PaletteSetStyles.SingleColDock | PaletteSetStyles.ShowCloseButton | PaletteSetStyles.ShowPropertiesMenu | PaletteSetStyles.ShowAutoHideButton;
           
            var width = config.Width;
            var height = config.Height;


            if (ps.Dock == DockSides.None)
            {
                foreach (var pconfig in config.PaletteConfigs)
                {
                    pconfig.View.Width = width;
                    pconfig.View.Height = height;
                }

            }
            else
            {
                foreach (var pconfig in config.PaletteConfigs)
                {
                    pconfig.View.Width = width - 2;
                    pconfig.View.Height = height - 30;
                }

            }



            //var c = View as System.Windows.Controls.Control;
            //if (c.Background == null) c.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));

            if (config.IsDock)
            {
                ps.Size = new System.Drawing.Size((int)width + 2, (int)height + 30);
                ps.DockEnabled = DockSides.Left;
                ps.Dock = DockSides.Left;
                foreach (var pconfig in config.PaletteConfigs)
                {
                    ps.AddVisual(pconfig.Title, pconfig.View);
                }
                ps.Visible = true;
            }
            else
            {
                ps.Visible = true;
                ps.Dock = DockSides.None;
                ps.DockEnabled = DockSides.None;
                foreach (var pconfig in config.PaletteConfigs)
                {
                    ps.AddVisual(pconfig.Title, pconfig.View);
                }
                ps.Size = new System.Drawing.Size((int)width + 36, (int)height + 12);
            }


            ps.SizeChanged += (sender, e) =>
            {
                if (ps.Dock == DockSides.None)
                {
                    foreach (var pconfig in config.PaletteConfigs)
                    {
                        pconfig.View.Width = e.Width;
                        pconfig.View.Height = e.Height;
                    }
                    
                }
                else
                {
                    foreach (var pconfig in config.PaletteConfigs)
                    {
                        pconfig.View.Width = e.Width - 2;
                        
                        pconfig.View.Height = e.Height - 30;
                    }
                    
                }
            };
            ps.PaletteSetDestroy += (sender, e) =>
            {
                OnPaletteSetClosed();
                ps = null;
            };
            ps.Visible = true;
            return ps;
        }
    }

    public class PaletteSetConfig
    {
        public string Name { get; set; }

        public bool IsDock { get; set; }

        public List<PaletteConfig> PaletteConfigs { get; set; }

        public int Width { get; set; } = 200;
        public int Height { get; set; } = 500;
    }

    public class PaletteConfig
    {

        public FrameworkElement View { get; set; }

        public string Title { get; set; }

    }


}
