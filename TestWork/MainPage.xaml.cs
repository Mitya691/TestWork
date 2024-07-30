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


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private XmlDocument doc;
        private Page1 page1;

        public MainPage()
        {
            InitializeComponent();
            LoadXmlFile("C:\\Users\\Денис\\Desktop\\Users.xml");

            page1 = new Page1();
            Loaded += MainPage_LoadedAsync;
        }

        private async void MainPage_LoadedAsync(object sender, RoutedEventArgs e)
        {
            // Даем немного времени для инициализации NavigationService
            await Task.Delay(100);

            // Проверяем, что NavigationService доступен
            if (NavigationService != null)
            {
                // Используем NavigationService для перехода на другую страницу
                // Например: NavigationService.Navigate(page1);
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
                    //MessageBox.Show("Все хорошо!");

                    // Создаем экземпляр Page1
                    //Page1 page1 = new Page1();

                    // Навигируем на Page1
                    //mainFrame.Navigate(page1);

                    //NavigationService.Navigate(pageLend);
                    // Проверяем, что NavigationService доступен
                    if (NavigationService != null)
                    {
                        // Используем NavigationService для перехода на другую страницу
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
