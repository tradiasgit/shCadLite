using sh.Creator.Cad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using sh.Creator.ViewModels.Repository;

namespace sh.Creator.ViewModels.Property
{
    class VM_PropertyPalette : CadViewModelBase
    {

        public VM_PropertyPalette()
        {
            EventManager.CadDocumentSelectionChanged += OnCadDocumentSelectionChanged;
            EventManager.RepositorySelectedItemChanged += OnRepositorySelectedItemChanged;
        }


        protected void OnCadDocumentSelectionChanged(object sender, EventArgs e)
        {
            Clear();
            var entinfo = EntityInfoFactory.GetFromSelection();
            if (entinfo == null) return;
            var entconfig = Models.EntityConfig.Get("Entity");
            if (entconfig != null)
            {
                Title = entinfo.EntityTypeName;
                PropertyItems = new ObservableCollection<IVM_ProperyItem>();
                Functions = new ObservableCollection<VM_Function>();
                foreach (var f in entconfig.EntityFunctions)
                {
                    if (f.Visible(entinfo))
                    {
                        var typestr = $"sh.Creator.Functions.{f.Command}";
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
                foreach (var p in entconfig.EntityData)
                {
                    if (p.Visible(entinfo))
                    {
                        var vm = new VM_DataItem(entinfo,p);                       
                        PropertyItems.Add(vm);
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
        protected void OnRepositorySelectedItemChanged(object sender, TreeItemEventArgs e)
        {
            Clear();
            if (e.SelectedItem is VM_TreeFile)
            {
                var item = e.SelectedItem as VM_TreeFile;
                var entinfo = EntityInfoFactory.Get(item.File);
                if (entinfo != null)
                {
                    Title = entinfo.EntityTypeName;
                    var entconfig = Models.EntityConfig.Get("enf");
                    if (entconfig != null)
                    {
                        PropertyItems = new ObservableCollection<IVM_ProperyItem>();
                        Functions = new ObservableCollection<VM_Function>();
                        foreach (var f in entconfig.EntityFunctions)
                        {
                            if (f.Visible(entinfo))
                            {
                                var typestr = $"sh.Creator.Functions.{f.Command}";
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
        }









        public void Clear()
        {
            Title = null;
            PropertyItems = null;
            Functions = null;
        }





        public string Title { get { return GetValue<string>(); } set { SetValue(value); } }


        public ObservableCollection<IVM_ProperyItem> PropertyItems { get { return GetValue<ObservableCollection<IVM_ProperyItem>>(); } set { SetValue(value); } }
        public ObservableCollection<VM_Function> Functions { get { return GetValue<ObservableCollection<VM_Function>>(); } set { SetValue(value); } }

    }
}
