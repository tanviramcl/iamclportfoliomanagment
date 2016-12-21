﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NAVvsMarketPriceReportViewer.aspx.cs" Inherits="UI_ReportViewer_NAVvsMarketPriceReportViewer" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NAV Vs Market Price Report</title>
</head>
<body>
    <form id="formNAVvsMarketPrice" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CRV_NAVvsMarketPrice" runat="server" 
            AutoDataBind="True" Height="50px" 
            Width="350px" />
    </div>
    </form>
</body>
</html>
