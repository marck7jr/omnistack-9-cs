using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace AirCnC.Shared.Models
{
    public abstract class Model : INotifyPropertyChanged
    {
        private Guid guid;

        public event PropertyChangedEventHandler PropertyChanged;

        [Key]
        public Guid Guid { get => guid; set => Set(ref guid, value); }

        public virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
