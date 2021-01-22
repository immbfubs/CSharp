using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace TreeTestApp
{
    public class ITRviewer
    {
        CommonTools.NodeCollection rootNodes = new CommonTools.NodeCollection(null);
        List<CommonTools.Node> allNodes = new List<CommonTools.Node>();

        internal ITRviewer(CommonTools.NodeCollection collection)
        {
            rootNodes = collection;
        }

        internal void BuildTree(string filePath)
        {
            int linesCount = CountFileLines(filePath);
            
            ITRLine line = new ITRLine();
            CommonTools.Node node;
            int level = 0;
            int[] index = new int[15];
            int[] totalInstrCount = new int[15];

            using (StreamReader reader = new StreamReader(filePath))
            {
                while (line.Read(reader) == true)
                {
                    if (line.indent == 0)
                    {
                        node = new CommonTools.Node(new object[5] { line.name, line.indent, line.address, line.instr, null });
                        if (rootNodes.Count > 0)
                        {
                            allNodes[index[0]]["allInstr"] = totalInstrCount[0];
                        }
                        rootNodes.Add(node);
                    }
                    else if (line.indent < level)
                    {
                        node = new CommonTools.Node(new object[5] { line.name, line.indent, line.address, line.instr, null });
                        allNodes[index[line.indent - 1]].Nodes.Add(node);
                        for (int i = level - 1; i >= line.indent - 1; i--)
                        {
                            allNodes[index[i]]["allInstr"] = totalInstrCount[i];
                        }
                        for (int i = line.indent; i >= 0; i--)
                        {
                            totalInstrCount[i] += line.instr;
                        }
                    }
                    else
                    {
                        node = new CommonTools.Node(new object[5] { line.name, line.indent, line.address, line.instr, null });
                        allNodes[index[line.indent - 1]].Nodes.Add(node);
                        for (int i = line.indent; i >= 0; i--)
                        {
                            totalInstrCount[i] += line.instr;
                        }
                    }

                    allNodes.Add(node);
                    level = line.indent;
                    index[level] = allNodes.Count - 1;
                    totalInstrCount[level] = line.instr;
                }
            }

            MessageBox.Show(+linesCount + " lines read");

            for (int i = level - 1; i >= 0; i--)
            {
                allNodes[index[i]]["allInstr"] = totalInstrCount[i];
            }

            //CommonTools.CellPainter asd;
            //asd.
        }

        private int CountFileLines(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                int lines = 0;
                while (reader.ReadLine() != null)
                {
                    lines++;
                }
                return lines;
            }
        }

        internal bool GetDistinct(CommonTools.Node node, ListView.ListViewItemCollection collection)
        {
            //Return all distinct nodes in the subtree of the parameter node. Returns in the parameter colleciton.
            if (!node.HasChildren)
            {
                MessageBox.Show("No children!");
                return false;
            }

            int index = allNodes.IndexOf(node);
            int stopIndex = FindStopIndex(node);
            int[] callCounter = new int[500];
            StringCollection strCollection = new StringCollection();

            for (int i = index + 1; i < stopIndex; i++)
            {
                string moduleName = allNodes[i]["name"].ToString();
                if (moduleName.Contains("@"))
                {
                    moduleName = moduleName.Substring(0, moduleName.IndexOf("@"));
                }

                if (!strCollection.Contains(moduleName))
                {
                    strCollection.Add(moduleName);

                    ListViewItem item = new ListViewItem(moduleName);
                    item.SubItems.Add("1");
                    collection.Add(item);
                }
                else
                {
                    callCounter[strCollection.IndexOf(moduleName)]++;
                    //To be continued...
                }
            }

            return true;
        }

        internal bool GetDistinctAt(CommonTools.Node node, ListView.ListViewItemCollection collection)
        {
            //Return all distinct nodes in the subtree of the parameter node. Returns in the parameter colleciton.
            if (!node.HasChildren)
            {
                MessageBox.Show("No children!");
                return false;
            }

            int index = allNodes.IndexOf(node);
            int stopIndex = FindStopIndex(node);
            int[] callCounter = new int[500];
            StringCollection strCollection = new StringCollection();

            for (int i = index + 1; i < stopIndex; i++)
            {
                string moduleName = allNodes[i]["name"].ToString();

                if (!strCollection.Contains(moduleName))
                {
                    strCollection.Add(moduleName);

                    ListViewItem item = new ListViewItem(moduleName);
                    item.SubItems.Add("1");
                    collection.Add(item);
                }
                else
                {
                    callCounter[strCollection.IndexOf(moduleName)]++;
                    //To be continued...
                }
            }

            return true;
        }

        private int FindStopIndex(CommonTools.Node node)
        {
            int index = allNodes.IndexOf(node.NextSibling);
            if (index == -1)
            {
                if (allNodes.IndexOf(node.Parent) == -1)
                {
                    return allNodes.Count;
                }
                return FindStopIndex(node.Parent);
            }
            return index;
        }

        internal void FindModule(string wanted)
        {
            MessageBox.Show("Passed", "Test");
            //rootNodes[2].
        }
    }
}
