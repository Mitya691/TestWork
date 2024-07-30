using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для SignIn.xaml
    /// </summary>
    public partial class SignIn : Page
    {
         private const string UsersFilePath = "Users.xml";


       

        public SignIn()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordBox.Password;
            string confirmPassword = confirmPasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                errorText.Text = "Заполните все поля!";
                return;
            }

            if (password != confirmPassword)
            {
                errorText.Text = "Пароли не совпадают!";
                return;
            }

            if (IsUserExists(login))
            {
                errorText.Text = "Пользователь с таким логином уже существует!";
                return;
            }

            // Регистрация пользователя
            RegisterUser(login, password);
            //errorText.Text = "Регистрация успешна!";
            
            NavigationService.Navigate(new MainPage());
        }

        private bool IsUserExists(string login)
        {
            try
            {
                XDocument doc = XDocument.Load(UsersFilePath);
                return doc.Descendants("login").Any(x => x.Value == login);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось открыть файл с пользователями");
                return false;
            }

        }

        private void RegisterUser(string login, string password)
        {
            try
            {
                XDocument doc = XDocument.Load(UsersFilePath);
                XElement usersElement = doc.Element("users");

                if (usersElement != null)
                {
                    usersElement.Add(new XElement("user",
                                        new XElement("login", login),
                                        new XElement("password", password)));

                    doc.Save(UsersFilePath);
                    MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка регистрации: " + e.Message);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}
   
