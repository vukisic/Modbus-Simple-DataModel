using Caliburn.Micro;
using Common.Core;
using Common.Logger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class LogWindowViewModel : Conductor<object>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private ObservableCollection<string> items;
        public ObservableCollection<string> Items { get { return items; } set { items = value; OnPropertyChanged("Items"); } }

        public LogWindowViewModel(ILogger logger)
        {
            Items = new ObservableCollection<string>();
            foreach (var item in logger.GetLogs())
            {
                items.Add(item);
            }
        }

        protected virtual void SetProperty<T>(ref T member, T val,
            [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
