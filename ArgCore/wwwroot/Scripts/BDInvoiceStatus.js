var oldStatus = $("#BalanceDueInfo_InvoiceStatus").val();
var balId = $("#BalanceDueInfo_BalanceId").val();
$("#BalanceDueInfo_InvoiceStatus").change(function () {
    var newStatus = $("#BalanceDueInfo_InvoiceStatus").val();
    $.post(SiteRoot + "BalanceDues/UpdateBDInvoiceStatus?balanceId=" + balId + "&oldStatus=" + oldStatus + "&newStatus=" + newStatus, null, function (data) {
        console.log(data);
        var htmlText = "";
        htmlText = data.Message + "<br />";
        if (data.MissingInvoiceBOL != null) {
            htmlText += "Invoice# assigned by Pasha is missing for BOL: " + data.MissingInvoiceBOL;
        }
        $("#lblInvoiceStatusMsg").html(htmlText);
        if (data.ChangesDone != null) {
        }
        //$(".ChangesDoneMsg").html(data.ChangesDone);
        if (data.ShowCloseReasonCode == true) {
            var html = '<div class="form-group"><label for="inputUrl" class="mylabel control-label col-md-2">Close Reason Code</label><div class="col-md-5">';
            html += '<select class="form-control" data-val="true" data-val-required="CloseReasonCode field is required" id="BalanceDueInfo_CloseReasonCode" name="BalanceDueInfo.CloseReasonCode"></select>';
            $(".UpdateInvoiceStatus").append(html);
            $.each(data.SelectedCloseReasonCodes, function (i, item) {
                if (item.Selected == true) {
                    $("#BalanceDueInfo_CloseReasonCode").append('<option value="' + item.Value + '" selected="' + item.Selected + '">' + item.Text + '</option>');
                }
                else {
                    $("#BalanceDueInfo_CloseReasonCode").append('<option value="' + item.Value + '">' + item.Text + '</option>');
                }
            });
        }
    });
});
$("body").on("change", "#BalanceDueInfo_CloseReasonCode", function () {
    console.log("Changed...");
    if ($("#BalanceDueInfo_CloseReasonCode").val().length <= 0) {
        console.log("00...");
        $("#lblCloseReasonCodeMsg").html("Please select Close Reason Code!");
        return;
    }
    console.log("inn...");
    $.post(SiteRoot + "BalanceDues/UpdateBDCloseReasonCode?balanceId=" + balId + "&closeReasonCode=" + $("#BalanceDueInfo_CloseReasonCode").val(), null, function (data) {
        $("#lblCloseReasonCodeMsg").html(data.Message);
    });
});
