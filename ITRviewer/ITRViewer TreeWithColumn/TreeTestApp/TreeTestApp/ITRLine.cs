using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TreeTestApp
{
    public class ITRLine
    {
        public string line;
        public int indent, instr;
        public string name, address;
        public bool Read(StreamReader reader)
        {
            if ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                indent = line.LastIndexOf("-") + 1;
                string[] parts = line.Substring(indent).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                name = parts[0];
                if (parts[1] == "000000")
                {
                    parts[1] = null;
                }
                address = parts[1];
                instr = Int32.Parse(parts[2]);
                return true;
            }
            return false;
        }
    }
}
