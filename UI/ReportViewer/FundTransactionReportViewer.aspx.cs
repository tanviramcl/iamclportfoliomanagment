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

public partial class UI_ReportViewer_FundTransactionReportViewer : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    private ReportDocument rdoc = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../../Default.aspx");
        }
        int comCode = Convert.ToInt32(Request.QueryString["companyName"]);
        int fundCode = Convert.ToInt32(Request.QueryString["fundName"]);
        string transType = Convert.ToString(Request.QueryString["transType"]).Trim();

        DataTable dtReprtSource = new DataTable();
        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbfilter = new StringBuilder();
        sbfilter.Append(" ");
        sbMst.Append("SELECT     INVEST.FUND_TRANS_HB.COMP_CD, INVEST.COMP.COMP_NM, INVEST.FUND_TRANS_HB.VCH_DT, INVEST.FUND_TRANS_HB.F_CD, ");
        sbMst.Append(" INVEST.FUND.F_NAME, INVEST.FUND_TRANS_HB.TRAN_TP, INVEST.FUND_TRANS_HB.NO_SHARE, INVEST.FUND_TRANS_HB.RATE,  INVEST.FUND_TRANS_HB.AMT_AFT_COM ");
        sbMst.Append("FROM         INVEST.COMP INNER JOIN ");
        sbMst.Append("INVEST.FUND_TRANS_HB ON INVEST.COMP.COMP_CD = INVEST.FUND_TRANS_HB.COMP_CD INNER JOIN INVEST.FUND ON INVEST.FUND_TRANS_HB.F_CD = INVEST.FUND.F_CD ");
        sbMst.Append("WHERE     (INVEST.FUND_TRANS_HB.VCH_DT BETWEEN '" + Convert.ToDateTime(Request.QueryString["howlaDateFrom"]).ToString("dd-MMM-yyyy") + "' AND '" + Convert.ToDateTime(Request.QueryString["howlaDateTo"]).ToString("dd-MMM-yyyy") + "')  ");
        if (transType !="0")
        {
            sbMst.Append(" AND (INVEST.FUND_TRANS_HB.TRAN_TP ='"+transType+"')");
        }
        if (comCode != 0)
        {
            sbMst.Append(" AND (INVEST.FUND_TRANS_HB.COMP_CD =" + comCode + ")");
        }
        if (fundCode != 0)
        {
            sbMst.Append(" AND (INVEST.FUND_TRANS_HB.F_CD =" + fundCode + ")");
        }
        
       // sbMst.Append(" AND (INVEST.FUND_TRANS_HB.COMP_CD in (172,169,167,182,179,173,186,175)) ");
        //sbMst.Append(" AND (INVEST.FUND_TRANS_HB.F_CD = 17) ");
        
        sbMst.Append(" ORDER BY INVEST.COMP.COMP_NM, INVEST.FUND_TRANS_HB.F_CD ");

        sbMst.Append(sbfilter.ToString());
        dtReprtSource = commonGatewayObj.Select(sbMst.ToString());
        dtReprtSource.TableName = "FundTransactionReport";

        string transTypeDetais = "";
        if (transType == "B")
        {
            transTypeDetais = "Bonus";
        }
        else if (transType == "C")
        {
            transTypeDetais = "Purchase";
        }
        else if (transType == "S")
        {
            transTypeDetais = "Sell";
        }
        else if (transType == "R")
        {
            transTypeDetais = "Right";
        }
        else if (transType == "I")
        {
            transTypeDetais = "IPO";
        }

        if (dtReprtSource.Rows.Count > 0)
        {
            //dtReprtSource.WriteXmlSchema(@"F:\PortfolioManagementSystem\UI\ReportViewer\Report\crtmFundTransactionReport.xsd");

            //ReportDocument rdoc = new ReportDocument();
            string Path = Server.MapPath("Report/FundTransactionReport.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CRV_FundTransaction.ReportSource = rdoc;
            CRV_FundTransaction.DisplayToolbar = true;
            CRV_FundTransaction.HasExportButton = true;
            CRV_FundTransaction.HasPrintButton = true;
            rdoc.SetParameterValue("prmtransTypeDetais", transTypeDetais);
            rdoc = ReportFactory.GetReport(rdoc.GetType());
            //rdoc.SetParameterValue("prmLetterPrintDate", letterPrintDateCr);
            //rdoc.SetParameterValue("prmNameOfPerson", nameOfPerson);
        }
        else
        {
            Response.Write("No Data Found");
        }
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        CRV_FundTransaction.Dispose();
        CRV_FundTransaction = null;
        rdoc.Close();
        rdoc.Dispose();
        rdoc = null;
        GC.Collect();
    }
}
