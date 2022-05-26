// Warning: Some assembly references could not be loaded. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the references to the list of loaded assemblies.
// TClassExtended.OnChatClass
using System;
using TClassExtended;
using TClassExtended.Nivel;
using TShockAPI;

public class OnChatClass
{
	public ClassColor Color;

	public string Format;

	public bool isConsole;

	public bool isAdmin
	{
		get;
		set;
	}

	public  OnChatClass(TSPlayer ply, string Text)
	{
		try
		{
			TClassCharacterInfo userByName = TCCI.GetUserByName(ply.Name);
			if (userByName != null)
			{
				if (ply.Group.Name != "superadmin")
				{
					if (userByName.Clase == PreClass.Cadete.ToString() || userByName.Clase == PreClass.Guerrero.ToString() || userByName.Clase == AdvancedClass.Berseker.ToString() || userByName.Clase == AdvancedClass.Paladin.ToString() || userByName.Clase == AdvancedClass.YoYoMaster.ToString())
					{
						Color = new ClassColor((byte)TCCI.Cnf.ColorClassGuerrero[0], (byte)TCCI.Cnf.ColorClassGuerrero[1], (byte)TCCI.Cnf.ColorClassGuerrero[2]);
					}
					if (userByName.Clase == PreClass.Rufian.ToString() || userByName.Clase == PreClass.Arquero.ToString() || userByName.Clase == AdvancedClass.Hunter.ToString() || userByName.Clase == AdvancedClass.RocketMen.ToString() || userByName.Clase == AdvancedClass.Sniper.ToString())
					{
						Color = new ClassColor((byte)TCCI.Cnf.ColorClassArquero[0], (byte)TCCI.Cnf.ColorClassArquero[1], (byte)TCCI.Cnf.ColorClassArquero[2]);
					}
					if (userByName.Clase == PreClass.Hechicero.ToString() || userByName.Clase == PreClass.Mago.ToString() || userByName.Clase == AdvancedClass.DrenaLife.ToString() || userByName.Clase == AdvancedClass.Ghost.ToString() || userByName.Clase == AdvancedClass.Summoner.ToString())
					{
						Color = new ClassColor((byte)TCCI.Cnf.ColorClassMago[0], (byte)TCCI.Cnf.ColorClassMago[1], (byte)TCCI.Cnf.ColorClassMago[2]);
					}
					if (userByName.Clase == "Novato")
					{
						Color = new ClassColor((byte)TCCI.Cnf.ColoClassNovato[0], (byte)TCCI.Cnf.ColoClassNovato[1], (byte)TCCI.Cnf.ColoClassNovato[2]);
					}
					if (userByName.ActiveTitle.Name != "none")
					{
						Format = string.Format(TShock.Config.Settings.ChatFormat, ply.Group.Name, "(" + userByName.Clase + ") ", ply.Name, " (Nivel " + userByName.Level + ") (" + userByName.ActiveTitle.Name + ")", Text);
					}
					else
					{
						Format = string.Format(TShock.Config.Settings.ChatFormat, ply.Group.Name, "(" + userByName.Clase + ") ", ply.Name, " (Nivel " + userByName.Level + ")", Text);
					}
				}
				else if (userByName.ActiveTitle.Name != "none")
				{
					isAdmin = true;
					if (Color == null)
					{
						Color = new ClassColor();
					}
					if (!ply.RealPlayer)
					{
						isConsole = true;
					}
					Format = string.Format(TShock.Config.Settings.ChatFormat, ply.Group.Name, "(" + userByName.Clase + ") ", ply.Name, " (Nivel " + userByName.Level + ") (" + userByName.ActiveTitle.Name + ")", Text);
				}
				else
				{
					isAdmin = true;
					if (Color == null)
					{
						Color = new ClassColor();
					}
					if (!ply.RealPlayer)
					{
						isConsole = true;
					}
					Format = string.Format(TShock.Config.Settings.ChatFormat, ply.Group.Name, "(" + userByName.Clase + ") ", ply.Name, " (Nivel " + userByName.Level + ")", Text);
				}
			}
			else
			{
				Color = new ClassColor();
				if (!ply.RealPlayer)
				{
					isConsole = true;
				}
				Format = string.Format(TShock.Config.Settings.ChatFormat, ply.Group.Name, "(" + userByName.Clase + ") ", ply.Name, " (Nivel " + userByName.Level + ") (" + userByName.ActiveTitle.Name + ")", Text);
			}
		}
		catch (Exception)
		{
		}
	}
}
