use Superheroes;

CREATE TABLE Superhero (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Alias VARCHAR(50) NOT NULL,
    Origin VARCHAR(50) NOT NULL
);
CREATE TABLE Assistant (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
);

CREATE TABLE Power (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Description TEXT
);
