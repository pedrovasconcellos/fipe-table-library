using System;
using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    //Note: Class not yet developed
    internal class FipeVehicleInformationUnnormalized
    {
        //internal FipeVehicleInformationUnnormalized() 
        //{ 
        //}

        //internal FipeVehicleInformationUnnormalized(
        //    long brandId,
        //    string fipeCode,
        //    int fipeReferenceId,
        //    long modelId, 
        //    short year,
        //    VehicleFuelTypesEnum vehicleFuelTypeId,
        //    decimal value,
        //    string authentication,
        //    FipeVehicleFuelTypesEnum fipeVehicleFuelTypeId
        //    )
        //{
        //    var vehicleIsValid = this.VehicleIsValid(
        //        brandId, 
        //        fipeCode, 
        //        fipeReferenceId, 
        //        modelId, year, 
        //        vehicleFuelTypeId, 
        //        value, 
        //        fipeVehicleFuelTypeId);

        //    var id = this.CreteId(fipeReferenceId, brandId, modelId, year, fipeVehicleFuelTypeId);

        //    if (vehicleIsValid)
        //    {
        //        this.Id = id;
        //        this.Active = true;
        //        this.IsValid = true;
        //    }
        //    else
        //    {
        //        this.Id = $"Invalid-Vehicle-{Guid.NewGuid()}-{id}";
        //        this.Active = false;
        //        this.IsValid = false;
        //    }
            
        //    this.FipeCode = fipeCode;
        //    this.FipeReferenceId = fipeReferenceId;
        //    this.FipeVehicleModelId = modelId;
        //    this.Year = year;
        //    this.VehicleFuelTypeId = vehicleFuelTypeId;
        //    this.Value = value;
        //    this.Authentication = authentication;
        //    this.FipeVehicleFuelTypeId = fipeVehicleFuelTypeId;
        //    this.Created = DateTime.UtcNow;
        //    this.Updated = null;
        //}

        //public string Id { get; set; }
        //public string FipeCode { get; set; }
        //public int FipeReferenceId { get; set; }
        //public long FipeVehicleModelId { get; set; }
        //public short Year { get; set; }
        //public VehicleFuelTypesEnum VehicleFuelTypeId { get; set; }
        //public decimal Value { get; set; }      
        //public string Authentication { get; set; }
        //public FipeVehicleFuelTypesEnum FipeVehicleFuelTypeId { get; set; }
        //public bool IsValid { get; set; }
        //public DateTime Created { get; set; }
        //public DateTime? Updated { get; set; }
        //public bool Active { get; set; }

        //public string CreteId(
        //    long fipeReferenceId,
        //    long brandId,
        //    long modelId,
        //    short year,
        //    FipeVehicleFuelTypesEnum fipeVehicleFuelTypeId)
        //{
        //    return $"{fipeReferenceId}-{brandId}-{modelId}-{year}-{(short)fipeVehicleFuelTypeId}";
        //}

        //private bool VehicleIsValid(
        //    long brandId, 
        //    string fipeCode, 
        //    long fipeReferenceId, 
        //    long modelId, 
        //    short year, 
        //    VehicleFuelTypesEnum vehicleFuelTypeId, 
        //    decimal value, 
        //    FipeVehicleFuelTypesEnum fipeVehicleFuelTypeId)
        //{
        //    var vehicleIsInvalid =
        //        brandId < 1 ||
        //        fipeReferenceId < 0 ||
        //        modelId < 1 ||
        //        year < 1886 ||
        //        (year > (DateTime.Now.Year + 1) && year != 32000) ||
        //        string.IsNullOrEmpty(fipeCode) ||
        //        !Enum.IsDefined(typeof(VehicleFuelTypesEnum), vehicleFuelTypeId) ||
        //        value <= 0 ||
        //        !Enum.IsDefined(typeof(FipeVehicleFuelTypesEnum), fipeVehicleFuelTypeId);

        //    return !vehicleIsInvalid;
        //}

        //public bool VehicleIsValid(long brandId) 
        //{
        //    return this.VehicleIsValid(
        //        brandId, 
        //        this.FipeCode, 
        //        this.FipeReferenceId, 
        //        this.FipeVehicleModelId, 
        //        this.Year, 
        //        this.VehicleFuelTypeId, 
        //        this.Value, 
        //        this.FipeVehicleFuelTypeId);
        //}
    }
}