using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{

    public class ImportModel
    {
        private const string questionFileName = @"\Question.txt";
        private const string answerFileName = @"\Answer.txt";

        public string WindowTitle { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Title { get; set; }
        public string Directory { get; set; }
        public string Image { get; set; }

        public ImportModel(string title)
        {
            WindowTitle = title;
        }

        public bool WriteToFile()
        {
            bool result = false;

            result = WriteToFile(Question, true);
            result &= WriteToFile(Answer);

            return result;
        }

        private bool WriteToFile(string input, bool isQuestion=false)
        {
            bool result = false;

            try
            {
                string filepath = Path.Combine(Directory, Title);

                if (isQuestion)
                {
                    filepath += questionFileName;
                }
                else
                {
                    filepath += answerFileName;
                }

                CreateDirectory(filepath);

                using (StreamWriter sw = new StreamWriter(filepath))
                {
                    sw.Write(input);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //TODO: log error here.
            }

            return result;
        }

        private static void CreateDirectory(string filepath)
        {
            string dir = Path.GetDirectoryName(filepath);

            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
        }

    }
}
