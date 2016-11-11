using System;
using System.Windows.Input;
namespace XamarinMobileTest
{
	public class DelegateCommand : ICommand
	{
		Predicate<object> _canExecute { get; set; }
		Action<object> _execute { get; set; }

		public event EventHandler CanExecuteChanged;

		public DelegateCommand (Action<object> execute) : this (execute, null)
		{ }

		public DelegateCommand (Action<object> execute, Predicate<object> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute (object parameter)
		{
			return _canExecute == null ? true : _canExecute (parameter);
		}

		public void Execute (object parameter)
		{
			_execute (parameter);
		}

		public void RaiseCanExecuteChanged ()
		{
			if (CanExecuteChanged == null)
				return;
			CanExecuteChanged (this, EventArgs.Empty);
		}
	}
}
