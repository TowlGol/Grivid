using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Assets.Imvisd.Scrpits {
    public class CriteriaFilterClass {
        public string field { get; set; }
        public List<string> valueArry { get; set; } = new List<string>();
        public List<List<string>> value2Arry { get; set; } = new List<List<string>>();
        public bool contain { get; set; }
        public bool strict { get; set; }
        public DataType type { get; set; }
    }
}
