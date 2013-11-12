using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace Import.Model
{
    public class ExportModel
    {
        string rootDirectory = ConfigurationManager.AppSettings["RootDirectory"];
        public List<mNemeStorage> mNemes = new List<mNemeStorage>();

        public ExportModel()
        {
            GetmNemes();
        }

        public bool ExportmNemes()
        {
            string path = "test.zip";

            using (ZipArchive archive = ZipFile.Open(path, ZipArchiveMode.Update))
            {
                archive.CreateEntryFromFile(@"C:\mNemonic\Unusual Words\acersecomic\Question.txt", "question.txt");
                archive.CreateEntryFromFile(@"C:\mNemonic\Unusual Words\acersecomic\acersecomic.jpg", "acersecomic.jpg");


            } 

            return true;
        }

        private void GetmNemes()
        {
            //Since we tightly control how things get in and out we don't really have to worry about recursion.
            List<string> topLevel = Directory.GetDirectories(rootDirectory).ToList();

            AddmNemeStorageToList(topLevel, mNemes);

            AddSubDirTomNemeStorageList();

        }

        private void AddSubDirTomNemeStorageList()
        {
            foreach (mNemeStorage mNeme in mNemes)
            {
                List<string> subDirectories = Directory.GetDirectories(mNeme.Name).ToList();

                //There is probably a better way of doing this.
                List<mNemeStorage> crutch = new List<mNemeStorage>();

                AddmNemeStorageToList(subDirectories, crutch);

                mNeme.SubDirectories = new System.Collections.ObjectModel.ObservableCollection<mNemeStorage>(crutch);
            }
        }

        private void AddmNemeStorageToList(List<string> sourceList, List<mNemeStorage> mNemes)
        {
            foreach (string dir in sourceList)
            {
                mNemes.Add(new mNemeStorage(dir));
            }
        }

    }
}
