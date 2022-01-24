use SitoWeb

--1 Numero delle richieste informazioni raccolte per Prodotto
Select  Product.Id , Product.Name, Count (*) as number from Product
inner Join InfoRequest
on InfoRequest.ProductId = Product.Id
group by Product.Id,  Product.Name
Order by Product.Id


--2 Numero delle richieste informazioni raccolte per Brand------------------------------------------------------------------------
Select  BrandId , Count(*) as number  from  Product
inner Join InfoRequest on InfoRequest.ProductId = Product.Id
--inner Join Brand on Brand.Id = Product.BrandId
group by BrandId
order by BrandId

--3 Numero delle richieste informazioni raccolte per Prodotto riportando anche il nome del Brand--------------------------------
-- Nome Brand | Nome Prodotto | Num Richieste

select Brand.BrandName, Product.Name,Count(ProductId) from Product 
inner join InfoRequest on InfoRequest.ProductId = Product.Id
inner join Brand on Product.BrandId = Brand.Id
group by Brand.Id,Brand.BrandName,Product.Name
order by Brand.BrandName

--4 Numero dei prodotti ordinati per Categoria di ciascun Brand:---------------------------------------------------------------
-- Nome Brand | Nome Categoria | Numero Prodotti

select Brand.BrandName, Category.Name as CategoryName, count(Product.Id) as ProductsNumber from Category
inner join ProductCategory on Category.Id = ProductCategory.IdCategory
inner join Product on ProductCategory.IdProduct = Product.Id
inner join Brand on Brand.Id = Product.BrandId
group by Brand.BrandName, Category.Name
order by BrandName,ProductsNumber



--5 Elenco dei prodotti con più di una categoria associata---------------------------------------------------------------
-- Nome Prodotto | Num Categorie

--v2 
select Id, Product.Name , Count(*) as CategoriesNumber from Product
inner join ProductCategory on Product.Id = ProductCategory.IdProduct
group by Id, Product.Name
having Count(*) > 1

--v1 
select * from 
(
	select Id, Product.Name , Count(*) as CategoriesNumber from Product
	inner join ProductCategory on Product.Id = ProductCategory.IdProduct
	group by Id, Product.Name
) as table1
where table1.CategoriesNumber > 1


--6 Elenco dei prodotti con nessuna categoria associata ---------------------------------------------------------------

--v1
select * from Product where
Product.Id not in (select ProductCategory.IdProduct from ProductCategory)

--v2 
select * from Product 
left join ProductCategory on Product.Id = ProductCategory.IdProduct
where ProductCategory.IdProduct is null

--7 Numero dei prodotti per Brand, ordinata per numero dei prodotti decrescente ------------------------------------------
--Nome Brand | Num Prodotti 

select  b.BrandName, count(p.Id) as ProductsNumber
from  Brand b
join Product p on p.BrandId= b.Id
where p.BrandId = b.Id
group by b.BrandName
order by ProductsNumber desc

--8 Prezzo medio dei prodotti per Brand, ordinata dal prezzo medio più alto al più basso -----------------------------------
-- Nome Brand | Prezzo Medio Prodotti

select BrandName,  avg(Product.Price) as avgPrice from Brand
inner join Product on Product.BrandId = Brand.Id
group by BrandName
order by avgPrice desc


--9 Il prodotto più caro di ciascun Brand con il rispettivo nome prodotto ----------------------------------------------------
-- Nome Brand | Nome Prodotto | Prezzo

select  Brand.BrandName,Product.Name,Product.Price from Product
inner join Brand on Product.BrandId = Brand.Id
where Product.Price = 
(
	select max(p2.Price) from Product as p2
	where p2.BrandId = Brand.Id
)
order by Price desc



--10 Il prodotto con il prezzo più alto e il prodotto con il prezzo e più basso per ciascun Brand con i rispettivi nomi prodotto ------
--Nome Brand | Nome Prodotto | Prezzo | Nome Prodotto | Prezzo

select table1.BrandName, table2.Name, table1.minPrice, table2.maxPrice from
(
	--prezzo più basso
	select  Brand.BrandName,Product.Name,Product.Price as minPrice from Product
	inner join Brand on Product.BrandId = Brand.Id
	where Product.Price = 
	(
		select min(p2.Price) from Product as p2
		where p2.BrandId = Brand.Id
	)  
)as table1
inner join
(
	select  Brand.BrandName,Product.Name,Product.Price as maxPrice from Product
	inner join Brand on Product.BrandId = Brand.Id
	where Product.Price = 
	(
		select max(p2.Price) from Product as p2
		where p2.BrandId = Brand.Id
	)
)as table2
on table1.BrandName = table2.BrandName


--11 Data una richiesta informazioni, elencare la cronologia delle risposte riportando i ------------------------------------------
--seguenti dati:
-- User: campo che rappresenta o il Nome e Cognome dello User o NomeBrand della tabella Brand a seconda di chi ha risposto
-- ReplyText
-- InsertDate

Declare @rand int 
select @rand = ROUND((RAND()* 2000),0)+1

--v1 UNION 

select  BrandName as AccountName , reply.InsertDate, reply.ReplyText from Brand
--select all replies of a request
inner join InfoRequestReply as reply  on reply.InfoRequestId =  @rand
--get the account
inner join Account on Account.Id = reply.AccountId
--select account 
where Account.Id = Brand.AccountId 
UNION --account
select  [User].Name as AccountName  , reply.InsertDate, reply.ReplyText from [User]
inner join InfoRequestReply as reply  on reply.InfoRequestId =  @rand
inner join Account on Account.Id = reply.AccountId
where Account.Id = [User].AccountId 


--v2 Case Subquery in select 
select Account.Id,case when Account.AccountType = 2 then (Select Brand.BrandName from Brand where Brand.AccountId = Account.Id)
else (Select [User].Name from [User] where [User].AccountId = Account.Id) end,
reply.InsertDate, reply.ReplyText
from Account
inner join InfoRequestReply as reply  on reply.AccountId = Account.Id
where reply.InfoRequestId = ROUND((RAND()* 2000),0)+1


--v3 CASE + Left join 
select reply.Id as ReplyId, Account.Id as AccountId, 
case when Account.AccountType = 1 then u.Name +' '+ u.LastName else b.BrandName end as AccountName,
reply.ReplyText, reply.InsertDate
from InfoRequestReply  as reply
inner join Account on reply.AccountId = Account.Id
left join Brand as b on Account.Id = b.AccountId
left join [User] as u on Account.Id = u.AccountId
where reply.InfoRequestId  =  ROUND((RAND()* 2000),0)+1



--12 Dato un brand, recuperare l’elenco delle richieste informazioni per prodotto ordinate per ------------------------------------------
--ultima risposta più recente
--Id InfoRequest | IdProdotto | Nome Prodotto | Nome Richiedente | Cognome
--Richiedente | Testo richiesta | Data ultima risposta

--v2
select InfoRequest.Id, InfoRequest.ProductId, Product.Name, InfoRequest.Name, InfoRequest.LastName, InfoRequest.RequestText, table1.lastResponse
from InfoRequest 
inner join Product on  InfoRequest.ProductId = Product.Id 
inner join Brand on Product.BrandId=Brand.Id
inner join
(
	--select the last reply for every request
	select InfoRequestId , Max(InfoRequestReply.InsertDate) as lastResponse from InfoRequestReply
	group by InfoRequestId
) as table1 on InfoRequest.Id = InfoRequestId 
Where Brand.Id = 1
order by lastResponse desc


---v1
select InfoRequest.Id, InfoRequest.ProductId, Product.Name, InfoRequest.Name, InfoRequest.LastName, InfoRequest.RequestText, Max(InfoRequestReply.InsertDate) as lastResponse
from InfoRequest 
inner join Product on  InfoRequest.ProductId = Product.Id 
inner join Brand on Product.BrandId=Brand.Id
inner join InfoRequestReply on InfoRequest.Id = InfoRequestReply.InfoRequestId

where Brand.Id = 1
group by  InfoRequest.Id, InfoRequest.ProductId, Product.Name, InfoRequest.Name, InfoRequest.LastName, InfoRequest.RequestText
order by lastResponse desc


--13  Seleziona i primi tre prodotti più costosi di ogni brand, ordinati per brand e prezzo
--    Nome brand | Nome prodotto | Prezzo

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

--cross join
select Brand.BrandName, t2.Name as ProductName,  t2.Price from Brand cross apply 
(
	select top(3) * from Product
	where Brand.Id = Product.BrandId
	order by Product.Price desc
) t2




