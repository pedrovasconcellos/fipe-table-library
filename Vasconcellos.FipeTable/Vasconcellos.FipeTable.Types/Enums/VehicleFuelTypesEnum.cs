using System.ComponentModel;

namespace Vasconcellos.FipeTable.Types.Enums
{
    /// <summary>
    /// Vehicle fuel types.
    /// </summary>
    public enum VehicleFuelTypesEnum : short
    {
        [Description("Gasolina")]
        Gasoline = 1,

        [Description("Etanol")]
        Ethanol = 2,

        [Description("Diesel")]
        Diesel = 3,

        [Description("Gás")]
        Gas = 4,

        [Description("Elétrico")]
        Electric = 5,

        [Description("Flex")]
        Flex = 6,
    }
}
