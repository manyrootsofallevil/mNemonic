using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace mNemonic
{
    public class Worker
    {
        const long ticksToSeconds = 10000000;

        string StoragePath { get; set; }
        string DBFile { get; set; }
        int TimerInterval { get; set; }

        IEnumerable<string> selectedDirectories;

        IEnumerable<mNeme> allmNemes = new List<mNeme>();

        Random rng;

        public Worker(string storagePath)
        {
            StoragePath = storagePath;
            DBFile = ConfigurationManager.AppSettings["DBFile"];
            TimerInterval = (((Timer)App.Current.FindResource("Timer")).Interval + 1337) / 1000;

            populatemNemeCollection(ConfigurationManager.AppSettings["CollectionsFile"]);

            rng = new Random();
        }

        public Worker(string storagePath, int interval)
        {
            StoragePath = storagePath;
            DBFile = ConfigurationManager.AppSettings["DBFile"];
            TimerInterval = interval;

            populatemNemeCollection(ConfigurationManager.AppSettings["CollectionsFile"]);

            rng = new Random();
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
                .Select(z => new mNeme(z.ToLowerInvariant()))  //We create a new nNeme for each of these
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

            IEnumerable<XAttribute> collectionDirectories = xdoc.Root.Descendants()
                .Where(x => Boolean.Parse(x.Attribute("Enabled").Value))
                .Select(x => x.Attribute("relativePath"));

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
                    var storedmNemes = xdoc.Root.Elements()
                        .Select(x => new StoredmNeme(
                            Path.Combine(ConfigurationManager.AppSettings["MainDirectory"],x.Attribute("Collection").Value.ToLowerInvariant(),x.Attribute("Name").Value.ToLowerInvariant()), 
                            Int32.Parse(x.Attribute("mNemeCoefficient").Value)
                        , ((DateTime.Now.Ticks - Int64.Parse(x.Attribute("Time").Value)) / TimeSpan.TicksPerSecond),
                         Int32.Parse(x.Attribute("Remembered").Value)));

                    //3. Join with the selected mNemes
                    var availablemNemes = allmNemes.Join(storedmNemes,
                        x => x.Location.ToLowerInvariant(), y => y.Location.ToLowerInvariant(), (x, y) => y).Distinct();


                    //3.5 if the sequences are different in length then, get a random mNeme of those that are not currently stored
                    if (availablemNemes.Count() < allmNemes.Count())
                    {
                        var alreadyShown = storedmNemes.Select(x => new mNeme(x.Location));

                        List<mNeme> toBeShown = new List<mNeme>();

                        foreach (var item in allmNemes)
                        {
                            if (!alreadyShown.Contains(item))
                            {
                                toBeShown.Add(item);
                            }
                        }
                        //no idea what i'm missing here
                        //var notShownmNemes = allmNemes.Except(alreadyShown);

                        //result = notShownmNemes.ElementAt(rng.Next(notShownmNemes.Count()));

                        result = toBeShown.ElementAt(rng.Next(toBeShown.Count()));

                    }
                    else
                    {
                        //4. Find the mNemes that meet certain criteria. At the moment this is driven by the time interval. The idea is preventing the possibility of the same
                        //mNeme appearing twice in a row. We then sort by coefficient, where the lower the value the less well remembered the mneme is
                        //then descending on time since the last time it was shown and then on the number of times it was remembered
                        var selection = availablemNemes.Where(x => x.Time > TimerInterval)
                            .OrderBy(x => x.Coefficient)
                            .ThenByDescending(x => x.Time)
                            .ThenByDescending(x => x.Remembered);

                        if (selection.Count() == 0)
                        {//5. If we don't find any, we just return one at random, which should ensure that we are not limited to the ones
                         // we already have seen. We keep looking until we find one that we havent seen before 
                            do
                            {
                                result = allmNemes.ElementAt(new Random().Next(allmNemes.Count()));
                            } while (availablemNemes.Contains(new StoredmNeme(result.Location))
                                && allmNemes.Count() != availablemNemes.Count());

                            result.PartiallyRandomlySelected = true;
                        }
                        else
                        {//6. If we do, we return the first one in the selection, on the assumption that the sorting is correct.
                            result = new mNeme(selection.ElementAt(0).Location);
                        }
                    }
                }
                else
                {
                    result = allmNemes.ElementAt(new Random().Next(allmNemes.Count()));
                    result.CompletelyRandomlySelected = true;
                }

                return result;
            });
        }
    }
}
