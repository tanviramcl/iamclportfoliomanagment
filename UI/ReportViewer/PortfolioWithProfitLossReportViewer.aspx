﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PortfolioWithProfitLossReportViewer.aspx.cs" Inherits="UI_ReportViewer_PortfolioWithProfitLossReportViewer" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Portfolio With Profit/Loss Report Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <CR:CrystalReportViewer ID="CRV_PfolioWithProfitLoss" runat="server" 
            AutoDataBind="true" 
             ToolbarImagesFolderUrl="\aspnet_client\system_web\2_0_50727\CrystalReportWebFormViewer4\images\toolbar\"  />
    </div>
    </form>
</body>
</html>
