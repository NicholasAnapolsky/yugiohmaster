CREATE TABLE Managers
(
	ManagerID int IDENTITY (1, 1) NOT NULL,
	LastName varchar(30) NOT NULL,
	FirstName varchar(30) NOT NULL,
	Email varchar(50) NOT NULL,
	[Password] varchar(50) NOT NULL
);