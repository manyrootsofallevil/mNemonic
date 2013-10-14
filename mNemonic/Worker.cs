using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace mNemonic
{
    public class Worker
    {
        string StoragePath { get; set; }
        IEnumerable<string> selectedDirectories;
        List<mNeme> allmNemes = new List<mNeme>();

        public Worker(string storagePath)
        {
            StoragePath = storagePath;

            string collectionsFile = ConfigurationManager.AppSettings["CollectionsFile"];

            populatemNemeCollection(collectionsFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionsFile"></param>
        private void populatemNemeCollection(string collectionsFile)
        {

            IEnumerable<XAttribute> collectionDirectories = getCollectionDirectories(collectionsFile);

            List<string> allDirectories = Directory.GetDirectories(StoragePath).ToList();

            foreach (var relPath in collectionDirectories)
            {
                if (selectedDirectories == null)
                {
                    selectedDirectories = allDirectories.Where(x => x.Equals(Path.Combine(StoragePath, relPath.Value), StringComparison.InvariantCultureIgnoreCase));
                }
                else
                {
                    selectedDirectories = allDirectories.Where(x => x.Equals(Path.Combine(StoragePath, relPath.Value), StringComparison.InvariantCultureIgnoreCase)).Concat(selectedDirectories);
                }
            }


            allmNemes = selectedDirectories
                .Select(x => Directory.GetDirectories(x)) //We get all the sub directories of each selected directories
                .SelectMany(y => y)                       //We flatten the returned string arrays  
                .Select(z => new mNeme(z))                //We create a new nNeme for each of these
                .ToList();                                //Not sure why....
        }

        /// <summary>
        /// Gets a collection of Directories that should be iterated through for remembering
        /// </summary>
        /// <param name="setsFile"></param>
        /// <returns></returns>
        private static IEnumerable<XAttribute> getCollectionDirectories(string setsFile)
        {
            XDocument xdoc = XDocument.Load(setsFile);

            IEnumerable<XAttribute> collectionDirectories = xdoc.Root.Descendants().Select(x => x.Attribute("relativePath"));

            return collectionDirectories;
        }

        public Task<mNeme> GetNextItemAsync()
        {
            return Task.Run(() =>
            {
                return allmNemes.ElementAt(new Random().Next(allmNemes.Count));
            });
        }

        Func<mNeme> bob = () => new mNeme("");

    }
}
