using Newtonsoft.Json;

namespace Libro.Google
{
    public partial class Volume 
    {
   
        public partial class VolumeInfoData
        {
            public class ImageLinksData
            {

                [JsonProperty("extraLarge")]
                public virtual string ExtraLarge { get; set; }

                [JsonProperty("large")]
                public virtual string Large { get; set; }

                [JsonProperty("medium")]
                public virtual string Medium { get; set; }

                [JsonProperty("small")]
                public virtual string Small { get; set; }

                [JsonProperty("smallThumbnail")]
                public virtual string SmallThumbnail { get; set; }

                [JsonProperty("thumbnail")]
                public virtual string Thumbnail { get; set; }

            }
        }
    }

}
