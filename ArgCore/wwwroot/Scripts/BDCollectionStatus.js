var statusValue = $("#BalanceDueInfo_CollectionStatus").val();
if (statusValue == "Closed") {
    console.log(statusValue);
    $("#BalanceDueInfo_CollectionStatus").attr("disabled", "disabled");
}
$("#BalanceDueInfo_CollectionStatus").change(function () {
    var balId = $("#BalanceDueInfo_BalanceId").val();
    console.log(balId);
    statusValue = $("#BalanceDueInfo_CollectionStatus").val();
    console.log(statusValue);
    if (statusValue == "Closed")
        $("#BalanceDueInfo_CollectionStatus").attr("disabled", "disabled");
    $.post(SiteRoot + "BalanceDues/UpdateBDCollectionStatus?balanceId=" + balId + "&status=" + statusValue, null, function (data) {
        $("#lblCollStatusMsg").html(data);
    });
});
