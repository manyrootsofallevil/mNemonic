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
        //TODO: this should have a type of file which will be the main file to be displayed, e.g. a picture or a piece of text, etc..
        //as well as the dimensions of the control so that it can be displayed accordingly, which means that we need to get the dimensions
        //of the image here. We should probably store the text here as well or should we?
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
            //TODO: This cleary needs changing to be more robust, but the idea is that if it contains an image file then it should be
            // a mneme of type image. We can do questions and answers with two text files, which raises the question of how to identify it
            //I guess this is for the importer/create new cards.
            //So the question in text files will be a file called question.
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
