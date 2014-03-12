Insert into Contact(FirstName, UID)
	values('Musterfirma', 123456789);	

Insert into Contact(FirstName, LastName, Company_FK)
	values('Max', 'Musterman', 0);
	
Insert into Contact(FirstName, Lastname)
	values('Maxine', 'Musterman');
	
Insert into Bill([DATE], DueBy, Comment, Message, Contact_FK)	
	Values(GETDATE(),DATEADD(DAY, 60 ,GETDATE()), 'Kommentar', 'Nachricht', 1);
	
Insert into BillingPosition(Name, Price, Tax, Amount, Bill_FK)
	values('Produkt 1', 100, 1.20, 10, 0);
	
Insert into BillingPosition(Name, Price, Tax, Amount, Bill_FK)
	values('Produkt 2', 200, 1.10, 30, 0);
	
	