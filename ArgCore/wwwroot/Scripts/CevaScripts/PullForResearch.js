$("#AddResearchReasonCodePopUp").click(function () {
    var url = SiteRoot + "Research/AddResearchReasonCode";
    console.log(url);
    $.get(url, function (html) {
        bootbox.dialog({
            message: html, size: "large"
        }).addClass('AddResearchReasonCodeDialog');
    });
});
