Alter Table Messdaten
	alter column 
		Temperatur numeric(5,2) null

Alter Table Messdaten
	alter column
		Datum datetime null

go

insert into Messdaten (Temperatur, Datum)
	select null, null 
go 10000 

update Messdaten 
	set Temperatur = ((ABS((CHECKSUM(NEWID()) % 10000)) / 100) +  20)% 40 

Update Messdaten
	set Datum = DATEADD(day, (ABS(CHECKSUM(NEWID())) % 3653), '2003-01-01')

go

Alter Table Messdaten
	alter column 
		Temperatur numeric(5,2) not null

Alter Table Messdaten
	alter column
		Datum datetime not null
go

Select * from Messdaten