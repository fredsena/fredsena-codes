using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.MemoryMappedFiles;
using ..Modelo;
using System.Data;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data.Objects.SqlClient;
using System.Data.Objects;
using System.ComponentModel;
using System.Globalization;

namespace ConsoleAppTest
{
    class Program
    {
        public enum Mes { Jan, Fev, Mar, Abr, Mai, Jun, Jul, Ago, Set, Out, Nov, Dez };

        static void Main(string[] args)
        {
            //foo();
            //PADLEFT();
            //UltimoDiaMes();
            //bool valido = ValidaData("01/12/0100");
            //GeraArquivos();
            //BuscaDuplicados();
            //BooleanJerkTest();
            //DivideTest();
            //AddSpaceIfNotEmpty();
            //Start();
            //bool diaUtil = EhDiaUtil(DateTime.Now.AddDays(3));
            //TesteJason();
            //DateTimeCalcDifference();
            //DateTimeCalcAnos();
            //LinqGroupBy();
            //LinqGroupBy2();
            //GeraArquivosPlanoConta__2014();
            //GeraArquivosTESTEPlanoConta__2014();
            //ConvertNumbers();
            //string result = FormataConta(2010, "511032105");
            //CountForDesc();

            //BuscaUltimaVersaoLancamentoAnterior();
            //LinqMAX();
            //BuscaPlanoContas();
            //int qtd = LinqDISTINCT_Count();
            //int index = "teste00".IndexOf(",");
            //GeraPeD();
            //bool valido = PeriodoEntre12Meses(Convert.ToDateTime("01/05/2014"), Convert.ToDateTime("01/04/2015"));
            //DataTable_Tests();
            //GeraDadosDE_PARA();
            //NorthWind_NotIn();

            //GeraArquivosTESTE_DE_PARA();
            //CountNodesXML();

            //GeraXML();

            //RelatorioAvisoNotificacaoInadimplencia();

            //ValidaBalancete012015();

            //Gera_INSERT_DE_PARA_X();

            //GERA_SCRIPTS_PARAMETROS_ENVIO();

            //GeraArquivos_INSERT_DE_PARA_X();

            //Conta2015Excluidas();

            //Conta2015OHARA();

            //bool teste = EhNumero("11020818231,44-");

            BuscaContasValidas_Por_Competencia();

            //Busca_Status_Primeiro_Envio_Competencia();


        }

        #region MÉTODOS


        private static byte? _flgmrc;

        public static byte? FlgMrc
        {
            get { return _flgmrc; }
            set { _flgmrc = value; }
        }

        public enum DadosPermitidos : byte
        {
            Marco_Obrigatório = 2,
            Percentual = 4,
            Data_Inicio = 8,
            Data_Conclusao = 16
        }
        static void Main(string[] args)
        {
            //TestIfEnum();
            CompareDates();
        }

        static void TestIfEnum()
        {
            //0,6,18,28
            //Resumo: habilita campos segundo as condições abaixo:
            //0 : não habilita nenhum campo para preenchimento
            //6 : habilita somente MarcoObrigatorio e Percentual
            //

            FlgMrc = 18;

            if (
                !(
                    (Convert.ToByte(DadosPermitidos.Data_Inicio) & FlgMrc) == Convert.ToByte(DadosPermitidos.Data_Inicio)
                 )
               )
            {
                string txtPrevInicio = "txtPrevInicio: Enabled = false / Visible = false";                
            }

            var test = 16 & 16;

            var test1 = Convert.ToByte(DadosPermitidos.Data_Inicio) & 0;
            var test2 = Convert.ToByte(DadosPermitidos.Data_Inicio) & 6;
            var test3 = Convert.ToByte(DadosPermitidos.Data_Inicio) & 18;
            var test4 = Convert.ToByte(DadosPermitidos.Data_Inicio) & 28;


            bool MarcoObrigatorioChecked = (Convert.ToByte(DadosPermitidos.Marco_Obrigatório) & Convert.ToByte(FlgMrc)) == Convert.ToByte(DadosPermitidos.Marco_Obrigatório);
            bool PercentualChecked = (Convert.ToByte(DadosPermitidos.Percentual) & Convert.ToByte(FlgMrc)) == Convert.ToByte(DadosPermitidos.Percentual);
            bool DatasInicioChecked = (Convert.ToByte(DadosPermitidos.Data_Inicio) & Convert.ToByte(FlgMrc)) == Convert.ToByte(DadosPermitidos.Data_Inicio);
            bool DatasConclusaoChecked = (Convert.ToByte(DadosPermitidos.Data_Conclusao) & Convert.ToByte(FlgMrc)) == Convert.ToByte(DadosPermitidos.Data_Conclusao);


        }

        static void Print(string texto)
        {
            Debug.WriteLine(texto);
        }

        static void CompareDates()
        {
            DateTime? date1 = null;
            DateTime? date2 = null;            

            int result = Nullable.Compare(date1,date2);

            /*
            Less than zero: The HasValue property for n1 is false, and the HasValue property for n2 is true.
            -or- The HasValue properties for n1 and n2 are true, and the value of the Value property for n1 is less than the value of the Value property for n2.
            
            Zero: The HasValue properties for n1 and n2 are false.
            -or- The HasValue properties for n1 and n2 are true, and the value of the Value property for n1 is equal to the value of the Value property for n2.

            Greater than zero: The HasValue property for n1 is true, and the HasValue property for n2 is false.
            -or- The HasValue properties for n1 and n2 are true, and the value of the Value property for n1 is greater than the value of the Value property for n2.
            */
        }	


        static void BuscaPlanoContas()
        {
            using (var db = new Entidades())
            {
                //615032939	2006-01-01	2006-04-30
                //			2006-05-01	2007-02-28

                int competenciaArquivo = 200512;
                bool contaValida = false;

                List<..Modelo.Conta> Contas =
                    (from c in db.Conta.Where(x => x.IdcUltimoGrau.Equals("S") && x.AnoPlanoConta.Equals("2010")) select c).OrderBy(x => x.NumConta).ToList();

                var conta = Contas.Where(x => x.NumConta.Equals("615032939")).OrderBy(x => x.DatInicio);

                if (conta.Count() > 0)
                {
                    //Busca conta que data final seja null (conta ativa)
                    //var conta1 = Contas.Where(x => x.NumConta.Equals("1120151") && !x.DatFinal.HasValue);

                    foreach (var itemConta in conta)
                    {
                        int competenciaContaInicial = Convert.ToInt32(new DateTime(itemConta.DatInicio.Year, itemConta.DatInicio.Month, 1).ToString("yyyyMM"));

                        //Busca conta que data final seja null (conta ativa) e valida data inicial
                        if (!itemConta.DatFinal.HasValue)
                        {
                            contaValida = (competenciaArquivo >= competenciaContaInicial);

                            if (contaValida) break;
                        }
                        else
                        {
                            //Verifica se a competência da itemConta.DatFinal está no período válido
                            int competenciaContaFinal = Convert.ToInt32(new DateTime(itemConta.DatFinal.Value.Year, itemConta.DatFinal.Value.Month, 1).ToString("yyyyMM"));
                            contaValida = ((competenciaArquivo >= competenciaContaInicial) && (competenciaArquivo <= competenciaContaFinal));

                            if (contaValida) break;
                        }
                    }
                }
                else
                {
                    string mensagem = "conta: INVÁLIDA";
                }

                string result;
                if (contaValida)
                    result = "TRUE";
                else
                    result = "FALSE";

            }
        }

        /*
        static List<TesteDados> ListaTesteDados()
        {
            List<TesteDados> teste = new List<TesteDados>();
            //teste.Add(new TesteDados { IdeBalancete = 73192, IdeAgente = 394, AnmCompetencia = "201409" });
            //teste.Add(new TesteDados { IdeBalancete = 73201, IdeAgente = 394, AnmCompetencia = "201409" });
            //teste.Add(new TesteDados { IdeBalancete = 73191, IdeAgente = 394, AnmCompetencia = "201408" });
            //teste.Add(new TesteDados { IdeBalancete = 73190, IdeAgente = 394, AnmCompetencia = "201407" });
            //teste.Add(new TesteDados { IdeBalancete = 45194, IdeAgente = 394, AnmCompetencia = "201406" });
            //teste.Add(new TesteDados { IdeBalancete = 64103, IdeAgente = 394, AnmCompetencia = "201409" });
            //teste.Add(new TesteDados { IdeBalancete = 51314, IdeAgente = 394, AnmCompetencia = "201405" });
            //teste.Add(new TesteDados { IdeBalancete = 68966, IdeAgente = 394, AnmCompetencia = "201404" });
            //teste.Add(new TesteDados { IdeBalancete = 12345, IdeAgente = 394, AnmCompetencia = "201403" });

            return teste;
        }
        */

        static List<String> listaDados()
        {
            List<String> lista = new List<string>();

            //      ideAgente              IdeBalancete
            //lista.Add("382, 201405, LIGHT, 57511");

            //50032	39	201405
            //60994	39	201404
            //61244	39	201403

            lista.Add("394, 201403, FURNAS, 73214");
            //lista.Add("39, 201404, COELCE, 60994");
            //lista.Add("39, 201405, COELCE, 50032");

            //lista.Add("692,201011,GUARANTA,60157");
            //lista.Add("7019,201407,AmE,62354");
            //lista.Add("5381,201407,CEDRAP,61730");
            //lista.Add("4950,201406,CEMIG-D,55565");
            //lista.Add("5454,201407,STC,54924");
            //lista.Add("6240,201406,ATEVI,57259");
            //lista.Add("4821,201407,LUMITRANS,57333");
            //lista.Add("5703,201405,CELGGT,69264");

            return lista;
        }

        static string ToXml(DataTable table, int metaIndex = 0)
        {
            XDocument xdoc = new XDocument(
                new XElement(table.TableName,
                    from column in table.Columns.Cast<DataColumn>()
                    where column != table.Columns[metaIndex]
                    select new XElement(column.ColumnName,
                        from row in table.AsEnumerable()
                        select new XElement(row.Field<string>(metaIndex), row[column])
                        )
                    )
                );

            return xdoc.ToString();
        }

        static void CountForDesc()
        {
            string teste = "";
            for (int i = (DateTime.Now.Year + 1); i >= 2000; i--)
            {
                teste = teste + i.ToString() + ",";
            }
        }

        public static string FormataConta(int anoCompetencia, string conta)
        {
            string result = "";

            if (anoCompetencia >= 2015)
            {
                //Plano Contas NOVO - XXXX.X.XX.XX (1102.1.01.05)
                switch (conta.Length)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        result = conta;
                        break;
                    case 5:
                        result = string.Format("{0}.{1}", conta.Substring(0, 4), conta.Substring(4, 1));
                        break;
                    case 7:
                        result = string.Format("{0}.{1}.{2}", conta.Substring(0, 4), conta.Substring(4, 1), conta.Substring(5, 2));
                        break;
                    case 9:
                        result = string.Format("{0}.{1}.{2}.{3}", conta.Substring(0, 4), conta.Substring(4, 1), conta.Substring(5, 2), conta.Substring(7, 2));
                        break;
                    default:
                        result = conta;
                        break;
                }
            }
            else
            {
                //Plano Contas Antigo - XXX.XX.X.X.XX (511.03.2.1.05)
                switch (conta.Length)
                {
                    case 1:
                    case 2:
                    case 3:
                        result = conta;
                        break;
                    case 5:
                        result = string.Format("{0}.{1}", conta.Substring(0, 3), conta.Substring(3, 2));
                        break;
                    case 6:
                        result = string.Format("{0}.{1}.{2}", conta.Substring(0, 3), conta.Substring(3, 2), conta.Substring(5, 1));
                        break;
                    case 7:
                        result = string.Format("{0}.{1}.{2}.{3}", conta.Substring(0, 3), conta.Substring(3, 2), conta.Substring(5, 1), conta.Substring(6, 1));
                        break;
                    case 9:
                        result = string.Format("{0}.{1}.{2}.{3}.{4}", conta.Substring(0, 3), conta.Substring(3, 2), conta.Substring(5, 1), conta.Substring(6, 1), conta.Substring(7, 2));
                        break;
                    default:
                        result = conta;
                        break;
                }
            }

            return result;
        }

        static void ConvertNumbers()
        {
            string numero = "1492479.52";
            Decimal dec = Convert.ToDecimal(numero);
        }

        static void GeraArquivosPlanoConta__2014()
        {
            string path = @"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\CSV_Plano_Contas__2014_Import.csv";

            StreamReader sr = new StreamReader(path, Encoding.GetEncoding("ISO-8859-1"));
            StringBuilder builderInsert = new StringBuilder();

            string nomeArquivo = Path.GetFileNameWithoutExtension(path);

            //Gera Pasta
            string pasta = @"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\IMPORTACAO\RESULT\" + Path.GetFileNameWithoutExtension(nomeArquivo);

            try
            {
                if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

                //var text = File.ReadAllText(file, Encoding.GetEncoding(codePage));
                //sr = File.ReadAllText(@"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\Plano_Contas__2014_Import_SEM_COL_GRAU.csv", Encoding.GetEncoding("ISO-8859-1"));

                string str = "";
                string[] linha = null;
                string ultimoGrau;

                //loop
                while (str != null)
                {
                    ultimoGrau = "";

                    //Código; Título; UltimoGrau

                    //@"INSERT INTO [].[].[Conta]
                    //    ([NumConta]
                    //    ,[DscConta]
                    //    ,[IdcUltimoGrau]
                    //    ,[DatInicio]
                    //    ,[DatFinal]
                    //    ,[IdeUsuario]
                    //    ,[AnoPlanoConta]
                    //    ,[IdeContaSuperior])
                    //VALUES
                    //    (<NumConta, varchar(9),>
                    //    ,<DscConta, varchar(1000),>
                    //    ,<IdcUltimoGrau, char(1),>
                    //    ,<DatInicio, date,>
                    //    ,<DatFinal, date,>
                    //    ,<IdeUsuario, int,>
                    //    ,<AnoPlanoConta, char(4),>
                    //    ,<IdeContaSuperior, int,>)";

                    str = sr.ReadLine();
                    if (str == null) break;

                    linha = str.Split(';');

                    if (linha[0] == "") continue;

                    if (linha[2].ToUpper() == "X")
                        ultimoGrau = "S";
                    else
                        ultimoGrau = "N";

                    builderInsert.AppendLine("INSERT INTO [].[].[Conta] VALUES ('"
                        + linha[0].Replace(".", "") + "', '" + linha[1] + "', '" + ultimoGrau + "' , '2015-01-01', null, 1, '2015', null)");

                }

            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Cria arquivo SCRIPT INSERT
                string filePath = pasta + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo) + ".txt";
                File.WriteAllText(filePath, builderInsert.ToString().Substring(0, builderInsert.Length));
                sr.Close();
                builderInsert = null;
            }
        }

        static void GeraArquivosTESTEPlanoConta__2014()
        {
            string path = @"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\PROVA_DOS_NOVE\CSV_Import__DOC.csv";

            StreamReader sr = new StreamReader(path, Encoding.GetEncoding("ISO-8859-1"));
            StringBuilder builderInsert = new StringBuilder();

            string nomeArquivo = Path.GetFileNameWithoutExtension(path);

            //Gera Pasta
            string pasta = @"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\PROVA_DOS_NOVE\RESULT\" + Path.GetFileNameWithoutExtension(nomeArquivo);

            try
            {
                if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

                //var text = File.ReadAllText(file, Encoding.GetEncoding(codePage));
                //sr = File.ReadAllText(@"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\Plano_Contas__2014_Import_SEM_COL_GRAU.csv", Encoding.GetEncoding("ISO-8859-1"));

                string str = "";
                string[] linha = null;

                //loop
                while (str != null)
                {
                    str = sr.ReadLine();
                    if (str == null) break;

                    linha = str.Split(';');

                    if (linha[0] == "") continue;

                    //builderInsert.AppendLine("INSERT INTO [].[].[Conta] VALUES ("
                    //    + linha[0].Replace(".", "") + ", '" + linha[1] + "', '" + ultimoGrau + "' , '2015-01-01', null, 1, '2015')");

                    builderInsert.AppendLine(linha[0].Replace(".", "") + " - " + linha[1]);
                }
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Cria arquivo SCRIPT INSERT
                string filePath = pasta + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo) + ".txt";
                File.WriteAllText(filePath, builderInsert.ToString().Substring(0, builderInsert.Length));
                sr.Close();
                builderInsert = null;
            }
        }

        static void BuscaUltimaVersaoLancamentoAnterior()
        {
            using (var db = new Entidades())
            {
                var resultado = (from b in db.Balancete
                                 where b.AnmCompetencia == "201312"
                                 && b.IdeAgente == 383
                                 select b);

                int valido = resultado.Count();


                //201501
                //73199
                //73198	

                //201409
                //73201
                //73200

                //var result2 = (from b in db.Balancete
                //               where b.AnmCompetencia.Equals("201409")
                //              && b.IdeAgente.Equals(394)
                //               select b).Take(2).OrderByDescending(o => o.IdeBalancete);
                //result2.ToList();                


                //var result0 = 
                //    (from b in db.Balancete where b.AnmCompetencia.Equals("201409") 
                //         && b.IdeAgente.Equals(394) select b.IdeBalancete).Max();                


                //var result =
                //       (from l in db.Lancamento
                //        join c in db.Conta on l.IdeConta equals c.IdeConta
                //        join b in db.Balancete on l.IdeBalancete equals b.IdeBalancete
                //        where b.AnmCompetencia.Equals("201409") &&
                //        b.IdeAgente.Equals(394) 
                //        && b.IdeBalancete.Equals(result0)
                //        //select new { c.NumConta, l.VlrCredito, l.VlrDebito, l.VlrSaldo }).ToList();
                //        select new LancamentoModel
                //        {
                //            NumConta = c.NumConta,
                //            VlrCredito = l.VlrCredito,
                //            VlrDebito = l.VlrDebito,
                //            VlrSaldo = l.VlrSaldo
                //        });


                //LancamentoModel lan = result.Where(x => x.NumConta.Equals("1120152")).SingleOrDefault();  

                /*                        
                    NumConta = c.NumConta,
                    VlrCredito = l.VlrCredito,
                    VlrDebito = l.VlrDebito,
                    VlrSaldo = l.VlrSaldo                        
                */

                DataTable dt = new DataTable();

                dt.Columns.Add("NumConta", typeof(string));
                dt.Columns.Add("VlrCredito", typeof(decimal));
                dt.Columns.Add("VlrDebito", typeof(decimal));
                dt.Columns.Add("VlrSaldo", typeof(decimal));


                //foreach (var item in result)
                //{
                //    DataRow dr = dt.NewRow();

                //    dr["NumConta"] = item.NumConta;
                //    dr["VlrCredito"] = item.VlrCredito;
                //    dr["VlrDebito"] = item.VlrDebito;
                //    dr["VlrSaldo"] = item.VlrSaldo;

                //    dt.Rows.Add(dr);
                //}
            }
        }

        static void LinqMAX()
        {
            using (var db = new Entidades())
            {
                var result =
                    (from b in db.Balancete
                     where b.IdeAgente.Equals(2210)
                     select b.AnmCompetencia).Max();

                string valor = result;


                //var result2 = (from lista in ListaTesteDados()
                //              select lista.AnmCompetencia).Max();
            }
        }

        static int LinqDISTINCT_Count()
        {
            using (var db = new Entidades())
            {
                var result =
                    (from b in db.Balancete
                     where b.IdeAgente.Equals(2210)
                     select b.AnmCompetencia).Distinct();

                return result.Count();
            }
        }

        static void LinqGroupBy()
        {
            using (var db = new Entidades())
            {
                //from m in context.MyTable
                //where m.creationtimestamp >= baselineDate
                //group m by System.Data.Entity.DbFunctions.TruncateTime(m.creationtimestamp) into grouping
                //select new { Name = grouping.Key.ToString(), Value = grouping.Count().ToString() })
                //.OrderBy(x => x.Name)
                //.ToList();                

                var destinatarios =
                   (from analista in db.AvisoNotificacaoInadimplencia
                    join agente in db.Agente on analista.IdeAgente equals agente.IdeAgente
                    join empresa in db.vPessoaJuridicaUF on agente.IdeAgente equals empresa.IdAgente
                    where analista.IdeDestinatario == 1422
                    orderby analista.DthEnvio
                    group analista by EntityFunctions.TruncateTime(analista.DthEnvio) into g
                    select new HistoricoEnvioDestinatario
                    {
                        IdeDestinatario = 1422,
                        DthEnvio = g.Key,
                        QtdRelatorios = g.Count(t => t.IdeAgente != null)
                    });



                DataTable dt = new DataTable();

                dt.Columns.Add("IdeDestinatario", typeof(int));
                dt.Columns.Add("Data", typeof(DateTime));
                dt.Columns.Add("Qtd", typeof(int));

                foreach (var item in destinatarios)
                {
                    DataRow dr = dt.NewRow();

                    dr["IdeDestinatario"] = item.IdeDestinatario;
                    dr["Data"] = item.DthEnvio;
                    dr["Qtd"] = item.QtdRelatorios;
                    dt.Rows.Add(dr);
                }
            }
        }

        static void LinqGroupBy2()
        {

            DateTime data = Convert.ToDateTime("2014-12-16");

            using (var db = new Entidades())
            {
                var destinatarios = (from analista in db.AvisoNotificacaoInadimplencia
                                     join agente in db.Agente on analista.IdeAgente equals agente.IdeAgente
                                     join empresa in db.vPessoaJuridicaUF on agente.IdeAgente equals empresa.IdAgente
                                     join periodo in db.PeriodoEnvioArquivo on analista.IdePeriodoEnvioArquivo equals periodo.IdePeriodoEnvioArquivo
                                     where analista.IdeDestinatario == 1422 && analista.DthEnvio == data

                                     orderby empresa.RazaoSocialPJ, periodo.AnmCompetencia

                                     select new HistoricoEnvioDestinatario
                                     {
                                         AnoMesCompetencia = periodo.AnmCompetencia,
                                         NomeEmpresa = (empresa.SiglaPJ + " " + empresa.RazaoSocialPJ).Trim()

                                     }).ToList();

                DataTable dt = new DataTable();

                dt.Columns.Add("AnoMesCompetencia", typeof(string));
                dt.Columns.Add("NomeEmpresa", typeof(string));

                foreach (var item in destinatarios)
                {
                    DataRow dr = dt.NewRow();
                    dr["AnoMesCompetencia"] = item.AnoMesCompetencia;
                    dr["NomeEmpresa"] = item.NomeEmpresa;
                    dt.Rows.Add(dr);
                }
            }
        }

        static bool PeriodoEntre12Meses(DateTime dTimeIni, DateTime dTimeFim)
        {
            DateTime dt__mais_12meses = dTimeIni.AddMonths(12).AddDays(-1);
            //int difMeses = Convert.ToInt32(dt__mais_12meses.Subtract(dTimeIni).TotalDays) / 30;
            DateTime dTimeCompara = new DateTime(dt__mais_12meses.Year, dt__mais_12meses.Month, 01, 00, 00, 00);

            if (dTimeFim > dTimeCompara)
                return false;
            else
                return true;
        }

        static void DateTimeCalcDifference()
        {
            //1 TOTAL DIAS
            DateTime a = new DateTime(2014, 03, 01, 00, 00, 00);
            DateTime b = new DateTime(2015, 03, 01, 00, 00, 00);

            //DateTime a = new DateTime(2014, 08, 04, 00, 00, 00);
            //DateTime b = new DateTime(2014, 08, 08, 00, 00, 00);

            Double dias = b.Subtract(a).TotalDays;
            int IntDias = Convert.ToInt32(b.Subtract(a).TotalDays);

            //2. Total Meses


            DateTime dt__mais_12meses = a.AddMonths(12).AddDays(-1);
            int difMeses = Convert.ToInt32(dt__mais_12meses.Subtract(a).TotalDays) / 30;
            DateTime dTimeCompara = new DateTime(dt__mais_12meses.Year, dt__mais_12meses.Month, 01, 00, 00, 00);

            bool MaiorQue12;

            if (b > dTimeCompara)
                MaiorQue12 = true;
            else
                MaiorQue12 = false;

        }

        static void DateTimeCalcAnos()
        {
            string tipo = "";

            int ano = 2014;
            int mes = 12;

            DateTime dt_Inicio_Relatorio = new DateTime(ano, mes, 1);
            DateTime dt_Menos_11_Meses = new DateTime(ano, mes, 1).AddMonths(-11);

            if (dt_Inicio_Relatorio.Year < 2015) tipo = "antigo";
            else if (dt_Inicio_Relatorio.Year > 2014 && dt_Menos_11_Meses.Year > 2014) tipo = "novo";
            else if (dt_Inicio_Relatorio.Year > 2014 && dt_Menos_11_Meses.Year < 2015) tipo = "ambos";
        }

        static void TesteJson()
        {

            int dias = 8;
            string dataInicioEnvio = "03/04/1973";
            string obs = "";
            DateTime DataInicio = DateTime.Now;

            obs = "Data final inválida: A data final calculada: " + DataInicio.AddDays(dias).ToString("dd/MM/yyyy") + " não é dia útil";

            string dados = "{\"dias\":\"" + dias
                + "\",\"dataInicioEnvio\":\"" + dataInicioEnvio
                + "\",\"dataFimEnvio\":\"" + DataInicio.AddDays(dias).ToString("dd/MM/yyyy")
                + "\",\"obs\":\"" + obs + "\"}";

        }

        static bool EhDiaUtil(DateTime data)
        {
            return
                data.DayOfWeek != DayOfWeek.Saturday &&
                data.DayOfWeek != DayOfWeek.Sunday;
        }

        static bool temRestricao;

        static void foo()
        {
            int i = 0;

            if (i > 0)
            {
                temRestricao = true;
            }
            else
            {
                temRestricao = false;
            }
        }

        static void BooleanJerkTest()
        {

            bool valido;
            /*
            string usuario = "ZZZ";
            

            if (usuario != "XXX" && usuario != "AAA")
            {
                valido = false;
            }

            if (usuario == "AAA" || usuario == "ZZZ"
                || usuario == "MMM" || usuario == "FFF")
                valido = true;
            else
                valido = false;

            //RctMdlVO.IdeRct > 0 && RctMdlVO.FlgTrfRct
            bool passou = (101292 > 0 && true);
            */

            //int mes = 3;

            //if (mes == 12 || mes == 1 || mes == 2)
            //{
            //    valido = true;
            //}


            //if ((1997 > DateTime.Now.Year) || (1997 < 1998))
            //{
            //    bool invalido = true;
            //}


            if (1 > 0)
            {
                valido = true;
            }


            if (201407 <= 201406)
            {

            }


            //if (!string.IsNullOrEmpty(UltimaCompetenciaEnvio) && QtdCompetenciaEnvio > 1)

            string UltimaCompetenciaEnvio = "";
            int QtdCompetenciaEnvio = 0;

            if (!string.IsNullOrEmpty(UltimaCompetenciaEnvio) && QtdCompetenciaEnvio > 1)
            {
                bool temCompetenciaAnterior = true;
            }


        }

        static void PADLEFT()
        {
            //7195000000
            string data = "2213";

            string result = data.PadLeft(10, '0');

            //StringBuilder sb = new StringBuilder();
            //sb.Append(" FRASE GRANDE");
            //sb.Append(" teste teste teste teste teste teste teste test ");
            //sb.Append(" The process is repeatable, so once the CheckBox is unchecked, checking it will fire the event. ");

            //string teste = sb.ToString();
        }

        static void UltimoDiaMes()
        {
            string valor = "11/2014";

            int ano = Convert.ToInt32(valor.Substring(3, 4));
            int mes = Convert.ToInt32(valor.Substring(0, 2));

            DateTime DtNow = Convert.ToDateTime("01/" + mes.ToString().PadLeft(2, '0') + "/" + ano);

            DateTime DtPrimeiroDiaProximoMes = DtNow.AddMonths(1);

            DateTime DtUltimoDiaMes = DtPrimeiroDiaProximoMes.AddDays(-1);
        }

        static bool ValidaData(string DataValida)
        {
            string data = SomenteNumeros(DataValida);
            int iDia = Convert.ToInt32(data.Substring(0, 2));
            int iMes = Convert.ToInt32(data.Substring(2, 2));
            int iAno = Convert.ToInt32(data.Substring(4, 4));

            if (iAno.ToString().Length < 4) return false;
            if ((iAno - 1980 < 0) && (iAno - 2080 > 0)) return false;
            if ((iMes - 12) > 0 || (iMes == 0)) return false;
            if (iMes == 2) return (((iDia - 1 >= 0) && (iDia - 28 <= 0)) || ((iDia == 29) && (Convert.ToDouble(iAno) % 4 == 0)));
            if ((iMes == 1) || (iMes == 3) || (iMes == 5) || (iMes == 7) || (iMes == 8) || (iMes == 10) || (iMes == 2)) return ((iDia - 1 >= 0) && (iDia - 31 <= 0));
            if ((iMes == 4) || (iMes == 6) || (iMes == 9) || (iMes == 11)) return ((iDia - 1 >= 0) && (iDia - 30 <= 0));

            return true;
        }

        static string SomenteNumeros(string sTexto)
        {
            string NovaString = String.Empty;
            foreach (char caracter in sTexto.ToCharArray())
                if (char.IsDigit(caracter)) NovaString += caracter;

            return sTexto = NovaString;
        }

        static void GeraArquivos()
        {

            StreamReader sr = null;
            StringBuilder builderDuplicidade = new StringBuilder();

            StringBuilder builderScriptUpdate = new StringBuilder();

            StringBuilder builderIdComUF = new StringBuilder();

            //string nomeArquivo = "ListaContatosGerais.txt";
            string nomeArquivo = "OUTORGAS__Emp_UF.csv";

            //Gera Pasta
            string pasta = AppDomain.CurrentDomain.BaseDirectory + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo);

            try
            {
                if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

                //sr = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + "\\" + nomeArquivo);
                sr = File.OpenText(@"C:\Users\fredsena\Documents\FRED_STUFF\OUTORGAS_\OUTORGAS__Emp_UF.csv");

                string str = "";
                string[] linha = null;
                //int IdTemp = 0;

                //loop
                while (str != null)
                {
                    str = sr.ReadLine();
                    if (str == null) break;

                    linha = str.Split(';');
                    //if (str.IndexOf("mail: ") == -1) continue;

                    builderScriptUpdate.AppendLine("UPDATE Empreendimento SET IdeUfPrincipal = '" + linha[1].ToUpper() + "' WHERE idEmpreendimento = " + linha[0]);

                    /*
                    if (Convert.ToInt32(linha[0]) == IdTemp)
                    {
                        //Registros DUPLICADO
                        builderDuplicidade.Append(linha[0]);
                        builderDuplicidade.Append(", ");                        
                    }
                    else
                    {
                        //Registro SEM duplicidade: Gera script BD e lista de id
                        builderScriptUpdate.AppendLine("UPDATE Empreendimento SET IdeUfPrincipal = '" + linha[1].ToUpper() + "' WHERE idEmpreendimento = " + linha[0]);
                        //builderIdComUF.Append(linha[0]);
                        //builderIdComUF.Append(", ");
                    }
                    IdTemp = Convert.ToInt32(linha[0]);
                    */
                }
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {

                //Cria arquivo SCRIPT Update
                string filePath = pasta + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo) + "_ScriptUpdate.txt";
                File.WriteAllText(filePath, builderScriptUpdate.ToString().Substring(0, builderScriptUpdate.Length));

                //filePath = pasta + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo) + "_Duplicidade.txt";
                //File.WriteAllText(filePath, builderDuplicidade.ToString().Substring(0, builderDuplicidade.ToString().Length - 2));

                //filePath = pasta + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo) + "_IdComUF.txt";
                //File.WriteAllText(filePath, builderIdComUF.ToString().Substring(0, builderIdComUF.ToString().Length - 2));

                sr.Close();
                //builderIdComUF = null;
                //builderDuplicidade = null;
                builderScriptUpdate = null;
            }
        }

        static void BuscaDuplicados()
        {
            //DISTINCT
            //int[] listOfItems = new[] { 1, 2, 3, 6, 8, 9, 10, 12, 14, 15, 16, 17, 19, 23, 27, 30, 31, 33, 37, 38, 40, 43, 47, 48, 49, 50, 53, 55, 57, 58, 60, 61, 63, 65, 68, 71, 72, 74, 75, 76, 77, 78, 80, 81, 82, 83, 84, 85, 87, 89, 90, 91, 92, 98, 99, 100, 101, 103, 104, 105, 109, 110, 112, 114, 115, 116, 118, 119, 120, 122, 124, 127, 129, 135, 136, 139, 140, 141, 145, 152, 161, 162, 165, 168, 169, 171, 175, 176, 177, 178, 179, 180, 181, 182, 184, 186, 187, 190, 193, 194, 196, 198, 201, 203, 207, 208, 212, 215, 217, 218, 225, 227, 228, 230, 232, 233, 234, 235, 236, 237, 239, 241, 243, 247, 253, 255, 258, 259, 261, 262, 263, 264, 265, 266, 269, 270, 272, 273, 274, 277, 284, 285, 287, 288, 292, 297, 298, 302, 307, 311, 312, 313, 315, 316, 317, 318, 319, 322, 323, 324, 326, 328, 329, 331, 333, 336, 338, 340, 342, 343, 344, 345, 349, 351, 353, 354, 379, 392, 395, 396, 397, 400, 404, 406, 426, 428, 429, 461, 462, 473, 475, 476, 491, 503, 512, 524, 526, 534, 561, 573, 581, 587, 588, 590, 592, 597, 598, 600, 601, 602, 604, 607, 608, 609, 610, 612, 614, 620, 623, 625, 626, 630, 632, 634, 635, 636, 640, 641, 642, 643, 646, 648, 650, 653, 654, 655, 659, 662, 663, 665, 666, 667, 670, 671, 674, 677, 678, 679, 680, 681, 683, 688, 700, 702, 705, 706, 708, 710, 711, 713, 714, 715, 717, 718, 719, 721, 723, 725, 726, 728, 729, 730, 733, 735, 737, 739, 740, 742, 744, 746, 747, 748, 750, 751, 753, 754, 759, 760, 762, 765, 766, 768, 769, 771, 775, 776, 778, 781, 782, 783, 784, 786, 788, 789, 793, 796, 797, 798, 803, 804, 805, 806, 807, 808, 809, 810, 812, 816, 817, 818, 819, 820, 824, 825, 827, 831, 832, 833, 835, 836, 838, 840, 842, 845, 866, 867, 868, 869, 871, 872, 876, 877, 878, 879, 880, 884, 891, 892, 893, 894, 896, 898, 904, 908, 912, 915, 922, 923, 928, 931, 933, 935, 937, 938, 939, 944, 945, 948, 949, 950, 951, 953, 954, 955, 956, 957, 958, 959, 960, 961, 962, 968, 969, 970, 972, 973, 975, 976, 977, 978, 980, 984, 990, 994, 995, 999, 1000, 1001, 1006, 1007, 1009, 1011, 1013, 1015, 1016, 1017, 1018, 1019, 1023, 1025, 1026, 1027, 1032, 1033, 1034, 1035, 1036, 1041, 1042, 1046, 1047, 1055, 1057, 1060, 1061, 1066, 1067, 1071, 1073, 1075, 1076, 1078, 1079, 1081, 1083, 1084, 1085, 1086, 1088, 1089, 1090, 1093, 1096, 1097, 1100, 1101, 1102, 1103, 1104, 1105, 1108, 1111, 1116, 1119, 1122, 1126, 1127, 1128, 1137, 1138, 1139, 1141, 1142, 1143, 1146, 1147, 1148, 1149, 1150, 1157, 1161, 1166, 1175, 1178, 1181, 1185, 1190, 1192, 1193, 1195, 1196, 1197, 1198, 1199, 1202, 1204, 1210, 1211, 1213, 1215, 1217, 1220, 1223, 1231, 1234, 1235, 1238, 1239, 1240, 1242, 1243, 1244, 1245, 1249, 1252, 1253, 1255, 1256, 1257, 1258, 1260, 1264, 1265, 1268, 1269, 1272, 1273, 1276, 1277, 1278, 1279, 1281, 1283, 1284, 1285, 1286, 1287, 1288, 1291, 1298, 1301, 1302, 1304, 1305, 1306, 1310, 1311, 1313, 1317, 1320, 1322, 1325, 1328, 1329, 1331, 1334, 1335, 1336, 1339, 1341, 1342, 1343, 1344, 1345, 1347, 1349, 1350, 1353, 1355, 1358, 1361, 1362, 1364, 1366, 1367, 1368, 1369, 1370, 1371, 1373, 1374, 1375, 1377, 1380, 1381, 1382, 1383, 1384, 1386, 1387, 1389, 1393, 1394, 1397, 1398, 1399, 1401, 1403, 1405, 1406, 1409, 1410, 1412, 1413, 1414, 1416, 1419, 1420, 1421, 1423, 1427, 1430, 1431, 1433, 1437, 1438, 1441, 1445, 1446, 1447, 1449, 1452, 1454, 1455, 1458, 1462, 1468, 1469, 1472, 1474, 1475, 1479, 1483, 1484, 1486, 1487, 1489, 1495, 1497, 1499, 1501, 1504, 1506, 1509, 1512, 1517, 1518, 1519, 1520, 1522, 1523, 1525, 1526, 1528, 1529, 1532, 1533, 1535, 1536, 1543, 1544, 1547, 1548, 1549, 1552, 1553, 1554, 1559, 1560, 1562, 1565, 1567, 1569, 1570, 1571, 1572, 1573, 1574, 1579, 1581, 1583, 1584, 1585, 1586, 1587, 1590, 1591, 1593, 1595, 1597, 1598, 1600, 1606, 1607, 1609, 1910, 1911, 1912, 1922, 1924, 1927, 1928, 1932, 1934, 1937, 1938, 1939, 1940, 1943, 1945, 1946, 1947, 1948, 1952, 1957, 1959, 1963, 1970, 1972, 1973, 1974, 1975, 1977, 1978, 1987, 1988, 1989, 1991, 1996, 1997, 1998, 1999, 2001, 2003, 2005, 2006, 2007, 2008, 2009, 2013, 2014, 2016, 2019, 2020, 2022, 2023, 2024, 2028, 2031, 2038, 2039, 2040, 2041, 2042, 2043, 2045, 2046, 2047, 2049, 2051, 2052, 2053, 2054, 2055, 2056, 2057, 2059, 2060, 2061, 2065, 2069, 2070, 2071, 2075, 2076, 2077, 2082, 2085, 2087, 2088, 2089, 2091, 2092, 2094, 2096, 2098, 2102, 2104, 2107, 2109, 2111, 2113, 2114, 2115, 2118, 2119, 2120, 2121, 2122, 2123, 2124, 2126, 2128, 2130, 2132, 2133, 2134, 2135, 2136, 2137, 2139, 2142, 2143, 2144, 2146, 2150, 2154, 2155, 2158, 2159, 2161, 2164, 2166, 2167, 2173, 2177, 2179, 2180, 2183, 2187, 2190, 2192, 2195, 2196, 2197, 2200, 2202, 2205, 2208, 2215, 2225, 2238, 2271, 2277, 2280, 2292, 2293, 2300, 2306, 2317, 2329, 2336, 2340, 2344, 2345, 2346, 2348, 2353, 2354, 2360, 2368, 2394, 2398, 2406, 2411, 2413, 2414, 2417, 2423, 2428, 2430, 2432, 2442, 2453, 2470, 2485, 2496, 2503, 2514, 2523, 2530, 2532, 2534, 2535, 2537, 2542, 2543, 2544, 2547, 2550, 2552, 2556, 2557, 2558, 2563, 2564, 2565, 2568, 2569, 2570, 2572, 2574, 2576, 2581, 2586, 2587, 2590, 2591, 2592, 2593, 2594, 2596, 2607, 2623, 2626, 2627, 2628, 2630, 2633, 2638, 2641, 2642, 2654, 2656, 2659, 2665, 2670, 2672, 2673, 2675, 2682, 2683, 2684, 2685, 2686, 2687, 2688, 2693, 2696, 2697, 2698, 2699, 2700, 2701, 2703, 2707, 2708, 2710, 2712, 2714, 2715, 2717, 2719, 2726, 2731, 2736, 2738, 2740, 2746, 2751, 2754, 2755, 2756, 2757, 2758, 2759, 2762, 2764, 2767, 2771, 2772, 2775, 2779, 2780, 2781, 2782, 2785, 2786, 2787, 2788, 2789, 2790, 2792, 2796, 2800, 2801, 2802, 2804, 2806, 2812, 2814, 2817, 2820, 2823, 2824, 2826, 2827, 2828, 2829, 2832, 2834, 2838, 2839, 2840, 2843, 2847, 2852, 2853, 2855, 2856, 2859, 2862, 2863, 2866, 2867, 2870, 2871, 2873, 2877, 2879, 2880, 2886, 2887, 2888, 2889, 2890, 2891, 2892, 2896, 2897, 2901, 2903, 2904, 2906, 2911, 2912, 2913, 2935, 2941, 2946, 2949, 2952, 2954, 2955, 2958, 2968, 2969, 2975, 2980, 2982, 2986, 2987, 2988, 2990, 2991, 2992, 2993, 2994, 2996, 2998, 3001, 3002, 3005, 3006, 3011, 3012, 3013, 3014, 3020, 3023, 3024, 3027, 3029, 3032, 3035, 3037, 3038, 3039, 3042, 3044, 3047, 3048, 3056, 3058, 3059, 3060, 26701, 26705, 26710, 26711, 26713, 26715, 26716, 26721, 26722, 26723, 26724, 26725, 26730, 26731, 26732, 26734, 26735, 26781, 26782, 26795, 26798, 26800, 26809, 26811, 26812, 26817, 26818, 26825, 26826, 26827, 26828, 26829, 26832, 26834, 26837, 26839, 26840, 26842, 26844, 26847, 26848, 26849, 26851, 26852, 26854, 26857, 26862, 26863, 26866, 26867, 26869, 26874, 26875, 26878, 26879, 26880, 26885, 26888, 27002, 27003, 27005, 27006, 27007, 27008, 27009, 27010, 27011, 27012, 27013, 27016, 27019, 27020, 27024, 27034, 27037, 27040, 27042, 27044, 27046, 27047, 27052, 27054, 27055, 27057, 27058, 27059, 27061, 27062, 27064, 27065, 27066, 27070, 27072, 27073, 27074, 27075, 27079, 27080, 27081, 27086, 27087, 27088, 27089, 27090, 27093, 27094, 27095, 27096, 27099, 27100, 27102, 27104, 27107, 27108, 27109, 27110, 27111, 27112, 27113, 27114, 27116, 27118, 27119, 27120, 27122, 27125, 27126, 27127, 27128, 27129, 27130, 27131, 27132, 27133, 27139, 27140, 27141, 27142, 27143, 27144, 27145, 27146, 27147, 27150, 27152, 27153, 27159, 27161, 27162, 27165, 27169, 27172, 27174, 27175, 27176, 27177, 27179, 27182, 27184, 27185, 27186, 27188, 27190, 27194, 27195, 27196, 27199, 27200, 27201, 27204, 27207, 27209, 27210, 27213, 27214, 27216, 27217, 27219, 27220, 27221, 27225, 27227, 27228, 27231, 27232, 27233, 27234, 27235, 27236, 27242, 27243, 27244, 27245, 27248, 27250, 27253, 27257, 27258, 27259, 27263, 27265, 27266, 27267, 27271, 27274, 27288, 27293, 27298, 27299, 27319, 27320, 27325, 27326, 27330, 27333, 27335, 27337, 27338, 27339, 27342, 27343, 27345, 27352, 27354, 27355, 27356, 27357, 27367, 27370, 27377, 27380, 27381, 27383, 27384, 27385, 27386, 27387, 27388, 27389, 27391, 27392, 27395, 27396, 27399, 27401, 27402, 27405, 27411, 27414, 27415, 27416, 27417, 27421, 27422, 27430, 27435, 27442, 27443, 27444, 27446, 27447, 27448, 27455, 27463, 27473, 27476, 27483, 27484, 27494, 27495, 27497, 27505, 27509, 27510, 27530, 27532, 27533, 27534, 27535, 27536, 27537, 27546, 27547, 27548, 27549, 27550, 27551, 27552, 27557, 27567, 27569, 27571, 27572, 27573, 27576, 27580, 27586, 27587, 27591, 27596, 27598, 27599, 27603, 27604, 27613, 27614, 27615, 27618, 27619, 27620, 27623, 27630, 27632, 27635, 27637, 27639, 27642, 27643, 27645, 27646, 27647, 27651, 27652, 27653, 27654, 27656, 27665, 27669, 27670, 27671, 27673, 27677, 27687, 27689, 27701, 27705, 27706, 27707, 27708, 27709, 27710, 27711, 27712, 27713, 27719, 27722, 27726, 27727, 27728, 27729, 27730, 27731, 27732, 27733, 27734, 27735, 27739, 27750, 27751, 27755, 27757, 27758, 27759, 27761, 27765, 27766, 27767, 27768, 27769, 27771, 27772, 27787, 27788, 27789, 27791, 27793, 27794, 27795, 27796, 27800, 27804, 27814, 27820, 27821, 27822, 27828, 27829, 27835, 27842, 27843, 27846, 27852, 27853, 27854, 27855, 27857, 27858, 27860, 27862, 27863, 27864, 27867, 27870, 27871, 27874, 27875, 27876, 27877, 27878, 27879, 27880, 27881, 27883, 27884, 27885, 27886, 27887, 27888, 27889, 27890, 27891, 27892, 27893, 27894, 27895, 27896, 27898, 27899, 27900, 27901, 27902, 27903, 27905, 27906, 27908, 27910, 27911, 27912, 27913, 27914, 27915, 27917, 27918, 27919, 27920, 27922, 27923, 27924, 27925, 27926, 27927, 27928, 27929, 27932, 27933, 27934, 27935, 27936, 27937, 27938, 27939, 27940, 27941, 27943, 27944, 27945, 27946, 27947, 27948, 27949, 27950, 27951, 27952, 27956, 27957, 27958, 27960, 27963, 27964, 27965, 27966, 27968, 27969, 27970, 27971, 27972, 27974, 27975, 27977, 27978, 27982, 27983, 27984, 27985, 27986, 27987, 27992, 27993, 27995, 27996, 27997, 27998, 27999, 28000, 28001, 28002, 28003, 28004, 28005, 28007, 28008, 28009, 28010, 28011, 28012, 28013, 28014, 28016, 28017, 28018, 28019, 28020, 28021, 28022, 28024, 28026, 28027, 28028, 28029, 28030, 28031, 28033, 28034, 28036, 28037, 28038, 28039, 28041, 28042, 28043, 28044, 28045, 28046, 28047, 28048, 28049, 28050, 28051, 28052, 28053, 28055, 28056, 28057, 28058, 28059, 28060, 28061, 28062, 28063, 28064, 28065, 28066, 28067, 28071, 28072, 28073, 28074, 28076, 28077, 28078, 28079, 28081, 28082, 28083, 28084, 28085, 28086, 28089, 28090, 28091, 28093, 28094, 28096, 28097, 28098, 28099, 28101, 28102, 28104, 28105, 28106, 28107, 28109, 28110, 28111, 28112, 28113, 28114, 28115, 28116, 28117, 28118, 28119, 28120, 28121, 28122, 28123, 28124, 28125, 28126, 28127, 28128, 28129, 28130, 28131, 28132, 28133, 28134, 28135, 28136, 28137, 28139, 28140, 28141, 28143, 28144, 28145, 28146, 28147, 28148, 28149, 28150, 28151, 28152, 28153, 28154, 28155, 28156, 28157, 28158, 28159, 28160, 28161, 28162, 28163, 28164, 28167, 28168, 28169, 28170, 28171, 28172, 28173, 28174, 28175, 28177, 28178, 28179, 28180, 28181, 28182, 28183, 28184, 28185, 28186, 28187, 28188, 28189, 28190, 28191, 28192, 28193, 28194, 28195, 28197, 28199, 28200, 28201, 28202, 28203, 28204, 28205, 28206, 28207, 28208, 28209, 28210, 28211, 28213, 28214, 28215, 28216, 28217, 28218, 28219, 28220, 28221, 28222, 28223, 28224, 28225, 28226, 28227, 28228, 28229, 28230, 28231, 28232, 28233, 28234, 28235, 28236, 28237, 28238, 28239, 28240, 28241, 28242, 28245, 28246, 28247, 28248, 28249, 28250, 28251, 28253, 28254, 28255, 28257, 28259, 28275, 28276, 28277, 28278, 28285, 28286, 28287, 28288, 28305, 28308, 28313, 28323, 28324, 28325, 28326, 28327, 28328, 28329, 28330, 28332, 28333, 28334, 28335, 28336, 28337, 28338, 28339, 28340, 28341, 28342, 28343, 28344, 28345, 28346, 28347, 28348, 28349, 28350, 28351, 28352, 28353, 28355, 28356, 28357, 28358, 28359, 28360, 28361, 28362, 28363, 28364, 28365, 28366, 28367, 28368, 28369, 28370, 28371, 28372, 28373, 28374, 28375, 28376, 28377, 28378, 28380, 28381, 28382, 28383, 28384, 28385, 28386, 28387, 28388, 28389, 28390, 28391, 28392, 28393, 28395, 28396, 28397, 28398, 28399, 28400, 28401, 28402, 28403, 28404, 28405, 28406, 28407, 28408, 28409, 28410, 28411, 28412, 28413, 28414, 28415, 28416, 28417, 28418, 28419, 28420, 28421, 28422, 28423, 28424, 28425, 28426, 28427, 28428, 28429, 28430, 28431, 28432, 28433, 28434, 28436, 28437, 28438, 28439, 28440, 28441, 28442, 28443, 28444, 28445, 28446, 28447, 28448, 28449, 28450, 28451, 28452, 28453, 28454, 28455, 28456, 28457, 28458, 28459, 28460, 28461, 28462, 28463, 28464, 28465, 28466, 28467, 28468, 28476, 28477, 28478, 28479, 28480, 28481, 28482, 28483, 28484, 28485, 28486, 28487, 28488, 28489, 28490, 28491, 28492, 28493, 28494, 28495, 28496, 28497, 28498, 28499, 28500, 28501, 28502, 28503, 28504, 28505, 28506, 28507, 28508, 28509, 28510, 28514, 28515, 28516, 28527, 28528, 28529, 28530, 28531, 28532, 28533, 28534, 28535, 28536, 28537, 28538, 28539, 28540, 28541, 28542, 28543, 28544, 28545, 28546, 28548, 28549, 28550, 28551, 28552, 28553, 28554, 28555, 28556, 28557, 28558, 28559, 28560, 28562, 28564, 28565, 28567, 28569, 28570, 28572, 28573, 28574, 28575, 28576, 28577, 28578, 28579, 28580, 28581, 28582, 28584, 28585, 28586, 28587, 28588, 28589, 28590, 28592, 28593, 28594, 28595, 28596, 28597, 28598, 28599, 28600, 28601, 28602, 28604, 28605, 28606, 28607, 28608, 28609, 28610, 28611, 28612, 28613, 28614, 28615, 28616, 28617, 28618, 28619, 28620, 28621, 28622, 28623, 28624, 28625, 28626, 28627, 28628, 28629, 28630, 28631, 28632, 28634, 28635, 28636, 28637, 28640, 28642, 28643, 28644, 28645, 28646, 28647, 28648, 28649, 28650, 28651, 28652, 28653, 28654, 28655, 28656, 28657, 28658, 28659, 28660, 28662, 28663, 28664, 28665, 28666, 28667, 28668, 28669, 28670, 28671, 28672, 28673, 28674, 28680, 28681, 28682, 28683, 28684, 28685, 28686, 28687, 28688, 28689, 28690, 28691, 28692, 28693, 28694, 28695, 28696, 28697, 28698, 28699, 28700, 28701, 28702, 28703, 28704, 28705, 28706, 28707, 28708, 28710, 28711, 28712, 28713, 28714, 28715, 28716, 28717, 28718, 28719, 28720, 28721, 28722, 28723, 28724, 28725, 28726, 28727, 28728, 28729, 28730, 28731, 28732, 28733, 28734, 28735, 28736, 28738, 28739, 28740, 28741, 28742, 28743, 28744, 28745, 28746, 28747, 28748, 28749, 28750, 28753, 28754, 28755, 28756, 28757, 28758, 28759, 28760, 28761, 28762, 28763, 28764, 28765, 28766, 28767, 28768, 28769, 28770, 28771, 28772, 28773, 28774, 28775, 28776, 28777, 28778, 28779, 28780, 28781, 28782, 28783, 28784, 28785, 28786, 28787, 28788, 28789, 28790, 28791, 28792, 28793, 28794, 28795, 28796, 28797, 28798, 28799, 28800, 28801, 28802, 28803, 28806, 28807, 28809, 28810, 28811, 28812, 28813, 28814, 28815, 28816, 28817, 28818, 28819, 28820, 28821, 28822, 28823, 28824, 28825, 28826, 28827, 28828, 28829, 28830, 28831, 28832, 28833, 28834, 28835, 28836, 28837, 28838, 28839, 28840, 28841, 28842, 28843, 28844, 28845, 28846, 28847, 28848, 28849, 28850, 28851, 28852, 28853, 28854, 28855, 28856, 28857, 28858, 28859, 28860, 28861, 28862, 28864, 28865, 28866, 28867, 28868, 28870, 28871, 28872, 28874, 28875, 28876, 28877, 28878, 28879, 28880, 28881, 28882, 28883, 28884, 28885, 28886, 28887, 28888, 28889, 28890, 28891, 28893, 28894, 28895, 28896, 28897, 28898, 28899, 28900, 28901, 28902, 28903, 28904, 28905, 28907, 28908, 28909, 28910, 28911, 28912, 28914, 28915, 28916, 28917, 28918, 28919, 28920, 28921, 28922, 28923, 28924, 28925, 28926, 28927, 28928, 28929, 28930, 28931, 28932, 28933, 28934, 28935, 28936, 28937, 28938, 28939, 28940, 28941, 28942, 28943, 28944, 28945, 28946, 28947, 28948, 28949, 28950, 28951, 28952, 28953, 28954, 28955, 28957, 28958, 28959, 28960, 28961, 28962, 28963, 28964, 28965, 28966, 28967, 28968, 28969, 28970, 28971, 28972, 28973, 28974, 28975, 28976, 28977, 28978, 28979, 28980, 28981, 28982, 28983, 28984, 28985, 28986, 28987, 28988, 28989, 28990, 28991, 28992, 28993, 28994, 28995, 28997, 28998, 28999, 29000, 29001, 29002, 29003, 29004, 29005, 29006, 29007, 29008, 29009, 29010, 29011, 29012, 29013, 29014, 29015, 29016, 29017, 29018, 29019, 29020, 29021, 29022, 29023, 29024, 29025, 29026, 29027, 29028, 29029, 29030, 29031, 29032, 29033, 29034, 29035, 29036, 29037, 29038, 29039, 29040, 29041, 29042, 29043, 29044, 29045, 29046, 29047, 29048, 29049, 29050, 29051, 29052, 29053, 29054, 29055, 29056, 29057, 29058, 29059, 29060, 29061, 29062, 29063, 29064, 29065, 29066, 29067, 29068, 29069, 29070, 29071, 29072, 29073, 29074, 29075, 29076, 29077, 29078, 29079, 29080, 29081, 29082, 29083, 29084, 29085, 29086, 29087, 29088, 29089, 29090, 29091, 29092, 29093, 29094, 29095, 29096, 29097, 29098, 29099, 29100, 29101, 29102, 29103, 29104, 29105, 29106, 29107, 29108, 29109, 29111, 29112, 29113, 29114, 29115, 29116, 29117, 29118, 29119, 29120, 29121, 29122, 29123, 29124, 29125, 29126, 29127, 29128, 29129, 29130, 29131, 29132, 29133, 29134, 29135, 29136, 29137, 29138, 29139, 29140, 29141, 29142, 29143, 29144, 29145, 29146, 29147, 29148, 29150, 29151, 29152, 29153, 29154, 29155, 29156, 29157, 29158, 29159, 29160, 29161, 29162, 29163, 29164, 29165, 29166, 29168, 29169, 29170, 29171, 29172, 29173, 29175, 29176, 29177, 29178, 29179, 29180, 29181, 29182, 29183, 29184, 29185, 29186, 29187, 29188, 29189, 29190, 29191, 29192, 29193, 29194, 29195, 29196, 29197, 29198, 29199, 29200, 29201, 29202, 29203, 29204, 29205, 29206, 29207, 29208, 29209, 29210, 29211, 29212, 29213, 29214, 29215, 29216, 29217, 29218, 29219, 29220, 29221, 29222, 29223, 29224, 29225, 29226, 29227, 29228, 29229, 29230, 29231, 29232, 29233, 29234, 29235, 29236, 29237, 29238, 29239, 29240, 29241, 29242, 29243, 29244, 29245, 29246, 29247, 29248, 29249, 29250, 29251, 29252, 29253, 29254, 29255, 29256, 29257, 29258, 29259, 29260, 29261, 29262, 29263, 29264, 29265, 29266, 29267, 29268, 29269, 29270, 29271, 29272, 29273, 29274, 29275, 29276, 29277, 29278, 29279, 29280, 29281, 29282, 29283, 29285, 29286, 29287, 29289, 29290, 29291, 29292, 29293, 29294, 29295, 29296, 29297, 29298, 29299, 29300, 29301, 29302, 29303, 29304, 29305, 29306, 29307, 29308, 29309, 29310, 29311, 29312, 29313, 29314, 29315, 29316, 29317, 29318, 29319, 29320, 29321, 29322, 29323, 29324, 29325, 29326, 29327, 29328, 29329, 29330, 29331, 29332, 29333, 29334, 29335, 29336, 29337, 29338, 29339, 29340, 29341, 29343, 29344, 29345, 29346, 29347, 29348, 29349, 29350, 29351, 29352, 29353, 29354, 29355, 29356, 29357, 29358, 29359, 29360, 29361, 29362, 29363, 29364, 29365, 29366, 29367, 29368, 29369, 29370, 29371, 29372, 29373, 29374, 29375, 29376, 29377, 29378, 29379, 29380, 29381, 29382, 29383, 29384, 29385, 29386, 29387, 29388, 29389, 29390, 29391, 29392, 29393, 29394, 29395, 29396, 29397, 29398, 29399, 29400, 29401, 29402, 29403, 29404, 29405, 29406, 29407, 29408, 29409, 29410, 29411, 29412, 29413, 29414, 29415, 29416, 29417, 29418, 29419, 29420, 29421, 29422, 29423, 29424, 29425, 29426, 29427, 29428, 29429, 29430, 29431, 29432, 29433, 29434, 29435, 29436, 29437, 29438, 29439, 29440, 29441, 29442, 29443, 29444, 29445, 29446, 29447, 29448, 29449, 29450, 29451, 29452, 29453, 29455, 29456, 29457, 29459, 29460, 29461, 29462, 29469, 29470, 29471, 29472, 29473, 29474, 29475, 29476, 29477, 29478, 29479, 29480, 29481, 29482, 29484, 29485, 29486, 29487, 29488, 29489, 29490, 29491, 29492, 29493, 29494, 29495, 29496, 29497, 29498, 29499, 29500, 29501, 29502, 29503, 29504, 29505, 29506, 29507, 29508, 29509, 29510, 29511, 29512, 29513, 29514, 29515, 29516, 29517, 29518, 29519, 29520, 29521, 29522, 29523, 29524, 29525, 29526, 29527, 29528, 29529, 29530, 29531, 29532, 29533, 29534, 29535, 29536, 29537, 29538, 29539, 29540, 29541, 29542, 29543, 29544, 29545, 29546, 29548, 29549, 29550, 29551, 29552, 29553, 29554, 29555, 29556, 29557, 29558, 29559, 29560, 29561, 29562, 29563, 29564, 29565, 29566, 29567, 29568, 29569, 29570, 29571, 29572, 29573, 29574, 29575, 29576, 29577, 29578, 29579, 29580, 29581, 29582, 29583, 29584, 29585, 29586, 29587, 29588, 29589, 29590, 29591, 29592, 29594, 29595, 29596, 29597, 29598, 29599, 29600, 29601, 29602, 29603, 29604, 29605, 29607, 29608, 29609, 29610, 29611, 29612, 29613, 29614, 29615, 29616, 29617, 29618, 29619, 29620, 29621, 29622, 29623, 29624, 29625, 29626, 29627, 29628, 29629, 29630, 29631, 29632, 29633, 29634, 29635, 29636, 29637, 29638, 29639, 29640, 29641, 29642, 29643, 29644, 29645, 29646, 29647, 29648, 29649, 29650, 29651, 29652, 29653, 29654, 29655, 29656, 29657, 29658, 29659, 29660, 29661, 29662, 29663, 29664, 29665, 29666, 29667, 29668, 29669, 29670, 29671, 29672, 29673, 29674, 29675, 29676, 29677, 29678, 29679, 29680, 29681, 29682, 29683, 29684, 29685, 29686, 29687, 29688, 29689, 29690, 29691, 29692, 29693, 29694, 29695, 29696, 29697, 29698, 29699, 29700, 29702, 29703, 29704, 29705, 29706, 29707, 29708, 29709, 29710, 29711, 29712, 29713, 29714, 29715, 29716, 29717, 29718, 29719, 29720, 29721, 29722, 29723, 29724, 29725, 29726, 29727, 29728, 29729, 29730, 29731, 29732, 29733, 29734, 29735, 29736, 29738, 29739, 29740, 29741, 29742, 29743, 29744, 29745, 29746, 29747, 29748, 29749, 29751, 29752, 29753, 29754, 29755, 29756, 29757, 29758, 29759, 29760, 29761, 29763, 29764, 29765, 29766, 29767, 29768, 29769, 29771, 29772, 29773, 29774, 29775, 29776, 29777, 29778, 29779, 29780, 29781, 29783, 29784, 29785, 29786, 29787, 29788, 29789, 29790, 29791, 29792, 29793, 29794, 29795, 29796, 29797, 29798, 29799, 29800, 29801, 29802, 29803, 29804, 29805, 29806, 29807, 29808, 29809, 29810, 29811, 29812, 29813, 29814, 29815, 29816, 29817, 29818, 29819, 29820, 29821, 29822, 29823, 29824, 29825, 29826, 29827, 29828, 29829, 29830, 29831, 29832, 29833, 29834, 29835, 29836, 29837, 29838, 29839, 29840, 29841, 29842, 29843, 29844, 29845, 29846, 29847, 29848, 29849, 29850, 29851, 29852, 29853, 29854, 29855, 29856, 29857, 29858, 29859, 29860, 29861, 29862, 29863, 29864, 29865, 29866, 29867, 29868, 29869, 29870, 29871, 29872, 29873, 29874, 29875, 29876, 29877, 29878, 29879, 29880, 29881, 29882, 29883, 29884, 29885, 29886, 29887, 29888, 29889, 29890, 29891, 29892, 29893, 29894, 29895, 29896, 29897, 29898, 29899, 29900, 29901, 29902, 29903, 29904, 29905, 29906, 29907, 29908, 29909, 29910, 29911, 29912, 29913, 29914, 29915, 29916, 29917, 29918, 29919, 29920, 29921, 29922, 29923, 29924, 29925, 29926, 29927, 29928, 29929, 29930, 29931, 29932, 29933, 29934, 29935, 29936, 29937, 29938, 29939, 29940, 29941, 29942, 29943, 29944, 29945, 29946, 29947, 29948, 29949, 29950, 29951, 29952, 29953, 29954, 29955, 29956, 29957, 29958, 29959, 29960, 29961, 29962, 29963, 29964, 29965, 29966, 29967, 29968, 29969, 29970, 29971, 29972, 29973, 29974, 29975, 29976, 29977, 29978, 29979, 29980, 29981, 29982, 29983, 29985, 29987, 29988, 29989, 29990, 29991, 29993, 29994, 29995, 29996, 29997, 29998, 29999, 30000, 30001, 30002, 30003, 30004, 30006, 30007, 30008, 30009, 30010, 30011, 30012, 30013, 30014, 30015, 30016, 30017, 30018, 30019, 30020, 30021, 30022, 30023, 30024, 30025, 30026, 30027, 30028, 30029, 30030, 30031, 30032, 30033, 30034, 30036, 30037, 30038, 30039, 30040, 30041, 30042, 30043, 30044, 30045, 30046, 30047, 30048, 30049, 30050, 30051, 30052, 30053, 30054, 30055, 30056, 30057, 30058, 30059, 30060, 30061, 30062, 30063, 30064, 30065, 30066, 30067, 30068, 30069, 30070, 30071, 30072, 30073, 30074, 30075, 30076, 30077, 30078, 30079, 30080, 30081, 30082, 30083, 30084, 30085, 30086, 30087, 30088, 30089, 30090, 30091, 30092, 30093, 30094, 30095, 30096, 30097, 30098, 30099, 30100, 30101, 30103, 30104, 30105, 30106, 30107, 30108, 30111, 30112, 30113, 30114, 30115, 30116, 30117, 30118, 30119, 30120, 30121, 30122, 30123, 30124, 30125, 30126, 30128, 30129, 30130, 30131, 30132, 30133, 30134, 30135, 30136, 30137, 30138, 30139, 30140, 30141, 30142, 30143, 30144, 30145, 30146, 30147, 30148, 30149, 30150, 30151, 30152, 30153, 30154, 30155, 30156, 30157, 30158, 30159, 30160, 30161, 30162, 30163, 30164, 30165, 30166, 30167, 30168, 30169, 30170, 30171, 30172, 30173, 30174, 30175, 30176, 30177, 30178, 30179, 30180, 30181, 30182, 30183, 30184, 30185, 30186, 30187, 30188, 30189, 30190, 30191, 30192, 30193, 30194, 30195, 30196, 30197, 30198, 30199, 30200, 30201, 30202, 30203, 30204, 30205, 30206, 30207, 30208, 30209, 30210, 30211, 30212, 30213, 30214, 30215, 30216, 30217, 30218, 30219, 30220, 30221, 30222, 30223, 30224, 30225, 30226, 30227, 30228, 30229, 30230, 30231, 30232, 30233, 30234, 30235, 30236, 30237, 30238, 30239, 30240, 30241, 30242, 30243, 30244, 30245, 30246, 30247, 30248, 30249, 30250, 30251, 30252, 30253, 30254, 30255, 30256, 30257, 30258, 30260, 30261, 30262, 30263, 30264, 30265, 30266, 30267, 30268, 30269, 30270, 30271, 30272, 30273, 30274, 30275, 30276, 30278, 30279, 30280, 30281, 30282, 30283, 30284, 30285, 30286, 30287, 30288, 30289, 30290, 30291, 30292, 30293, 30294, 30295, 30296, 30297, 30298, 30299, 30300, 30301, 30302, 30303, 30304, 30305, 30306, 30307, 30308, 30309, 30310, 30311, 30312, 30313, 30314, 30315, 30316, 30317, 30318, 30319, 30320, 30321, 30322, 30323, 30324, 30325, 30326, 30327, 30328, 30329, 30330, 30331, 30332, 30333, 30334, 30335, 30336, 30337, 30338, 30339, 30340, 30341, 30342, 30343, 30344, 30345, 30346, 30347, 30348, 30349, 30350, 30351, 30352, 30353, 30354, 30355, 30356, 30357, 30358, 30359, 30360, 30361, 30362, 30363, 30364, 30365, 30366, 30367, 30368, 30369, 30370, 30371, 30372, 30373, 30374, 30375, 30376, 30377, 30378, 30379, 30380, 30381, 30382, 30383, 30384, 30385, 30386, 30387, 30388, 30389, 30390, 30391, 30392, 30393, 30394, 30395, 30396, 30397, 30398, 30399, 30400, 30401, 30402, 30403, 30404, 30405, 30406, 30407, 30408, 30409, 30410, 30411, 30412, 30413, 30414, 30415, 30416, 30417, 30418, 30419, 30420, 30421, 30422, 30423, 30424, 30425, 30426, 30427, 30428, 30429, 30430, 30431, 30432, 30433, 30434, 30435, 30436, 30437, 30438, 30439, 30440, 30441, 30442, 30443, 30444, 30445, 30446, 30447, 30448, 30449, 30450, 30451, 30452, 30453, 30454, 30455, 30456, 30457, 30458, 30459, 30460, 30461, 30462, 30463, 30464, 30465, 30466, 30467, 30468, 30469, 30470, 30472, 30473, 30474, 30475, 30476, 30477, 30478, 30479, 30480, 30481, 30482, 30483, 30484, 30485, 30486, 30487, 30488, 30489, 30490, 30491, 30492, 30493, 30494, 30495, 30496, 30497, 30498, 30499, 30500, 30501, 30502, 30503, 30504, 30505, 30506, 30507, 30508, 30509, 30510, 30511, 30512, 30513, 30514, 30515, 30516, 30517, 30518, 30519, 30520, 30521, 30522, 30523, 30524, 30525, 30526, 30527, 30528, 30529, 30530, 30531, 30532, 30533, 30534, 30535, 30536, 30537, 30538, 30539, 30540, 30541, 30542, 30543, 30544, 30545, 30546, 30547, 30548, 30549, 30550, 30551, 30552, 30553, 30554, 30555, 30556, 30559, 30560, 30561, 30562, 30563, 30564, 30565, 30566, 30567, 30568, 30569, 30570, 30571, 30572, 30573, 30575, 30576, 30577, 30578, 30579, 30580, 30581, 30582, 30583, 30584, 30585, 30586, 30587, 30588, 30589, 30590, 30591, 30592, 30593, 30594, 30595, 30596, 30597, 30598, 30599, 30600, 30601, 30602, 30603, 30604, 30605, 30606, 30607, 30608, 30609, 30610, 30611, 30612, 30613, 30614, 30615, 30616, 30617, 30618, 30619, 30620, 30621, 30622, 30623, 30624, 30625, 30626, 30627, 30628, 30629, 30630, 30631, 30632, 30633, 30634, 30635, 30636, 30637, 30638, 30639, 30640, 30641, 30642, 30643, 30644, 30645, 30646, 30647, 30648, 30649, 30650, 30651, 30652, 30653, 30655, 30656, 30657, 30658, 30659, 30660, 30661, 30662, 30663, 30664, 30665, 30666, 30667, 30668, 30669, 30670, 30671, 30672, 30673, 30674, 30675, 30676, 30677, 30678, 30679, 30680, 30681, 30682, 30683, 30684, 30685, 30686, 30687, 30688, 30689, 30690, 30691, 30692, 30693, 30694, 30695, 30696, 30697, 30698, 30699, 30700, 30701, 30702, 30703, 30704, 30705, 30706, 30707, 30708, 30709, 30710, 30711, 30712, 30713, 30714, 30715, 30716, 30717, 30718, 30719, 30720, 30721, 30722, 30723, 30724, 30725, 30726, 30727, 30728, 30729, 30730, 30731, 30732, 30733, 30734, 30735, 30736, 30737, 30738, 30739, 30740, 30741, 30742, 30743, 30744, 30745, 30746, 30747, 30748, 30749, 30750, 30751, 30752, 30753, 30754, 30755, 30756, 30757, 30760, 30761, 30762, 30763, 30767, 30768, 30769, 30770, 30771, 30772, 30773, 30774, 30776, 30777, 30778, 30779, 30780, 30781, 30782, 30783, 30784, 30785, 30786, 30787, 30788, 30789, 30790, 30791, 30792, 30793, 30794, 30795, 30796, 30797, 30798, 30799, 30800, 30801, 30802, 30803, 30804, 30805, 30806, 30807, 30808, 30809, 30810, 30813, 30814, 30815, 30816, 30819, 30820, 30821, 30822, 30823, 30827, 30828, 30829, 30830, 30831, 30832, 30833, 30834, 30836, 30837, 30838, 30839, 30840, 30841, 30842, 30843, 30844, 30845, 30846, 30847, 30848, 30849, 30850, 30851, 30853, 30855, 30856, 30857, 30858, 30859, 30860, 30861, 30862, 30863, 30864, 30865, 30866, 30867, 30868, 30869, 30870, 30871, 30872, 30873, 30874, 30875, 30876, 30877, 30878, 30879, 30880, 30881, 30882, 30883, 30885, 30886, 30887, 30888, 30889, 30890, 30891, 30892, 30893, 30894, 30895, 30896, 30897, 30898, 30899, 30900, 30901, 30902, 30903, 30904, 30905, 30906, 30907, 30908, 30909, 30910, 30911, 30912, 30913, 30914, 30915, 30916, 30917, 30918, 30919, 30920, 30921, 30922, 30923, 30924, 30925, 30926, 30927, 30928, 30929, 30930, 30931, 30932, 30933, 30934, 30935, 30936, 30937, 30938, 30939, 30940, 30941, 30942, 30943, 30944, 30945, 30946, 30947, 30948, 30949, 30950, 30951, 30952, 30953, 30954, 30955, 30956, 30957, 30958, 30959, 30960, 30961, 30962, 30963, 30964, 30965, 30966, 30967, 30968, 30969, 30970, 30971, 30972, 30973, 30974, 30975, 30976, 30977, 30978, 30979, 30980, 30981, 30982, 30983, 30984, 30985, 30987, 30988, 30989, 30990, 30991, 30992, 30993, 30994, 30995, 30996, 30997, 30998, 30999, 31000, 31001, 31002, 31003, 31005, 31006, 31007, 31008, 31009, 31010, 31011, 31012, 31013, 31014, 31015, 31016, 31017, 31018, 31019, 31020, 31021, 31022, 31023, 31024, 31025, 31026, 31027, 31028, 31029, 31030, 31031, 31032, 31033, 31034, 31035, 31036, 31037, 31038, 31039, 31040, 31041, 31042, 31043, 31044, 31045, 31046, 31047, 31048, 31049, 31050, 31051, 31052, 31053, 31054, 31055, 31056, 31057, 31058, 31059, 31060, 31061, 31062, 31063, 31064, 31065, 31066, 31067, 31068, 31069, 31070, 31071, 31072, 31073, 31074, 31075, 31076, 31077, 31078, 31079, 31080, 31081, 31082, 31083, 31084, 31085, 31086, 31087, 31089, 31090, 31091, 31092, 31093, 31094, 31095, 31096, 31097, 31098, 31099, 31101, 31102, 31103, 31104, 31105, 31106, 31107, 31108, 31109, 31110, 31111, 31112, 31113, 31114, 31115, 31117, 31118, 31119, 31120, 31121, 31122, 31123, 31124, 31125, 31126, 31127, 31128, 31129, 31130, 31131, 31132, 31133, 31134, 31135, 31136, 31137, 31138, 31139, 31140, 31141, 31142, 31143, 31144, 31145, 31146, 31147, 31148, 31149, 31150, 31151, 31152, 31153, 31154, 31155, 31156, 31157, 31158, 31159, 31160, 31161, 31162, 31163, 31164, 31165, 31166, 31167, 31168, 31169, 31170, 31171, 31172, 31173, 31174, 31175, 31176, 31177, 31178, 31179, 31180, 31181, 31182, 31183, 31184, 31185, 31186, 31187, 31188, 31189, 31190, 31191, 31192, 31193, 31194, 31195, 31197, 31198, 31199, 31200, 31201, 31202, 31203, 31204, 31205, 31206, 31207, 31208, 31209, 31210, 31211, 31212, 31213, 31214, 31215, 31216, 31217, 31218, 31219, 31220, 31221, 31222, 31223, 31224, 31225, 31226, 31227, 31228, 31229, 31230, 31231, 31232, 31233, 31234, 31235, 31236, 31237, 31238, 31239, 31240, 31241, 31242, 31243, 31244, 31245, 31246, 31247, 31248, 31249, 31250, 31251, 31252, 31253, 31254, 31255, 31256, 31257, 31258, 31259, 31260, 31261, 31262, 31263, 31264, 31265, 31266, 31267, 31268, 31269, 31270, 31271, 31272, 31273, 31274, 31275, 31276, 31277, 31278, 31279, 31280, 31281, 31282, 31283, 31284, 31285, 31286, 31287, 31288, 31289, 31290, 31291, 31292, 31293, 31294, 31295, 31296, 31298, 31299, 31300, 31301, 31302, 31303, 31304, 31305, 31306, 31307, 31308, 31309, 31310, 31311, 31312, 31313, 31314, 31315, 31316, 31317, 31318, 31319, 31320, 31321, 31322, 31323, 31324, 31325, 31326, 31327, 31328, 31329, 31330, 31331, 31332, 31333, 31334, 31335, 31336, 31337, 31338, 31339, 31340, 31341, 31342, 31343, 31344, 31345, 31346, 31347, 31348, 31349, 31350, 31352, 31353, 31354, 31355, 31356, 31357, 31358, 31359, 31360, 31361, 31362, 31363, 31364, 31365, 31366, 31367, 31368, 31369, 31370, 31371, 31372, 31373, 31374, 31375, 31376, 31377, 31378, 31379, 31380, 31381, 31382, 31383, 31384, 31385, 31386, 31387, 31388, 31389, 31390, 31391, 31392, 31393, 31394, 31395, 31396, 31397, 31398, 31399, 31400, 31401, 31402, 31403, 31404, 31405, 31406, 31407, 31408, 31409, 31410, 31411, 31412, 31413, 31414, 31415, 31416, 31417, 31418, 31419, 31420, 31421, 31422, 31423, 31424, 31425, 31426, 31427, 31428, 31429, 31430, 31431, 31432, 31433, 31434, 31435, 31436, 31437, 31438, 31439, 31440, 31441, 31442, 31443, 31444, 31445, 31446, 31447, 31448, 31449, 31450, 31451, 31452, 31453, 31454, 31455, 31456, 31457, 31458, 31459, 31460, 31461, 31462, 31463, 31464, 31465, 31466, 31467, 31468, 31469, 31470, 31471, 31472, 31473, 31474, 31475, 31476, 31477, 31478, 31479, 31480, 31481, 31482, 31483, 31484, 31485, 31486, 31487, 31488, 31489, 31490, 31491, 31492, 31493, 31494, 31495, 31496, 31497, 31498, 31499, 31500, 31501, 31502, 31503, 31504, 31505, 31506, 31507, 31508, 31509 };

            int[] listOfItems = new[] { 1, 2, 3, 6, 8, 9, 10, 12, 14, 15, 16, 17, 19, 23, 27, 30, 31, 33, 37, 38, 40, 41, 41, 42, 42, 43, 47, 48, 49, 50, 53, 55, 57, 58, 60, 61, 63, 65, 66, 66, 68, 71, 72, 74, 75, 76, 77, 78, 80, 81, 82, 83, 84, 85, 87, 89, 90, 91, 92, 98, 99, 100, 101, 103, 104, 105, 109, 110, 112, 114, 115, 116, 118, 119, 120, 122, 124, 127, 129, 135, 136, 139, 140, 141, 145, 152, 161, 162, 165, 168, 169, 171, 175, 176, 177, 178, 179, 180, 181, 182, 184, 186, 187, 190, 193, 194, 196, 198, 201, 203, 207, 208, 212, 215, 217, 218, 225, 227, 228, 230, 232, 233, 234, 235, 236, 237, 239, 241, 243, 247, 253, 255, 258, 259, 261, 262, 263, 264, 265, 266, 267, 267, 269, 270, 272, 273, 274, 277, 284, 285, 287, 288, 291, 291, 292, 297, 298, 302, 307, 311, 312, 313, 315, 316, 317, 318, 319, 322, 323, 324, 326, 328, 329, 331, 333, 336, 338, 340, 342, 343, 344, 345, 349, 351, 353, 354, 379, 392, 395, 396, 397, 400, 404, 406, 426, 428, 429, 440, 440, 461, 462, 473, 475, 476, 491, 503, 512, 524, 526, 528, 534, 561, 573, 581, 587, 588, 590, 592, 597, 598, 600, 601, 602, 604, 607, 608, 609, 610, 612, 614, 620, 623, 625, 626, 630, 632, 634, 635, 636, 640, 641, 642, 643, 646, 647, 647, 648, 650, 653, 654, 655, 657, 657, 659, 662, 663, 665, 666, 667, 670, 671, 674, 677, 678, 679, 680, 681, 683, 688, 700, 702, 705, 706, 708, 710, 711, 713, 714, 715, 717, 718, 719, 721, 723, 725, 726, 728, 729, 730, 733, 735, 737, 739, 740, 742, 744, 746, 747, 748, 750, 751, 753, 754, 759, 760, 762, 764, 764, 765, 766, 768, 769, 771, 775, 776, 778, 781, 782, 783, 784, 786, 788, 789, 793, 796, 797, 798, 803, 804, 805, 806, 807, 808, 809, 810, 812, 816, 817, 818, 819, 820, 824, 825, 827, 831, 832, 833, 835, 836, 838, 840, 842, 845, 859, 859, 866, 867, 868, 869, 871, 872, 876, 877, 878, 879, 880, 884, 891, 892, 893, 894, 896, 898, 904, 908, 912, 915, 917, 917, 922, 923, 928, 931, 933, 935, 937, 938, 939, 944, 945, 948, 949, 950, 951, 953, 954, 955, 956, 957, 958, 959, 960, 961, 962, 968, 969, 970, 972, 973, 975, 976, 977, 978, 980, 984, 990, 992, 992, 994, 995, 999, 1000, 1001, 1006, 1007, 1009, 1011, 1013, 1015, 1016, 1017, 1018, 1019, 1023, 1025, 1026, 1027, 1032, 1033, 1034, 1035, 1036, 1041, 1042, 1046, 1047, 1055, 1057, 1060, 1061, 1066, 1067, 1071, 1073, 1075, 1076, 1078, 1079, 1081, 1083, 1084, 1085, 1086, 1088, 1089, 1090, 1093, 1096, 1097, 1098, 1098, 1100, 1101, 1102, 1103, 1104, 1105, 1108, 1111, 1113, 1113, 1116, 1119, 1120, 1120, 1122, 1126, 1127, 1128, 1137, 1138, 1139, 1141, 1142, 1143, 1146, 1147, 1148, 1149, 1150, 1152, 1152, 1157, 1161, 1166, 1174, 1174, 1175, 1178, 1181, 1185, 1190, 1192, 1193, 1194, 1194, 1195, 1196, 1197, 1198, 1199, 1202, 1204, 1210, 1211, 1213, 1215, 1217, 1220, 1223, 1225, 1225, 1231, 1234, 1235, 1238, 1239, 1240, 1242, 1243, 1244, 1245, 1249, 1252, 1253, 1255, 1256, 1257, 1258, 1260, 1264, 1265, 1268, 1269, 1272, 1273, 1276, 1277, 1278, 1279, 1281, 1282, 1282, 1283, 1284, 1285, 1286, 1287, 1288, 1291, 1298, 1301, 1302, 1304, 1305, 1306, 1310, 1311, 1313, 1317, 1320, 1322, 1325, 1328, 1329, 1331, 1334, 1335, 1336, 1339, 1341, 1342, 1343, 1344, 1345, 1347, 1349, 1350, 1353, 1355, 1356, 1356, 1358, 1361, 1362, 1364, 1366, 1367, 1368, 1369, 1370, 1371, 1373, 1374, 1375, 1377, 1380, 1381, 1382, 1383, 1384, 1386, 1387, 1389, 1393, 1394, 1397, 1398, 1399, 1401, 1403, 1405, 1406, 1409, 1410, 1412, 1413, 1414, 1416, 1417, 1417, 1419, 1420, 1421, 1423, 1427, 1430, 1431, 1432, 1432, 1433, 1437, 1438, 1441, 1445, 1446, 1447, 1449, 1452, 1454, 1455, 1458, 1462, 1468, 1469, 1472, 1474, 1475, 1479, 1483, 1484, 1486, 1487, 1489, 1495, 1497, 1498, 1498, 1499, 1501, 1504, 1506, 1509, 1510, 1510, 1512, 1517, 1518, 1519, 1520, 1522, 1523, 1525, 1526, 1528, 1529, 1532, 1533, 1535, 1536, 1543, 1544, 1547, 1548, 1549, 1552, 1553, 1554, 1559, 1560, 1562, 1565, 1567, 1569, 1570, 1571, 1572, 1573, 1574, 1579, 1581, 1583, 1584, 1585, 1586, 1587, 1590, 1591, 1593, 1595, 1597, 1598, 1600, 1606, 1607, 1609, 1910, 1911, 1912, 1922, 1924, 1927, 1928, 1932, 1934, 1937, 1938, 1939, 1940, 1943, 1945, 1946, 1947, 1948, 1952, 1957, 1959, 1963, 1970, 1972, 1973, 1974, 1975, 1977, 1978, 1987, 1988, 1989, 1991, 1996, 1997, 1998, 1999, 2001, 2003, 2005, 2006, 2007, 2008, 2009, 2012, 2012, 2013, 2014, 2016, 2019, 2020, 2022, 2023, 2024, 2028, 2031, 2038, 2039, 2040, 2041, 2042, 2043, 2045, 2046, 2047, 2049, 2051, 2052, 2053, 2054, 2055, 2056, 2057, 2059, 2060, 2061, 2065, 2069, 2070, 2071, 2075, 2076, 2077, 2082, 2085, 2087, 2088, 2089, 2091, 2092, 2094, 2096, 2098, 2102, 2103, 2103, 2104, 2107, 2109, 2111, 2113, 2114, 2115, 2117, 2117, 2118, 2119, 2120, 2121, 2122, 2123, 2124, 2126, 2127, 2127, 2128, 2130, 2132, 2133, 2134, 2135, 2136, 2137, 2139, 2142, 2143, 2144, 2146, 2150, 2154, 2155, 2156, 2156, 2158, 2159, 2161, 2164, 2166, 2167, 2173, 2176, 2176, 2177, 2179, 2180, 2183, 2187, 2190, 2192, 2195, 2196, 2197, 2200, 2202, 2205, 2208, 2215, 2225, 2238, 2271, 2277, 2280, 2292, 2293, 2300, 2306, 2317, 2329, 2336, 2340, 2344, 2345, 2346, 2348, 2353, 2354, 2360, 2368, 2394, 2398, 2406, 2411, 2413, 2414, 2417, 2423, 2428, 2430, 2432, 2442, 2453, 2470, 2485, 2496, 2503, 2514, 2523, 2530, 2532, 2534, 2535, 2537, 2542, 2543, 2544, 2547, 2550, 2552, 2553, 2553, 2555, 2555, 2556, 2557, 2558, 2563, 2564, 2565, 2568, 2569, 2570, 2572, 2574, 2576, 2581, 2586, 2587, 2590, 2591, 2592, 2593, 2594, 2596, 2607, 2623, 2626, 2627, 2628, 2630, 2633, 2638, 2641, 2642, 2648, 2648, 2654, 2656, 2659, 2665, 2670, 2672, 2673, 2675, 2682, 2683, 2684, 2685, 2686, 2687, 2688, 2693, 2696, 2697, 2698, 2699, 2700, 2701, 2703, 2704, 2704, 2707, 2708, 2710, 2712, 2714, 2715, 2717, 2719, 2726, 2731, 2736, 2738, 2740, 2746, 2751, 2754, 2755, 2756, 2757, 2758, 2759, 2762, 2764, 2767, 2771, 2772, 2775, 2779, 2780, 2781, 2782, 2785, 2786, 2787, 2788, 2789, 2790, 2792, 2796, 2800, 2801, 2802, 2804, 2806, 2812, 2814, 2817, 2820, 2821, 2821, 2823, 2824, 2826, 2827, 2828, 2829, 2832, 2834, 2838, 2839, 2840, 2843, 2847, 2850, 2850, 2852, 2853, 2855, 2856, 2859, 2862, 2863, 2866, 2867, 2870, 2871, 2873, 2877, 2879, 2880, 2886, 2887, 2888, 2889, 2890, 2891, 2892, 2896, 2897, 2901, 2903, 2904, 2906, 2911, 2912, 2913, 2935, 2941, 2946, 2949, 2952, 2954, 2955, 2958, 2968, 2969, 2975, 2980, 2982, 2986, 2987, 2988, 2990, 2991, 2992, 2993, 2994, 2996, 2998, 3001, 3002, 3005, 3006, 3011, 3012, 3013, 3014, 3020, 3023, 3024, 3027, 3029, 3032, 3035, 3037, 3038, 3039, 3042, 3044, 3045, 3045, 3047, 3048, 3056, 3058, 3059, 3060, 26701, 26705, 26710, 26711, 26713, 26715, 26716, 26721, 26722, 26723, 26724, 26725, 26730, 26731, 26732, 26734, 26735, 26781, 26782, 26791, 26791, 26792, 26792, 26795, 26798, 26800, 26809, 26811, 26812, 26817, 26818, 26825, 26826, 26827, 26828, 26829, 26832, 26834, 26837, 26839, 26840, 26842, 26844, 26847, 26848, 26849, 26851, 26852, 26854, 26857, 26862, 26863, 26866, 26867, 26869, 26874, 26875, 26878, 26879, 26880, 26885, 26888, 27002, 27003, 27005, 27006, 27007, 27008, 27009, 27010, 27011, 27012, 27013, 27016, 27019, 27020, 27024, 27034, 27037, 27040, 27042, 27044, 27046, 27047, 27048, 27048, 27049, 27049, 27050, 27050, 27052, 27053, 27053, 27054, 27055, 27057, 27058, 27059, 27061, 27062, 27064, 27065, 27066, 27070, 27072, 27073, 27074, 27075, 27079, 27080, 27081, 27086, 27087, 27088, 27089, 27090, 27092, 27092, 27093, 27094, 27095, 27096, 27099, 27100, 27102, 27104, 27107, 27108, 27109, 27110, 27111, 27112, 27113, 27114, 27115, 27115, 27116, 27118, 27119, 27120, 27122, 27125, 27126, 27127, 27128, 27129, 27130, 27131, 27132, 27133, 27139, 27140, 27141, 27142, 27143, 27144, 27145, 27146, 27147, 27150, 27152, 27153, 27159, 27161, 27162, 27165, 27169, 27172, 27174, 27175, 27176, 27177, 27179, 27182, 27184, 27185, 27186, 27188, 27190, 27194, 27195, 27196, 27199, 27200, 27201, 27204, 27207, 27209, 27210, 27213, 27214, 27216, 27217, 27219, 27220, 27221, 27225, 27227, 27228, 27231, 27232, 27233, 27234, 27235, 27236, 27242, 27243, 27244, 27245, 27248, 27250, 27253, 27257, 27258, 27259, 27263, 27265, 27266, 27267, 27271, 27274, 27288, 27293, 27298, 27299, 27319, 27320, 27325, 27326, 27330, 27333, 27335, 27337, 27338, 27339, 27342, 27343, 27345, 27352, 27354, 27355, 27356, 27357, 27367, 27370, 27377, 27380, 27381, 27383, 27384, 27385, 27386, 27387, 27388, 27389, 27391, 27392, 27395, 27396, 27399, 27401, 27402, 27405, 27411, 27414, 27415, 27416, 27417, 27421, 27422, 27430, 27435, 27442, 27443, 27444, 27446, 27447, 27448, 27455, 27463, 27473, 27476, 27483, 27484, 27494, 27495, 27497, 27505, 27509, 27510, 27530, 27532, 27533, 27534, 27535, 27536, 27537, 27546, 27547, 27548, 27549, 27550, 27551, 27552, 27556, 27556, 27557, 27567, 27569, 27571, 27572, 27573, 27575, 27575, 27576, 27580, 27586, 27587, 27591, 27596, 27598, 27599, 27603, 27604, 27613, 27614, 27615, 27618, 27619, 27620, 27623, 27630, 27632, 27635, 27637, 27639, 27642, 27643, 27644, 27644, 27645, 27646, 27647, 27651, 27652, 27653, 27654, 27656, 27665, 27669, 27670, 27671, 27672, 27672, 27673, 27677, 27687, 27689, 27701, 27705, 27706, 27707, 27708, 27709, 27710, 27711, 27712, 27713, 27719, 27722, 27726, 27727, 27728, 27729, 27730, 27731, 27732, 27733, 27734, 27735, 27739, 27750, 27751, 27755, 27757, 27758, 27759, 27761, 27765, 27766, 27767, 27768, 27769, 27771, 27772, 27787, 27788, 27789, 27791, 27793, 27794, 27795, 27796, 27800, 27804, 27814, 27820, 27821, 27822, 27828, 27829, 27835, 27842, 27843, 27846, 27852, 27853, 27854, 27855, 27857, 27858, 27860, 27862, 27863, 27864, 27867, 27870, 27871, 27872, 27872, 27874, 27875, 27876, 27877, 27878, 27879, 27880, 27881, 27883, 27884, 27885, 27886, 27887, 27888, 27889, 27890, 27891, 27892, 27893, 27894, 27895, 27896, 27898, 27899, 27900, 27901, 27902, 27903, 27905, 27906, 27908, 27910, 27911, 27912, 27913, 27914, 27915, 27917, 27918, 27919, 27920, 27922, 27923, 27924, 27925, 27926, 27927, 27928, 27929, 27932, 27933, 27934, 27935, 27936, 27937, 27938, 27939, 27940, 27941, 27943, 27944, 27945, 27946, 27947, 27948, 27949, 27950, 27951, 27952, 27956, 27957, 27958, 27960, 27963, 27964, 27965, 27966, 27968, 27969, 27970, 27971, 27972, 27974, 27975, 27977, 27978, 27982, 27983, 27984, 27985, 27986, 27987, 27992, 27993, 27995, 27996, 27997, 27998, 27999, 28000, 28001, 28002, 28003, 28004, 28005, 28007, 28008, 28009, 28010, 28011, 28012, 28013, 28014, 28016, 28017, 28018, 28019, 28020, 28021, 28022, 28023, 28023, 28024, 28026, 28027, 28028, 28029, 28030, 28031, 28033, 28034, 28036, 28037, 28038, 28039, 28041, 28042, 28043, 28044, 28045, 28046, 28047, 28048, 28049, 28050, 28051, 28052, 28053, 28055, 28056, 28057, 28058, 28059, 28060, 28061, 28062, 28063, 28064, 28065, 28066, 28067, 28071, 28072, 28073, 28074, 28076, 28077, 28078, 28079, 28081, 28082, 28083, 28084, 28085, 28086, 28089, 28090, 28091, 28093, 28094, 28096, 28097, 28098, 28099, 28101, 28102, 28104, 28105, 28106, 28107, 28109, 28110, 28111, 28112, 28113, 28114, 28115, 28116, 28117, 28118, 28119, 28120, 28121, 28122, 28123, 28124, 28125, 28126, 28127, 28128, 28129, 28130, 28131, 28132, 28133, 28134, 28135, 28136, 28137, 28139, 28140, 28141, 28143, 28144, 28145, 28146, 28147, 28148, 28149, 28150, 28151, 28152, 28153, 28154, 28155, 28156, 28157, 28158, 28159, 28160, 28161, 28162, 28163, 28164, 28167, 28168, 28169, 28170, 28171, 28172, 28173, 28174, 28175, 28177, 28178, 28179, 28180, 28181, 28182, 28183, 28184, 28185, 28186, 28187, 28188, 28189, 28190, 28191, 28192, 28193, 28194, 28195, 28197, 28199, 28200, 28201, 28202, 28203, 28204, 28205, 28206, 28207, 28208, 28209, 28210, 28211, 28213, 28214, 28215, 28216, 28217, 28218, 28219, 28220, 28221, 28222, 28223, 28224, 28225, 28226, 28227, 28228, 28229, 28230, 28231, 28232, 28233, 28234, 28235, 28236, 28237, 28238, 28239, 28240, 28241, 28242, 28245, 28246, 28247, 28248, 28249, 28250, 28251, 28253, 28254, 28255, 28257, 28259, 28275, 28276, 28277, 28278, 28285, 28286, 28287, 28288, 28305, 28308, 28313, 28323, 28324, 28325, 28326, 28327, 28328, 28329, 28330, 28331, 28331, 28332, 28333, 28334, 28335, 28336, 28337, 28338, 28339, 28340, 28341, 28342, 28343, 28344, 28345, 28346, 28347, 28348, 28349, 28350, 28351, 28352, 28353, 28354, 28354, 28355, 28356, 28357, 28358, 28359, 28360, 28361, 28362, 28363, 28364, 28365, 28366, 28367, 28368, 28369, 28370, 28371, 28372, 28373, 28374, 28375, 28376, 28377, 28378, 28380, 28381, 28382, 28383, 28384, 28385, 28386, 28387, 28388, 28389, 28390, 28391, 28392, 28393, 28394, 28394, 28395, 28396, 28397, 28398, 28399, 28400, 28401, 28402, 28403, 28404, 28405, 28406, 28407, 28408, 28409, 28410, 28411, 28412, 28413, 28414, 28415, 28416, 28417, 28418, 28419, 28420, 28421, 28422, 28423, 28424, 28425, 28426, 28427, 28428, 28429, 28430, 28431, 28432, 28433, 28434, 28436, 28437, 28438, 28439, 28440, 28441, 28442, 28443, 28444, 28445, 28446, 28447, 28448, 28449, 28450, 28451, 28452, 28453, 28454, 28455, 28456, 28457, 28458, 28459, 28460, 28461, 28462, 28463, 28464, 28465, 28466, 28467, 28468, 28476, 28477, 28478, 28479, 28480, 28481, 28482, 28483, 28484, 28485, 28486, 28487, 28488, 28489, 28490, 28491, 28492, 28493, 28494, 28495, 28496, 28497, 28498, 28499, 28500, 28501, 28502, 28503, 28504, 28505, 28506, 28507, 28508, 28509, 28510, 28514, 28515, 28516, 28527, 28528, 28529, 28530, 28531, 28532, 28533, 28534, 28535, 28536, 28537, 28538, 28539, 28540, 28541, 28542, 28543, 28544, 28545, 28546, 28548, 28549, 28550, 28551, 28552, 28553, 28554, 28555, 28556, 28557, 28558, 28559, 28560, 28562, 28563, 28563, 28564, 28565, 28566, 28566, 28567, 28568, 28568, 28569, 28570, 28572, 28573, 28574, 28575, 28576, 28577, 28578, 28579, 28580, 28581, 28582, 28584, 28585, 28586, 28587, 28588, 28589, 28590, 28592, 28593, 28594, 28595, 28596, 28597, 28598, 28599, 28600, 28601, 28602, 28604, 28605, 28606, 28607, 28608, 28609, 28610, 28611, 28612, 28613, 28614, 28615, 28616, 28617, 28618, 28619, 28620, 28621, 28622, 28623, 28624, 28625, 28626, 28627, 28628, 28629, 28630, 28631, 28632, 28634, 28635, 28636, 28637, 28639, 28639, 28640, 28642, 28643, 28644, 28645, 28646, 28647, 28648, 28649, 28650, 28651, 28652, 28653, 28654, 28655, 28656, 28657, 28658, 28659, 28660, 28661, 28661, 28662, 28663, 28664, 28665, 28666, 28667, 28668, 28669, 28670, 28671, 28672, 28673, 28674, 28680, 28681, 28682, 28683, 28684, 28685, 28686, 28687, 28688, 28689, 28690, 28691, 28692, 28693, 28694, 28695, 28696, 28697, 28698, 28699, 28700, 28701, 28702, 28703, 28704, 28705, 28706, 28707, 28708, 28710, 28711, 28712, 28713, 28714, 28715, 28716, 28717, 28718, 28719, 28720, 28721, 28722, 28723, 28724, 28725, 28726, 28727, 28728, 28729, 28730, 28731, 28732, 28733, 28734, 28735, 28736, 28737, 28737, 28738, 28739, 28740, 28741, 28742, 28743, 28744, 28745, 28746, 28747, 28748, 28749, 28750, 28752, 28752, 28753, 28754, 28755, 28756, 28757, 28758, 28759, 28760, 28761, 28762, 28763, 28764, 28765, 28766, 28767, 28768, 28769, 28770, 28771, 28772, 28773, 28774, 28775, 28776, 28777, 28778, 28779, 28780, 28781, 28782, 28783, 28784, 28785, 28786, 28787, 28788, 28789, 28790, 28791, 28792, 28793, 28794, 28795, 28796, 28797, 28798, 28799, 28800, 28801, 28802, 28803, 28804, 28804, 28805, 28805, 28806, 28807, 28809, 28810, 28811, 28812, 28813, 28814, 28815, 28816, 28817, 28818, 28819, 28820, 28821, 28822, 28823, 28824, 28825, 28826, 28827, 28828, 28829, 28830, 28831, 28832, 28833, 28834, 28835, 28836, 28837, 28838, 28839, 28840, 28841, 28842, 28843, 28844, 28845, 28846, 28847, 28848, 28849, 28850, 28851, 28852, 28853, 28854, 28855, 28856, 28857, 28858, 28859, 28860, 28861, 28862, 28863, 28863, 28864, 28865, 28866, 28867, 28868, 28870, 28871, 28872, 28874, 28875, 28876, 28877, 28878, 28879, 28880, 28881, 28882, 28883, 28884, 28885, 28886, 28887, 28888, 28889, 28890, 28891, 28893, 28894, 28895, 28896, 28897, 28898, 28899, 28900, 28901, 28902, 28903, 28904, 28905, 28907, 28908, 28909, 28910, 28911, 28912, 28914, 28915, 28916, 28917, 28918, 28919, 28920, 28921, 28922, 28923, 28924, 28925, 28926, 28927, 28928, 28929, 28930, 28931, 28932, 28933, 28934, 28935, 28936, 28937, 28938, 28939, 28940, 28941, 28942, 28943, 28944, 28945, 28946, 28947, 28948, 28949, 28950, 28951, 28952, 28953, 28954, 28955, 28957, 28958, 28959, 28960, 28961, 28962, 28963, 28964, 28965, 28966, 28967, 28968, 28969, 28970, 28971, 28972, 28973, 28974, 28975, 28976, 28977, 28978, 28979, 28980, 28981, 28982, 28983, 28984, 28985, 28986, 28987, 28988, 28989, 28990, 28991, 28992, 28993, 28994, 28995, 28996, 28996, 28997, 28998, 28999, 29000, 29001, 29002, 29003, 29004, 29005, 29006, 29007, 29008, 29009, 29010, 29011, 29012, 29013, 29014, 29015, 29016, 29017, 29018, 29019, 29020, 29021, 29022, 29023, 29024, 29025, 29026, 29027, 29028, 29029, 29030, 29031, 29032, 29033, 29034, 29035, 29036, 29037, 29038, 29039, 29040, 29041, 29042, 29043, 29044, 29045, 29046, 29047, 29048, 29049, 29050, 29051, 29052, 29053, 29054, 29055, 29056, 29057, 29058, 29059, 29060, 29061, 29062, 29063, 29064, 29065, 29066, 29067, 29068, 29069, 29070, 29071, 29072, 29073, 29074, 29075, 29076, 29077, 29078, 29079, 29080, 29081, 29082, 29083, 29084, 29085, 29086, 29087, 29088, 29089, 29090, 29091, 29092, 29093, 29094, 29095, 29096, 29097, 29098, 29099, 29100, 29101, 29102, 29103, 29104, 29105, 29106, 29107, 29108, 29109, 29111, 29112, 29113, 29114, 29115, 29116, 29117, 29118, 29119, 29120, 29121, 29122, 29123, 29124, 29125, 29126, 29127, 29128, 29129, 29130, 29131, 29132, 29133, 29134, 29135, 29136, 29137, 29138, 29139, 29140, 29141, 29142, 29143, 29144, 29145, 29146, 29147, 29148, 29150, 29151, 29152, 29153, 29154, 29155, 29156, 29157, 29158, 29159, 29160, 29161, 29162, 29163, 29164, 29165, 29166, 29168, 29169, 29170, 29171, 29172, 29173, 29175, 29176, 29177, 29178, 29179, 29180, 29181, 29182, 29183, 29184, 29185, 29186, 29187, 29188, 29189, 29190, 29191, 29192, 29193, 29194, 29195, 29196, 29197, 29198, 29199, 29200, 29201, 29202, 29203, 29204, 29205, 29206, 29207, 29208, 29209, 29210, 29211, 29212, 29213, 29214, 29215, 29216, 29217, 29218, 29219, 29220, 29221, 29222, 29223, 29224, 29225, 29226, 29227, 29228, 29229, 29230, 29231, 29232, 29233, 29234, 29235, 29236, 29237, 29238, 29239, 29240, 29241, 29242, 29243, 29244, 29245, 29246, 29247, 29248, 29249, 29250, 29251, 29252, 29253, 29254, 29255, 29256, 29257, 29258, 29259, 29260, 29261, 29262, 29263, 29264, 29265, 29266, 29267, 29268, 29269, 29270, 29271, 29272, 29273, 29274, 29275, 29276, 29277, 29278, 29279, 29280, 29281, 29282, 29283, 29285, 29286, 29287, 29289, 29290, 29291, 29292, 29293, 29294, 29295, 29296, 29297, 29298, 29299, 29300, 29301, 29302, 29303, 29304, 29305, 29306, 29307, 29308, 29309, 29310, 29311, 29312, 29313, 29314, 29315, 29316, 29317, 29318, 29319, 29320, 29321, 29322, 29323, 29324, 29325, 29326, 29327, 29328, 29329, 29330, 29331, 29332, 29333, 29334, 29335, 29336, 29337, 29338, 29339, 29340, 29341, 29343, 29344, 29345, 29346, 29347, 29348, 29349, 29350, 29351, 29352, 29353, 29354, 29355, 29356, 29357, 29358, 29359, 29360, 29361, 29362, 29363, 29364, 29365, 29366, 29367, 29368, 29369, 29370, 29371, 29372, 29373, 29374, 29375, 29376, 29377, 29378, 29379, 29380, 29381, 29382, 29383, 29384, 29385, 29386, 29387, 29388, 29389, 29390, 29391, 29392, 29393, 29394, 29395, 29396, 29397, 29398, 29399, 29400, 29401, 29402, 29403, 29404, 29405, 29406, 29407, 29408, 29409, 29410, 29411, 29412, 29413, 29414, 29415, 29416, 29417, 29418, 29419, 29420, 29421, 29422, 29423, 29424, 29425, 29426, 29427, 29428, 29429, 29430, 29431, 29432, 29433, 29434, 29435, 29436, 29437, 29438, 29439, 29440, 29441, 29442, 29443, 29444, 29445, 29446, 29447, 29448, 29449, 29450, 29451, 29452, 29453, 29454, 29454, 29455, 29456, 29457, 29458, 29458, 29459, 29460, 29461, 29462, 29469, 29470, 29471, 29472, 29473, 29474, 29475, 29476, 29477, 29478, 29479, 29480, 29481, 29482, 29484, 29485, 29486, 29487, 29488, 29489, 29490, 29491, 29492, 29493, 29494, 29495, 29496, 29497, 29498, 29499, 29500, 29501, 29502, 29503, 29504, 29505, 29506, 29507, 29508, 29509, 29510, 29511, 29512, 29513, 29514, 29515, 29516, 29517, 29518, 29519, 29520, 29521, 29522, 29523, 29524, 29525, 29526, 29527, 29528, 29529, 29530, 29531, 29532, 29533, 29534, 29535, 29536, 29537, 29538, 29539, 29540, 29541, 29542, 29543, 29544, 29545, 29546, 29548, 29549, 29550, 29551, 29552, 29553, 29554, 29555, 29556, 29557, 29558, 29559, 29560, 29561, 29562, 29563, 29564, 29565, 29566, 29567, 29568, 29569, 29570, 29571, 29572, 29573, 29574, 29575, 29576, 29577, 29578, 29579, 29580, 29581, 29582, 29583, 29584, 29585, 29586, 29587, 29588, 29589, 29590, 29591, 29592, 29594, 29595, 29596, 29597, 29598, 29599, 29600, 29601, 29602, 29603, 29604, 29605, 29607, 29608, 29609, 29610, 29611, 29612, 29613, 29614, 29615, 29616, 29617, 29618, 29619, 29620, 29621, 29622, 29623, 29624, 29625, 29626, 29627, 29628, 29629, 29630, 29631, 29632, 29633, 29634, 29635, 29636, 29637, 29638, 29639, 29640, 29641, 29642, 29643, 29644, 29645, 29646, 29647, 29648, 29649, 29650, 29651, 29652, 29653, 29654, 29655, 29656, 29657, 29658, 29659, 29660, 29661, 29662, 29663, 29664, 29665, 29666, 29667, 29668, 29669, 29670, 29671, 29672, 29673, 29674, 29675, 29676, 29677, 29678, 29679, 29680, 29681, 29682, 29683, 29684, 29685, 29686, 29687, 29688, 29689, 29690, 29691, 29692, 29693, 29694, 29695, 29696, 29697, 29698, 29699, 29700, 29702, 29703, 29704, 29705, 29706, 29707, 29708, 29709, 29710, 29711, 29712, 29713, 29714, 29715, 29716, 29717, 29718, 29719, 29720, 29721, 29722, 29723, 29724, 29725, 29726, 29727, 29728, 29729, 29730, 29731, 29732, 29733, 29734, 29735, 29736, 29738, 29739, 29740, 29741, 29742, 29743, 29744, 29745, 29746, 29747, 29748, 29749, 29751, 29752, 29753, 29754, 29755, 29756, 29757, 29758, 29759, 29760, 29761, 29763, 29764, 29765, 29766, 29767, 29768, 29769, 29771, 29772, 29773, 29774, 29775, 29776, 29777, 29778, 29779, 29780, 29781, 29783, 29784, 29785, 29786, 29787, 29788, 29789, 29790, 29791, 29792, 29793, 29794, 29795, 29796, 29797, 29798, 29799, 29800, 29801, 29802, 29803, 29804, 29805, 29806, 29807, 29808, 29809, 29810, 29811, 29812, 29813, 29814, 29815, 29816, 29817, 29818, 29819, 29820, 29821, 29822, 29823, 29824, 29825, 29826, 29827, 29828, 29829, 29830, 29831, 29832, 29833, 29834, 29835, 29836, 29837, 29838, 29839, 29840, 29841, 29842, 29843, 29844, 29845, 29846, 29847, 29848, 29849, 29850, 29851, 29852, 29853, 29854, 29855, 29856, 29857, 29858, 29859, 29860, 29861, 29862, 29863, 29864, 29865, 29866, 29867, 29868, 29869, 29870, 29871, 29872, 29873, 29874, 29875, 29876, 29877, 29878, 29879, 29880, 29881, 29882, 29883, 29884, 29885, 29886, 29887, 29888, 29889, 29890, 29891, 29892, 29893, 29894, 29895, 29896, 29897, 29898, 29899, 29900, 29901, 29902, 29903, 29904, 29905, 29906, 29907, 29908, 29909, 29910, 29911, 29912, 29913, 29914, 29915, 29916, 29917, 29918, 29919, 29920, 29921, 29922, 29923, 29924, 29925, 29926, 29927, 29928, 29929, 29930, 29931, 29932, 29933, 29934, 29935, 29936, 29937, 29938, 29939, 29940, 29941, 29942, 29943, 29944, 29945, 29946, 29947, 29948, 29949, 29950, 29951, 29952, 29953, 29954, 29955, 29956, 29957, 29958, 29959, 29960, 29961, 29962, 29963, 29964, 29965, 29966, 29967, 29968, 29969, 29970, 29971, 29972, 29973, 29974, 29975, 29976, 29977, 29978, 29979, 29980, 29981, 29982, 29983, 29985, 29987, 29988, 29989, 29990, 29991, 29993, 29994, 29995, 29996, 29997, 29998, 29999, 30000, 30001, 30002, 30003, 30004, 30006, 30007, 30008, 30009, 30010, 30011, 30012, 30013, 30014, 30015, 30016, 30017, 30018, 30019, 30020, 30021, 30022, 30023, 30024, 30025, 30026, 30027, 30028, 30029, 30030, 30031, 30032, 30033, 30034, 30036, 30037, 30038, 30039, 30040, 30041, 30042, 30043, 30044, 30045, 30046, 30047, 30048, 30049, 30050, 30051, 30052, 30053, 30054, 30055, 30056, 30057, 30058, 30059, 30060, 30061, 30062, 30063, 30064, 30065, 30066, 30067, 30068, 30069, 30070, 30071, 30072, 30073, 30074, 30075, 30076, 30077, 30078, 30079, 30080, 30081, 30082, 30083, 30084, 30085, 30086, 30087, 30088, 30089, 30090, 30091, 30092, 30093, 30094, 30095, 30096, 30097, 30098, 30099, 30100, 30101, 30103, 30104, 30105, 30106, 30107, 30108, 30111, 30112, 30113, 30114, 30115, 30116, 30117, 30118, 30119, 30120, 30121, 30122, 30123, 30124, 30125, 30126, 30127, 30127, 30128, 30129, 30130, 30131, 30132, 30133, 30134, 30135, 30136, 30137, 30138, 30139, 30140, 30141, 30142, 30143, 30144, 30145, 30146, 30147, 30148, 30149, 30150, 30151, 30152, 30153, 30154, 30155, 30156, 30157, 30158, 30159, 30160, 30161, 30162, 30163, 30164, 30165, 30166, 30167, 30168, 30169, 30170, 30171, 30172, 30173, 30174, 30175, 30176, 30177, 30178, 30179, 30180, 30181, 30182, 30183, 30184, 30185, 30186, 30187, 30188, 30189, 30190, 30191, 30192, 30193, 30194, 30195, 30196, 30197, 30198, 30199, 30200, 30201, 30202, 30203, 30204, 30205, 30206, 30207, 30208, 30209, 30210, 30211, 30212, 30213, 30214, 30215, 30216, 30217, 30218, 30219, 30220, 30221, 30222, 30223, 30224, 30225, 30226, 30227, 30228, 30229, 30230, 30231, 30232, 30233, 30234, 30235, 30236, 30237, 30238, 30239, 30240, 30241, 30242, 30243, 30244, 30245, 30246, 30247, 30248, 30249, 30250, 30251, 30252, 30253, 30254, 30255, 30256, 30257, 30258, 30260, 30261, 30262, 30263, 30264, 30265, 30266, 30267, 30268, 30269, 30270, 30271, 30272, 30273, 30274, 30275, 30276, 30278, 30279, 30280, 30281, 30282, 30283, 30284, 30285, 30286, 30287, 30288, 30289, 30290, 30291, 30292, 30293, 30294, 30295, 30296, 30297, 30298, 30299, 30300, 30301, 30302, 30303, 30304, 30305, 30306, 30307, 30308, 30309, 30310, 30311, 30312, 30313, 30314, 30315, 30316, 30317, 30318, 30319, 30320, 30321, 30322, 30323, 30324, 30325, 30326, 30327, 30328, 30329, 30330, 30331, 30332, 30333, 30334, 30335, 30336, 30337, 30338, 30339, 30340, 30341, 30342, 30343, 30344, 30345, 30346, 30347, 30348, 30349, 30350, 30351, 30352, 30353, 30354, 30355, 30356, 30357, 30358, 30359, 30360, 30361, 30362, 30363, 30364, 30365, 30366, 30367, 30368, 30369, 30370, 30371, 30372, 30373, 30374, 30375, 30376, 30377, 30378, 30379, 30380, 30381, 30382, 30383, 30384, 30385, 30386, 30387, 30388, 30389, 30390, 30391, 30392, 30393, 30394, 30395, 30396, 30397, 30398, 30399, 30400, 30401, 30402, 30403, 30404, 30405, 30406, 30407, 30408, 30409, 30410, 30411, 30412, 30413, 30414, 30415, 30416, 30417, 30418, 30419, 30420, 30421, 30422, 30423, 30424, 30425, 30426, 30427, 30428, 30429, 30430, 30431, 30432, 30433, 30434, 30435, 30436, 30437, 30438, 30439, 30440, 30441, 30442, 30443, 30444, 30445, 30446, 30447, 30448, 30449, 30450, 30451, 30452, 30453, 30454, 30455, 30456, 30457, 30458, 30459, 30460, 30461, 30462, 30463, 30464, 30465, 30466, 30467, 30468, 30469, 30470, 30472, 30473, 30474, 30475, 30476, 30477, 30478, 30479, 30480, 30481, 30482, 30483, 30484, 30485, 30486, 30487, 30488, 30489, 30490, 30491, 30492, 30493, 30494, 30495, 30496, 30497, 30498, 30499, 30500, 30501, 30502, 30503, 30504, 30505, 30506, 30507, 30508, 30509, 30510, 30511, 30512, 30513, 30514, 30515, 30516, 30517, 30518, 30519, 30520, 30521, 30522, 30523, 30524, 30525, 30526, 30527, 30528, 30529, 30530, 30531, 30532, 30533, 30534, 30535, 30536, 30537, 30538, 30539, 30540, 30541, 30542, 30543, 30544, 30545, 30546, 30547, 30548, 30549, 30550, 30551, 30552, 30553, 30554, 30555, 30556, 30557, 30557, 30559, 30560, 30561, 30562, 30563, 30564, 30565, 30566, 30567, 30568, 30569, 30570, 30571, 30572, 30573, 30575, 30576, 30577, 30578, 30579, 30580, 30581, 30582, 30583, 30584, 30585, 30586, 30587, 30588, 30589, 30590, 30591, 30592, 30593, 30594, 30595, 30596, 30597, 30598, 30599, 30600, 30601, 30602, 30603, 30604, 30605, 30606, 30607, 30608, 30609, 30610, 30611, 30612, 30613, 30614, 30615, 30616, 30617, 30618, 30619, 30620, 30621, 30622, 30623, 30624, 30625, 30626, 30627, 30628, 30629, 30630, 30631, 30632, 30633, 30634, 30635, 30636, 30637, 30638, 30639, 30640, 30641, 30642, 30643, 30644, 30645, 30646, 30647, 30648, 30649, 30650, 30651, 30652, 30653, 30655, 30656, 30657, 30658, 30659, 30660, 30661, 30662, 30663, 30664, 30665, 30666, 30667, 30668, 30669, 30670, 30671, 30672, 30673, 30674, 30675, 30676, 30677, 30678, 30679, 30680, 30681, 30682, 30683, 30684, 30685, 30686, 30687, 30688, 30689, 30690, 30691, 30692, 30693, 30694, 30695, 30696, 30697, 30698, 30699, 30700, 30701, 30702, 30703, 30704, 30705, 30706, 30707, 30708, 30709, 30710, 30711, 30712, 30713, 30714, 30715, 30716, 30717, 30718, 30719, 30720, 30721, 30722, 30723, 30724, 30725, 30726, 30727, 30728, 30729, 30730, 30731, 30732, 30733, 30734, 30735, 30736, 30737, 30738, 30739, 30740, 30741, 30742, 30743, 30744, 30745, 30746, 30747, 30748, 30749, 30750, 30751, 30752, 30753, 30754, 30755, 30756, 30757, 30760, 30761, 30762, 30763, 30767, 30768, 30769, 30770, 30771, 30772, 30773, 30774, 30776, 30777, 30778, 30779, 30780, 30781, 30782, 30783, 30784, 30785, 30786, 30787, 30788, 30789, 30790, 30791, 30792, 30793, 30794, 30795, 30796, 30797, 30798, 30799, 30800, 30801, 30802, 30803, 30804, 30805, 30806, 30807, 30808, 30809, 30810, 30813, 30814, 30815, 30816, 30819, 30820, 30821, 30822, 30823, 30827, 30828, 30829, 30830, 30831, 30832, 30833, 30834, 30836, 30837, 30838, 30839, 30840, 30841, 30842, 30843, 30844, 30845, 30846, 30847, 30848, 30849, 30850, 30851, 30853, 30855, 30856, 30857, 30858, 30859, 30860, 30861, 30862, 30863, 30864, 30865, 30866, 30867, 30868, 30869, 30870, 30871, 30872, 30873, 30874, 30875, 30876, 30877, 30878, 30879, 30880, 30881, 30882, 30883, 30885, 30886, 30887, 30888, 30889, 30890, 30891, 30892, 30893, 30894, 30895, 30896, 30897, 30898, 30899, 30900, 30901, 30902, 30903, 30904, 30905, 30906, 30907, 30908, 30909, 30910, 30911, 30912, 30913, 30914, 30915, 30916, 30917, 30918, 30919, 30920, 30921, 30922, 30923, 30924, 30925, 30926, 30927, 30928, 30929, 30930, 30931, 30932, 30933, 30934, 30935, 30936, 30937, 30938, 30939, 30940, 30941, 30942, 30943, 30944, 30945, 30946, 30947, 30948, 30949, 30950, 30951, 30952, 30953, 30954, 30955, 30956, 30957, 30958, 30959, 30960, 30961, 30962, 30963, 30964, 30965, 30966, 30967, 30968, 30969, 30970, 30971, 30972, 30973, 30974, 30975, 30976, 30977, 30978, 30979, 30980, 30981, 30982, 30983, 30984, 30985, 30987, 30988, 30989, 30990, 30991, 30992, 30993, 30994, 30995, 30996, 30997, 30998, 30999, 31000, 31001, 31002, 31003, 31005, 31006, 31007, 31008, 31009, 31010, 31011, 31012, 31013, 31014, 31015, 31016, 31017, 31018, 31019, 31020, 31021, 31022, 31023, 31024, 31025, 31026, 31027, 31028, 31029, 31030, 31031, 31032, 31033, 31034, 31035, 31036, 31037, 31038, 31039, 31040, 31041, 31042, 31043, 31044, 31045, 31046, 31047, 31048, 31049, 31050, 31051, 31052, 31053, 31054, 31055, 31056, 31057, 31058, 31059, 31060, 31061, 31062, 31063, 31064, 31065, 31066, 31067, 31068, 31069, 31070, 31071, 31072, 31073, 31074, 31075, 31076, 31077, 31078, 31079, 31080, 31081, 31082, 31083, 31084, 31085, 31086, 31087, 31089, 31090, 31091, 31092, 31093, 31094, 31095, 31096, 31097, 31098, 31099, 31101, 31102, 31103, 31104, 31105, 31106, 31107, 31108, 31109, 31110, 31111, 31112, 31113, 31114, 31115, 31117, 31118, 31119, 31120, 31121, 31122, 31123, 31124, 31125, 31126, 31127, 31128, 31129, 31130, 31131, 31132, 31133, 31134, 31135, 31136, 31137, 31138, 31139, 31140, 31141, 31142, 31143, 31144, 31145, 31146, 31147, 31148, 31149, 31150, 31151, 31152, 31153, 31154, 31155, 31156, 31157, 31158, 31159, 31160, 31161, 31162, 31163, 31164, 31165, 31166, 31167, 31168, 31169, 31170, 31171, 31172, 31173, 31174, 31175, 31176, 31177, 31178, 31179, 31180, 31181, 31182, 31183, 31184, 31185, 31186, 31187, 31188, 31189, 31190, 31191, 31192, 31193, 31194, 31195, 31197, 31198, 31199, 31200, 31201, 31202, 31203, 31204, 31205, 31206, 31207, 31208, 31209, 31210, 31211, 31212, 31213, 31214, 31215, 31216, 31217, 31218, 31219, 31220, 31221, 31222, 31223, 31224, 31225, 31226, 31227, 31228, 31229, 31230, 31231, 31232, 31233, 31234, 31235, 31236, 31237, 31238, 31239, 31240, 31241, 31242, 31243, 31244, 31245, 31246, 31247, 31248, 31249, 31250, 31251, 31252, 31253, 31254, 31255, 31256, 31257, 31258, 31259, 31260, 31261, 31262, 31263, 31264, 31265, 31266, 31267, 31268, 31269, 31270, 31271, 31272, 31273, 31274, 31275, 31276, 31277, 31278, 31279, 31280, 31281, 31282, 31283, 31284, 31285, 31286, 31287, 31288, 31289, 31290, 31291, 31292, 31293, 31294, 31295, 31296, 31298, 31299, 31300, 31301, 31302, 31303, 31304, 31305, 31306, 31307, 31308, 31309, 31310, 31311, 31312, 31313, 31314, 31315, 31316, 31317, 31318, 31319, 31320, 31321, 31322, 31323, 31324, 31325, 31326, 31327, 31328, 31329, 31330, 31331, 31332, 31333, 31334, 31335, 31336, 31337, 31338, 31339, 31340, 31341, 31342, 31343, 31344, 31345, 31346, 31347, 31348, 31349, 31350, 31352, 31353, 31354, 31355, 31356, 31357, 31358, 31359, 31360, 31361, 31362, 31363, 31364, 31365, 31366, 31367, 31368, 31369, 31370, 31371, 31372, 31373, 31374, 31375, 31376, 31377, 31378, 31379, 31380, 31381, 31382, 31383, 31384, 31385, 31386, 31387, 31388, 31389, 31390, 31391, 31392, 31393, 31394, 31395, 31396, 31397, 31398, 31399, 31400, 31401, 31402, 31403, 31404, 31405, 31406, 31407, 31408, 31409, 31410, 31411, 31412, 31413, 31414, 31415, 31416, 31417, 31418, 31419, 31420, 31421, 31422, 31423, 31424, 31425, 31426, 31427, 31428, 31429, 31430, 31431, 31432, 31433, 31434, 31435, 31436, 31437, 31438, 31439, 31440, 31441, 31442, 31443, 31444, 31445, 31446, 31447, 31448, 31449, 31450, 31451, 31452, 31453, 31454, 31455, 31456, 31457, 31458, 31459, 31460, 31461, 31462, 31463, 31464, 31465, 31466, 31467, 31468, 31469, 31470, 31471, 31472, 31473, 31474, 31475, 31476, 31477, 31478, 31479, 31480, 31481, 31482, 31483, 31484, 31485, 31486, 31487, 31488, 31489, 31490, 31491, 31492, 31493, 31494, 31495, 31496, 31497, 31498, 31499, 31500, 31501, 31502, 31503, 31504, 31505, 31506, 31507, 31508, 31509, 31510, 31511, 31511, 31512 };

            //DUPLICIDADES
            //int[] listOfItems = new[] { 1, 2, 3, 6, 8, 9, 10, 12, 14, 15, 16, 17, 19, 23, 27, 30, 31, 33, 37, 38, 40, 41, 41, 42, 42, 43, 47, 48, 49, 50, 53, 55, 57, 58, 60, 61, 63, 65, 66, 66, 68, 71, 72, 74, 75, 76, 77, 78, 80, 81, 82, 83, 84, 85, 87, 89, 90, 91, 92, 98, 99, 100, 101, 103, 104, 105, 109, 110, 112, 114, 115, 116, 118, 119, 120, 122, 124, 127, 129, 135, 136, 139, 140, 141, 145, 152, 161, 162, 165, 168, 169, 171, 175, 176, 177, 178, 179, 180, 181, 182, 184, 186, 187, 190, 193, 194, 196, 198, 201, 203, 207, 208, 212, 215, 217, 218, 225, 227, 228, 230, 232, 233, 234, 235, 236, 237, 239, 241, 243, 247, 253, 255, 258, 259, 261, 262, 263, 264, 265, 266, 267, 267, 269, 270, 272, 273, 274, 277, 284, 285, 287, 288, 291, 291, 292, 297, 298, 302, 307, 311, 312, 313, 315, 316, 317, 318, 319, 322, 323, 324, 326, 328, 329, 331, 333, 336, 338, 340, 342, 343, 344, 345, 349, 351, 353, 354, 379, 392, 395, 396, 397, 400, 404, 406, 426, 428, 429, 440, 440, 461, 462, 473, 475, 476, 491, 503, 512, 524, 526, 528, 534, 561, 573, 581, 587, 588, 590, 592, 597, 598, 600, 601, 602, 604, 607, 608, 609, 610, 612, 614, 620, 623, 625, 626, 630, 632, 634, 635, 636, 640, 641, 642, 643, 646, 647, 647, 648, 650, 653, 654, 655, 657, 657, 659, 662, 663, 665, 666, 667, 670, 671, 674, 677, 678, 679, 680, 681, 683, 688, 700, 702, 705, 706, 708, 710, 711, 713, 714, 715, 717, 718, 719, 721, 723, 725, 726, 728, 729, 730, 733, 735, 737, 739, 740, 742, 744, 746, 747, 748, 750, 751, 753, 754, 759, 760, 762, 764, 764, 765, 766, 768, 769, 771, 775, 776, 778, 781, 782, 783, 784, 786, 788, 789, 793, 796, 797, 798, 803, 804, 805, 806, 807, 808, 809, 810, 812, 816, 817, 818, 819, 820, 824, 825, 827, 831, 832, 833, 835, 836, 838, 840, 842, 845, 859, 859, 866, 867, 868, 869, 871, 872, 876, 877, 878, 879, 880, 884, 891, 892, 893, 894, 896, 898, 904, 908, 912, 915, 917, 917, 922, 923, 928, 931, 933, 935, 937, 938, 939, 944, 945, 948, 949, 950, 951, 953, 954, 955, 956, 957, 958, 959, 960, 961, 962, 968, 969, 970, 972, 973, 975, 976, 977, 978, 980, 984, 990, 992, 992, 994, 995, 999, 1000, 1001, 1006, 1007, 1009, 1011, 1013, 1015, 1016, 1017, 1018, 1019, 1023, 1025, 1026, 1027, 1032, 1033, 1034, 1035, 1036, 1041, 1042, 1046, 1047, 1055, 1057, 1060, 1061, 1066, 1067, 1071, 1073, 1075, 1076, 1078, 1079, 1081, 1083, 1084, 1085, 1086, 1088, 1089, 1090, 1093, 1096, 1097, 1098, 1098, 1100, 1101, 1102, 1103, 1104, 1105, 1108, 1111, 1113, 1113, 1116, 1119, 1120, 1120, 1122, 1126, 1127, 1128, 1137, 1138, 1139, 1141, 1142, 1143, 1146, 1147, 1148, 1149, 1150, 1152, 1152, 1157, 1161, 1166, 1174, 1174, 1175, 1178, 1181, 1185, 1190, 1192, 1193, 1194, 1194, 1195, 1196, 1197, 1198, 1199, 1202, 1204, 1210, 1211, 1213, 1215, 1217, 1220, 1223, 1225, 1225, 1231, 1234, 1235, 1238, 1239, 1240, 1242, 1243, 1244, 1245, 1249, 1252, 1253, 1255, 1256, 1257, 1258, 1260, 1264, 1265, 1268, 1269, 1272, 1273, 1276, 1277, 1278, 1279, 1281, 1282, 1282, 1283, 1284, 1285, 1286, 1287, 1288, 1291, 1298, 1301, 1302, 1304, 1305, 1306, 1310, 1311, 1313, 1317, 1320, 1322, 1325, 1328, 1329, 1331, 1334, 1335, 1336, 1339, 1341, 1342, 1343, 1344, 1345, 1347, 1349, 1350, 1353, 1355, 1356, 1356, 1358, 1361, 1362, 1364, 1366, 1367, 1368, 1369, 1370, 1371, 1373, 1374, 1375, 1377, 1380, 1381, 1382, 1383, 1384, 1386, 1387, 1389, 1393, 1394, 1397, 1398, 1399, 1401, 1403, 1405, 1406, 1409, 1410, 1412, 1413, 1414, 1416, 1417, 1417, 1419, 1420, 1421, 1423, 1427, 1430, 1431, 1432, 1432, 1433, 1437, 1438, 1441, 1445, 1446, 1447, 1449, 1452, 1454, 1455, 1458, 1462, 1468, 1469, 1472, 1474, 1475, 1479, 1483, 1484, 1486, 1487, 1489, 1495, 1497, 1498, 1498, 1499, 1501, 1504, 1506, 1509, 1510, 1510, 1512, 1517, 1518, 1519, 1520, 1522, 1523, 1525, 1526, 1528, 1529, 1532, 1533, 1535, 1536, 1543, 1544, 1547, 1548, 1549, 1552, 1553, 1554, 1559, 1560, 1562, 1565, 1567, 1569, 1570, 1571, 1572, 1573, 1574, 1579, 1581, 1583, 1584, 1585, 1586, 1587, 1590, 1591, 1593, 1595, 1597, 1598, 1600, 1606, 1607, 1609, 1910, 1911, 1912, 1922, 1924, 1927, 1928, 1932, 1934, 1937, 1938, 1939, 1940, 1943, 1945, 1946, 1947, 1948, 1952, 1957, 1959, 1963, 1970, 1972, 1973, 1974, 1975, 1977, 1978, 1987, 1988, 1989, 1991, 1996, 1997, 1998, 1999, 2001, 2003, 2005, 2006, 2007, 2008, 2009, 2012, 2012, 2013, 2014, 2016, 2019, 2020, 2022, 2023, 2024, 2028, 2031, 2038, 2039, 2040, 2041, 2042, 2043, 2045, 2046, 2047, 2049, 2051, 2052, 2053, 2054, 2055, 2056, 2057, 2059, 2060, 2061, 2065, 2069, 2070, 2071, 2075, 2076, 2077, 2082, 2085, 2087, 2088, 2089, 2091, 2092, 2094, 2096, 2098, 2102, 2103, 2103, 2104, 2107, 2109, 2111, 2113, 2114, 2115, 2117, 2117, 2118, 2119, 2120, 2121, 2122, 2123, 2124, 2126, 2127, 2127, 2128, 2130, 2132, 2133, 2134, 2135, 2136, 2137, 2139, 2142, 2143, 2144, 2146, 2150, 2154, 2155, 2156, 2156, 2158, 2159, 2161, 2164, 2166, 2167, 2173, 2176, 2176, 2177, 2179, 2180, 2183, 2187, 2190, 2192, 2195, 2196, 2197, 2200, 2202, 2205, 2208, 2215, 2225, 2238, 2271, 2277, 2280, 2292, 2293, 2300, 2306, 2317, 2329, 2336, 2340, 2344, 2345, 2346, 2348, 2353, 2354, 2360, 2368, 2394, 2398, 2406, 2411, 2413, 2414, 2417, 2423, 2428, 2430, 2432, 2442, 2453, 2470, 2485, 2496, 2503, 2514, 2523, 2530, 2532, 2534, 2535, 2537, 2542, 2543, 2544, 2547, 2550, 2552, 2553, 2553, 2555, 2555, 2556, 2557, 2558, 2563, 2564, 2565, 2568, 2569, 2570, 2572, 2574, 2576, 2581, 2586, 2587, 2590, 2591, 2592, 2593, 2594, 2596, 2607, 2623, 2626, 2627, 2628, 2630, 2633, 2638, 2641, 2642, 2648, 2648, 2654, 2656, 2659, 2665, 2670, 2672, 2673, 2675, 2682, 2683, 2684, 2685, 2686, 2687, 2688, 2693, 2696, 2697, 2698, 2699, 2700, 2701, 2703, 2704, 2704, 2707, 2708, 2710, 2712, 2714, 2715, 2717, 2719, 2726, 2731, 2736, 2738, 2740, 2746, 2751, 2754, 2755, 2756, 2757, 2758, 2759, 2762, 2764, 2767, 2771, 2772, 2775, 2779, 2780, 2781, 2782, 2785, 2786, 2787, 2788, 2789, 2790, 2792, 2796, 2800, 2801, 2802, 2804, 2806, 2812, 2814, 2817, 2820, 2821, 2821, 2823, 2824, 2826, 2827, 2828, 2829, 2832, 2834, 2838, 2839, 2840, 2843, 2847, 2850, 2850, 2852, 2853, 2855, 2856, 2859, 2862, 2863, 2866, 2867, 2870, 2871, 2873, 2877, 2879, 2880, 2886, 2887, 2888, 2889, 2890, 2891, 2892, 2896, 2897, 2901, 2903, 2904, 2906, 2911, 2912, 2913, 2935, 2941, 2946, 2949, 2952, 2954, 2955, 2958, 2968, 2969, 2975, 2980, 2982, 2986, 2987, 2988, 2990, 2991, 2992, 2993, 2994, 2996, 2998, 3001, 3002, 3005, 3006, 3011, 3012, 3013, 3014, 3020, 3023, 3024, 3027, 3029, 3032, 3035, 3037, 3038, 3039, 3042, 3044, 3045, 3045, 3047, 3048, 3056, 3058, 3059, 3060, 26701, 26705, 26710, 26711, 26713, 26715, 26716, 26721, 26722, 26723, 26724, 26725, 26730, 26731, 26732, 26734, 26735, 26781, 26782, 26791, 26791, 26792, 26792, 26795, 26798, 26800, 26809, 26811, 26812, 26817, 26818, 26825, 26826, 26827, 26828, 26829, 26832, 26834, 26837, 26839, 26840, 26842, 26844, 26847, 26848, 26849, 26851, 26852, 26854, 26857, 26862, 26863, 26866, 26867, 26869, 26874, 26875, 26878, 26879, 26880, 26885, 26888, 27002, 27003, 27005, 27006, 27007, 27008, 27009, 27010, 27011, 27012, 27013, 27016, 27019, 27020, 27024, 27034, 27037, 27040, 27042, 27044, 27046, 27047, 27048, 27048, 27049, 27049, 27050, 27050, 27052, 27053, 27053, 27054, 27055, 27057, 27058, 27059, 27061, 27062, 27064, 27065, 27066, 27070, 27072, 27073, 27074, 27075, 27079, 27080, 27081, 27086, 27087, 27088, 27089, 27090, 27092, 27092, 27093, 27094, 27095, 27096, 27099, 27100, 27102, 27104, 27107, 27108, 27109, 27110, 27111, 27112, 27113, 27114, 27115, 27115, 27116, 27118, 27119, 27120, 27122, 27125, 27126, 27127, 27128, 27129, 27130, 27131, 27132, 27133, 27139, 27140, 27141, 27142, 27143, 27144, 27145, 27146, 27147, 27150, 27152, 27153, 27159, 27161, 27162, 27165, 27169, 27172, 27174, 27175, 27176, 27177, 27179, 27182, 27184, 27185, 27186, 27188, 27190, 27194, 27195, 27196, 27199, 27200, 27201, 27204, 27207, 27209, 27210, 27213, 27214, 27216, 27217, 27219, 27220, 27221, 27225, 27227, 27228, 27231, 27232, 27233, 27234, 27235, 27236, 27242, 27243, 27244, 27245, 27248, 27250, 27253, 27257, 27258, 27259, 27263, 27265, 27266, 27267, 27271, 27274, 27288, 27293, 27298, 27299, 27319, 27320, 27325, 27326, 27330, 27333, 27335, 27337, 27338, 27339, 27342, 27343, 27345, 27352, 27354, 27355, 27356, 27357, 27367, 27370, 27377, 27380, 27381, 27383, 27384, 27385, 27386, 27387, 27388, 27389, 27391, 27392, 27395, 27396, 27399, 27401, 27402, 27405, 27411, 27414, 27415, 27416, 27417, 27421, 27422, 27430, 27435, 27442, 27443, 27444, 27446, 27447, 27448, 27455, 27463, 27473, 27476, 27483, 27484, 27494, 27495, 27497, 27505, 27509, 27510, 27530, 27532, 27533, 27534, 27535, 27536, 27537, 27546, 27547, 27548, 27549, 27550, 27551, 27552, 27556, 27556, 27557, 27567, 27569, 27571, 27572, 27573, 27575, 27575, 27576, 27580, 27586, 27587, 27591, 27596, 27598, 27599, 27603, 27604, 27613, 27614, 27615, 27618, 27619, 27620, 27623, 27630, 27632, 27635, 27637, 27639, 27642, 27643, 27644, 27644, 27645, 27646, 27647, 27651, 27652, 27653, 27654, 27656, 27665, 27669, 27670, 27671, 27672, 27672, 27673, 27677, 27687, 27689, 27701, 27705, 27706, 27707, 27708, 27709, 27710, 27711, 27712, 27713, 27719, 27722, 27726, 27727, 27728, 27729, 27730, 27731, 27732, 27733, 27734, 27735, 27739, 27750, 27751, 27755, 27757, 27758, 27759, 27761, 27765, 27766, 27767, 27768, 27769, 27771, 27772, 27787, 27788, 27789, 27791, 27793, 27794, 27795, 27796, 27800, 27804, 27814, 27820, 27821, 27822, 27828, 27829, 27835, 27842, 27843, 27846, 27852, 27853, 27854, 27855, 27857, 27858, 27860, 27862, 27863, 27864, 27867, 27870, 27871, 27872, 27872, 27874, 27875, 27876, 27877, 27878, 27879, 27880, 27881, 27883, 27884, 27885, 27886, 27887, 27888, 27889, 27890, 27891, 27892, 27893, 27894, 27895, 27896, 27898, 27899, 27900, 27901, 27902, 27903, 27905, 27906, 27908, 27910, 27911, 27912, 27913, 27914, 27915, 27917, 27918, 27919, 27920, 27922, 27923, 27924, 27925, 27926, 27927, 27928, 27929, 27932, 27933, 27934, 27935, 27936, 27937, 27938, 27939, 27940, 27941, 27943, 27944, 27945, 27946, 27947, 27948, 27949, 27950, 27951, 27952, 27956, 27957, 27958, 27960, 27963, 27964, 27965, 27966, 27968, 27969, 27970, 27971, 27972, 27974, 27975, 27977, 27978, 27982, 27983, 27984, 27985, 27986, 27987, 27992, 27993, 27995, 27996, 27997, 27998, 27999, 28000, 28001, 28002, 28003, 28004, 28005, 28007, 28008, 28009, 28010, 28011, 28012, 28013, 28014, 28016, 28017, 28018, 28019, 28020, 28021, 28022, 28023, 28023, 28024, 28026, 28027, 28028, 28029, 28030, 28031, 28033, 28034, 28036, 28037, 28038, 28039, 28041, 28042, 28043, 28044, 28045, 28046, 28047, 28048, 28049, 28050, 28051, 28052, 28053, 28055, 28056, 28057, 28058, 28059, 28060, 28061, 28062, 28063, 28064, 28065, 28066, 28067, 28071, 28072, 28073, 28074, 28076, 28077, 28078, 28079, 28081, 28082, 28083, 28084, 28085, 28086, 28089, 28090, 28091, 28093, 28094, 28096, 28097, 28098, 28099, 28101, 28102, 28104, 28105, 28106, 28107, 28109, 28110, 28111, 28112, 28113, 28114, 28115, 28116, 28117, 28118, 28119, 28120, 28121, 28122, 28123, 28124, 28125, 28126, 28127, 28128, 28129, 28130, 28131, 28132, 28133, 28134, 28135, 28136, 28137, 28139, 28140, 28141, 28143, 28144, 28145, 28146, 28147, 28148, 28149, 28150, 28151, 28152, 28153, 28154, 28155, 28156, 28157, 28158, 28159, 28160, 28161, 28162, 28163, 28164, 28167, 28168, 28169, 28170, 28171, 28172, 28173, 28174, 28175, 28177, 28178, 28179, 28180, 28181, 28182, 28183, 28184, 28185, 28186, 28187, 28188, 28189, 28190, 28191, 28192, 28193, 28194, 28195, 28197, 28199, 28200, 28201, 28202, 28203, 28204, 28205, 28206, 28207, 28208, 28209, 28210, 28211, 28213, 28214, 28215, 28216, 28217, 28218, 28219, 28220, 28221, 28222, 28223, 28224, 28225, 28226, 28227, 28228, 28229, 28230, 28231, 28232, 28233, 28234, 28235, 28236, 28237, 28238, 28239, 28240, 28241, 28242, 28245, 28246, 28247, 28248, 28249, 28250, 28251, 28253, 28254, 28255, 28257, 28259, 28275, 28276, 28277, 28278, 28285, 28286, 28287, 28288, 28305, 28308, 28313, 28323, 28324, 28325, 28326, 28327, 28328, 28329, 28330, 28331, 28331, 28332, 28333, 28334, 28335, 28336, 28337, 28338, 28339, 28340, 28341, 28342, 28343, 28344, 28345, 28346, 28347, 28348, 28349, 28350, 28351, 28352, 28353, 28354, 28354, 28355, 28356, 28357, 28358, 28359, 28360, 28361, 28362, 28363, 28364, 28365, 28366, 28367, 28368, 28369, 28370, 28371, 28372, 28373, 28374, 28375, 28376, 28377, 28378, 28380, 28381, 28382, 28383, 28384, 28385, 28386, 28387, 28388, 28389, 28390, 28391, 28392, 28393, 28394, 28394, 28395, 28396, 28397, 28398, 28399, 28400, 28401, 28402, 28403, 28404, 28405, 28406, 28407, 28408, 28409, 28410, 28411, 28412, 28413, 28414, 28415, 28416, 28417, 28418, 28419, 28420, 28421, 28422, 28423, 28424, 28425, 28426, 28427, 28428, 28429, 28430, 28431, 28432, 28433, 28434, 28436, 28437, 28438, 28439, 28440, 28441, 28442, 28443, 28444, 28445, 28446, 28447, 28448, 28449, 28450, 28451, 28452, 28453, 28454, 28455, 28456, 28457, 28458, 28459, 28460, 28461, 28462, 28463, 28464, 28465, 28466, 28467, 28468, 28476, 28477, 28478, 28479, 28480, 28481, 28482, 28483, 28484, 28485, 28486, 28487, 28488, 28489, 28490, 28491, 28492, 28493, 28494, 28495, 28496, 28497, 28498, 28499, 28500, 28501, 28502, 28503, 28504, 28505, 28506, 28507, 28508, 28509, 28510, 28514, 28515, 28516, 28527, 28528, 28529, 28530, 28531, 28532, 28533, 28534, 28535, 28536, 28537, 28538, 28539, 28540, 28541, 28542, 28543, 28544, 28545, 28546, 28548, 28549, 28550, 28551, 28552, 28553, 28554, 28555, 28556, 28557, 28558, 28559, 28560, 28562, 28563, 28563, 28564, 28565, 28566, 28566, 28567, 28568, 28568, 28569, 28570, 28572, 28573, 28574, 28575, 28576, 28577, 28578, 28579, 28580, 28581, 28582, 28584, 28585, 28586, 28587, 28588, 28589, 28590, 28592, 28593, 28594, 28595, 28596, 28597, 28598, 28599, 28600, 28601, 28602, 28604, 28605, 28606, 28607, 28608, 28609, 28610, 28611, 28612, 28613, 28614, 28615, 28616, 28617, 28618, 28619, 28620, 28621, 28622, 28623, 28624, 28625, 28626, 28627, 28628, 28629, 28630, 28631, 28632, 28634, 28635, 28636, 28637, 28639, 28639, 28640, 28642, 28643, 28644, 28645, 28646, 28647, 28648, 28649, 28650, 28651, 28652, 28653, 28654, 28655, 28656, 28657, 28658, 28659, 28660, 28661, 28661, 28662, 28663, 28664, 28665, 28666, 28667, 28668, 28669, 28670, 28671, 28672, 28673, 28674, 28680, 28681, 28682, 28683, 28684, 28685, 28686, 28687, 28688, 28689, 28690, 28691, 28692, 28693, 28694, 28695, 28696, 28697, 28698, 28699, 28700, 28701, 28702, 28703, 28704, 28705, 28706, 28707, 28708, 28710, 28711, 28712, 28713, 28714, 28715, 28716, 28717, 28718, 28719, 28720, 28721, 28722, 28723, 28724, 28725, 28726, 28727, 28728, 28729, 28730, 28731, 28732, 28733, 28734, 28735, 28736, 28737, 28737, 28738, 28739, 28740, 28741, 28742, 28743, 28744, 28745, 28746, 28747, 28748, 28749, 28750, 28752, 28752, 28753, 28754, 28755, 28756, 28757, 28758, 28759, 28760, 28761, 28762, 28763, 28764, 28765, 28766, 28767, 28768, 28769, 28770, 28771, 28772, 28773, 28774, 28775, 28776, 28777, 28778, 28779, 28780, 28781, 28782, 28783, 28784, 28785, 28786, 28787, 28788, 28789, 28790, 28791, 28792, 28793, 28794, 28795, 28796, 28797, 28798, 28799, 28800, 28801, 28802, 28803, 28804, 28804, 28805, 28805, 28806, 28807, 28809, 28810, 28811, 28812, 28813, 28814, 28815, 28816, 28817, 28818, 28819, 28820, 28821, 28822, 28823, 28824, 28825, 28826, 28827, 28828, 28829, 28830, 28831, 28832, 28833, 28834, 28835, 28836, 28837, 28838, 28839, 28840, 28841, 28842, 28843, 28844, 28845, 28846, 28847, 28848, 28849, 28850, 28851, 28852, 28853, 28854, 28855, 28856, 28857, 28858, 28859, 28860, 28861, 28862, 28863, 28863, 28864, 28865, 28866, 28867, 28868, 28870, 28871, 28872, 28874, 28875, 28876, 28877, 28878, 28879, 28880, 28881, 28882, 28883, 28884, 28885, 28886, 28887, 28888, 28889, 28890, 28891, 28893, 28894, 28895, 28896, 28897, 28898, 28899, 28900, 28901, 28902, 28903, 28904, 28905, 28907, 28908, 28909, 28910, 28911, 28912, 28914, 28915, 28916, 28917, 28918, 28919, 28920, 28921, 28922, 28923, 28924, 28925, 28926, 28927, 28928, 28929, 28930, 28931, 28932, 28933, 28934, 28935, 28936, 28937, 28938, 28939, 28940, 28941, 28942, 28943, 28944, 28945, 28946, 28947, 28948, 28949, 28950, 28951, 28952, 28953, 28954, 28955, 28957, 28958, 28959, 28960, 28961, 28962, 28963, 28964, 28965, 28966, 28967, 28968, 28969, 28970, 28971, 28972, 28973, 28974, 28975, 28976, 28977, 28978, 28979, 28980, 28981, 28982, 28983, 28984, 28985, 28986, 28987, 28988, 28989, 28990, 28991, 28992, 28993, 28994, 28995, 28996, 28996, 28997, 28998, 28999, 29000, 29001, 29002, 29003, 29004, 29005, 29006, 29007, 29008, 29009, 29010, 29011, 29012, 29013, 29014, 29015, 29016, 29017, 29018, 29019, 29020, 29021, 29022, 29023, 29024, 29025, 29026, 29027, 29028, 29029, 29030, 29031, 29032, 29033, 29034, 29035, 29036, 29037, 29038, 29039, 29040, 29041, 29042, 29043, 29044, 29045, 29046, 29047, 29048, 29049, 29050, 29051, 29052, 29053, 29054, 29055, 29056, 29057, 29058, 29059, 29060, 29061, 29062, 29063, 29064, 29065, 29066, 29067, 29068, 29069, 29070, 29071, 29072, 29073, 29074, 29075, 29076, 29077, 29078, 29079, 29080, 29081, 29082, 29083, 29084, 29085, 29086, 29087, 29088, 29089, 29090, 29091, 29092, 29093, 29094, 29095, 29096, 29097, 29098, 29099, 29100, 29101, 29102, 29103, 29104, 29105, 29106, 29107, 29108, 29109, 29111, 29112, 29113, 29114, 29115, 29116, 29117, 29118, 29119, 29120, 29121, 29122, 29123, 29124, 29125, 29126, 29127, 29128, 29129, 29130, 29131, 29132, 29133, 29134, 29135, 29136, 29137, 29138, 29139, 29140, 29141, 29142, 29143, 29144, 29145, 29146, 29147, 29148, 29150, 29151, 29152, 29153, 29154, 29155, 29156, 29157, 29158, 29159, 29160, 29161, 29162, 29163, 29164, 29165, 29166, 29168, 29169, 29170, 29171, 29172, 29173, 29175, 29176, 29177, 29178, 29179, 29180, 29181, 29182, 29183, 29184, 29185, 29186, 29187, 29188, 29189, 29190, 29191, 29192, 29193, 29194, 29195, 29196, 29197, 29198, 29199, 29200, 29201, 29202, 29203, 29204, 29205, 29206, 29207, 29208, 29209, 29210, 29211, 29212, 29213, 29214, 29215, 29216, 29217, 29218, 29219, 29220, 29221, 29222, 29223, 29224, 29225, 29226, 29227, 29228, 29229, 29230, 29231, 29232, 29233, 29234, 29235, 29236, 29237, 29238, 29239, 29240, 29241, 29242, 29243, 29244, 29245, 29246, 29247, 29248, 29249, 29250, 29251, 29252, 29253, 29254, 29255, 29256, 29257, 29258, 29259, 29260, 29261, 29262, 29263, 29264, 29265, 29266, 29267, 29268, 29269, 29270, 29271, 29272, 29273, 29274, 29275, 29276, 29277, 29278, 29279, 29280, 29281, 29282, 29283, 29285, 29286, 29287, 29289, 29290, 29291, 29292, 29293, 29294, 29295, 29296, 29297, 29298, 29299, 29300, 29301, 29302, 29303, 29304, 29305, 29306, 29307, 29308, 29309, 29310, 29311, 29312, 29313, 29314, 29315, 29316, 29317, 29318, 29319, 29320, 29321, 29322, 29323, 29324, 29325, 29326, 29327, 29328, 29329, 29330, 29331, 29332, 29333, 29334, 29335, 29336, 29337, 29338, 29339, 29340, 29341, 29343, 29344, 29345, 29346, 29347, 29348, 29349, 29350, 29351, 29352, 29353, 29354, 29355, 29356, 29357, 29358, 29359, 29360, 29361, 29362, 29363, 29364, 29365, 29366, 29367, 29368, 29369, 29370, 29371, 29372, 29373, 29374, 29375, 29376, 29377, 29378, 29379, 29380, 29381, 29382, 29383, 29384, 29385, 29386, 29387, 29388, 29389, 29390, 29391, 29392, 29393, 29394, 29395, 29396, 29397, 29398, 29399, 29400, 29401, 29402, 29403, 29404, 29405, 29406, 29407, 29408, 29409, 29410, 29411, 29412, 29413, 29414, 29415, 29416, 29417, 29418, 29419, 29420, 29421, 29422, 29423, 29424, 29425, 29426, 29427, 29428, 29429, 29430, 29431, 29432, 29433, 29434, 29435, 29436, 29437, 29438, 29439, 29440, 29441, 29442, 29443, 29444, 29445, 29446, 29447, 29448, 29449, 29450, 29451, 29452, 29453, 29454, 29454, 29455, 29456, 29457, 29458, 29458, 29459, 29460, 29461, 29462, 29469, 29470, 29471, 29472, 29473, 29474, 29475, 29476, 29477, 29478, 29479, 29480, 29481, 29482, 29484, 29485, 29486, 29487, 29488, 29489, 29490, 29491, 29492, 29493, 29494, 29495, 29496, 29497, 29498, 29499, 29500, 29501, 29502, 29503, 29504, 29505, 29506, 29507, 29508, 29509, 29510, 29511, 29512, 29513, 29514, 29515, 29516, 29517, 29518, 29519, 29520, 29521, 29522, 29523, 29524, 29525, 29526, 29527, 29528, 29529, 29530, 29531, 29532, 29533, 29534, 29535, 29536, 29537, 29538, 29539, 29540, 29541, 29542, 29543, 29544, 29545, 29546, 29548, 29549, 29550, 29551, 29552, 29553, 29554, 29555, 29556, 29557, 29558, 29559, 29560, 29561, 29562, 29563, 29564, 29565, 29566, 29567, 29568, 29569, 29570, 29571, 29572, 29573, 29574, 29575, 29576, 29577, 29578, 29579, 29580, 29581, 29582, 29583, 29584, 29585, 29586, 29587, 29588, 29589, 29590, 29591, 29592, 29594, 29595, 29596, 29597, 29598, 29599, 29600, 29601, 29602, 29603, 29604, 29605, 29607, 29608, 29609, 29610, 29611, 29612, 29613, 29614, 29615, 29616, 29617, 29618, 29619, 29620, 29621, 29622, 29623, 29624, 29625, 29626, 29627, 29628, 29629, 29630, 29631, 29632, 29633, 29634, 29635, 29636, 29637, 29638, 29639, 29640, 29641, 29642, 29643, 29644, 29645, 29646, 29647, 29648, 29649, 29650, 29651, 29652, 29653, 29654, 29655, 29656, 29657, 29658, 29659, 29660, 29661, 29662, 29663, 29664, 29665, 29666, 29667, 29668, 29669, 29670, 29671, 29672, 29673, 29674, 29675, 29676, 29677, 29678, 29679, 29680, 29681, 29682, 29683, 29684, 29685, 29686, 29687, 29688, 29689, 29690, 29691, 29692, 29693, 29694, 29695, 29696, 29697, 29698, 29699, 29700, 29702, 29703, 29704, 29705, 29706, 29707, 29708, 29709, 29710, 29711, 29712, 29713, 29714, 29715, 29716, 29717, 29718, 29719, 29720, 29721, 29722, 29723, 29724, 29725, 29726, 29727, 29728, 29729, 29730, 29731, 29732, 29733, 29734, 29735, 29736, 29738, 29739, 29740, 29741, 29742, 29743, 29744, 29745, 29746, 29747, 29748, 29749, 29751, 29752, 29753, 29754, 29755, 29756, 29757, 29758, 29759, 29760, 29761, 29763, 29764, 29765, 29766, 29767, 29768, 29769, 29771, 29772, 29773, 29774, 29775, 29776, 29777, 29778, 29779, 29780, 29781, 29783, 29784, 29785, 29786, 29787, 29788, 29789, 29790, 29791, 29792, 29793, 29794, 29795, 29796, 29797, 29798, 29799, 29800, 29801, 29802, 29803, 29804, 29805, 29806, 29807, 29808, 29809, 29810, 29811, 29812, 29813, 29814, 29815, 29816, 29817, 29818, 29819, 29820, 29821, 29822, 29823, 29824, 29825, 29826, 29827, 29828, 29829, 29830, 29831, 29832, 29833, 29834, 29835, 29836, 29837, 29838, 29839, 29840, 29841, 29842, 29843, 29844, 29845, 29846, 29847, 29848, 29849, 29850, 29851, 29852, 29853, 29854, 29855, 29856, 29857, 29858, 29859, 29860, 29861, 29862, 29863, 29864, 29865, 29866, 29867, 29868, 29869, 29870, 29871, 29872, 29873, 29874, 29875, 29876, 29877, 29878, 29879, 29880, 29881, 29882, 29883, 29884, 29885, 29886, 29887, 29888, 29889, 29890, 29891, 29892, 29893, 29894, 29895, 29896, 29897, 29898, 29899, 29900, 29901, 29902, 29903, 29904, 29905, 29906, 29907, 29908, 29909, 29910, 29911, 29912, 29913, 29914, 29915, 29916, 29917, 29918, 29919, 29920, 29921, 29922, 29923, 29924, 29925, 29926, 29927, 29928, 29929, 29930, 29931, 29932, 29933, 29934, 29935, 29936, 29937, 29938, 29939, 29940, 29941, 29942, 29943, 29944, 29945, 29946, 29947, 29948, 29949, 29950, 29951, 29952, 29953, 29954, 29955, 29956, 29957, 29958, 29959, 29960, 29961, 29962, 29963, 29964, 29965, 29966, 29967, 29968, 29969, 29970, 29971, 29972, 29973, 29974, 29975, 29976, 29977, 29978, 29979, 29980, 29981, 29982, 29983, 29985, 29987, 29988, 29989, 29990, 29991, 29993, 29994, 29995, 29996, 29997, 29998, 29999, 30000, 30001, 30002, 30003, 30004, 30006, 30007, 30008, 30009, 30010, 30011, 30012, 30013, 30014, 30015, 30016, 30017, 30018, 30019, 30020, 30021, 30022, 30023, 30024, 30025, 30026, 30027, 30028, 30029, 30030, 30031, 30032, 30033, 30034, 30036, 30037, 30038, 30039, 30040, 30041, 30042, 30043, 30044, 30045, 30046, 30047, 30048, 30049, 30050, 30051, 30052, 30053, 30054, 30055, 30056, 30057, 30058, 30059, 30060, 30061, 30062, 30063, 30064, 30065, 30066, 30067, 30068, 30069, 30070, 30071, 30072, 30073, 30074, 30075, 30076, 30077, 30078, 30079, 30080, 30081, 30082, 30083, 30084, 30085, 30086, 30087, 30088, 30089, 30090, 30091, 30092, 30093, 30094, 30095, 30096, 30097, 30098, 30099, 30100, 30101, 30103, 30104, 30105, 30106, 30107, 30108, 30111, 30112, 30113, 30114, 30115, 30116, 30117, 30118, 30119, 30120, 30121, 30122, 30123, 30124, 30125, 30126, 30127, 30127, 30128, 30129, 30130, 30131, 30132, 30133, 30134, 30135, 30136, 30137, 30138, 30139, 30140, 30141, 30142, 30143, 30144, 30145, 30146, 30147, 30148, 30149, 30150, 30151, 30152, 30153, 30154, 30155, 30156, 30157, 30158, 30159, 30160, 30161, 30162, 30163, 30164, 30165, 30166, 30167, 30168, 30169, 30170, 30171, 30172, 30173, 30174, 30175, 30176, 30177, 30178, 30179, 30180, 30181, 30182, 30183, 30184, 30185, 30186, 30187, 30188, 30189, 30190, 30191, 30192, 30193, 30194, 30195, 30196, 30197, 30198, 30199, 30200, 30201, 30202, 30203, 30204, 30205, 30206, 30207, 30208, 30209, 30210, 30211, 30212, 30213, 30214, 30215, 30216, 30217, 30218, 30219, 30220, 30221, 30222, 30223, 30224, 30225, 30226, 30227, 30228, 30229, 30230, 30231, 30232, 30233, 30234, 30235, 30236, 30237, 30238, 30239, 30240, 30241, 30242, 30243, 30244, 30245, 30246, 30247, 30248, 30249, 30250, 30251, 30252, 30253, 30254, 30255, 30256, 30257, 30258, 30260, 30261, 30262, 30263, 30264, 30265, 30266, 30267, 30268, 30269, 30270, 30271, 30272, 30273, 30274, 30275, 30276, 30278, 30279, 30280, 30281, 30282, 30283, 30284, 30285, 30286, 30287, 30288, 30289, 30290, 30291, 30292, 30293, 30294, 30295, 30296, 30297, 30298, 30299, 30300, 30301, 30302, 30303, 30304, 30305, 30306, 30307, 30308, 30309, 30310, 30311, 30312, 30313, 30314, 30315, 30316, 30317, 30318, 30319, 30320, 30321, 30322, 30323, 30324, 30325, 30326, 30327, 30328, 30329, 30330, 30331, 30332, 30333, 30334, 30335, 30336, 30337, 30338, 30339, 30340, 30341, 30342, 30343, 30344, 30345, 30346, 30347, 30348, 30349, 30350, 30351, 30352, 30353, 30354, 30355, 30356, 30357, 30358, 30359, 30360, 30361, 30362, 30363, 30364, 30365, 30366, 30367, 30368, 30369, 30370, 30371, 30372, 30373, 30374, 30375, 30376, 30377, 30378, 30379, 30380, 30381, 30382, 30383, 30384, 30385, 30386, 30387, 30388, 30389, 30390, 30391, 30392, 30393, 30394, 30395, 30396, 30397, 30398, 30399, 30400, 30401, 30402, 30403, 30404, 30405, 30406, 30407, 30408, 30409, 30410, 30411, 30412, 30413, 30414, 30415, 30416, 30417, 30418, 30419, 30420, 30421, 30422, 30423, 30424, 30425, 30426, 30427, 30428, 30429, 30430, 30431, 30432, 30433, 30434, 30435, 30436, 30437, 30438, 30439, 30440, 30441, 30442, 30443, 30444, 30445, 30446, 30447, 30448, 30449, 30450, 30451, 30452, 30453, 30454, 30455, 30456, 30457, 30458, 30459, 30460, 30461, 30462, 30463, 30464, 30465, 30466, 30467, 30468, 30469, 30470, 30472, 30473, 30474, 30475, 30476, 30477, 30478, 30479, 30480, 30481, 30482, 30483, 30484, 30485, 30486, 30487, 30488, 30489, 30490, 30491, 30492, 30493, 30494, 30495, 30496, 30497, 30498, 30499, 30500, 30501, 30502, 30503, 30504, 30505, 30506, 30507, 30508, 30509, 30510, 30511, 30512, 30513, 30514, 30515, 30516, 30517, 30518, 30519, 30520, 30521, 30522, 30523, 30524, 30525, 30526, 30527, 30528, 30529, 30530, 30531, 30532, 30533, 30534, 30535, 30536, 30537, 30538, 30539, 30540, 30541, 30542, 30543, 30544, 30545, 30546, 30547, 30548, 30549, 30550, 30551, 30552, 30553, 30554, 30555, 30556, 30557, 30557, 30559, 30560, 30561, 30562, 30563, 30564, 30565, 30566, 30567, 30568, 30569, 30570, 30571, 30572, 30573, 30575, 30576, 30577, 30578, 30579, 30580, 30581, 30582, 30583, 30584, 30585, 30586, 30587, 30588, 30589, 30590, 30591, 30592, 30593, 30594, 30595, 30596, 30597, 30598, 30599, 30600, 30601, 30602, 30603, 30604, 30605, 30606, 30607, 30608, 30609, 30610, 30611, 30612, 30613, 30614, 30615, 30616, 30617, 30618, 30619, 30620, 30621, 30622, 30623, 30624, 30625, 30626, 30627, 30628, 30629, 30630, 30631, 30632, 30633, 30634, 30635, 30636, 30637, 30638, 30639, 30640, 30641, 30642, 30643, 30644, 30645, 30646, 30647, 30648, 30649, 30650, 30651, 30652, 30653, 30655, 30656, 30657, 30658, 30659, 30660, 30661, 30662, 30663, 30664, 30665, 30666, 30667, 30668, 30669, 30670, 30671, 30672, 30673, 30674, 30675, 30676, 30677, 30678, 30679, 30680, 30681, 30682, 30683, 30684, 30685, 30686, 30687, 30688, 30689, 30690, 30691, 30692, 30693, 30694, 30695, 30696, 30697, 30698, 30699, 30700, 30701, 30702, 30703, 30704, 30705, 30706, 30707, 30708, 30709, 30710, 30711, 30712, 30713, 30714, 30715, 30716, 30717, 30718, 30719, 30720, 30721, 30722, 30723, 30724, 30725, 30726, 30727, 30728, 30729, 30730, 30731, 30732, 30733, 30734, 30735, 30736, 30737, 30738, 30739, 30740, 30741, 30742, 30743, 30744, 30745, 30746, 30747, 30748, 30749, 30750, 30751, 30752, 30753, 30754, 30755, 30756, 30757, 30760, 30761, 30762, 30763, 30767, 30768, 30769, 30770, 30771, 30772, 30773, 30774, 30776, 30777, 30778, 30779, 30780, 30781, 30782, 30783, 30784, 30785, 30786, 30787, 30788, 30789, 30790, 30791, 30792, 30793, 30794, 30795, 30796, 30797, 30798, 30799, 30800, 30801, 30802, 30803, 30804, 30805, 30806, 30807, 30808, 30809, 30810, 30813, 30814, 30815, 30816, 30819, 30820, 30821, 30822, 30823, 30827, 30828, 30829, 30830, 30831, 30832, 30833, 30834, 30836, 30837, 30838, 30839, 30840, 30841, 30842, 30843, 30844, 30845, 30846, 30847, 30848, 30849, 30850, 30851, 30853, 30855, 30856, 30857, 30858, 30859, 30860, 30861, 30862, 30863, 30864, 30865, 30866, 30867, 30868, 30869, 30870, 30871, 30872, 30873, 30874, 30875, 30876, 30877, 30878, 30879, 30880, 30881, 30882, 30883, 30885, 30886, 30887, 30888, 30889, 30890, 30891, 30892, 30893, 30894, 30895, 30896, 30897, 30898, 30899, 30900, 30901, 30902, 30903, 30904, 30905, 30906, 30907, 30908, 30909, 30910, 30911, 30912, 30913, 30914, 30915, 30916, 30917, 30918, 30919, 30920, 30921, 30922, 30923, 30924, 30925, 30926, 30927, 30928, 30929, 30930, 30931, 30932, 30933, 30934, 30935, 30936, 30937, 30938, 30939, 30940, 30941, 30942, 30943, 30944, 30945, 30946, 30947, 30948, 30949, 30950, 30951, 30952, 30953, 30954, 30955, 30956, 30957, 30958, 30959, 30960, 30961, 30962, 30963, 30964, 30965, 30966, 30967, 30968, 30969, 30970, 30971, 30972, 30973, 30974, 30975, 30976, 30977, 30978, 30979, 30980, 30981, 30982, 30983, 30984, 30985, 30987, 30988, 30989, 30990, 30991, 30992, 30993, 30994, 30995, 30996, 30997, 30998, 30999, 31000, 31001, 31002, 31003, 31005, 31006, 31007, 31008, 31009, 31010, 31011, 31012, 31013, 31014, 31015, 31016, 31017, 31018, 31019, 31020, 31021, 31022, 31023, 31024, 31025, 31026, 31027, 31028, 31029, 31030, 31031, 31032, 31033, 31034, 31035, 31036, 31037, 31038, 31039, 31040, 31041, 31042, 31043, 31044, 31045, 31046, 31047, 31048, 31049, 31050, 31051, 31052, 31053, 31054, 31055, 31056, 31057, 31058, 31059, 31060, 31061, 31062, 31063, 31064, 31065, 31066, 31067, 31068, 31069, 31070, 31071, 31072, 31073, 31074, 31075, 31076, 31077, 31078, 31079, 31080, 31081, 31082, 31083, 31084, 31085, 31086, 31087, 31089, 31090, 31091, 31092, 31093, 31094, 31095, 31096, 31097, 31098, 31099, 31101, 31102, 31103, 31104, 31105, 31106, 31107, 31108, 31109, 31110, 31111, 31112, 31113, 31114, 31115, 31117, 31118, 31119, 31120, 31121, 31122, 31123, 31124, 31125, 31126, 31127, 31128, 31129, 31130, 31131, 31132, 31133, 31134, 31135, 31136, 31137, 31138, 31139, 31140, 31141, 31142, 31143, 31144, 31145, 31146, 31147, 31148, 31149, 31150, 31151, 31152, 31153, 31154, 31155, 31156, 31157, 31158, 31159, 31160, 31161, 31162, 31163, 31164, 31165, 31166, 31167, 31168, 31169, 31170, 31171, 31172, 31173, 31174, 31175, 31176, 31177, 31178, 31179, 31180, 31181, 31182, 31183, 31184, 31185, 31186, 31187, 31188, 31189, 31190, 31191, 31192, 31193, 31194, 31195, 31197, 31198, 31199, 31200, 31201, 31202, 31203, 31204, 31205, 31206, 31207, 31208, 31209, 31210, 31211, 31212, 31213, 31214, 31215, 31216, 31217, 31218, 31219, 31220, 31221, 31222, 31223, 31224, 31225, 31226, 31227, 31228, 31229, 31230, 31231, 31232, 31233, 31234, 31235, 31236, 31237, 31238, 31239, 31240, 31241, 31242, 31243, 31244, 31245, 31246, 31247, 31248, 31249, 31250, 31251, 31252, 31253, 31254, 31255, 31256, 31257, 31258, 31259, 31260, 31261, 31262, 31263, 31264, 31265, 31266, 31267, 31268, 31269, 31270, 31271, 31272, 31273, 31274, 31275, 31276, 31277, 31278, 31279, 31280, 31281, 31282, 31283, 31284, 31285, 31286, 31287, 31288, 31289, 31290, 31291, 31292, 31293, 31294, 31295, 31296, 31298, 31299, 31300, 31301, 31302, 31303, 31304, 31305, 31306, 31307, 31308, 31309, 31310, 31311, 31312, 31313, 31314, 31315, 31316, 31317, 31318, 31319, 31320, 31321, 31322, 31323, 31324, 31325, 31326, 31327, 31328, 31329, 31330, 31331, 31332, 31333, 31334, 31335, 31336, 31337, 31338, 31339, 31340, 31341, 31342, 31343, 31344, 31345, 31346, 31347, 31348, 31349, 31350, 31352, 31353, 31354, 31355, 31356, 31357, 31358, 31359, 31360, 31361, 31362, 31363, 31364, 31365, 31366, 31367, 31368, 31369, 31370, 31371, 31372, 31373, 31374, 31375, 31376, 31377, 31378, 31379, 31380, 31381, 31382, 31383, 31384, 31385, 31386, 31387, 31388, 31389, 31390, 31391, 31392, 31393, 31394, 31395, 31396, 31397, 31398, 31399, 31400, 31401, 31402, 31403, 31404, 31405, 31406, 31407, 31408, 31409, 31410, 31411, 31412, 31413, 31414, 31415, 31416, 31417, 31418, 31419, 31420, 31421, 31422, 31423, 31424, 31425, 31426, 31427, 31428, 31429, 31430, 31431, 31432, 31433, 31434, 31435, 31436, 31437, 31438, 31439, 31440, 31441, 31442, 31443, 31444, 31445, 31446, 31447, 31448, 31449, 31450, 31451, 31452, 31453, 31454, 31455, 31456, 31457, 31458, 31459, 31460, 31461, 31462, 31463, 31464, 31465, 31466, 31467, 31468, 31469, 31470, 31471, 31472, 31473, 31474, 31475, 31476, 31477, 31478, 31479, 31480, 31481, 31482, 31483, 31484, 31485, 31486, 31487, 31488, 31489, 31490, 31491, 31492, 31493, 31494, 31495, 31496, 31497, 31498, 31499, 31500, 31501, 31502, 31503, 31504, 31505, 31506, 31507, 31508, 31509 };

            //DUPLICADOS
            //41, 42, 66, 267, 291, 440, 647, 657, 764, 859, 917, 992, 1098, 1113, 1120, 1152, 1174, 1194, 1225, 1282, 1356, 1417, 1432, 1498, 1510, 2012, 2103, 2117, 2127, 2156, 2176, 2553, 2555, 2648, 2704, 2821, 2850, 3045, 26791, 26792, 27048, 27049, 27050, 27053, 27092, 27115, 27556, 27575, 27644, 27672, 27872, 28023, 28331, 28354, 28394, 28563, 28566, 28568, 28639, 28661, 28737, 28752, 28804, 28805, 28863, 28996, 29454, 29458, 30127, 30557, 31511, 

            var duplicates = listOfItems.GroupBy(i => i).Where(g => g.Count() > 1).Select(g => g.Key);

            string fim = String.Join(",", duplicates);

            //StringBuilder sb = new StringBuilder();
            //foreach (var d in duplicates)
            //{
            //    //Console.WriteLine(d);
            //    sb.Append(d);
            //    sb.Append(", ");
            //}
            //string result = sb.ToString();
        }

        static void DivideTest()
        {
            //string valor = "18,2";
            //decimal valorDecimal = Convert.ToDecimal(valor);
            //double doubleValorFinal = (double)valorDecimal / 3;
            //string result = String.Format("{0:0.00}" ,doubleValorFinal);

            string valor = "3044546545640,26546540";

            double doubleValorFinal = Convert.ToDouble(valor);
            doubleValorFinal = doubleValorFinal / 3;
            string resultado = String.Format("{0:0.##}", doubleValorFinal);

        }

        static void AddSpaceIfNotEmpty()
        {
            string modulo = " ";

            string teste = "Instalação, na SE JAURU, de um módulo de conexão do compensador síncrono, "
                + ((modulo.Trim() == string.Empty) ? string.Empty : modulo.Trim() + " ") + "em 230 kV -  Mvar CS 100/-70 Mvar IMPERATRIZ CS1 MA.";

        }

        static void DataTable_Tests()
        {
            DataTable dt = new DataTable("conta");

            using (SqlConnection conn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True"))
            {
                conn.Open();
                //                string query = @" SELECT C.NumConta numero, VlrDebito debito, VlrCredito credito, VlrSaldo saldo
                //                                      FROM .Lancamento L INNER JOIN .Conta C ON L.IdeConta = C.IdeConta
                //                                      WHERE ideBalancete = " + row[3];
                string query = @"
                    Select	P.ProductName, 
		                    P.UnitPrice, C.CategoryName, 
		                    C.Description, 
		                    S.CompanyName, 
		                    S.Region, 
		                    S.Country
                    FROM Products P
	                    INNER JOIN 
		                    Categories C ON P.CategoryID = C.CategoryID
	                    INNER JOIN 
		                    Suppliers S ON S.SupplierID = P.SupplierID";

                SqlCommand cmd = new SqlCommand(query, conn);
                dt.Load(cmd.ExecuteReader());
            }

            dt.Columns.Add("Contrato1");
            dt.Columns.Add("Empresa1");

            dt.Columns.Add("Contrato2");
            dt.Columns.Add("Empresa2");

            dt.Columns.Add("Contrato3");
            dt.Columns.Add("Empresa3");

            dt.Columns.Add("Contrato4");
            dt.Columns.Add("Empresa4");

            dt.Columns.Add("Contrato5");
            dt.Columns.Add("Empresa5");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Country"].ToString().Trim().Equals("Brazil"))
                {
                    //Busca dados
                    for (int x = 0; x < 5; x++)
                    {
                        dt.Rows[i]["Contrato" + (x + 1)] = "TesteContrato" + (x + 1);
                        dt.Rows[i]["Empresa" + (x + 1)] = "TesteEmpresa" + (x + 1);
                    }
                }
            }
        }

        static void GeraPD()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            int x = 0;

            List<vPJRamo> ListaRamoPJ = new List<vPJRamo>();

            using (var db = new Entidades())
            {
                ListaRamoPJ = db.vPJRamo.Where(n => n.IdAgente == 394).ToList();
            }

            var ramos = String.Join(",", ListaRamoPJ.Select(l => l.DescRamo).ToArray());

            List<string> ls = new List<string>();
            ls.Add("one");
            ls.Add("two");
            string type = String.Join(",", ls.Select(s => s).ToArray());


            bool temDistribuicao = ListaRamoPJ.Where(r => r.IdRamo.Equals(3)).Count() > 0;
            bool temGeracao = ListaRamoPJ.Where(r => r.IdRamo.Equals(1)).Count() > 0;
            bool temTransmissao = ListaRamoPJ.Where(r => r.IdRamo.Equals(2)).Count() > 0;


            //Data Source=;Initial Catalog=;Persist Security Info=True;User ID=usr;Password=KZ0uvzYJ
            //using (SqlConnection conn = new SqlConnection("Data Source=;Initial Catalog=;Integrated Security=True"))
            using (SqlConnection conn = new SqlConnection("Data Source=;Initial Catalog=;Persist Security Info=True;User ID=usr;Password=KZ0uvzYJ"))
            {
                SqlCommand cmd = new SqlCommand("[].[]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AnmReferenciaInicio", "201501");
                cmd.Parameters.AddWithValue("@AnmReferenciaFim", "201509");
                cmd.Parameters.AddWithValue("@IdeAgente", 394);
                var da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count == 0) dt = null;
            }

            #region Inicializa DECIMAL ARRAYS

            decimal[] somaRECEITA_Operacional = new decimal[dt.Columns.Count - 2];
            decimal[] somaDESPESA_Operacional = new decimal[dt.Columns.Count - 2];

            decimal[] RolINTERMEDIARIA_C = new decimal[dt.Columns.Count - 2];
            decimal[] RolFINAL_D = new decimal[dt.Columns.Count - 2];

            decimal[] PeD_E = new decimal[dt.Columns.Count - 2];
            decimal[] PEE_F = new decimal[dt.Columns.Count - 2];

            decimal[] PeD_G_DISTRIBUICAO = new decimal[dt.Columns.Count - 2];
            decimal[] MME_H_DISTRIBUICAO = new decimal[dt.Columns.Count - 2];
            decimal[] FNDCT_I_DISTRIBUICAO = new decimal[dt.Columns.Count - 2];

            decimal[] PeD_G_GERACAO = new decimal[dt.Columns.Count - 2];
            decimal[] MME_H_GERACAO = new decimal[dt.Columns.Count - 2];
            decimal[] FNDCT_I_GERACAO = new decimal[dt.Columns.Count - 2];

            decimal[] PeD_SOMA_G_H_I_DISTRIBUICAO = new decimal[dt.Columns.Count - 2];
            decimal[] PeD_SOMA_G_H_I_GERACAO = new decimal[dt.Columns.Count - 2];

            #endregion

            var countReceita = (from r in dt.AsEnumerable()
                                where r.Field<string>("DscTipo").Trim().Equals("Receita Operacional")
                                select r).Count();

            #region Totaliza os valores por coluna e armazena em array de decimais

            //Totaliza os valores por coluna e armazena em array de decimais
            for (int column = 0; column < dt.Columns.Count; column++)
            {
                if (dt.Columns[column].ColumnName.ToUpper().Equals("DSCCONTA")) continue;
                if (dt.Columns[column].ColumnName.ToUpper().Equals("DSCTIPO")) continue;

                for (int rows = 0; rows < dt.Rows.Count; rows++)
                {
                    string stringValue = dt.Rows[rows][column].ToString();
                    decimal d;

                    if (decimal.TryParse(stringValue, out d))
                    {
                        if (dt.Rows[rows][0].ToString().Trim().Equals("Receita Operacional"))
                            somaRECEITA_Operacional[x] += d;
                        else if (dt.Rows[rows][0].ToString().Trim().Equals("Despesa Operacional"))
                            somaDESPESA_Operacional[x] += d;
                    }
                }
                x++;
            }

            #endregion

            #region REALIZA OS CÁLCULOS PARA O P&D

            //REALIZA OS CÁLCULOS PARA O P&D
            for (int i = 0; i < somaRECEITA_Operacional.Count() - 1; i++)
            {
                RolINTERMEDIARIA_C[i] = (somaDESPESA_Operacional[i] < 0) ? somaRECEITA_Operacional[i] + somaDESPESA_Operacional[i] : somaRECEITA_Operacional[i] - somaDESPESA_Operacional[i];
                RolFINAL_D[i] = RolINTERMEDIARIA_C[i] / Convert.ToDecimal(1.01);
                PeD_E[i] = RolFINAL_D[i] * Convert.ToDecimal(0.01);

                PEE_F[i] = PeD_E[i] * Convert.ToDecimal(0.5);

                PeD_G_DISTRIBUICAO[i] = PeD_E[i] * Convert.ToDecimal(0.2);
                MME_H_DISTRIBUICAO[i] = PeD_E[i] * Convert.ToDecimal(0.1);
                FNDCT_I_DISTRIBUICAO[i] = PeD_E[i] * Convert.ToDecimal(0.2);

                PeD_G_GERACAO[i] = PeD_E[i] * Convert.ToDecimal(0.4);
                MME_H_GERACAO[i] = PeD_E[i] * Convert.ToDecimal(0.2);
                FNDCT_I_GERACAO[i] = PeD_E[i] * Convert.ToDecimal(0.4);

                PeD_SOMA_G_H_I_DISTRIBUICAO[i] = PeD_G_DISTRIBUICAO[i] + MME_H_DISTRIBUICAO[i] + FNDCT_I_DISTRIBUICAO[i];
                PeD_SOMA_G_H_I_GERACAO[i] = PeD_G_GERACAO[i] + MME_H_GERACAO[i] + FNDCT_I_GERACAO[i];
            }

            #endregion

            #region GERA O CALCULO DOS TOTAIS

            //GERA O CALCULO DOS TOTAIS
            RolINTERMEDIARIA_C[RolINTERMEDIARIA_C.Count() - 1] = RolINTERMEDIARIA_C.Sum();

            RolFINAL_D[RolFINAL_D.Count() - 1] = RolFINAL_D.Sum();
            PeD_E[PeD_E.Count() - 1] = PeD_E.Sum();
            PEE_F[PEE_F.Count() - 1] = PEE_F.Sum();

            PeD_G_DISTRIBUICAO[PeD_G_DISTRIBUICAO.Count() - 1] = PeD_G_DISTRIBUICAO.Sum();
            MME_H_DISTRIBUICAO[MME_H_DISTRIBUICAO.Count() - 1] = MME_H_DISTRIBUICAO.Sum();
            FNDCT_I_DISTRIBUICAO[FNDCT_I_DISTRIBUICAO.Count() - 1] = FNDCT_I_DISTRIBUICAO.Sum();

            PeD_G_GERACAO[PeD_G_GERACAO.Count() - 1] = PeD_G_GERACAO.Sum();
            MME_H_GERACAO[MME_H_GERACAO.Count() - 1] = MME_H_GERACAO.Sum();
            FNDCT_I_GERACAO[FNDCT_I_GERACAO.Count() - 1] = FNDCT_I_GERACAO.Sum();

            PeD_SOMA_G_H_I_DISTRIBUICAO[PeD_SOMA_G_H_I_DISTRIBUICAO.Count() - 1] = PeD_SOMA_G_H_I_DISTRIBUICAO.Sum();
            PeD_SOMA_G_H_I_GERACAO[PeD_SOMA_G_H_I_GERACAO.Count() - 1] = PeD_SOMA_G_H_I_GERACAO.Sum();

            #endregion

            //DscTipo	            DscConta	                        201501	201502	     201503	    Total
            //Receita Operacional	Fornecimento - CVA Ativa e Passiva	2923.93	15215207.87	-171498.64	15046633.16

            #region Insere Registros no DATATABLE

            //Monta Registro de totais de RECEITA
            GeraDataRow("( + ) RECEITA OPERACIONAL", ref somaRECEITA_Operacional, ref dt, ref dr);
            dt.Rows.InsertAt(dr, 0);

            //Monta Registro de totais de DESPESA
            GeraDataRow("( - ) DESPESA OPERACIONAL", ref somaDESPESA_Operacional, ref dt, ref dr);
            dt.Rows.InsertAt(dr, (countReceita + 1));

            //Monta Registro de "Programa de Eficiência Energética - PEE"
            GeraDataRow("Programa de Eficiência Energética - PEE", ref PEE_F, ref dt, ref dr);
            dt.Rows.Add(dr);

            //Monta Registro de "Pesquisa e Desenvolvimento - P&D"
            //OBS: VERIFICAR O TIPO DE RAMO DA EMPRESA
            //PeD_SOMA_G_H_I_DISTRIBUICAO OU PeD_SOMA_G_H_I_GERACAO
            GeraDataRow("Pesquisa e Desenvolvimento - P&D", ref PeD_SOMA_G_H_I_DISTRIBUICAO, ref dt, ref dr);
            dt.Rows.Add(dr);

            GeraDataRow("( = ) ROL INTERMEDIÁRIA", ref RolINTERMEDIARIA_C, ref dt, ref dr);
            dt.Rows.Add(dr);

            GeraDataRow("( = ) ROL FINAL", ref RolFINAL_D, ref dt, ref dr);
            dt.Rows.Add(dr);

            GeraDataRow("P&D + EE", ref PeD_E, ref dt, ref dr);
            dt.Rows.Add(dr);

            #endregion

            #region Altera o cabeçalho

            //Altera o cabeçalho
            string nomeColuna = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                nomeColuna = dt.Columns[i].ColumnName;

                if (nomeColuna.ToUpper().Equals("DSCTIPO")) continue;


                if (nomeColuna.ToUpper().Equals("DSCCONTA"))
                    dt.Columns[i].ColumnName = "DESCRIÇÃO";
                else if (nomeColuna.ToUpper().Equals("TOTAL"))
                    dt.Columns[i].ColumnName = "TOTAL DO PERÍODO";
                else
                    dt.Columns[i].ColumnName = (string)Enum.GetName(typeof(Mes), (Convert.ToInt32(nomeColuna.ToString().Substring(4, 2)) == 1) ? 0 : Convert.ToInt32(nomeColuna.ToString().Substring(4, 2)) - 1) + "/" + nomeColuna.ToString().Substring(0, 4);
            }

            #endregion

            DataTable dt2 = dt.Clone();
            dt.Columns.RemoveAt(0);
            dt2.Columns.RemoveAt(0);

            #region Gera outra DataTable para registros de cálculo adicionais

            temGeracao = false;
            temDistribuicao = true;

            if (temDis)
            {
                //Altera o Nome da Coluna
                dt2.Columns[0].ColumnName = "Cálculo para Distribuição";

                GeraDataRow2("PEE - Programa ", ref PEE_F, ref dt2, ref dr);
                dt2.Rows.Add(dr);
            }

            if (temGer)
            {
                if (!temDis)
                {
                    //Altera o nome da coluna
                    dt2.Columns[0].ColumnName = "Cálculo Geração";
                }
                else
                {
                    GeraDataRowTitulo("Cálculo Geração", ref dt2, ref dr);
                    dt2.Rows.Add(dr);
                }

                GeraDataRow2("P&D - Programa ", ref PeD_G_GERACAO, ref dt2, ref dr);
                dt2.Rows.Add(dr);


            }

            #endregion

        }

        static void GeraDataRow(string descricao, ref decimal[] valores, ref DataTable dt, ref DataRow dr)
        {
            string coluna = "";
            int sequencialArray = 0;
            dr = null;
            dr = dt.NewRow();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                coluna = dt.Columns[i].ColumnName;

                if (coluna.Trim().ToUpper().Equals("DSCTIPO"))
                    dr[coluna] = "";
                else if (coluna.Trim().ToUpper().Equals("DSCCONTA"))
                    dr[coluna] = descricao;
                else
                {
                    dr[coluna] = valores[sequencialArray];
                    sequencialArray++;
                }
            }
        }
        static void GeraDataRow2(string descricao, ref decimal[] valores, ref DataTable dt, ref DataRow dr)
        {
            string coluna = "";
            int sequencialArray = 0;
            dr = null;
            dr = dt.NewRow();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                coluna = dt.Columns[i].ColumnName;

                if (i == 0)
                {
                    dr[coluna] = descricao;
                }
                else
                {
                    dr[coluna] = valores[sequencialArray];
                    sequencialArray++;
                }
            }
        }
        static void GeraDataRowTitulo(string descricao, ref DataTable dt, ref DataRow dr)
        {
            string coluna = "";

            dr = null;
            dr = dt.NewRow();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                coluna = dt.Columns[i].ColumnName;

                if (i == 0)
                {
                    dr[coluna] = descricao;
                }
                else
                {
                    dr[coluna] = DBNull.Value;
                }
            }
        }

        static void GeraDadosDE_PARA()
        {
            StreamReader sr = null;
            StringBuilder builderScriptInsert = new StringBuilder();
            string nomeArquivo = "DE_PARA.csv";

            //Gera Pasta
            string pasta = AppDomain.CurrentDomain.BaseDirectory + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo);

            try
            {
                if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

                //sr = File.OpenText(@"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\Contas  2014 11 20.csv");
                sr = new StreamReader(@"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\Contas  2014 11 20.csv", Encoding.GetEncoding("ISO-8859-1"));

                string str = "";
                string[] linha = null;

                while (str != null)
                {
                    str = sr.ReadLine();
                    if (str == null) break;

                    linha = str.Split(';');
                    //if (str.IndexOf("mail: ") == -1) continue;

                    builderScriptInsert.AppendLine(("INSERT INTO [TESTE].[dbo].[ContaDePara] VALUES ("
                        + Convert.ToInt32(linha[0]) + ",'"
                        + ((string.IsNullOrEmpty(linha[1].Trim())) ? "NULL" : linha[1].Trim()) + "', '"
                        + ((string.IsNullOrEmpty(linha[2].Trim())) ? "NULL" : linha[2].Trim()) + "', '"
                        + ((string.IsNullOrEmpty(linha[3].Trim())) ? "NULL" : linha[3].Trim()) + "', '"
                        + ((string.IsNullOrEmpty(linha[4].Trim())) ? "NULL" : linha[4].Trim()) + "', '"
                        + ((string.IsNullOrEmpty(linha[5].Trim())) ? "NULL" : linha[5].Trim()) + "', '"
                        + ((string.IsNullOrEmpty(linha[6].Trim())) ? "NULL" : linha[6].Trim()) + "', '"
                        + ((string.IsNullOrEmpty(linha[7].Trim())) ? "NULL" : linha[7].Trim()) + "')").Replace("'NULL'", "null"));
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Cria arquivo SCRIPT Update
                string filePath = pasta + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo) + "_Script_INSERT.txt";
                File.WriteAllText(filePath, builderScriptInsert.ToString().Substring(0, builderScriptInsert.Length));
                sr.Close();
                builderScriptInsert = null;
            }
        }

        static void NorthWind_NotIn()
        {
            using (NorthwindEntities db = new NorthwindEntities())
            {
                //select ProductID, ProductName from dbo.Products where CategoryID not in (8)
                //var product8 = (from p in db.Products where p.CategoryID == 8 select p);
                var prodNotIn = db.Products.Where(x => x.CategoryID == 8).Select(y => y.ProductID);
                var result = db.Products.Where(x => !prodNotIn.Contains(x.ProductID));
            }
        }

        static void GeraArquivosTESTE_DE_PARA()
        {
            string path = @"C:\Users\fredsena\Documents\FRED_STUFF\DE_PARA_Plano_Contas__SEM6-7.csv";

            StreamReader sr = new StreamReader(path, Encoding.GetEncoding("ISO-8859-1"));
            StringBuilder builderInsert = new StringBuilder();

            string nomeArquivo = Path.GetFileNameWithoutExtension(path);

            //Gera Pasta
            string pasta = @"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\ARQUIVOS_TESTE_DE-PARA\" + Path.GetFileNameWithoutExtension(nomeArquivo);

            try
            {
                if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

                //var text = File.ReadAllText(file, Encoding.GetEncoding(codePage));
                //sr = File.ReadAllText(@"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\Plano_Contas__2014_Import_SEM_COL_GRAU.csv", Encoding.GetEncoding("ISO-8859-1"));

                string str = "";
                string[] linha = null;

                string dado = string.Empty;

                //loop
                while (str != null)
                {
                    str = sr.ReadLine();
                    if (str == null) break;
                    linha = str.Split(';');

                    if (linha[0] == "") continue;

                    //.PlanoContas2014Para2015
                    //NumConta2014 VARCHAR(9) NOT NULL,
                    //NumConta2015 VARCHAR(9) NOT NULL

                    // 0 1  2          3                         4   5           6
                    //10;A;111.02; APLICAÇÕES NO MERCADO ABERTO ;A;1101.2; Equivalentes de Caixa ;

                    if (!string.IsNullOrEmpty(linha[2].Trim()) && !string.IsNullOrEmpty(linha[5].Trim()))
                    {
                        dado = "INSERT INTO .PlanoContas2014Para2015 VALUES ('" + linha[2].Trim().Replace(".", "") + "','" + linha[5].Trim().Replace(".", "") + "')";
                    }

                    //dado = String.Format("{0};{1};{2};{3};{4};{5};{6};{7}", linha[0].Trim(), linha[1].Trim(), FormataConta(2010, linha[2].Trim()), linha[3].Trim(), linha[4].Trim(), linha[5].Trim(), linha[6].Trim(), linha[7].Trim());
                    builderInsert.AppendLine(dado);
                }
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Cria arquivo SCRIPT INSERT
                string filePath = pasta + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo) + ".txt";
                File.WriteAllText(filePath, builderInsert.ToString().Substring(0, builderInsert.Length));
                sr.Close();
                builderInsert = null;
            }
        }

        static void CountNodesXML()
        {
            string path = @"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\ARQUIVOS_TESTE\201409_A.xml";

            StreamReader sr = new StreamReader(path, Encoding.GetEncoding("ISO-8859-1"));
            StringBuilder builderInsert = new StringBuilder();

            string nomeArquivo = Path.GetFileNameWithoutExtension(path);

            //Gera Pasta
            string pasta = @"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\ARQUIVOS_TESTE_DE-PARA\" + Path.GetFileNameWithoutExtension(nomeArquivo);

            try
            {
                if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

                //var text = File.ReadAllText(file, Encoding.GetEncoding(codePage));
                //sr = File.ReadAllText(@"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\Plano_Contas__2014_Import_SEM_COL_GRAU.csv", Encoding.GetEncoding("ISO-8859-1"));

                string str = "";
                //string[] linha = null;

                string dado = string.Empty;

                //loop
                while (str != null)
                {
                    str = sr.ReadLine();
                    if (str == null) break;

                    if (str.IndexOf("<numero>") > -1)
                    {
                        //linha = str.Split(';');
                        //if (linha[0] == "") continue;
                        //dado = String.Format("{0};{1};{2};{3};{4};{5};{6};{7}", linha[0].Trim(), linha[1].Trim(), FormataConta(2010, linha[2].Trim()), linha[3].Trim(), linha[4].Trim(), linha[5].Trim(), linha[6].Trim(), linha[7].Trim());
                        builderInsert.AppendLine(str);
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Cria arquivo SCRIPT INSERT
                string filePath = pasta + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo) + ".txt";
                File.WriteAllText(filePath, builderInsert.ToString().Substring(0, builderInsert.Length));
                sr.Close();
                builderInsert = null;
            }
        }

        static void GeraXML()
        {


            foreach (var item in listaDados())
            {
                string[] row = item.Split(',');

                DataTable dt = new DataTable("conta");

                using (SqlConnection conn = new SqlConnection("Data Source=;Initial Catalog=;Integrated Security=True"))
                {
                    conn.Open();
                    string query = @" SELECT C.NumConta numero, VlrDebito debito, VlrCredito credito, VlrSaldo saldo
                                      FROM .Lancamento L INNER JOIN .Conta C ON L.IdeConta = C.IdeConta
                                      WHERE ideBalancete = " + row[3];

                    SqlCommand cmd = new SqlCommand(query, conn);
                    dt.Load(cmd.ExecuteReader());
                }

                StringWriter sw = new StringWriter();

                DataSet ds = new DataSet("contas");
                ds.Tables.Add(dt);
                //ds.WriteXml(@"C:\Teste_" + DateTime.Now.ToString("dd.MM.yyyy.hhmmss") + ".xml");

                ds.WriteXml(sw, XmlWriteMode.IgnoreSchema);

                //APL0383_AMPLA_201401_A.xml
                //lista.Add("6957,201407,IEMADEIRA,60798");

                string competencia = "";
                int ano = 0;
                int mes = 0;

                //50032	39	201405
                //60994	39	201404
                //61244	39	201403

                //if (row[1].Trim().Substring(4, 2) == "12")
                //{
                //    ano = Convert.ToInt32(row[1].Trim().Substring(0, 4)) + 1;
                //    mes = 1;
                //    competencia = ano + mes.ToString().PadLeft(2, '0');
                //}
                //else
                //{
                //    ano = Convert.ToInt32(row[1].Trim().Substring(0, 4));
                //    mes = Convert.ToInt32(row[1].Trim().Substring(4, 2)) + 1;
                //    competencia = ano + mes.ToString().PadLeft(2, '0');
                //}

                string result = "<?xml version='1.0' encoding='utf-8'?><><</ano></>"
                    + sw.ToString().Replace("0.00", "0,00").Replace(".", ",");

                //result = result.Replace("CODEMP", row[0].Trim().PadLeft(4, '0'));
                //result = result.Replace("CODMES", mes.ToString().PadLeft(2, '0'));
                //result = result.Replace("CODANO", ano.ToString()) + "</>";

                result = result.Replace("CODEMP", row[0].Trim().PadLeft(4, '0'));
                result = result.Replace("CODMES", row[1].Trim().Substring(4, 2));
                result = result.Replace("CODANO", row[1].Trim().Substring(0, 4)) + "</>";


                File.WriteAllText(@"C:\APL" + row[0].Trim().PadLeft(4, '0') + "_" + row[2].Trim() + "_" + row[1].Trim() + "_A.xml", result);
            }
        }

        #endregion

        static void RelatorioAvisoNotificacaoInadimplencia()
        {
            DataTable table1 = new DataTable();
            table1.Columns.Add("Data Envio", typeof(string));
            table1.Columns.Add("Qtd. Relatórios", typeof(string));

            DataTable dtemp = new DataTable();
            DataTable table2 = new DataTable();

            using (SqlConnection conn = new SqlConnection("Data Source=;Initial Catalog=;Integrated Security=True"))
            {
                conn.Open();
                string query = @" SELECT CAST(AV.DthEnvio AS DATE) [Data Envio], count(1) as [Qtd. Relatórios] 
                                    from . AV
	                                    INNER JOIN . AG ON AV.IdeAgente = AG.IdeAgente
	                                    INNER JOIN . PJ ON AG.IdeAgente = PJ.IdAgente
	                                    INNER JOIN . PE ON PE. = AV.
                                    where AV. = 1422
                                    group by CAST(AV.DthEnvio AS DATE) 
                                    order by CAST(AV.DthEnvio AS DATE)";

                SqlCommand cmd = new SqlCommand(query, conn);
                table1.Load(cmd.ExecuteReader());
            }

            //Clona a tabela
            dtemp = table1.Clone();

            DataRow dr;

            DataTable dtConsulta = new DataTable();
            string valor;
            bool passou = false;

            foreach (var item in table1.Rows)
            {
                dtConsulta = RetornaConsulta("1422", ((System.Data.DataRow)(item)).ItemArray[0].ToString());

                if (passou)
                {
                    //insere linha
                    dr = dtemp.NewRow();
                    dr[0] = "";
                    dr[1] = "";
                    dtemp.Rows.Add(dr);

                    //insere título
                    dr = dtemp.NewRow();
                    dr[0] = "Data Envio";
                    dr[1] = "Qtd. Relatórios";
                    dtemp.Rows.Add(dr);
                }

                //Insere em dtemp
                dr = dtemp.NewRow();

                dr[0] = Convert.ToDateTime(((System.Data.DataRow)(item)).ItemArray[0].ToString()).ToShortDateString();
                dr[1] = ((System.Data.DataRow)(item)).ItemArray[1].ToString();
                dtemp.Rows.Add(dr);

                dr = dtemp.NewRow();
                dr[0] = "Mês/Competencia";
                dr[1] = "Empresa";
                dtemp.Rows.Add(dr);

                foreach (var row in dtConsulta.Rows)
                {
                    dr = dtemp.NewRow();
                    valor = ((System.Data.DataRow)(row)).ItemArray[0].ToString();
                    dr[0] = (string)Enum.GetName(typeof(Mes), (Convert.ToInt32(valor.Substring(4, 2)) == 1) ? 0 : Convert.ToInt32(valor.Substring(4, 2)) - 1) + "/" + valor.Substring(0, 4);
                    dr[1] = ((System.Data.DataRow)(row)).ItemArray[1].ToString();
                    dtemp.Rows.Add(dr);
                }
                passou = true;
            }
        }

        static DataTable RetornaConsulta(string id, string data)
        {
            DataTable table2 = new DataTable();

            data = Convert.ToDateTime(data).ToString("yyyy-MM-dd");

            using (SqlConnection conn = new SqlConnection("Data Source=;Initial Catalog=;Integrated Security=True"))
            {
                conn.Open();
                string query = @" SELECT PE., PJ.RazaoSocialPJ
                                    from . AV
	                                    INNER JOIN .Agente AG ON AV.IdeAgente = AG.IdeAgente
	                                    INNER JOIN . PJ ON AG.IdeAgente = PJ.IdAgente
	                                    INNER JOIN .PeriodoEnvioArquivo PE ON PE.IdePeriodoEnvioArquivo = AV.IdePeriodoEnvioArquivo
                                    where AV.IdeDestinatario = " + id + " " + @"
                                    and CAST(AV.DthEnvio AS DATE) = '" + data + @"' group by PJ.RazaoSocialPJ, PE.AnmCompetencia
                                    order by PJ.RazaoSocialPJ, PE.AnmCompetencia";

                SqlCommand cmd = new SqlCommand(query, conn);
                table2.Load(cmd.ExecuteReader());

                return table2;
            }
        }

        static void ValidaBalancete012015()
        {
            using (var db = new Entidades())
            {
                ObjectParameter idcArquivoValido = new ObjectParameter("idcArquivoValido", typeof(Int32));
                var result = db.pValidaBalancete012015_Result(397, idcArquivoValido);
                Int32 valor = result.SingleOrDefault().Value;

                //var lista = db.ValidacaoBalancete012015.Where(x => x.IdeAgente == 397 && x.IdcValido == "N").Select(x => x.NumConta).ToList();
                //var erros = new List<string>();
                //foreach (var item in lista) erros.Add(string.Format("Erro na conta {0}, O valor do saldo da conta é inválido", item));

                //ValidacaoBalancete012015 tbValida = db.ValidacaoBalancete012015.First(i => i.IdeAgente == 394);
                //db.ValidacaoBalancete012015.DeleteObject(tbValida);

                //db.ExecuteStoreCommand("DELETE FROM .ValidacaoBalancete012015 WHERE IdeAgente = " + 394);

                db.SaveChanges();

            }
        }

        static void Gera_INSERT_DE_PARA_X()
        {
            string path = @"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\ARQUIVOS_TESTE_DE-PARA\DE-PARA_com_X_e_barra_IMPORTACAO_CSV.csv";

            StreamReader sr = new StreamReader(path, Encoding.GetEncoding("ISO-8859-1"));
            StringBuilder builderInsert = new StringBuilder();

            string nomeArquivo = Path.GetFileNameWithoutExtension(path);

            //Gera Pasta
            string pasta = @"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\IMPORTACAO\RESULT\" + Path.GetFileNameWithoutExtension(nomeArquivo);

            try
            {
                if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

                //var text = File.ReadAllText(file, Encoding.GetEncoding(codePage));
                //sr = File.ReadAllText(@"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\Plano_Contas__2014_Import_SEM_COL_GRAU.csv", Encoding.GetEncoding("ISO-8859-1"));

                string str = "";
                string[] linha = null;

                while (str != null)
                {
                    str = sr.ReadLine();
                    if (str == null) break;

                    linha = str.Split(';');

                    if (linha[0] == "") continue;

                    builderInsert.AppendLine("INSERT INTO [dbo].[ContaDeParaX] VALUES ('"
                        + linha[0].Trim() + "', '" + linha[1].Trim() + "', '" + linha[2].Trim() + "', '" + linha[3].Trim() + "')");
                }
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Cria arquivo SCRIPT INSERT
                string filePath = pasta + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo) + ".txt";
                File.WriteAllText(filePath, builderInsert.ToString().Substring(0, builderInsert.Length));
                sr.Close();
                builderInsert = null;
            }
        }

        static void GERA_SCRIPTS_PARAMETROS_ENVIO()
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();

            using (SqlConnection conn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=TESTE;Integrated Security=True"))
            {
                conn.Open();
                string query = @"SELECT datfrd data FROM ME_Feriado";
                SqlCommand cmd = new SqlCommand(query, conn);
                dt.Load(cmd.ExecuteReader());
            }

            DateTime dataIni = DateTime.MinValue;
            string dataFim = string.Empty;
            int difDias = 0;
            string mensagem = string.Empty;

            var maxRow = dt.Select("data = MIN(data)");
            int ano = ((System.DateTime)(maxRow[0].ItemArray[0])).Date.Year;

            //1. For min data to year + 1
            for (int x = ano; x <= DateTime.Now.Year; x++)
            {
                //2. for 1 to 12
                for (int i = 1; i <= 12; i++)
                {
                    dataIni = new DateTime(x, i, 01, 00, 00, 00);

                    switch (i)
                    {
                        case 1:
                        case 2:
                            //Até o dia 30 de abril do ano corrente.
                            CalculaDataFinal(ref difDias, ref dt, ref dataIni, ref dataFim, ref mensagem);

                            sb.AppendLine("INSERT INTO .PeriodoEnvioArquivo VALUES ('"
                                + x + i.ToString().PadLeft(2, '0')
                                + "','" + mensagem + "','"
                                + dataIni.ToString("yyyy-MM-dd") + "','" + dataFim + "',1)");

                            break;

                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            //Até 40 (quarenta) dias após término do mês.
                            difDias = 40;
                            CalculaDataFinal(ref difDias, ref dt, ref dataIni, ref dataFim, ref mensagem);

                            sb.AppendLine("INSERT INTO .PeriodoEnvioArquivo VALUES ('"
                                + x + i.ToString().PadLeft(2, '0')
                                + "','" + mensagem + "','"
                                + dataIni.ToString("yyyy-MM-dd") + "','" + dataFim + "',1)");
                            break;

                        case 12:
                            //Até o dia 30 de abril do ano subsequente.
                            CalculaDataFinal(ref difDias, ref dt, ref dataIni, ref dataFim, ref mensagem);

                            sb.AppendLine("INSERT INTO .PeriodoEnvioArquivo VALUES ('"
                                + x + i.ToString().PadLeft(2, '0')
                                + "','" + mensagem + "','"
                                + dataIni.ToString("yyyy-MM-dd") + "','" + dataFim + "',1)");
                            break;

                        default:
                            break;
                    }

                    dataIni = DateTime.MinValue;
                    dataFim = string.Empty;
                    difDias = 0;
                    mensagem = string.Empty;
                }
            }

            string filePath = "c:\\.PeriodoEnvioArquivo.txt";
            File.WriteAllText(filePath, sb.ToString().Substring(0, sb.Length));
            sb = null;
        }

        static void CalculaDataFinal(ref int difDias, ref DataTable dt, ref DateTime dataIni, ref string dataFim, ref string mensagem)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            string nomeMes;
            DateTime dtTemp = DateTime.MinValue;
            DateTime dt40 = DateTime.MinValue;
            int diferenca = 0;

            if (difDias == 0) // calcula data
            {
                if (dataIni.Month == 1 || dataIni.Month == 2)
                {
                    dtTemp = BuscaDataUtil(new DateTime(dataIni.Year, 4, 30), ref dt);
                    nomeMes = dtTemp.ToString("MMMM", culture);
                    mensagem = "Até o dia " + dtTemp.Day + " de " + nomeMes + " do ano corrente";
                    dataFim = dtTemp.ToString("yyyy-MM-dd");
                }
                else if (dataIni.Month == 12)
                {
                    int ano = dataIni.Year + 1;
                    dtTemp = BuscaDataUtil(new DateTime(ano, 4, 30), ref dt);
                    nomeMes = dtTemp.ToString("MMMM", culture);
                    mensagem = "Até o dia " + dtTemp.Day + " de " + nomeMes + " do ano subsequente";
                    dataFim = dtTemp.ToString("yyyy-MM-dd");
                }
            }
            else
            {
                //Até 40 (quarenta) dias após término do mês.
                dtTemp = dataIni.AddMonths(1);
                dtTemp = dtTemp.AddDays(-1);
                dt40 = dtTemp.AddDays(difDias);

                dt40 = BuscaDataUtil(dt40, ref dt);
                diferenca = Convert.ToInt32(dt40.Subtract(dtTemp).TotalDays);
                mensagem = "Até " + diferenca + " dias após término do mês.";
                dataFim = dt40.ToString("yyyy-MM-dd");
            }
        }

        static DateTime BuscaDataUtil(DateTime dtTemp, ref DataTable dt)
        {
            while (true)
            {
                System.Data.DataRow[] row = null;

                //verifica se não é feriado e nem fim de semana
                row = dt.Select("data = #" + dtTemp.ToString("yyyy-MM-dd") + "#");

                if (row.Count() == 0 && EhDiaUtil(dtTemp))
                {
                    break;
                }
                else
                {
                    //adiciona mais um dia para verificação
                    dtTemp = dtTemp.AddDays(1);
                }
            }

            return dtTemp;
        }

        static void GeraArquivos_INSERT_DE_PARA_X()
        {
            string path = @"C:\Users\fredsena\Documents\FRED_STUFF\de_para_x_insert.csv";

            StreamReader sr = new StreamReader(path, Encoding.GetEncoding("ISO-8859-1"));
            StringBuilder builderInsert = new StringBuilder();

            string nomeArquivo = Path.GetFileNameWithoutExtension(path);

            //Gera Pasta
            string pasta = @"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\ARQUIVOS_TESTE_DE-PARA\" + Path.GetFileNameWithoutExtension(nomeArquivo);

            try
            {
                if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

                string str = "";
                string[] linha = null;
                string dado = string.Empty;

                //loop
                while (str != null)
                {
                    str = sr.ReadLine();
                    if (str == null) break;
                    linha = str.Split(';');

                    if (linha[0] == "") continue;
                    dado = "INSERT INTO . VALUES ('" + linha[0].Trim() + "','" + linha[1].Trim() + "')";
                    builderInsert.AppendLine(dado);
                }
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Cria arquivo SCRIPT INSERT
                string filePath = pasta + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo) + ".txt";
                File.WriteAllText(filePath, builderInsert.ToString().Substring(0, builderInsert.Length));
                sr.Close();
                builderInsert = null;
            }
        }

        static void Conta2015Excluidas()
        {
            string path = @"C:\Users\fredsena\Documents\FRED_STUFF\.csv";

            StreamReader sr = new StreamReader(path, Encoding.GetEncoding("ISO-8859-1"));
            StringBuilder builderInsert = new StringBuilder();

            string nomeArquivo = Path.GetFileNameWithoutExtension(path);

            //Gera Pasta
            string pasta = @"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\IMPORTACAO\RESULT\" + Path.GetFileNameWithoutExtension(nomeArquivo);

            try
            {
                if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

                string str = "";
                string[] linha = null;
                string dado = string.Empty;

                //loop
                while (str != null)
                {
                    str = sr.ReadLine();
                    if (str == null) break;
                    linha = str.Split(';');

                    if (linha[0] == "") continue;
                    dado = "INSERT INTO [dbo].[Conta2015Excluidas] VALUES ('" + linha[0].Trim() + "')";
                    builderInsert.AppendLine(dado);
                }
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Cria arquivo SCRIPT INSERT
                string filePath = pasta + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo) + ".txt";
                File.WriteAllText(filePath, builderInsert.ToString().Substring(0, builderInsert.Length));
                sr.Close();
                builderInsert = null;
            }
        }

        static void Conta2015OHARA()
        {
            string path = @"C:\Users\fredsena\Documents\FRED_STUFF\Conta2015OHARA.csv";

            StreamReader sr = new StreamReader(path, Encoding.GetEncoding("ISO-8859-1"));
            StringBuilder builderInsert = new StringBuilder();

            string nomeArquivo = Path.GetFileNameWithoutExtension(path);

            //Gera Pasta
            string pasta = @"C:\Users\fredsena\Documents\FRED_STUFF\SISTEMAS \\IMPORTACAO\RESULT\" + Path.GetFileNameWithoutExtension(nomeArquivo);

            try
            {
                if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

                string str = "";
                string[] linha = null;
                string dado = string.Empty;

                //loop
                while (str != null)
                {
                    str = sr.ReadLine();
                    if (str == null) break;
                    linha = str.Split(';');

                    if (linha[0] == "") continue;
                    dado = "INSERT INTO [dbo].[] VALUES ('" + linha[0].Trim() + "')";
                    builderInsert.AppendLine(dado);
                }
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Cria arquivo SCRIPT INSERT
                string filePath = pasta + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo) + ".txt";
                File.WriteAllText(filePath, builderInsert.ToString().Substring(0, builderInsert.Length));
                sr.Close();
                builderInsert = null;
            }
        }

        static bool EhNumero(string valor)
        {
            decimal d;
            return (System.Text.RegularExpressions.Regex.IsMatch(valor, "^[0-9]*$")) || (decimal.TryParse(valor, out d));
        }

        static void BuscaContasValidas_Por_Competencia()
        {
            string query;

            using (var db = new Entidades())
            {
//                query = String.Format(@"SELECT * FROM .Conta WHERE 
//                    (('{0}' >= CAST(YEAR(datInicio) AS CHAR(4)) + CAST(MONTH(datInicio) AS CHAR (2)) AND datfinal is null) 
//                    OR '{0}' BETWEEN (CAST(YEAR(datInicio) AS CHAR(4)) + CAST(MONTH(datInicio) AS CHAR (2))) AND (CAST(YEAR(datFinal) AS CHAR(4)) + CAST(MONTH(datFinal) AS CHAR (2))))
//                    and anoplanoconta = {1} and idcultimograu = 'S'", "201411", "2010");

//                List<Conta> contas = db.ExecuteStoreQuery<Conta>(query).ToList();


//                int IdeBalancete =
//                        (from b in db.Balancete
//                         where b.AnmCompetencia.Equals("201401")
//                                && b.IdeAgente.Equals(7526)
//                         select b.IdeBalancete).Max();


                var result = (from b in db.Balancete
                              where b.AnmCompetencia == "201401"
                              && b.IdeAgente == 7526
                              select b).Take(2).OrderByDescending(o => o.DthEnvio);


                IList<Balancete> bala = result.ToList();

            }

            DataTable dtCompetenciaData = new DataTable();
            DataTable dtBalancete = new DataTable();

//            using (SqlConnection conn = new SqlConnection("Data Source=;Initial Catalog=;Persist Security Info=True;User ID=usr;Password="))
//            {
//                conn.Open();
//                query = @" SELECT TOP 1 anmcompetencia, MAX(dthenvio) as dthenvio from .balancete 
//                                where ideagente = 7526
//                                and anmcompetencia = 201401
//                                group by anmcompetencia";

//                SqlCommand cmd = new SqlCommand(query, conn);
//                dtCompetenciaData.Load(cmd.ExecuteReader());

//                if (dtCompetenciaData.Rows.Count == 0)
//                {

//                }

//                query = String.Format(@"SELECT top 1 * from .balancete where anmcompetencia = {0} 
//                        and dthenvio = '{1}' and ideagente = {2}",
//                        dtCompetenciaData.Rows[0]["anmcompetencia"],
//                        Convert.ToDateTime(dtCompetenciaData.Rows[0]["dthenvio"]).ToString("yyyy-MM-dd HH:mm:ss.FFF"),
//                        7526);

//                cmd = new SqlCommand(query, conn);
//                dtBalancete.Load(cmd.ExecuteReader());
//            }
       }

        static void Busca_Status_Primeiro_Envio_Competencia()
        {
            DataTable dtCompetenciaData = new DataTable();
            DataTable dtBalancete = new DataTable();

            using (SqlConnection conn = new SqlConnection("Data Source=;Initial Catalog=;Persist Security Info=True;User ID=usr;Password="))
            {
                conn.Open();
                string query = @" SELECT TOP 1 anmcompetencia, MIN(dthenvio) as dthenvio from .balancete 
                                where ideagente = 7526
                                and anmcompetencia = 201401
                                group by anmcompetencia";

                SqlCommand cmd = new SqlCommand(query, conn);
                dtCompetenciaData.Load(cmd.ExecuteReader());

                if (dtCompetenciaData.Rows.Count == 0)
                {

                }

                query = String.Format(@"SELECT top 1 * from .balancete where anmcompetencia = {0} 
                        and dthenvio = '{1}' and ideagente = {2}",
                        dtCompetenciaData.Rows[0]["anmcompetencia"],
                        Convert.ToDateTime(dtCompetenciaData.Rows[0]["dthenvio"]).ToString("yyyy-MM-dd HH:mm:ss.FFF"),
                        7526);

                cmd = new SqlCommand(query, conn);
                dtBalancete.Load(cmd.ExecuteReader());
            }
        }
    }

    public class TesteDados
    {
        public int IdeBalancete { get; set; }
        public int IdeAgente { get; set; }
        public string AnmCompetencia { get; set; }
    }

    /*
protected void sendMail(string path)
{
    string from = "fredsena.@..br";
    string to = "fredsena.@..br";

    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
    mail.To.Add(to);
    mail.From = new System.Net.Mail.MailAddress(from, "Sistema ", System.Text.Encoding.UTF8);
    mail.Subject = "arquivo compactado relatório sistema ";
    mail.SubjectEncoding = System.Text.Encoding.UTF8;
    mail.Body = "Segue arquivo compactado referente ao relatório solicitado pelo sistema .<br><br>";
    mail.BodyEncoding = System.Text.Encoding.UTF8;
    mail.IsBodyHtml = true;
    mail.Priority = System.Net.Mail.MailPriority.High;

    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
    //Add the Creddentials- use your own email id and password
    client.Credentials = new System.Net.NetworkCredential(from, "x");

    client.Port = 25;
    client.Host = "172.17.1.124";
    client.EnableSsl = true;

    try
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            byte[] contentAsBytes = Encoding.UTF8.GetBytes(path);
            memoryStream.Write(contentAsBytes, 0, contentAsBytes.Length);
            // Set the position to the beginning of the stream.
            memoryStream.Seek(0, SeekOrigin.Begin);
            mail.Attachments.Add(new System.Net.Mail.Attachment(memoryStream, Path.GetFileName(path), MediaTypeNames.Application.Zip));

            client.Send(mail);
        }
    }
    catch (Exception ex)
    {
        Exception ex2 = ex;
        string errorMessage = string.Empty;
        while (ex2 != null)
        {
            errorMessage += ex2.ToString();
            ex2 = ex2.InnerException;
        }
        HttpContext.Current.Response.Write(errorMessage);
    }
}
*/
}

