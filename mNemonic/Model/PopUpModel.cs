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

        public PopUpModel(mNeme mNeme)
        {
            this.currentmNeme = mNeme;
            this.DBFile = ConfigurationManager.AppSettings["DBFile"];
        }

        async public Task<bool> DoTheRemembering(int mNemeCoefficient)
        {
            return await Task.Run(() =>
              {
                  XDocument doc = XDocument.Load(this.DBFile);

                  var alreadyStored = doc.Root.Elements()
                      .Where(x => x.Attribute("Location").Value.Equals(this.currentmNeme.Location, StringComparison.InvariantCultureIgnoreCase));

                  if (alreadyStored.Count() == 1)
                  {
                      alreadyStored.FirstOrDefault().Attribute("mNemeCoefficient").Value = mNemeCoefficient.ToString();
                      
                      //The idea here is that if we do remember it we don't want it showning up until the interval has passed
                      //This is pretty much a guess at the moment. It would also probably be good idea to lengthen the interval between repetitions
                      //
                      if (mNemeCoefficient == Constants.doRemember)
                      {
                          alreadyStored.FirstOrDefault().Attribute("Time").Value = DateTime.Now.AddDays(Constants.intervalForRememberedmNemes).Ticks.ToString();
                      }
                      else
                      {
                          alreadyStored.FirstOrDefault().Attribute("Time").Value = DateTime.Now.Ticks.ToString();
                      }
                  }
                  else
                  {
                      doc.Root.Add(new XElement("mNeme", new XAttribute("Location", this.currentmNeme.Location),
                      new XAttribute("mNemeCoefficient", mNemeCoefficient), new XAttribute("Time", DateTime.Now.Ticks)));
                  }

                  doc.Save(this.DBFile);

                  return true;
              });
        }
    }
}
