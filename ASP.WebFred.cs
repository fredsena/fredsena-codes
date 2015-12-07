
//Encript connection strings

<%
'Find IIS application identity
Response.Write(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
%>
<br>
<% 
foreach (string var in Request.ServerVariables)
{
  Response.Write(var + " " + Request[var] + "<br>");
} 
%>


//1. Adding ACL for access to the RSA Key container...
C:\Windows\Microsoft.NET\Framework64\v4.0.30319>aspnet_regiis.exe -pa "NetFrameworkConfigurationKey" "IIS APPPOOL\sitename.com"

//2. Encrypting a Web Configuration Section
C:\Windows\Microsoft.NET\Framework64\v4.0.30319>aspnet_regiis.exe -pe "connectionStrings" -app "/" -site "2"

//3. Decrypting a Web Configuration Section
aspnet_regiis -pd "connectionStrings" -app "/SampleApplication"


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Net.Mail;

public partial class _Default : System.Web.UI.Page
{
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    GestaoFinanceira gestaoFin = new GestaoFinanceira();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MontaTodasCombos();
            BuscaPeriodoAtual();
            MontaGrid();
        }
        
        //ScriptManager.GetCurrent(this).RegisterPostBackControl(ImgBtn);
    }
    protected void MontaGrid()
    {
        DataTable dt = new DataTable();
        grdLancamentos.DataBind();
        ViewState.Remove("dtLancamentos");
        txtValorEntradas.Text = "";
        txtValorSaidas.Text = "";
        txtValorSaldoMes.Text = "";
        txtValorSaldoAcumulado.Text = "";
        txtPagamentoRestante.Text = "";

        txtValorEntradas.BackColor = System.Drawing.Color.White;
        txtValorSaidas.BackColor = System.Drawing.Color.White;
        txtValorSaldoMes.BackColor = System.Drawing.Color.White;
        txtValorSaldoAcumulado.BackColor = System.Drawing.Color.White;
        txtPagamentoRestante.BackColor = System.Drawing.Color.White;

        try
        {
            if (string.IsNullOrEmpty(txtDataMesIni.Text) || string.IsNullOrEmpty(txtDataMesFim.Text)) return;
            //cboMesReferencia.SelectedIndex = Convert.ToDateTime(txtDataMesFim.Text.ToString()).Month;
            dt = gestaoFin.BuscaRegistros(txtDataMesIni.Text, txtDataMesFim.Text);
        }
        catch (Exception ex)
        {
            //Mensagem(this.Page, LimpaMessage(ex.Message));

            Exception ex2 = ex;
            string errorMessage = string.Empty;
            while (ex2 != null)
            {
                errorMessage += ex2.ToString();
                ex2 = ex2.InnerException;
            }

            //HttpContext.Current.Response.Write(errorMessage);
            sendMail(errorMessage);
            return;
        }

        if (dt != null)
        {
            grdLancamentos.DataSource = dt;
            grdLancamentos.DataBind();
            ViewState.Add("dtLancamentos", dt);

            BuscaSaldoPeriodo();
            grdLancamentos.SelectedIndex = -1;
            LimparCampos();
        }
        else
        {
            //Mensagem(this.Page, "Não há registros para esta consulta");
        }
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }
    private void SortGridView(string sortExpression, string direction)
    {
        //  You can cache the DataTable for improving performance
        DataTable dt = (DataTable)ViewState["dtLancamentos"]; //GetData().Tables[0];

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        grdLancamentos.DataSource = dv;
        grdLancamentos.DataBind();
    }
    protected void grdLancamentos_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        }
    }
    protected void grdLancamentos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            ViewState.Remove("MovimentoId");

            int linha = int.Parse((string)e.CommandArgument);

            if (grdLancamentos.DataKeys[linha]["tipoLancto"].ToString() == "D")
            {
                rbDebito.Checked = true;
                rbCredito.Checked = false;
            }
            else
            {
                rbCredito.Checked = true;
                rbDebito.Checked = false;
            }

            //MovimentoId,formaPgto,Data,tipoLancto,Historico,Valor,Pago

            txtDataVencimento.Text = Convert.ToDateTime(grdLancamentos.DataKeys[linha]["Data"].ToString()).ToString("dd/MM/yyyy");
            txtValor.Text = string.Format("{0:C2}", Convert.ToDouble(grdLancamentos.DataKeys[linha]["Valor"].ToString())).Replace("R$", "").Trim();
            cboFormaPagamento.SelectedValue = grdLancamentos.DataKeys[linha]["formaPgto"].ToString().Trim();
            cboHistorico.SelectedValue = grdLancamentos.DataKeys[linha]["Historico"].ToString();
            txtParcela.Text = grdLancamentos.DataKeys[linha]["nrParcela"].ToString().Trim();
            txtObservacao.Text = grdLancamentos.DataKeys[linha]["Documento"].ToString().Trim();

            if (grdLancamentos.DataKeys[linha]["Pago"].ToString() == "S")
                chkPago.Checked = true;
            else
                chkPago.Checked = false;

            ViewState.Add("MovimentoId", grdLancamentos.DataKeys[linha]["MovimentoId"].ToString());

            ScriptManager.RegisterClientScriptBlock(this.Page, sender.GetType(), Guid.NewGuid().ToString(), "javascript:ScrollGrdLancamentos();", true);
        }
    }
    protected void grdLancamentos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            string data = DataBinder.Eval(e.Row.DataItem, "Data").ToString();
            data = string.Format("{0:dd/MM/yyyy}", data.Substring(0, 10));
            e.Row.Cells[0].Text = data;

            string tipo = DataBinder.Eval(e.Row.DataItem, "tipoLancto").ToString();
            string pago = DataBinder.Eval(e.Row.DataItem, "Pago").ToString();

            if (tipo == "D")
            {
                if (pago == "S")
                {
                    e.Row.Font.Bold = true;
                    e.Row.ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[1].BackColor = System.Drawing.Color.Red;
                }
                else
                {
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[1].Font.Bold = true;
                }
            }
            else
            {
                if (pago == "S")
                {
                    e.Row.Font.Bold = true;
                    e.Row.ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[1].BackColor = System.Drawing.Color.Blue;
                }
                else
                {
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[1].Font.Bold = true;
                }
            }

            string valor = DataBinder.Eval(e.Row.DataItem, "Valor").ToString();
            valor = string.Format("{0:C2}", Convert.ToDouble(valor)).Replace("R$ ", "");
            e.Row.Cells[4].Text = valor;

            GridRowStyle(e, grdLancamentos.ClientID, grdLancamentos.UniqueID);
        }
    }

    protected void BuscaPeriodoAtual()
    {
        try
        {
            //Verifica se o período do mês atual encontra-se antes ou depois do dia 20 (mês corrente)

            if (DateTime.Now.Day < 20)
            {
                if (DateTime.Now.Month == 1)
                {
                    txtDataMesIni.Text = "20/12" + "/" + (DateTime.Now.Year - 1).ToString();
                    txtDataMesFim.Text = "19/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    txtDataMesIni.Text = "20/" + (DateTime.Now.Month - 1).ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString();
                    txtDataMesFim.Text = "19/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString();
                }

                cboMesReferencia.SelectedIndex = DateTime.Now.Month;
                cboAno.SelectedValue = DateTime.Now.Year.ToString();
            }
            else
            {
                txtDataMesIni.Text = "20/" + (DateTime.Now.Month).ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString();

                if (DateTime.Now.Month == 12)
                {
                    txtDataMesFim.Text = "19/01" + "/" + (DateTime.Now.Year + 1).ToString();
                    cboMesReferencia.SelectedIndex = 1;
                    cboAno.SelectedValue = (DateTime.Now.Year + 1).ToString();
                }
                else
                {
                    txtDataMesFim.Text = "19/" + (DateTime.Now.Month + 1).ToString() + "/" + DateTime.Now.Year.ToString();
                    cboMesReferencia.SelectedIndex = DateTime.Now.Month + 1;
                    cboAno.SelectedValue = DateTime.Now.Year.ToString();
                }
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
            //HttpContext.Current.Response.Write(errorMessage);
            sendMail(errorMessage);
            throw ex;
        }
    }
    protected void BuscaSaldoPeriodo()
    {
        try
        {
            DataTable dt = new DataTable();

            txtValorEntradas.Text = "";
            txtValorSaidas.Text = "";
            txtValorSaldoMes.Text = "";
            txtValorSaldoAcumulado.Text = "";
            txtPagamentoRestante.Text = "";

            dt = gestaoFin.BuscaSaldo(txtDataMesIni.Text, txtDataMesFim.Text);

            if (dt != null)
            {
                double valorEntrada = 0;

                if (dt.Rows[0][1].ToString() == "C")
                {
                    if (dt.Rows[0][0].ToString() != "")
                        valorEntrada = Convert.ToDouble(dt.Rows[0][0].ToString());
                }
                else
                {
                    if (dt.Rows[1][0] != null)
                        valorEntrada = Convert.ToDouble(dt.Rows[1][0].ToString());
                }

                txtValorEntradas.Text = string.Format("{0:C2}", valorEntrada);

                txtValorEntradas.ForeColor = System.Drawing.Color.White;
                if (valorEntrada < 0)
                    txtValorEntradas.BackColor = System.Drawing.Color.Red;
                else
                    txtValorEntradas.BackColor = System.Drawing.Color.Blue;

                double valorSaida = 0;

                if (dt.Rows[1][1].ToString() == "D")
                    valorSaida = Convert.ToDouble(dt.Rows[1][0].ToString());
                else
                    valorSaida = Convert.ToDouble(dt.Rows[0][0].ToString());

                txtValorSaidas.Text = string.Format("{0:C2}", valorSaida);

                txtValorSaidas.ForeColor = System.Drawing.Color.White;
                if (valorSaida < 0)
                    txtValorSaidas.BackColor = System.Drawing.Color.Red;
                else
                    txtValorSaidas.BackColor = System.Drawing.Color.Blue;

                double valorSaldoMes = valorEntrada - valorSaida;
                txtValorSaldoMes.Text = string.Format("{0:C2}", valorSaldoMes);

                txtValorSaldoMes.ForeColor = System.Drawing.Color.White;
                if (valorSaldoMes < 0)
                    txtValorSaldoMes.BackColor = System.Drawing.Color.Red;
                else
                    txtValorSaldoMes.BackColor = System.Drawing.Color.Blue;
            }

            //dt = gestaoFin.BuscaSaldoAcumulado(txtDataMesFim.Text);
            dt = gestaoFin.BuscaSaldoPoupanca(txtDataMesFim.Text);

            if (dt != null)
            {
                double valorTotalEntrada = 0;

                //if (dt.Rows[0][1].ToString() == "C")
                //    valorTotalEntrada = Convert.ToDouble(dt.Rows[0][0].ToString());
                //else
                //    valorTotalEntrada = Convert.ToDouble(dt.Rows[1][0].ToString());

                //double valorTotalSaida = 0;

                //if (dt.Rows[1][1].ToString() == "D")
                //    valorTotalSaida = Convert.ToDouble(dt.Rows[1][0].ToString());
                //else
                //    valorTotalSaida = Convert.ToDouble(dt.Rows[0][0].ToString());

                //double valorTotalSaldoacumulado = valorTotalEntrada - valorTotalSaida;

                //txtValorSaldoAcumulado.Text = string.Format("{0:C2}", valorTotalSaldoacumulado);

                //txtValorSaldoAcumulado.ForeColor = System.Drawing.Color.White;

                //if (valorTotalSaldoacumulado < 0)
                //    txtValorSaldoAcumulado.BackColor = System.Drawing.Color.Red;
                //else
                //    txtValorSaldoAcumulado.BackColor = System.Drawing.Color.Blue;


                txtValorSaldoAcumulado.BackColor = System.Drawing.Color.Blue;
                txtValorSaldoAcumulado.ForeColor = System.Drawing.Color.White;

                if (dt.Rows[0][0].ToString() != "")
                    valorTotalEntrada = Convert.ToDouble(dt.Rows[0][0].ToString());

                txtValorSaldoAcumulado.Text = string.Format("{0:C2}", valorTotalEntrada);
            }

            dt = gestaoFin.BuscaSaldoApagar(txtDataMesIni.Text, txtDataMesFim.Text);

            if (dt != null)
            {
                double valorEntrada = 0;
                if (dt.Rows[0][0].ToString() != "")
                    valorEntrada = Convert.ToDouble(dt.Rows[0][0].ToString());

                txtPagamentoRestante.Text = string.Format("{0:C2}", valorEntrada);
                txtPagamentoRestante.BackColor = System.Drawing.Color.Yellow;
                txtPagamentoRestante.ForeColor = System.Drawing.Color.Black;
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
            //HttpContext.Current.Response.Write(errorMessage);
            sendMail(errorMessage);
            throw ex;
        }
    }

    protected void btnAtualizar_Click(object sender, EventArgs e)
    {
        if (txtDataMesIni.Text.Trim().Equals(""))
        {
            Mensagem(this.Page, "Informe a Data inicial");
            ScriptManager.GetCurrent(this).SetFocus(txtDataMesIni);
            return;
        }

        if (txtDataMesFim.Text.Trim().Equals(""))
        {
            Mensagem(this.Page, "Informe a Data final");
            ScriptManager.GetCurrent(this).SetFocus(txtDataMesFim);
            return;
        }

        if (!ValidaData(txtDataMesIni.Text))
        {
            Mensagem(this.Page, "Data inicial Inválida");
            ScriptManager.GetCurrent(this).SetFocus(txtDataMesIni);
            return;
        }

        if (!ValidaData(txtDataMesFim.Text))
        {
            Mensagem(this.Page, "Data final Inválida");
            ScriptManager.GetCurrent(this).SetFocus(txtDataMesFim);
            return;
        }

        LimparCampos();
        MontaGrid();
    }
    protected void btnNovo_Click(object sender, EventArgs e)
    {
        LimparCampos();
        ViewState.Remove("MovimentoId");
        grdLancamentos.SelectedIndex = -1;
        ScriptManager.GetCurrent(this).SetFocus(txtDataVencimento);
    }
    protected void btnExcluir_Click(object sender, EventArgs e)
    {
        if (ViewState["MovimentoId"] == null)
        {
            Mensagem(this.Page, "Selecione um registro para excluir");
            return;
        }

        try
        {
            gestaoFin.ExcluiRegistro(ViewState["MovimentoId"].ToString());
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

            //HttpContext.Current.Response.Write(errorMessage);
            sendMail(errorMessage);
            throw ex;
        }

        MontaGrid();
    }
    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        try
        {
            if (!ValidaCampos()) return;

            gestaoFin.ContaId = 1;
            gestaoFin.Data = Convert.ToDateTime(txtDataVencimento.Text);
            gestaoFin.Mes = gestaoFin.Data.Month.ToString();
            gestaoFin.Ano = gestaoFin.Data.Year.ToString();
            gestaoFin.Nv = 0;

            if (rbCredito.Checked)
                gestaoFin.TipoLancto = "C";
            else
                gestaoFin.TipoLancto = "D";

            gestaoFin.Valor = Convert.ToDouble(txtValor.Text);

            if (chkPago.Checked)
                gestaoFin.Pago = "S";
            else
                gestaoFin.Pago = "N";

            gestaoFin.Obs = txtObservacao.Text.Trim();
            gestaoFin.Historico = cboHistorico.SelectedValue;
            gestaoFin.FormaPgto = cboFormaPagamento.SelectedValue;

            if (string.IsNullOrEmpty(txtParcela.Text)) txtParcela.Text = "0";

            gestaoFin.NrParcela = Convert.ToInt16(txtParcela.Text);
            gestaoFin.DBAuto = "N";
            gestaoFin.DFiscalIR = "N";

            if (ViewState["MovimentoId"] == null)
            {
                //if (!ValidaParcelas(gestaoFin)) return;
                gestaoFin.InsereRegistro(gestaoFin);
            }
            else
            {
                gestaoFin.MovimentoId = Convert.ToInt32(ViewState["MovimentoId"].ToString());
                gestaoFin.AtualizaRegistro(gestaoFin);
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
            ViewState.Remove("MovimentoId");
            sendMail(errorMessage);
            
            //Mensagem(this.Page, errorMessage);
            //HttpContext.Current.Response.Write(errorMessage);
            //return;
            throw ex;
        }

        ViewState.Remove("MovimentoId");

        MontaGrid();
    }
    protected void btnRelatorioPoupancas_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt = gestaoFin.BuscaSaldoPoupancas();

        if (dt.Rows.Count > 0)
            GeraRelatorioPoupancas(dt);
    }
    protected void btnConfirmarPagamentos_Click(object sender, EventArgs e)
    {
        if (txtDataVencimento.Text.Trim().Equals(""))
        {
            Mensagem(this.Page, "Informe uma Data para a confirmar os pagamentos");
            ScriptManager.GetCurrent(this).SetFocus(txtDataVencimento);
            return;
        }

        if (!ValidaData(txtDataVencimento.Text))
        {
            Mensagem(this.Page, "Data Inválida");
            ScriptManager.GetCurrent(this).SetFocus(txtDataVencimento);
            return;
        }

        try
        {
            gestaoFin.ConfirmaPagamentos(txtDataVencimento.Text);
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
            //HttpContext.Current.Response.Write(errorMessage);
            sendMail(errorMessage);
            throw ex;
        }

        MontaGrid();
    }

    [System.Web.Services.WebMethod]
    public static System.Collections.ArrayList MontaComboJquery(string tipo)
    {
        DataTable dt = new DataTable();
        ListItem item = new ListItem("Selecione...", "");

        System.Collections.ArrayList lista = new System.Collections.ArrayList();

        switch (tipo.ToUpper())
        {
            #region FORMAPAGAMENTO
            case "FORMAPAGTO":
                {
                    FormaPagamento forma = new FormaPagamento();
                    dt = forma.BuscaRegistros();

                    //return new System.Collections.ArrayList() {
                    //        new { Value = "1", Display = "Joe" },
                    //        new { Value = "2", Display = "Tom" },
                    //        new { Value = "3", Display = "Sylvain" }
                    //};

                    break;
                }

            #endregion

            #region HISTORICO
            case "HISTORICO":
                {
                    Historico historico = new Historico();
                    dt = historico.BuscaRegistros();
                    break;
                }

            #endregion
        }

        foreach (DataRow row in dt.Rows)
        {
            lista.Add(new { Value = row[1].ToString(), Display = row[1].ToString() });
        }

        //lista.Add(new { Value = "FRED", Display = "FRED" });

        return lista;
    }
    protected void GeraRelatorioPoupancas(DataTable dtResultado)
    {
        StringBuilder _tabela = new StringBuilder();

        #region TITULO ###############################################################################

        _tabela.Append(@"<table style='WIDTH: 90%; FONT-FAMILY: Verdana;' border='0'>");
        _tabela.Append("<tr>");
        _tabela.Append("<td align='center' style='FONT-SIZE: 14pt; font-weight:bold; FONT-FAMILY: Verdana;'>Relatório de Poupanças</td>");
        _tabela.Append("</tr>");
        _tabela.Append("<tr>");
        _tabela.Append("<td style='FONT-SIZE: 9pt;' align='right'>Data impressão:" + DateTime.Now.ToString() + "</td>");
        _tabela.Append("</tr>");

        _tabela.Append("<tr>");
        //_tabela.Append("<tr style='height: 12.75pt'>");
        //_tabela.Append("</tr>");

        _tabela.Append(@"</table>");
        //_tabela.Append("<br/>");

        #endregion

        #region HEADER ###############################################################################

        _tabela.Append(@"<table style='FONT-SIZE: 8pt; WIDTH: 90%; FONT-FAMILY: Verdana;' border='1' align='center'>");
        _tabela.Append("<tr>");

        //Data          Valor   Histórico           OBS
        //20/12/2010 	1208	POUPANÇA	        ? ? ? ? ?
        //17/02/2011 	15,2	POUPANÇA	        NULL
        //16/02/2011 	15,2	POUPANÇA BANCO X	teste

        _tabela.Append("<td style='font-weight:bold;' bgColor='#E8E8E8'>Histórico</td>");
        _tabela.Append("<td style='font-weight:bold;' bgColor='#E8E8E8'>Data</td>");
        _tabela.Append("<td style='font-weight:bold;' bgColor='#E8E8E8'>OBS</td>");
        _tabela.Append("<td style='font-weight:bold;' bgColor='#E8E8E8'>Valor</td>");
        _tabela.Append("</tr>");

        //_tabela.Append("<tr style='height: 12.75pt'>");
        //_tabela.Append("</tr>");
        //_tabela.Append(@"</table>");

        #endregion

        #region DADOS ################################################################################

        //_tabela.Append(@"<table style='FONT-SIZE: 8pt; WIDTH: 90%; FONT-FAMILY: Verdana;' border='1' align='center'>");			

        string historico = dtResultado.Rows[0][2].ToString();
        double soma = 0;
        string temp = "";

        for (int i = 0; i <= (dtResultado.Rows.Count - 1); i++)
        {
            if (historico == dtResultado.Rows[i][2].ToString())
            {
                _tabela.Append("<tr>");
                _tabela.Append("<td>" + dtResultado.Rows[i][2].ToString() + "</td>");
                _tabela.Append("<td>" + dtResultado.Rows[i][0].ToString() + "</td>");
                _tabela.Append("<td>" + dtResultado.Rows[i][3].ToString() + "</td>");
                _tabela.Append("<td align='right'>" + String.Format("{0:C2}", Convert.ToDouble(dtResultado.Rows[i][1].ToString())) + "</td>");
                _tabela.Append("</tr>");

                soma = soma + Convert.ToDouble(dtResultado.Rows[i][1].ToString());

                if (i == (dtResultado.Rows.Count - 1))
                {
                    _tabela.Append("<tr>");
                    _tabela.Append("<td> &nbsp; </td>");
                    _tabela.Append("<td> &nbsp; </td>");
                    _tabela.Append("<td> &nbsp; </td>");
                    _tabela.Append("<td align='right'> TOTAL " + historico + " : " + String.Format("{0:C2}", soma) + "</td>");
                    _tabela.Append("</tr>");
                }
            }
            else
            {
                temp = historico;
                historico = dtResultado.Rows[i][2].ToString();

                _tabela.Append("<tr>");
                _tabela.Append("<td> &nbsp; </td>");
                _tabela.Append("<td> &nbsp; </td>");
                _tabela.Append("<td> &nbsp; </td>");
                _tabela.Append("<td align='right'> TOTAL " + temp + " : " + String.Format("{0:C2}", soma) + "</td>");
                _tabela.Append("</tr>");

                soma = 0;

                _tabela.Append("<tr>");
                _tabela.Append("<td>" + dtResultado.Rows[i][2].ToString() + "</td>");
                _tabela.Append("<td>" + dtResultado.Rows[i][0].ToString() + "</td>");
                _tabela.Append("<td>" + dtResultado.Rows[i][3].ToString() + "</td>");
                _tabela.Append("<td align='right'>" + String.Format("{0:C2}", Convert.ToDouble(dtResultado.Rows[i][1].ToString())) + "</td>");
                _tabela.Append("</tr>");

                soma = soma + Convert.ToDouble(dtResultado.Rows[i][1].ToString());
            }
        }

        _tabela.Append(@"</table>");

        #endregion

        Session.Add("strHtml", _tabela.ToString());

        string scriptjs = "var leftjs = (window.screen.width - 800)/2; " +
            "var topjs = ((window.screen.height - 600)/2)+ 60; " +
            "window.open('GeraPDF.aspx', 'Relatório', 'left='+leftjs+',top='+topjs+',width=640,height=480,resizable=1,directories=0,scrollbars=0,status=0');";

        ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(), scriptjs, true);
        
        //string script = "window.open('../Mostra.aspx', '', 'height=650px,width=990px,menubar=yes,resizable=yes,scrollbars=yes');";
        //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "page1", "<script>" + script + "</script>");
        
        
        
        
    }

    protected void sendMail(string erro)
    {
        string from = "mim@gmail.com"; //Replace this with your own correct Gmail Address
        string to = "mim@gmail.com"; //Replace this with the Email Address to whom you want to send the mail

        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        mail.To.Add(to);
        mail.From = new MailAddress(from, "system", System.Text.Encoding.UTF8);
        mail.Subject = "Erro: system";
        mail.SubjectEncoding = System.Text.Encoding.UTF8;
        mail.Body = erro;
        mail.BodyEncoding = System.Text.Encoding.UTF8;
        mail.IsBodyHtml = true;
        mail.Priority = MailPriority.High;

        SmtpClient client = new SmtpClient();
        //Add the Creddentials- use your own email id and password

        client.Credentials = new System.Net.NetworkCredential(from, "XXX");

        client.Port = 587; // Gmail works on this port
        client.Host = "smtp.gmail.com";
        client.EnableSsl = true; //Gmail works on Server Secured Layer
        try
        {
            client.Send(mail);
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

    #region COMBOS
    protected void MontaTodasCombos()
    {
        MontaCombos("ANO");
        MontaCombos("MES");
        MontaCombos("HISTORICO");
        MontaCombos("FORMAPAGAMENTO");
    }
    protected void MontaCombos(string tipo)
    {
        DataTable dt = new DataTable();
        ListItem item = new ListItem("Selecione...", "");
        ListItem itemTodos = new ListItem("Todos...", "");

        switch (tipo.ToUpper())
        {
            #region ANO
            case "ANO":
                {
                    if (cboAno.Items.Count == 0)
                    {
                        cboAno.Items.Clear();

                        for (int i = 2010; i <= 2100; i++)
                        {
                            ListItem temp = new ListItem(i.ToString(), i.ToString());
                            cboAno.Items.Add(temp);
                        }
                        cboAno.Items.Insert(0, itemTodos);
                    }
                    break;
                }
            #endregion

            #region MES
            case "MES":
                {
                    if (cboMesReferencia.Items.Count == 0)
                    {
                        cboMesReferencia.Items.Clear();
                        cboMesReferencia.Items.Insert(0, itemTodos);
                        cboMesReferencia.Items.Insert(1, "Janeiro");
                        cboMesReferencia.Items.Insert(2, "Fevereiro");
                        cboMesReferencia.Items.Insert(3, "Março");
                        cboMesReferencia.Items.Insert(4, "Abril");
                        cboMesReferencia.Items.Insert(5, "Maio");
                        cboMesReferencia.Items.Insert(6, "Junho");
                        cboMesReferencia.Items.Insert(7, "Julho");
                        cboMesReferencia.Items.Insert(8, "Agosto");
                        cboMesReferencia.Items.Insert(9, "Setembro");
                        cboMesReferencia.Items.Insert(10, "Outubro");
                        cboMesReferencia.Items.Insert(11, "Novembro");
                        cboMesReferencia.Items.Insert(12, "Dezembro");
                    }
                    break;
                }
            #endregion

            #region HISTORICO
            case "HISTORICO":
                {
                    cboHistorico.Items.Clear();
                    Historico historico = new Historico();
                    dt = historico.BuscaRegistros();

                    if (dt.Rows.Count > 0)
                    {
                        cboHistorico.DataTextField = "descricao";
                        cboHistorico.DataValueField = "descricao";
                        cboHistorico.DataSource = dt;
                        cboHistorico.DataBind();
                    }

                    cboHistorico.Items.Insert(0, item);

                    //if (Retaguarda.Web.Code.Util.Sessao.CrpId != 0)
                    //{
                    //    cmbCRP.SelectedValue = Retaguarda.Web.Code.Util.Sessao.CrpId.ToString();
                    //    cmbCRP.Enabled = false;
                    //}
                    //else
                    //{
                    //    if (ViewState["ator"].ToString() == "MFI")
                    //        BuscaCrpAgencia(false);

                    //    cmbCRP.Enabled = true;
                    //    cmbCRP.Items.Insert(0, item);
                    //}

                    break;
                }
            #endregion

            #region FORMAPAGAMENTO
            case "FORMAPAGAMENTO":
                {
                    cboFormaPagamento.Items.Clear();
                    FormaPagamento formaPagto = new FormaPagamento();
                    dt = formaPagto.BuscaRegistros();

                    if (dt.Rows.Count > 0)
                    {
                        cboFormaPagamento.DataTextField = "descricao";
                        cboFormaPagamento.DataValueField = "descricao";
                        cboFormaPagamento.DataSource = dt;
                        cboFormaPagamento.DataBind();
                    }

                    cboFormaPagamento.Items.Insert(0, item);
                    break;
                }
            #endregion
        }
    }
    protected void cboMesReferencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        AtualizaConSulta();
    }
    protected void cboAno_SelectedIndexChanged(object sender, EventArgs e)
    {
        AtualizaConSulta();
    }

    protected void AtualizaConSulta()
    {
        try
        {


            if (string.IsNullOrEmpty(txtDataMesIni.Text) || string.IsNullOrEmpty(txtDataMesFim.Text)) return;
            if (cboMesReferencia.SelectedIndex == 0) return;
            if (cboAno.SelectedIndex == 0) return;

            if (cboMesReferencia.SelectedIndex == 1)//Janeiro
            {
                txtDataMesIni.Text = "20/12/" + (Convert.ToInt16(cboAno.SelectedValue) - 1).ToString();
                txtDataMesFim.Text = "19/" + cboMesReferencia.SelectedIndex.ToString().PadLeft(2, '0') + "/" + cboAno.SelectedValue;
            }
            else
            {
                txtDataMesIni.Text = "20/" + (cboMesReferencia.SelectedIndex - 1).ToString().PadLeft(2, '0') + "/" + cboAno.SelectedValue;
                txtDataMesFim.Text = "19/" + cboMesReferencia.SelectedIndex.ToString().PadLeft(2, '0') + "/" + cboAno.SelectedValue;
            }

            MontaGrid();

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
            sendMail(errorMessage);
        }
    }

    #endregion

    #region UTIL
    protected bool ValidaCampos()
    {
        if (rbCredito.Checked == false && rbDebito.Checked == false)
        {
            Mensagem(this.Page, "Selecione o tipo de lançamento (Crédito/Débito)");
            return false;
        }

        if (txtDataVencimento.Text.Trim().Equals(""))
        {
            Mensagem(this.Page, "Informe a Data de Vencimento");
            ScriptManager.GetCurrent(this).SetFocus(txtDataVencimento);
            return false;
        }

        if (!ValidaData(txtDataVencimento.Text))
        {
            Mensagem(this.Page, "Data Inválida");
            ScriptManager.GetCurrent(this).SetFocus(txtDataVencimento);
            return false;
        }

        if (txtValor.Text.Trim().Equals(""))
        {
            Mensagem(this.Page, "Informe o Valor");
            ScriptManager.GetCurrent(this).SetFocus(txtValor);
            return false;
        }

        if (cboFormaPagamento.SelectedIndex == 0)
        {
            Mensagem(this.Page, "Selecione uma Forma de Pagamento");
            ScriptManager.GetCurrent(this).SetFocus(cboFormaPagamento);
            return false;
        }

        if (cboHistorico.SelectedIndex == 0)
        {
            Mensagem(this.Page, "Selecione o Histórico");
            ScriptManager.GetCurrent(this).SetFocus(cboHistorico);
            return false;
        }

        return true;
    }
    public static void Mensagem(Page pagina, string mensagem)
    {
        ScriptManager.RegisterClientScriptBlock(pagina, pagina.GetType(), Guid.NewGuid().ToString(), "alert('" + mensagem + "')", true);
    }
    protected void OpenPopUp(string pagina, string titulo, Page sender)
    {
        //"window.showModalDialog('" + pagina + "', '_blank','Resizable:no; DialogHeight: 530px; DialogWidth:770px; Edge:raised; Help:no; Scroll:no; Status:no; Center:yes;')";

        //string scriptjs = "var leftjs = (window.screen.availWidth - 800)/2; " +
        //                  "var topjs = ((window.screen.availHeight - 600)/2)+ 0; " +
        //    "window.open('" + pagina + "', '_blank', 'left=' + leftjs + ',top=' + topjs + ',width=770px,height=530px,resizable=0,directories=0,scrollbars=1,status');";

        string scriptjs = "popupfilho=dhtmlmodal.open('googlebox', 'iframe', '" + pagina + "', '" + titulo + "', 'width=450px,height=410px,resize=0,scrolling=0,center=1','recal')";
        ScriptManager.RegisterClientScriptBlock(sender, sender.GetType(), Guid.NewGuid().ToString(), scriptjs, true);

    }
    protected string LimpaMessage(string mensagem)
    {
        return mensagem = mensagem.Replace("\r", "").Replace("\n", "").Replace("\'", "").Replace("\"", "").Replace("\\", "").Replace("\0", "").Replace("\a", "").Replace("\b", "").Replace("\f", "").Replace("\t", "").Replace("\v", "");
    }
    public static void GridRowStyle(GridViewRowEventArgs e, string clientID, string uniqueID)
    {
        e.Row.Attributes.Add("onMouseOver", "GridMouseOver(this, '" + clientID + "');");
        e.Row.Attributes.Add("onMouseOut", "GridMouseOut(this, '" + clientID + "');");
        //e.Row.Attributes.Add("onClick", "GridOnClick('" + clientID + "',\"" + "javascript:__doPostBack('" + uniqueID + "','Select$" + e.Row.RowIndex.ToString() + "');" + "\");");
    }
    protected void LimparCampos()
    {
        rbCredito.Checked = false;
        rbDebito.Checked = false;
        txtDataVencimento.Text = "";
        txtValor.Text = "";
        cboFormaPagamento.SelectedIndex = 0;
        cboHistorico.SelectedIndex = 0;
        chkPago.Checked = false;
        txtParcela.Text = "";
        txtObservacao.Text = "";
        ViewState.Remove("MovimentoId");
    }
    public static bool ValidaData(string DataValida)
    {
        //01022006
        //01234567

        string data = SomenteNumeros(DataValida);
        //if (data.Length < 10) return false;
        int iDia = Convert.ToInt32(data.Substring(0, 2));
        int iMes = Convert.ToInt32(data.Substring(2, 2));
        int iAno = Convert.ToInt32(data.Substring(4, 4));

        if (iAno.ToString().Length < 4) return false;
        if ((iAno - 1980 < 0) && (iAno - 2080 > 0)) return false;
        if ((iMes - 12) > 0) return false;
        if (iMes == 2) return (((iDia - 1 >= 0) && (iDia - 28 <= 0)) || ((iDia == 29) && (Convert.ToDouble(iAno) % 4 == 0)));
        if ((iMes == 1) || (iMes == 3) || (iMes == 5) || (iMes == 7) || (iMes == 8) || (iMes == 10) || (iMes == 2)) return ((iDia - 1 >= 0) && (iDia - 31 <= 0));
        if ((iMes == 4) || (iMes == 6) || (iMes == 9) || (iMes == 11)) return ((iDia - 1 >= 0) && (iDia - 30 <= 0));

        return true;
    }
    protected bool ValidaParcelas(GestaoFinanceira gFin)
    {
        if (gestaoFin.ExisteRegistro(gFin.Data.ToString("dd/MM/yyyy"), gFin.Valor, gFin.FormaPgto.Trim(), gFin.Historico.Trim()))
        {
            Mensagem(this.Page, "Já existe um lançamento para a parcela abaixo: Data Vencimento: "
                + gFin.Data.ToString("dd/MM/yyyy") + " Valor: "
                + gFin.Valor + " Forma de Pagamento: "
                + gFin.FormaPgto.Trim() + " Histórico: "
                + gFin.Historico.Trim());
            return false;
        }
        return true;
    }
    public static string SomenteNumeros(string sTexto)
    {
        string NovaString = String.Empty;

        foreach (char caracter in sTexto.ToCharArray())
        {
            if (char.IsDigit(caracter))
            {
                NovaString += caracter;
            }
        }

        return sTexto = NovaString;
    }
    #endregion

}
