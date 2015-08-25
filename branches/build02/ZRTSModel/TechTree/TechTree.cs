using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using System.IO;
using System.Xml;
using ZRTSModel.Factories;
using ZRTSModel.Player;
namespace ZRTSModel.TechTree
{
	/// <summary>
	/// This class will be used to define the "Tech-Tree" of the game. It will define what units and buildings the player must
	/// have in order to produce/build newer buildings and units.
	/// 
	/// Example XML: Defines a TechTree that contains a building "house" and a unit "soldier." "house" has no requirements while
	/// "soldier" requires a house.
	/// 
	/// <TechTree>
	///		<Requirements type = "building" btype = "house">
	///			<ReqList>
	///			</ReqList>
	///		</Requirements>
	/// 
	///		<Requirements type = "unit" utype = "soldier">
	///			<ReqList>
	///				<Req type = "building" btype = "house"/>
	///			</ReqList>
	///		</Requirements>
	/// </TechTree>
	/// </summary>
	public class TechTree
	{
		const string TECH_TREE_FILE = "Content/techTree.xml";
		Dictionary<UnitStats, ReqList> unitReqs; // Requirements for units.
		Dictionary<BuildingStats, ReqList> buildingReqs; // Requirements for buildings.

		public TechTree()
		{
			string xmlString = readFile(TECH_TREE_FILE);
			unitReqs = new Dictionary<Entities.UnitStats, ReqList>();
			buildingReqs = new Dictionary<Entities.BuildingStats, ReqList>();
			parseListXML(xmlString);
		}

		private void parseListXML(string xml)
		{
			bool endOfList = false;
			XmlReader reader = XmlReader.Create(new StringReader(xml));

			while (!endOfList)
			{
				try
				{
					reader.ReadToFollowing("Requirements");
					string type = reader.GetAttribute("type");

					if (type.ToUpper().Equals("BUILDING"))
					{
						string btype = reader.GetAttribute("btype");
						BuildingStats bstats = BuildingFactory.Instance.getStats(btype);
						if (bstats != null)
						{
							string innerXML = reader.ReadInnerXml();
							ReqList reqList = new ReqList(innerXML, bstats);
							this.buildingReqs.Add(bstats, reqList);
						}
						
					}
					else if (type.ToUpper().Equals("UNIT"))
					{
						string utype = reader.GetAttribute("utype");
						UnitStats ustats = UnitFactory.Instance.getStats(utype);

						if (ustats != null)
						{
							string innerXML = reader.ReadInnerXml();
							ReqList reqList = new ReqList(innerXML, ustats);
							this.unitReqs.Add(ustats, reqList);
						}
					}
					
				}
				catch
				{
					endOfList = true;
				}
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
		/// This function returns a Dictionary<BuildingStats, ReqList> of all of the requirements necessary for buildings
		/// </summary>
		/// <returns></returns>
		public Dictionary<BuildingStats, ReqList> getBuildingRequirements()
		{
			return this.buildingReqs;
		}

		/// <summary>
		/// Given a BuildingStats object, this function will return a ReqList defining the requirements necessary to build 
		/// that Building.
		/// </summary>
		/// <param name="bstats"></param>
		/// <returns></returns>
		public ReqList getBuildingRequirements(BuildingStats bstats)
		{
			return this.buildingReqs[bstats];
		}

		/// <summary>
		/// This function returns a Dictonary<UnitStats, ReqList> of all of the requirements necessary for units.
		/// </summary>
		/// <returns></returns>
		public Dictionary<UnitStats, ReqList> getUnitRequirements()
		{
			return this.unitReqs;
		}

		/// <summary>
		/// Given a UnitStats object, this function will return a ReqList defining the requirements necessary to produce
		/// that Unit.
		/// </summary>
		/// <param name="ustats"></param>
		/// <returns></returns>
		public ReqList getUnitRequirements(UnitStats ustats)
		{
			return this.unitReqs[ustats];
		}

		/// <summary>
		/// This function will deterimine if a Player meets the requirements to produce a Unit.
		/// </summary>
		/// <param name="player">Player being tested</param>
		/// <param name="ustats">UnitStats defining the Unit</param>
		/// <returns>true if the player meets the requirements, false if the player does not.</returns>
		public bool playerMeetsRequirements(Player.Player player, UnitStats ustats)
		{
			ReqList reqList = unitReqs[ustats];
			return reqList.playerMeetsReqs(player);
		}

		/// <summary>
		/// This function will determine if a Player meets the requirements to produce a Building.
		/// </summary>
		/// <param name="player">Playe being tested.</param>
		/// <param name="bstats">BuildingStats defining the Building.</param>
		/// <returns>true if the player meets the requirements, false if the player does not.</returns>
		public bool playerMeetsRequirements(Player.Player player, BuildingStats bstats)
		{
			ReqList reqList = buildingReqs[bstats];
			return reqList.playerMeetsReqs(player);
		}
	}
}
