using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.IO;

namespace PlaylistSearcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string playlistPath;
        private string destinationFolder = @"%UserProfile%\Desktop\Folder";
        private int countFilesToCopy;
        private string musicFilePathOld;
        private string musicFilePathNew;
        private bool stopToCopy = false;

        List<string> musicFiles = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void LabelClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed) Close();
        }

        private void ButtonChoosePlaylist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDestinationFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonStopCopy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonCopy_Click(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(destinationFolder);
        }
    }
}
