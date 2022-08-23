CREATE TABLE FipeVehicleInformationDenormalized
(
    Id VARCHAR(45) NOT NULL PRIMARY KEY,
    FipeCode VARCHAR(30) NOT NULL,
    VehicleTypeId SMALLINT NOT NULL,
    VehicleTypeDescription VARCHAR(30) NOT NULL,
    FipeVehicleBrandId BIGINT NOT NULL,
    FipeVehicleBrandDescription VARCHAR(80) NOT NULL,
    FipeVehicleModelId BIGINT NOT NULL,
    FipeVehicleModelDescription VARCHAR(100) NOT NULL,
    [Year] SMALLINT NOT NULL,
    VehicleFuelTypeId SMALLINT NOT NULL,
    VehicleFuelTypeDescription VARCHAR(20) NOT NULL,
    [Authentication] VARCHAR(30) NOT NULL,
    FipeVehicleFuelTypeId SMALLINT NOT NULL,
    FipeVehicleFuelTypeDescription VARCHAR(20) NOT NULL,
    IsValid BIT NOT NULL DEFAULT(0),
    Created DATETIME NOT NULL,
    Updated DATETIME NULL,
    Active BIT NOT NULL DEFAULT(1)
)

ALTER TABLE [FipeVehicleInformationDenormalized]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehicleInformationDenormalized_FipeVehicleType] 
FOREIGN KEY([FipeVehicleTypeId]) REFERENCES [FipeVehicleType] ([Id])

ALTER TABLE [FipeVehicleInformationDenormalized]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehicleInformationDenormalized_FipeVehicleBrand] 
FOREIGN KEY([FipeVehicleBrandId]) REFERENCES [FipeVehicleBrand] ([Id])

ALTER TABLE [FipeVehicleInformationDenormalized]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehicleInformationDenormalized_FipeVehicleModel] 
FOREIGN KEY([FipeVehicleModelId]) REFERENCES [FipeVehicleModel] ([Id])

ALTER TABLE [FipeVehicleInformationDenormalized]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehicleInformationDenormalized_FipeVehicleFuelType] 
FOREIGN KEY([FipeVehicleFuelTypeId]) REFERENCES [FipeVehicleFuelType] ([Id])

ALTER TABLE [FipeVehicleInformationDenormalized]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehicleInformationDenormalized_VehicleFuelType] 
FOREIGN KEY([VehicleFuelTypeId]) REFERENCES [VehicleFuelTypes] ([Id])
