using System.Collections.Generic;

namespace Vasconcellos.FipeTable.DownloadService.Models.Responses
{
    public class AuxModel
    {
        public AuxModel()
        {
            this.Modelos = new List<Model>();
        }
        public List<Model> Modelos { get; set; }
    }
}
