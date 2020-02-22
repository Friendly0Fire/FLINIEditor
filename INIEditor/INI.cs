using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Threading;

namespace INIEditor
{
	
	public class INIBlock : IComparable<INIBlock>, IEquatable<INIBlock>, BindingListView<INIBlock>.IFilterable
	{
		static Regex header = new Regex(@"^\s*\[(?<name>[^\]]+)\](\s*;.*)?$", RegexOptions.Compiled);
		private static int counter = 0;
		
		public static INIBlock Create(PeekStreamReader sr, string path)
		{
			string str = "";
			bool flag = false;
			List<string> lines = new List<string>();
			int line = sr.LineAt;
			while(true)
			{
				str = sr.PeekLine();
				if(str == null) break;
				
				if(header.IsMatch(str))
				{
					if(!flag)
						flag = true;
					else
						break;
				}
				if(flag)
					lines.Add(str);

				sr.ReadLine();
			}
			
			if(lines.Count == 0) return null;
			
			return new INIBlock(lines.ToArray<string>(), path, line);
		}

		private string type;
		private List<INIParameter> parameters;
		private HashSet<XMLParameter> xparameters;
		private List<INIBlock> blocks;
		private XMLBlock xml;
		
		private INIBlock parent;
		
		private bool valid;
		private HashSet<INIBlock> references;
		private HashSet<INIBlock> referenced;

		private List<ValidationError> errors;
		
		private int line, id;
		
		private string path;

        private readonly bool exists;

        public INIBlock(string name)
        {
            exists = false;

            path = "<none>";

            id = -1;

            line = -1;

            type = "<unknown>";

            parameters = new List<INIParameter>();
            xparameters = new HashSet<XMLParameter>();
            blocks = new List<INIBlock>();

            valid = false;
            references = new HashSet<INIBlock>();
            referenced = new HashSet<INIBlock>();

            errors = new List<ValidationError>();

            errors.Add(new ValidationError("This block does not exist but is referenced elsewhere.", ValidationError.XSeverity.ERROR, ValidationError.XType.BAD_REFERENCE_ERROR));

            nameValue = new INIValue(name, 0, null);
        }
		
		private INIBlock(string[] lines, string pt, int l)
		{
            exists = true;

			line = l;
			id = counter++;
			
			type = header.Match(lines[0]).Groups["name"].Value;
			
			path = pt;

			parameters = new List<INIParameter>();
			xparameters = new HashSet<XMLParameter>();
			blocks = new List<INIBlock>();

			valid = true;
			references = new HashSet<INIBlock>();
			referenced = new HashSet<INIBlock>();
			
			errors = new List<ValidationError>();
			
			for(int a = 1; a < lines.Length; a++)
			{
				INIParameter p = INIParameter.Create(lines[a], this);
				if(p != null)
					parameters.Add(p);
			}
		}

		public override string ToString()
		{
			string output = "BLOCK " + type + (Matched ? "(matched)" : "") + Environment.NewLine;
			foreach(INIParameter p in parameters)
				output += p + Environment.NewLine;
			
			if(blocks.Count > 0)
			{
				output += Environment.NewLine + "SUB-BLOCKS-----------" + Environment.NewLine + Environment.NewLine;
				foreach(INIBlock b in blocks)
				{
					output += b + Environment.NewLine;
				}
				output += "END SUB-BLOCKS-------" + Environment.NewLine;
			}
			
			return output;
		}

		public bool Match(XMLBlock x, List<XMLParameter> xassums)
		{
			if (exists == false || xml != null || x.Name != this.type.ToLower())
				return false;

			xml = x;

			List<ValidationError> localErrors = new List<ValidationError>();
			List<XMLParameter> localXParameters = new List<XMLParameter>();
			if (!ParseAlternate(x, xassums, localErrors, localXParameters))
			{
				if(x.Alternates.Count > 0)
                {
					bool found = false;
					foreach(XMLBlock x2 in x.Alternates)
					{
                        List<ValidationError> localErrors2 = new List<ValidationError>();
                        List<XMLParameter> localXParameters2 = new List<XMLParameter>();

                        bool altMatch = ParseAlternate(x2, xassums, localErrors2, localXParameters2);

                        if (altMatch || localErrors2.Count < localErrors.Count)
                        {
                            xml = x2;
                            localErrors = localErrors2;
                            localXParameters = localXParameters2;
                            found = altMatch;

                            if (found) break;
                        }
					}
					if(!found)
					{
						valid = false;
						errors.AddRange(localErrors);
					}
				}
				else
				{
					errors.AddRange(localErrors);
					valid = false;
				}
			}
			
			foreach(XMLParameter p in localXParameters)
				if(!xparameters.Contains(p)) xparameters.Add(p);

			return true;
		}

		private bool ParseAlternate(XMLBlock x, List<XMLParameter> xassums, List<ValidationError> localErrors, List<XMLParameter> localXParameters)
		{
            if (!exists)
                return false;

			bool currValid = true;
			localXParameters.Clear();
			localErrors.Clear();
			
			foreach (INIParameter p in parameters)
			{
				p.Clear();
				
				foreach (XMLParameter xp in x.Parameters)
				{
					if (p.Match(xp))
					{
						if (!localXParameters.Contains(xp))
							localXParameters.Add(xp);
						else if ((xp.Behavior & XMLParameter.XBehavior.REPEATABLE) != XMLParameter.XBehavior.REPEATABLE)
							p.Errors.Add(new ValidationError("Parameter '" + p.Name + "' is already defined.", ValidationError.XSeverity.WARNING, ValidationError.XType.CONFLICT_ERROR));
						if (p.Errors != null) localErrors.AddRange(p.Errors);
					}
				}
				if (xassums != null && !p.Matched)
				{
					foreach (XMLParameter xp in xassums)
					{
						if (p.Match(xp))
						{
							if (!localXParameters.Contains(xp))
								localXParameters.Add(xp);
							else if ((xp.Behavior & XMLParameter.XBehavior.REPEATABLE) != XMLParameter.XBehavior.REPEATABLE)
								p.Errors.Add(new ValidationError("Parameter '" + p.Name + "' is already defined.", ValidationError.XSeverity.WARNING, ValidationError.XType.CONFLICT_ERROR));
							if (p.Errors != null) localErrors.AddRange(p.Errors);
						}
					}
				}

				currValid = currValid && p.Valid;
			}

			foreach (XMLParameter xp in x.Parameters)
			{
				if ((xp.Behavior & XMLParameter.XBehavior.REQUIRED) == XMLParameter.XBehavior.REQUIRED && !localXParameters.Contains(xp))
				{
					localErrors.Add(new ValidationError("Parameter '" + xp.Name + "' is missing.", ValidationError.XSeverity.ERROR, ValidationError.XType.MISSING_ERROR));
					currValid = false;
				}
			}

			foreach (XMLParameter xp in xassums)
			{
				if (x.Required.Contains(xp.Name) && !localXParameters.Contains(xp))
				{
					localErrors.Add(new ValidationError("Parameter '" + xp.Name + "' is missing.", ValidationError.XSeverity.ERROR, ValidationError.XType.MISSING_ERROR));
					currValid = false;
				}
			}
			
			return currValid;
		}

		public void MatchAssumptions(List<XMLParameter> xassums)
		{
            if (!exists)
                return;

			foreach (INIParameter p in parameters)
			{
				foreach (XMLParameter xp in xassums)
				{
					if (p.Match(xp))
					{
						if (!xparameters.Contains(xp))
							xparameters.Add(xp);
						else if ((xp.Behavior & XMLParameter.XBehavior.REPEATABLE) != XMLParameter.XBehavior.REPEATABLE)
							p.Errors.Add(new ValidationError("Parameter '" + p.Name + "' is already defined.", ValidationError.XSeverity.WARNING, ValidationError.XType.CONFLICT_ERROR));
						if(p.Errors != null) errors.AddRange(p.Errors);
					}
				}

				valid = valid && p.Valid;
			}
		}

		public bool MatchNext(INIBlock b, List<XMLParameter> xassums)
        {
            if (!exists)
                return false;

			foreach(XMLBlock x in xml.Blocks)
			{
				if (b.Match(x, xassums))
				{
					blocks.Add(b);
					b.Parent = this;
					return true;
				}
			}
			
			return false;
		}
		
		public bool Matched
		{
			get
			{
				return xml != null;
			}
		}

        public bool Exists
        {
            get
            {
                return exists;
            }
        }
		
		public INIBlock Parent
		{
			get
			{
				return parent;
			}
			
			set
			{
				if(parent == null)
				parent = value;
			}
		}

		public string Type
		{
			get
			{
				return type;
			}
		}
		
		private INIValue nameValue;

		public string Name
		{
			get
			{
				INIValue v = NameValue;
				if(v == null) return "<none>";
				else return v.Value;
			}
        }

        public uint Hash
        {
            get
            {
                INIValue v = NameValue;
                if (v == null) return 0;
                else return v.Hash;
            }
        }

        public uint Crc
        {
            get
            {
                INIValue v = NameValue;
                if (v == null) return 0;
                else return v.Crc;
            }
        }

		public int Line
		{
			get
			{
				return line;
			}
		}

		public int ID
		{
			get
			{
				return id;
			}
		}
		
		public INIValue NameValue
		{
			get
			{
				if(nameValue != null) return nameValue;
				
				foreach (INIParameter p in parameters)
				{
					foreach (INIValue v in p)
                        if (v.Xml != null && ((v.Xml.Behavior & XMLValue.XBehavior.ID) == XMLValue.XBehavior.ID || (v.Xml.Behavior & XMLValue.XBehavior.PARTID) == XMLValue.XBehavior.PARTID))
						{
							nameValue = v;
							return v;
						}
				}
				
				return null;
			}
		}

        public bool Partial
        {
            get
            {
                return (nameValue.Xml != null && (nameValue.Xml.Behavior & XMLValue.XBehavior.PARTID) == XMLValue.XBehavior.PARTID);
            }
        }
		
		public bool Valid
		{
			get
			{
				return valid;
			}
		}
		
		public void Revalidate()
		{
			if(errors.Count > 0)
				valid = false;
		}

		public HashSet<INIBlock> References
		{
			get
			{
				return references;
			}
		}

		public HashSet<INIBlock> Referenced
		{
			get
			{
				return referenced;
			}
		}
		
		public List<INIBlock> Blocks
		{
			get
			{
				return blocks;
			}
		}

		public int ReferencesCount
		{
			get
			{
				return references.Count;
			}
		}

		public int ReferencedCount
		{
			get
			{
				return referenced.Count;
			}
		}
		
		public List<INIParameter> Parameters
		{
			get
			{
				return parameters;
			}
		}

		public List<ValidationError> Errors
		{
			get
			{
				return errors;
			}
		}

		public List<INIValue> GetReferenceValues()
        {

            List<INIValue> refs = new List<INIValue>();
            if (!exists)
                return refs;

			foreach(INIParameter p in parameters)
			{
				foreach(INIValue v in p)
				{
                    if (v.Xml == null) continue;

                    if ((v.Xml.Behavior & XMLValue.XBehavior.REF) == XMLValue.XBehavior.REF
                        || (v.Xml.Behavior & XMLValue.XBehavior.REF2) == XMLValue.XBehavior.REF2)
                        refs.Add(v);
				}
			}
			
			return refs;
        }

        public List<INIValue> GetReference2Values()
        {
            List<INIValue> refs = new List<INIValue>();
            if (!exists)
                return refs;

            foreach (INIParameter p in parameters)
            {
                foreach (INIValue v in p)
                {
                    if (v.Xml == null) continue;

                    if ((v.Xml.Behavior & XMLValue.XBehavior.REF2) == XMLValue.XBehavior.REF2)
                        refs.Add(v);
                }
            }

            return refs;
        }
		
		public string Location
		{
			get
			{
				return path;
			}
		}

		#region IComparable<INIBlock> Members

		public int CompareTo(INIBlock other)
		{
			if(this.Type.CompareTo(other.Type) != 0)
				return this.Type.CompareTo(other.Type);
			
			return this.Name.CompareTo(other.Name);
		}

		#endregion

		#region IEquatable<INIBlock> Members

		public bool Equals(INIBlock other)
		{
			return this.ID == other.ID;
		}

		#endregion

		#region IFilterable Members

		public Dictionary<string, string> GetFilterDictionary(List<string> values)
		{
			Dictionary<string, string> d = new Dictionary<string, string>();
			foreach(string s in values)
			{
				if(d.ContainsKey(s)) continue;

                if (s == "name") d.Add("name", Name);
                else if (s == "hash") d.Add("hash", Hash.ToString());
                else if (s == "crc") d.Add("crc", Crc.ToString());
                else if (s == "type") d.Add("type", Type);
                else if (s == "matched") d.Add("matched", Matched.ToString());
                else if (s == "valid") d.Add("valid", Valid.ToString());
                else if (s == "references") d.Add("references", ReferencesCount.ToString());
                else if (s == "referenced") d.Add("referenced", ReferencedCount.ToString());
                else if (s == "file") d.Add("file", Location);
                else
                {
                    foreach (INIParameter p in parameters)
                    {
                        if (p.Name == s)
                            d.Add(s, p[0].Value);
                    }
                }
			}
			
			return d;
		}

		#endregion
	}

	public class INIParameter : IEnumerable<INIValue>, IComparable<INIParameter>
	{
		static Regex parameter = new Regex(@"^\s*(?<name>[a-zA-Z0-9_\-\. ]+)\s*(=\s*(?<value>[a-zA-Z0-9_\- \.\\,""#]+))?(\s*;.*)?$", RegexOptions.Compiled);

		readonly private string name;
		readonly private INIValue[] values;
		private XMLParameter xml;

		private INIBlock parent;
		
		private bool valid;
		
		private List<ValidationError> errors;
		
		public static INIParameter Create(string line, INIBlock parent)
		{
			Match m = parameter.Match(line);
			string name = m.Groups["name"].Value;
			if (name == "") return null;
			string vals = m.Groups["value"].Value;
			
			return new INIParameter(name, vals, parent);
		}

		private INIParameter(string name, string vals, INIBlock parent)
		{
			this.name = name.Trim();
			this.parent = parent;

			errors = new List<ValidationError>();
			
			valid = true;
			
			string[] valsArray = vals.Split(',');
			values = new INIValue[valsArray.Length];
			
			for (int a = 0; a < valsArray.Length; a++)
				values[a] = new INIValue(valsArray[a].Trim(), a+1, this);
		}

		public override string ToString()
		{
			string output = "\"" + name + "\" (" + (xml == null ? XMLParameter.XBehavior.OPTIONAL : xml.Behavior) + ") -> {";
			foreach(INIValue v in values)
				output += "[" + v + "];";
			output = output.Substring(0, output.Length-1) + "}";
			
			return output;
		}
		
		public bool Match(XMLParameter x)
		{
			if (xml != null || x.Name != name.ToLower())
				return false;
			
			xml = x;

			List<ValidationError> localErrors = new List<ValidationError>();
			if(ParseAlternate(x, localErrors))
				return true;
			
			if(x.Alternates != null)
			{
				foreach(XMLParameter x2 in x.Alternates)
				{
					localErrors.Clear();
					if (ParseAlternate(x2, localErrors))
					{
						xml = x2;
						return true;
					}
				}
			}

			valid = false;
			if(x.Alternates != null) xml = x.Alternates[x.Alternates.Count-1];
			errors.AddRange(localErrors);
			
			return true;
		}
		
		private bool ParseAlternate(XMLParameter x, List<ValidationError> localErrors)
		{
			bool currValid = true;
			
			int off = 0;
			for (int a = 0; a < values.Length; a++)
			{
				if(a-off >= x.Count)
				{
					localErrors.Add(new ValidationError("Parameter '" + this.Name + "' has too many values.", ValidationError.XSeverity.ERROR, ValidationError.XType.MISSING_ERROR));
					return false;
				}
				values[a].Xml = x[a - off];
				if(values[a].Valid && (x[a - off].Behavior & XMLValue.XBehavior.REPEATABLE) == XMLValue.XBehavior.REPEATABLE)
					off++;
				else if(!values[a].Valid && off > 0)
				{
					off--;
					a--;
					continue;
				}

				currValid = currValid && values[a].Valid;
				localErrors.AddRange(values[a].Errors);
			}
			foreach(INIValue v in values)
			{
				if(v.Xml == null)
				{
					localErrors.Add(new ValidationError("Parameter '" + this.Name + "' has missing values.", ValidationError.XSeverity.ERROR, ValidationError.XType.MISSING_ERROR));
					currValid = false;
				}
			}
			
			return currValid;
		}
		
		public void Clear()
		{
			xml = null;
			valid = true;
			errors.Clear();
		}

		public bool Matched
		{
			get
			{
				return xml != null;
			}
		}
		
		public string Name
		{
			get
			{
				return name;
			}
		}

		public INIBlock Parent
		{
			get
			{
				return parent;
			}

			set
			{
				if (parent == null)
					parent = value;
			}
		}
		
		public XMLParameter.XBehavior Behavior
		{
			get
			{
				if(xml != null)
					return xml.Behavior;
				else
					return XMLParameter.XBehavior.OPTIONAL;
			}
		}
		
		public INIValue this[int index]
		{
			get
			{
				return values[index];
			}
		}

		public string Values
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				foreach (INIValue v in values)
				{
					sb.Append(v.Value);
					sb.Append(" / ");
				}
				return sb.ToString();
			}
		}

		public string XValues
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				foreach (INIValue v in values)
				{
					if(v.Xml == null) sb.Append("no match");
					else sb.Append(v.Xml.ToString());
					sb.Append(" / ");
				}
				return sb.ToString();
			}
		}
		
		public List<ValidationError> Errors
		{
			get
			{
				return errors;
			}
		}

		#region IEnumerable<INIValue> Members

		public IEnumerator<INIValue> GetEnumerator()
		{
			return ((IEnumerable<INIValue>) values).GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return values.GetEnumerator();
		}

		#endregion

		public bool Valid
		{
			get
			{
				return valid;
			}
		}

		public void Revalidate()
		{
			if (errors.Count > 0)
				valid = false;
			foreach(ValidationError e in Errors)
			{
				if(!Parent.Errors.Contains(e))
					Parent.Errors.Add(e);
			}
			
			Parent.Revalidate();
		}

		#region IComparable<INIParameter> Members

		public int CompareTo(INIParameter other)
		{
			return this.Name.CompareTo(other.Name);
		}

		#endregion
	}

	public class INIValue : IComparable<INIValue>
	{
		public readonly string Value;

        public readonly uint Hash, Crc;

		public readonly int Offset;
		
		private XMLValue xml;

		public readonly INIParameter Parent;
		
		public List<ValidationError> Errors;

        private bool valid;
		
		public INIValue(string v, int offset, INIParameter parent)
		{
			Value = v;

            Hash = CRC.GetInstance().ComputeHash(v);
            Crc = CRC.GetInstance().ComputeCRC(v);
			
			Offset = offset;
			
			valid = true;
			
			Parent = parent;
			
			Errors = new List<ValidationError>();
		}

		public override string ToString()
		{
			string output = Value;
			output += Xml.ToString();
			
			return output;
		}
		
		public XMLValue Xml
		{
			get
			{
				return xml;
			}
			set
			{
				xml = value;
				Validate();
			}
		}

		public bool Valid
		{
			get
			{
				return valid;
			}
		}

		public void Revalidate()
		{
			if (Errors.Count > 0)
				valid = false;
			foreach (ValidationError e in Errors)
			{
				if (!Parent.Errors.Contains(e))
					Parent.Errors.Add(e);
			}

			Parent.Revalidate();
		}
		
		private void Validate()
		{
			Errors.Clear();
			valid = xml.Matches(Value, out Errors);
		}

		#region IComparable<INIValue> Members

		public int CompareTo(INIValue other)
		{
			return this.Value.CompareTo(other.Value);
		}

		#endregion
	}
	
	public class INIComparer : IComparer<INIBlock>
	{
		private PropertyDescriptor property;
		private ListSortDirection direction;
		
		public INIComparer(PropertyDescriptor p, ListSortDirection d)
		{
			property = p;
			direction = d;
		}
		
		public int Compare(INIBlock x, INIBlock y)
		{
			int dir = direction == ListSortDirection.Ascending ? 1 : -1;
			int res = 0;
			
			switch(property.Name)
			{
				case "Type":
					res = x.Type.CompareTo(y.Type);
					if(res != 0) break;
					res = x.Name.CompareTo(y.Name);
					if (res != 0) break;
					res = x.Valid.CompareTo(y.Valid);
					break;
				case "Name":
					res = x.Name.CompareTo(y.Name);
					if (res != 0) break;
					res = x.Type.CompareTo(y.Type);
					if (res != 0) break;
					res = x.Valid.CompareTo(y.Valid);
					break;
				case "Valid":
					res = x.Valid.CompareTo(y.Valid);
					if (res != 0) break;
					res = x.Type.CompareTo(y.Type);
					if (res != 0) break;
					res = x.Name.CompareTo(y.Name);
					break;
				case "Matched":
					res = x.Matched.CompareTo(y.Matched);
					if (res != 0) break;
					res = x.Type.CompareTo(y.Type);
					if (res != 0) break;
					res = x.Name.CompareTo(y.Name);
					if (res != 0) break;
					res = x.Valid.CompareTo(y.Valid);
					break;
				case "ReferencesCount":
					res = x.ReferencesCount.CompareTo(y.ReferencesCount);
					if (res != 0) break;
					res = x.Type.CompareTo(y.Type);
					if (res != 0) break;
					res = x.Name.CompareTo(y.Name);
					if (res != 0) break;
					res = x.Valid.CompareTo(y.Valid);
					break;
				case "ReferencedCount":
					res = x.ReferencedCount.CompareTo(y.ReferencedCount);
					if (res != 0) break;
					res = x.Type.CompareTo(y.Type);
					if (res != 0) break;
					res = x.Name.CompareTo(y.Name);
					if (res != 0) break;
					res = x.Valid.CompareTo(y.Valid);
					break;
				case "Location":
				case "Line":
					res = x.Location.CompareTo(y.Location);
					if (res != 0) break;
					res = x.Line.CompareTo(y.Line);
					if (res != 0) break;
					res = x.Type.CompareTo(y.Type);
					if (res != 0) break;
					res = x.Name.CompareTo(y.Name);
					if (res != 0) break;
					res = x.Valid.CompareTo(y.Valid);
					break;
			}
			
			return res * dir;
		}
	}
}
