using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace INIEditor
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
		public static string DataPath
		{
			get
			{
				return Properties.Settings.Default["DataPath"] as string;
			}
			set
			{
				Properties.Settings.Default["DataPath"] = value;
			}
		}
		
		public static string DefinitionsPath
		{
			get
			{
				return Properties.Settings.Default["DefinitionsPath"] as string;
			}
			set
			{
				Properties.Settings.Default["DefinitionsPath"] = value;
			}
		}
	}
}
