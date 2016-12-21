﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;

public partial class UI_ReportViewer_CompanyWiseSecuritiesTransactionReportViewer : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    Pf1s1DAO pf1Obj = new Pf1s1DAO();
    private ReportDocument rdoc = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sbFilter = new StringBuilder();
        string fromDate = "";
        string toDate = "";
        string transType = "";
        string fundCode = "";
        string companyCode = "";
        string bdf = "";
        DataTable dtIntimationReport = new DataTable();

        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../../Default.aspx");
        }
        else
        {
            fromDate = (string)Session["fromDate"];
            toDate = (string)Session["toDate"];
            transType = (string)Session["transType"];
            fundCode = (string)Session["fundCode"];
            companyCode = (string)Session["companyCode"];
            bdf = (string)Session["bdf"];
        }
        
        DataTable dtReprtSource = new DataTable();
        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbfilter = new StringBuilder();
        sbfilter.Append(" ");
        sbMst.Append("SELECT     INVEST.COMP.COMP_NM, INVEST.FUND.F_NAME, SUM(INVEST.FUND_TRANS_HB.NO_SHARE) AS NO_OF_SHARE, ");
        sbMst.Append("SUM(INVEST.FUND_TRANS_HB.CRT_AFT_COM * INVEST.FUND_TRANS_HB.NO_SHARE) AS COST_PRICE, ");
        sbMst.Append("ROUND(SUM(INVEST.FUND_TRANS_HB.CRT_AFT_COM * INVEST.FUND_TRANS_HB.NO_SHARE) / SUM(INVEST.FUND_TRANS_HB.NO_SHARE), 2) ");
        sbMst.Append("AS COST_RATE, SUM(INVEST.FUND_TRANS_HB.AMT_AFT_COM) AS TOTAL_AMT_AFT_COM, ROUND(SUM(INVEST.FUND_TRANS_HB.AMT_AFT_COM) ");
        sbMst.Append("/ SUM(INVEST.FUND_TRANS_HB.NO_SHARE), 2) AS TRANSACTION_RATE, ROUND(SUM(INVEST.FUND_TRANS_HB.AMT_AFT_COM) ");
        sbMst.Append("- SUM(INVEST.FUND_TRANS_HB.CRT_AFT_COM * INVEST.FUND_TRANS_HB.NO_SHARE), 2) AS PROFIT_LOSS, ");
        sbMst.Append("decode(INVEST.FUND_TRANS_HB.TRAN_TP, 'B', 'Report for Bonus Shares', 'C', 'Report for Purchase Shares', 'S', 'STATEMENT OF PROFIT ON SALE OF INVESTMENT', 'R', 'Report for Right Shares', 'I', 'Report for IPO Shares') AS TRAN_TYPE, ");
        sbMst.Append("INVEST.SECT_MAJ.SECT_MAJ_CD, INVEST.SECT_MAJ.SECT_MAJ_NM ");
        sbMst.Append("FROM         INVEST.FUND INNER JOIN ");
        sbMst.Append("INVEST.FUND_TRANS_HB ON INVEST.FUND.F_CD = INVEST.FUND_TRANS_HB.F_CD INNER JOIN ");
        sbMst.Append("INVEST.COMP ON INVEST.FUND_TRANS_HB.COMP_CD = INVEST.COMP.COMP_CD INNER JOIN ");
        sbMst.Append(" INVEST.SECT_MAJ ON INVEST.COMP.SECT_MAJ_CD = INVEST.SECT_MAJ.SECT_MAJ_CD ");
        sbMst.Append("WHERE     (INVEST.FUND_TRANS_HB.VCH_DT BETWEEN '" + fromDate.ToString() + "' AND '" + toDate.ToString() + "')  ");

        if (String.Compare(bdf.ToString(), "no", true) == 0)
        {
            sbMst.Append(" AND INVEST.FUND.F_cd <>17 ");
        }
        if (transType != "0")
        {
            sbMst.Append(" AND (INVEST.FUND_TRANS_HB.TRAN_TP ='" + transType.ToString() + "')");
        }
        if (fundCode != "0")
        {
            sbMst.Append(" AND (INVEST.FUND_TRANS_HB.F_CD =" + Convert.ToInt16(fundCode.ToString()) + ")");
        }
        if (companyCode != "0")
        {
            sbMst.Append(" AND (INVEST.FUND_TRANS_HB.COMP_CD =" + Convert.ToInt16(companyCode.ToString()) + ")");
        }
        sbMst.Append(" GROUP BY INVEST.COMP.COMP_NM, INVEST.FUND_TRANS_HB.TRAN_TP, INVEST.FUND.F_NAME, INVEST.FUND.F_CD, INVEST.SECT_MAJ.SECT_MAJ_CD, INVEST.SECT_MAJ.SECT_MAJ_NM ");
        sbMst.Append(" ORDER BY INVEST.SECT_MAJ.SECT_MAJ_NM, INVEST.COMP.COMP_NM, INVEST.FUND.F_CD ");
        sbMst.Append(sbfilter.ToString());
        dtReprtSource = commonGatewayObj.Select(sbMst.ToString());
        

        if (dtReprtSource.Rows.Count > 0)
        {
            dtReprtSource.TableName = "CompanyWiseFundTransactionReport";
            //dtReprtSource.WriteXmlSchema(@"F:\PortfolioManagementSystem\UI\ReportViewer\Report\crtCompanyWiseFundTransaction_sale.xsd");

            //ReportDocument rdoc = new ReportDocument();
            string Path = "";

           if (string.Compare(transType, "S", true) == 0)
            {
                if (string.Compare(fundCode, "0", true) != 0)
                {
                    Path = Server.MapPath("Report/crtCompanyWiseFundTransaction_sale_indvidual_fund.rpt");
                }
                else
                {
                    Path = Server.MapPath("Report/crtCompanyWiseFundTransaction_sale.rpt");
                }
            }
            else 
            {
                if (string.Compare(fundCode, "0", true) != 0)
                {
                    Path = Server.MapPath("Report/crtCompanyWiseFundTransaction_without_sale_individual_fund.rpt");
                }
                else
                {
                    Path = Server.MapPath("Report/crtCompanyWiseFundTransaction_without_sale.rpt");
                }
            }
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CRV_CompanyWiseTransaction.ReportSource = rdoc;
            rdoc.SetParameterValue("prmFromDate", fromDate);
            rdoc.SetParameterValue("prmToDate", toDate);
            CRV_CompanyWiseTransaction.DisplayToolbar = true;
            CRV_CompanyWiseTransaction.HasExportButton = true;
            CRV_CompanyWiseTransaction.HasPrintButton = true;
            rdoc = ReportFactory.GetReport(rdoc.GetType());
        }
        else
        {
            Response.Write("No Data Found");
        } 
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        CRV_CompanyWiseTransaction.Dispose();
        CRV_CompanyWiseTransaction = null;
        rdoc.Close();
        rdoc.Dispose();
        rdoc = null;
        GC.Collect();
    }
}
