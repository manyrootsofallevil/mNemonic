using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{

    public class AddmNemeModel
    {
        private const string questionFileName = @"\Question.txt";
        private const string answerFileName = @"\Answer.txt";

        public string WindowTitle { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Title { get; set; }
        public string RootDirectory { get; set; }
        public string Image { get; set; }

        public AddmNemeModel(string title)
        {
            WindowTitle = title;
            RootDirectory = ConfigurationManager.AppSettings["RootDirectory"];
        }

        public bool WriteToFile()
        {
            bool result = false;

            result = WriteToFile(Question, true);
            result &= WriteToFile(Answer);

            if (!string.IsNullOrEmpty(Image))
            {
                string filepath = Path.Combine(RootDirectory, Title);
                //Since we are selecting from somewhere it seems reasonable to assume that the file exists
                File.Copy(Image, Path.Combine(filepath, Path.GetFileName(Image)));
            }

            return result;
        }

        private bool WriteToFile(string input, bool isQuestion=false)
        {
            bool result = false;

            try
            {
                string filepath = Path.Combine(RootDirectory, Title);

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
