using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using ReplaceTags;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void ChoosePattern(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                string filePath = ofd.FileName;
                patternPath.Text = filePath;
            }
        }

        private void ChooseFolder(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFolderDialog();
            if(ofd.ShowDialog() == true)
            {
                string folder = ofd.FolderName;
                folderPath.Text = folder;
            }
        }

        private async void MainProcess(object sender, RoutedEventArgs e)
        {

            Document doc = new Document();
            doc.FileNamePattern = patternPath.Text.ToString();
            doc.FolderName = folderPath.Text.ToString();
            doc.FileName = fileName.Text.ToString();
            await doc.ExecuteAsync();  
        }
    }
}
