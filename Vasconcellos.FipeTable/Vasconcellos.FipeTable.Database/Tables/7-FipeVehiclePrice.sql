CREATE TABLE FipeVehiclePrice
(
    Id VARCHAR(50) NOT NULL PRIMARY KEY,
    FipeReferenceId INT NOT NULL,
    FipeVehicleInformationId VARCHAR(45) NOT NULL,
    [Value] FLOAT NOT NULL,
    Active BIT NOT NULL DEFAULT(1)
)

ALTER TABLE [FipeVehiclePrice]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehiclePrice_FipeReference] 
FOREIGN KEY([FipeReferenceId]) REFERENCES [FipeReference] ([Id])

ALTER TABLE [FipeVehiclePrice]  WITH CHECK ADD  CONSTRAINT [FK_FipeVehiclePrice_FipeVehicleInformation] 
FOREIGN KEY([FipeVehicleInformationId]) REFERENCES [FipeVehicleInformation] ([Id])