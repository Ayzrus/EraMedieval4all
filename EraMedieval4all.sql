-- --------------------------------------------------------
-- Anfitrião:                    127.0.0.1
-- Versão do servidor:           10.4.32-MariaDB - mariadb.org binary distribution
-- SO do servidor:               Win64
-- HeidiSQL Versão:              12.8.0.6908
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- A despejar estrutura da base de dados para eramedieval4all
CREATE DATABASE IF NOT EXISTS `eramedieval4all` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `eramedieval4all`;

-- A despejar estrutura para tabela eramedieval4all.alugar
CREATE TABLE IF NOT EXISTS `alugar` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DataAlugou` datetime NOT NULL DEFAULT current_timestamp(),
  `Cliente` int(11) NOT NULL,
  `Estado` enum('Decorrer','Finalizado') NOT NULL DEFAULT 'Decorrer',
  `DataEntrega` datetime NOT NULL,
  `Evento` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_alugar_clientes` (`Cliente`),
  KEY `FK_alugar_eventos` (`Evento`),
  CONSTRAINT `FK_alugar_clientes` FOREIGN KEY (`Cliente`) REFERENCES `clientes` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_alugar_eventos` FOREIGN KEY (`Evento`) REFERENCES `eventos` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Exportação de dados não seleccionada.

-- A despejar estrutura para tabela eramedieval4all.armazems
CREATE TABLE IF NOT EXISTS `armazems` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nome` text NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Exportação de dados não seleccionada.

-- A despejar estrutura para tabela eramedieval4all.clientes
CREATE TABLE IF NOT EXISTS `clientes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nif` varchar(9) NOT NULL,
  `Nome` varchar(100) NOT NULL,
  `Morada` varchar(150) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Telefone` varchar(15) NOT NULL,
  `Estado` enum('Ativo','Inativo') NOT NULL DEFAULT 'Ativo',
  `User` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Nif` (`Nif`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Exportação de dados não seleccionada.

-- A despejar estrutura para tabela eramedieval4all.cliente_utilizador
CREATE TABLE IF NOT EXISTS `cliente_utilizador` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Cliente` int(11) NOT NULL,
  `Utilizador` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_cliente_utilizador_clientes` (`Cliente`),
  KEY `FK_cliente_utilizador_utilizadores` (`Utilizador`),
  CONSTRAINT `FK_cliente_utilizador_clientes` FOREIGN KEY (`Cliente`) REFERENCES `clientes` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_cliente_utilizador_utilizadores` FOREIGN KEY (`Utilizador`) REFERENCES `utilizadores` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Exportação de dados não seleccionada.

-- A despejar estrutura para procedimento eramedieval4all.definir_traje_indisponivel
DELIMITER //
CREATE PROCEDURE `definir_traje_indisponivel`(
	IN `traje_id` INT
)
BEGIN
    -- Atualiza o estado do traje para 'Indisponivel' na tabela trajes
    UPDATE trajes
    SET Estado = 'Indisponivel'
    WHERE Id = traje_id;
END//
DELIMITER ;

-- A despejar estrutura para tabela eramedieval4all.eventos
CREATE TABLE IF NOT EXISTS `eventos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Localidade` text NOT NULL,
  `Descricao` text NOT NULL,
  `Titulo` text NOT NULL,
  `DataInicio` date NOT NULL,
  `DataFim` date NOT NULL,
  `Facebook` text NOT NULL,
  `Instagram` text NOT NULL,
  `TikTok` text NOT NULL,
  `Organizador` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_eventos_organizadores` (`Organizador`),
  CONSTRAINT `FK_eventos_organizadores` FOREIGN KEY (`Organizador`) REFERENCES `organizadores` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Exportação de dados não seleccionada.

-- A despejar estrutura para tabela eramedieval4all.logs_login
CREATE TABLE IF NOT EXISTS `logs_login` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Entrou` datetime NOT NULL DEFAULT current_timestamp(),
  `Saiu` datetime DEFAULT NULL,
  `Email` text NOT NULL,
  `Acao` enum('Sucesso','Falha','Bloqueada') NOT NULL,
  `Ip` text NOT NULL,
  `Motivo` text NOT NULL,
  `DataCriacao` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_logs_login_utilizadores` (`Email`(768)) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=400 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Exportação de dados não seleccionada.

-- A despejar estrutura para tabela eramedieval4all.organizadores
CREATE TABLE IF NOT EXISTS `organizadores` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nome` text NOT NULL,
  `Localidade` text NOT NULL,
  `Nif` varchar(9) NOT NULL DEFAULT '',
  `IdTipo` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_organizadores_tipo_organizador` (`IdTipo`) USING BTREE,
  CONSTRAINT `FK_organizadores_tipo_organizador` FOREIGN KEY (`IdTipo`) REFERENCES `tipo_organizador` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Exportação de dados não seleccionada.

-- A despejar estrutura para tabela eramedieval4all.tipo_organizador
CREATE TABLE IF NOT EXISTS `tipo_organizador` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Descricao` text NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Exportação de dados não seleccionada.

-- A despejar estrutura para tabela eramedieval4all.tipo_utilizadores
CREATE TABLE IF NOT EXISTS `tipo_utilizadores` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Descricao` enum('Administrador','user') NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Exportação de dados não seleccionada.

-- A despejar estrutura para tabela eramedieval4all.trajes
CREATE TABLE IF NOT EXISTS `trajes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nome` text NOT NULL,
  `Estado` enum('Disponivel','Indisponivel','Alugado','Inativo') NOT NULL DEFAULT 'Disponivel',
  `Valor` double NOT NULL,
  `Armazem` int(11) NOT NULL,
  `Foto` text DEFAULT NULL,
  `Quantidade` int(11) NOT NULL,
  `Especificacao` text NOT NULL,
  `Tipo` text NOT NULL,
  `Ref` text NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_trajes_armazems` (`Armazem`),
  CONSTRAINT `FK_trajes_armazems` FOREIGN KEY (`Armazem`) REFERENCES `armazems` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Exportação de dados não seleccionada.

-- A despejar estrutura para tabela eramedieval4all.trajes_alugar
CREATE TABLE IF NOT EXISTS `trajes_alugar` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Traje` int(11) NOT NULL,
  `Alugar` int(11) NOT NULL,
  `Data` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`Id`),
  KEY `FK_trajesalugar_trajes` (`Traje`),
  KEY `FK_trajes_alugar_alugar` (`Alugar`),
  CONSTRAINT `FK_trajes_alugar_alugar` FOREIGN KEY (`Alugar`) REFERENCES `alugar` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_trajesalugar_trajes` FOREIGN KEY (`Traje`) REFERENCES `trajes` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=68 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Exportação de dados não seleccionada.

-- A despejar estrutura para tabela eramedieval4all.utilizadores
CREATE TABLE IF NOT EXISTS `utilizadores` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nome` text NOT NULL,
  `Morada` text NOT NULL,
  `Telefone` int(11) NOT NULL,
  `Email` text NOT NULL,
  `Password` text NOT NULL,
  `Pin` text NOT NULL,
  `TipoUser` int(11) NOT NULL,
  `Status` enum('Ativo','Inativo','Bloqueado') NOT NULL DEFAULT 'Ativo',
  `DataCriacao` datetime NOT NULL DEFAULT current_timestamp(),
  `UltimoAcesso` datetime DEFAULT NULL,
  `Cliente` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_utilizadores_tipo_utilizadores` (`TipoUser`),
  CONSTRAINT `FK_utilizadores_tipo_utilizadores` FOREIGN KEY (`TipoUser`) REFERENCES `tipo_utilizadores` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Exportação de dados não seleccionada.

-- A despejar estrutura para disparador eramedieval4all.atualizar_estado_traje_alugado
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_ZERO_IN_DATE,NO_ZERO_DATE,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER atualizar_estado_traje_alugado
AFTER INSERT ON trajes_alugar
FOR EACH ROW
BEGIN
    -- Atualiza o estado do traje para 'Alugado' na tabela trajes
    UPDATE trajes
    SET Estado = 'Alugado'
    WHERE Id = NEW.Traje;  -- Aqui assumimos que a coluna 'Trajes' na tabela 'trajes_alugar' contém o Id do traje
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
