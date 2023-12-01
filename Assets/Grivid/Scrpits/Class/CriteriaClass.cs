using Newtonsoft.Json.Linq;
public enum DataType {
    Number,
    Time,
    String,
    NULL
}

namespace Assets.Imvisd.Scrpits {
     class CriteriaClass {
        public string id { get; set; }
        public JToken criteriaFilter;
    }
}
