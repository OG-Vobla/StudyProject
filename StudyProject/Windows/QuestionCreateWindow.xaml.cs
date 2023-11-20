using Microsoft.Win32;
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
    /// Логика взаимодействия для QuestionCreateWindow.xaml
    /// </summary>
    public partial class QuestionCreateWindow : Window
    {
        public QuestionCreateWindow()
        {
            InitializeComponent();
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

  /*      private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameText.Text != "" && Category.Text != "" && selectedImage.Source != null && QuesCount.Text != "")
            {
                int quesCount = 0;
                if (Int32.TryParse(QuesCount.Text, out quesCount))
                {
                    ConfirmWindow confirmWindow = new ConfirmWindow("После согласия начнется ввод вопросов.");
                    confirmWindow.ShowDialog();
                    if (confirmWindow.DialogResult)
                    {

                        ConectionClass.DataBase.Quiz.Add(new Quiz() { Name = NameText.Text, Category_id = ConectionClass.DataBase.Category.Where(x => x.Name == Category.Text).FirstOrDefault().Id_Category, User_id = ConectionClass.user.Id_User });
                        ConectionClass.DataBase.QuizImage.Add(new QuizImage() { Quiz_id = 1, QuizImageSrc = imageData });
                        ConectionClass.DataBase.SaveChanges();
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
   */     private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Discription.Text != "" && QuesNum.Text != "" && FirstAnswer.Text != "" && SecondAnswer.Text != "" && ThirdAnswer.Text != "" && FourthAnswer.Text != "")
            {
                ConectionClass.DataBase.Question.Add(new Question() {Description = Discription.Text, FirstAnswer = FirstAnswer.Text, SecondAnwer = SecondAnswer.Text, ThirdAnswer = ThirdAnswer.Text, FourthAnswer = FourthAnswer.Text, Quiz_id = ConectionClass.quizNum, CorrectAnswerNum = Int32.Parse(QuesNum.Text )});
                ConectionClass.DataBase.SaveChanges();
                if (selectedImage.Source != null)
                {
                    byte[] imageData;
                    using (MemoryStream stream = new MemoryStream())
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)selectedImage.Source));
                        encoder.Save(stream);
                        imageData = stream.ToArray();
                    }
                    var ais = ConectionClass.DataBase.Question.Where(x => x.Description == Discription.Text && x.FirstAnswer == FirstAnswer.Text && x.SecondAnwer == SecondAnswer.Text && x.ThirdAnswer == ThirdAnswer.Text && x.FourthAnswer == FourthAnswer.Text && x.Quiz_id == ConectionClass.quizNum).FirstOrDefault();
                    while (ais==null)
                    {
                        ais = ConectionClass.DataBase.Question.Where(x => x.Description == Discription.Text && x.FirstAnswer == FirstAnswer.Text && x.SecondAnwer == SecondAnswer.Text && x.ThirdAnswer == ThirdAnswer.Text && x.FourthAnswer == FourthAnswer.Text && x.Quiz_id == ConectionClass.quizNum).FirstOrDefault();

                    }
                    ConectionClass.DataBase.QuestionImage.Add(new QuestionImage() { Question_id =ais.Id_Question, QuestionImageSrc = imageData });
                ConectionClass.DataBase.SaveChanges();
                   
                }
                if (ConectionClass.quesCount != 1)
                {
                    ConectionClass.quesCount -= 1;
                    QuestionCreateWindow questionCreateWindow = new QuestionCreateWindow();
                    questionCreateWindow.ShowDialog();
                    this.Close();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Не все поля заполнены.");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                selectedImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
    }
}
