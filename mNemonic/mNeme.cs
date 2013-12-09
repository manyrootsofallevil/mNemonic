﻿using System;
using System.Collections.Generic;
using System.Drawing;
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
       
        public List<Tuple<string, FileType>> Items { get; set; }

        public mNeme(string location, mNemeType type)
        {
            Type = type;
            Location = location;
            Items = new List<Tuple<string, FileType>>();
            getItems();
        }

        public mNeme(string location)
        {
            Location = location;
            Items = new List<Tuple<string, FileType>>();
            getItems();
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

    }
}
