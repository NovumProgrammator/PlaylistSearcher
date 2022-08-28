using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Collections.Generic;

namespace PlaylistSearcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> playListWMP;
        private List<string> musicFilePathList;

        private bool stopCopy = true;
        private string playlistPath;
        private string fileName;
        private string destinationFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\\Playlist Music";
        
        public MainWindow()
        {
            InitializeComponent();
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
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "WPL|*.wpl";

            try
            {
                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    playlistPath = fileDialog.FileName;
                    LabelPath.Content = fileDialog.FileName;
                    playListWMP = new List<string>(File.ReadAllLines(playlistPath));

                    CreateListMedia();

                    ProgressBar.Maximum = 100;
                    ProgressBar.Value = 0;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void ButtonDestinationFolder_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            
            if(folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                destinationFolder = folderDialog.SelectedPath;
            }
        }

        private async void ButtonCopy_Click(object sender, RoutedEventArgs e)
        {
            // default folder
            Directory.CreateDirectory(destinationFolder);

            // progressbar variable | show progress in label under progressbar
            var progress = new Progress<int>(value =>
            {
                ProgressBar.Value = value;
                LabelProgress.Content = $"{value}%";
            });

            //stopCopy = false;
            //CopyFiles(progress);

            await System.Threading.Tasks.Task.Run(() =>
            {
                if (playlistPath != null)
                {
                    stopCopy = false;
                    CopyFiles(progress);
                }
                else
                {
                    System.Windows.MessageBox.Show("Choose your playlist and destination folder");
                }
            });

            stopCopy = true;
            ProgressBar.Value = ProgressBar.Maximum;
            LabelProgress.Content = $"Complete";
        }

        private void ButtonStopCopy_Click(object sender, RoutedEventArgs e)
        {
            stopCopy = true;
        }

        ////////////////////////////////////////////////////////////////////////

        // creates a list of file paths to copy
        private void CreateListMedia()
        {
            musicFilePathList = new List<string>();
            string filePath;

            // loop through choosen playlist and extracte music paths
            foreach (var item in playListWMP)
            {
                if (item.Contains("media"))
                {
                    // start of music path in current line
                    int startIndex = item.IndexOf('"') + 1;

                    // removing extra characters & replace special symbols
                    filePath = item.Substring(startIndex).Remove(item.Substring(startIndex).IndexOf("\""));
                    filePath = filePath.Replace(@"&apos;", @"'");
                    filePath = filePath.Replace("&amp;", "&");

                    musicFilePathList.Add(filePath);
                }
            }
        }
        private void CopyFiles(IProgress<int> progress)
        {
            // for progressbar
            int percentComplete = 0;
            int oneItemPercentage = 100 / musicFilePathList.Count;

            for (int i = 0; i < musicFilePathList.Count; i++)
            {
                if (stopCopy == true)
                    break;

                string filePath = musicFilePathList[i];

                if (File.Exists(filePath))
                {
                    // extracting file name only
                    fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);

                    string newFilePath = destinationFolder + $"\\{fileName}";

                    File.Copy(filePath, newFilePath, true);

                    // show percent in progressbar
                    percentComplete += oneItemPercentage;
                    progress.Report(percentComplete);
                }
            }
        }

        
    }
}
