
using System.ComponentModel;

namespace Vasconcellos.FipeTable.Types.Enums
{
    /// <summary>
    /// Fipe vehicle fuel types.
    /// </summary>
    public enum FipeVehicleFuelTypesEnum : short
    {
        [Description("Gasolina")]
        Gasoline = 1,

        [Description("Etanol")]
        Ethanol = 2,

        [Description("Diesel")]
        Diesel = 3,
    }
}
