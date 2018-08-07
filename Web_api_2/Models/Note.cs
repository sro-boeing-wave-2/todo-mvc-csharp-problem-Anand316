using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_api_2.Models
{
    public class Note
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string PlainText { get; set; }
        public bool Pin { get; set; }
        public List<Labels> Labels { get; set; }
        public List<CheckList> CList { get; set; }
    }
}
