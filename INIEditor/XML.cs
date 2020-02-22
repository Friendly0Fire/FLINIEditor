using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace INIEditor
{
	public class XMLBlock
	{
		private string name;
		private List<XMLBlock> blocks;
		private List<XMLParameter> parameters;
		private string[] required;

		private List<XMLBlock> alternates;
		
		public XMLBlock(string name)
		{
			this.name = name.ToLower();
			blocks = new List<XMLBlock>();
			parameters = new List<XMLParameter>();
			alternates = new List<XMLBlock>();
			required = new string[0];
		}
		
		public void AddBlock(XMLBlock b)
		{
			blocks.Add(b);
		}

		public void AddParameter(XMLParameter p)
		{
			foreach(XMLParameter p2 in parameters)
			{
				if(p2.Name == p.Name)
				{
					p2.Add(p);
					
					return;
				}
			}
			
			parameters.Add(p);
		}

		public void Add(XMLBlock b)
		{
			if (alternates == null)
				alternates = new List<XMLBlock>();
			alternates.Add(b);
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		public List<XMLParameter> Parameters
		{
			get
			{
				return parameters;
			}
		}

		public List<XMLBlock> Blocks
		{
			get
			{
				return blocks;
			}
		}

		public List<XMLBlock> Alternates
		{
			get { return alternates; }
		}
		
		public void SetRequired(string values)
		{
			required = values.Split(',');
			for(int a = 0; a < required.Length; a++)
				required[a] = required[a].Trim().ToLower();
		}
		
		public string[] Required
		{ get { return required; } }
	}

	public class XMLParameter : IEquatable<XMLParameter>
	{
		private XMLValue[] values;
		private string name;
		private XBehavior behavior;
		
		private List<XMLParameter> alternates;

		public XMLParameter(string name, string behavior, List<XMLParamData> data)
		{
			this.name = name.ToLower();

			if (behavior == "") this.behavior = XBehavior.REQUIRED;
			else this.behavior = (XBehavior)Enum.Parse(this.behavior.GetType(), behavior.ToUpper());
			
			values = new XMLValue[data.Count];
			
			int a = 0;
			foreach(XMLParamData d in data)
			{
				values[a] = new XMLValue(d);
				a++;
			}
			
		}
		
		public void Add(XMLParameter p)
		{
			if(alternates == null)
				alternates = new List<XMLParameter>();
			alternates.Add(p);
		}

		public enum XBehavior
		{
			REQUIRED = 1,
			OPTIONAL = 2,
			REPEATABLE = 4
		}

		public string Name
		{
			get
			{
				return name;
			}
		}
		
		public List<XMLParameter> Alternates
		{
			get { return alternates; }
		}
		
		public XMLValue this[int index]
		{
			get
			{
				return values[index];
			}
		}
		
		public int Count
		{
			get
			{
				return values.Length;
			}
		}
		public XBehavior Behavior
		{
			get
			{
				return behavior;
			}
		}

		#region IEquatable<XMLParameter> Members

		public bool Equals(XMLParameter other)
		{
			return this.Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase);
		}

		#endregion
	}
	
	public struct XMLParamData
	{
		public string type;
		public string behaviors;

		public List<string> masksPattern;
		public List<string> masksRegex;
		public List<string> masksNumeric;
	}

	public class XMLValue
	{
		public XType Type;
		public XBehavior Behavior;
		private List<XMLValueMask> masks;
		private bool noMasks;
		public string RefBlockType;

		public static readonly char RefDelimiter = '/';
		public static readonly char MultiRefDelimiter = ',';
		public static readonly char BehaviorDelimiter = '|';
		public static readonly IFormatProvider FormatProvider = System.Globalization.NumberFormatInfo.InvariantInfo;

		public XMLValue(XMLParamData data)
		{
			if (data.type == null || data.type == "") Type = XType.UNDEFINED;
			else Type = (XType)Enum.Parse(this.Type.GetType(), data.type.ToUpper());

			if (data.behaviors == null || data.behaviors == "") Behavior = XBehavior.DEFAULT;
			else
			{
				string[] behaviordata = data.behaviors.Split(BehaviorDelimiter);
				for(int a = 0; a < behaviordata.Length; a++)
				{
					string[] refdata = behaviordata[a].Split(RefDelimiter);
					if (refdata.Length == 2)
					{
						RefBlockType = refdata[1].ToLower();
                        Behavior |= (XBehavior)Enum.Parse(this.Behavior.GetType(), refdata[0].ToUpper());
					} else
						Behavior |= (XBehavior)Enum.Parse(this.Behavior.GetType(), behaviordata[a].ToUpper());
				}
			}

			masks = new List<XMLValueMask>();
			foreach (string s in data.masksPattern)
				masks.Add(new XMLValueMaskPattern(s));
			foreach (string s in data.masksRegex)
				masks.Add(new XMLValueMaskRegex(s));
			foreach (string s in data.masksNumeric)
				masks.Add(new XMLValueMaskNumeric(s));
			
			if(masks.Count == 0) noMasks = true;
			else noMasks = false;
		}
		
		public bool Matches(string value)
		{
			List<ValidationError> d;
			return Matches(value, out d);
		}
		
		public bool Matches(string value, out List<ValidationError> errs)
		{
			errs = new List<ValidationError>();
			
			switch(Type)
			{
				case XType.FLOAT:
					float f;
					if (!float.TryParse(value, System.Globalization.NumberStyles.Float, FormatProvider, out f))
					{
						errs.Add(new ValidationError("Value '" + value + "' cannot be interpreted as a floating-point number.", ValidationError.XSeverity.ERROR, ValidationError.XType.PARSE_ERROR));
						return false;
					}
					break;
				case XType.INT:
					long i;
					if (!long.TryParse(value, System.Globalization.NumberStyles.Number, FormatProvider, out i))
					{
						errs.Add(new ValidationError("Value '" + value + "' cannot be interpreted as an integer.", ValidationError.XSeverity.ERROR, ValidationError.XType.PARSE_ERROR));
						return false;
					}
					break;
				case XType.PATH:
					if (!System.IO.File.Exists(System.IO.Path.Combine(Program.DataPath, value))
						&& !System.IO.Directory.Exists(System.IO.Path.Combine(Program.DataPath, value)))
					{
						errs.Add(new ValidationError("Value '" + value + "' is not a valid path.", ValidationError.XSeverity.ERROR, ValidationError.XType.PATH_ERROR));
						return false;
					}
					break;
				case XType.BOOLEAN:
					value = value.ToLower();
					if(value != "true" && value != "false")
					{
						errs.Add(new ValidationError("Value '" + value + "' is not a boolean (true|false).", ValidationError.XSeverity.ERROR, ValidationError.XType.PARSE_ERROR));
						return false;
					}
					break;
			}
			
			if(noMasks) return true;
			
			bool validated = false;
			
			foreach(XMLValueMask m in masks)
			{
				ValidationError err = null;
				validated = validated || m.Matches(value, out err);
				if(err != null) errs.Add(err);
			}
			
			if(validated) errs.Clear();

			return validated;
		}

		public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (XBehavior x in Enum.GetValues(typeof(XBehavior)))
            {
                if ((this.Behavior & x) == x)
                    sb.Append(x.ToString() + "|");
            }
            sb.Append(";");
            foreach (XMLValueMask m in masks)
                sb.Append(m.ToString() + "|");
			
			return this.Type + ";" + sb.ToString();
		}

		public enum XBehavior
		{
			ID = 1,
            PARTID = 64,
			REF = 2,
            REF2 = 32,
			NAME = 4,
			INFO = 8,
			REPEATABLE = 16,
			DEFAULT = 0
		}

		public enum XType
		{
			INT = 1,
			FLOAT = 2,
			STRING = 4,
			PATH = 8,
			BOOLEAN = 16,
			UNDEFINED = 0
		}
	}
	
	public class XMLValueMask
	{
		public virtual bool Matches(string value, out ValidationError err)
		{
			err = null;
			return true;
		}

		public override string ToString()
		{
			return "none";
		}
	}

	public class XMLValueMaskRegex : XMLValueMask
	{
		private Regex reg;
		private string pattern;

		public XMLValueMaskRegex(string pattern)
		{
			reg = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			this.pattern = pattern;
		}

		public override bool Matches(string value, out ValidationError err)
		{
			err = null;
			bool r = reg.IsMatch(value);
			if (!r) err = new ValidationError("Value '" + value + "' does not match regular expression '" + pattern + "'.", ValidationError.XSeverity.ERROR, ValidationError.XType.MASK_ERROR);
			return r;
		}

		public override string ToString()
		{
			return pattern;
		}
	}

	public class XMLValueMaskPattern : XMLValueMask
	{
		private Regex reg;
		private string pattern;

		public XMLValueMaskPattern(string pattern)
		{
			reg = new Regex("^" + pattern.Replace("*", "(.*)") + "$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			this.pattern = pattern;
		}

		public override bool Matches(string value, out ValidationError err)
		{
			err = null;
			bool r = reg.IsMatch(value);
			if (!r) err = new ValidationError("Value '" + value + "' does not match pattern '" + pattern + "'.", ValidationError.XSeverity.ERROR, ValidationError.XType.MASK_ERROR);
			return r;
		}

		public override string ToString()
		{
			return pattern;
		}
	}
	
	public class XMLValueMaskNumeric : XMLValueMask
	{
		double max;
		double min;
		bool mine, maxe;

		private Regex reg = new Regex(@"((?<mine>\[)|(\]))((?<min>-?[0-9]+(\.[0-9]+)?)|(?<mini>-inf)),\s*((?<max>-?[0-9]+(\.[0-9]+)?)|(?<maxi>inf))((?<maxe>\])|(\[))", RegexOptions.Compiled);
		
		public XMLValueMaskNumeric(string s)
		{
			Match m = reg.Match(s);
			
			if (m.Groups["mine"].Success) mine = true;
			if (m.Groups["maxe"].Success) maxe = true;
			
			if(m.Groups["mini"].Success) min = Double.MinValue;
			else min = Double.Parse(m.Groups["min"].Value, System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowDecimalPoint, XMLValue.FormatProvider);
			if (m.Groups["maxi"].Success) max = Double.MaxValue;
			else max = Double.Parse(m.Groups["max"].Value, System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowDecimalPoint, XMLValue.FormatProvider);
		}

		public override bool Matches(string value, out ValidationError err)
		{
			err = null;
			double v;
			if (!Double.TryParse(value, System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowDecimalPoint, XMLValue.FormatProvider, out v))
			{
				err = new ValidationError("Value '" + value + "' cannot be interpreted as a number.", ValidationError.XSeverity.ERROR, ValidationError.XType.PARSE_ERROR);
				return false;
			}
			bool r = ((mine && min <= v) || (!mine && min < v)) && ((maxe && max >= v) || (!maxe && max > v));
			if (!r) err = new ValidationError("Value '" + value + "' does not fit interval " + ToString() + ".", ValidationError.XSeverity.ERROR, ValidationError.XType.MASK_ERROR);
			return r;
		}

		public override string ToString()
		{
			string mins = this.min == Double.MinValue ? "-inf" : this.min.ToString();
			string maxs = this.max == Double.MaxValue ? "inf" : this.max.ToString();
			return (mine ? "[" : "]") + mins + "," + maxs + (maxe ? "]" : "[");
		}
	}
	
	public class ValidationError : IComparable<ValidationError>
	{
		public readonly string ErrorMessage;
		public readonly XSeverity Severity;
		public readonly XType Type;
		
		public ValidationError(string msg, XSeverity svr, XType type)
		{
			ErrorMessage = msg;
			Severity = svr;
			Type = type;
		}
		
		public enum XSeverity
		{
			NOTICE = 0,
			WARNING = 1,
			ERROR = 2
		}
		
		public enum XType
		{
			PARSE_ERROR = 1,
			PATH_ERROR = 2,
			MASK_ERROR = 4,
			CONFLICT_ERROR = 8,
			BAD_REFERENCE_ERROR = 16,
			MISSING_ERROR = 32
		}
		
		public string ErrorText
		{
			get
			{
				return ErrorMessage;
			}
		}
		
		public string SeverityText
		{
			get
			{
				return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Severity.ToString());
			}
		}

		#region IComparable<ValidationError> Members

		public int CompareTo(ValidationError other)
		{
			return ErrorMessage.CompareTo(other.ErrorMessage);
		}

		#endregion
	}
}
