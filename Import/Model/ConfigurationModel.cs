using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Import.Model
{
    public class ConfigurationModel
    {
        string rootDirectory = ConfigurationManager.AppSettings["RootDirectory"];
        string collectionsFile = ConfigurationManager.AppSettings["CollectionsFile"];
        public List<mNemeStorage> mNemesCollection = new List<mNemeStorage>();

        public ConfigurationModel()
        {
            LoadConfigurationFile();
        }

        public bool Save()
        {
            bool result = false;

            try
            {
                XDocument xdoc = XDocument.Load(this.collectionsFile);

                foreach (mNemeStorage record in mNemesCollection)
                {
                    var alreadyStored = xdoc.Root.Elements()
                        .Where(x => x.Attribute("Name").Value.Equals(record.Name, StringComparison.InvariantCultureIgnoreCase));

                    if (alreadyStored.Count() == 1)
                    {
                        alreadyStored.FirstOrDefault().Attribute("Enabled").Value = record.IsChecked.ToString();
                    }
                    else
                    {
                        xdoc.Root.Add(new XElement("collection", new XAttribute("Name", record.Name),
                        new XAttribute("relativePath", new DirectoryInfo(record.Directory).Name), new XAttribute("Enabled", true)));
                    }
                }

                xdoc.Save(this.collectionsFile);
                result = true;
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public Tuple<bool, string, mNemeStorage> AddToCollections(string path)
        {
            XDocument xdoc;
            string name = new DirectoryInfo(path).Name;
            try
            {

                if (File.Exists(this.collectionsFile))
                {
                    xdoc = XDocument.Load(this.collectionsFile);

                    var alreadystored = xdoc.Root.Elements()
                        .Where(x => x.Attribute("Name").Value.Equals(name, StringComparison.InvariantCultureIgnoreCase));

                    if (alreadystored.Count() == 0)
                    {
                        xdoc.Root.Add(new XElement("collection", new XAttribute("Name", name),
                           new XAttribute("relativePath", name), new XAttribute("Enabled", true)));
                    }
                    else
                    {
                        return new Tuple<bool, string, mNemeStorage>(false, "Collection has already been added", null);
                    }
                }
                else
                {
                    xdoc = new XDocument();
                    xdoc.Add(new XElement("Collections"));
                    xdoc.Root.Add(new XElement("collection", new XAttribute("Name", name),
                       new XAttribute("relativePath", name), new XAttribute("Enabled", true)));
                }

                xdoc.Save(this.collectionsFile);

                return new Tuple<bool, string, mNemeStorage>(true, string.Empty, new mNemeStorage(name, true));

            }
            catch (Exception ex)
            {

            }
            //The genius of this EM is that there is no log file :)
            return new Tuple<bool, string, mNemeStorage>(false, "An error ocurred, see log file", null);

        }

        public Tuple<bool, string> RemoveCollection(string name)
        {
            XDocument xdoc;

            try
            {
                if (File.Exists(this.collectionsFile))
                {
                    xdoc = XDocument.Load(this.collectionsFile);
                    var stored = xdoc.Root.Elements()
                        .Where(x => x.Attribute("Name").Value.Equals(name, StringComparison.InvariantCultureIgnoreCase));

                    if (stored.Count() == 1)
                    {
                        stored.Remove();
                    }

                    xdoc.Save(this.collectionsFile);

                    return new Tuple<bool, string>(true, string.Empty);
                }
            }
            catch (Exception ex)
            {
            }

            return new Tuple<bool, string>(false, "An error ocurred, see log file");
        }

        private void LoadConfigurationFile()
        {
            try
            {
                XDocument xdoc = XDocument.Load(this.collectionsFile);

                mNemesCollection.AddRange(xdoc.Root.Elements()
                    .Select(x => new mNemeStorage(Path.Combine(rootDirectory, x.Attribute("relativePath").Value),
                        Boolean.Parse(x.Attribute("Enabled").Value))));
            }
            catch (Exception ex)
            {
            }
        }
    }
}
