

//Linq NOT IN:
public JsonResult GetCursos(string alunoId)
{
    int id = Convert.ToInt32(alunoId);
    var cursosNotIn = db.AlunoCurso.Where(x => x.AlunoId == id).Select(y => y.CursoId);
    var result = db.Curso.Where(x => !cursosNotIn.Contains(x.CursoId)).Select(c => new CursoModel { CursoId = c.CursoId, NomeCurso = c.NomeCurso }).ToList();
    return Json(result);
}

//Zeroes values from a specific column 
dt.Rows.OfType<DataRow>().ToList().ForEach(x => x["VlrPagamento"] = DBNull.Value);

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

DataRow[] dr = dt.AsEnumerable().Take(1).ToArray();
object[] dr1 = dr[0].ItemArray;
int[] dr2 = Array.ConvertAll(dr1, (p => Convert.ToInt32(p)));

public List<SomeViewModel> Listar(string status = "", string name = "", int Id = 0)
{
    using (var db = new Entities())
    {
        var result = (from d in db.Table1
                     join e in db.vTable2 on d.Id equals e.Ide                             
                     select new SomeViewModel
                     {
                         IdeEmp = d.Id,
                         Idc = d.Idc
                     });
                     
        //ativos, inativos, todos
        if (!String.IsNullOrEmpty(status))                
            result = result.Where(r => r.Idc == status);                

        if (!String.IsNullOrEmpty(name))                
            result = result.Where(r => r.NomEmp.Contains(name));

        if (Id != 0)
            result = result.Where(r => r.Id == Id);

        return result.OrderBy(o => o.NamEmp).ToList();
    }
}