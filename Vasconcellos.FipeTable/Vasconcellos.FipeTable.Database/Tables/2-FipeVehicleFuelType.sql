CREATE TABLE FipeVehicleFuelType
(
    Id SMALLINT NOT NULL PRIMARY KEY,
    [Description] VARCHAR(20) NOT NULL,
    Active BIT NOT NULL DEFAULT(1)
)

INSERT INTO FipeVehicleFuelType (Id, [Description]) VALUES (1, 'Gasolina')
INSERT INTO FipeVehicleFuelType (Id, [Description]) VALUES (2, 'Álcool')
INSERT INTO FipeVehicleFuelType (Id, [Description]) VALUES (3, 'Diesel')
