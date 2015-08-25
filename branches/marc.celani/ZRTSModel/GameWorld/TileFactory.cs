using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;

namespace ZRTSModel.GameWorld
{
    /// <summary>
    /// This class will generate a list of Tiles based on the file "Content/tiles.xml". 
    /// 
    /// - Daniel Gephardt
    /// </summary>
    public class TileFactory
    {
        private static TileFactory instance;

        private static string TILES_FILE = "Content/tiles.xml";
        private static string TILES_DIRECTORY = "Content/Images/";

        List<Tile> tiles;
        List<string> tileTypes;
        Dictionary<string, Tile> tileDict;
        Dictionary<string, Bitmap> bitmaps;

        /// <summary>
        /// Creates a new TileFactory and loads the tile information from the xml file.
        /// </summary>
        private TileFactory()
        {
            tiles = new List<Tile>();
            tileTypes = new List<string>();
            tileDict = new Dictionary<string, Tile>();
            bitmaps = new Dictionary<string, Bitmap>();
            string xml = readFile(TILES_FILE);
            parseTilesXML(xml);
        }

        /// <summary>
        /// Gets an instance of the singleton TileFactory.
        /// </summary>
        public static TileFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TileFactory();
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
            while (reader.ReadToFollowing("Tile"))
            {

                    string type = reader.GetAttribute("type");              // read type attribute
                    string passableStr = reader.GetAttribute("passable");   // read passable attribute
                    string indexStr = reader.GetAttribute("index");         // read index attribute

                    // Convert passableStr into a bool.
                    bool passable = true;
                    if (passableStr.ToUpper().Equals("FALSE"))
                    {
                        passable = false;
                    }

                    // Convert indexStr into a string.
                    int index = -1;
                    index = Int32.Parse(indexStr);

                    // Create a new Tile and add it to the list.
                    tileTypes.Add(type);
                    // Tile tile = new Tile(type, passable, index);
                    // tiles.Add(tile);
                    // tileDict.Add(type, tile);
                    Bitmap bm = new Bitmap(TILES_DIRECTORY + type + ".png");
                    bitmaps.Add(type, bm);
            }

            reader.Close();
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

        /// <summary>
        /// </summary>
        /// <returns>A list of Tiles.</returns>
        public List<Tile> getTiles()
        {
            return this.tiles;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns a list of strings containing the tile types.</returns>
        public List<string> getTileTypes()
        {
            return this.tileTypes;
        }

        /// <summary>
        /// Given a string representing the Tile type, returns that Tile.
        /// </summary>
        /// <param name="type">the type of the Tile.</param>
        /// <returns>a Tile</returns>
        public Tile getTile(string type)
        {
            return tileDict[type];
        }

        /// <summary>
        /// Given a string representing the Tile type, returns that Bitmap.
        /// </summary>
        /// <param name="type">the type of the Tile.</param>
        /// <returns>a Tile</returns>
        public Bitmap getBitmap(string type)
        {
            return bitmaps[type];
        }

        public ZRTSModel.Tile GetImprovedTile(string type)
        {
            ZRTSModel.Tile tile = null;
            // TODO Improve this.
            if (type != null)
            {
                if (type.Equals("grass"))
                {
                    return new Grass();
                }
                else if (type.Equals("mountain"))
                {
                    return new Mountain();
                }
                else if (type.Equals("lightgrass"))
                {
                    return new Sand();
                }
            }
            return tile;
        }

        // TODO: Fix this to follow Visitor pattern.  Just testing for now.
        public Bitmap getBitmapImproved(ZRTSModel.Tile tile)
        {
            if (tile.GetType() == typeof(Mountain))
            {
                return bitmaps["mountain"];
            }
            else if (tile.GetType() == typeof(Grass))
            {
                return bitmaps["grass"];
            }
            else if (tile.GetType() == typeof(Sand))
            {
                return bitmaps["lightgrass"];
            }
            return null;
        }
    }
}
