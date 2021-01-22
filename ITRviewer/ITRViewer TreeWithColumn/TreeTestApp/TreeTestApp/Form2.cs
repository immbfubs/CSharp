using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TreeTestApp
{
	public partial class Form2 : Form
	{
		public Form2(CommonTools.Node node, ITRviewer itrView, bool maimuna)
		{
			InitializeComponent();
			bool itemsFound = false;

			distinctView.BeginUpdate();
			if (maimuna)
			{
				itemsFound = itrView.GetDistinctAt(node, distinctView.Items);
			}
			else
			{
				itemsFound = itrView.GetDistinct(node, distinctView.Items);
			}
			distinctView.EndUpdate();

			if(itemsFound) this.Show();
		}
	}
}