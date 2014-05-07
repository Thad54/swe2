Insert into Contact(FirstName, UID , CreationDate, Address, BillingAddress, DeliveryAddress)
	values('Musterfirma', 123456789, '12-9-1990', 'Firmengasse 7', 'Rechenstrasse 4', 'Paketweg 89');

Insert into Contact(FirstName, UID , CreationDate, Address, BillingAddress, DeliveryAddress)
	values('Vorpis', 123456789, '8-7-1995', 'Hohlgasse 13', 'Reichengasse 8', 'Huberstrasse 5');		

Insert into Contact(FirstName, LastName, Company_FK, Title, Suffix, CreationDate, Address, BillingAddress, DeliveryAddress)
	values('Max', 'Musterman', 0, 'Mag.', 'BA', '12-9-1980', 'Sponheimerstrasse 10', 'Müllerstraße 8', 'Lieferstrasse 12');
	
Insert into Contact(FirstName, Lastname, Title, Suffix, CreationDate, Address, BillingAddress, DeliveryAddress)
	values('Maxine', 'Musterman', 'Dr.', 'MA', '10-7-1976', 'Heimstrasse 5', 'Rechnungsgasse 8', 'Lieferweg 17');
	
Insert into Bill([DATE], DueBy, Comment, Message, Contact_FK)	
	Values(GETDATE(),DATEADD(DAY, 60 ,GETDATE()), 'Kommentar', 'Nachricht', 1);
	
Insert into BillingPosition(Name, Price, Tax, Amount, Bill_FK)
	values('Produkt 1', 100, 1.20, 10, 0);
	
Insert into BillingPosition(Name, Price, Tax, Amount, Bill_FK)
	values('Produkt 2', 200, 1.10, 30, 0);
	
	