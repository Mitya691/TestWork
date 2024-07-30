using System;
using System.Collections.Generic;
using System.IO;
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


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private XmlDocument doc;
        private Page1 page1;
        private const string usersFilePath = "Users.xml";

        public void CreateFile()
        {
            try
            {
                // Создаем директорию, если она не существует
                string directoryPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, usersFilePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                File.WriteAllText(usersFilePath, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<users>\r\n</users>");
                MessageBox.Show("Создан файл с пользователями");
            }
            catch (Exception e)
            {
                MessageBox.Show("При создании файла пользователей произошла ошибка:" + e.Message);
            }
        }

        public MainPage()
        {
            InitializeComponent();
            if (File.Exists(usersFilePath) == false)
            {
                CreateFile();
            }
            LoadXmlFile(usersFilePath);

            page1 = new Page1();
            Loaded += MainPage_LoadedAsync;
        }

        private async void MainPage_LoadedAsync(object sender, RoutedEventArgs e)
        {
            await Task.Delay(100);

            if (NavigationService != null)
            {
            }
            else
            {
                MessageBox.Show("NavigationService не доступен.");
            }
        }

        private void LoadXmlFile(string filePath)
        {
            doc = new XmlDocument();

            try
            {
                doc.Load(filePath);
            }
            catch (XmlException)
            {
                MessageBox.Show("Ошибка при загрузке XML-файла.");
                doc = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
                doc = null;
            }
        }

        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string pass = passBox.Password.Trim();

            if (login.Length < 5)
            {
                textBoxLogin.ToolTip = "Это поле введено некорректно";
                textBoxLogin.Background = Brushes.DarkRed;
            }
            else if (pass.Length < 5)
            {
                passBox.ToolTip = "Это поле введено некорректно";
                passBox.Background = Brushes.DarkRed;
            }
            else if (doc == null)
            {
                MessageBox.Show("XML-файл не загружен.");
            }
            else
            {
                textBoxLogin.ToolTip = "";
                textBoxLogin.Background = Brushes.Transparent;
                passBox.ToolTip = "";
                passBox.Background = Brushes.Transparent;

                // Аутентификационный пользователь
                bool isAuthenticated = false;

                // Получение корневого элемента
                XmlElement rootElement = doc.DocumentElement;

                // Обход дочерних элементов
                foreach (XmlNode node in rootElement.ChildNodes)
                {
                    if (node is XmlElement userElement)
                    {
                        var xmlLoginNode = userElement["login"];
                        var xmlPasswordNode = userElement["password"];

                        if (xmlLoginNode != null && xmlPasswordNode != null)
                        {
                            string xmlLogin = xmlLoginNode.InnerText;
                            string xmlPassword = xmlPasswordNode.InnerText;

                            if (xmlLogin == login && xmlPassword == pass)
                            {
                                isAuthenticated = true;
                                break;
                            }
                        }
                    }
                }

                if (isAuthenticated)
                {

                    if (NavigationService != null)
                    {
                        NavigationService.Navigate(page1);
                    }
                    else
                    {
                        MessageBox.Show("NavigationService не доступен.");
                    }
                }
                else
                {
                    MessageBox.Show("Вы ввели что-то некорректно!");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignIn());
        }
    }
}
