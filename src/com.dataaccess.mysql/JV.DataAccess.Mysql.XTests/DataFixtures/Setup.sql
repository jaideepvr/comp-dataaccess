-- Create user table
CREATE TABLE `user` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT,
  `user_name` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  `first_name` varchar(45) DEFAULT NULL,
  `last_name` varchar(45) DEFAULT NULL,
  `date_of_birth` date NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

INSERT
  INTO `user`
       (user_name, password, first_name, last_name, date_of_birth)
VALUES ('jaideep', 'a', 'Jaideep', 'Vinakota', '1972-06-08');
INSERT
  INTO `user`
       (user_name, password, first_name, last_name, date_of_birth)
VALUES ('sridevi', 'a', 'Sridevi', 'Vinakota', '1973-01-10');

CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_InsertUser`(
	in  p_userName		varchar(45),
	in  p_firstName		varchar(45),
	in  p_lastName		varchar(45),
	in  p_password		varchar(45),
	in  p_date_of_birth date
)
BEGIN
	INSERT
      INTO `user`
		   (user_name, password, first_name, last_name, date_of_birth)
	VALUES (p_userName, p_password, p_firstName, p_lastName, p_date_of_birth);
END;

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetUsers`()
BEGIN
   SELECT usr.user_id, usr.user_name, usr.first_name, usr.last_name, date_of_birth
     FROM `user` usr;
END;

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetUserMultipleViews`()
BEGIN
   SELECT usr.user_id, usr.user_name, usr.first_name, usr.last_name, date_of_birth
     FROM `user` usr;

   SELECT usr.user_id, usr.user_name, usr.first_name, usr.last_name, date_of_birth
     FROM `user` usr
	LIMIT 1;
END;
