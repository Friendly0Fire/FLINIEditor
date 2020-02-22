using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace INIEditor
{
	public class PeekStreamReader : StreamReader
	{
		string next = null;
		int linecount;
		
		public PeekStreamReader(string file) : base(file)
		{
			linecount = 0;
		}
		
		public string PeekLine()
		{
			if(next == null)
				next = ReadLineCore();
			return next;
		}
		
		public override string ReadLine()
		{
			string ret;
			if(next != null)
			{
				ret = next;
				next = null;
			} else
				ret = ReadLineCore();
			
			return ret;
		}
		
		private string ReadLineCore()
		{
			linecount++;
			return base.ReadLine();
		}
		
		public int LineAt
		{ get { return linecount; } }
	}
}
