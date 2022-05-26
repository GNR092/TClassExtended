using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System;
using TShockAPI;

namespace TClassExtended
{
	/// <summary>
	/// Configuracion del plugin TClass.
	/// </summary>
	public class Config
	{
		public int[] ColorClassGuerrero = { 255, 0, 0 };
		public int[] ColorClassArquero = { 255, 255, 0 };
		public int[] ColorClassMago = { 76, 0, 153 };
		public int[] ColoClassNovato = { 255, 255, 255 };
		public List<NetTitle> SetTitles = new List<NetTitle>();
        public static string path { get { return Path.Combine(TShock.SavePath, "TClassConfig.json"); } }
		
		
		public static Config Read(string path)
		{
			return File.Exists(path) ? JsonConvert.DeserializeObject<Config>(File.ReadAllText(path)) : new Config();
		}

	}
}
