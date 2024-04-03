var clientId = $("#ddlClients").val();
$(document).ready(function () {
    if (clientId.length > 0) {
        $("#FilesDDL").removeClass("hidden");
    }
});
$("#BtnLoadClientData").click(function () {
    $("#lblLoadClientsDataMsg").html("");
    if (clientId.length <= 0) {
        $("#lblLoadClientsDataMsg").html("Please select Client!");
        return;
    }
    //var fileId = $("#ddlDataFiles").val();
    //console.log(fileId);
    //if (fileId.length <= 0) {
    //    $("#lblLoadClientsDataMsg").html("Please select File!");
    //    return false;
    //}
    //var filesList = $('#ddlDataFiles').children('option').map(function (i, e) {
    //    return e.innerText;
    //}).get();
    //console.log(filesList);
    $.post(SiteRoot + "Import/ImportData?companyId=" + clientId, null, function (data) {
        $("#lblLoadClientsDataMsg").css("overflow", "scroll");
        $("#lblLoadClientsDataMsg").html(data.Message);
    });
    LoadProgress();
});
function LoadProgress() {
    setInterval(function () {
        $.get(SiteRoot + "Import/GetImportProgress", null, function (data) {
            $("#lblLoadClientsDataMsg").html("");
            $.each(data, function (i, item) {
                console.log(item);
                $("#lblLoadClientsDataMsg").append(item + "<br />");
            });
        });
    }, 10000);
}
$("#BtnMngMappings").click(function () {
    $("#lblLoadClientsDataMsg").html("");
    var selectedFile = $("#ddlDataFiles").val();
    if (selectedFile.length <= 0) {
        $("#lblLoadClientsDataMsg").html("Please select File!");
        return;
    }
    var url = SiteRoot + "Import/ManageMappings?file=" + selectedFile;
    window.open(url, '_blank');
});
$("#BtnMngTableSettings").click(function () {
    var url = SiteRoot + "Import/ManageTableSettings";
    window.open(url, '_blank');
});
