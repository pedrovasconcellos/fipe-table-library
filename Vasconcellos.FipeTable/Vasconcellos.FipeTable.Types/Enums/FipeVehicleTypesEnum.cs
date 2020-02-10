
using System.ComponentModel;

namespace Vasconcellos.FipeTable.Types.Enums
{
    /// <summary>
    /// Fipe vehicle types.
    /// </summary>
    public enum FipeVehicleTypesEnum : short
    {
        [Description("Carro")]
        Car = 1,

        [Description("Motocicleta")]
        Motorcycle = 2,

        [Description("Caminhão e Micro-ônibus")]
        TruckAndMicroBus = 3,
    }
}
