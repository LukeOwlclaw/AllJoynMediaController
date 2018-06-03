using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableItemListView.Models
{
    public class VariableType
    {
        public int v { get; set; }

        public VariableType() { }
        public VariableType(int v)
        {
            this.v = v;
        }
    }
}
