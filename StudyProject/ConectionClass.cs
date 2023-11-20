using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyProject
{
    public class ConectionClass
    {
        public static StudyProjectDbEntities1 DataBase = new StudyProjectDbEntities1();
        public static User user = new User();
        public static Quiz corectQuiz = new Quiz();
        public static List<Question> questions = new List<Question>();
        public static int quesCount = 0;
        public static int quizNum = 0;
        public static int MistakeCount = 0;
    }
}
