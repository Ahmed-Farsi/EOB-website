DROP DATABASE IF EXISTS eob;

CREATE DATABASE eob;

CREATE TABLE medewerkers{
    id NOT NULL AUTO_INCREMENT,
    naam NOT NULL,
    
}

CREATE TABLE Abonnement (
    id INT NOT NULL PRIMARY KEY,
    naam VARCHAR(255),
    prijs DECIMAL(10, 2),
    geldigheid_start DATE,
    geldigheid_eind DATE
);

CREATE TABLE Gebruiker (
    id INT NOT NULL PRIMARY KEY,
    voornaam VARCHAR(255),
    achternaam VARCHAR(255),
    email VARCHAR(255),
    wachtwoord VARCHAR(255),
    abonnement_id INT,
    FOREIGN KEY (abonnement_id) REFERENCES Abonnement(id)
);

CREATE TABLE Betaling (
    id INT NOT NULL PRIMARY KEY,
    datum DATE,
    bedrag DECIMAL(10, 2),
    abonnement_id INT,
    FOREIGN KEY (abonnement_id) REFERENCES Abonnement(id)
);

CREATE TABLE Toegang (
    id INT NOT NULL PRIMARY KEY,
    datum DATE,
    gebruiker_id INT,
    abonnement_id INT,
    FOREIGN KEY (gebruiker_id) REFERENCES Gebruiker(id),
    FOREIGN KEY (abonnement_id) REFERENCES Abonnement(id)
);
