using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloximmo.be
{
    public class CloximmoItems
    {
        public List<CloximmoItem> items { get; set; }

        public class CloximmoItem
        {
            public int _id { get; set; }
        }
    }
}