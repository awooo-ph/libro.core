using Newtonsoft.Json;

namespace Libro.Google
{
    public partial class Volume 
    {
        [JsonProperty("id")]
        public virtual string Id { get; set; }

        [JsonProperty("volumeInfo")]
        public virtual VolumeInfoData VolumeInfo { get; set; }
    }

}
