CREATE TABLE FipeVehicleBrand
(
    Id BIGINT NOT NULL PRIMARY KEY,
    FipeVehicleTypeId SMALLINT NOT NULL,
    [Description] VARCHAR(80) NOT NULL,
    Active BIT NOT NULL DEFAULT(1)
)

ALTER TABLE [FipeVehicleBrand]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehicleBrand_FipeVehicleType] 
FOREIGN KEY([FipeVehicleTypeId]) REFERENCES [FipeVehicleType] ([Id])
