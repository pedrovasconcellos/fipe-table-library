using System.Collections.Generic;

namespace Vasconcellos.FipeTable.DownloadService.Models.Responses
{
    public class Model
    {
        public Model()
        {
            this.YearAndFuels = new List<YearAndFuel>();
        }
        public string Value { get; set; }
        public string Label { get; set; }
        public List<YearAndFuel> YearAndFuels { get; set; }
    }
}
