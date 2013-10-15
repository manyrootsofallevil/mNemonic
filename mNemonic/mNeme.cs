using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mNemonic
{
    public class mNeme
    {
        public mNemeType Type { get; set; }
        public string Location { get; set; }
        //TODO: this should have a type of file which will be the main file to be displayed, e.g. a picture or a piece of text, etc..
        //as well as the dimensions of the control so that it can be displayed accordingly, which means that we need to get the dimensions
        //of the image here. We should probably store the text here as well or should we?
        public List<Tuple<string,mNemeType>> Items{get; set;}

        public mNeme(mNemeType type, string location)
        {
            Type = type;
            Location = location;
            Items = new List<Tuple<string, mNemeType>>();
            getItems();
        }

        public mNeme( string location)
        {
            Location = location;
            Items = new List<Tuple<string, mNemeType>>();
            getItems();
        }

        private void getItems()
        {
            var files = Directory.EnumerateFiles(Location);
            //TODO: This cleary needs changing to be more robust.
            foreach (var file in files)
            {
                if (file.Split('.').Take(1).Equals("txt"))
                {
                    Items.Add(new Tuple<string, mNemeType>(file, mNemeType.Text));

                }
                else
                {
                    Items.Add(new Tuple<string, mNemeType>(file, mNemeType.Image));
                }
            }
         
          
           
        }

    }
}
