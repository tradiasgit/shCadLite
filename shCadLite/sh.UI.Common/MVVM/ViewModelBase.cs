using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.UI.Common.MVVM
{



    public class ViewModelBase<TModel> : IViewModelBase
    {
        public ViewModelBase()
        {
            CommandFactory = new DelegateCommandFactory(this);
        }

        #region 核心成员

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void RaiseAllPropertyChanged()
        {
            foreach (var kv in Values)
            {
                RaisePropertyChanged(kv.Key);
            }
        }

        #endregion

        #region Values

        protected Dictionary<string, object> Values { get; } = new Dictionary<string, object>();

        protected T GetValue<T>([CallerMemberName]string name = null)
        {
            if (!Values.ContainsKey(name))
                return default(T);
            else return (T)Values[name];
        }
        protected void SetValue(object value, [CallerMemberName]string name = null)
        {
            if (!Values.ContainsKey(name))
                Values.Add(name, value);
            else Values[name] = value;
            RaisePropertyChanged(name);
        }

        #endregion

      

        public virtual TModel Model
        {
            get { return GetValue<TModel>(); }
            set { SetValue(value); }
        }

        #endregion




        #region Commands

        public DelegateCommandFactory CommandFactory { get; private set; }

        #endregion

        #region 扩展功能

        public IViewModelBase Parent { get; set; }

        private ViewHelper _viewHelper;
        public ViewHelper ViewHelper
        {
            get
            {
                if (_viewHelper != null) return _viewHelper;
                var pvm = Parent as ViewModelBase;
                if (pvm != null) return pvm.ViewHelper;
                else return new ViewHelper();
            }
            set { _viewHelper = value; }
        }

        private string _workingDirectory;
        public string WorkingDirectory
        {
            get
            {
                if (_workingDirectory != null) return _workingDirectory;
                var pvm = Parent as ViewModelBase;
                if (pvm != null) return pvm.WorkingDirectory;
                else return new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;
            }
            set { _workingDirectory = value; }
        }


        protected void ShowMessage(string message)
        {
            ViewHelper.ShowMessage(message);
        }
        protected void Confirm(string message)
        {
            ViewHelper.Confirm(message);
        }

        protected virtual void OnException(Exception ex)
        {
            ShowMessage(ex.Message);
        }


        #endregion


        #region 权限控制

        private string _authorizationFileName;
        /// <summary>
        /// 如不设置，返回"{WorkingDirectory}\CommandAuthorization\{GetType().Name}.txt" 
        /// </summary>
        public virtual string AuthorizationFileName
        {
            get
            {
                if (_authorizationFileName != null) return _authorizationFileName;
                else return $@"{WorkingDirectory}\CommandAuthorization\{GetType().Name}.txt";
            }
            set { _authorizationFileName = value; }
        }
        #endregion
    }


    public class ViewModelBase : ViewModelBase<object> { }
}
