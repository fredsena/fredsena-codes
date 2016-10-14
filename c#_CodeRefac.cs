using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeraArquivo_para_.DbData;
using System.IO;
using GeraArquivo_para_.Domain;

namespace GeraArquivo_para_
{
    public class Arquivo
    {
        public string NomeArquivo { get; set; }        

        public Arquivo(DateTime date)
        {
            NomeArquivo = "C__DataRef_" + date.ToString("dd-MM-yyyy") + "_Gerado_em-" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".txt";
            List<> _C = new DbDataScp().ExecutaConsulta(date);
            ProcessaArquivo(_C, date);
        }

        private void ProcessaArquivo(List<> _C, DateTime date)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append(GeraCabecalho(date)); 

                foreach (var causa in _C)
                {
                    sb.AppendLine(obj.GeraDados(obj));   
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sb.AppendLine("Total de Linhas: " + _C.Count());
                string path = Path.Combine(CriarPasta(), NomeArquivo);
                File.WriteAllText(path, sb.ToString().Substring(0, sb.Length), Encoding.Default);
                sb = null;
            }
        }

        private string GeraCabecalho(DateTime date)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DATA: " + DateTime.Today.ToString("dd/MM/yyyy") + ";");

            return sb.ToString();

        }

        private string CriarPasta()
        {
            string folder = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())), "ArquivoSaida");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return folder;
        }
    }
}
﻿
﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeraArquivo_para_.Domain.Interfaces;
using GeraArquivo_para_.Helpers;
using GeraArquivo_para_.Domain;

namespace GeraArquivo_para_
{
    public abstract class Base
    {
        public string DATA { get; set; }

        public IClassificacao Tipo { get; private set; }

        public void SetTipo(IClassificacao tipo)
        {
            this.Tipo = tipo;
        }

        public virtual string GeraDados(Base obj)
        {
            SetTipo(BuscaTipoClassificacao(obj));
            return GeraLinhaArquivo(obj);
        }

        private string GeraLinhaArquivo(Base obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Tipo.CodigoEmpresa).Append(";").Append(Tipo.StatusTipoAcao.PadRight(49,' ')).Append(";")
                .Append(PreencheCampos(obj));            

            return sb.ToString();
        }

        private string PreencheCampos(Base obj)
        {
            StringBuilder builderFile = new StringBuilder();

            try
            {
                builderFile.Append(HelperStrings.Formata5ZerosDireitaComPontoVirgulaFinal(obj.x));                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return builderFile.ToString();

        }


        protected IClassificacao BuscaTipoClassificacao(Base obj)
        {
            switch (obj.D)
            {
                case "3":
                    return (IClassificacao)new X(obj.P);
                case "4":
                    return (IClassificacao)new C();                    
                case "5":
                    return (IClassificacao)new C();                    
                default:
                    if (X == "S")
                    {
                        var  = new X_S(obj);
                        this.X = .X;
                        return (IClassificacao);
                    }
                    else
                    {
                        return (IClassificacao)new X_N(obj);
                    }
            }
        }


    }
}
﻿using GeraArquivo_para_.Domain.Enums;

namespace GeraArquivo_para_.Domain
{
    public class C : Classificacao
    {
        private string _statusTipoAcao;

        public override ClassificacaoTipo TipoClassificacao
        {
            get { return ClassificacaoTipo.C; }
        }

        public override string StatusTipoAcao { 
            
            get { 
                return "C SEM "; 
            }
            set {
                _statusTipoAcao = "C SEM "; 
            }
        }

        public override string Codigo
        {
            get { return "X20"; }
        }
    }
}﻿using GeraArquivo_para_.Domain.Enums;

namespace GeraArquivo_para_.Domain
{
    public class Consorcios : Classificacao
    {

        private string _statusTipoAcao;

        public override ClassificacaoTipo TipoClassificacao
        {
            get { return ClassificacaoTipo.Consorcios; }
        }

        public override string StatusTipoAcao {

            get 
            {
                return "C  - SEM ";
            }
            set 
            {
                _statusTipoAcao = "C  - SEM ";
            }        
        }

        public override string Codigo
        {
            get { return "X60"; }
        }
    }
}﻿using GeraArquivo_para_.Domain.Enums;

namespace GeraArquivo_para_.Domain
{
    public class X_N : Classificacao
    {
        public X_N(Base obj)
        {
            StatusTipoAcao = "C  -  S";

            if (string.IsNullOrEmpty(obj.COD))
            {
                if (obj.D.Length < 12)
                {
                    StatusTipoAcao = "C  - ";
                }
            }
            else if (obj.NUM_SIN.Length < 12)
            {
                StatusTipoAcao = "C  - ";
            }
        }

        public override ClassificacaoTipo TipoClassificacao
        {
            get { return ClassificacaoTipo.X_N; }
        }

        public override string StatusTipoAcao { get; set; }

        public override string Codigo
        {
            get { return "XX"; }
        }
    }
}
﻿using GeraArquivo_para_.DbData;
using GeraArquivo_para_.Domain.Enums;
using System;
using System.Data;

namespace GeraArquivo_para_.Domain
{
    public class X_S : Classificacao
    {

        public X_S(Base obj)
        {
            StatusTipoAcao = Identifica_StatusTipoAcao(obj.NUM);
        }        

        public override ClassificacaoTipo TipoClassificacao
        {
            get { return ClassificacaoTipo.X_S; }
        }

        public override string StatusTipoAcao { get; set; }

        public int X { get; set; }

        public override string Codigo
        {
            get { return "XX"; }
        }

        private string Identifica_StatusTipoAcao(string COD_, string UM, string UM, string COD_, string DCOD, string NUM_)
        {
            string wc = "";
            string w = "";

            if (string.IsNullOrEmpty(COD_C))
            {
                wc = UM;
                w = UM;
                wDCOD = string.IsNullOrEmpty(DCOD) ? "" : DCOD;
            }
            else
            {
                wc = COD_C;
                w = NUM_;
                w = string.IsNullOrEmpty(COD_) ? "" : COD_;
            }

            StatusTipoAcao = "C  -  S";



            DataTable dt = new DbDataScp().ExecutaConsulta(sql);

            if (Convert.ToInt32(dt.Rows[0]["QTD"].ToString()) != 0)
            {
                StatusTipoAcao = "C  - COM  OFICIAL (X)";
            }
            else
            {
                dt = new DbDataScp().ExecutaConsulta(sql);

                if (dt.Rows.Count != 0)
                {
                    string um = dt.Rows[0]["UM"].ToString().Trim();

                    if (dt.Rows.Count == 1 && um.Substring(2, 2) == wDCOD.Trim())
                    {
                        X = Convert.ToInt32(um);
                        StatusTipoAcao = "C  - (X)";
                    }
                }
            }

            return StatusTipoAcao;
        }
    }
}﻿ug GeraArquivo_para_.Domain.Enums;

namespace GeraArquivo_para_.Domain
{
    public class V : Classificacao
    {

        public V(string P)
        {

        }

        public override ClassificacaoTipo TipoClassificacao
        {
            get { return ClassificacaoTipo.V; }
        }

        public override string StatusTipoAcao { get; set; }

        public override string Codigo
        {
            get { return "XX"; }
        }        
    }
}
﻿
namespace GeraArquivo_para_.Domain
{
    public class  : Base
    {

    }
}
﻿ug GeraArquivo_para_.Domain.Enums;
ug GeraArquivo_para_.Domain.Interfaces;
ug System;
ug System.Collections.Generic;
ug System.Linq;
ug System.Text;
ug System.Threading.Tasks;

namespace GeraArquivo_para_.Domain
{
    public abstract class Classificacao : IClassificacao
    {
        public abstract ClassificacaoTipo TipoClassificacao { get; }
        public abstract string StatusTipoAcao { get; set; }
        public abstract string Codigo { get; }
    }
}
﻿ug System;
ug System.Collections.Generic;
ug System.Linq;
ug System.Text;
ug System.Threading.Tasks;

namespace GeraArquivo_para_.Domain.Enums
{
    public enum ClassificacaoTipo: int
    {
        //3	 V E         
        //4	  S/A          
        //5	 C                     

        V = 3,
         = 4,
        P = 5,
        X_S = 1,
        X_N = 0
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeraArquivo_para_.DbData
{
    public static class DataHelper
    {
        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> jection)
        {
            while (reader.Read())
            {
                yield return jection(reader);
            }
        }
    }
}
using GeraArquivo_para_.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeraArquivo_para_.DbData
{
    public class DbDataScp
    {
        string StringConn = @"";
        public List<> ExecutaConsulta(DateTime date)
        {
            List<> C = new List<>();

            using (SqlConnection conn = new SqlConnection(StringConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(QueryScp2(date), conn);                

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    C = reader.Select(r => new 
                    {
                        DC = r["D"] is DBNull ? null : r["D"].ToString().Trim(),


                    }).ToList();
                }

            }

            return C;
        }

        public DataTable ExecutaConsulta(string sql)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(StringConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                dt.Load(cmd.ExecuteReader());
            }

            return dt;
        }

        private string QueryScp(DateTime date)
        {
            var sql = new StringBuilder("SELECT DISTINCT ");
            sql.Append(" C.* ");
            
            return sql.ToString();
        }

        private string QueryScp2(DateTime date)
        {
            string sql = String.Format(
            @"SELECT DISTINCT  
	            , date.ToString("yyyy-MM-dd"), date.Month.ToString().PadLeft(2, '0'), date.Year.ToString());

            return sql;
        }

        private string QueryScp_Original(DateTime date)
        {
            StringBuilder sql;

            sql = new StringBuilder("SELECT distinct  ");          
            return sql.ToString();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeraArquivo_para_.Helpers
{
    public class HelperStrings
    {
        public static string AlinhaStringComEspacosADireitaComPontoVirgulaFinal(string value, int QtdSpaces)
        {
            try
            {
                if (string.IsNullOrEmpty(value)) value = "";

                return value.PadRight(QtdSpaces, ' ') + ";";
            }
            catch (Exception ex)
            {       
                throw ex;
            }
            
        }

        public static string Formata5ZerosDireitaComPontoVirgulaFinal(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) str = "";

                return str.PadLeft(5, '0') + ";";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
using GeraArquivo_para_.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeraArquivo_para_.Domain.Interfaces
{
    public interface IClassificacao
    {
        ClassificacaoTipo TipoClassificacao { get; }
        string StatusTipoAcao { get; set; }
        string Codigo { get; }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeraArquivo_para_
{
    public static class LinkHelper
    {
        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> jection)
        {
            while (reader.Read())
            {
                yield return jection(reader);
            }
        }
    }
}
using GeraArquivo_para_.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeraArquivo_para_
{
    class gram
    {
        static StringBuilder builderFile = new StringBuilder();
        static DateTime date = Convert.ToDateTime("2016-05-31 X:X:X.X0");
        static string folder { get; set; }
        static int X;

        static string StringConn = @"Data Source=";


        static void Main(string[] args)
        {
            //GeraArquivo_para_(date);
            Arquivo arquivo = new Arquivo(date);
        }

        static void GeraArquivo_para_(DateTime date)
        {
            List<> C = ExecutaConsulta(date);

            cessaArquivo(C);
        }

        private static List<> ExecutaConsulta(DateTime date)
        {
            List<> C = new List<>();

            using (SqlConnection conn = new SqlConnection(StringConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(QueryScp2(date), conn);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    C = reader.Select(r => new 
                    {
                        D = r["D"] is DBNull ? null : r["D"].ToString().Trim(),
                    }).ToList();
                }

            }

            return C;
        }

        private static void cessaArquivo(List<> C)
        {
            int Total_Reg = 0;

            string nomeArquivo = "C__DataRef_" + date.ToString("dd-MM-yyyy") + "_Gerado_em-" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".txt";

            try
            {
                GeraCabecalho(date);                

                foreach (var obj in C)
                {
                    IdentificaStatus(obj);

                    PreencheCampos(obj);

                    Total_Reg++;
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                builderFile.AppendLine("Total de Linhas: " + Total_Reg);
                CriarPasta();
                string path = Path.Combine(folder, nomeArquivo);
                File.WriteAllText(path, builderFile.ToString().Substring(0, builderFile.Length), Encoding.Default);
                builderFile = null;
            }

        }

        private static DataTable ExecutaConsulta(string sql)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(StringConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                dt.Load(cmd.ExecuteReader());
            }

            return dt;
        }

        private static void PreencheCampos( obj)
        {
            try
            {

                builderFile.Append(Formata5ZerosDireitaComPontoVirgulaFinal(obj.D));

                builderFile.AppendLine(obj.S.PadRight(10, ' ') + ";");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static void IdentificaStatus( obj)
        {
            try
            {
                X = 0;
                string wc = "";
                string w = "";

                string STATUS_ = "";               

                builderFile.Append(STATUS_ + new String(' ', 49 - STATUS_.Length) + ";");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static void GeraCabecalho(DateTime date)
        {

            builderFile.AppendLine("S;");

        }

        static string QueryScp(DateTime date)
        {
            var sql = new StringBuilder("SELECT DISTINCT ");

            return sql.ToString();
        }

        static string QueryScp2(DateTime date)
        {
            string sql = String.Format(
            @"SELECT DISTINCT  ", date.ToString("yyyy-MM-dd"), date.Month.ToString().PadLeft(2, '0'), date.Year.ToString());

            return sql;
        }

        static void CriarPasta()
        {
            folder = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())), "ArquivoSaida");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        static string Formata5ZerosDireitaComPontoVirgulaFinal(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) str = "";

                return str.PadLeft(5, '0') + ";";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
