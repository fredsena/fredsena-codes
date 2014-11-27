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
