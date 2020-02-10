CREATE TABLE dbo.FipeVehicleBrand
(
    Id BIGINT NOT NULL PRIMARY KEY,
    FipeVehicleTypeId SMALLINT NOT NULL,
    [Description] VARCHAR(80) NOT NULL,
)

ALTER TABLE [dbo].[FipeVehicleBrand]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehicleBrand_FipeVehicleType] 
FOREIGN KEY([FipeVehicleTypeId]) REFERENCES [dbo].[FipeVehicleType] ([Id])
