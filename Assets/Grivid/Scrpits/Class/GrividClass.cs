using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public enum Target {
    NULL,
    User,
    Chart,
    Scene,
    Item
}


namespace Assets.Imvisd.Scrpits {
    class GrividClass {
        public JToken imvidItem { get; set; }
        public Target target { get; set; }
        public JToken criteria { get; set; }
        public JToken interaction { get; set; }
        public JToken param { get; set; }
    }
}
