CREATE TABLE FipeVehicleType
(
    Id SMALLINT NOT NULL PRIMARY KEY,
    [Description] VARCHAR(30) NOT NULL,
    Active BIT NOT NULL DEFAULT(1)
)

INSERT INTO FipeVehicleType (Id, [Description]) VALUES (1, 'Carro')
INSERT INTO FipeVehicleType (Id, [Description]) VALUES (2, 'Moto')
INSERT INTO FipeVehicleType (Id, [Description]) VALUES (3, 'Caminhão e Micro-Ônibus')
