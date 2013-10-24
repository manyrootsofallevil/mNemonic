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
        public const int dontRemember = 0;
        public const int vaguelyRemember = 1;
        public const int doRemember = 2;
        private string DBFile;
        

        public PopUpModel(mNeme mNeme)
        {
            this.currentmNeme = mNeme;
            this.DBFile = ConfigurationManager.AppSettings["DBFile"];
        }

        async public Task<bool> DoTheRemembering(int input)
        {
            return await Task.Run(() =>
              {
                  using (StreamWriter sw = new StreamWriter(this.DBFile,true))
                  {
                      sw.WriteLine("{0},{1},{2}", this.currentmNeme.Location.Split('\\').LastOrDefault(),input, DateTime.Now.Ticks);
                  }
                  return true;
              });
        }
    }
}
