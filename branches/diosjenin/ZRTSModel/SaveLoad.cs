using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using ZRTSModel.GameWorld;
using System.Runtime.Serialization.Formatters.Binary;


namespace ZRTSModel
{
    public class SaveLoad
    {

        public SaveLoad()
        {
        }


        public void saveGameWorld(GameWorld.GameWorld gw, string name)
        {
            string path = "Content/savedMaps/" + name + ".bin";

            using (Stream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, gw);
            }

        }

        public GameWorld.GameWorld loadGameWorld(Stream map)
        {

            BinaryFormatter bin = new BinaryFormatter();

            GameWorld.GameWorld gw = (GameWorld.GameWorld)bin.Deserialize(map);

            return gw;
        }

    }
}
