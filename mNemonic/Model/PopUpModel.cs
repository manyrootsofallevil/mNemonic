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
        public const int dontRemember = 1;
        public const int vaguelyRemember = 2;
        public const int doRemember = 3;
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
                  XDocument doc;
                  //TODO: need to update entries rather than continuously add them. As otherwise the size of this DB will balloon.
                  if (File.Exists(this.DBFile))
                  {
                      doc = XDocument.Load(this.DBFile);
                      doc.Root.Add(new XElement("mNeme",  new XAttribute("Location", this.currentmNeme.Location),
                      new XAttribute("mNemeCoefficient", mNemeCoefficient), new XAttribute("Time", DateTime.Now.Ticks)));
                  }
                  else
                  {
                      doc = new XDocument();
                      doc.Add(new XElement("mNemes"));
                      doc.Root.Add(new XElement("mNeme",  new XAttribute("Location", this.currentmNeme.Location),
                      new XAttribute("mNemeCoefficient", mNemeCoefficient), new XAttribute("Time", DateTime.Now.Ticks)));
                      
                  }

                  doc.Save(this.DBFile);

                  return true;
              });
        }
    }
}
