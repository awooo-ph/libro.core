using Newtonsoft.Json;

namespace Libro.Google
{
    public partial class Volume 
    {
   
        public partial class VolumeInfoData
        {
            public class IndustryIdentifiersData
            {
                [JsonProperty("identifier")]
                public virtual string Identifier { get; set; }

                [JsonProperty("type")]
                public virtual string Type { get; set; }

            }
        }
    }

}
