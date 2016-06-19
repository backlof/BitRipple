using BitRipple.Services;
using BitRipple.Utilities;
using BitRipple.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BitRipple.View;

namespace BitRipple.ViewModel
{
	public class FeedEditorViewModel : NotifyBase
	{
		public FeedEditorViewModel(ApplicationService application, IWindowService windowService)
		{
			Application = application;
			WindowService = windowService;
			Window = WindowService.GetFeedEditorWindow();

			onPropertyChanged("Title");
			Feed = new Feed(CurrentFeedInApplication.Feed);
		}

		public ApplicationService Application { get; set; }
		public IWindowService WindowService { get; set; }
		public WindowBase Window { get; set; }

		public FeedEdit CurrentFeedInApplication
		{
			get
			{
				return Application.CurrentEditedFeed;
			}
			set
			{
				Application.CurrentEditedFeed = value;
			}
		}


		private Feed _Feed;
		public Feed Feed
		{
			get
			{
				return _Feed;
			}
			set
			{
				_Feed = value;
				onPropertyChanged("Feed");
			}
		}

		public string Title
		{
			get
			{
				return CurrentFeedInApplication.Type == FeedEditType.Edit ? "Edit Feed" : "New Feed";
			}
		}

		#region Close COMMAND
		public ICommand CloseCommand
		{
			get
			{
				return new RelayCommand(ExecuteCloseCommand, CanCloseCommand);
			}
		}

		public void ExecuteCloseCommand(object parameter)
		{
			CurrentFeedInApplication = null;
			WindowService.SaveFeedEditorWindow(Window);
			Application.Save();
		}

		public bool CanCloseCommand(object parameter)
		{
			return true;
		}
		#endregion

		#region Cancel COMMAND
		public ICommand CancelCommand
		{
			get
			{
				return new RelayCommand(ExecuteCancelCommand, CanCancelCommand);
			}
		}

		public void ExecuteCancelCommand(object parameter)
		{
			FeedEditorWindow window = parameter as FeedEditorWindow;
			CurrentFeedInApplication = null;
			window.Close();
		}

		public bool CanCancelCommand(object parameter)
		{
			return true;
		}
		#endregion


		#region Save COMMAND
		public ICommand SaveCommand
		{
			get
			{
				return new RelayCommand(ExecuteSaveCommand, CanSaveCommand);
			}
		}

		public void ExecuteSaveCommand(object parameter)
		{
			FeedEditorWindow window = parameter as FeedEditorWindow;

			if (CurrentFeedInApplication.Type == FeedEditType.Edit)
			{
				int index = Application.Feeds.IndexOf(CurrentFeedInApplication.Feed);
				Application.Feeds[index] = Feed;
			}
			if (CurrentFeedInApplication.Type == FeedEditType.New)
			{
				Application.Feeds.Add(Feed);
			}

			CurrentFeedInApplication = null;
			window.Close();
		}

		public bool CanSaveCommand(object parameter)
		{
			return true;
		}
		#endregion

	}
}
