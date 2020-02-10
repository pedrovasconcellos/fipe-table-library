CREATE TABLE VehicleFuelTypes (
    Id SMALLINT PRIMARY KEY NOT NULL,
    [Description] VARCHAR(20) NOT NULL,
    Active BIT NOT NULL DEFAULT(1)
)

INSERT INTO VehicleFuelTypes (Id, Description, Active) VALUES (1, 'Gasolina', 1)
INSERT INTO VehicleFuelTypes (Id, Description, Active) VALUES (2, 'Etanol', 1)
INSERT INTO VehicleFuelTypes (Id, Description, Active) VALUES (3, 'Diesel', 1)
INSERT INTO VehicleFuelTypes (Id, Description, Active) VALUES (4, 'Gás', 1)
INSERT INTO VehicleFuelTypes (Id, Description, Active) VALUES (5, 'Elétrico', 1)
INSERT INTO VehicleFuelTypes (Id, Description, Active) VALUES (6, 'Flex', 1)
