Insert into Contact(FirstName, UID )
	values('Musterfirma', 123456789);	

Insert into Contact(FirstName, LastName, Company_FK, Title, Suffix, CreationDate, Address, BillingAddress, DeliveryAddress)
	values('Max', 'Musterman', 0, 'Mag.', ' ', '12-9-1980', 'Sponheimerstraﬂe 10', 'M¸llerstraﬂe 8', 'Lieferstraﬂe 12');
	
Insert into Contact(FirstName, Lastname, Title, Suffix, CreationDate, Address, BillingAddress, DeliveryAddress)
	values('Maxine', 'Musterman', 'Dr.', ' ', '10-7-1976', 'Heimstraﬂe 5', 'Rechnungsgasse 8', 'Lieferweg 17');
	
Insert into Bill([DATE], DueBy, Comment, Message, Contact_FK)	
	Values(GETDATE(),DATEADD(DAY, 60 ,GETDATE()), 'Kommentar', 'Nachricht', 1);
	
Insert into BillingPosition(Name, Price, Tax, Amount, Bill_FK)
	values('Produkt 1', 100, 1.20, 10, 0);
	
Insert into BillingPosition(Name, Price, Tax, Amount, Bill_FK)
	values('Produkt 2', 200, 1.10, 30, 0);
	
	