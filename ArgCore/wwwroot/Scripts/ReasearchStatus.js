var oldStatus = $("#ddlResStatus").val();
var resId = $("#ResearchInfo_ResearchId").val();
console.log("Old value: " + oldStatus);
$("#ddlResStatus").change(function () {
    var newStatus = $("#ddlResStatus").val();
    $.post(SiteRoot + "Research/UpdateStatus?researchId=" + resId + "&oldStatus=" + oldStatus + "&newStatus=" + newStatus, null, function (data) {
        $("#lblResearchStatusMsg").html(data.Message);
    });
});
