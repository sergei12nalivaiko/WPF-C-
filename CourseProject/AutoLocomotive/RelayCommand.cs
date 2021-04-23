using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoLocomotive
{
    class RelayCommand:ICommand
    {
        private Action executeAction;
        private Func<bool> canExecute;
        
        public RelayCommand(Action executeAction, Func<bool> canExecute)
        {
            this.canExecute = canExecute;
            this.executeAction=executeAction;   
        }

        public bool CanExecute(object parameter)
        {
           if (canExecute==null)
            {
                return true;
            }
            else
            {
                return canExecute();
            }
        }

         public event EventHandler CanExecuteChanged
                {
                    add
                    {   
                            CommandManager.RequerySuggested += value; 
                    }
                    remove
                    {
         
                            CommandManager.RequerySuggested -= value; 
                   }
            }

        public void Execute(object parameter)
        {
            executeAction();
        }
    }
    
}
