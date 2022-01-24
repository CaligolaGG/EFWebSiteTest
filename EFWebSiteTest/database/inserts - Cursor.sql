use SitoWeb

--clearing tables and resetting primary key index-------------------------------------------------------------------------------
IF(not EXISTS(SELECT top (1) * FROM dbo.InfoRequestReply))
begin DBCC CHECKIDENT ('InfoRequestReply', RESEED , 1) end
else
begin
DELETE FROM InfoRequestReply
DBCC CHECKIDENT ('InfoRequestReply', RESEED , 0)
end

IF(not EXISTS(SELECT top (1) * FROM dbo.InfoRequest))
begin DBCC CHECKIDENT ('InfoRequest', RESEED , 1) end
else
begin
DELETE FROM InfoRequest
DBCC CHECKIDENT ('InfoRequest', RESEED , 0)
end

truncate table ProductCategory

DELETE FROM Category
DBCC CHECKIDENT ('Category', RESEED , 0)

IF(not EXISTS(SELECT top (1) * FROM dbo.Nation))
begin
DBCC CHECKIDENT ('Nation', RESEED , 1)
end
else
begin
DELETE FROM Nation
DBCC CHECKIDENT ('Nation', RESEED , 0)
end

IF(not EXISTS(SELECT top (1) * FROM dbo.Product))
begin DBCC CHECKIDENT ('Product', RESEED , 1) end
else
begin
DELETE FROM Product
DBCC CHECKIDENT ('Product', RESEED , 0)
end

IF(not EXISTS(SELECT top (1) * FROM dbo.Brand))
begin DBCC CHECKIDENT ('Brand', RESEED , 1) end
else
begin
DELETE FROM Brand
DBCC CHECKIDENT ('Brand', RESEED , 0)
end


IF(not EXISTS(SELECT top (1) * FROM dbo.[User]))
begin DBCC CHECKIDENT ('User', RESEED , 1) end
else
begin
DELETE FROM [User]
DBCC CHECKIDENT ('User', RESEED , 0)
end


IF(not EXISTS(SELECT top (1) * FROM dbo.Account))
begin DBCC CHECKIDENT ('Account', RESEED , 1) end
else
begin
DELETE FROM Account
DBCC CHECKIDENT ('Account', RESEED , 0)
end


--Inserts ----------------------------------------------------------------------------------------------------------------
--account, user, brands

Declare @USERNUM int = 50
Declare @BRANDNUM int = 50

Declare @counter int = 1
Declare @mail nvarchar(50)
Declare @password nvarchar(50)


While @counter <= @USERNUM
begin
	set @mail = CONVERT(nvarchar(50), NEWID())
	set @password  = CONVERT(nvarchar(50), NEWID())
	insert into Account (Email,Password,AccountType) values (@mail, @password, 1)
	insert into [User] (AccountId, Name,LastName ) values(@counter,  CONVERT(nvarchar(50), NEWID()), CONVERT(nvarchar(50), NEWID()))
	set @counter += 1
end
While @counter <= @USERNUM + @BRANDNUM
begin
	set @mail  = CONVERT(nvarchar(50), NEWID())
	set @password = CONVERT(nvarchar(50), NEWID())
	insert into Account (Email,Password,AccountType) values (@mail, @password, 2)
	insert into Brand (AccountId,BrandName,Description) values(@counter,  CONVERT(nvarchar(50), NEWID()), CONVERT(nvarchar(50), NEWID()))
	set @counter += 1
end
SET NOCOUNT OFF
go

 --Prodotti
declare @counter2 int
declare @Id int
declare @rndNum int

-- Declare Cursor
DECLARE MyCursor CURSOR FORWARD_ONLY FOR SELECT Id FROM [dbo].Brand
-- Open the Cursor
OPEN MyCursor
-- Fetch the next record from the cursor
FETCH NEXT FROM MyCursor INTO @Id  

-- Set the status for the cursor
WHILE @@FETCH_STATUS = 0  
begin
	set @counter2 = 1
	select @rndNum = ROUND((RAND()*50+1),10)
	while (@counter2<=@rndNum)
		begin
			insert into Product (BrandId, Name, ShortDescription, Price, Description)
			values (@Id, CONVERT(varchar(50), NEWID()) ,'short description',ROUND((RAND()*19+1),0),CONVERT(varchar(50), NEWID()))
			set @counter2 += 1
		end
	FETCH NEXT FROM MyCursor INTO @Id  
end
CLOSE MyCursor
DEALLOCATE MyCursor
go

--Categoria
Declare @NUMCATEGORIES int = 20
Declare @counter3 int = 0

While @counter3 < @NUMCATEGORIES
begin
	insert into Category values(CONVERT(varchar(50), NEWID()))
	set @counter3 += 1
end
go


--Categoria Prodotto

declare @NUMCATEGORIES int = 20

declare @counter4 int
declare @catId int
declare @idColumn int
declare @rndNum int
declare @count int


DECLARE MyCursor CURSOR FORWARD_ONLY FOR SELECT Id FROM [dbo].Product
OPEN MyCursor
FETCH NEXT FROM MyCursor INTO @idColumn  

WHILE @@FETCH_STATUS = 0  
begin
	set @counter4 = 1
	Set @catId = 1
	select @rndNum = ROUND((RAND()*5),0)
	while (@counter4<=@rndNum)
	begin
		if(@catId > @NUMCATEGORIES)
			set @catId = 1
		--solving duplicated keys errors 
		IF(NOT EXISTS(SELECT top(1) * FROM ProductCategory where ProductCategory.IdCategory = @catId and ProductCategory.IdProduct = @idColumn))
			insert into ProductCategory
			values (@idColumn, @catId)
		----------
		set @counter4+=1
		set @catId += 1
	end
	FETCH NEXT FROM MyCursor INTO @idColumn  
end
CLOSE MyCursor
DEALLOCATE MyCursor
go

--Nations
Declare @counter int
set @counter=1
while (@counter<=20)
begin
	insert into Nation(Name)
	values('Nation '+ CAST(@counter as varchar(2)))
	set @counter+=1
end
go


--inforequest

declare @idColumn int
declare @counter2 int
declare @rndNum int 

DECLARE MyCursor CURSOR FORWARD_ONLY FOR SELECT Id FROM [dbo].Product
OPEN MyCursor
FETCH NEXT FROM MyCursor INTO @idColumn  

WHILE @@FETCH_STATUS = 0  
begin
	set @counter2 = 1
	select @rndNum = ROUND((RAND()*10),0)
	while (@counter2<=@rndNum)
	begin

		--temp user creation
		declare @userId int
		select @userId=ROUND((RAND()*49+1),0)
		declare @TempUser TABLE(
			Id int ,
			AccountId int,
			Name nvarchar(50),
			LastName nvarchar(50)
		)
		insert into @TempUser select * from [User] where Id=@userId
		declare @userName nvarchar(50)
		select @userName=Name From [User] where Id=@userId
		declare @userLastName nvarchar(50)
		select @userLastName=LastName From [User] where Id=@userId
		---------------

		if(@counter2 = 1)
			insert into InfoRequest
			values (null,@idColumn,CONVERT(nvarchar(50), NEWID()), CONVERT(nvarchar(50), NEWID()),CONVERT(nvarchar(50), NEWID()),CONVERT(varchar(50), NEWID()),  ROUND((RAND()*19+1),0),
			'12345678901234', '12345',CONVERT(nvarchar(50), NEWID()),DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530),0))
		else
			insert into InfoRequest
			values (@userId,@idColumn,@userName, @userLastName,CONVERT(nvarchar(50), NEWID()),CONVERT(nvarchar(50), NEWID()),  ROUND((RAND()*19+1),0),
			'12345678901234', '12345',CONVERT(nvarchar(50), NEWID()),DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530),0))
				
		set @counter2 += 1
	end
	FETCH NEXT FROM MyCursor INTO @idColumn  
end
CLOSE MyCursor
DEALLOCATE MyCursor
SET NOCOUNT OFF
go


--inforequest replies
declare @NUMACCOUNT int =50

declare @idColumn int
declare @userId int
declare @counter int
declare @rndNum int
declare @prodId int
declare @accountId int
declare @brandId int

DECLARE MyCursor CURSOR FORWARD_ONLY FOR SELECT Id, InfoRequest.UserId, InfoRequest.ProductId FROM [dbo].InfoRequest
OPEN MyCursor
FETCH NEXT FROM MyCursor INTO @idColumn ,@userId, @prodId

WHILE @@FETCH_STATUS = 0  
begin
	set @counter = 1

	select @rndNum = ROUND((RAND()*2),0)+1
	
	if (@userId is null )
		begin
			set @accountId = null
		end
	else
		begin
			select @accountId =[User].AccountId from [User] where [User].Id = @userId
		end

	select @brandId = Product.BrandId from Product where Product.Id = @prodId
	select @brandId = Brand.AccountId from Brand where Brand.Id = @brandId
	while (@counter<=@rndNum)
		begin
			declare @rndNum2 int
			set @rndNum2 = ROUND((RAND()*1),0)
			if(@rndNum2 = 0) --user reply
				insert into InfoRequestReply 
				values (@idColumn, @accountId, CONVERT(nvarchar(50), NEWID()),DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530),0)  )
			else			--brand reply
				insert into InfoRequestReply
				values (@idColumn, @brandId , CONVERT(nvarchar(50), NEWID()),DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530),0)  )
			set @counter+=1
		end
FETCH NEXT FROM MyCursor INTO @idColumn ,@userId, @prodId
end
CLOSE MyCursor
DEALLOCATE MyCursor
SET NOCOUNT OFF
go



