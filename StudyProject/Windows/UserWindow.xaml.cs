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
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    /// 
    public class ImageItemViewModel
    {
        public BitmapImage ImageSource { get; set; }
        public string Name { get; set; }
        public string QuizCategory { get; set; }
        public string Mistake { get; set; }
    }

    public partial class UserWindow : Window
    {
        public ObservableCollection<ImageItemViewModel> ImageItems { get; set; }
        public ObservableCollection<ImageItemViewModel> ImageItems1 { get; set; }

        public UserWindow()
        {
            InitializeComponent();
            ImageItems = new ObservableCollection<ImageItemViewModel>();
            ImageItems1 = new ObservableCollection<ImageItemViewModel>();
            LoadImagesFromDatabase();
            QuizsTable.ItemsSource = ImageItems;
            CompleteQuizsTable.ItemsSource = ImageItems1;

        }


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void LoadImagesFromDatabase()
        {
                foreach (var i in ConectionClass.DataBase.Quiz)
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

                    ImageItems.Add(new ImageItemViewModel { ImageSource = image, Name = i.Name, QuizCategory = i.Category.Name });
                }
            var e = ConectionClass.user.Id_User;
            foreach (var i in ConectionClass.DataBase.Quiz.Where(x=> ConectionClass.DataBase.CompletedQuizes.Where(y => y.Quiz_id == x.Id_Quiz && y.User_id == e).FirstOrDefault() != null ))
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
                var AllQues = ConectionClass.DataBase.Question.Where(l => l.Quiz_id == i.Id_Quiz);
                List<CompletedQuestion> corectQues = new List<CompletedQuestion>();
                foreach (var item in AllQues)
                {
                    var k = ConectionClass.DataBase.CompletedQuestion.Where(x => x.User_id == e && x.Question_id == item.Id_Question && x.isCorect == 1).FirstOrDefault();
                    if (k != null)
                    {

                        corectQues.Add(k);
                    }
                }
                ImageItems1.Add(new ImageItemViewModel { ImageSource = image, Name = i.Name, QuizCategory = i.Category.Name, Mistake = $"Правильных ответов {corectQues.Count()} из {AllQues.Count()}." });
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ImageItemViewModel item = (ImageItemViewModel)button.DataContext;
            ConectionClass.corectQuiz = ConectionClass.DataBase.Quiz.Where(x=> x.Name == item.Name).FirstOrDefault();
            var i = ConectionClass.user.Id_User;
            var b = ConectionClass.corectQuiz.Id_Quiz;
            if (ConectionClass.DataBase.CompletedQuizes.Where(x=> x.User_id == i && x.Quiz_id ==  b).FirstOrDefault() != null)
            {
                MessageBox.Show("Вы уже проходили эту викторину");
            }
            else
            {
                ConectionClass.questions = ConectionClass.DataBase.Question.Where(x=> x.Quiz_id == ConectionClass.corectQuiz.Id_Quiz).ToList();
                ConectionClass.quesCount = ConectionClass.questions.Count;
                ConectionClass.quizNum = ConectionClass.corectQuiz.Id_Quiz;
                ConectionClass.MistakeCount = 0;
                QuestionWindow questionWindow   = new QuestionWindow();
                questionWindow.ShowDialog();
            }
        }
    }
}
