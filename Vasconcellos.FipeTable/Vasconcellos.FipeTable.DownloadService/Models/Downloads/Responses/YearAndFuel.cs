using Vasconcellos.FipeTable.Types.Enums;
using System;

namespace Vasconcellos.FipeTable.DownloadService.Models.Responses
{
    public class YearAndFuel
    {
        public string Value { get; set; }
        public string Label { get; set; }

        private short _year { get; set; }
        public short Year
        {
            get
            {
                if (this._year == 0)
                    this._year = Convert.ToInt16(this.Value?.Split("-")?[0]);
                return this._year;
            }
        }

        private FipeVehicleFuelTypesEnum _fuel { get; set; }
        public FipeVehicleFuelTypesEnum Fuel
        {
            get
            {
                if (this._fuel == 0)
                    this._fuel = (FipeVehicleFuelTypesEnum)Convert.ToInt16(this.Value?.Split("-")?[1]);
                return this._fuel;
            }
        }
    }
}
