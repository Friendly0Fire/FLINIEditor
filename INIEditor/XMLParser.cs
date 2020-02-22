using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace INIEditor
{
	public class XMLParser
	{
		private List<XMLBlock> xblocks;
		private List<XMLParameter> xparams;

		//List<string> references;
		
		private string filepath;

		public XMLParser(string file)
		{
			filepath = file;
			//references = new List<string>();

			XmlReaderSettings readerSettings = new XmlReaderSettings();
			readerSettings.IgnoreWhitespace = true;
			readerSettings.IgnoreComments = true;
			readerSettings.CheckCharacters = true;
			readerSettings.CloseInput = true;
			readerSettings.IgnoreProcessingInstructions = false;
			readerSettings.ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.None;
			readerSettings.ValidationType = ValidationType.None;

			XmlDocument xd = new XmlDocument();
			xd.Load(XmlReader.Create(new FileStream(filepath, FileMode.Open), readerSettings));


			XmlNode root = xd.ChildNodes[1];
			XmlNode defs = null/*, refs = null*/, assums = null;
			foreach (XmlNode n in root.ChildNodes)
			{
				if (n.Name == "definitions") defs = n;
				//if (n.Name == "references") refs = n;
				if (n.Name == "assumptions") assums = n;
			}

			if (assums != null)
			{
				xparams = new List<XMLParameter>();
				foreach (XmlNode n in assums.ChildNodes)
				{
					xparams.Add(ParseParameterXml(n));
				}
			}

			if (defs != null)
			{
				xblocks = new List<XMLBlock>();
				foreach (XmlNode n in defs.ChildNodes)
				{
					bool found = false;
					XMLBlock b = ParseBlockXml(n);
					foreach(XMLBlock b2 in xblocks)
					{
						if(b2.Name == b.Name)
						{
							b2.Alternates.Add(b);
							found = true;
							break;
						}
					}
					if(!found) xblocks.Add(b);
				}
			}

			/*if (refs != null)
			{
				foreach (XmlNode n in refs.ChildNodes)
				{
					if (n.Name == "entry")
						references.Add(n.InnerText.Trim());
				}
			}*/
		}

		private XMLBlock ParseBlockXml(XmlNode node)
		{
			/*bool alias = false;
			if(node.Attributes["alias"] != null && node.Attributes["alias"].Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
				alias = true;*/
			XMLBlock b = new XMLBlock(node.Attributes["name"].Value);
			foreach (XmlNode n in node.ChildNodes)
			{
				switch (n.Name)
				{
					case "block":
						b.AddBlock(ParseBlockXml(n));
						break;
					case "parameter":
						b.AddParameter(ParseParameterXml(n));
						break;
					case "required":
						b.SetRequired(n.InnerText.Trim());
						break;
				}
			}

			return b;
		}

		private XMLParameter ParseParameterXml(XmlNode n)
		{
			string paramName = n.Attributes["name"].Value;
			string paramBehavior = "";
			if (n.Attributes["behavior"] != null)
				paramBehavior = n.Attributes["behavior"].Value;

			List<XMLParamData> data = new List<XMLParamData>();
			foreach (XmlNode n2 in n.ChildNodes)
			{
				XMLParamData d = new XMLParamData();
				d.type = n2.Name;
				if (n2.Attributes["behavior"] != null)
					d.behaviors = n2.Attributes["behavior"].Value;

				d.masksRegex = new List<string>();
				d.masksPattern = new List<string>();
				d.masksNumeric = new List<string>();

				foreach (XmlNode n3 in n2.ChildNodes)
				{
					if (n3.Name == "pattern")
						d.masksPattern.Add(n3.InnerText.Trim());
					else if (n3.Name == "regex")
						d.masksRegex.Add(n3.InnerText.Trim());
					else if (n3.Name == "numeric")
						d.masksNumeric.Add(n3.InnerText.Trim());
				}

				data.Add(d);
			}

			return new XMLParameter(paramName, paramBehavior, data);
		}

		public List<XMLBlock> Blocks
		{
			get
			{
				return xblocks;
			}
		}

		public List<XMLParameter> Parameters
		{
			get
			{
				return xparams;
			}
		}
		
		
	}
}