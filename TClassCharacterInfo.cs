/*
 * Creado por SharpDevelop.
 * Usuario: GNR092
 * Fecha: 23/11/2017
 * Hora: 02:22 p.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using TShockAPI;
using TShockAPI.DB;
using TClassExtended.Nivel;
using Newtonsoft.Json;

namespace TClassExtended
{
	/// <summary>
	/// Description of UserControl1.
	/// </summary>
	public static class TCCI
	{
		public static IDbConnection db { get { return TShock.DB; } }
		public static Config Cnf {get{return Config.Read(Config.path);}}
				
		#region TClassCharacterInfo
		public static TClassCharacterInfo GetUserByName(string username)
		{
			try
			{
				return GetInfo(new TClassCharacterInfo { UserName = username });
			}
			catch
			{
				return null;
			}
		}

		public static TClassCharacterInfo GetInfo(TClassCharacterInfo usename)
		{
			object name = usename.UserName;
			string query = "SELECT * FROM tclass WHERE UserName=@0;";
			try
			{
				using (var reader = db.QueryReader(query, name))
				{
					if (reader.Read())
					{
						usename = LoadInfoCharacter(usename, reader);
						return usename;
					}
				}
				return null;
			}
			catch
			{
				return null;
			}
		}

		private static TClassCharacterInfo LoadInfoCharacter(TClassCharacterInfo tclass, QueryResult r)
		{
			tclass.UserName = r.Get<string>("UserName");
			tclass.Clase = r.Get<string>("Clase");
			tclass.Level = r.Get<int>("Level");
			tclass.Exp = r.Get<int>("Exp");
			tclass.NetxExpLvl = r.Get<int>("NextExpLvl");
			tclass.HP = r.Get<int>("HP");
			tclass.Str = r.Get<int>("Str");
			tclass.Vit = r.Get<int>("Vit");
			tclass._Int = r.Get<int>("_Int");
			tclass.Agi = r.Get<int>("Agi");
			tclass.Lck = r.Get<int>("Lck");
			tclass.Money = r.Get<long>("Money");
			tclass.AhLvl = r.Get<int>("AhLvl");
			tclass.HpPercent = r.Get<double>("HpPercent");

			foreach (var item in LoadTitles(tclass.UserName))
			{
				tclass.Titles = item.Key;
				tclass.ActiveTitle = item.Value;
			}
			var ply = TSPlayer.FindByNameOrID(tclass.UserName);
			if (ply.Count != 0)
			{
				tclass.Player = ply[0];
			}
			return tclass;
		}

		public static Dictionary<List<NetTitle>, NetTitle> LoadTitles(string username)
		{
			return loadTitles(username);
		}

		private static Dictionary<List<NetTitle>, NetTitle> loadTitles(string username)
		{
			Dictionary<List<NetTitle>, NetTitle> Dic = new Dictionary<List<NetTitle>, NetTitle>();

			List<NetTitle> titles = new List<NetTitle>();

			object name = username;
			string query = "SELECT * FROM ttitles WHERE Name=@0;";
			try
			{
				using (var reader = db.QueryReader(query, name))
				{
					if (reader.Read())
					{
						var str = reader.Get<string>("Titles").Split(',');
						foreach (var item in Cnf.SetTitles)
						{
							for (int i = 0; i < str.Length; i++)
							{
								if (item.Name == str[i])
								{
									titles.Add(item);
									break;
								}

							}
						}
						var act = reader.Get<string>("AvtiveTitle");
						var active = titles.Find(x => x.Name.Contains(act));
						Dic.Add(titles, active);
						return Dic;
					}
				}
				return null;
			}
			catch
			{
				return null;
			}
		}
		#endregion
	}
	public class OnChatClass
	{
        public ClassColor Color;
        public string Format;
        public bool isConsole = false;

        public OnChatClass(TSPlayer ply, string Text)
        {
            try
            {
                #region ClassColor
                var info = TCCI.GetUserByName(ply.Name);
                if (info != null)
                {
                    if (ply.Group.Name != "superadmin")
                    {
                        if (info.Clase == PreClass.Cadete.ToString() || info.Clase == PreClass.Guerrero.ToString() || info.Clase == AdvancedClass.Berseker.ToString() || info.Clase == AdvancedClass.Paladin.ToString() || info.Clase == AdvancedClass.YoYoMaster.ToString())
                        {
                            Color = new ClassColor((byte)TCCI.Cnf.ColorClassGuerrero[0], (byte)TCCI.Cnf.ColorClassGuerrero[1], (byte)TCCI.Cnf.ColorClassGuerrero[2]);
                        }
                        if (info.Clase == PreClass.Rufian.ToString() || info.Clase == PreClass.Arquero.ToString() || info.Clase == AdvancedClass.Hunter.ToString() || info.Clase == AdvancedClass.RocketMen.ToString() || info.Clase == AdvancedClass.Sniper.ToString())
                        {
                            Color = new ClassColor((byte)TCCI.Cnf.ColorClassArquero[0],
                            (byte)TCCI.Cnf.ColorClassArquero[1],
                            (byte)TCCI.Cnf.ColorClassArquero[2]);
                        }
                        if (info.Clase == PreClass.Hechicero.ToString() || info.Clase == PreClass.Mago.ToString() || info.Clase == AdvancedClass.DrenaLife.ToString() || info.Clase == AdvancedClass.Ghost.ToString() || info.Clase == AdvancedClass.Summoner.ToString())
                        {
                            Color = new ClassColor((byte)TCCI.Cnf.ColorClassMago[0],
                            (byte)TCCI.Cnf.ColorClassMago[1],
                            (byte)TCCI.Cnf.ColorClassMago[2]);
                        }
                        if (info.Clase == "Novato")
                        {
                            Color = new ClassColor((byte)TCCI.Cnf.ColoClassNovato[0],
                            (byte)TCCI.Cnf.ColoClassNovato[1],
                            (byte)TCCI.Cnf.ColoClassNovato[2]);
                        }
                #endregion

                        #region titulos
                        if (info.ActiveTitle.Name != "none")
                        {
                            Format = string.Format(TShock.Config.Settings.ChatFormat, ply.Group.Name, "(" + info.Clase + ") ", ply.Name, " (Nivel " + info.Level + ") " + "(" + info.ActiveTitle.Name + ")", Text);


                        }
                        else
                        {
                            Format = string.Format(TShock.Config.Settings.ChatFormat, ply.Group.Name, "(" + info.Clase + ") ", ply.Name, " (Nivel " + info.Level + ")", Text);

                        }
                        #endregion
                    }
                    else
                    {
                        #region superadmin
                        if (info.ActiveTitle.Name != "none")
                        {
                            IsAdmin = true;
                            if (Color == null) Color = new ClassColor();
                            if (!ply.RealPlayer) isConsole = true;
                            Format = string.Format(TShock.Config.Settings.ChatFormat, ply.Group.Name, "(" + info.Clase + ") ", ply.Name, " (Nivel " + info.Level + ") " + "(" + info.ActiveTitle.Name + ")", Text);

                        }
                        else
                        {
                            IsAdmin = true;
                            if (Color == null) Color = new ClassColor();
                            if (!ply.RealPlayer) isConsole = true;
                            Format = string.Format(TShock.Config.Settings.ChatFormat, ply.Group.Name, "(" + info.Clase + ") ", ply.Name, " (Nivel " + info.Level + ")", Text);
                        }
                        #endregion
                    }
                }
                else
                {
                    Color = new ClassColor();
                    if (!ply.RealPlayer) isConsole = true;
                    Format = string.Format(TShock.Config.Settings.ChatFormat, ply.Group.Name, "(" + info.Clase + ") ", ply.Name, " (Nivel " + info.Level + ") " + "(" + info.ActiveTitle.Name + ")", Text);
                }
            }
            catch { }
        
        }

        public bool IsAdmin { get; set; }
    }
    public class ClassColor
    {
        public byte R = 255;
        public byte G = 255;
        public byte B = 255;
        public ClassColor()
        {

        }
        public ClassColor(byte r,byte g,byte b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
	public class TClassCharacterInfo
	{
		public string UserName { get; set; }
		public string Clase { get; set; }
		public int Level { get; set; }
		public int Exp { get; set; }
		public int NetxExpLvl { get; set; }
		public int HP { get; set; }
		public int Str { get; set; }
		public int Vit { get; set; }
		public int _Int { get; set; }
		public int Agi { get; set; }
		public int Lck { get; set; }
		public long Money { get; set; }
		public int AhLvl { get; set; }
		public double HpPercent { get; set; }
		public NPC npc { get; set; }
		public List<NetTitle> Titles { get; set; }
		public NetTitle ActiveTitle { get; set; }
		public TSPlayer Player { get; set; }

		public TClassCharacterInfo()
		{
			UserName = "";
			Clase = "";
			Level = 0;
			Exp = 0;
			NetxExpLvl = 0;
			HP = 0;
			Str = 0;
			Vit = 0;
			_Int = 0;
			Agi = 0;
			Lck = 0;
			Money = 0;
			AhLvl = 0;
			HpPercent = 0;
			npc = new NPC();
			Titles = new List<NetTitle>();
			ActiveTitle = new NetTitle();

		}

		public TClassCharacterInfo(string username, string clase, int level, int exp, int netxexplvl, int hp, int str, int vit, int _int, int agi, int lck, long money, int ahlvl, int hpPercent, NPC npc,List<NetTitle> titles,NetTitle active)
		{
			UserName = username;
			Clase = clase;
			Level = level;
			Exp = exp;
			NetxExpLvl = netxexplvl;
			HP = hp;
			Str = str;
			Vit = vit;
			_Int = _int;
			Agi = agi;
			Lck = lck;
			Money = money;
			AhLvl = ahlvl;
			HpPercent = hpPercent;
			this.npc = npc;
			Titles = titles;
			ActiveTitle = active;
			var ply = TSPlayer.FindByNameOrID(username);
			if (ply != null)
			{
				Player = ply[0];
			}
		}
	}

    [JsonObject(MemberSerialization.OptIn)]
	public struct NetTitle
	{
		public string Name { get { return _Name; } }
		public double Heal { get { return _Heal; } }
		public double Mana { get { return _Mana; } }


        [JsonProperty("Name")]
        private string _Name;
        [JsonProperty("Heal")]
        private double _Heal;
        [JsonProperty("Mana")]
        private double _Mana;

		public NetTitle(string name, double heal, double mana)
		{
			_Name = name;
			_Heal = heal;
			_Mana = mana;
		}

		public override string ToString()
		{
			return string.Format("{0},{1},{2}", _Name, _Heal, _Mana);
		}

		public static NetTitle Parse(string str)
		{
			if (str == null)
				throw new ArgumentNullException("str");
			string[] strArray = str.Split(',');
			if (strArray.Length != 3)
				throw new FormatException("String does not contain three sections.");
			return new NetTitle(strArray[0], double.Parse(strArray[1]), double.Parse(strArray[2]));
		}
	}
}