
Password=usr;Persist Security Info=False;User ID=Usr;Initial Catalog=Bd;Data Source=BD

Data Source=.\SQLEXPRESS;Initial Catalog=MvcMusicStore;Integrated Security=True

//splited by comma
var ramos = String.Join(",", Lista.Select(l => l.Desc).ToArray());

//DATETIME NULL
return dt.Rows[0]["DatRgsTemLib"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0].ToString()) : (DateTime?) null;

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
