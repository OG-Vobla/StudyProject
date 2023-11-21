using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для StudentsWindow.xaml
    /// </summary>
    /// 
    public partial class StudentsWindow : Window
    {
        public class ImageItemViewModel
        {
            public string FName { get; set; }
            public string LName { get; set; }
            public string Mistake { get; set; }
        }
        public ObservableCollection<ImageItemViewModel> ImageItems { get; set; }

        public StudentsWindow()
        {
            InitializeComponent();
            ImageItems = new ObservableCollection<ImageItemViewModel>();
            LoadImagesFromDatabase();
            CompleteQuizsTable.ItemsSource = ImageItems;

        }
        private void LoadImagesFromDatabase()
        {
            var e = ConectionClass.corectQuiz;
            foreach (var i in ConectionClass.DataBase.CompletedQuizes.Where(x => x.Quiz_id == e.Id_Quiz))
            {
                var h = i.User_id;
                foreach (var a in ConectionClass.DataBase.Quiz.Where(x => ConectionClass.DataBase.CompletedQuizes.Where(y => y.Quiz_id == x.Id_Quiz && y.User_id == h).FirstOrDefault() != null))
                {
                    var AllQues = ConectionClass.DataBase.Question.Where(l => l.Quiz_id == a.Id_Quiz);
                    List<CompletedQuestion> corectQues = new List<CompletedQuestion>();
                    foreach (var item in AllQues)
                    {
                        var k = ConectionClass.DataBase.CompletedQuestion.Where(x => x.User_id == h && x.Question_id == item.Id_Question && x.isCorect == 1).FirstOrDefault();
                        if (k != null)
                        {

                            corectQues.Add(k);
                        }
                    }

                    ImageItems.Add(new ImageItemViewModel { FName = i.User.FName, LName = i.User.LName, Mistake = $"Правильных ответов {corectQues.Count()} из {AllQues.Count()}" });
                }

            }
        }
    }
}