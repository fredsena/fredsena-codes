
/* *******************************************************************/
//use Enum as optional parameter
public enum StatusService : int { NO_DATA = 0, HAS_DATA = 1, ERROR = 2 }
public string Execute(string someString, StatusService status = StatusService.HAS_DATA) {//DoSomething...}

/* *******************************************************************/
//Get the name of current function
Console.WriteLine(new StackTrace().GetFrame(0).GetMethod().Name.ToString());
/* *******************************************************************/

//Get project path: COOL!!!!!!
string projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
string projectPath2 = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName,"abc.txt")
/* *******************************************************************/

string connString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
Password=usr;Persist Security Info=False;User ID=Usr;Initial Catalog=Bd;Data Source=BD
Data Source=.\SQLEXPRESS;Initial Catalog=MvcMusicStore;Integrated Security=True
/* *******************************************************************/

//splited by comma
var ramos = String.Join(",", Lista.Select(l => l.Desc).ToArray());
/* *******************************************************************/

//DATETIME NULL
return dt.Rows[0]["DatRgsTemLib"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0].ToString()) : (DateTime?) null;
/* *******************************************************************/

//Connection and query
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
