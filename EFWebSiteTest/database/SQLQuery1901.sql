use SitoWeb
go

--Query nuova
--13  Seleziona i primi tre prodotti più costosi di ogni brand, ordinati per brand e prezzo
--    Nome brand | Nome prodotto | Prezzo

--cross join
select Brand.BrandName, t2.Name as ProductName,  t2.Price from Brand cross apply 
(
	select top(3) * from Product
	where Brand.Id = Product.BrandId
	order by Product.Price desc
) t2

--v1 
;WITH t1 AS (
SELECT *, ROW_NUMBER() OVER (PARTITION BY Product.BrandId ORDER BY Product.Price DESC) AS rowN
FROM Product 
)
SELECT Brand.BrandName ,t1.Name as ProductName,t1.Price
FROM t1
inner join Brand on t1.BrandId = Brand.Id
WHERE rowN < 4 


--v2 
select Brand.BrandName ,t1.Name as ProductName, t1.Price from 
(
select *,ROW_NUMBER() OVER (PARTITION BY Product.BrandId ORDER BY Product.Price DESC) AS rowN
from Product 
) as t1
inner join Brand on t1.BrandId = Brand.Id
where rowN < 4 


--v 3
select * from(
select Brand.BrandName ,Product.Name as ProductName, Product.Price , 
ROW_NUMBER() OVER (PARTITION BY Product.BrandId ORDER BY Product.Price DESC) AS rowN
from Product 
inner join Brand on Product.BrandId = Brand.Id) as result
where rowN < 4 



--Paging--------------------------------------------------------------------------------------
use SitoWeb
go

--v1 OFFSET FETCH NEXT 

IF EXISTS (SELECT 1 FROM sys.procedures WHERE [name] = 'Paging')
BEGIN
DROP PROCEDURE dbo.Paging;
END;
GO

Create Procedure Paging
(
@pageDimension int,
@pageNum int,
@category int,
@orderBy tinyint
)
as
	begin
		select * from Product
		order by 
		case 
			when @orderBy=1 then Product.Name
			when @orderBy=2 then Product.Price
		end asc,
		case when @orderBy=3 then Product.price end desc
		OFFSET     @pageNum * @pageDimension ROWS       
		FETCH NEXT @pageDimension ROWS ONLY; 
	end
go

exec dbo.Paging @pageDimension=30, @pageNum=1,@category=1,@orderBy=3
go

--v2 paging ROW_NUMBER()
use SitoWeb
go

IF EXISTS (SELECT 1 FROM sys.procedures WHERE [name] = 'PagingOld')
BEGIN
DROP PROCEDURE dbo.PagingOld;
END;
GO

--pagenum must be > 1
Create Procedure PagingOld
(
@pageDimension int,
@pageNum int,
@category int = 0,
@orderBy tinyint = 0
)
as
begin
	set @pageNum -=1 

	if (@pageNum < 0)
		throw 51000,'page num must be > 0' ,1
	if (@pageDimension < 0)
		throw 51000,'pageDimension cannot be negative' ,1
	if (@orderBy > 3 or @orderBy <0)
		throw 51000, 'orderby must be between 0 and 3',1
	

	;WITH t1 AS 
	 (SELECT *
	   , ROW_NUMBER() OVER (ORDER BY 
		case 
		when @orderBy=1 then Product.Name
		when @orderBy=2 then Product.Price
		end asc,
		case when @orderBy=3 then Product.price end desc
   
	   ) AS RowNumber
	   from Product 
	   inner join ProductCategory on Product.Id = ProductCategory.IdProduct
	   where @category = 0 or ProductCategory.IdCategory = @category    
	 )
	SELECT *
	FROM t1
	WHERE RowNumber > @pageNum * @pageDimension - 1 AND RowNumber <= @pageNum * @pageDimension + @pageDimension - 1
	order by RowNumber
end
go

exec dbo.PagingOld @pageDimension=30, @pageNum=1,@category=0,@orderBy=3
go

exec dbo.Paging @pageDimension=30, @pageNum=0,@category=0,@orderBy=3
go



------------------------------------
--Restituire il seguente elenco prodotti con ordine Custom:
--* come primi, i 20 più recenti,
--* come seconda fascia i primi 100 prodotti con più richieste informazioni ricevute
--* come terza fascia, i 10 con prezzo compreso tra 200 - 500€
--* come quarta fascia, i 100 con nessuna richiesta informazione

--v1 Function
IF EXISTS (SELECT 1 FROM sys.objects WHERE [name] = 'CustomOrder' AND [type] = 'TF')
BEGIN
DROP FUNCTION dbo.CustomOrder;
END;
GO

Create Function CustomOrder ()
returns
 @tempProduct table
(
	Id int ,
	BrandId int ,
	Name nvarchar(50) ,
	ShortDescription nvarchar(20),
	Price decimal(19,4),
	Description nvarchar(50)
)
as   
begin

insert into @tempProduct 
select top(20) * from Product
order by Product.Id desc

insert into @tempProduct values (null,null,null,null,null,null)

insert into @tempProduct
select top(10) * from Product
where Product.Price > 10 and Product.Price < 15

insert into @tempProduct values (null,null,null,null,null,null)


insert into @tempProduct
select top(100) Product.Id, BrandId,Product.Name,ShortDescription,Price,Description from Product
inner join InfoRequest on InfoRequest.ProductId = Product.Id
group by Product.Id, BrandId,Product.Name,ShortDescription,Price,Description
order by count(InfoRequest.Id)  desc

insert into @tempProduct values (null,null,null,null,null,null)


insert into @tempProduct
select top(100) Product.Id,BrandId,Product.Name,ShortDescription,Price,Description from Product
left join InfoRequest on InfoRequest.ProductId = Product.Id
where InfoRequest.ProductId is null

return
end
go

select * from CustomOrder()
go

--v2 Query con Union
select * from (
select top(20) * from Product
order by Product.Id desc) as t1
UNION ALL

select * from (
select top(10) * from Product
where Product.Price > 10 and Product.Price < 15
) as t2

UNION ALL

select * from (
select top(100) Product.Id, BrandId,Product.Name,ShortDescription,Price,Description from Product
inner join InfoRequest on InfoRequest.ProductId = Product.Id
group by Product.Id, BrandId,Product.Name,ShortDescription,Price,Description
order by count(InfoRequest.Id)  desc
) as t3

UNION ALL
select * from (
select top(100) Product.Id,BrandId,Product.Name,ShortDescription,Price,Description from Product
left join InfoRequest on InfoRequest.ProductId = Product.Id
where InfoRequest.ProductId is null
) as t4

--batches di allineamento

--allineamento prodotti senza categorie
declare @Id int

DECLARE MyCursor2 CURSOR FOR SELECT Id FROM  
Product left join ProductCategory on Product.Id = ProductCategory.IdProduct
where ProductCategory.IdProduct is null

OPEN MyCursor2
FETCH NEXT FROM MyCursor2 INTO @Id

WHILE @@FETCH_STATUS = 0 
begin
	insert into ProductCategory(IdProduct,IdCategory) values (@id,(SELECT TOP 1 Id FROM Category ORDER BY NEWID()))
	FETCH NEXT FROM MyCursor2 INTO @id
end
CLOSE MyCursor2
DEALLOCATE MyCursor2

go



--individuare gli utenti guest e registrarli al sito come account utenti

declare @Name nvarchar(50)
declare @LastName nvarchar(50)
declare @Email nvarchar(50)
declare @AccountId int
declare @UserId int

DECLARE MyCursor3 CURSOR FORWARD_ONLY FOR SELECT [Name],LastName, Email FROM 
InfoRequest where UserId is null
OPEN MyCursor3
FETCH NEXT FROM MyCursor3 INTO @Name,@LastName,@Email

WHILE @@FETCH_STATUS = 0  
begin
	if  NOT EXISTS( select top(1)* from Account where Email = @Email ) --request of transforming guest user into registered user
	begin
		insert into Account values (@Email,'',1)
		
		--Update User
		select @AccountId = Account.Id from Account WHERE Account.Email = @Email
		insert into [User] values(@AccountId,@Name,@LastName)
		select @UserId = u.Id from [User] as u WHERE u.AccountId = @AccountId
		
		--Update InfoRequest
		Update InfoRequest set UserId = @UserId  where Email = @Email

	end
	else
		Update InfoRequest set UserId = @UserId  where Email = @Email

	--- utente guest ma con mail registrata
	FETCH NEXT FROM MyCursor3 INTO @Name,@LastName,@Email
end
CLOSE MyCursor3
DEALLOCATE MyCursor3

go
