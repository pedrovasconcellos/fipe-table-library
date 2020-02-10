CREATE TABLE FipeVehicleModel 
(
    Id BIGINT NOT NULL PRIMARY KEY,
    FipeVehicleBrandId BIGINT NOT NULL,
    [Description] VARCHAR(100) NOT NULL,
)

ALTER TABLE [FipeVehicleModel]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehicleModel_FipeVehicleBrand] 
FOREIGN KEY([FipeVehicleBrandId]) REFERENCES [FipeVehicleBrand] ([Id])
