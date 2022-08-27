using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace PlaylistSearcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> musicFilePath;

        private bool stopCopy = true;
        private int numberFilesToCopy;
        private string playlistPath;
        private string filePathSource;
        private string fileName;
        private string destinationFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\\Playlist Music";
        
        public MainWindow()
        {
            InitializeComponent();
            //DataContext = this;
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
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "WPL|*.wpl";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                playlistPath = fileDialog.FileName;
                LabelPath.Content = fileDialog.FileName;
                musicFilePath = new List<string>(File.ReadAllLines(playlistPath));
            }

            ProgressBar.Maximum = GetNumberFilesToCopy();
            ProgressBar.Value = 0;
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
            Directory.CreateDirectory(destinationFolder);

            await Task.Run(() =>
            {
                if (playlistPath != null)
                {
                    stopCopy = false;
                    CopyFiles();
                }
                else
                {
                    System.Windows.MessageBox.Show("Choose your playlist and destination folder");
                }
            });

            stopCopy = true;
            System.Windows.MessageBox.Show("Done");
        }

        private void ButtonStopCopy_Click(object sender, RoutedEventArgs e)
        {
            stopCopy = true;
        }

        ////////////////////////////////////////////////////////////////////////
        public int GetNumberFilesToCopy()
        {
            numberFilesToCopy = 0;
            foreach (var item in musicFilePath)
            {
                if (item.Contains("media"))
                {
                    numberFilesToCopy++;
                }
            }
            return numberFilesToCopy;
        }

        private void CopyFiles()
        {
            foreach (var file in musicFilePath)
            {
                if (stopCopy == true)
                    break;

                if (file.Contains("media"))
                {
                    int startIndex = file.IndexOf('"') + 1;

                    filePathSource = file.Substring(startIndex).Remove(file.Substring(startIndex).IndexOf("\""));
                    filePathSource = filePathSource.Replace(@"&apos;", @"'");
                    filePathSource = filePathSource.Replace("&amp;", "&");
                    filePathSource = filePathSource.Replace("..", "C:");

                    if (File.Exists(filePathSource))
                    {
                        fileName = filePathSource;
                        fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                        string newFilePath = destinationFolder + $"\\{fileName}";

                        File.Copy(filePathSource, newFilePath, true);
                    }
                }
            }
        }


    }
}
