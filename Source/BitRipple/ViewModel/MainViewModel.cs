using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitRipple.Services;
using System.Collections.ObjectModel;
using BitRipple.Model;
using BitRipple.Utilities;
using System.Windows.Input;
using System.IO;
using System.Collections;
using System.Windows.Threading;
using System.Windows.Shell;

namespace BitRipple.ViewModel
{
	public class MainViewModel : NotifyBase
	{
		public MainViewModel(ApplicationService application, IWindowService windowService)
		{
			Application = application;
			WindowService = windowService;

			Window = WindowService.GetMainWindow();

			onPropertyChanged("SelectedFilterIndex");
			onPropertyChanged("SelectedFeedIndex");
			onPropertyChanged("SelectedUpdateFrequency");
			onPropertyChanged("TorrentDropDirectory");

			StartTimer();
		}

		public ApplicationService Application { get; set; }
		public IWindowService WindowService { get; set; }
		public MainWindowSettings Window { get; set; }
		private DispatcherTimer timedUpdate;

		#region Properties

		public string[] UpdateOptions
		{
			get
			{
				return ApplicationSettings.UPDATE_FREQUENCY_OPTIONS;
			}
		}

		public string LatestDownload
		{
			get
			{
				var downloads = Application.DownloadedTorrents;
				var count = downloads.Count;
				return count > 0 ? downloads[count - 1].Title : "None";
			}
		}

		private bool _IsUpdating = false; // Should be the one to show green loading indicator?
		public bool IsUpdating
		{
			get
			{
				return _IsUpdating;
			}
			set
			{
				_IsUpdating = value;
				onPropertyChanged("IsUpdating");
			}
		}

		private bool _IsSaving = false;
		public bool IsSaving
		{
			get
			{
				return _IsSaving;
			}
			set
			{
				_IsSaving = value;
				onPropertyChanged("IsSaving");
			}
		}


		public ObservableCollection<Filter> Filters
		{
			get
			{
				return Application.Filters;
			}
			set
			{
				Application.Filters = value;
				onPropertyChanged("Filters");
			}
		}

		public ObservableCollection<Feed> Feeds
		{
			get
			{
				return Application.Feeds;
			}
			set
			{
				Application.Feeds = value;
				onPropertyChanged("Feeds");
			}
		}

		public ObservableCollection<Torrent> DownloadedTorrents
		{
			get
			{
				return Application.DownloadedTorrents;
			}
			set
			{
				Application.DownloadedTorrents = value;
				onPropertyChanged("DownloadedTorrents");
			}
		}

		public int SelectedFilterIndex
		{
			get
			{
				return Application.State.SelectedFilterIndex;
			}
			set
			{
				Application.State.SelectedFilterIndex = value;
				onPropertyChanged("SelectedFilterIndex");
			}
		}

		public int SelectedFeedIndex
		{
			get
			{
				return Application.State.SelectedFeedIndex;
			}
			set
			{
				Application.State.SelectedFeedIndex = value;
				onPropertyChanged("SelectedFeedIndex");
			}
		}

		public DateTime LastUpdate
		{
			get
			{
				return Application.State.LastUpdate ?? DateTime.MinValue;
			}
			set
			{
				Application.State.LastUpdate = value;
				onPropertyChanged("LastUpdate");
			}
		}

		public int CountUpdates
		{
			get
			{
				return Application.State.CountUpdates;
			}
			set
			{
				Application.State.CountUpdates = value;
				onPropertyChanged("CountUpdates");
			}
		}

		public int SelectedUpdateFrequency
		{
			get
			{
				return Application.State.SelectedUpdateFrequency;
			}
			set
			{
				Application.State.SelectedUpdateFrequency = value;
				onPropertyChanged("SelectedUpdateFrequency");
			}
		}

		public string TorrentDropDirectory
		{
			get
			{
				return Application.State.TorrentDropDirectory;
			}
			set
			{
				Application.State.TorrentDropDirectory = value;
				onPropertyChanged("TorrentDropDirectory");
			}
		}

		private bool _WindowActive;
		public bool WindowActive
		{
			get
			{
				return _WindowActive;
			}
			set
			{
				_WindowActive = value;
				onPropertyChanged("WindowActive");
			}
		}


		#endregion

		#region OneWay bindings

		private Filter _SelectedFilter;
		public Filter SelectedFilter
		{
			get
			{
				return _SelectedFilter;
			}
			set
			{
				_SelectedFilter = value;
				onPropertyChanged("SelectedFilter");
			}
		}

		private Feed _SelectedFeed;
		public Feed SelectedFeed
		{
			get
			{
				return _SelectedFeed;
			}
			set
			{
				_SelectedFeed = value;
				onPropertyChanged("SelectedFeed");
			}
		}

		#endregion

		#region Functionality

		private void Save()
		{
			this.IsSaving = true;
			Application.Save();
			this.IsSaving = false;
		}

		private void ForceUpdate()
		{
			ResetTimer();
			Update();
		}

		private async void Update()
		{
			IsUpdating = true; NotifyLoading();

			bool hasDownloaded = await Application.Update();

			if (hasDownloaded)
			{
				NotifyNow();
			}
			else
			{
				NotifyDeactivate();
			}

			IsUpdating = false;
			CountUpdates++;
			LastUpdate = DateTime.Now;
			Save();

			if (hasDownloaded)
			{
				onPropertyChanged("Filters");
				onPropertyChanged("Feeds");
				onPropertyChanged("DownloadedTorrents");
				onPropertyChanged("LatestDownload");
			}
		}

		#endregion

		#region Notification

		private TaskbarItemProgressState _NotificationState = TaskbarItemProgressState.None;
		public TaskbarItemProgressState NotificationState
		{
			get
			{
				return _NotificationState;
			}
			set
			{
				_NotificationState = value;
				onPropertyChanged("NotificationState");
			}
		}

		public bool IsNotifying
		{
			get
			{
				return NotificationState == TaskbarItemProgressState.Normal; // Not affecting loading
			}
		}

		public void NotifyLoading()
		{
			NotificationState = TaskbarItemProgressState.Indeterminate;
		}

		public void NotifyNow()
		{
			NotificationState = TaskbarItemProgressState.Normal;

			if (WindowActive)
			{
				NotifyTimedDeactive(2000);
			}
		}

		private async void NotifyTimedDeactive(int millisecs)
		{
			await System.Threading.Tasks.Task.Delay(millisecs);
			NotifyDeactivate();
		}

		public void NotifyDeactivate()
		{
			NotificationState = TaskbarItemProgressState.None;
		}

		#region WindowDeactivated COMMAND
		public ICommand WindowDeactivatedCommand
		{
			get
			{
				return new RelayCommand(ExecuteWindowDeactivatedCommand, CanWindowDeactivatedCommand);
			}
		}

		public void ExecuteWindowDeactivatedCommand(object parameter)
		{
			WindowActive = false;
		}

		public bool CanWindowDeactivatedCommand(object parameter)
		{
			return true;
		}
		#endregion

		#region WindowActivated COMMAND
		public ICommand WindowActivatedCommand
		{
			get
			{
				return new RelayCommand(ExecuteWindowActivatedCommand, CanWindowActivatedCommand);
			}
		}

		public void ExecuteWindowActivatedCommand(object parameter)
		{
			if (IsNotifying) { NotificationState = TaskbarItemProgressState.None; }
			WindowActive = true;
		}

		public bool CanWindowActivatedCommand(object parameter)
		{
			return true;
		}
		#endregion


		#endregion

		#region Timer

		public void StartTimer()
		{
			timedUpdate = new DispatcherTimer();
			timedUpdate.Tick += new EventHandler(TickEvent);
			timedUpdate.Interval = new TimeSpan(0, UpdateFrequency, 0); // Hour, minute, second
			timedUpdate.Start();
			NextUpdate = DateTime.Now.AddMinutes(UpdateFrequency);
		}

		public void ResetTimer()
		{
			timedUpdate.Stop();
			StartTimer();
		}

		private void TickEvent(object sender, EventArgs e)
		{
			NextUpdate = DateTime.Now.AddMinutes(UpdateFrequency);
			this.Update();
		}

		private int UpdateFrequency
		{
			get
			{
				return ApplicationSettings.UPDATE_FREQUENCY_IN_MINUTES[SelectedUpdateFrequency];
			}
		}

		private DateTime _NextUpdate;
		public DateTime NextUpdate
		{
			get
			{
				return _NextUpdate;
			}
			set
			{
				_NextUpdate = value;
				onPropertyChanged("NextUpdate");
			}
		}

		#endregion

		#region Commands

		#region Window Closing COMMAND
		public ICommand CloseCommand
		{
			get
			{
				return new RelayCommand(ExecuteClose, CanClose);
			}
		}

		public void ExecuteClose(object parameter)
		{
			WindowService.SaveMainWindow(Window);
			Application.Save();
		}

		public bool CanClose(object parameter)
		{
			return true;
		}
		#endregion

		#region Choose a Directory COMMAND
		public ICommand ChooseDirectory
		{
			get
			{
				return new RelayCommand(ExecuteChooseDirectory, CanChooseDirectory);
			}
		}

		public void ExecuteChooseDirectory(object parameter)
		{
			using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				try
				{
					dialog.SelectedPath = Path.GetFullPath(TorrentDropDirectory);
				}
				catch (ArgumentException)
				{
					dialog.SelectedPath = ApplicationSettings.DEFAULT_TORRENT_DROP_FOLDER;
				}

				System.Windows.Forms.DialogResult result = dialog.ShowDialog();

				if (result == System.Windows.Forms.DialogResult.OK)
				{
					TorrentDropDirectory = Paths.MakeRelativeDirectory(dialog.SelectedPath);
				}
			}
		}

		public bool CanChooseDirectory(object parameter)
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
			Save();
		}

		public bool CanSaveCommand(object parameter)
		{
			return !IsUpdating && !IsSaving;
		}
		#endregion

		#region Delete Filter COMMAND
		public ICommand DeleteFilterCommand
		{
			get
			{
				return new RelayCommand(ExecuteDeleteFilterCommand, CanDeleteFilterCommand);
			}
		}

		public void ExecuteDeleteFilterCommand(object parameter)
		{
			Filter filter = parameter as Filter;
			int index = Filters.IndexOf(filter);

			foreach (var downloadedTorrent in DownloadedTorrents)
			{
				if (downloadedTorrent.Download.CaughtByFilter == index) { downloadedTorrent.Download.CaughtByFilter = -1; }
				if (downloadedTorrent.Download.CaughtByFilter < index) { downloadedTorrent.Download.CaughtByFilter--; }
			}

			Filters.RemoveAt(index);
		}

		public bool CanDeleteFilterCommand(object parameter)
		{
			return !IsSaving && !IsUpdating && parameter != null;
		}
		#endregion

		#region Delete Feed COMMAND
		public ICommand DeleteFeedCommand
		{
			get
			{
				return new RelayCommand(ExecuteDeleteFeedCommand, CanDeleteFeedCommand);
			}
		}

		public void ExecuteDeleteFeedCommand(object parameter)
		{

			Feed feed = parameter as Feed;
			int index = Feeds.IndexOf(feed);

			SelectedFilterIndex = -1;

			foreach (var filter in Filters.Where(x => x.Feed == feed))
			{
				filter.Feed = null; filter.FeedIndex = -1;
			}

			Feeds.RemoveAt(index);
		}

		public bool CanDeleteFeedCommand(object parameter)
		{
			return parameter != null;
		}
		#endregion

		#region Remove Downloaded Torrent COMMAND
		public ICommand RemoveDownloadedTorrentCommand
		{
			get
			{
				return new RelayCommand(ExecuteRemoveDownloadedTorrentCommand, CanRemoveDownloadedTorrentCommand);
			}
		}

		public void ExecuteRemoveDownloadedTorrentCommand(object parameter)
		{
			var indexOfSelectedItems = parameter as IList<int>;
			foreach (var index in indexOfSelectedItems)
			{
				Application.DownloadedTorrents.RemoveAt(index);
			}

			// Get more than 1 from commands
			// http://stackoverflow.com/questions/6398046/wpf-datagrid-remove-selecteditems
			// http://stackoverflow.com/questions/18257516/how-to-pass-listbox-selecteditem-as-command-parameter-in-a-button
			// http://stackoverflow.com/questions/25452189/pass-selected-item-as-command-parameter-vs-using-a-bound-viewmodel-object-mvvm
		}

		public bool CanRemoveDownloadedTorrentCommand(object parameter)
		{
			var indexOfSelectedItems = parameter as IList<int>;
			return indexOfSelectedItems.Count > 0;
		}
		#endregion

		#region Manual Update COMMAND
		public ICommand UpdateCommand
		{
			get
			{
				return new RelayCommand(ExecuteUpdateCommand, CanUpdateCommand);
			}
		}

		public void ExecuteUpdateCommand(object parameter)
		{
			ForceUpdate();
		}

		public bool CanUpdateCommand(object parameter)
		{
			return !IsUpdating && !IsSaving;
		}
		#endregion

		#region OpenManual COMMAND
		public ICommand OpenManualCommand
		{
			get
			{
				return new RelayCommand(ExecuteOpenManualCommand, CanOpenManualCommand);
			}
		}

		public void ExecuteOpenManualCommand(object parameter)
		{
			System.Diagnostics.Process.Start("http://github.com/backlof/BitRipple/blob/master/README.md");
		}

		public bool CanOpenManualCommand(object parameter)
		{
			return true;
		}
		#endregion

		#region ExitApplication COMMAND
		public ICommand ExitApplicationCommand
		{
			get
			{
				return new RelayCommand(ExecuteExitApplicationCommand, CanExitApplicationCommand);
			}
		}

		public void ExecuteExitApplicationCommand(object parameter)
		{
			WindowService.SaveMainWindow(Window);
			Application.Save();
			System.Windows.Application.Current.Shutdown();
		}

		public bool CanExitApplicationCommand(object parameter)
		{
			return !IsUpdating && !IsSaving;
		}
		#endregion

		#region IntervalChanged COMMAND
		public ICommand IntervalChangedCommand
		{
			get
			{
				return new RelayCommand(ExecuteIntervalChangedCommand, CanIntervalChangedCommand);
			}
		}

		public void ExecuteIntervalChangedCommand(object parameter)
		{
			ResetTimer();
		}

		public bool CanIntervalChangedCommand(object parameter)
		{
			return !IsUpdating && !IsSaving;
		}
		#endregion

		#region Create Filter COMMAND
		public ICommand CreateFilterCommand
		{
			get
			{
				return new RelayCommand(ExecuteCreateFilterCommand, CanCreateFilterCommand);
			}
		}

		public void ExecuteCreateFilterCommand(object parameter)
		{
			var filter = parameter as Filter;
			if (filter != null)
			{
				System.Diagnostics.Debug.WriteLine(filter.ToStringExpanded());
			}

			//copy parts of feed when creating new
		}

		public bool CanCreateFilterCommand(object parameter)
		{
			return !IsUpdating && !IsSaving;
		}
		#endregion

		#region Create Feed COMMAND
		public ICommand CreateFeedCommand
		{
			get
			{
				return new RelayCommand(ExecuteCreateFeedCommand, CanCreateFeedCommand);
			}
		}

		public void ExecuteCreateFeedCommand(object parameter)
		{
			Application.CurrentEditedFeed = new FeedEdit()
			{
				Feed = new Feed()
				{
					Title = "",
					URL = ""
				},
				Type = FeedEditType.New
			};
			new View.FeedEditorWindow().Show();
		}

		public bool CanCreateFeedCommand(object parameter)
		{
			return Application.CurrentEditedFeed == null && !IsUpdating && !IsSaving;
		}
		#endregion

		#region Edit Feed COMMAND
		public ICommand EditFeedCommand
		{
			get
			{
				return new RelayCommand(ExecuteEditFeedCommand, CanEditFeedCommand);
			}
		}

		public void ExecuteEditFeedCommand(object parameter)
		{
			Feed feed = parameter as Feed;

			Application.CurrentEditedFeed = new FeedEdit()
			{
				Feed = feed,
				Type = FeedEditType.Edit
			};
			new View.FeedEditorWindow().Show();
		}

		public bool CanEditFeedCommand(object parameter)
		{
			return Application.CurrentEditedFeed == null && parameter != null && !IsUpdating && !IsSaving;
		}
		#endregion

		#region ManuallyDownloadTorrents COMMAND
		public ICommand ManuallyDownloadTorrentsCommand
		{
			get
			{
				return new RelayCommand(ExecuteManuallyDownloadTorrentsCommand, CanManuallyDownloadTorrentsCommand);
			}
		}

		public async void ExecuteManuallyDownloadTorrentsCommand(object parameter)
		{
			IsUpdating = true; NotifyLoading();

			List<Torrent> torrents = (parameter as IList).Cast<Torrent>().ToList();
			List<Task<bool>> tasks = new List<Task<bool>>();

			foreach (Torrent torrent in torrents)
			{
				Torrent copy = new Torrent(torrent);
				copy.Download = new Download()
				{
					CaughtByFilter = -1
				};
				tasks.Add(Application.Download(copy));
			}

			await Task.WhenAll(tasks);
			bool hasDownloaded = tasks.Any(x => x.Result);

			IsUpdating = false;
			if (hasDownloaded) { NotifyNow(); }
			else { NotifyDeactivate(); }

			onPropertyChanged("DownloadedTorrents");
			onPropertyChanged("LatestDownload");
		}

		public bool CanManuallyDownloadTorrentsCommand(object parameter)
		{
			if (parameter == null || IsUpdating || IsSaving)
			{
				return false;
			}

			List<Torrent> torrents = (parameter as IList).Cast<Torrent>().ToList();
			return torrents.Count > 0;
		}
		#endregion

		#region RemoveDownloads COMMAND
		public ICommand RemoveDownloadsCommand
		{
			get
			{
				return new RelayCommand(ExecuteRemoveDownloadsCommand, CanRemoveDownloadsCommand);
			}
		}

		public void ExecuteRemoveDownloadsCommand(object parameter)
		{
			List<Torrent> downloads = (parameter as IList).Cast<Torrent>().ToList();
			foreach (Torrent torrent in downloads)
			{
				DownloadedTorrents.Remove(torrent);
			}
		}

		public bool CanRemoveDownloadsCommand(object parameter)
		{
			if (IsUpdating || IsSaving || parameter == null)
			{
				return false;
			}

			List<Torrent> downloads = (parameter as IList).Cast<Torrent>().ToList();
			return downloads.Count > 0;
		}
		#endregion

		#region LoadHighestEpisode COMMAND
		public ICommand LoadHighestEpisodeCommand
		{
			get
			{
				return new RelayCommand(ExecuteLoadHighestEpisodeCommand, CanLoadHighestEpisodeCommand);
			}
		}

		public void ExecuteLoadHighestEpisodeCommand(object parameter)
		{
			Filter filter = parameter as Filter;
			int indexOfFilter = Filters.IndexOf(filter);
			List<Torrent> downloads = DownloadedTorrents.FindDownloadedTorrents(indexOfFilter);
			filter.LoadHighestEpisode(downloads);
		}

		public bool CanLoadHighestEpisodeCommand(object parameter)
		{
			if (parameter == null)
			{
				return false;
			}

			Filter filter = parameter as Filter;
			int indexOfFilter = Filters.IndexOf(filter);
			List<Torrent> downloads = DownloadedTorrents.FindDownloadedTorrents(indexOfFilter);

			if (downloads.Count == 0)
			{
				return false;
			}

			return filter.HasHigher(downloads);
		}
		#endregion

		#region ForceFilterSelectedFilter COMMAND
		public ICommand ForceFilterSelectedFilterCommand
		{
			get
			{
				return new RelayCommand(ExecuteForceFilterSelectedFilterCommand, CanForceFilterSelectedFilterCommand);
			}
		}

		public async void ExecuteForceFilterSelectedFilterCommand(object parameter)
		{
			Filter filter = parameter as Filter;

			IsUpdating = true; NotifyLoading();

			bool hasDownloaded = await Application.FilterFeed(filter.Feed, filter);

			if (hasDownloaded)
			{
				NotifyNow();
			}
			else
			{
				NotifyDeactivate();
			}

			IsUpdating = false;

			onPropertyChanged("DownloadedTorrents");
			onPropertyChanged("LatestDownload");
		}

		public bool CanForceFilterSelectedFilterCommand(object parameter)
		{
			if (parameter == null || IsUpdating || IsSaving)
			{
				return false;
			}

			Filter filter = parameter as Filter;
			return filter.Enabled && filter.Feed != null;
		}
		#endregion

		#region ResetFilter COMMAND
		public ICommand ResetFilterCommand
		{
			get
			{
				return new RelayCommand(ExecuteResetFilterCommand, CanResetFilterCommand);
			}
		}

		public void ExecuteResetFilterCommand(object parameter)
		{
			Filter filter = parameter as Filter;
			int indexOfFilter = Filters.IndexOf(filter);
			List<Torrent> downloads = DownloadedTorrents.FindDownloadedTorrents(indexOfFilter);

			foreach (Torrent torrent in downloads)
			{
				DownloadedTorrents.Remove(torrent);
			}
			onPropertyChanged("DownloadedTorrents");
			onPropertyChanged("LatestDownload");
		}

		public bool CanResetFilterCommand(object parameter)
		{
			if (parameter == null || IsUpdating || IsSaving)
			{
				return false;
			}

			Filter filter = parameter as Filter;
			int indexOfFilter = Filters.IndexOf(filter);
			List<Torrent> downloads = DownloadedTorrents.FindDownloadedTorrents(indexOfFilter);

			return downloads.Count > 0;
		}
		#endregion

		#endregion
	}
}
