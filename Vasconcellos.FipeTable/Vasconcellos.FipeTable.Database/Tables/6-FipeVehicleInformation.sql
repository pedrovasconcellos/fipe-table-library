CREATE TABLE FipeVehicleInformation
(
    Id VARCHAR(45) NOT NULL PRIMARY KEY,
    FipeCode VARCHAR(30) NOT NULL,
    FipeVehicleModelId BIGINT NOT NULL,
    [Year] SMALLINT NOT NULL,
    VehicleFuelTypeId SMALLINT NOT NULL,
    [Authentication] VARCHAR(30) NOT NULL,
    FipeVehicleFuelTypeId SMALLINT NOT NULL,
    IsValid BIT NOT NULL DEFAULT(0),
    Created DATETIME NOT NULL,
    Updated DATETIME NULL,
    Active BIT NOT NULL DEFAULT(1)
)

ALTER TABLE [FipeVehicleInformation]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehicleInformation_FipeVehicleModel] 
FOREIGN KEY([FipeVehicleModelId]) REFERENCES [FipeVehicleModel] ([Id])

ALTER TABLE [FipeVehicleInformation]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehicleInformation_FipeVehicleFuelType] 
FOREIGN KEY([FipeVehicleFuelTypeId]) REFERENCES [FipeVehicleFuelType] ([Id])

ALTER TABLE [FipeVehicleInformation]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehicleInformation_VehicleFuelType] 
FOREIGN KEY([VehicleFuelTypeId]) REFERENCES [VehicleFuelTypes] ([Id])