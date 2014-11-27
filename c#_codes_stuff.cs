Enter file contents here

Linq NOT IN:

public JsonResult GetCursos(string alunoId)
{
    int id = Convert.ToInt32(alunoId);
    var cursosNotIn = db.AlunoCurso.Where(x => x.AlunoId == id).Select(y => y.CursoId);
    var result = db.Curso.Where(x => !cursosNotIn.Contains(x.CursoId)).Select(c => new CursoModel { CursoId = c.CursoId, NomeCurso = c.NomeCurso }).ToList();
    return Json(result);
}

Password=usr;Persist Security Info=False;User ID=Usr;Initial Catalog=Bd;Data Source=BD

Data Source=.\SQLEXPRESS;Initial Catalog=MvcMusicStore;Integrated Security=True

//Join list of string to comma separated and enclosed in single quotes
var dados = dt.Rows.Cast<DataRow>().Where(x => x.Field<string>("Tem") == "1");

string planos = "'" + String.Join("','", dados.Select(row => row.Field<string>("Plano").ToString()).ToArray()) + "'";

var countReceita = (from r in dt.AsEnumerable()
					where r.Field<string>("DscTipo").Trim().Equals("campo")
					select r).Count();
					
var sum = table.AsEnumerable().Sum(x=>x.Field<int>(3));
var sum = table.AsEnumerable().Sum(x=>x.Field<int>("SomeProperty"));
					

//Data table to array int 
var result = dt.Rows.Cast<DataRow>().Select(row => row[0].ToString()).ToArray();
int[] dr2 = Array.ConvertAll(result, (p => Convert.ToInt32(p)));

//Seperado por Virgula
String.Split

var ramos = String.Join(",", Lista.Select(l => l.Desc).ToArray());

//DATETIME NULL
return dt.Rows[0]["DatRgsTemLib"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0].ToString()) : (DateTime?) null;

DataRow[] dr = dt.AsEnumerable().Take(1).ToArray();
object[] dr1 = dr[0].ItemArray;
int[] dr2 = Array.ConvertAll(dr1, (p => Convert.ToInt32(p)));

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

CREATE PROCEDURE [BMP].[pGeraBalancete](@IdeBalancete INT)
AS
DECLARE	@IdeBalanceteAnterior INT

SELECT 
	@IdeBalancete = B.IdeBalancete, 
	@IdeBalanceteAnterior = MAX(BA.IdeBalancete) 
FROM BMP.Balancete B
LEFT JOIN BMP.Balancete BA
	ON B.IdeAgente = BA.IdeAgente
	AND BA.AnmCompetencia = CORP.UTIL.f_DeslocaAnoMes(b.AnmCompetencia, 'MES', -1)
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

	public DataTable BuscaDestinatario(string usuarioId)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection("Data Source=BD;Initial Catalog=CG;Integrated Security=True"))
      {
          conn.Open();
          string query = @"  SELECT DISTINCT ;
          SqlCommand cmd = new SqlCommand(query, conn);
          dt.Load(cmd.ExecuteReader());
      }
      return dt;
  }
