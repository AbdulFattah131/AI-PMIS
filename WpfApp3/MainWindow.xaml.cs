using MusicPlayer.Data.Objects;
using MusicPlayer.UIComponents.Constants;
using MusicPlayer.UIComponents.ViewModels;
using MusicPlayer.Utility;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;


namespace MusicPlayer.UIComponents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ThemeDesignerViewModel themeDesignerVM;

        public MainWindowViewModel m_vm;

        private AudioPlayer _player;

        public MainWindow()
        {
            InitializeComponent();

            InitializeViewModel();

            LoadThemes();

            InitializeUIState();

            ApplyRestoreWindow();

            _player = new AudioPlayer();
            m_vm.MusicPlayerCache.StartClock();
        }
       
        
        private void InitializeViewModel()
        {             
            //Initialize ViewModel
            m_vm = MainWindowViewModel.Instance;
            this.DataContext = m_vm;
        }

        private void LoadThemes()
        { 
            //Load Themes
            foreach (Theme objTheme in ThemeReader.Instance.GetThemes())
            {
                if (m_vm.Settings.CurrentThemeName == objTheme.Name)
                {
                    m_vm.CurrentTheme = objTheme;
                    break;
                }
            }
        }
        private void InitializeUIState()
        { 
            //Initialize UI State
            btnToggleAlbums.IsChecked = true;
        }

        // Title Bar
        private void ApplyRestoreWindow()
        {
            this.Left = m_vm.Settings.LastWindowCoordinates.X;
            this.Top = m_vm.Settings.LastWindowCoordinates.Y;
            this.Width = m_vm.Settings.LastWindowDimensions.X;
            this.Height = m_vm.Settings.LastWindowDimensions.Y;
        }

        #region Theme designer Button
        private void bdrThemeDesigner_MouseEnter(object sender, MouseEventArgs e)
        {
            grdThemeDesigner.Background = Brushes.DarkGray;
        }

        private void bdrThemeDesigner_MouseLeave(object sender, MouseEventArgs e)
        {
            grdThemeDesigner.Background = null;
        }

        private ThemeDesigner _themeDesigner;
        private void bdrThemeDesigner_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_themeDesigner == null || !_themeDesigner.IsLoaded)
            {
                _themeDesigner = new ThemeDesigner();
                _themeDesigner.Show();
            }
            else
            {
                _themeDesigner.Activate();
            }
        }

        #endregion

        #region Settings Button
        private void bdrSettings_MouseEnter(object sender, MouseEventArgs e)
        {
            grdSettings.Background = Brushes.DarkGray;

        }

        private void bdrSettings_MouseLeave(object sender, MouseEventArgs e)
        {
            grdSettings.Background = null;
        }

        private void bdrSettings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RefreshSongsAndAlbums();
        }

        private void RefreshSongsAndAlbums()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            else
            {
                m_vm.MusicPlayerCache.Clear();
                FileScanner.Instance.UpdateFilePath(dialog.SelectedPath);
            }

            m_vm.Init();
        }
        #endregion

        #region Close Button
        private void bdrClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void bdrClose_MouseEnter(object sender, MouseEventArgs e)
        {
            bdrClose.Background = (m_vm.CurrentTheme.WindowPrimary.Brush);
        }

        private void bdrClose_MouseLeave(object sender, MouseEventArgs e)
        {
            bdrClose.Background = Brushes.Transparent;
        }

        #endregion

        #region Minimize Button
        private void bdrMinimize_MouseEnter(object sender, MouseEventArgs e)
        {
            grdMinimize.Background = Brushes.DarkGray;
        }

        private void bdrMinimize_MouseLeave(object sender, MouseEventArgs e)
        {
            grdMinimize.Background = Brushes.Transparent;
        }

        private void bdrMinimize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        #endregion

        #region Resize
        private void bdrResize_MouseEnter(object sender, MouseEventArgs e)
        {
            grdResize.Background = Brushes.DarkGray;
        }

        private void bdrResize_MouseLeave(object sender, MouseEventArgs e)
        {
            grdResize.Background = null;
        }

        private void bdrResize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                tbResize.Text = "🗗";
            }
            else
            {
                this.WindowState = WindowState.Normal;
                tbResize.Text = "🗖";
            }
        }

        // Double Click
        private void bdrTitleBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                }
                return;
            }
        }

        #endregion

        #region On Window Close
        private void Window_Closed(object sender, EventArgs e)
        {
            m_vm.Settings.CurrentThemeName = m_vm.CurrentTheme.Name;
            m_vm.Settings.LastWindowCoordinates = new Point(this.Left, this.Top);
            m_vm.Settings.LastWindowDimensions = new Point(this.Width, this.Height);
       

            SettingsWriter.Instance.WriteToFile(m_vm.Settings);
        }

        #endregion

        #region Move Window (through title bar)
        private void borderWindowMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == e.LeftButton)
                this.DragMove();
        }

        #endregion

        // Music Control

        #region Playback Controls
        private void tbPlayPause_Click(object sender, RoutedEventArgs e)
        {
            m_vm.MusicPlayerCache.PlayPause();
        }
     

        private void tbPrevious_Click(object sender, RoutedEventArgs e)
        {
            m_vm.MusicPlayerCache.Previous();
        }

        private void tbNext_Click(object sender, RoutedEventArgs e)
        {
            m_vm.MusicPlayerCache.Next();
        }

        private void tbShuffle_Click(object sender, RoutedEventArgs e)
        {
            m_vm.MusicPlayerCache.Shuffle();
        }

        #endregion

        #region Music Player Seek Bar
        private void sdrMusicPlayerSeekBar_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is not MainWindowViewModel m_vm)
                return;

            var slider = sender as Slider;
            if (slider == null) return;

            m_vm.MusicPlayerCache.IsUserDragging = true;

            // Check what was clicked
            if (e.OriginalSource is FrameworkElement fe && fe.TemplatedParent is Thumb)
                return; // allow normal thumb dragging, do NOT handle

            var track = slider.Template.FindName("PART_Track", slider) as System.Windows.Controls.Primitives.Track;
            if (track == null) return;

            Point clickPoint = e.GetPosition(track);
            double ratio = clickPoint.X / track.ActualWidth;
            double newValue = ratio * (slider.Maximum - slider.Minimum) + slider.Minimum;

            slider.Value = newValue;
            double normalized = newValue / slider.Maximum;

            m_vm.MusicPlayerCache.Player.Seek(newValue);

            e.Handled = true; // only handle non-thumb clicks

        }

        private void sdrMusicPlayerSeekBar_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainWindowViewModel m_vm)
            {
                m_vm.MusicPlayerCache.IsUserDragging = false;
                m_vm.MusicPlayerCache.CurrentPosition = (int)((Slider)sender).Value;
            }
        }

        #endregion

        private new void PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer == null) return;

            // Move the content
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;
        }
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var clickedToggle = sender as ToggleButton;
            foreach (var child in ToggleGroup.Children)
            {
                if (child is Border border)
                {
                    if (border.Child is ToggleButton toggle)
                    {
                        if (toggle != clickedToggle) 
                        {
                            toggle.IsChecked = false;
                        }
                        else
                        {
                            if (toggle.Name == "btnToggleAlbums")
                            {
                                //lbAllSongsList.SelectionChanged -= AllSongsListBox_SelectionChanged;

                                //lbAlbumContents.SelectionChanged -= AlbumContentsListBox_SelectionChanged;
                                //lbAlbumContents.SelectionChanged += AlbumContentsListBox_SelectionChanged;
                            }

                            if (toggle.Name == "btnToggleSongs")
                            {
                                //lbAlbumContents.SelectionChanged -= AlbumContentsListBox_SelectionChanged;

                                //lbAllSongsList.SelectionChanged -= AllSongsListBox_SelectionChanged;
                                //lbAllSongsList.SelectionChanged += AllSongsListBox_SelectionChanged;
                            }

                            if (toggle.Name == "btnPlaylistSongs")
                            {
                                //lbAlbumContents.SelectionChanged -= AlbumContentsListBox_SelectionChanged;
                                //lbAllSongsList.SelectionChanged -= AllSongsListBox_SelectionChanged;

                                //lbPlaylistList.SelectionChanged -= lbPlaylistList_SelectionChanaged;
                                //lbPlaylistList.SelectionChanged += lbPlaylistList_SelectionChanaged;
                            }
                        }
                    }
                }
            }
        }
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;

            if (sender is ToggleButton btn)
            {
                btn.IsChecked = true;
            }
        }
        
        #region New Playlist Button

        private PlaylistsWindow _playlistsWindow;
        private void NewPlaylist_Click(object sender, RoutedEventArgs e)
        {
            if (_playlistsWindow == null || !_playlistsWindow.IsLoaded)
            {
                _playlistsWindow = new PlaylistsWindow();
                _playlistsWindow.Owner = this;
                _playlistsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                _playlistsWindow.Show();
            }
            else
            {
                _playlistsWindow.Activate();
            }
        }

        #endregion

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as ListBox)?.ScrollIntoView((sender as ListBox)?.SelectedItem);
        }

        private void AlbumContentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as ListBox)?.ScrollIntoView((sender as ListBox)?.SelectedItem);
        }

        private void AllSongsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox element = sender as ListBox;
            (sender as ListBox)?.ScrollIntoView((sender as ListBox)?.SelectedItem);
            if (MainWindowViewModel.Instance.MusicPlayerCache.CurrentMode != ENMusicPlayerMode.Songs)
                MainWindowViewModel.Instance.MusicPlayerCache.CurrentMode = ENMusicPlayerMode.Songs;
        }
        private void PlaylistsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as ListBox)?.ScrollIntoView((sender as ListBox)?.SelectedItem);
        }
    }
}

