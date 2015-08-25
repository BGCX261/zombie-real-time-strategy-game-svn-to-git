using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using ZRTSModel.Entities;

namespace ZRTSModel.Factories
{
    /// <summary>
    /// This class will be used to give the correct stats to a unit based on the units type. It will read these stats 
    /// from an appropriate XML file located in "Content/units/file.xml" where "file" is the name of the xml file for the stats
    /// of an individual unit. Each unit will have its own file (i.e. soldier.xml, zombieBasic.xml, ect.)
    /// </summary>
    public class UnitFactory
    {

        string BASE_DIR = "Content/units/";
        string unitList = "unitList.xml";
        List<string> unitPrefixes;
        Dictionary<string, UnitStats> stats;

        UnitStats SoldierStats = new UnitStats();
        public UnitFactory()
        {
            unitPrefixes = new List<string>();
            stats = new Dictionary<string, UnitStats>();
            parseUnitList();
            loadAllUnitsStats();
        }

        public Unit createUnit(Unit unit, string type)
        {
            return unit;
        }

        public UnitStats getStats(string prefix)
        {
            return stats[prefix];
        }

        private void loadAllUnitsStats()
        {
            foreach (string s in unitPrefixes)
            {
                loadUnitStats(stats[s], s + ".xml");
            }
        }

        private void loadUnitStats(UnitStats stats, string fileName)
        {
            string xml = readFile(BASE_DIR + fileName);
            parseXML(stats, xml);
        }

        private void parseUnitList()
        {
            string xml = readFile(BASE_DIR + unitList);
            bool endOfList = false;
            XmlReader reader = XmlReader.Create(new StringReader(xml));

            while (!endOfList)
            {
                try
                {
                    reader.ReadToFollowing("Unit");
                    unitPrefixes.Add(reader.ReadElementContentAsString());
                }
                catch
                {
                    endOfList = true;
                }
            }

            foreach (string s in unitPrefixes)
            {
                stats.Add(s, new UnitStats());
            }

            reader.Close();
        }


        private void parseXML(UnitStats stats, string xml)
        {
            XmlReader reader = XmlReader.Create(new StringReader(xml));

            // maxHealth
            reader.ReadToFollowing("maxHealth");
            stats.maxHealth = (short)reader.ReadElementContentAsInt();

            // speed
            reader.ReadToFollowing("speed");
            stats.speed = reader.ReadElementContentAsFloat();

            // attackRange
            reader.ReadToFollowing("attackRange");
            stats.attackRange = reader.ReadElementContentAsFloat();

            // attack
            reader.ReadToFollowing("attack");
            stats.attack = (short)reader.ReadElementContentAsInt();

            //canAttack
            reader.ReadToFollowing("canAttack");
            stats.canAttack = reader.ReadElementContentAsBoolean();

            //canHarvest
            reader.ReadToFollowing("canHarvest");
            stats.canHarvest = reader.ReadElementContentAsBoolean();

            //canBuild
            reader.ReadToFollowing("canBuild");
            stats.canBuild = reader.ReadElementContentAsBoolean();

        }


        public short testRead()
        {
            return stats["soldier"].attack;
        }

        private string readFile(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            string input = "";
            try
            {
                do
                {
                    input+= (reader.ReadLine());
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

        public List<string> getPrefixes()
        {
            return this.unitPrefixes;
        }

        public void addUnit(string unitName)
        {

            UnitStats s = new UnitStats();
            unitPrefixes.Add(unitName);
            stats.Add(unitName, s);
        }

        public void outputUnits()
        {
            this.outputUnitList();
        }

        private void outputUnitList()
        {
            using (XmlWriter listWriter = XmlWriter.Create("unitList.xml"))
            {

                listWriter.WriteStartElement("UnitList");

                foreach (string s in unitPrefixes)
                {
                    listWriter.WriteElementString("Unit", s);
                    this.outputUnit(s);
                }

                listWriter.WriteEndElement();
            }

        }

        private void outputUnit(string u)
        {
            UnitStats s = stats[u];

            using (XmlWriter listWriter = XmlWriter.Create(u + ".xml"))
            {
                listWriter.WriteStartElement("Unit");

                listWriter.WriteElementString("maxHealth", s.maxHealth.ToString());
                listWriter.WriteElementString("speed", s.speed.ToString());
                listWriter.WriteElementString("attackRange", s.attackRange.ToString());
                listWriter.WriteElementString("attack", s.attack.ToString());
                listWriter.WriteElementString("canAttack", s.canAttack.ToString());
                listWriter.WriteElementString("canHarvest", s.canHarvest.ToString());
                listWriter.WriteElementString("canBuild", s.canBuild.ToString());

                listWriter.WriteEndElement();
            }

        }
    }
}
