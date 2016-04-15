-- MySqlBackup.NET 2.0.9.3
-- Dump Time: 2016-04-15 17:00:36
-- --------------------------------------
-- Server version 5.6.17 MySQL Community Server (GPL)


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES latin1 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of classe
-- 

DROP TABLE IF EXISTS `classe`;
CREATE TABLE IF NOT EXISTS `classe` (
  `idClasse` tinyint(3) unsigned NOT NULL AUTO_INCREMENT,
  `Promotion` year(4) NOT NULL,
  `Location` varchar(255) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`idClasse`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COMMENT='une ligne pour chaque classe';

-- 
-- Dumping data for table classe
-- 

/*!40000 ALTER TABLE `classe` DISABLE KEYS */;
INSERT INTO `classe`(`idClasse`,`Promotion`,`Location`) VALUES
(1,2017,'1'),
(2,2016,'2');
/*!40000 ALTER TABLE `classe` ENABLE KEYS */;

-- 
-- Definition of eleve
-- 

DROP TABLE IF EXISTS `eleve`;
CREATE TABLE IF NOT EXISTS `eleve` (
  `idEleve` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Nom` varchar(30) NOT NULL COMMENT 'Nom de l''élève',
  `Prenom` varchar(30) NOT NULL COMMENT 'Prénom de l''élève',
  `idClasse` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`idEleve`),
  KEY `idClasse` (`idClasse`),
  CONSTRAINT `fk_classe_eleve` FOREIGN KEY (`idClasse`) REFERENCES `classe` (`idClasse`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque élève';

-- 
-- Dumping data for table eleve
-- 

/*!40000 ALTER TABLE `eleve` DISABLE KEYS */;
INSERT INTO `eleve`(`idEleve`,`Nom`,`Prenom`,`idClasse`) VALUES
(2,'Daniel','jack',1),
(4,'Dumortier','Paul',1),
(5,'Duhamel','Baptiste',1),
(6,'Morand','maxence',2),
(7,'Vancayzeele','Matthieu',2);
/*!40000 ALTER TABLE `eleve` ENABLE KEYS */;

-- 
-- Definition of note
-- 

DROP TABLE IF EXISTS `note`;
CREATE TABLE IF NOT EXISTS `note` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `idPdf` int(10) unsigned NOT NULL,
  `idCompetence` varchar(10) NOT NULL COMMENT 'ex: cp2.2 ; autonomie ; ...',
  `Note` varchar(10) NOT NULL,
  `maxNote` varchar(10) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `idTp` (`idPdf`),
  CONSTRAINT `fk_note_tp` FOREIGN KEY (`idPdf`) REFERENCES `tp` (`idPdf`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=52 DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque note';

-- 
-- Dumping data for table note
-- 

/*!40000 ALTER TABLE `note` DISABLE KEYS */;
INSERT INTO `note`(`ID`,`idPdf`,`idCompetence`,`Note`,`maxNote`) VALUES
(3,2,'CP2.3','9','11'),
(4,2,'CP2.4','2.5','3.5'),
(5,4,'CP2.3','5','10'),
(6,2,'CP2.5','6','6'),
(7,4,'CP2.3','42','55'),
(8,4,'CP2.3','25','47'),
(9,18,'CP3.1','5.5','8'),
(10,10,'CP1.1','4','5.5'),
(11,10,'CP1.2','9','16'),
(12,10,'CP1.3','2','6'),
(13,11,'CP1.1','2','5.5'),
(14,11,'CP1.2','10','16'),
(15,11,'CP1.3','3','6'),
(16,14,'CP2.1','5','7'),
(17,14,'CP2.2','4','8'),
(18,14,'CP2.3','2','7'),
(19,15,'CP2.1','6','7'),
(20,15,'CP2.2','3','8'),
(21,15,'CP2.3','3.5','7'),
(22,16,'CP2.1','5','7'),
(23,16,'CP2.2','3','8'),
(24,16,'CP2.3','3','7'),
(25,17,'CP2.1','5.5','7'),
(26,17,'CP2.2','4.5','8'),
(27,17,'CP2.3','3.5','7'),
(28,18,'CP3.3','9.5','14'),
(29,18,'CP2.2','4','9'),
(30,18,'CP1.1','2','3.5'),
(31,19,'CP3.3','6','14'),
(32,19,'CP2.2','5','9'),
(33,19,'CP1.1','2','3.5'),
(34,20,'CP3.3','8','14'),
(35,20,'CP2.2','4','9'),
(36,20,'CP1.1','1','3.5'),
(37,21,'CP3.3','6','14'),
(38,21,'CP2.2','9','9'),
(39,21,'CP1.1','3','3.5'),
(40,22,'CP1.2','2','5'),
(41,22,'CP2.3','7','10'),
(42,22,'CP3.1','10','15'),
(43,23,'CP1.2','4','5'),
(44,23,'CP2.3','6','10'),
(45,23,'CP3.1','13','15'),
(46,24,'CP1.2','4','5'),
(47,24,'CP2.3','8','10'),
(48,24,'CP3.1','13','15'),
(49,25,'CP1.2','2','5'),
(50,25,'CP2.3','3','10'),
(51,25,'CP3.1','6','15');
/*!40000 ALTER TABLE `note` ENABLE KEYS */;

-- 
-- Definition of tp
-- 

DROP TABLE IF EXISTS `tp`;
CREATE TABLE IF NOT EXISTS `tp` (
  `idPdf` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idTp` varchar(50) NOT NULL COMMENT 'ex: 2 => TP2',
  `idEleve` int(10) unsigned NOT NULL,
  `idcorrecteur` int(10) unsigned NOT NULL,
  `date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idPdf`),
  KEY `idPdf` (`idPdf`),
  KEY `idEleve` (`idEleve`),
  KEY `idcorrecteur` (`idcorrecteur`),
  CONSTRAINT `fk_tp_eleve` FOREIGN KEY (`idEleve`) REFERENCES `eleve` (`idEleve`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque PDF de lu';

-- 
-- Dumping data for table tp
-- 

/*!40000 ALTER TABLE `tp` DISABLE KEYS */;
INSERT INTO `tp`(`idPdf`,`idTp`,`idEleve`,`idcorrecteur`,`date`) VALUES
(2,'4',2,1,'2016-03-02 00:00:00'),
(4,'2',2,1,'2016-03-10 00:00:00'),
(5,'2',2,1,'2016-03-31 00:00:00'),
(10,'TP5',4,1,'2016-03-09 00:00:00'),
(11,'TP5',5,1,'2016-03-09 00:00:00'),
(12,'TP carross',2,4,'2016-04-06 00:00:00'),
(13,'TP verrin',2,3,'2016-04-12 00:00:00'),
(14,'TP portail',4,2,'2016-03-02 00:00:00'),
(15,'TP portail',5,2,'2016-03-09 00:00:00'),
(16,'TP portail',6,2,'2016-03-09 00:00:00'),
(17,'TP portail',7,2,'2016-03-09 00:00:00'),
(18,'TP verin',4,3,'2016-03-30 00:00:00'),
(19,'TP verin',5,3,'2016-03-30 00:00:00'),
(20,'TP verin',6,3,'2016-03-30 00:00:00'),
(21,'TP verin',7,3,'2016-03-30 00:00:00'),
(22,'TP carrosserie',4,4,'2016-07-17 00:00:00'),
(23,'Tp carrosserie',5,4,'2016-07-17 00:00:00'),
(24,'TP carrosserie',6,4,'2016-07-17 00:00:00'),
(25,'TP carrosserie',7,4,'2016-07-17 00:00:00');
/*!40000 ALTER TABLE `tp` ENABLE KEYS */;

-- 
-- Definition of user
-- 

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `idUser` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Login` varchar(32) NOT NULL,
  `Password` varchar(32) NOT NULL,
  `Admin` tinyint(1) DEFAULT '0' COMMENT 'si 1 => admin',
  PRIMARY KEY (`idUser`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque utilisateur (prof)';

-- 
-- Dumping data for table user
-- 

/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user`(`idUser`,`Login`,`Password`,`Admin`) VALUES
(1,'Atogue','azerty',1),
(2,'a','a',1),
(3,'z','z',0),
(4,'Pkubiak','root',1),
(5,'Pkubiak','root',1);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2016-04-15 17:00:36
-- Total time: 0:0:0:0:71 (d:h:m:s:ms)
