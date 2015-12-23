
/* Find All Triggers */
SELECT 
     sysobjects.name AS trigger_name 
    ,USER_NAME(sysobjects.uid) AS trigger_owner 
    ,s.name AS table_schema 
    ,OBJECT_NAME(parent_obj) AS table_name 
    ,OBJECTPROPERTY( id, 'ExecIsUpdateTrigger') AS isupdate 
    ,OBJECTPROPERTY( id, 'ExecIsDeleteTrigger') AS isdelete 
    ,OBJECTPROPERTY( id, 'ExecIsInsertTrigger') AS isinsert 
    ,OBJECTPROPERTY( id, 'ExecIsAfterTrigger') AS isafter 
    ,OBJECTPROPERTY( id, 'ExecIsInsteadOfTrigger') AS isinsteadof 
    ,OBJECTPROPERTY(id, 'ExecIsTriggerDisabled') AS [disabled] 
FROM sysobjects 

INNER JOIN sysusers 
    ON sysobjects.uid = sysusers.uid 

INNER JOIN sys.tables t 
    ON sysobjects.parent_obj = t.object_id 

INNER JOIN sys.schemas s 
    ON t.schema_id = s.schema_id 

WHERE sysobjects.type = 'TR' 
AND OBJECT_NAME(parent_obj) like 'SomeTable%'


/* Field value (description) displayed in SQL Management Studio in design mode */
select 
    st.name [Table],
    sc.name [Column],
    sep.value [Description]
from sys.tables st
inner join sys.columns sc on st.object_id = sc.object_id
left join sys.extended_properties sep on st.object_id = sep.major_id
                                     and sc.column_id = sep.minor_id
                                     and sep.name = 'MS_Description'                     
where st.name = 'Table Name'
--and sc.name = @ColumnName
order by st.name


--Serch for range date intervals (year + month) in date fields 
SELECT * FROM X.Conta WHERE 
(('201411' >= CAST(YEAR(datInicio) AS CHAR(4)) + CAST(MONTH(datInicio) AS CHAR (2)) AND datfinal is null) 
OR '201411' BETWEEN (CAST(YEAR(datInicio) AS CHAR(4)) + CAST(MONTH(datInicio) AS CHAR (2))) AND  LTRIM(RTRIM((CAST(YEAR(datFinal) AS CHAR(4)))) + CAST(MONTH(datFinal) AS CHAR (2))))
and ano = 2010 and ult = 'S'


-- DISTINTOS COM REPETIÇÃO de UF
SELECT DISTINCT Id, UF FROM 
(
	SELECT 
		COUNT(EMP.id) OVER(PARTITION BY EMP.id) AS Qtd, EMP.id, MUN.UF
	FROM 
		Empr EMP
		INNER JOIN Loca LOC ON EMP.id = LOC.id
		INNER JOIN Muni MUN ON LOC.idMun = MUN.idMun
	GROUP BY 
		EMP.id, MUN.UF
) a
WHERE a.qtd > 1
ORDER BY 
	id

--//Adicionar arquivo MDB no SQL SERVER	
USE [master] --//retirar o MvcMusicStore_log.ldf
 GO
-- Method 1: I use this method
EXEC sp_attach_single_file_db @dbname='MvcMusicStore',
@physname=N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\MvcMusicStore.mdf'
GO

CREATE PROCEDURE [pGeraB](@Ide INT)
AS
DECLARE	@IdeBa INT

SELECT 
	@IdeBalancete = B.IdeBal, 
	@IdeBalanceteA = MAX(BA.IdeBal) 
FROM X.Bal B
LEFT JOIN X.Bal BA
	ON B.IdeA = BA.IdeA
	AND BA.Anm = AnoMes(b.Anm, 'MES', -1)
WHERE B.Ide = @Ide
GROUP BY B.Ide;

WITH CTE (IdeCo, NumCo, DscCo, IdeCons, VlrDeb, VlrCre, VlrSal, VlrSaldoA) AS
(
	SELECT 
		C.IdeCo, 
		C.NumCo, 
		C.DscCo, 
		C.IdeContaS, 
		SUM(CAST(ISNULL(L.VlrDeb, 0) AS DECIMAL(15,2))), 
		SUM(CAST(ISNULL(L.VlrCred, 0) AS DECIMAL(15,2))), 
		SUM(CAST(ISNULL(L.VlrSal, 0) AS DECIMAL(15,2))),
		SUM(CAST(ISNULL(LA.VlrSal, 0) AS DECIMAL(15,2))) AS VlrSaldoA
	FROM X.Bal B
	LEFT JOIN X.Lanc L 
		ON B.IdeB = @IdeB
		AND B.Ide = L.Ide
	LEFT JOIN X.Lanc LA
		ON B.IdeB = @IdeBala 
		AND B.IdeB = LA.IdeB
	INNER JOIN X.Con C
		ON C.IdeCon = L.IdeCon
		OR	C.IdeCon = LA.IdeCon
	--WHERE CAST(''01-'' + LEFT(B.Anm, 2) + ''-'' + RIGHT(B.Anm,4)  AS DATE) > C.DatInicio AND 
		--(CAST(''01-'' + LEFT(B.Anm, 2) + ''-'' + RIGHT(B.Anm,4)  AS DATE) < C.DatFinal OR C.DatFinal IS NULL)
	GROUP BY 
		C.IdeConta, 
		C.NumConta, 
		C.DscConta, 
		C.IdeContaS
		
	UNION ALL
	SELECT 
	C.IdeCon, 
	C.NumCon,
	C.DscCon, 
	C.IdeContaS, 
	T.VlrDeb, 
	T.VlrCred, 
	T.VlrSal,
	T.VlrSaldoA
	FROM CTE T
	INNER JOIN X.Con C
		ON T.IdeContaS = C.IdeCon
)
SELECT 
	c.IdeCon, 
	c.NumCon, 
	c.DscCon, 
	SUM(ISNULL(VlrSaldoA, 0)) AS VlrSaldoA, 	
	SUM(ISNULL(VlrDeb, 0)) as VlrDeb, 
	SUM(ISNULL(VlrCred, 0)) as VlrCred, 
	SUM(ISNULL(VlrSal, 0)) - SUM(ISNULL(VlrSaldoA, 0)) AS [Vari(R$)],
	CAST(CASE WHEN (SUM(ISNULL(VlrSal, 0))/100) = 0 THEN 0.000
	ELSE (SUM(ISNULL(VlrDeb, 0)) - SUM(ISNULL(VlrCred, 0)))/(SUM(ISNULL(VlrSal, 0))/100) END AS DECIMAL(10,3)) AS [Vari(%)],
	SUM(ISNULL(VlrSal, 0)) AS VlrSal
FROM X.Con C 
INNER JOIN CTE
	ON c.IdeCon = cte.IdeCon 
GROUP BY c.IdeCon, c.NumCon, c.DscCon
ORDER BY 2,1
