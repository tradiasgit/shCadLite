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
        public PaletteSet ShowPaletteSet(PaletteSetConfig config)
        {
            var ps = new PaletteSet(config.Name);
            ps.Text = config.Name;
            ps.Style = PaletteSetStyles.Snappable | PaletteSetStyles.Notify | PaletteSetStyles.SingleColDock | PaletteSetStyles.ShowCloseButton | PaletteSetStyles.ShowPropertiesMenu | PaletteSetStyles.ShowAutoHideButton;
            ps.Visible = true;
            if (config.IsDock)
            {
                ps.DockEnabled = DockSides.Left;
                ps.Dock = DockSides.Left;
            }
            else
            {
                ps.Dock = DockSides.None;
                ps.DockEnabled = DockSides.None;
            }

            foreach (var pconfig in config.PaletteConfigs)
            {
                ps.AddVisual(pconfig.Key, pconfig.Value);
            }
            return ps;
        }
    }

    public class PaletteSetConfig
    {
        public string Name { get; set; }

        public bool IsDock { get; set; }

        public Dictionary<string,FrameworkElement> PaletteConfigs { get; set; }

        public int Width { get; set; } = 200;
        public int Height { get; set; } = 500;
    }

}
