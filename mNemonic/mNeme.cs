﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mNemonic
{
    public class mNeme: IEquatable<mNeme>
    {
        private TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        public mNemeType Type { get; set; }
        public string Location { get; set; }
        public string Collection { get; set; }
        public string Name { get; set; }
        //These are really for sanity checking. To ensure that it's behaving as it should.
        public bool CompletelyRandomlySelected { get; set; }
        public bool PartiallyRandomlySelected { get; set; }
       
        public List<Tuple<string, FileType>> Items { get; set; }

        public mNeme(string location, mNemeType type)
        {
            Type = type;
            Location = location;
            getNames();
            Items = new List<Tuple<string, FileType>>();
            getItems();
        }

        public mNeme(string location)
        {
            Location = location;
            Items = new List<Tuple<string, FileType>>();
            getNames();

            try
            {
                getItems();
            }
            catch (Exception ex)
            {
            }
        }

        public bool Equals(mNeme other)
        {
            bool result = false;

            if (this.Location.Equals(other.Location, StringComparison.InvariantCultureIgnoreCase))
            {
                result = true;
            }

            return result;
        }

        public override int GetHashCode()
        {

            //Get hash code for the Name field if it is not null. 
            int hashmNemeType = Type== null ? 0 : Type.GetHashCode();

            //Get hash code for the Code field. 
            int hashmNemeLocation = Location.GetHashCode();

            int hashName = Name.GetHashCode();

            int hashCollection = Collection.GetHashCode();

            //Calculate the hash code for the product. 
            return hashmNemeType ^ hashmNemeLocation ^ hashCollection ^ hashName;
        }

        private void getItems()
        {
            var files = Directory.EnumerateFiles(Location);
           
            var extensions = files.Select(x => x.Split('.').Last());

            foreach (var file in files)
            {
                switch (file.Split('.').Last().ToLower())
                {
                    case "txt":
                        Items.Add(new Tuple<string, FileType>(file, FileType.Text));

                        if (Type == mNemeType.Unknown)
                        {
                            Type = mNemeType.Text;
                        }
                        break;
                    case "png":
                    case "jpg":
                        Items.Add(new Tuple<string, FileType>(file, FileType.Image));
                        Type = mNemeType.Image;
                        break;
                }

            }

        }

        private void getNames()
        {
            DirectoryInfo locationInfo = new DirectoryInfo(this.Location);

            this.Name = ti.ToTitleCase(locationInfo.Name);
            this.Collection = ti.ToTitleCase(locationInfo.Parent.Name);
        }


    }
}
