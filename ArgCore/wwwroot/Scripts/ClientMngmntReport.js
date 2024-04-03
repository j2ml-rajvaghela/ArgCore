$("#ClientManagementReports #GetClientReportResults").click(function () {
    if ($("#ddlClients").val().length <= 0) {
        $("#lblClientReportMsg").html("Please select client!");
        return false;
    }
    if ($('#SearchOptions_ClientStartDate').val() == '') {
        $("#lblClientReportMsg").html("Please select start date!");
        return false;
    }
    if ($('#SearchOptions_ClientEndDate').val() == '') {
        $("#lblClientReportMsg").html("Please select end date!");
        return false;
    }
});
$("#GenerateClientReportPdf").click(function () {
    debugger;
    if ($('#SearchOptions_ClientStartDate').val() == '') {
        $("#lblClientReportMsg").html("Please select start date!");
        return false;
    }
    if ($('#SearchOptions_ClientEndDate').val() == '') {
        $("#lblClientReportMsg").html("Please select end date!");
        return false;
    }
    $("#lblClientReportMsg").html("Generating....");
    var companyId = $("#ddlClients").val();
    if (companyId.length <= 0)
        companyId = 0;
    var startDate = $("#SearchOptions_ClientStartDate").val();
    var endDate = $("#SearchOptions_ClientEndDate").val();
    var regions = $("#ddlRegions").val();
    var reporttypes = $("#ddlReportTypes").val();
    $.get(SiteRoot + "Clients/GenerateManagementReportPDF?companyId=" + companyId + "&startDate=" + startDate + "&endDate=" + endDate + "&regions=" + regions + "&ReportTypes=" + reporttypes + "&generatePdf=True", null, function (data) {
        var html = "<h4><a target='_blank' class='viewInvoice' href='" + data.InvoiceUrl + "'>View Report</a></h4>";
        $("#lblClientReportMsg").html(html);
    });
});
