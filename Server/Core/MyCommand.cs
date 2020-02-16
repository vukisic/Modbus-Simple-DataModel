using System;
using System.Windows.Input;

namespace Server.Core
{
    public class MyCommand
    {
        public class MyICommand : ICommand
        {
            readonly Action _TargetExecuteMethod;


            public MyICommand(Action executeMethod)
            {
                _TargetExecuteMethod = executeMethod;
            }

            public void RaiseCanExecuteChanged()
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }

            bool ICommand.CanExecute(object parameter)
            {

                return true;
            }

            public event EventHandler CanExecuteChanged = delegate { };

            void ICommand.Execute(object parameter)
            {
                _TargetExecuteMethod?.Invoke();
            }
        }

        public class MyICommand<T> : ICommand
        {
            readonly Action<T> _TargetExecuteMethod;
            readonly Func<T, bool> _TargetCanExecuteMethod;

            public MyICommand(Action<T> executeMethod)
            {
                _TargetExecuteMethod = executeMethod;
            }

            public MyICommand(Action<T> executeMethod, Func<T, bool> func)
            {
                _TargetExecuteMethod = executeMethod;
                _TargetCanExecuteMethod = func;
            }

            public void RaiseCanExecuteChanged()
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }

            #region ICommand Members

            bool ICommand.CanExecute(object parameter)
            {

                if (_TargetCanExecuteMethod != null)
                {
                    T tparm = (T)parameter;
                    return _TargetCanExecuteMethod(tparm);
                }

                if (_TargetExecuteMethod != null)
                {
                    return true;
                }

                return false;
            }

            public event EventHandler CanExecuteChanged = delegate { };

            void ICommand.Execute(object parameter)
            {
                _TargetExecuteMethod?.Invoke((T)parameter);
            }

            #endregion
        }
    }
}