using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml;

namespace ZRTSMapEditor.MapEditorModel{

    class BitmapManager
    {
        private static BitmapManager instance;

        private static string BITMAPS_FILE = "Content/bitmaps.xml";
        private static string BITMAPS_DIRECTORY = "Content/Images/";

        Dictionary<string, Bitmap> bitmaps;
        List<string> types;

        private BitmapManager()
        {
            types = new List<string>();
            bitmaps = new Dictionary<string, Bitmap>();
            string xml = readFile(BITMAPS_FILE);
            parseTilesXML(xml);
        }

        /// <summary>
        /// Gets an instance of the singleton BitmapManager.
        /// </summary>
        public static BitmapManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BitmapManager();
                }
                return instance;
            }
        }


        /// <summary>
        /// Parses the xml string for the Tile information.
        /// </summary>
        /// <param name="xml">The string containing the xml data.</param>
        private void parseTilesXML(string xml)
        {
            XmlReader reader = XmlReader.Create(new StringReader(xml));

            // While not at the end of the file.
            while (reader.ReadToFollowing("Bitmap"))
            {

                string type = reader.GetAttribute("type");              // read type attribute
                string filename = reader.GetAttribute("filename");   // read passable attribute

                // Create a new Tile and add it to the list.
                types.Add(type);
                Bitmap bm = new Bitmap(BITMAPS_DIRECTORY + filename);
                bm.MakeTransparent(Color.FromArgb(255, 255, 255));
                bitmaps.Add(type, bm);
            }

            reader.Close();
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns a list of strings containing the bitmap types.</returns>
        public List<string> getTypes()
        {
            return this.types;
        }

        /// <summary>
        /// Given a string representing the Bitmap type, returns that Bitmap
        /// </summary>
        /// <param name="type">the type of the Bitmap.</param>
        /// <returns>a Bitmap</returns>
        public Bitmap getBitmap(string type)
        {
            return bitmaps[type];
        }

        private string readFile(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            string input = "";
            try
            {
                do
                {
                    input += (reader.ReadLine());
                }
                while (reader.Peek() != -1);
            }

            catch
            {
            }

            finally
            {
                reader.Close();
            }

            return input;
        }
    }

}
