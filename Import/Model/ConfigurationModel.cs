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
    {//TODO: I think i'm going to change this so that when we import something it's added to the collections file
        //then we just read the collections file and toggle them on or off.
        string rootDirectory = ConfigurationManager.AppSettings["RootDirectory"];
        string collectionsFile = ConfigurationManager.AppSettings["CollectionsFile"];
        public List<mNemeStorage> mNemesCollection = new List<mNemeStorage>();
        
        public ConfigurationModel()
        {
            foreach (string dir in Directory.GetDirectories(rootDirectory))
            {
                mNemesCollection.Add(new mNemeStorage(dir));
            }
        }

        public bool Save()
        {
            bool result = false;

            XDocument xdoc = XDocument.Load(this.collectionsFile);

            foreach (mNemeStorage record in mNemesCollection)
            {
                var alreadyStored = xdoc.Root.Elements()
                    .Where(x => x.Attribute("Name").Value.Equals(record.Name, StringComparison.InvariantCultureIgnoreCase));

                if (alreadyStored.Count() == 1)
                {
                    alreadyStored.FirstOrDefault().Attribute("Enabled").Value = "true";
                }
                else
                {
                    xdoc.Root.Add(new XElement("collection", new XAttribute("Name", record.Name),
                    new XAttribute("relativePath", new DirectoryInfo(record.Directory).Name), new XAttribute("Enabled", true)));
                }
            }

            xdoc.Save(this.collectionsFile);

            return result;
        }
    }
}
