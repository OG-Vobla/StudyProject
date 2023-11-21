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
using System.Windows.Shapes;

namespace StudyProject.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        public RegWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (LName.Text != "" && FName.Text != "" && Login.Text != "" && Password.Password != "")
            {
                if (Password.Password == RepPassword.Password)
                {
                    if (ConectionClass.DataBase.User.Where(x => x.Login == Login.Text).FirstOrDefault() == null)
                    {
                        if (ConectionClass.DataBase.User.Where(x => x.LName == LName.Text && x.FName == FName.Text).FirstOrDefault() == null)
                        {
                            ConectionClass.DataBase.User.Add(new User() {Login = Login.Text, FName = FName.Text, LName = LName.Text,  Password = Password.Password, IsTeacher = 0});
                            ConectionClass.DataBase.SaveChanges();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Пользователь с таким именем и фамилией уже есть.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пользователь с таким логином уже есть.");
                    }
                }
                else
                {
                    MessageBox.Show("Пароли не совпадают.");
                }
            }
            else
            {
                MessageBox.Show("Не все поля заполнены.");
            }

        }
    }
}
