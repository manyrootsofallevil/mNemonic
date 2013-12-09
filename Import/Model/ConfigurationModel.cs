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

        private void LoadConfigurationFile()
        {
            try
            {
                XDocument xdoc = XDocument.Load(this.collectionsFile);

                mNemesCollection.AddRange(xdoc.Root.Elements()
                    //.Where(x => x.Attribute("Enabled").Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    .Select(x => new mNemeStorage(Path.Combine(rootDirectory, x.Attribute("relativePath").Value), 
                        Boolean.Parse(x.Attribute("Enabled").Value))));
            }
            catch (Exception ex)
            {
            }
        }
    }
}
