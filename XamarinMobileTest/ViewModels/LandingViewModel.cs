using System.Windows.Input;
namespace XamarinMobileTest
{
	public class LandingViewModel : BaseViewModel
	{
		#region Properties

		string _message;
		public string Message
		{
			get
			{ return _message; }
			set
			{
				_message = value;
				OnPropertyChanged(nameof(Message));
			}
		}

		string _userMessage;
		public string UserMessage
		{
			get
			{ return _userMessage; }
			set
			{
				_userMessage = value;
				OnPropertyChanged(nameof(UserMessage));
			}
		}

		#endregion

		#region Commands

		public ICommand OkButtonCommand
		{ get { return new DelegateCommand((o) => Message = UserMessage); } }

		#endregion
	}
}
