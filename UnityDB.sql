SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8 ;
USE `mydb` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Jogador` (
  `idJogador` INT NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(45) NOT NULL,
  `email` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idJogador`))
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `mydb`.`Run` (
  `idRun` INT NOT NULL AUTO_INCREMENT,
  `pontuacao` INT NOT NULL,
  `data` DATE NOT NULL,
  `Jogador_idJogador` INT NOT NULL,
  PRIMARY KEY (`idRun`, `Jogador_idJogador`),
  INDEX `fk_Run_Jogador_idx` (`Jogador_idJogador` ASC) VISIBLE,
  CONSTRAINT `fk_Run_Jogador`
    FOREIGN KEY (`Jogador_idJogador`)
    REFERENCES `mydb`.`Jogador` (`idJogador`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

INSERT INTO Jogador (nome, email) VALUES ("PepePassos07","ppp@gmail.com");
INSERT INTO Jogador (nome, email) VALUES ("GoodAdventure","goodOfWar@hotmail.com");
INSERT INTO Jogador (nome, email) VALUES ("Alvinho","allvezCapitao@gmail.com");
INSERT INTO Jogador (nome, email) VALUES ("éOconras","felipeconrado@outlook.com");

INSERT INTO Run (pontuacao, `data`, Jogador_idJogador) VALUES (300, now(), 1);
INSERT INTO Run (pontuacao, `data`, Jogador_idJogador) VALUES (512, now(), 3);
INSERT INTO Run (pontuacao, `data`, Jogador_idJogador) VALUES (110, now(), 4);
INSERT INTO Run (pontuacao, `data`, Jogador_idJogador) VALUES (480, now(), 2);


SELECT * FROM Jogador;
SELECT * FROM Run;

SELECT Jogador.idJogador, Jogador.nome, Run.pontuacao FROM Run INNER JOIN Jogador ON Jogador.idJogador=Run.Jogador_idJogador ORDER BY Run.pontuacao DESC;