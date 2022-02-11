USE master 
go

--ALTER DATABASE SitoWeb SET SINGLE_USER WITH ROLLBACK IMMEDIATE
--DROP DATABASE SitoWeb

create database SitoWeb
go

use SitoWeb
go

--IF EXISTS
--(SELECT 1 FROM sys.tables WHERE [Name] = 'Account')
--BEGIN
--DROP TABLE dbo.Account;
--END;

Create table Account
(
	Id int primary key identity,
	Email nvarchar(50) not null unique,
	Password nvarchar(50) not null,
	AccountType tinyint  NOT NULL
)
go

Create table Brand
(
	Id int primary key identity,
	AccountId int not null,
	BrandName nvarchar(50) unique not null,
	Description nvarchar(50),
)
go

ALTER TABLE Brand
ADD CONSTRAINT FK_Account_Brand
FOREIGN KEY (AccountId) REFERENCES Account(Id)
go

Create table [User]
(
	Id integer primary key identity,
	AccountId int not null,
	Name nvarchar(50) not null ,
	LastName nvarchar(50) not null,
)
go

ALTER TABLE [User]
ADD CONSTRAINT FK_Account_User
FOREIGN KEY (AccountId) REFERENCES Account(Id);
go

Create table Product
(
	Id int primary key identity,
	BrandId int not null,
	Name nvarchar(50) not null,
	ShortDescription nvarchar(20),
	Price decimal(19,4) not null,
	Description nvarchar(50)
)
go

ALTER TABLE Product
ADD CONSTRAINT FK_Brand_Product
FOREIGN KEY (BrandId) REFERENCES Brand(id)
go

Create table Category
(
	Id int primary key identity,
	Name varchar(50) not null
)
go

Create table ProductCategory
(
	IdProduct int not null ,
	IdCategory int not null ,
	primary key (IdProduct,IdCategory)
)
go

ALTER TABLE ProductCategory
ADD CONSTRAINT FK_Product_ProductCategory
FOREIGN KEY (IdProduct) REFERENCES Product(Id)
go

ALTER TABLE ProductCategory
ADD CONSTRAINT FK_Category_ProductCategory
FOREIGN KEY (IdCategory) REFERENCES Category(Id)
go


Create table Nation
(
	Id integer primary key identity,
	Name varchar(50) not null
)
go

Create table InfoRequest
(
	Id integer primary key identity,
	UserId int null,
	ProductId int not null ,
	Name nvarchar(50),
	LastName nvarchar(50),
	Email nvarchar(50) ,
	City varchar(50),
	NationId int  not null, 
	PhoneNumber char(15) not null,
	Cap char(5),
	RequestText nvarchar(50),
	InsertDate Datetime
)
go

ALTER TABLE InfoRequest
ADD CONSTRAINT FK_User_InfoRequest
FOREIGN KEY (UserId) REFERENCES [User](Id)
go

ALTER TABLE InfoRequest
ADD CONSTRAINT FK_Prodotto_InfoRequest
FOREIGN KEY (ProductId) REFERENCES Product(Id)
go

ALTER TABLE InfoRequest
ADD CONSTRAINT FK_Nation_InfoRequest
FOREIGN KEY (NationId) REFERENCES Nation(Id)
go


Create table InfoRequestReply
(
	Id int primary key identity,
	InfoRequestId int  not null,
	AccountId int  null,
	ReplyText nvarchar(50),
	InsertDate datetime
)
go

ALTER TABLE InfoRequestReply
ADD CONSTRAINT FK_InfoRequest_InfoRequestReply
FOREIGN KEY (InfoRequestId) REFERENCES InfoRequest(Id)
go

ALTER TABLE InfoRequestReply
ADD CONSTRAINT FK_Account_InfoRequestReply
FOREIGN KEY (AccountId) REFERENCES Account(Id)
go

ALTER TABLE [User]  Add  isDeleted bit  default 0 not null
ALTER TABLE brand  Add  isDeleted bit  default 0 not null
ALTER TABLE Account  Add  isDeleted bit  default 0 not null
ALTER TABLE Product  Add  isDeleted bit  default 0 not null
ALTER TABLE Nation  Add  isDeleted bit  default 0 not null
ALTER TABLE Category Add  isDeleted bit  default 0 not null
ALTER TABLE ProductCategory  Add  isDeleted bit  default 0 not null
ALTER TABLE InfoRequest  Add  isDeleted bit  default 0 not null
ALTER TABLE InfoRequestReply  Add  isDeleted bit  default 0 not null



--Indexes

IF EXISTS (SELECT 1 FROM sys.indexes WHERE [name] = 'IX_NC_InfoRequestReply_InfoRequestId')
BEGIN
DROP INDEX IX_NC_InfoRequestReply_InfoRequestId ON dbo.InfoRequestReply;
END;

CREATE NONCLUSTERED INDEX IX_NC_InfoRequestReply_InfoRequestId ON
dbo.InfoRequestReply(InfoRequestId);
GO


IF EXISTS (SELECT 1 FROM sys.indexes WHERE [name] = 'IX_NC_InfoRequest_ProductId')
BEGIN
DROP INDEX IX_NC_InfoRequest_ProductId ON dbo.InfoRequest;
END;

CREATE NONCLUSTERED INDEX IX_NC_InfoRequest_ProductId ON
dbo.InfoRequest(ProductId);
GO


IF EXISTS (SELECT 1 FROM sys.indexes WHERE [name] = 'IX_NC_InfoRequestReply_InsertDate')
BEGIN
DROP INDEX IX_NC_InfoRequestReply_InsertDate ON dbo.InfoRequestReply;
END;


CREATE NONCLUSTERED INDEX IX_NC_InfoRequestReply_InsertDate ON
dbo.InfoRequestReply(InsertDate);
GO


IF EXISTS (SELECT 1 FROM sys.indexes WHERE [name] = 'IX_NC_Product_Brand')
BEGIN
DROP INDEX IX_NC_Product_Brand ON dbo.Product;
END;


CREATE NONCLUSTERED INDEX IX_NC_Product_Brand ON
dbo.Product(BrandId);
GO

















--appunti

--other operations on indexes : DISABLE,REBUILD,REORGANIZE

--ALTER INDEX IX_C_ContactAddresses_ContactIdPostcode ON dbo.ContactAddresses DISABLE;

--ALTER INDEX IX_C_ContactAddresses_ContactIdPostcode ON dbo.ContactAddresses
--REBUILD;
--ALTER INDEX ALL ON dbo.ContactAddresses REBUILD;


--ALTER INDEX IX_C_ContactAddresses_ContactIdPostcode ON dbo.ContactAddresses REORGANIZE;
--ALTER INDEX ALL ON dbo.ContactAddresses REORGANIZE;