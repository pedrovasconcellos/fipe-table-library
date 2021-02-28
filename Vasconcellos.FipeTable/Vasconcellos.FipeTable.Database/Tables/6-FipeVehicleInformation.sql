CREATE TABLE FipeVehicleInformation
(
    Id VARCHAR(50) NOT NULL PRIMARY KEY,
    FipeCode VARCHAR(30) NOT NULL,
    FipeReferenceCode INT NOT NULL, 
    FipeVehicleModelId BIGINT NOT NULL,
    [Year] SMALLINT NOT NULL,
    VehicleFuelTypeId SMALLINT NOT NULL,
    [Value] FLOAT NOT NULL,
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

CREATE UNIQUE NONCLUSTERED INDEX [IX_FipeVehicleInformation] ON [FipeVehicleInformation]
(
    [FipeVehicleModelId] ASC,
    [Year] DESC,
    [FipeVehicleFuelTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]