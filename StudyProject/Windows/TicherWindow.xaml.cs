using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace StudyProject.Windows
{
    /// <summary>
    /// Логика взаимодействия для TicherWindow.xaml
    /// </summary>
    public partial class TicherWindow : Window
    {
        public class ImageItemViewModel
        {
            public BitmapImage ImageSource { get; set; }
            public string Name { get; set; }
            public string QuizCategory { get; set; }
            public string Mistake { get; set; }
        }
        public ObservableCollection<ImageItemViewModel> ImageItems { get; set; }
        public TicherWindow()
        {
            InitializeComponent();
            ImageItems = new ObservableCollection<ImageItemViewModel>();
            LoadImagesFromDatabase();
            CompleteQuizsTable.ItemsSource = ImageItems;
            foreach (var item in ConectionClass.DataBase.Category.ToList())
            {
                Category.Items.Add(item.Name);
            }
           Login.Text = ConectionClass.user.Login;
           FName.Text = ConectionClass.user.FName;
           LName.Text = ConectionClass.user.LName;
            Password.Password = ConectionClass.user.Password;
            RepPassword.Password = ConectionClass.user.Password;
        }
        private void LoadImagesFromDatabase()
        {
            var e = ConectionClass.user.Id_User;
            foreach (var i in ConectionClass.DataBase.Quiz.Where(x =>x.User_id == e))
            {
                byte[] imageData = ConectionClass.DataBase.QuizImage.Where(x => x.Quiz_id == i.Id_Quiz).FirstOrDefault().QuizImageSrc;
                BitmapImage image = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                }
                var AllQues = ConectionClass.DataBase.User.Where(l => l.IsTeacher == 0);
                List<User> users = new List<User>();
                foreach (var item in AllQues)
                {
                    var k = ConectionClass.DataBase.CompletedQuizes.Where(x => x.User_id == item.Id_User && x.Quiz_id == i.Id_Quiz).FirstOrDefault();
                    if (k != null)
                    {
                        users.Add(item);
                    }
                }
                ImageItems.Add(new ImageItemViewModel { ImageSource = image, Name = i.Name, QuizCategory = i.Category.Name, Mistake = $"Прошли {users.Count()} из {AllQues.Count()} учеников." });
            }

        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                selectedImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameText.Text != "" && Category.Text != "" && selectedImage.Source != null && QuesCount.Text != "")
            {
                int quesCount = 0;
                if (Int32.TryParse(QuesCount.Text,out quesCount))
                {
                    if (ConectionClass.DataBase.Quiz.Where(x=> x.Name == NameText.Text).FirstOrDefault() != null)
                    {
                        MessageBox.Show("Викторина с таким названием уже существует.");
                    }
                    else
                    {

                        ConfirmWindow confirmWindow = new ConfirmWindow("После согласия начнется ввод вопросов.");
                        confirmWindow.ShowDialog();
                        if (confirmWindow.DialogResult)
                        {
                            byte[] imageData;
                            using (MemoryStream stream = new MemoryStream())
                            {
                                BitmapEncoder encoder = new PngBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)selectedImage.Source));
                                encoder.Save(stream);
                                imageData = stream.ToArray();
                            }
                            ConectionClass.DataBase.Quiz.Add(new Quiz() { Name = NameText.Text, Category_id = ConectionClass.DataBase.Category.Where(x => x.Name == Category.Text).FirstOrDefault().Id_Category, User_id = ConectionClass.user.Id_User });
                            ConectionClass.DataBase.SaveChanges();
                            ConectionClass.quizNum = ConectionClass.DataBase.Quiz.Where(x=> x.Name == NameText.Text).FirstOrDefault().Id_Quiz;
                            ConectionClass.DataBase.QuizImage.Add(new QuizImage() { Quiz_id = ConectionClass.quizNum, QuizImageSrc = imageData });
                            ConectionClass.DataBase.SaveChanges();
                            ConectionClass.quesCount = quesCount;
                            QuestionCreateWindow questionWindow = new QuestionCreateWindow();
                            questionWindow.ShowDialog();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Не правильно заданно колличество вопросов.");
                }
            }
            else
            {
                MessageBox.Show("Не все поля заполнены");
            }
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ImageItemViewModel item = (ImageItemViewModel)button.DataContext;
            var corectQuiz = ConectionClass.DataBase.Quiz.Where(x => x.Name == item.Name).FirstOrDefault();
            ConfirmWindow confirmWindow = new ConfirmWindow("Вы уверены что хотите удалить викторину?");
            confirmWindow.ShowDialog();
            if (confirmWindow.DialogResult)
            {
                foreach (var item2 in ConectionClass.DataBase.Question.Where(x=> x.Quiz_id == corectQuiz.Id_Quiz))
                {
                    var quizzz = ConectionClass.DataBase.CompletedQuestion.Where(x => x.Question_id == item2.Id_Question).FirstOrDefault();
                    if (quizzz != null)
                    {
                        ConectionClass.DataBase.CompletedQuestion.Remove(quizzz);
                    }
                    var quizzzz = ConectionClass.DataBase.QuestionImage.Where(x => x.Question_id == item2.Id_Question).FirstOrDefault();
                    if (quizzzz != null)
                    {
                        ConectionClass.DataBase.QuestionImage.Remove(quizzzz);
                    }
                    ConectionClass.DataBase.Question.Remove(item2);
                }

                var quizz = ConectionClass.DataBase.CompletedQuizes.Where(x => x.Quiz_id == corectQuiz.Id_Quiz).FirstOrDefault();
                if (quizz!= null)
                {
                    ConectionClass.DataBase.CompletedQuizes.Remove(quizz);
                }
                var quizzIm = ConectionClass.DataBase.QuizImage.Where(x => x.Quiz_id == corectQuiz.Id_Quiz).FirstOrDefault();
                if (quizzIm != null)
                {
                    ConectionClass.DataBase.QuizImage.Remove(quizzIm);
                }
                ConectionClass.DataBase.Quiz.Remove(corectQuiz);
                ConectionClass.DataBase.SaveChanges();
                ImageItems = new ObservableCollection<ImageItemViewModel>();
                LoadImagesFromDatabase();
                CompleteQuizsTable.ItemsSource = ImageItems;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ImageItemViewModel item = (ImageItemViewModel)button.DataContext;
            ConectionClass.corectQuiz = ConectionClass.DataBase.Quiz.Where(x => x.Name == item.Name).FirstOrDefault();
            QrCodeWindow qrCodeWindow = new QrCodeWindow(ConectionClass.corectQuiz.Id_Quiz.ToString());
            qrCodeWindow.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ImageItemViewModel item = (ImageItemViewModel)button.DataContext;
            ConectionClass.corectQuiz = ConectionClass.DataBase.Quiz.Where(x => x.Name == item.Name).FirstOrDefault();
            StudentsWindow studentsWindow = new StudentsWindow();
            studentsWindow.ShowDialog();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var us = ConectionClass.user;
            if (LName.Text != "" && FName.Text != "" && Login.Text != "" && Password.Password != "")
            {
                if (Password.Password == RepPassword.Password)
                {
                    if (ConectionClass.DataBase.User.Where(x => x.Login == Login.Text && x.Login != us.Login).FirstOrDefault() == null)
                    {
                        if (ConectionClass.DataBase.User.Where(x => x.LName == LName.Text && x.FName == FName.Text && us.LName != x.LName && us.FName != x.FName).FirstOrDefault() == null)
                        {
                            us.LName = LName.Text;
                            us.FName = FName.Text;
                            us.Login = Login.Text;
                            us.Password = Password.Password;
                            ConectionClass.DataBase.SaveChanges();
                            ConectionClass.user = ConectionClass.DataBase.User.Where(x => x.Login == Login.Text).FirstOrDefault();
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
