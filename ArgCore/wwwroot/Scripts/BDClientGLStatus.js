var balId = $("#BalanceDueInfo_BalanceId").val();
$("#BalanceDueInfo_CloseReasonCode").change(function () {
    if ($("#BalanceDueInfo_CloseReasonCode").val().length <= 0) {
        $("#lblClientGLStatusMsg").html("Please select Close Reason Code!");
        return;
    }
    $.post(SiteRoot + "BalanceDues/UpdateBDCloseReasonCode?balanceId=" + balId + "&closeReasonCode=" + $("#BalanceDueInfo_CloseReasonCode").val(), null, function (data) {
        $("#lblClientGLStatusMsg").html(data.Message);
    });
});
var oldGLStatus = $("#BalanceDueInfo_ClientGlStatus").val();
console.log("Old value: " + oldGLStatus);
//$(".btnSaveGlStatus").click(function () {
$("#BalanceDueInfo_ClientGlStatus").change(function () {
    if (oldGLStatus == "Open") {
        if ($("#BalanceDueInfo_CloseReasonCode").val().length <= 0) {
            $("#lblClientGLStatusMsg").html("Please select Close Reason Code!");
            return;
        }
    }
    var newStatus = $("#BalanceDueInfo_ClientGlStatus").val();
    $.post(SiteRoot + "BalanceDues/UpdateBDClientGLStatus?balanceId=" + balId + "&oldStatus=" + oldGLStatus + "&newStatus=" + newStatus, null, function (data) {
        $("#lblClientGLStatusMsg").html(data.Message);
    });
});
