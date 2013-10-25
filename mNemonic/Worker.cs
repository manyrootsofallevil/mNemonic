﻿using System;
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
        long ticksToSeconds = 10000000;
        string StoragePath { get; set; }
        string DBFile { get; set; }

        IEnumerable<string> selectedDirectories;

        IEnumerable<mNeme> allmNemes = new List<mNeme>();

        public Worker(string storagePath)
        {
            StoragePath = storagePath;
            DBFile = ConfigurationManager.AppSettings["DBFile"];

            populatemNemeCollection(ConfigurationManager.AppSettings["CollectionsFile"]);
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
                .Select(x => Directory.GetDirectories(x))      //We get all the sub directories of each selected directories
                .SelectMany(y => y)                            //We flatten the returned string arrays  
                .Select(z => new mNeme(z))                     //We create a new nNeme for each of these
                .Where(n => !n.Type.Equals(mNemeType.Unknown));//We filter the unknowns out
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
                mNeme result = null;

                if (File.Exists(DBFile))
                {
                    //1. Load the DB file with all the data regarding when and how well remembered the mNeme was
                    XDocument xdoc = XDocument.Load(DBFile);
                    //2. Create a collection of the stored mNemes (This is unlikely to be terribly efficient) 
                    //TODO: look for a better way.
                    var storedmNemes = xdoc.Root.Elements()
                        .Select(x => new
                        {
                            Location = x.Attribute("Location").Value,
                            Coefficient = Int32.Parse(x.Attribute("mNemeCoefficient").Value),
                            Time = ((DateTime.Now.Ticks - Int64.Parse(x.Attribute("Time").Value)) / ticksToSeconds)
                        });
                    //3. Join with the selected mNemes
                    var availablemNemes = allmNemes.Join(storedmNemes, x => x.Location, y => y.Location, (x, y) => y).Distinct();
                    //4. Find the mNemes that meet certain criteria
                    var selection = availablemNemes.Where(x => x.Time * x.Coefficient > 100);

                    if (selection.Count() ==0)
                    {//5. If we don't find any, we just return one at random.
                        result = allmNemes.ElementAt(new Random().Next(allmNemes.Count()));
                    }
                    else
                    {//6. If we do, we still return one at random TODO. This needs to be better
                        result = new mNeme(selection.ElementAt(new Random().Next(selection.Count())).Location);
                    }
                }
                else
                {
                    result = allmNemes.ElementAt(new Random().Next(allmNemes.Count()));
                }

                return result;
            });
        }
    }
}
