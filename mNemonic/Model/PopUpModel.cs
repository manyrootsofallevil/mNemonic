﻿using System;
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
                        alreadyStored.FirstOrDefault().Attribute("Time").Value = SetNextTime(mNemeCoefficient, remembered);
                        alreadyStored.FirstOrDefault().Attribute("Remembered").Value = (++remembered).ToString();

                    }
                    else
                    {
                        //If the item has not been fully remembered then the interval is simply the ticking interval times
                        //NofIntervalForNotRemembered (set to 5).
                        //This means that some might show up again on the same day assuming 60 minutes interval.
                        //They might show up anyway, as if there isn't anything that should be shown a mNeme will be shown up at random
                        alreadyStored.FirstOrDefault().Attribute("Time").Value = SetNextTime(mNemeCoefficient);
                        //Reset the interval if the mNeme is forgotten. Not sure if this is correct.
                        alreadyStored.FirstOrDefault().Attribute("Remembered").Value = "0";
                    }
                }
                else
                {
                    doc.Root.Add(new XElement("mNeme", new XAttribute("Location", this.currentmNeme.Location),
                    new XAttribute("mNemeCoefficient", mNemeCoefficient), new XAttribute("Time", SetNextTime(mNemeCoefficient)),
                    new XAttribute("Remembered", mNemeCoefficient == Constants.doRemember ? 1 : 0)));
                }
            }
            else
            {
                doc = new XDocument();
                doc.Add(new XElement("mNemes"));

                doc.Root.Add(new XElement("mNeme", new XAttribute("Location", this.currentmNeme.Location),
                   new XAttribute("mNemeCoefficient", mNemeCoefficient), new XAttribute("Time", SetNextTime(mNemeCoefficient)),
                   new XAttribute("Remembered", mNemeCoefficient == Constants.doRemember ? 1 : 0)));
            }
            doc.Save(storeFile);
        }

        private string SetNextTime(int coefficient, int remembered=0)
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
               .Where(x => x.Attribute("Location").Value.Equals(this.currentmNeme.Location, StringComparison.InvariantCultureIgnoreCase))
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

            doc.Root.Add(new XElement("mNeme", new XAttribute("Location", this.currentmNeme.Location),
            new XAttribute("mNemeCoefficient", mNemeCoefficient), new XAttribute("Time", DateTime.Now.ToString("o")),
            new XAttribute("NumberoftimesDisplayed", ++count),
            new XAttribute("IntervalSinceLastDisplayinMinutes", interval),
            new XAttribute("RandomlySelected", this.currentmNeme.PartiallyRandomlySelected)));

            doc.Save(storeFile);
        }
    }
}
