Create Database ThermoDB
	create Table Messdaten(
	MessNR int primary key identity(0,1) Not null ,
	Datum datetime not null,
	Temperatur numeric (4,2) not null,
	)
