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
        public string DestinationFile;

        //TODO: sort a couple of issues out.
        //1.Unselecting from a child item will not unselect the parent. 
        //2. Selecting all child items will not select parent item.
        //Have a look at this http://www.codeproject.com/Articles/28306/Working-with-Checkboxes-in-the-WPF-TreeView

        public ExportModel()
        {
            GetmNemes();
        }

        public bool ExportmNemes()
        {
            using (ZipArchive archive = ZipFile.Open(DestinationFile, ZipArchiveMode.Update))
            {

                foreach (var item in mNemes)
                {
                    var checkedItems = item.SubDirectories.Where(x => x.IsChecked);

                    foreach (var checkeditem in checkedItems)
                    {
                        foreach (string file in Directory.EnumerateFiles(checkeditem.Directory))
                        {
                            archive.CreateEntryFromFile(file, string.Format("{0}/{1}",checkeditem.Name , Path.GetFileName(file)), CompressionLevel.Fastest);
                        }
                    }

                }
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
                List<string> subDirectories = Directory.GetDirectories(mNeme.Directory).ToList();

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
