using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace mNemonic.Model
{
    public class PopUpModel
    {
        const string zero = "0";
        public mNeme currentmNeme { get; private set; }
        private string DBFile;
        private string StatsFile;
        private bool collectStats = false;

        public PopUpModel(mNeme mNeme)
        {
            this.currentmNeme = mNeme;
            this.DBFile = ConfigurationManager.AppSettings["DBFile"];
            this.StatsFile = ConfigurationManager.AppSettings["StatsFile"];
            this.collectStats = Boolean.Parse(ConfigurationManager.AppSettings["CollectStats"]);
        }

        async public Task<bool> DoTheRemembering(int mNemeCoefficient)
        {
            return await Task.Run(() =>
              {
                  StoreInFile(mNemeCoefficient, this.DBFile);

                  if (collectStats)
                  {
                      CollectStats(mNemeCoefficient, this.StatsFile);
                  }

                  return true;
              });
        }

        private void StoreInFile(int mNemeCoefficient, string storeFile)
        {
            XDocument doc;

            if (File.Exists(storeFile))
            {
                doc = XDocument.Load(storeFile);

                var alreadyStored = doc.Root.Elements()
                .Where(x => x.Attribute("Collection").Value.Equals(this.currentmNeme.Collection, StringComparison.InvariantCultureIgnoreCase)
                && x.Attribute("Name").Value.Equals(this.currentmNeme.Name, StringComparison.InvariantCultureIgnoreCase));

                if (alreadyStored.Count() == 1)
                {
                    alreadyStored.FirstOrDefault().Attribute("mNemeCoefficient").Value = mNemeCoefficient.ToString();

                    //The idea here is that if we do remember it we don't want it showning up until the interval has passed
                    //The interval size is pretty much a guess at the moment. 
                    //Furthermore, the interval increases with each remembered time.
                    if (mNemeCoefficient == Constants.doRemember)
                    {
                        int remembered = Convert.ToInt32(alreadyStored.FirstOrDefault().Attribute("Remembered").Value);
                        int forgotten = Convert.ToInt32(alreadyStored.FirstOrDefault().Attribute("Forgotten").Value);

                        alreadyStored.FirstOrDefault().Attribute("Time").Value = SetNextTime(mNemeCoefficient, remembered);
                        alreadyStored.FirstOrDefault().Attribute("Remembered").Value = (++remembered).ToString();
                        alreadyStored.FirstOrDefault().Attribute("Forgotten").Value = forgotten == 0 ? zero : (--forgotten).ToString();

                    }
                    else
                    {
                        //If the item has not been fully remembered then the interval is simply the ticking interval times
                        //NofIntervalForNotRemembered (set to 5).
                        //This means that some might show up again on the same day assuming 60 minutes interval.
                        //They might show up anyway, as if there isn't anything that should be shown a mNeme will be shown up at random
                        alreadyStored.FirstOrDefault().Attribute("Time").Value = SetNextTime(mNemeCoefficient);

                        //Rather than fully resetting the interval if the mNeme is forgot
                        int remembered = Convert.ToInt32(alreadyStored.FirstOrDefault().Attribute("Remembered").Value);
                        int forgotten = Convert.ToInt32(alreadyStored.FirstOrDefault().Attribute("Forgotten").Value);

                        alreadyStored.FirstOrDefault().Attribute("Forgotten").Value = remembered > 0 ? (++forgotten).ToString() : zero;

                        if (remembered > 0)
                        {
                            remembered -= forgotten;
                            alreadyStored.FirstOrDefault().Attribute("Remembered").Value = remembered.ToString();
                        }
                    }
                }
                else
                {
                    AddNewmNemeToFile(mNemeCoefficient, doc);
                }
            }
            else
            {
                doc = new XDocument();
                doc.Add(new XElement("mNemes"));

                AddNewmNemeToFile(mNemeCoefficient, doc);
            }

            doc.Save(storeFile);
        }

        private void AddNewmNemeToFile(int mNemeCoefficient, XDocument doc)
        {
            doc.Root.Add(new XElement("mNeme", new XAttribute("Collection", this.currentmNeme.Collection),
                new XAttribute("Name", this.currentmNeme.Name),
                new XAttribute("mNemeCoefficient", mNemeCoefficient),
                new XAttribute("Time", SetNextTime(mNemeCoefficient)),
                new XAttribute("Remembered", mNemeCoefficient == Constants.doRemember ? 1 : 0),
                new XAttribute("Forgotten", zero)) //A new one can't have been forgotten ;)
               );
        }

        private string SetNextTime(int coefficient, int remembered = 0)
        {
            string result = string.Empty;

            if (coefficient == Constants.doRemember)
            {
                result = DateTime.Now.AddDays(++remembered * Constants.intervalForRememberedmNemes).Ticks.ToString();
            }
            else
            {
                result = DateTime.Now.AddMinutes(double.Parse(ConfigurationManager.AppSettings["Interval"])
                                            * double.Parse(ConfigurationManager.AppSettings["NofIntervalForNotRemembered"])).Ticks.ToString();
            }
            return result;
        }

        private void CollectStats(int mNemeCoefficient, string storeFile)
        {
            int interval = 0, count = 0;
            XDocument doc;

            if (File.Exists(storeFile))
            {
                doc = XDocument.Load(storeFile);
                var alreadyStored = doc.Root.Elements()
               .Where(x => x.Attribute("Collection").Value.Equals(this.currentmNeme.Collection, StringComparison.InvariantCultureIgnoreCase)
                && x.Attribute("Name").Value.Equals(this.currentmNeme.Name, StringComparison.InvariantCultureIgnoreCase))
               .OrderByDescending(x => x.Attribute("Time").Value);

                count = alreadyStored.Count();

                if (alreadyStored.Count() > 0)
                {
                    interval = (int)(DateTime.Now - Convert.ToDateTime(alreadyStored.First().Attribute("Time").Value)).TotalMinutes;
                }
            }
            else
            {
                doc = new XDocument();
                doc.Add(new XElement("mNemes"));
            }

            doc.Root.Add(new XElement("mNeme", new XAttribute("Collection", this.currentmNeme.Collection),
            new XAttribute("Name", this.currentmNeme.Name),
            new XAttribute("mNemeCoefficient", mNemeCoefficient), new XAttribute("Time", DateTime.Now.ToString("o")),
            new XAttribute("NumberoftimesDisplayed", ++count),
            new XAttribute("IntervalSinceLastDisplayinMinutes", interval),
            new XAttribute("RandomlySelected", this.currentmNeme.PartiallyRandomlySelected)));

            doc.Save(storeFile);
        }
    }
}
