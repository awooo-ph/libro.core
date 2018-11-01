using Newtonsoft.Json;

namespace Libro.Google
{
    public partial class Volume 
    {
   
        public partial class VolumeInfoData
        {
            public class PanelizationSummaryData
            {
                [JsonProperty("containsEpubBubbles")]
                public virtual bool? ContainsEpubBubbles { get; set; }

                [JsonProperty("containsImageBubbles")]
                public virtual bool? ContainsImageBubbles { get; set; }

                [JsonProperty("epubBubbleVersion")]
                public virtual string EpubBubbleVersion { get; set; }

                [JsonProperty("imageBubbleVersion")]
                public virtual string ImageBubbleVersion { get; set; }

            }
        }
    }

}
