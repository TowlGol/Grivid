using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum Trigger {
    NULL,
    Onclick,
    Onhover,
    HoverExit,
    Ondrag,
    DragExit
    
}
namespace Assets.Imvisd.Scrpits {
    class InteractionClass {
        public string name;
        public string function;
        public Trigger trigger;
        public List<string> attribute = new List<string>();
    }
}
