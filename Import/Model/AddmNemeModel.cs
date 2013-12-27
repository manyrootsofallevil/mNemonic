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

        public AddmNemeModel()
        {
            RootDirectory = ConfigurationManager.AppSettings["RootDirectory"];
        }

        public AddmNemeModel(string rootDirectory)
        {
            if (string.IsNullOrEmpty(rootDirectory))
            {
                RootDirectory = ConfigurationManager.AppSettings["RootDirectory"];
            }
            else
            {
                RootDirectory = rootDirectory;
            }
        }

        public Tuple<bool,string> WriteToFile()
        {
            bool result = false;
            string message = string.Empty;
            string filepath = Path.Combine(RootDirectory, Title);

            try
            {
                if (!Directory.Exists(filepath))
                {
                    result = WriteToFile(Question, true);
                    result &= WriteToFile(Answer);

                    if (!string.IsNullOrEmpty(Image))
                    {
                        //Since we are selecting from somewhere it seems reasonable to assume that the file exists
                        File.Copy(Image, Path.Combine(filepath, Path.GetFileName(Image)));
                    }
                }
                else
                {
                    message = string.Format("Directory {0} already exists. Try a different title for your mNeme or a different collection", filepath);
                }
            }
            catch (Exception ex)
            {
                message = string.Format("An error occurred saving {0}. Exception: {1}.", filepath,ex);
            }

            return new Tuple<bool, string>(result, message);
        }

        private bool WriteToFile(string input, bool isQuestion = false)
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
                //If this had some logging it wouldn't be as pointless as it looks, just a little bit pointless.
                throw;
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
