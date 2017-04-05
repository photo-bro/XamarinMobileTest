using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Linq;


namespace XamarinMobileTest
{
	public abstract class BaseViewModel : INotifyPropertyChanged, IDisposable
	{

		#region Constructors

		protected BaseViewModel() : this(null, new List<BaseViewModel>())
		{ }

		protected BaseViewModel(BaseViewModel parent, IList<BaseViewModel> children)
		{
			Parent = parent;
			Children = children;

			InitParentAndChildRelationships();
		}

		#endregion

		#region Base Properties

		protected BaseViewModel Parent { get; private set; }

		protected IList<BaseViewModel> Children { get; private set; }

		#endregion

		#region Base Methods

		protected void AddChild(BaseViewModel child)
		{
			if (child == null)
				throw new ArgumentNullException(nameof(child));

			if (Children == null)
				Children = new List<BaseViewModel>();

			child.Parent = this;

			// Connect PropertyChanged events to proper handlers
			// Parent -> Child
			Parent.PropertyChanged += ParentPropertyChanged;
			// Child -> Parent
			child.PropertyChanged += Parent.ChildPropertyChanged;

			// Siblings
			foreach (var sibling in Children.Where(c => c != null))
			{
				// Sibling -> Child
				sibling.PropertyChanged += SiblingPropertyChanged;
				// Child -> Sibling
				child.PropertyChanged += sibling.SiblingPropertyChanged;
			}

			Children.Add(child);
		}

		protected void RemoveChild(BaseViewModel child)
		{
			if (child == null)
				throw new ArgumentNullException(nameof(child));

			// Disconnect PropertyChanged events from connected handlers
			// Parent -> Child
			Parent.PropertyChanged -= ParentPropertyChanged;
			// Child -> Parent
			child.PropertyChanged -= Parent.ChildPropertyChanged;

			if (Children == null || !Children.Contains(child))
				return;

			Children.Remove(child);

			// Siblings
			foreach (var sibling in Children.Where(c => c != null))
			{
				// Sibling -> Child
				sibling.PropertyChanged -= SiblingPropertyChanged;
				// Child -> Sibling
				child.PropertyChanged -= sibling.SiblingPropertyChanged;
			}

			child.Parent = null;
		}

		#endregion

		#region Base Commands

		public ICommand DisposeCommand
		{ get { return new DelegateCommand((o) => Dispose()); } }

		#endregion

		#region Virtual Methods

		protected virtual void Init()
		{
			if (Children == null)
				return;

			foreach (var child in Children.Where(c => c != null))
				child.Init();
		}

		public void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
		{ }

		public void SiblingPropertyChanged(object sender, PropertyChangedEventArgs e)
		{ }

		public void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
		{ }

		#endregion

		#region Private Methods

		void InitParentAndChildRelationships()
		{
			if (Children != null)
				foreach (var child in Children.Where(c => c != null))
					AddChild(child);
			else
				Children = new List<BaseViewModel>();

		}

		void DeInitParentAndChildRelationships()
		{
			if (Children != null)
				foreach (var child in Children.Where(c => c != null))
					RemoveChild(child);
		}

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{ PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

		#endregion

		#region IDisposable 

		public EventHandler<EventArgs> OnDisposal;

		bool disposedValue; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (disposedValue)
				return;

			if (disposing)
			{
				OnDisposal?.Invoke(this, null);
				DeInitParentAndChildRelationships();
			}

			disposedValue = true;
		}

		~BaseViewModel()
		{ Dispose(true); }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}