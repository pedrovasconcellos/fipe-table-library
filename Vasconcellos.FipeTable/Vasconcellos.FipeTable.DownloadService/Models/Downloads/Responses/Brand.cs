using System.Collections.Generic;

namespace Vasconcellos.FipeTable.DownloadService.Models.Responses
{
    public class Brand
    {
        public Brand()
        {
            this.Models = new List<Model>();
        }
        public string Value { get; set; }
        public string Label { get; set; }
        public List<Model> Models { get; set; }
    }
}
