using System.Collections.Generic;
using Newtonsoft.Json;

namespace Libro.Google
{
    public class Volumes
    {
        [JsonProperty("items")]
        public virtual IList<Volume> Items { get; set; }

        [JsonProperty("kind")]
        public virtual string Kind { get; set; }

        [JsonProperty("totalItems")]
        public virtual int? TotalItems { get; set; }

        public virtual string ETag { get; set; }
    }
}
