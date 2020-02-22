using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace INIEditor
{
	class INIParser
	{
		List<INIBlock> blocks;
		Dictionary<string, INIBlock> refMap;
		
		private string datapath;
        private FileSystemWatcher fsw;
		
		public INIParser(string folder)
		{
			datapath = folder;
			
			blocks = new List<INIBlock>();
			refMap = new Dictionary<string, INIBlock>();
			
			ParseFolder(datapath);

            fsw = new FileSystemWatcher(datapath, "*.ini");
            fsw.IncludeSubdirectories = true;
            fsw.Changed += new FileSystemEventHandler(fsw_Changed);
            fsw.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            fsw.EnableRaisingEvents = true;
		}

        public event EventHandler INIChanged, INIChangedStart;

        System.Threading.Timer fswTimer;

        void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            /*List<INIBlock> removedBlocks = new List<INIBlock>();

            blocks.RemoveAll(
                new Predicate<INIBlock>(
                    (INIBlock b) =>
                    {
                        if (b.Location == e.FullPath)
                        {
                            removedBlocks.Add(b);
                            return true;
                        }
                        else
                            return false;
                    }
                    ));

            List<string> keys = new List<string>();

            foreach (KeyValuePair<string, INIBlock> kvp in refMap)
            {
                if (removedBlocks.Contains(kvp.Value))
                    keys.Add(kvp.Key);
            }

            foreach (string s in keys)
                refMap.Remove(s);

            ParseFile(e.FullPath);*/

            if (fswTimer != null)
                return;

            fswTimer = new System.Threading.Timer(fsw_Execute, null, 500, System.Threading.Timeout.Infinite);
        }

        private void fsw_Execute(object o)
        {
            fswTimer = null;

            if (INIChangedStart != null)
                INIChangedStart(this, new EventArgs());

            blocks = new List<INIBlock>();
            refMap = new Dictionary<string, INIBlock>();

            ParseFolder(datapath);

            if (INIChanged != null)
                INIChanged(this, new EventArgs());
        }
		
		private void ParseFolder(string foldername)
		{
			if (!Directory.Exists(foldername))
				throw new IOException("Directory " + foldername + " does not exist. Did the filesystem change during parsing?");

			string[] files = Directory.GetFiles(foldername, "*.ini");
			foreach (string f in files)
				ParseFile(f);

			string[] folders = Directory.GetDirectories(foldername);
			foreach (string f in folders)
				ParseFolder(f);
		}

		private void ParseFile(string filename)
		{
			if (!File.Exists(filename))
				throw new IOException("File " + filename + " does not exist. Did the filesystem change during parsing?");

			PeekStreamReader sr = new PeekStreamReader(filename);
			while (true)
			{
				INIBlock i = INIBlock.Create(sr, filename);
				if (i != null)
					blocks.Add(i);
				else break;
			}
			sr.Close();
		}

		private void AddToRefMap(INIBlock b)
		{
			if (b.Name == "<none>" || b.Name == null) return;
			// Add boolean instead?

			string key = b.Type.ToLower() + XMLValue.RefDelimiter + b.Name.ToLower();
			if (!refMap.ContainsKey(key))
			{
				refMap.Add(key, b);
			}
			else if(!b.Partial && !refMap[key].Partial)
			{
				INIBlock b2 = refMap[key];
				if(b.Parent != null || b2.Parent != null && b.Parent != b2.Parent) return;
				
				bool alreadyAdded = false;
				foreach (ValidationError e in b2.Errors)
				{
					if (e.Type == ValidationError.XType.CONFLICT_ERROR)
					{
						alreadyAdded = true;
						break;
					}
				}
				INIValue v1 = b.NameValue;
				INIValue v2 = b2.NameValue;

				v1.Errors.Add(new ValidationError("Block '" + b.Name + "' of type '" + b.Type + "' has a name conflict.", ValidationError.XSeverity.ERROR, ValidationError.XType.CONFLICT_ERROR));
				v1.Revalidate();

				if (!alreadyAdded)
				{
					v2.Errors.Add(new ValidationError("Block '" + b2.Name + "' of type '" + b2.Type + "' has a name conflict.", ValidationError.XSeverity.ERROR, ValidationError.XType.CONFLICT_ERROR));
					v2.Revalidate();
				}
			}
		}

		public void MatchBlocks(XMLParser xParser)
		{
			List<INIBlock> removals = new List<INIBlock>();

			INIBlock current = null;
			foreach (INIBlock b in blocks)
			{
				if(b.Matched) continue;
				
				foreach (XMLBlock x in xParser.Blocks)
				{
					if (current != null)
					{
						if (!current.MatchNext(b, xParser.Parameters))
							current = null;
						else
						{
							removals.Add(b);
							AddToRefMap(b);
						}
					}
					if (current == null && b.Match(x, xParser.Parameters))
					{
						current = b;
						AddToRefMap(b);
					}
					
					if(b.Matched) break;
				}
				
				if (!b.Matched)
				{
					b.MatchAssumptions(xParser.Parameters);
					AddToRefMap(b);
				}
			}

			for (int a = 0; a < blocks.Count; a++)
			{
				INIBlock b = blocks[a];

                List<INIValue> refs = b.GetReferenceValues();
                List<INIValue> refs2 = b.GetReference2Values();
                foreach (INIValue v in refs)
                {
                    string[] rs = v.Xml.RefBlockType.Split(XMLValue.MultiRefDelimiter);
                    bool hasOneKey = false;
                    INIBlock matchedBlock = null;
                    foreach (string type in rs)
                    {
                        string key = type + XMLValue.RefDelimiter + v.Value.ToLower();
                        if (refMap.ContainsKey(key))
                        {
                            matchedBlock = refMap[key];

                            b.References.Add(matchedBlock);
                            if (refs2.Contains(v)) b.Referenced.Add(matchedBlock);
                            matchedBlock.Referenced.Add(b);

                            hasOneKey = true;
                        }
                    }

                    if (!hasOneKey || !matchedBlock.Exists)
                    {
                        v.Errors.Add(new ValidationError("Value " + v.Offset + ", '" + v.Value + "', of parameter '" + v.Parent.Name + "' references to a non-existent entity of type(s) '" + v.Xml.RefBlockType.Replace(",", "' or '") + "'.", ValidationError.XSeverity.ERROR, ValidationError.XType.BAD_REFERENCE_ERROR));
                        v.Revalidate();

                        if (matchedBlock == null)
                        {
                            INIBlock fb = new INIBlock(v.Value);
                            fb.References.Add(b);
                            if (refs2.Contains(v)) b.Referenced.Add(fb);
                            b.References.Add(fb);

                            blocks.Add(fb);
                        }
                    }
                }
				
				if (removals.Contains(b))
				{
					removals.Remove(b);
					blocks.Remove(b);
					a--;
				}
			}
		}

		public List<INIBlock> Blocks
		{
			get
			{
				return blocks;
			}
		}
	}
}
