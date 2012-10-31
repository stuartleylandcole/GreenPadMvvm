﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenPadMvvm.Commands;
using System.Windows.Input;

namespace GreenPadMvvm.ViewModels
{
    public abstract class TabViewModel : ViewModelBase
    {
        private RelayCommand _closeCommand;
        private string _tabDescription;

        protected TabViewModel()
        {
        }

        public string TabDescription
        {
            get
            {
                return _tabDescription;
            }
            set
            {
                _tabDescription = value;
            }
        }

        public abstract void SetTabTitle();

        #region Close command

        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(param => this.OnRequestClose());
                }
                return _closeCommand;
            }
        }

        #endregion

        #region RequestClose

        public event EventHandler RequestClose;

        private void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
