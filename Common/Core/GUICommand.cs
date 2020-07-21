using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Common.Core
{
    public class GUICommand
    {
        public class GUIICommand : ICommand
        {
            private readonly Action _TargetExecuteMethod;


            public GUIICommand(Action executeMethod)
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

        public class GUIICommand<T> : ICommand
        {
            private readonly Action<T> _TargetExecuteMethod;
            private readonly Func<T, bool> _TargetCanExecuteMethod;

            public GUIICommand(Action<T> executeMethod)
            {
                _TargetExecuteMethod = executeMethod;
            }

            public GUIICommand(Action<T> executeMethod, Func<T, bool> func)
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
