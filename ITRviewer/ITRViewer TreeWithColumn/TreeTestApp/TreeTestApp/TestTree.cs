using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TreeTestApp
{
	class TestTree : CommonTools.TreeListView
	{
		ContextMenu m_contextMenu = null;
		ITRviewer itrHelp;

		public TestTree()
		{
			m_contextMenu = new ContextMenu();
			m_contextMenu.MenuItems.Add(new MenuItem("Collapse All Children", new EventHandler(OnCollapseAllChildren)));
			m_contextMenu.MenuItems.Add(new MenuItem("Expand All Children", new EventHandler(OnExpandAllChildren)));
			m_contextMenu.MenuItems.Add(new MenuItem("Show Distinct", new EventHandler(OnShowDistinct)));
			m_contextMenu.MenuItems.Add(new MenuItem("Show Distinct with @", new EventHandler(OnShowDistinctAt)));
			//m_contextMenu.MenuItems.Add(new MenuItem("Delete Selected Node", new EventHandler(OnDeleteSelectedNode)));

			itrHelp = new ITRviewer(Nodes);
		}

		void OnCollapseAllChildren(object sender, EventArgs e)
		{
			BeginUpdate();
			if (MultiSelect && NodesSelection.Count > 0)
			{
				foreach (CommonTools.Node selnode in NodesSelection)
				{
					foreach (CommonTools.Node node in selnode.Nodes)
						node.Collapse();
				}
				NodesSelection.Clear();
			}
			if (FocusedNode != null && FocusedNode.HasChildren)
			{
				foreach (CommonTools.Node node in FocusedNode.Nodes)
					node.Collapse();
			}
			EndUpdate();
		}

		void OnExpandAllChildren(object sender, EventArgs e)
		{
			BeginUpdate();
			if (MultiSelect && NodesSelection.Count > 0)
			{
				foreach (CommonTools.Node selnode in NodesSelection)
					selnode.ExpandAll();
				NodesSelection.Clear();
			}
			if (FocusedNode != null)
				FocusedNode.ExpandAll();
			EndUpdate();
		}

		void OnDeleteSelectedNode(object sender, EventArgs e)
		{
			BeginUpdate();
			CommonTools.Node node = FocusedNode;
			if (node != null && node.Owner != null)
			{
				node.Collapse();
				CommonTools.Node nextnode = CommonTools.NodeCollection.GetNextNode(node, 1);
				if (nextnode == null)
					nextnode = CommonTools.NodeCollection.GetNextNode(node, -1);
				node.Owner.Remove(node);
				FocusedNode = nextnode;
			}
			EndUpdate();
		}

		void OnShowDistinct(object sender, EventArgs e)
		{
			if (this.NodesSelection.Count == 1)
			{
				new Form2(NodesSelection[0], itrHelp, true);
			}
			else
			{
				MessageBox.Show("Works for single node!");
			}
		}

		void OnShowDistinctAt(object sender, EventArgs e)
		{
			if (this.NodesSelection.Count == 1)
			{
				new Form2(this.NodesSelection[0], itrHelp, false);
			}
			else
			{
				MessageBox.Show("Works for single node!");
			}
		}

		internal void OnFill(object sender, EventArgs e)
		{
			OpenFileDialog opf = new OpenFileDialog();
			if (Directory.Exists(@"F:\Ivaylo\IBA\ITR files")) opf.InitialDirectory = @"F:\Ivaylo\IBA\ITR files";
			else opf.InitialDirectory = "c:\\";
			opf.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			opf.FilterIndex = 2;
			opf.RestoreDirectory = true;

			if (opf.ShowDialog() == DialogResult.OK)
			{
				BeginUpdate();
				itrHelp.BuildTree(opf.FileName);
				EndUpdate();
			}
		}

		internal void OnSearch(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				BeginUpdate();
				//-------------------------------------------------------test1

				CommonTools.TreeList.TextFormatting newFormat = new CommonTools.TreeList.TextFormatting();
				//CommonTools.TreeList.TextFormatting newFormat = new CommonTools.TreeList.TextFormatting(GetFormatting(Nodes[1], Columns[0]));
				newFormat.BackColor = System.Drawing.Color.Red;

				CellPainter.PaintCell(CreateGraphics(), GetPlusMinusRectangle(Nodes[1], Columns[1], CommonTools.NodeCollection.GetVisibleNodeIndex(Nodes[1])), Nodes[1], Columns[1], newFormat, GetData(Nodes[1], Columns[1]));

				//-------------------------------------------------------test4
				//-------------------------------------------------------test5
				EndUpdate();

				itrHelp.FindModule(((TextBox)sender).Text);
			}
		}

		protected override void BeforeShowContextMenu()
		{
			if (GetHitNode() == null)
				ContextMenu = null;
			else
				ContextMenu = m_contextMenu;
		}

		protected override object GetData(CommonTools.Node node, CommonTools.TreeListColumn column)
		{
			object data = base.GetData(node, column);
			if (data != null)
				return data;

			if (column.Fieldname == "childCount")
			{
				if (node.HasChildren)
					return node.Nodes.Count;
				return "";
			}
			if (column.Fieldname == "visibleCount")
			{
				if (node.HasChildren)
					return node.VisibleNodeCount;
				return "";
			}
			return string.Empty;
		}

	}
}
