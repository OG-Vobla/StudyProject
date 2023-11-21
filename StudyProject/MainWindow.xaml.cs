using StudyProject.Windows;
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

namespace StudyProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegWindow regWindow = new RegWindow();
            regWindow.ShowDialog();
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Login.Text != "" && Password.Password != "")
            {
                var user = ConectionClass.DataBase.User.Where(x => x.Login == Login.Text).FirstOrDefault();
                if (user != null)
                {
                    if (user.Password == Password.Password)
                    {
                        ConectionClass.user = user;
                        if (user.IsTeacher == 0)
                        {
                            UserWindow userWindow = new UserWindow();  
                            userWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            TicherWindow userWindow = new TicherWindow();
                            userWindow.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не верный пароль.");

                    }
                }
                else
                {
                    MessageBox.Show("Пользователя с таким логином нет.");
                }
            }
            else
            {
                MessageBox.Show("Не все поля заполнены.");
            }

        }
    }
}
