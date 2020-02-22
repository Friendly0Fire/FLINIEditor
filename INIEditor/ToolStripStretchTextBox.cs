using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace INIEditor
{
	class ToolStripStretchTextBox : ToolStripTextBox
	{
		public override System.Drawing.Size GetPreferredSize(System.Drawing.Size constrainingSize)
		{
			if (IsOnOverflow || Owner.Orientation == Orientation.Vertical)
			{
				return DefaultSize;
			}
			
			Size n = TextRenderer.MeasureText(this.Text, this.Font);
			n.Width = (int) (n.Width * 1.1);
			if(n.Width < DefaultSize.Width) return DefaultSize;
			else return n;
		}
	}
}
