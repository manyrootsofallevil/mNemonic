using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mNemonic;
using System.Configuration;
using mNemonic.Model;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace mNemonicTests
{
    [TestClass]
    public class mNemonicTests
    {

        const string mNemeLocation = @"C:\mNemonic\Unusual Words\Acersecomic";

        //This will only work if mNemeLocation exits and contains a mNeme
        [TestMethod]
        public void StatsCollector()
        {
            int expected = 1;
            int actual = 0;

            RememberTheSamemNemeTwice();

            XDocument doc = XDocument.Load(ConfigurationManager.AppSettings["StatsFile"]);

            var statRecord = doc.Root.Elements()
                .Where(x => x.Attribute("Location").Value.Equals(mNemeLocation, StringComparison.InvariantCultureIgnoreCase))
                .OrderByDescending(x => x.Attribute("Time").Value).FirstOrDefault();

            actual = Int32.Parse(statRecord.Attribute("IntervalSinceLastDisplayinMinutes").Value);

            Assert.AreEqual(expected, actual);

        }

        private static void RememberTheSamemNemeTwice()
        {
            mNeme first = new mNeme(mNemeLocation, mNemeType.Text);
            PopUpModel firstmodel = new PopUpModel(first);
            firstmodel.DoTheRemembering(1);

            System.Threading.Thread.Sleep(60 * 1000);

            mNeme second = new mNeme(mNemeLocation, mNemeType.Text);
            PopUpModel secondmodel = new PopUpModel(first);
            firstmodel.DoTheRemembering(1);
        }

        [TestMethod]
        public void GetmNemes()
        {
            Worker worker = new Worker(ConfigurationManager.AppSettings["Maindirectory"],100);
            
            Task<mNeme> next;

            do
            {
                next = worker.GetNextItemAsync();
                PopUpModel model = new PopUpModel(next.Result);
                System.Diagnostics.Debug.WriteLine("{0} {1}",next.Result.Name, next.Result.PartiallyRandomlySelected);
                var x =model.DoTheRemembering(1);
                x.Wait();

            } while (next.Result.PartiallyRandomlySelected != true);

        }

    }
}
