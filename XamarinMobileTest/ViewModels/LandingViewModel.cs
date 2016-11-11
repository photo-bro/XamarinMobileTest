using System.Windows.Input;
namespace XamarinMobileTest
{
	public class LandingViewModel : BaseViewModel
	{
		string _message;
		public string Message {
			get {
				return _message;
			}
			set {
				_message = value;
				OnPropertyChanged ("Message");
			}
		}

		string _userMessage;
		public string UserMessage {
			get {
				return _userMessage;
			}
			set {
				_userMessage = value;
				OnPropertyChanged ("UserMessage");
			}
		}

		public ICommand OkButtonCommand {
			get {
				return new DelegateCommand ((obj) => { Message = UserMessage; });
			}
		}
	}
}
