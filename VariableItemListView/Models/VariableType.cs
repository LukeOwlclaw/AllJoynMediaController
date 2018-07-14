using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableItemListView.Models
{
    public class VariableType
    {
        public object Value { get; set; }
        public string Name { get { return PropertyPath?.LastOrDefault(); } }
        public IEnumerable<string> PropertyPath { get; set; }
        public string PropertyPathString { get { return (PropertyPath == null || PropertyPath.Count() == 0) ? null : string.Join(".", PropertyPath);  } }

        public int Index { get; set; }

        public VariableType() { }

        public override string ToString()
        {
            return Index + " " + PropertyPathString;
        }
    }
}
