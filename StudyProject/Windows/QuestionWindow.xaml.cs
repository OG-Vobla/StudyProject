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
using System.Windows.Shapes;

namespace StudyProject.Windows
{
    /// <summary>
    /// Логика взаимодействия для QuestionWindow.xaml
    /// </summary>
    public partial class QuestionWindow : Window
    {
        public QuestionWindow()
        {
            InitializeComponent();
            Discription.Text = ConectionClass.questions[0].Description;
            FirstAnswerText.Text = ConectionClass.questions[0].FirstAnswer;
            SecondAnswerText.Text = ConectionClass.questions[0].SecondAnwer;
            ThirdAnswerText.Text = ConectionClass.questions[0].ThirdAnswer;
            FourthAnswerText.Text = ConectionClass.questions[0].FourthAnswer;
            int targetQuestionId = ConectionClass.questions[0].Id_Question;
            var a = ConectionClass.DataBase.QuestionImage
    .Where(x => x.Question_id == targetQuestionId)
    .FirstOrDefault();
            if (a != null)
            {
                byte[] imageData = ConectionClass.DataBase.QuestionImage.Where(x => x.Question_id == targetQuestionId).FirstOrDefault().QuestionImageSrc;
                BitmapImage image = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                }
                selectedImage.Source = image;
            }
        }

        private void FirstAnswerText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ConectionClass.DataBase.CompletedQuestion.Add(new CompletedQuestion() {Question_id = ConectionClass.questions[0].Id_Question, isCorect = (ConectionClass.questions[0].CorrectAnswerNum == 1 ? 1 : 0), User_id = ConectionClass.user.Id_User  });
            ConectionClass.DataBase.SaveChanges();
            if (ConectionClass.questions[0].CorrectAnswerNum == 1)
            {
                Color myColor = (Color)ColorConverter.ConvertFromString("#FF24CC00");
                SolidColorBrush myBrush = new SolidColorBrush(myColor);
                FirstAnswerBtn.Background= myBrush;
            }
            else
            {
                ConectionClass.MistakeCount += 1;
                Color myColor = (Color)ColorConverter.ConvertFromString("#FFE91313");
                SolidColorBrush myBrush = new SolidColorBrush(myColor);
                FirstAnswerBtn.Background = myBrush;
            }
            if (ConectionClass.quesCount != 1)
            {
                ConectionClass.quesCount -= 1;
                ConectionClass.questions.RemoveAt(0);
                QuestionWindow questionCreateWindow = new QuestionWindow();
                questionCreateWindow.ShowDialog();
                this.Close();
            }
            else
            {
                ConectionClass.DataBase.CompletedQuizes.Add(new CompletedQuizes() {User_id = ConectionClass.user.Id_User,Quiz_id = ConectionClass.corectQuiz.Id_Quiz   });
                ConectionClass.DataBase.SaveChanges();
                MessageBox.Show($"Вы совершили {ConectionClass.MistakeCount} ошибок.");
                this.Close();
            }
        }

        private void SecondAnswerText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ConectionClass.DataBase.CompletedQuestion.Add(new CompletedQuestion() { Question_id = ConectionClass.questions[0].Id_Question, isCorect = (ConectionClass.questions[0].CorrectAnswerNum == 2 ? 1 : 0), User_id = ConectionClass.user.Id_User });
            ConectionClass.DataBase.SaveChanges();
            if (ConectionClass.questions[0].CorrectAnswerNum == 2)
            {
                Color myColor = (Color)ColorConverter.ConvertFromString("#FF24CC00");
                SolidColorBrush myBrush = new SolidColorBrush(myColor);
                SecondAnswerBtn.Background = myBrush;
            }
            else
            {
                ConectionClass.MistakeCount += 1;
                Color myColor = (Color)ColorConverter.ConvertFromString("#FFE91313");
                SolidColorBrush myBrush = new SolidColorBrush(myColor);
                SecondAnswerBtn.Background = myBrush;
            }
            if (ConectionClass.quesCount != 1)
            {
                ConectionClass.quesCount -= 1;
                ConectionClass.questions.RemoveAt(0);

                QuestionWindow questionCreateWindow = new QuestionWindow();
                questionCreateWindow.ShowDialog();
                this.Close();
            }
            else
            {
                ConectionClass.DataBase.CompletedQuizes.Add(new CompletedQuizes() { User_id = ConectionClass.user.Id_User, Quiz_id = ConectionClass.corectQuiz.Id_Quiz });
                ConectionClass.DataBase.SaveChanges();
                MessageBox.Show($"Вы совершили {ConectionClass.MistakeCount} ошибок.");
                this.Close();
            }
        }

        private void ThirdAnswerText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ConectionClass.DataBase.CompletedQuestion.Add(new CompletedQuestion() { Question_id = ConectionClass.questions[0].Id_Question, isCorect = (ConectionClass.questions[0].CorrectAnswerNum == 3 ? 1 : 0), User_id = ConectionClass.user.Id_User });
            ConectionClass.DataBase.SaveChanges();
            if (ConectionClass.questions[0].CorrectAnswerNum == 3)
            {
                Color myColor = (Color)ColorConverter.ConvertFromString("#FF24CC00");
                SolidColorBrush myBrush = new SolidColorBrush(myColor);
                ThirdAnswerBtn.Background = myBrush;
            }
            else
            {
                ConectionClass.MistakeCount += 1;
                Color myColor = (Color)ColorConverter.ConvertFromString("#FFE91313");
                SolidColorBrush myBrush = new SolidColorBrush(myColor);
                ThirdAnswerBtn.Background = myBrush;
            }
            if (ConectionClass.quesCount != 1)
            {
                ConectionClass.quesCount -= 1;
                ConectionClass.questions.RemoveAt(0);

                QuestionWindow questionCreateWindow = new QuestionWindow();
                questionCreateWindow.ShowDialog();
                this.Close();
            }
            else
            {
                ConectionClass.DataBase.CompletedQuizes.Add(new CompletedQuizes() { User_id = ConectionClass.user.Id_User, Quiz_id = ConectionClass.corectQuiz.Id_Quiz });
                ConectionClass.DataBase.SaveChanges();
                MessageBox.Show($"Вы совершили {ConectionClass.MistakeCount} ошибок.");
                this.Close();
            }
        }

        private void FourthAnswerText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ConectionClass.DataBase.CompletedQuestion.Add(new CompletedQuestion() { Question_id = ConectionClass.questions[0].Id_Question, isCorect = (ConectionClass.questions[0].CorrectAnswerNum == 4 ? 1 : 0), User_id = ConectionClass.user.Id_User });
            ConectionClass.DataBase.SaveChanges();
            if (ConectionClass.questions[0].CorrectAnswerNum == 4)
            {
                Color myColor = (Color)ColorConverter.ConvertFromString("#FF24CC00");
                SolidColorBrush myBrush = new SolidColorBrush(myColor);
                FourthAnswerBtn.Background = myBrush;
            }
            else
            {
                ConectionClass.MistakeCount += 1;
                Color myColor = (Color)ColorConverter.ConvertFromString("#FFE91313");
                SolidColorBrush myBrush = new SolidColorBrush(myColor);
                FourthAnswerBtn.Background = myBrush;
            }
            if (ConectionClass.quesCount != 1)
            {
                ConectionClass.quesCount -= 1;
                ConectionClass.questions.RemoveAt(0);

                QuestionWindow questionCreateWindow = new QuestionWindow();
                questionCreateWindow.ShowDialog();
                this.Close();
            }
            else
            {
                ConectionClass.DataBase.CompletedQuizes.Add(new CompletedQuizes() { User_id = ConectionClass.user.Id_User, Quiz_id = ConectionClass.corectQuiz.Id_Quiz });
                ConectionClass.DataBase.SaveChanges();
                MessageBox.Show($"Вы совершили {ConectionClass.MistakeCount} ошибок.");
                this.Close();
            }
        }
    }
}
