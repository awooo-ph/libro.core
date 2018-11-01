using Newtonsoft.Json;

namespace Libro.Google
{
    public partial class Volume 
    {
   
        public partial class VolumeInfoData
        {
            public class DimensionsData
            {
                [JsonProperty("height")]
                public virtual string Height { get; set; }

                [JsonProperty("thickness")]
                public virtual string Thickness { get; set; }

                [JsonProperty("width")]
                public virtual string Width { get; set; }

            }
        }
    }

}
