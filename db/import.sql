DROP DATABASE IF EXISTS eob;

CREATE DATABASE eob;

USE eob;

CREATE TABLE medewerkers (
    id INT NOT NULL AUTO_INCREMENT,
    naam VARCHAR(255) NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE Abonnement (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    naam VARCHAR(255),
    prijs DECIMAL(10, 2),
    geldigheid_start DATE,
    geldigheid_eind DATE
);

CREATE TABLE Gebruiker (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    voornaam VARCHAR(255),
    achternaam VARCHAR(255),
    email VARCHAR(255),
    wachtwoord VARCHAR(255),
    abonnement_id INT,
    FOREIGN KEY (abonnement_id) REFERENCES Abonnement(id)
);

CREATE TABLE Betaling (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    datum DATE,
    bedrag DECIMAL(10, 2),
    abonnement_id INT,
    FOREIGN KEY (abonnement_id) REFERENCES Abonnement(id)
);

CREATE TABLE Toegang (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    datum DATE,
    gebruiker_id INT,
    abonnement_id INT,
    FOREIGN KEY (gebruiker_id) REFERENCES Gebruiker(id),
    FOREIGN KEY (abonnement_id) REFERENCES Abonnement(id)
);
