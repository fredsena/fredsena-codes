
//sql_stuff.sql

SELECT * FROM BMP.Conta WHERE 
(('201411' >= CAST(YEAR(datInicio) AS CHAR(4)) + CAST(MONTH(datInicio) AS CHAR (2)) AND datfinal is null) 
OR '201411' BETWEEN (CAST(YEAR(datInicio) AS CHAR(4)) + CAST(MONTH(datInicio) AS CHAR (2))) AND (CAST(YEAR(datFinal) AS CHAR(4)) + CAST(MONTH(datFinal) AS CHAR (2))))
and ano = 2010 and ultimograu = 'S'


-- DISTINTOS COM REPETIÇÃO de UF
SELECT DISTINCT IdEmpreendimento, UF FROM 
(
	SELECT 
		COUNT(EMP.idEmpreendimento) OVER(PARTITION BY EMP.idEmpreendimento) AS Qtd, EMP.idEmpreendimento, MUN.UF
	FROM 
		Empreendimento EMP
		INNER JOIN Localidade LOC ON EMP.idEmpreendimento = LOC.idEmpreendimento
		INNER JOIN Municipio MUN ON LOC.idMunicipio = MUN.idMunicipio
	GROUP BY 
		EMP.idEmpreendimento, MUN.UF
) a
WHERE a.qtd > 1
ORDER BY 
	idEmpreendimento

--//Adicionar arquivo MDB no SQL SERVER	
USE [master] --//retirar o MvcMusicStore_log.ldf
 GO
-- Method 1: I use this method
EXEC sp_attach_single_file_db @dbname='MvcMusicStore',
@physname=N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\MvcMusicStore.mdf'
GO

CREATE PROCEDURE [pGeraBalan](@IdeBalancete INT)
AS
DECLARE	@IdeBalanceteAnterior INT

SELECT 
	@IdeBalancete = B.IdeBalancete, 
	@IdeBalanceteAnterior = MAX(BA.IdeBalancete) 
FROM BMP.Balancete B
LEFT JOIN BMP.Balancete BA
	ON B.IdeAgente = BA.IdeAgente
	AND BA.AnmCompetencia = UTIL.f_DeslocaAnoMes(b.AnmCompetencia, 'MES', -1)
WHERE B.IdeBalancete = @IdeBalancete
GROUP BY B.IdeBalancete;

WITH CTE (IdeConta, NumConta, DscConta, IdeContaSuperior, VlrDebito, VlrCredito, VlrSaldo, VlrSaldoAnterior) AS
(
	SELECT 
		C.IdeConta, 
		C.NumConta, 
		C.DscConta, 
		C.IdeContaSuperior, 
		SUM(CAST(ISNULL(L.VlrDebito, 0) AS DECIMAL(15,2))), 
		SUM(CAST(ISNULL(L.VlrCredito, 0) AS DECIMAL(15,2))), 
		SUM(CAST(ISNULL(L.VlrSaldo, 0) AS DECIMAL(15,2))),
		SUM(CAST(ISNULL(LA.VlrSaldo, 0) AS DECIMAL(15,2))) AS VlrSaldoAnterior
	FROM BMP.Balancete B
	LEFT JOIN BMP.Lancamento L 
		ON B.IdeBalancete = @IdeBalancete 
		AND B.IdeBalancete = L.IdeBalancete
	LEFT JOIN BMP.Lancamento LA
		ON B.IdeBalancete = @IdeBalanceteAnterior 
		AND B.IdeBalancete = LA.IdeBalancete
	INNER JOIN BMP.Conta C
		ON C.IdeConta = L.IdeConta
		OR	C.IdeConta = LA.IdeConta
	--WHERE CAST(''01-'' + LEFT(B.AnmCompetencia, 2) + ''-'' + RIGHT(B.AnmCompetencia,4)  AS DATE) > C.DatInicio AND 
		--(CAST(''01-'' + LEFT(B.AnmCompetencia, 2) + ''-'' + RIGHT(B.AnmCompetencia,4)  AS DATE) < C.DatFinal OR C.DatFinal IS NULL)
	GROUP BY 
		C.IdeConta, 
		C.NumConta, 
		C.DscConta, 
		C.IdeContaSuperior
		
	UNION ALL
	SELECT 
	C.IdeConta, 
	C.NumConta,
	C.DscConta, 
	C.IdeContaSuperior, 
	T.VlrDebito, 
	T.VlrCredito, 
	T.VlrSaldo,
	T.VlrSaldoAnterior
	FROM CTE T
	INNER JOIN BMP.Conta C
		ON T.IdeContaSuperior = C.IdeConta
)
SELECT 
	c.IdeConta, 
	c.NumConta, 
	c.DscConta, 
	SUM(ISNULL(VlrSaldoAnterior, 0)) AS VlrSaldoAnterior, 	
	SUM(ISNULL(VlrDebito, 0)) as VlrDebito, 
	SUM(ISNULL(VlrCredito, 0)) as VlrCredito, 
	SUM(ISNULL(VlrSaldo, 0)) - SUM(ISNULL(VlrSaldoAnterior, 0)) AS [Variacao(R$)],
	CAST(CASE WHEN (SUM(ISNULL(VlrSaldo, 0))/100) = 0 THEN 0.000
	ELSE (SUM(ISNULL(VlrDebito, 0)) - SUM(ISNULL(VlrCredito, 0)))/(SUM(ISNULL(VlrSaldo, 0))/100) END AS DECIMAL(10,3)) AS [Variacao(%)],
	SUM(ISNULL(VlrSaldo, 0)) AS VlrSaldo
FROM BMP.Conta C 
INNER JOIN CTE
	ON c.IdeConta = cte.IdeConta 
GROUP BY c.IdeConta, c.NumConta, c.DscConta
ORDER BY 2,1
-----------------------------------------------------------
-- FIM DA PROCEDURE
-----------------------------------------------------------
