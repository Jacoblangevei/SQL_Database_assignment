use Superheroes;
CREATE TABLE SuperheroPower (
    SuperheroId INT,
    PowerId INT,
    PRIMARY KEY (SuperheroId, PowerId),
    FOREIGN KEY (SuperheroId) REFERENCES Superhero(Id),
    FOREIGN KEY (PowerId) REFERENCES Power(Id)
);
