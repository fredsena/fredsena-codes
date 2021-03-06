
//Group a List<KeyValuePair<object1,object2>> as grouped object2 with object1
var result = 
	GetMethod() //List<KeyValuePair<object1,object2>>
	.GroupBy(p => p.UserId, p => p.MethodGuid, (key, g) => new { UserId = key, MethodGuid = g.ToList().Distinct() })
	.ToList();

//get duplicate items from a list using LINQ
//https://stackoverflow.com/questions/18547354/c-sharp-linq-find-duplicates-in-list?noredirect=1&lq=1
//https://stackoverflow.com/questions/3811464/how-to-get-duplicate-items-from-a-list-using-linq

var duplicates = lst.GroupBy(s => s).SelectMany(grp => grp.Skip(1));


//http://blog.developers.ba/using-classic-ado-net-in-asp-net-vnext/

//Convert DataReader to List<T>
//http://stackoverflow.com/questions/1464883/how-can-i-easily-convert-datareader-to-listt
public static List<ObjectDTO> GetDistinctObjects()
{
    var collections = new List<ObjectDTO>();

    using (SqlConnection conn = new SqlConnection(@"Data Source=XYZ;Initial Catalog=DBSomething;Integrated Security=True"))
    {
	conn.Open();
	var cmd = new SqlCommand(SqlQueries.GetDistinctObjects(), conn);

	using (IDataReader reader = cmd.ExecuteReader())
	{
	    collections = reader.Select(r => new ObjectDTO
	    {
		Name = r["Name"] is DBNull? null : r["Name"].ToString().Trim(),
		Year = r["Year"] is DBNull? 0 : Convert.ToInt32(r["Year"].ToString().Trim()),
		DateUpdated = r["DateUpdated"] is DBNull ? (DateTime?)null : Convert.ToDateTime(r["DateUpdated"].ToString().Trim()),
		DataGuid = r["DataGuid"] is DBNull ? Guid.Empty : new Guid(r["DataGuid"].ToString().Trim()),
		Token = r["Token"] is DBNull ?new byte() : Convert.ToByte(r["Token"].ToString().Trim()),
		Active = r["Active"] is DBNull ? false : Convert.ToBoolean(r["Active"].ToString().Trim())
	    }).ToList();
	}
     }	
     return collections;
}

public static class LinkHelper
{
	//Helper method for "Convert DataReader to List<T>"
	public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> projection)
	{
		while (reader.Read())
		{
			yield return projection(reader);
		}
	}
	
	//Helper method for "Convert IEnumerable to DataTable"
	public static DataTable AsDataTable<T>(this IEnumerable<T> data)
	{
	    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

	    var table = new DataTable();

	    foreach (PropertyDescriptor prop in properties)
		table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

	    foreach (T item in data)
	    {
		DataRow row = table.NewRow();
		foreach (PropertyDescriptor prop in properties)
		    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
		table.Rows.Add(row);
	    }

	    return table;
	}
}

/* ############################################################################################################################### */
//extension method to Bulk insert data from List<T> into SQL Server.
//http://blog.developers.ba/bulk-insert-generic-list-sql-server-minimum-lines-code/
var listPerson = new List<Person>
    {
         new Person() {Id = 1}, 
         new Person() {Id = 2}
    };
 
using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SomeConnectionString"].ConnectionString))
{
 connection.Open();
 SqlTransaction transaction = connection.BeginTransaction();

 using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
 {
    bulkCopy.BatchSize = 100;
    bulkCopy.DestinationTableName = "dbo.Person";
    try
    {
	bulkCopy.WriteToServer(listPerson.AsDataTable());
    }
    catch (Exception)
    {
	transaction.Rollback();
	connection.Close();
    }
  }

  transaction.Commit();
}
/* ############################################################################################################################### */

//Linq NOT IN:
public JsonResult GetCursos(string alunoId)
{
    int id = Convert.ToInt32(alunoId);
    var cursosNotIn = db.AlunoCurso.Where(x => x.AlunoId == id).Select(y => y.CursoId);
    var result = db.Curso.Where(x => !cursosNotIn.Contains(x.CursoId)).Select(c => new CursoModel { CursoId = c.CursoId, NomeCurso = c.NomeCurso }).ToList();
    return Json(result);
}

/* ############################################################################################################################### */

//Linq NOT IN(2):
var emails = new List<string> { "Atum@fred.com", "me@hi.com", "Pal@fred.com", "fred_sena@fred.com"};

var foundEmailsFromUsers = 
	Users.Where(x => !Users.Any(s => !emails.Contains(x.Email)))
		 .Select(x => x.Email);

var notfoundEmails = from e in emails where !foundEmailsFromUsers.Contains(e) select e;		

//Use ".Dump()" in LinQPad (Language: C# Statements)
notfoundEmails.Dump();

/* ############################################################################################################################### */

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
var result = dt.Rows.Cast<DataRow>().Select(row => row[0].ToString()).OrderBy(n => n).ToArray();
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
