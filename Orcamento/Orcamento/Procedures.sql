create procedure CadastroCliente
	(
		@Nome varchar(255),	@Telefone varchar(255)
	)
	As
	Begin 

	insert into  Clientes values(@Nome,@Telefone)

	End

GO 

create procedure CadastroProduto
	(
		@Nome varchar(255),	@Preco decimal
	)
	As
	Begin 

	insert into  Produtoss values(@Nome,@Preco)

	End

GO 

create procedure CadastroOrcamento
	(
		@CodigoOrcamento int,	@ClienteId int, @ProdutoId int
	)
	As
	Begin 

	insert into  Vendas values(@CodigoOrcamento,@ClienteId,@ProdutoId)

	End

GO 


create procedure alterarCliente
(
@id int,
@nome varchar(255),
@Telefone varchar(255)
)
As
Begin 

update Clientes set Nome = @nome, Telefone = @Telefone where Id = @id

End

GO

create procedure alterarProduto
(
@id int,
@nome varchar(255),
@Preco decimal
)
As
Begin 

update Produtoss set Nome = @nome, Valor = @Preco where Id = @id

End

GO

create or alter procedure alterarOrcamento
(
@id int, 
@ProdutoId int,
@idVenda int
)
As
Begin 

update Vendas set ProdutoId = @ProdutoId where CodigoOrcamento = @id and id =@idVenda

End

GO


CREATE FUNCTION consultaClientes (@id INT )
RETURNS TABLE
AS
RETURN (SELECT *
        FROM  Clientes
        WHERE Id = @id)
go

CREATE FUNCTION consultaProduto (@id INT )
RETURNS TABLE
AS
RETURN (SELECT *
        FROM  Produtoss
        WHERE Id = @id)
		go

CREATE FUNCTION consultaOrcamento (@id INT )
RETURNS TABLE
AS
RETURN (SELECT *
        FROM  Vendas
        WHERE CodigoOrcamento = @id)
		go

CREATE  OR ALTER   FUNCTION consultaOrcamentoIndex()
RETURNS  table
RETURN ( select * from Vendas  as v inner join Produtoss as prod on prod.Id = v.ProdutoId inner join Clientes as c on c.Id = v.ClienteId
WHERE 
   v.id = (SELECT top 1 p.id  FROM Vendas as p  WHERE v.CodigoOrcamento = p.CodigoOrcamento )  )
go
