using sh.Cad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sh.Creator.Models.Property;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace sh.Creator.ViewModels.Property
{
    class VM_PropertyPalette : CadViewModelBase, IEntitySelectionListener
    {




        public static VM_PropertyPalette Inctance { get; set; }

        public VM_PropertyPalette() { Inctance = this; EventManager.RegisterSelectionListener(this); }



        public void OnSelectedTreeItemChanged(FileInfo file)
        {
            Clear();
            var entinfo = EntityInfo.Get(file);
            if(entinfo != null)
            {
                Title = entinfo.EntityTypeName;
                var entconfig = Models.EntityConfig.Get("enf");
                if (entconfig != null)
                {
                    PropertyItems = new ObservableCollection<VM_PropertyItem>();
                    Functions = new ObservableCollection<VM_Function>();
                    foreach (var f in entconfig.EntityFunctions)
                    {
                        if (f.Visible(entinfo))
                        {
                            var typestr = $"sh.Creator.CadCommands.{f.Command}";
                            ICommand cmd = Activator.CreateInstance(Type.GetType(typestr)) as ICommand;
                            if (cmd != null)
                            {
                                var vm = new VM_Function();
                                vm.Title = f.Title;
                                vm.Command = cmd;
                                vm.CommandParameter = entinfo;
                                Functions.Add(vm);
                            }
                        }
                    }

                    foreach (var p in entconfig.PropertyConfigs)
                    {
                        if (p.Visible(entinfo))
                        {
                            var vm = new VM_PropertyItem();
                            vm.Label = p.Title;
                            vm.Value = p.GetValue(entinfo);
                            vm.Category = p.Category;
                            vm.IsEditable = p.IsEditable;
                            PropertyItems.Add(vm);
                        }
                    }
                }

            }
        }


        public void Clear()
        {
            Title = null;
            PropertyItems = null;
            Functions = null;
        }


        public void OnSelectionChanged(EntitySelection selection)
        {
            Clear();
            if (selection.Count == 1)
            {
                var entinfo = selection.GetEntity();
                OnSelectionChanged(entinfo);

            }
        }


        public void OnSelectionChanged(EntityInfo entinfo)
        {

            var entconfig = Models.EntityConfig.Get("Entity");
            if (entconfig != null)
            {
                Title = entinfo.EntityTypeName;
                PropertyItems = new ObservableCollection<VM_PropertyItem>();
                Functions = new ObservableCollection<VM_Function>();
                foreach (var f in entconfig.EntityFunctions)
                {
                    if (f.Visible(entinfo))
                    {
                        var typestr = $"sh.Creator.CadCommands.{f.Command}";
                        ICommand cmd = Activator.CreateInstance(Type.GetType(typestr)) as ICommand;
                        if (cmd != null)
                        {
                            var vm = new VM_Function();
                            vm.Title = f.Title;
                            vm.Command = cmd;
                            vm.CommandParameter = entinfo;
                            Functions.Add(vm);
                        }
                    }
                }

                foreach (var p in entconfig.PropertyConfigs)
                {
                    if (p.Visible(entinfo))
                    {
                        var vm = new VM_PropertyItem();
                        vm.Label = p.Title;
                        vm.Value = p.GetValue(entinfo);
                        vm.Category = p.Category;
                        vm.IsEditable = p.IsEditable;
                        PropertyItems.Add(vm);
                    }
                }
            }
        }

        public string Title { get { return GetValue<string>(); } set { SetValue(value); } }


        public ObservableCollection<VM_PropertyItem> PropertyItems { get { return GetValue<ObservableCollection<VM_PropertyItem>>(); } set { SetValue(value); } }
        public ObservableCollection<VM_Function> Functions { get { return GetValue<ObservableCollection<VM_Function>>(); } set { SetValue(value); } }

    }
}
