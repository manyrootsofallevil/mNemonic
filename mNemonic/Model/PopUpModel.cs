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
                  {//TODO: This needs thinking about more carefully. What do you want to store here?
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
                .Where(x => x.Attribute("Location").Value.Equals(this.currentmNeme.Location, StringComparison.InvariantCultureIgnoreCase));

                if (alreadyStored.Count() == 1)
                {
                    alreadyStored.FirstOrDefault().Attribute("mNemeCoefficient").Value = mNemeCoefficient.ToString();

                    //The idea here is that if we do remember it we don't want it showning up until the interval has passed
                    //The interval size is pretty much a guess at the moment. 
                    //Furthermore, the interval increases with each remembered time.
                    if (mNemeCoefficient == Constants.doRemember)
                    {
                        int remembered = Convert.ToInt32(alreadyStored.FirstOrDefault().Attribute("Remembered").Value);
                        alreadyStored.FirstOrDefault().Attribute("Time").Value = DateTime.Now.AddDays(++remembered * Constants.intervalForRememberedmNemes).Ticks.ToString();

                    }
                    else
                    {
                        //If the item has not been fully remembered then the interval is simply the ticking interval times 5
                        //This means that some might show up again on the same day.
                        alreadyStored.FirstOrDefault().Attribute("Time").Value =
                            DateTime.Now.AddSeconds(double.Parse(ConfigurationManager.AppSettings["Interval"]) * 1000 * 60 * 5).Ticks.ToString();
                        //Reset the interval if the mNeme is forgotten. Not sure if this is correct.
                        alreadyStored.FirstOrDefault().Attribute("Remembered").Value = "0";
                    }
                }
                else
                {
                    doc.Root.Add(new XElement("mNeme", new XAttribute("Location", this.currentmNeme.Location),
                    new XAttribute("mNemeCoefficient", mNemeCoefficient), new XAttribute("Time", DateTime.Now.AddSeconds(double.Parse(ConfigurationManager.AppSettings["Interval"]) * 1000 * 60 * 5).Ticks),
                    new XAttribute("Remembered", 0)));
                }
            }
            else
            {
                doc = new XDocument();
                doc.Add(new XElement("mNemes"));

                doc.Root.Add(new XElement("mNeme", new XAttribute("Location", this.currentmNeme.Location),
                   new XAttribute("mNemeCoefficient", mNemeCoefficient), new XAttribute("Time", DateTime.Now.AddSeconds(double.Parse(ConfigurationManager.AppSettings["Interval"]) * 1000 * 60 * 5).Ticks),
                   new XAttribute("Remembered", 0)));
            }
            doc.Save(storeFile);
        }

        private void CollectStats(int mNemeCoefficient, string storeFile)
        {
            int interval = 0, count = 0;
            XDocument doc;

            if (File.Exists(storeFile))
            {
                doc = XDocument.Load(storeFile);
                var alreadyStored = doc.Root.Elements()
               .Where(x => x.Attribute("Location").Value.Equals(this.currentmNeme.Location, StringComparison.InvariantCultureIgnoreCase))
               .OrderByDescending(x => x.Attribute("Time").Value);

                count = alreadyStored.Count();

                if (alreadyStored.Count() > 0)
                {
                    interval = (DateTime.Now - Convert.ToDateTime(alreadyStored.First().Attribute("Time").Value)).Minutes;
                }
            }
            else
            {
                doc = new XDocument();
                doc.Add(new XElement("mNemes"));
            }

            doc.Root.Add(new XElement("mNeme", new XAttribute("Location", this.currentmNeme.Location),
            new XAttribute("mNemeCoefficient", mNemeCoefficient), new XAttribute("Time", DateTime.Now.ToString("o")),
            new XAttribute("NumberoftimesDisplayed", count),
            new XAttribute("IntervalSinceLastDisplayinMinutes", interval)));

            doc.Save(storeFile);
        }
    }
}
