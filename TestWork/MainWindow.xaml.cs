using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
//using System.Collections.Generic;



namespace TestWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Content = new MainPage();

        }

        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            //MainPage mainPage = new MainPage();
            //this.Content = mainPage;

            //MainPage mainPage = new MainPage();
            //mainFrame.NavigationService.Navigate(mainPage);

            //mainFrame.Navigate(new Uri("MainPage.xaml", UriKind.Relative));

            //mainFrame.Navigate(new MainPage());

            //NavigationService.Navigate(new MainPage());


        }
    }
}
