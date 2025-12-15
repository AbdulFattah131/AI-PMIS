using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MusicPlayer.Data.Objects;
using MusicPlayer.UIComponents.ViewModels;
using MusicPlayer.Utility;

namespace MusicPlayer.UIComponents
{
    /// <summary>
    /// Interaction logic for ThemeDesigner.xaml
    /// </summary>
    public partial class ThemeDesigner : Window
    {
        private ThemeDesignerViewModel m_vm;
        public ThemeDesigner()
        {
            InitializeComponent();
            var mainWindowVM = new MainWindowViewModel();
            m_vm = new ThemeDesignerViewModel(mainWindowVM);
            this.DataContext = m_vm;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ThemeWriter.Instance.WriteToFile(m_vm.CustomTheme);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var theme = MainWindowViewModel.Instance.CurrentTheme;
            bool isDefault = theme?.Name is "Lavender" or "Lavender Dark";

            tbSwitchTheme.Visibility = isDefault ? Visibility.Visible : Visibility.Hidden;
            tbSwitchTheme.IsEnabled = isDefault;
            tbSwitchTheme.IsChecked = theme?.Name == "Lavender Dark";
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e == null || sender == null)
                return;

            TextBox itbQueryTextBox = sender as TextBox;
            string stQueryText = itbQueryTextBox.Text;
            Brush tempBrush = null;

            try
            {
                tempBrush = (SolidColorBrush)(new BrushConverter().ConvertFromString(stQueryText));
            }
            catch (Exception ex)
            {
                itbQueryTextBox.BorderBrush = Brushes.DarkGray;
                return;
            }

            if (itbQueryTextBox.Name == "itbName")
            {
                m_vm.CustomTheme.Name = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbWindowTextForegroundHex")
            {
                m_vm.CustomTheme.WindowTextForeground.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbWindowHeadingForegroundHex")
            {
                m_vm.CustomTheme.WindowHeadingForeground.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbWindowPrimaryAnchorHex")
            {
                m_vm.CustomTheme.WindowPrimaryAnchor.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbSearchBoxBackgroundHex")
            {
                m_vm.CustomTheme.SearchBoxBackground.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbWindowPrimarySliderHex")
            {
                m_vm.CustomTheme.WindowPrimarySlider.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbWindowPrimaryHex")
            {
                m_vm.CustomTheme.WindowPrimary.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbWindowTitleForegroundHex")
            {
                m_vm.CustomTheme.WindowTitleForeground.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbWindowContentBackgroundHex")
            {
                m_vm.CustomTheme.WindowContentBackground.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbComplementGrayHex")
            {
                m_vm.CustomTheme.ComplementGray.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbCurrentSongArtistForegroundHex")
            {
                m_vm.CustomTheme.CurrentSongArtistForeground.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbPlayerControlBackgroundHex")
            {
                m_vm.CustomTheme.PlayerControlBackground.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbTitleBarBackgroundHex")
            {
                m_vm.CustomTheme.TitleBarBackground.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbListBoxHoverHex")
            {
                m_vm.CustomTheme.ListBoxHover.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbListBoxSelectedHex")
            {
                m_vm.CustomTheme.ListBoxSelected.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbLeftToggleHoverHex")
            {
                m_vm.CustomTheme.LeftToggleHover.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbLeftToggleSelectedHex")
            {
                m_vm.CustomTheme.LeftToggleSelected.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbPlayPauseBackgroundHex")
            {
                m_vm.CustomTheme.PlayPauseBackground.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbGenresTextForegroundHex")
            {
                m_vm.CustomTheme.GenresTextForeground.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbNowPlayingForegroundHex")
            {
                m_vm.CustomTheme.NowPlayingForeground.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbNewPlaylistHoverHex")
            {
                m_vm.CustomTheme.NewPlaylistHover.Hex = stQueryText;
            }
            if (itbQueryTextBox.Name == "itbPlaylistsToggleSelectedHex")
            {
                m_vm.CustomTheme.PlaylistsToggleSelected.Hex = stQueryText;
            }

            itbQueryTextBox.BorderBrush = Brushes.Gray;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ThemeWriter.Instance.WriteToFile(m_vm.CustomTheme);
            MessageBox.Show("Theme saved!");
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            m_vm.CustomTheme = (Theme)(cbThemes.SelectedItem);
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {

            var mainWindow = Application.Current.MainWindow as MainWindow;
            var td_vm = (ThemeDesignerViewModel)DataContext;
            var theme = cbThemes.SelectedItem as Theme ?? td_vm.CustomTheme;

            MainWindowViewModel.Instance.CurrentTheme = theme;

            bool isDefault = theme?.Name is "Lavender" or "Lavender Dark";

            tbSwitchTheme.Visibility = isDefault ? Visibility.Visible : Visibility.Hidden;
            tbSwitchTheme.IsEnabled = isDefault;

            if (theme?.Name == "Lavender Dark")
                tbSwitchTheme.IsChecked = true;
            else
                tbSwitchTheme.IsChecked = false;
        }

        // Switch Button
        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            var m_vm = MainWindowViewModel.Instance.SwitchTheme;
            m_vm.Invoke();

            var lavenderDarkTheme = cbThemes.Items
                .Cast<Theme>()
                .FirstOrDefault(t => t.Name == "Lavender Dark");

            if (lavenderDarkTheme != null)
            {
                cbThemes.SelectedItem = lavenderDarkTheme;
            }
        }
    }
}
