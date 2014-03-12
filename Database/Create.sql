Create Database MicroERP
GO

use MicroERP
Go

Create Table Contact(
	CNT_ID int identity(0,1) Primary Key Not null,
	Title			nvarchar(20),
	FirstName 		nvarchar(40),
	LastName  		nvarchar(40),
	[UID]			nvarchar(20),
	Suffix			nvarchar(20),
	CreationDate	date,
	Address			nvarchar(50),
	BillingAddress	nvarchar(50),
	DeliveryAddress	nvarchar(50),
	Company_FK		int
)
/*
Create Table Company(
	CMP_ID int identity(0,1) Primary Key Not null,
	Title			nvarchar(20),
	Name 			nvarchar(40),
	UID		  		int,
	Suffix			nvarchar(20),
	OpeningDate		date,
	Address			nvarchar(50),
	BillingAddress	nvarchar(50),
	DeliveryAddress	nvarchar(50)
)
*/

Create Table Bill(
	BLL_ID int identity(0,1) Primary Key Not null,
	[Date]				datetime,
	DueBy				date,
	[Comment]			nvarchar(300),
	Message				nvarchar(300),
	Contact_FK			int
)

Create Table BillingPosition(
	BLP_ID int identity(0,1) Primary Key Not null,
	Name		nvarchar(30),
	Price		money,
	Tax			numeric(4,2),
	Amount 		int,
	Bill_FK		int
)


ALTER TABLE Contact
ADD CONSTRAINT Contact_Contact_FK FOREIGN KEY ( Company_FK ) REFERENCES Contact (CNT_ID);

ALTER TABLE BillingPosition
ADD CONSTRAINT BillingPosition_Bill_FK FOREIGN KEY ( Bill_FK ) REFERENCES Bill (BLL_ID);

Alter Table Bill
ADD CONSTRAINT Bill_Contact_FK FOREIGN KEY ( Contact_FK ) REFERENCES Contact (CNT_ID);