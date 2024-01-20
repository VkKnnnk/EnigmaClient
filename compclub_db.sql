-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1:3306
-- Время создания: Янв 20 2024 г., 16:44
-- Версия сервера: 8.0.24
-- Версия PHP: 7.1.33

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `compclub_db`
--

-- --------------------------------------------------------

--
-- Структура таблицы `authdata`
--

CREATE TABLE `authdata` (
  `idAuth` int NOT NULL,
  `phone` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `password` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `authdata`
--

INSERT INTO `authdata` (`idAuth`, `phone`, `password`) VALUES
(29, '+7 (000) 000-00-00', 'AXPGlqMOS6XZdsHxy1gKTyWaSNn8p9vCaugmQRKlD1huWzWw'),
(30, '+7 (965) 533-21-02', 'BeonzIo8Epv5OBWp6acB6yqRDIqVYgsMAKmpX+lkeRu4AG6p'),
(31, '+7 (111) 111-11-11', '08EyhbMylmPWoSUGq0rkuFdYpCvGibEK4sNJtbNqQP8CnHEi'),
(32, '+7 (222) 222-22-22', '9wULs9sC95teP8kiknXwXBJFYbDjspalIIGnpjQ6fMFwPMH2');

-- --------------------------------------------------------

--
-- Структура таблицы `computers`
--

CREATE TABLE `computers` (
  `idComp` int NOT NULL,
  `idTypeTariffs` int NOT NULL,
  `idProcessor` int DEFAULT NULL,
  `idVideo_card` int DEFAULT NULL,
  `RAM` int DEFAULT NULL,
  `idMonitor` int DEFAULT NULL,
  `idKeyboard` int DEFAULT NULL,
  `idMouse` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `computers`
--

INSERT INTO `computers` (`idComp`, `idTypeTariffs`, `idProcessor`, `idVideo_card`, `RAM`, `idMonitor`, `idKeyboard`, `idMouse`) VALUES
(1, 1, 1, 1, 8, 1, 1, 1),
(2, 1, 1, 1, 8, 1, 1, 1),
(3, 1, 1, 1, 8, 1, 1, 1),
(4, 1, 1, 1, 8, 1, 1, 1),
(5, 1, 1, 1, 8, 1, 1, 1),
(6, 1, 1, 1, 8, 1, 1, 1),
(7, 1, 1, 1, 8, 1, 1, 1),
(8, 1, 1, 1, 8, 1, 1, 1),
(9, 1, 1, 1, 8, 1, 1, 1),
(10, 1, 1, 1, 8, 1, 1, 1),
(11, 2, 2, 2, 8, 2, 2, 2),
(12, 2, 2, 2, 8, 2, 2, 2),
(13, 2, 2, 2, 8, 2, 2, 2),
(14, 2, 2, 2, 8, 2, 2, 2),
(15, 2, 2, 2, 8, 2, 2, 2),
(16, 3, 3, 3, 16, 3, 3, 3),
(17, 3, 3, 3, 16, 3, 3, 3),
(18, 3, 3, 3, 16, 3, 3, 3),
(19, 3, 3, 3, 16, 3, 3, 3),
(20, 3, 3, 3, 16, 3, 3, 3),
(21, 3, 3, 3, 16, 3, 3, 3),
(22, 3, 3, 3, 16, 3, 3, 3),
(23, 3, 3, 3, 16, 3, 3, 3),
(24, 3, 3, 3, 16, 3, 3, 3),
(25, 3, 3, 3, 16, 3, 3, 3),
(26, 4, 4, 4, 32, 4, 4, 4),
(27, 4, 4, 4, 32, 4, 4, 4),
(28, 4, 4, 4, 32, 4, 4, 4),
(29, 4, 4, 4, 32, 4, 4, 4),
(30, 4, 4, 4, 32, 4, 4, 4),
(31, 5, 5, 5, 64, 5, 5, 5),
(32, 5, 5, 5, 64, 5, 5, 5),
(33, 5, 5, 5, 64, 5, 5, 5);

-- --------------------------------------------------------

--
-- Структура таблицы `keyboards`
--

CREATE TABLE `keyboards` (
  `idKeyboard` int NOT NULL,
  `model` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `keyboards`
--

INSERT INTO `keyboards` (`idKeyboard`, `model`) VALUES
(1, 'Logitech G G213 Prodigy'),
(2, 'HyperX Alloy Origins'),
(3, 'HyperX Alloy Elite 2'),
(4, 'ASUS ROG Strix Flare Silent'),
(5, 'ASUS ROG Claymore II');

-- --------------------------------------------------------

--
-- Структура таблицы `mice`
--

CREATE TABLE `mice` (
  `idMouse` int NOT NULL,
  `model` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `mice`
--

INSERT INTO `mice` (`idMouse`, `model`) VALUES
(1, 'A4Tech X-7120'),
(2, 'A4Tech Bloody P30 Pro'),
(3, 'HyperX Pulsefire Surge'),
(4, 'Razer Orochi V2'),
(5, 'Razer Naga Pro');

-- --------------------------------------------------------

--
-- Структура таблицы `monitors`
--

CREATE TABLE `monitors` (
  `idMonitor` int NOT NULL,
  `model` varchar(50) NOT NULL,
  `frequency` int NOT NULL,
  `resolution` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `monitors`
--

INSERT INTO `monitors` (`idMonitor`, `model`, `frequency`, `resolution`) VALUES
(1, 'ASUS TUF Gaming VG249Q', 144, '1920x1080'),
(2, 'Acer Nitro', 170, '2560x1440'),
(3, 'GIGABYTE AORUS FI25F', 240, '1920x1080'),
(4, 'Samsung Odyssey G7', 240, '2560x1440'),
(5, 'Samsung Odyssey Neo G9', 240, '5120x1440');

-- --------------------------------------------------------

--
-- Структура таблицы `processors`
--

CREATE TABLE `processors` (
  `idProcessor` int NOT NULL,
  `model` varchar(50) NOT NULL,
  `core_amount` varchar(10) NOT NULL,
  `frequency` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `processors`
--

INSERT INTO `processors` (`idProcessor`, `model`, `core_amount`, `frequency`) VALUES
(1, 'Intel Core i7-7700', '4+8', '4,2'),
(2, 'AMD Ryzen 5 2600', '6+12', '3,9'),
(3, 'Intel Core i9-9900', '8+16', '5'),
(4, 'AMD EPYC 7502', '32+64', '3,35'),
(5, 'AMD EPYC 7763', '64+128', '3,5');

-- --------------------------------------------------------

--
-- Структура таблицы `sessions`
--

CREATE TABLE `sessions` (
  `idSession` int NOT NULL,
  `idUser` int NOT NULL,
  `idTariff` int NOT NULL,
  `start_session` datetime NOT NULL,
  `end_session` datetime NOT NULL,
  `idComputer` int NOT NULL,
  `cost` int NOT NULL,
  `status` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `sessions`
--

INSERT INTO `sessions` (`idSession`, `idUser`, `idTariff`, `start_session`, `end_session`, `idComputer`, `cost`, `status`) VALUES
(24, 24, 1, '2023-06-24 09:43:55', '2023-06-24 13:43:55', 1, 0, 1),
(25, 25, 2, '2023-06-24 10:13:36', '2023-06-24 17:13:36', 1, 0, 1);

-- --------------------------------------------------------

--
-- Структура таблицы `tariff`
--

CREATE TABLE `tariff` (
  `idTariff` int NOT NULL,
  `name` varchar(100) NOT NULL,
  `duration` float NOT NULL,
  `idTypeTariffs` int NOT NULL,
  `fix_price` float DEFAULT NULL,
  `appearance_hour` int DEFAULT NULL,
  `appearance_duration` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `tariff`
--

INSERT INTO `tariff` (`idTariff`, `name`, `duration`, `idTypeTariffs`, `fix_price`, `appearance_hour`, `appearance_duration`) VALUES
(1, 'Пакет на 3 часа', 3, 1, NULL, NULL, NULL),
(2, 'Пакет на 6 часов', 6, 1, NULL, NULL, NULL),
(3, 'Специальный пакет', 24, 1, 1500, NULL, NULL),
(4, 'Пакет на 1 час', 1, 1, NULL, NULL, NULL),
(5, 'Ночной пакет', 12, 1, NULL, 22, 8),
(6, 'Эксперементальный', 3, 1, 450, 9, 3);

-- --------------------------------------------------------

--
-- Структура таблицы `tariff_allowance`
--

CREATE TABLE `tariff_allowance` (
  `idAllowance` int NOT NULL,
  `name` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `allowance_percent` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `tariff_allowance`
--

INSERT INTO `tariff_allowance` (`idAllowance`, `name`, `allowance_percent`) VALUES
(1, 'Выходной', -30),
(2, 'Праздник', 50),
(3, 'Ночь', 30),
(4, 'За каждый час', 30);

-- --------------------------------------------------------

--
-- Структура таблицы `type_tariffs`
--

CREATE TABLE `type_tariffs` (
  `idTypeTariffs` int NOT NULL,
  `name` varchar(50) NOT NULL,
  `price_hour` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `type_tariffs`
--

INSERT INTO `type_tariffs` (`idTypeTariffs`, `name`, `price_hour`) VALUES
(1, 'STANDART', 50),
(2, 'STANDART+', 60),
(3, 'CYBERSPORT', 90),
(4, 'ALLSTAR', 140),
(5, 'VIP', 200);

-- --------------------------------------------------------

--
-- Структура таблицы `users`
--

CREATE TABLE `users` (
  `idUser` int NOT NULL,
  `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `idAuth` int NOT NULL,
  `personal_discount` int DEFAULT NULL,
  `cash` float DEFAULT NULL,
  `avatar_img` varchar(150) DEFAULT NULL,
  `auth_status` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `users`
--

INSERT INTO `users` (`idUser`, `name`, `idAuth`, `personal_discount`, `cash`, `avatar_img`, `auth_status`) VALUES
(24, 'Enigma CLient', 29, NULL, 300, NULL, 0),
(25, 'Enigma CLient', 30, NULL, 0, NULL, 0),
(26, 'Enigma CLient', 31, NULL, NULL, NULL, 0),
(27, 'Enigma CLient', 32, NULL, NULL, NULL, 0);

-- --------------------------------------------------------

--
-- Структура таблицы `video_cards`
--

CREATE TABLE `video_cards` (
  `idVideo_card` int NOT NULL,
  `model` varchar(50) NOT NULL,
  `memory` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `video_cards`
--

INSERT INTO `video_cards` (`idVideo_card`, `model`, `memory`) VALUES
(1, 'Nvidia GTX 1060', 6),
(2, 'AMD RX Vega-56', 8),
(3, 'NVIDIA GeForce GTX 1660 Ti', 6),
(4, 'AMD Radeon Pro VII', 16),
(5, 'AMD Radeon RX 6900 XT', 16);

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `authdata`
--
ALTER TABLE `authdata`
  ADD PRIMARY KEY (`idAuth`);

--
-- Индексы таблицы `computers`
--
ALTER TABLE `computers`
  ADD PRIMARY KEY (`idComp`),
  ADD KEY `idTariff` (`idTypeTariffs`),
  ADD KEY `idProcessor` (`idProcessor`),
  ADD KEY `idVideo_card` (`idVideo_card`),
  ADD KEY `idMonitor` (`idMonitor`),
  ADD KEY `idKeyboard` (`idKeyboard`),
  ADD KEY `idMouse` (`idMouse`);

--
-- Индексы таблицы `keyboards`
--
ALTER TABLE `keyboards`
  ADD PRIMARY KEY (`idKeyboard`);

--
-- Индексы таблицы `mice`
--
ALTER TABLE `mice`
  ADD PRIMARY KEY (`idMouse`);

--
-- Индексы таблицы `monitors`
--
ALTER TABLE `monitors`
  ADD PRIMARY KEY (`idMonitor`);

--
-- Индексы таблицы `processors`
--
ALTER TABLE `processors`
  ADD PRIMARY KEY (`idProcessor`);

--
-- Индексы таблицы `sessions`
--
ALTER TABLE `sessions`
  ADD PRIMARY KEY (`idSession`),
  ADD KEY `idUser` (`idUser`),
  ADD KEY `idComputer` (`idComputer`),
  ADD KEY `sessions_ibfk_2` (`idTariff`);

--
-- Индексы таблицы `tariff`
--
ALTER TABLE `tariff`
  ADD PRIMARY KEY (`idTariff`),
  ADD KEY `idTypeTariffs` (`idTypeTariffs`);

--
-- Индексы таблицы `tariff_allowance`
--
ALTER TABLE `tariff_allowance`
  ADD PRIMARY KEY (`idAllowance`);

--
-- Индексы таблицы `type_tariffs`
--
ALTER TABLE `type_tariffs`
  ADD PRIMARY KEY (`idTypeTariffs`);

--
-- Индексы таблицы `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`idUser`),
  ADD KEY `idAuth` (`idAuth`);

--
-- Индексы таблицы `video_cards`
--
ALTER TABLE `video_cards`
  ADD PRIMARY KEY (`idVideo_card`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `authdata`
--
ALTER TABLE `authdata`
  MODIFY `idAuth` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=33;

--
-- AUTO_INCREMENT для таблицы `keyboards`
--
ALTER TABLE `keyboards`
  MODIFY `idKeyboard` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `mice`
--
ALTER TABLE `mice`
  MODIFY `idMouse` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `monitors`
--
ALTER TABLE `monitors`
  MODIFY `idMonitor` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `processors`
--
ALTER TABLE `processors`
  MODIFY `idProcessor` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `sessions`
--
ALTER TABLE `sessions`
  MODIFY `idSession` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- AUTO_INCREMENT для таблицы `tariff`
--
ALTER TABLE `tariff`
  MODIFY `idTariff` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT для таблицы `tariff_allowance`
--
ALTER TABLE `tariff_allowance`
  MODIFY `idAllowance` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT для таблицы `type_tariffs`
--
ALTER TABLE `type_tariffs`
  MODIFY `idTypeTariffs` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `users`
--
ALTER TABLE `users`
  MODIFY `idUser` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;

--
-- AUTO_INCREMENT для таблицы `video_cards`
--
ALTER TABLE `video_cards`
  MODIFY `idVideo_card` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `computers`
--
ALTER TABLE `computers`
  ADD CONSTRAINT `computers_ibfk_1` FOREIGN KEY (`idTypeTariffs`) REFERENCES `type_tariffs` (`idTypeTariffs`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `computers_ibfk_2` FOREIGN KEY (`idProcessor`) REFERENCES `processors` (`idProcessor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `computers_ibfk_3` FOREIGN KEY (`idVideo_card`) REFERENCES `video_cards` (`idVideo_card`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `computers_ibfk_4` FOREIGN KEY (`idMonitor`) REFERENCES `monitors` (`idMonitor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `computers_ibfk_5` FOREIGN KEY (`idKeyboard`) REFERENCES `keyboards` (`idKeyboard`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `computers_ibfk_6` FOREIGN KEY (`idMouse`) REFERENCES `mice` (`idMouse`) ON DELETE RESTRICT ON UPDATE RESTRICT;

--
-- Ограничения внешнего ключа таблицы `sessions`
--
ALTER TABLE `sessions`
  ADD CONSTRAINT `sessions_ibfk_1` FOREIGN KEY (`idUser`) REFERENCES `users` (`idUser`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `sessions_ibfk_2` FOREIGN KEY (`idTariff`) REFERENCES `tariff` (`idTariff`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `sessions_ibfk_3` FOREIGN KEY (`idComputer`) REFERENCES `computers` (`idComp`) ON DELETE RESTRICT ON UPDATE RESTRICT;

--
-- Ограничения внешнего ключа таблицы `tariff`
--
ALTER TABLE `tariff`
  ADD CONSTRAINT `tariff_ibfk_1` FOREIGN KEY (`idTypeTariffs`) REFERENCES `type_tariffs` (`idTypeTariffs`) ON DELETE RESTRICT ON UPDATE RESTRICT;

--
-- Ограничения внешнего ключа таблицы `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `users_ibfk_1` FOREIGN KEY (`idAuth`) REFERENCES `authdata` (`idAuth`) ON DELETE RESTRICT ON UPDATE RESTRICT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
