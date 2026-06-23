-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 23-Jun-2026 às 02:26
-- Versão do servidor: 10.4.32-MariaDB
-- versão do PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Banco de dados: `graficos`
--

-- --------------------------------------------------------

--
-- Estrutura da tabela `temp_obr`
--

CREATE TABLE `temp_obr` (
  `id` int(11) NOT NULL,
  `VALOR01` varchar(100) NOT NULL,
  `VALOR02` varchar(100) NOT NULL,
  `codemp` varchar(100) NOT NULL,
  `codutil` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Extraindo dados da tabela `temp_obr`
--

INSERT INTO `temp_obr` (`id`, `VALOR01`, `VALOR02`, `codemp`, `codutil`) VALUES
(1, '2', '5', '1', '999'),
(2, '1', '2', '1', '999'),
(3, '2', '1', '1', '999'),
(4, '3', '4', '1', '999'),
(5, '4', '3', '1', '999'),
(6, '5', '6', '1', '999'),
(7, '6', '5', '1', '999'),
(8, '7', '8', '1', '999'),
(9, '8', '7', '1', '999'),
(10, '9', '10', '1', '999'),
(11, '10', '9', '1', '999'),
(12, '11', '12', '1', '999'),
(13, '12', '11', '1', '999'),
(14, '13', '14', '1', '999'),
(15, '14', '13', '1', '999'),
(16, '15', '16', '1', '999'),
(17, '16', '15', '1', '999'),
(18, '17', '18', '1', '999'),
(19, '18', '17', '1', '999'),
(20, '19', '20', '1', '999'),
(21, '20', '19', '1', '999');

-- --------------------------------------------------------

--
-- Estrutura da tabela `temp_ofi_vendas`
--

CREATE TABLE `temp_ofi_vendas` (
  `id` int(11) NOT NULL,
  `Valor01` varchar(50) NOT NULL,
  `Valor02` varchar(50) NOT NULL,
  `Valor03` varchar(50) NOT NULL,
  `Valor04` varchar(50) NOT NULL,
  `Valor05` varchar(50) NOT NULL,
  `Valor06` varchar(50) NOT NULL,
  `Valor07` varchar(50) NOT NULL,
  `Valor08` varchar(50) NOT NULL,
  `Valor09` varchar(50) NOT NULL,
  `Valor10` varchar(50) NOT NULL,
  `Valor11` varchar(50) NOT NULL,
  `Valor12` varchar(50) NOT NULL,
  `ano` int(50) NOT NULL,
  `codemp` int(50) NOT NULL,
  `codutil` int(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Extraindo dados da tabela `temp_ofi_vendas`
--

INSERT INTO `temp_ofi_vendas` (`id`, `Valor01`, `Valor02`, `Valor03`, `Valor04`, `Valor05`, `Valor06`, `Valor07`, `Valor08`, `Valor09`, `Valor10`, `Valor11`, `Valor12`, `ano`, `codemp`, `codutil`) VALUES
(1, '100', '120', '110', '130', '125', '140', '150', '145', '160', '170', '165', '180', 2026, 1, 999),
(2, '200', '190', '210', '220', '215', '230', '240', '235', '250', '260', '255', '270', 2026, 1, 999),
(3, '300', '320', '310', '330', '325', '340', '350', '345', '360', '370', '365', '380', 2026, 1, 999),
(4, '400', '390', '410', '420', '415', '430', '440', '435', '450', '460', '455', '470', 2026, 1, 999),
(5, '500', '520', '510', '530', '525', '540', '550', '545', '560', '570', '565', '580', 2026, 1, 999),
(6, '600', '590', '610', '620', '615', '630', '640', '635', '650', '660', '655', '670', 2026, 1, 999),
(7, '700', '720', '710', '730', '725', '740', '750', '745', '760', '770', '765', '780', 2026, 1, 999),
(8, '800', '790', '810', '820', '815', '830', '840', '835', '850', '860', '855', '870', 2026, 1, 999),
(9, '900', '920', '910', '930', '925', '940', '950', '945', '960', '970', '965', '980', 2026, 1, 999),
(10, '1000', '990', '1010', '1020', '1015', '1030', '1040', '1035', '1050', '1060', '1055', '1070', 2026, 1, 999),
(11, '1100', '1120', '1110', '1130', '1125', '1140', '1150', '1145', '1160', '1170', '1165', '1180', 2026, 1, 999),
(12, '1200', '1190', '1210', '1220', '1215', '1230', '1240', '1235', '1250', '1260', '1255', '1270', 2026, 1, 999),
(13, '1300', '1320', '1310', '1330', '1325', '1340', '1350', '1345', '1360', '1370', '1365', '1380', 2026, 1, 999),
(14, '1400', '1390', '1410', '1420', '1415', '1430', '1440', '1435', '1450', '1460', '1455', '1470', 2026, 1, 999),
(15, '1500', '1520', '1510', '1530', '1525', '1540', '1550', '1545', '1560', '1570', '1565', '1580', 2026, 1, 999),
(16, '1600', '1590', '1610', '1620', '1615', '1630', '1640', '1635', '1650', '1660', '1655', '1670', 2026, 1, 999),
(17, '1700', '1720', '1710', '1730', '1725', '1740', '1750', '1745', '1760', '1770', '1765', '1780', 2026, 1, 999),
(18, '1800', '1790', '1810', '1820', '1815', '1830', '1840', '1835', '1850', '1860', '1855', '1870', 2026, 1, 999),
(19, '1900', '1920', '1910', '1930', '1925', '1940', '1950', '1945', '1960', '1970', '1965', '1980', 2026, 1, 999),
(20, '2000', '1990', '2010', '2020', '2015', '2030', '2040', '2035', '2050', '2060', '2055', '2070', 2026, 1, 999);

--
-- Índices para tabelas despejadas
--

--
-- Índices para tabela `temp_obr`
--
ALTER TABLE `temp_obr`
  ADD PRIMARY KEY (`id`);

--
-- Índices para tabela `temp_ofi_vendas`
--
ALTER TABLE `temp_ofi_vendas`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de tabelas despejadas
--

--
-- AUTO_INCREMENT de tabela `temp_obr`
--
ALTER TABLE `temp_obr`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT de tabela `temp_ofi_vendas`
--
ALTER TABLE `temp_ofi_vendas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
