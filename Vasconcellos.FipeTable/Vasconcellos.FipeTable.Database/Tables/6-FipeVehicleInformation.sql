CREATE TABLE dbo.FipeVehicleInformation
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    FipeCode VARCHAR(30) NOT NULL,
    FipeVehicleModelId BIGINT NOT NULL,
    [Year] SMALLINT NOT NULL,
    FipeVehicleFuelTypeId SMALLINT NOT NULL,
    VehicleFuelTypeId SMALLINT NOT NULL,
    [Value] FLOAT NOT NULL,
    FipeReferenceId INT NOT NULL, 
    [Authentication] VARCHAR(30) NOT NULL,
    Created DATETIME NOT NULL,
    Updated DATETIME NULL,
    Active BIT NOT NULL DEFAULT(1)
)

ALTER TABLE [dbo].[FipeVehicleInformation]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehicleInformation_FipeVehicleModel] 
FOREIGN KEY([FipeVehicleModelId]) REFERENCES [dbo].[FipeVehicleModel] ([Id])

ALTER TABLE [dbo].[FipeVehicleInformation]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehicleInformation_FipeVehicleFuelType] 
FOREIGN KEY([FipeVehicleFuelTypeId]) REFERENCES [dbo].[FipeVehicleFuelType] ([Id])

ALTER TABLE [dbo].[FipeVehicleInformation]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehicleInformation_VehicleFuelType] 
FOREIGN KEY([VehicleFuelTypeId]) REFERENCES [dbo].[VehicleFuelType] ([Id])

CREATE UNIQUE NONCLUSTERED INDEX [IX_FipeVehicleInformation] ON [dbo].[FipeVehicleInformation]
(
    [FipeVehicleModelId] ASC,
    [Year] DESC,
    [FipeVehicleFuelTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]