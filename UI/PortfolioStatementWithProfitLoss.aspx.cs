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

public partial class UI_PortfolioStatementWithProfitLoss : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    DropDownList dropDownListObj = new DropDownList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }
        DataTable dtHowlaDateDropDownList = dropDownListObj.HowlaDateDropDownList();
        DataTable dtFundNameDropDownList = dropDownListObj.FundNameDropDownList();
        if (!IsPostBack)
        {
            fundNameDropDownList.DataSource = dtFundNameDropDownList;
            fundNameDropDownList.DataTextField = "F_NAME";
            fundNameDropDownList.DataValueField = "F_CD";
            fundNameDropDownList.DataBind();

            portfolioAsOnDropDownList.DataSource = dtHowlaDateDropDownList;
            portfolioAsOnDropDownList.DataTextField = "Howla_Date";
            portfolioAsOnDropDownList.DataValueField = "VCH_DT";
            portfolioAsOnDropDownList.DataBind();
        }
    }
    protected void showButton_Click(object sender, EventArgs e)
    {
        string statementType = "";
        if (profitRadioButton.Checked)
        {
            statementType = "Profit";
        }
        else if (lossRadioButton.Checked)
        {
            statementType = "Loss";
        }
        else if (allRadioButton.Checked)
        {
            statementType = "All";
        }
        
        string orderType = "";
        if (investmentRadioButton.Checked)
        {
            orderType = "orderByInvest";
        }
        else if (appriciationRadioButton.Checked)
        {
            orderType = "orderByAppriciation";
        }

        else if (ErrosionRadioButton.Checked)
        {
            orderType = "orderByErosion";
        }
        else if (RateDiffAscRadioButton.Checked)
        {
            orderType = "orderByRateDiffASC";
        }
        else if (RateDiffDescRadioButton.Checked)
        {
            orderType = "orderByRateDiffDSC";
        }

        Session["statementType"] = statementType;
        Session["fundCode"] = fundNameDropDownList.SelectedValue.ToString();
        Session["balDate"] = portfolioAsOnDropDownList.SelectedValue.ToString();
        Session["orderType"] = orderType;

        ClientScript.RegisterStartupScript(this.GetType(), "PortfolioSummaryReportViewer", "window.open('ReportViewer/PortfolioWithProfitLossReportViewer.aspx')", true);
    }
}
