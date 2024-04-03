/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
$('.chzn-select').chosen();
$(".firstSpacetruncate").on("keypress", function (e) {
    if (e.which === 32 && !this.value.length)
        e.preventDefault();
});
$("#SortByBlock").sortable({
    group: 'no-drop',
    handle: 'i.fa-arrows',
    onDragStart: function ($item, container, _super) {
        // Duplicate items of the no drop area
        if (!container.options.drop)
            $item.clone().insertAfter($item);
        _super($item, container);
    },
    update: function (event, ui) {
        UpdateSortIndexes();
    },
});

$(".balDueSearchForm #GetBDResults").click(function () {
    if ($("#ddlClients").val().length <= 0) {
        $("#lblClientReportMsg").html("Please select client!");
        return false;
    }
});

$("#ClientManagementReports #GetClientReportResults").click(function () {
    if ($("#ddlClients").val().length <= 0) {
        $("#lblClientReportMsg").html("Please select client!");
        return false;
    }
});
function ShowNoty(text) {
    new Noty({
        type: 'success',
        text: text
    }).show();
}
$(".btnDelete").click(function (e) {
    var link = $(this).attr("href");
    e.preventDefault();
    bootbox.confirm("Are you sure you want to delete?", function (result) {
        if (result) {
            document.location.href = link;
        }
    });
});
$("body").on("click", ".loadComments", function () {
    var playId = $(this).data("playid");
    $.get(SiteRoot + "BOL/LoadPlaybookComments?playId=" + playId, function (html) {
        $("#InvoiceStatusDetails").html(html);
        $('.ckeditor').ckeditor();
    });
});
$("body").on("click", ".btnDelCustContact", function () {
    var contactId = $(this).data("contactid");
    var url = SiteRoot + "CustomerContacts/Delete?contactId=" + contactId;
    $.post(url, function (data) {
        if (data == "Deleted") {
            $(".CancelButton").click();
            bootbox.hideAll();
            $(".btnCancel").click();
        }
    }).fail(function (data) {
        console.error("Failed!");
    });
});
//$("#AddCustomerContactPopUp").click(function () {
//    console.log("Add");
//    var customerId = $(this).data("customerid");
//    console.log(customerId);
//    var url = SiteRoot + "CustomerContacts/Save?contactId=0&customerId=" + customerId;
//    console.log(url);
//    $.get(url, function (html) {
//        bootbox.dialog({
//            message: html, size: "large"
//        }).addClass('AddCustContactDialog');
//    });
//});
$("body").on("click", ".bootbox-pop", function () {
    var url = $(this).attr("href");
    console.log(url);
    var isFullScreen = $(this).hasClass("modal-fullscreen");
    var auditsorting = $(this).hasClass("auditsorting");
    console.log("isFullScreen: " + isFullScreen);
    $.get(url, function (html) {
        if (isFullScreen) {
            if (auditsorting) {
                $("#modal-fullscreen .modal-body").html(html);
            }
            else {
                $(".modal-body").html(html);
            }
            $('#modal-fullscreen').modal('show');
            $('.ckeditor').ckeditor();
        }
        else {
            bootbox.dialog({
                message: html, size: "large"
            }).addClass('AddCustContactDialog');
            $('.ckeditor').ckeditor();
        }
    });
    return false;
});
function OnBegin() {
    $(".btnCustContactAdded").attr('disabled', 'disabled');
}
function OnNewCustContactAdded(ajaxContext) {
    if (ajaxContext.Message == "Added") {
        $(".btnCustContactAdded").removeAttr('disabled');
        $(".CancelButton").click();
        bootbox.hideAll();
        $(".close").click();
        window.location.reload();
    }
    else {
        $(".btnCustContactAdded").removeAttr('disabled');
        $("#lblCustContactMsg").html(ajaxContext.Message);
    }
}
function UpdateSortIndexes() {
    console.log("Updating Sort Indexes");
    var sortopts = $("#SortByBlock.sortable > div > .idx");
    var idx = 0;
    sortopts.each(function () {
        $(this).val(idx);
        console.log($(this));
        idx++;
    });
}
function GetFileExtension(filename) {
    return typeof filename != "undefined" ? filename.substring(filename.lastIndexOf(".") + 1, filename.length).toLowerCase() : false;
}
//function executeCopy(text) {
//    var input = document.createElement('textarea');
//    document.body.appendChild(input);
//    input.value = text;
//    input.focus();
//    input.select();
//    document.execCommand('Copy');
//    input.remove();
//}
//function executeCopy2(html) {
//    var doc = new DOMParser().parseFromString(html, 'text/html');
//    var text = doc.body.textContent;
//    return executeCopy(text);
//}
$("#txtSearch").keydown(function (event) {
    if (event.keyCode === 13) {
        $('#btnSearch').click();
    }
});
$("#btnSearch").click(function () {
    if ($("#txtSearch").val().length <= 0) {
        return;
    }
    var href = document.location.href;
    if (href.indexOf("?q") > 0) {
        href = href.substr(0, href.indexOf("?q"));
    }
    var url = href + "?q=" + $("#txtSearch").val();
    console.log("findbtn: " + url);
    window.location.href = url;
});
$("#btnJumpToRecord").click(function () {
    var queryId = $(this).data("queryid");
    var companyId = $(this).data("companyid");
    var idx = $("#idx").val();
    window.location.href = SiteRoot + "Bol/AuditingResults?queryId=" + queryId + "&idx=" + idx + "&companyId=" + companyId;
});
$("body").on("click", "#btnBookingJumpToRecord", (function () {
    var queryId = $(this).data("queryid");
    var idx = $("#idx").val();
    window.location.href = SiteRoot + "Booking/AuditingResults?queryId=" + queryId + "&idx=" + idx;
}));
$(".btnCancel").click(function () {
    var url = document.location.href;
    if (url.indexOf("CustomerContacts") > 0) {
        url = SiteRoot + "Customers/Index";
    }
    else {
        url = url.replace("/Save", "/Index");
        if (url.indexOf("ShowGroups") > 0)
            url = url.replace("SettingGroups/ShowGroups?groupId=1", "Admin/Index");
    }
    window.location.href = url;
});
$('#GetActivityResults').validate({
    highlight: function (element) {
        $(element).removeClass("input-validation-error");
    }
});
//Empty Datetime fields
if ($("#InvoiceDetail_InvoiceId").val() == 0) {
    $("#AddInvoice #InvoiceDetail_InvoiceDate").val('');
    $("#AddInvoice #InvoiceDetail_DueDate").val('');
}
if ($("#CommissionDetail_CommissionId").val() == 0) {
    $("#AddCommission #CommissionDetail_InvoiceDate").val('');
    $("#AddCommission #CommissionDetail_BolExecutionDate").val('');
}
if ($("#SearchResearchResults").length > 0) {
    if ($("#SearchOptions_BolExecutionStartDate,#SearchOptions_BolExecutionEndDate,#SearchOptions_LastModifiedStartDate,#SearchOptions_LastModifiedEndDate").val() == "1/1/0001 12:00:00 AM") {
        $("#SearchOptions_BolExecutionStartDate,#SearchOptions_BolExecutionEndDate,#SearchOptions_LastModifiedStartDate,#SearchOptions_LastModifiedEndDate").val('');
    }
}
if ($("#UserStat-details").length > 0) {
    if ($("#SearchOptions_BeginDate,#SearchOptions_EndDate").val() == "1/1/0001 12:00:00 AM") {
        $("#SearchOptions_BeginDate,#SearchOptions_EndDate").val('');
    }
}
if ($(".dateCtrl").length > 0) {
    $(".dateCtrl").each(function () {
        if ($(this).val().indexOf("0001") > 0)
            $(this).val("");
    });
}
function LoadChosenSelectedVals() {
    $(".chzn-select").each(function () {
        var selected = $(this).attr("selectedvals");
        if (typeof (selected) != "undefined" && selected.length > 0) {
            console.log(selected);
            var selectedVals = selected.split(",");
            console.log(selectedVals);
            var ctrlId = $(this).attr("id");
            $("#" + ctrlId + " > option").each(function () {
                if ($.inArray(this.value, selectedVals) > -1) {
                    $(this).attr("selected", "selected");
                }
            });
            $(this).trigger("chosen:updated");
        }
    });
}
var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)), sURLVariables = sPageURL.split('&'), sParameterName, i;
    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};
$("#AddToPlaybook").change(function () {
    console.log(this);
    if (this.checked) {
        var queryId = getUrlParameter("queryId");
        console.log(queryId);
        $.get(SiteRoot + "BOL/AddToPlaybook?queryId=" + queryId, function (data) {
            console.log(data);
            console.log(data.PlayID);
            console.log(data.QueryId);
            if (data.QueryId != "undefined" && data.PlayID != "undefined") {
                window.location.href = SiteRoot + "BOL/AuditorPlaybook?queryId=" + data.QueryId + "&playId=" + data.PlayID;
            }
            else {
                window.location.reload(true);
            }
        });
    }
});
$('body').on("click", ".deleteplaybook", function () {
    var tr = $(this).parent().parent();
    var playId = $(this).attr("data-playid");
    var companyId = $(this).attr("data-companyid");
    bootbox.confirm("Are you sure you want to delete?", function (result) {
        if (result) {
            //if (confirm("Are you sure you want to delete?")) {            
            console.log($(this));
            $.post(SiteRoot + "BOL/DeletePlaybook?playId=" + playId + "&companyId=" + companyId, null, function () {
                tr.remove();
            });
        }
    });
});
$("body").on("change", "#ddlArgInvoiceCompany", function () {
    var companyId = $("#ddlArgInvoiceCompany").chosen().val();
    console.log("Active Client selected...");
    $.get(SiteRoot + "ArgInvoicesBD/Index?companyId=" + companyId, function (data) {
        console.log(data);
        window.location.reload(true);
    });
});
$("body").on("change", ".SelectActiveClient", function () {
    var companyId = $(".SelectActiveClient").val();
    console.log("Active Client selected...");
    $.post(SiteRoot + "BOL/SetActiveClient?companyId=" + companyId, function (data) {
        console.log(data);
        window.location.reload(true);
    });
});
$("body").on("change", ".SelectActiveClientPerformance", function () {
    var companyId = $(".SelectActiveClientPerformance").val();
    console.log("Active Client selected...");
    $.post(SiteRoot + "BOL/SetActiveClient?companyId=" + companyId, function (data) {
        console.log(data);
        window.location.href = SiteRoot + "Analyst/Performance";
    });
});
$("body").on("change", ".SelectActive", function () {
    var companyId = $(".SelectActive").val();
    console.log("Active Client selected...");
    $.post(SiteRoot + "BOL/SetActiveClient?companyId=" + companyId, function (data) {
        console.log(data);
        window.location.href = SiteRoot + "BOL/Index";
        // window.location.reload(true);
    });
});
$("body").on("change", ".SelectActiveBalDue", function () {
    var companyId = $(".SelectActiveBalDue").val();
    console.log("Active Client selected...");
    $.post(SiteRoot + "BOL/SetActiveClient?companyId=" + companyId, function (data) {
        console.log(data);
        window.location.href = SiteRoot + "BalanceDues/Index";
        // window.location.reload(true);
    });
});
$("body").on("change", ".SelectInvoiceType", function () {
    var invoiceType = $(".SelectInvoiceType").val();
    if (invoiceType == "Per Diem Invoice") {
        $(".excludefrombilltype").addClass("hidden");
    }
    else {
        $(".excludefrombilltype").removeClass("hidden");
    }
});
$("body").on("change", "#Stats-filter-list", function () {
    var queryId = $("#Stats-filter-list").attr("data-queryid");
    var groupType = $("#Stats-filter-list").val();
    if ($("#Stats-filter-list option:selected").text() == "Group by Shipper") {
        window.location.href = SiteRoot + "BOL/ViewAuditingResultStatsByShipper" + "?queryId=" + queryId + "&group=" + groupType;
    }
    else if ($("#Stats-filter-list option:selected").text() == "Group by Origin/Destination") {
        window.location.href = SiteRoot + "BOL/ViewAuditingResultStatsByOrigin" + "?queryId=" + queryId + "&group=" + groupType;
    }
    else if ($("#Stats-filter-list option:selected").text() == "Group by POL") {
        window.location.href = SiteRoot + "BOL/ViewAuditingResultStatsByPOL" + "?queryId=" + queryId + "&group=" + groupType;
    }
    else if ($("#Stats-filter-list option:selected").text() == "Group by Shipper/Origin/Destination") {
        window.location.href = SiteRoot + "BOL/ViewAuditingResultStats" + "?queryId=" + queryId + "&group=" + groupType;
    }
    else {
        window.location.href = SiteRoot + "BOL/ViewAuditingResultStats" + "?queryId=" + queryId;
    }
});
$("body").on("change", "#Booking-Stats-filter-list", function () {
    var queryId = $("#Booking-Stats-filter-list").attr("data-queryid");
    var groupType = $("#Booking-Stats-filter-list").val();
    if ($("#Booking-Stats-filter-list option:selected").text() == "Group by SHIPPER, POL, POD") {
        window.location.href = SiteRoot + "Booking/ViewAuditingResultStats" + "?queryId=" + queryId + "&group=" + groupType;
    }
    else if ($("#Booking-Stats-filter-list option:selected").text() == "Group by POL, POD") {
        window.location.href = SiteRoot + "Booking/ViewAuditingResultStatsByOrigin" + "?queryId=" + queryId + "&group=" + groupType;
    }
    else if ($("#Booking-Stats-filter-list option:selected").text() == "Group by POL") {
        window.location.href = SiteRoot + "Booking/ViewAuditingResultStatsByPOL" + "?queryId=" + queryId + "&group=" + groupType;
    }
    else {
        window.location.href = SiteRoot + "Booking/ViewAuditingResultStats" + "?queryId=" + queryId;
    }
});
if ($("#ResearchItemDetail_ResearchId").val() <= 0) {
    $("#ResearchItemDetail_BolExecutionDate").val('');
}
$(".auditorplaybook-list td.options > a.updateplaybookstatus").click(function () {
    $(this).parent().parent().removeClass("deleteplaybookStatus playplaybookStatus playbookcommentreadmore commentplaybookStatus");
    $(this).parent().parent().addClass("updateplaybook");
});
$(".auditorplaybook-list td.options > a.deleteplaybook").click(function () {
    $(this).parent().parent().removeClass("updateplaybook playplaybookStatus playbookcommentreadmore commentplaybookStatus");
    $(this).parent().parent().addClass("deleteplaybookStatus");
});
$(".auditorplaybook-list td.options > a.btnplaybookcomment").click(function () {
    $(this).parent().parent().removeClass("updateplaybook playplaybookStatus playbookcommentreadmore deleteplaybookStatus");
    $(this).parent().parent().addClass("commentplaybookStatus");
});
$(".auditorplaybook-list td.options > a.playbookquery").click(function () {
    $(this).parent().parent().removeClass("updateplaybook commentplaybookStatus playbookcommentreadmore deleteplaybookStatus");
    $(this).parent().parent().addClass("playplaybookStatus");
});
$(".auditorplaybook-list .playcomments .playbookcomment").click(function () {
    $(this).parent().parent().parent().parent().removeClass("updateplaybook commentplaybookStatus playplaybookStatus deleteplaybookStatus");
    $(this).parent().parent().parent().parent().addClass("playbookcommentreadmore");
});
$("#stats-list td a.ViewAuditStats").click(function () {
    $(this).parent().parent().addClass("optionviewauditstats");
});
$("#auditor-table td a.ViewAuditingResultTableFormat").click(function () {
    $(this).parent().parent().addClass("bolresulttable");
});
$("#research-list td a").click(function () {
    $(this).parent().parent().addClass("editresearchitem");
});
$(".auditorplaybook-list td > #playtableformat").click(function () {
    var items;
    $(this).parent().parent().addClass("selected-row");
    if ($('.auditorplaybook-list .selected-row #playtableformat').is(':checked')) {
        items = $(".auditorplaybook-list .selected-row > td");
        console.log(items);
        $(items).each(function () {
            var queryId = $(".auditorplaybook-list .selected-row a.playbookquery").data("qid");
            $(".auditorplaybook-list .selected-row a.playbookquery").attr("href", SiteRoot + "Bol/ViewAuditingResultTableFormat?queryId=" + queryId + "&idx=1");
        });
    }
    else {
        var queryId = $(".auditorplaybook-list .selected-row a.playbookquery").data("qid");
        var sqlquery = $(".auditorplaybook-list .selected-row a.playbookquery").data("sqlquery");
        if (sqlquery == "undefined" || sqlquery == "") {
            $(".auditorplaybook-list .selected-row a.playbookquery").attr("href", SiteRoot + "Bol/Index?queryId=" + queryId);
        }
        else {
            $(".auditorplaybook-list .selected-row a.playbookquery").attr("href", SiteRoot + "Bol/AuditingResults?queryId=" + queryId + "&idx=1");
        }
    }
    ;
    $(".auditorplaybook-list .selected-row").removeClass("selected-row");
});
$(document).ready(function () {
    $("body").scrollbar();
    if ($("#RoleId option:selected").text() == "Administrator") {
        $(".validStatus").addClass("hidden");
    }
    //if (currentUserRole == "Client User" || currentUserRole == "ClientManager")
    //if (currentUserRole == "Administrator") {
    //    $(".child_menu li a").each(function () {
    //        var ctrl = $(this);
    //        var ctrlText = ctrl.text();
    //        console.log(ctrlText.indexOf("Manage Client Invoices") > 0);
    //        if (ctrlText.indexOf("Manage Client Invoices") > 0) {
    //            ctrl.text("View Invoices");
    //        }
    //        if (ctrlText.indexOf("Manage Balance Dues & Collections") > 0) {
    //            ctrl.text("View Balance Dues & Collections");
    //        }
    //    });
    //}
    LoadChosenSelectedVals();
    //Done to remove empty option getting by Chosen.. Clearing the null value in dropdown
    $("#BolAuditing option[value='']").remove();
    $("#ddlRefTypes, #ddlContainerEventTypes, #ddlShipmentType, #ddlShipmentStatus, #ddlShipmentCLStatus").val('');
    $("#ddlUsers").val('');
    $("#SearchOptions_BOLViews").val('');
    $("#SortByBlock .chkDesc").bootstrapSwitch();
    $(".bootstrap-switch-wrapper").addClass("bootstrap-switch-mini");
    $("#SortByBlock .bootstrap-switch-handle-on").text("DESC");
    $("#SortByBlock .bootstrap-switch-handle-off").text("ASC");
    $(".neverClearedTouched").removeClass("neverClearedTouched").add("neverClear");
    $(".neverClear").click(function () {
        $(this).removeClass("neverclear").addClass("neverClearedTouched");
    });
    $("#frmBOLAuditing .clearDDL").val('');
    $("body").on("click", "#ClearForm", function () {
        $("#frmBOLAuditing").find('input:text:not(.neverclear), select:not(.neverclear), textarea:not(.neverclear)').val('');
        $("#frmBOLAuditing").find('input:checkbox:not(.neverclear)').removeAttr('checked').removeAttr('selected');
        $('.chzn-select').val(null).trigger('chosen:updated');
    });
    $("body").on("click", "#spreedsheetAuth", function () {
        debugger;
        $.get(SiteRoot + "Settings/AuthSetting", function (data) {
            console.log(data);
            $("#authMessage").text(data);
        });
    });
    $("body").on("click", "#ClearFormBooking", function () {
        $("#frmBookingAuditing").find('input:text:not(.neverclear), select:not(.neverclear), textarea:not(.neverclear)').val('');
        $("#frmBookingAuditing").find('input:checkbox:not(.neverclear)').removeAttr('checked').removeAttr('selected');
        $('.chzn-select').val(null).trigger('chosen:updated');
    });
    $("body").on("click", "#ClearBDForm", function () {
        $(".balDueSearchForm").find('input:text:not(.neverclear), select:not(.neverclear), textarea:not(.neverclear)').val('');
        $(".balDueSearchForm").find('input:checkbox:not(.neverclear)').removeAttr('checked').removeAttr('selected');
        $('.chzn-select').val(null).trigger('chosen:updated');
    });
    $(".datepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'mm-dd-yy',
        yearRange: "-0:+50"
    });
    $(".ValidtillDatepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'mm-dd-yy',
        yearRange: "-0:+50",
        minDate: "0"
    });
    $(".BDDatepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'mm-dd-yy',
        yearRange: "-10:+40"
    });
    $(".DepEndDatepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'mm-dd-yy',
        yearRange: "-10:+40"
    });
    setTimeout(function () { $(".leftBlock").height($(".rightBlock").height()); }, 500);
    //$(".leftBlock").height($(".rightBlock").height());
    $('#trig').on('click', function () {
        $('#colMain').toggleClass('col-md-9 col-md-12');
        $('#colPush').toggleClass('col-md-3 col-md-0').addClass("openMenu");
    });
    $('#DailyBalDueCircle').circleProgress({
        value: 1,
        thickness: 10,
        size: 180,
        value: 50,
        fill: {
            gradient: ["#45AF7F", "#5DCE9A"]
        }
    });
    $('#WeeklyBalCircle').circleProgress({
        value: 1,
        size: 180,
        thickness: 10,
        fill: {
            gradient: ["#55A1C1", "#65B5D6"]
        }
    });
    $('#DailyBalCollectedCircle').circleProgress({
        value: 1,
        thickness: 10,
        size: 180,
        fill: {
            gradient: ["#FB7851", "#E87352"]
        }
    });
    $('#WeeklyBalDueCircle').circleProgress({
        value: 1,
        thickness: 10,
        size: 180,
        fill: {
            gradient: ["#E0BB44", "#EECA5A"]
        }
    });
    //$('.counter').counterUp({
    //    delay: 20,
    //    time: 500
    //});
    $("#RoleId").change(function () {
        if ($("#RoleId option:selected").text() == "Administrator") {
            $(".validStatus").addClass("hidden");
        }
        else {
            $(".validStatus").removeClass("hidden");
        }
    });
    if ($("#AddInvoice").length > 0) {
        console.log("Add Invoice found..");
        $("#ddlCompanies,#InvoiceDetail_Region").change(function () {
            console.log("Calculation Invoice value..");
            CalculateInvoice();
        });
    }
    //#stats-list >> ViewAuditingResultStats
    $("#stats-list").DataTable({
        "bFilter": false,
        "pageLength": 5000,
        //"ordering": false,
        "bLengthChange": false
    });
    $("#table-stats-list").DataTable({
        //"bFilter": false,
        "pageLength": 5000,
    });
    $("#auditor-table").DataTable({
        "paging": false,
        "bFilter": false,
        "order": []
    });
    $('#research-list').DataTable({
        "paging": false,
        "bFilter": false,
        "order": []
    });
    //$("#audPlaybook-list").DataTable({
    //    "bFilter": false,
    //    "pageLength": 5000,
    //    //"ordering": false,
    //    "bLengthChange": false
    //});
    //http://localhost:26744/ActivityStats/Index
    var dataTableOptions = {
        "order": [[0, 'asc'], [1, 'asc']],
        "pagingType": "full_numbers",
        "dom": 'rtip',
        language: {
            paginate: {
                next: '<i class="fa fa-fw fa-long-arrow-right">',
                previous: '<i class="fa fa-fw fa-long-arrow-left">',
                sFirst: '',
                sLast: ''
            }
        },
    };
    var shouldPaginate = $('#activityStats-list tbody tr').length > 10;
    dataTableOptions["bPaginate"] = shouldPaginate;
    $('#activityStats-list').DataTable(dataTableOptions);

    var shouldPaginate = $('#regions-list tbody tr').length > 10;
    dataTableOptions["bPaginate"] = shouldPaginate;
    $('#regions-list').DataTable(dataTableOptions);

    //Don't uncomment it add necessary in uppar datatable..
    //$("#research-list, #users-list,#balance-list,#clients-list,#invoices-list,#regions-list").DataTable({
    //    "bFilter": false,
    //    "ordering": false,
    //    "bLengthChange": false
    //});
    //$('#customers-list').DataTable({
    //    "bFilter": false, "bLengthChange": false, "ordering": true
    //});
});
$('.selectableDatatable').on('click', 'tr', function () {
    $(this).toggleClass('selected');
});
function Calculate8DigitRandomCode() {
    var randomCode = Math.floor(Math.random() * 1E8);
    return randomCode;
}
function CalculateInvoice() {
    var region = $("#InvoiceDetail_Region").val();
    //console.log("Region Value: " + region);
    var companyID = $("#ddlCompanies :selected").val();
    //console.log("Company ID: " + companyID);
    if (region.length > 0 && companyID.length > 0) {
        var invoice = companyID + region + Calculate8DigitRandomCode();
        //console.log("Invoice Value: " + invoice);
        $("#InvoiceDetail_Invoice").val(invoice.toUpperCase());
    }
}
function AddCompanyRow(targetTableId, companyName, companyId) {
    $("#" + targetTableId).append("<tr><td class='compName' data-companyid='" +
        companyId + "'>" + companyName + "</td></tr>");
}
$('#auditor-table tbody').on('click', '#chkbolViewed', function () {
    if (this.checked) {
        var bol = $(this).attr("data-bol");
        var companyid = $(this).attr("data-companyid");
        $.post(SiteRoot + "BOL/BOLViewed?bolNo=" + bol + "&companyId=" + companyid, null, function () {
            var clsviewed = ".bolviewed-" + bol;
            $(clsviewed).attr('disabled', 'disabled');
        });
    }
});
$('#auditor-table tbody').on('click', '#chkHBLNOViewed', function () {
    if (this.checked) {
        var bol = $(this).attr("data-bol");
        var companyid = $(this).attr("data-companyid");
        $.post(SiteRoot + "Booking/BOLViewed?bolNo=" + bol + "&companyId=" + companyid, null, function () {
            var clsviewed = ".bolviewed-" + bol;
            $(clsviewed).attr('disabled', 'disabled');
        });
    }
});
$('#assignCompany').on('click', function () {
    console.log("assigning company..");
    var items = $("#companiesList .selected > td");
    $(items).each(function () {
        var companyId = $(this).data("companyid");
        var companyName = $(this).text();
        $.post(SiteRoot + "Companies/AssignCompany", {
            userId: userId,
            companyId: companyId
        }, function (result) {
            AddCompanyRow("assignedCompaniesList", companyName, companyId);
            $("#companiesList .selected:first").remove();
        });
    });
});
$('#removeCompany').on('click', function () {
    console.log("removing assigned company..");
    var items = $("#assignedCompaniesList .selected > td");
    $(items).each(function () {
        var companyId = $(this).data("companyid");
        var companyName = $(this).text();
        $.post(SiteRoot + "Companies/RemoveCompany", {
            userId: userId,
            companyId: companyId
        }, function (result) {
            AddCompanyRow("companiesList", companyName, companyId);
            $("#assignedCompaniesList .selected:first").remove();
        });
    });
});
function AddMenuRow(targetTableId, itemName, itemId) {
    $("#" + targetTableId).append("<tr><td class='menuItemName' data-itemid='" +
        itemId + "'>" + itemName + "</td></tr>");
}
function AddAppActionRow(targetTableId, itemName, itemId) {
    $("#" + targetTableId).append("<tr><td class='menuItemName' data-appactionid='" +
        itemId + "'>" + itemName + "</td></tr>");
}
$('#assignMenuItem').on('click', function () {
    console.log("assigning menu..");
    var items = $("#menusList .selected > td");
    $(items).each(function () {
        var itemId = $(this).data("itemid");
        var itemName = $(this).text();
        $.post(SiteRoot + "Menus/AssignMenuItem", {
            roleId: roleId,
            itemId: itemId
        }, function (result) {
            AddMenuRow("assignedMenuItemsList", itemName, itemId);
            $("#menusList .selected:first").remove();
        });
    });
});
$('#removeMenuItem').on('click', function () {
    console.log("removing assigned menu..");
    var items = $("#assignedMenuItemsList .selected > td");
    $(items).each(function () {
        var itemId = $(this).data("itemid");
        var itemName = $(this).text();
        $.post(SiteRoot + "Menus/RemoveAssignedMenuItem", {
            roleId: roleId,
            itemId: itemId
        }, function (result) {
            AddMenuRow("menusList", itemName, itemId);
            $("#assignedMenuItemsList .selected:first").remove();
        });
    });
});
//var companiesSel = $("#ddlCompanies");
//var regionsSel = $("#ddlRegions");
//var customersSel = $("#ddlCustomers");
//var bdErrorCodesSel = $("#ddlBdErrorCode");
$("#SearchBalanceResults #ddlCompanies").change(function () {
    var companyId = $("#ddlCompanies").val();
    console.log(companyId);
    window.location.href = SiteRoot + "BalanceDues/Index" + "?companyId=" + companyId;
    //LoadRegions();
    //LoadCustomers();
    //LoadBDErrorCodes();re
});
$("#AddCommission #ddlCompanies").change(function () {
    var companyId = $("#ddlCompanies").val();
    var commissionId = $("#CommissionDetail_CommissionId").val();
    window.location.href = SiteRoot + "Commissions/Save?commisionId=" + commissionId + "&companyId=" + companyId;
});
$("#SearchResearchResults #ddlCompanies").change(function () {
    var companyId = $("#ddlCompanies").val();
    window.location.href = SiteRoot + "Research/Save?companyId=" + companyId;
});
$("#SearchCommissionResults #ddlCompanies").change(function () {
    var companyId = $("#ddlCompanies").val();
    window.location.href = SiteRoot + "Commissions/Index?companyId=" + companyId;
});
$("#SearchInvoicesBalanceResults #ddlCompanies").change(function () {
    var companyId = $("#ddlCompanies").val();
    console.log(companyId);
    window.location.href = SiteRoot + "ArgInvoicesBD/Index" + "?companyId=" + companyId;
});
$("#ClientManagementReports #ddlCompanies").change(function () {
    var companyId = $("#ddlCompanies").val();
    console.log(companyId);
    window.location.href = SiteRoot + "Clients/ManagementReports" + "?companyId=" + companyId;
});
$("#GetAuditResults").click(function () {
    var companyId = $(".SelectActive").val();
    console.log(companyId);
    if (companyId === null) {
        bootbox.alert("Please select a Client!");
        return false;
    }
});
$("#Stats-list-origin").click(function () {
    if ($('#Stats-list-origin').is(':checked')) {
        var queryId = $("#Stats-list-origin").data("queryid");
        window.location.href = SiteRoot + "BOL/ViewAuditingResultStatsByOrigin" + "?queryId=" + queryId;
    }
});
$("#Stats-list-pol").click(function () {
    if ($('#Stats-list-pol').is(':checked')) {
        var queryId = $("#Stats-list-pol").data("queryid");
        window.location.href = SiteRoot + "BOL/ViewAuditingResultStatsByPol" + "?queryId=" + queryId;
    }
});
$("#Stats-list-shipper").click(function () {
    if ($('#Stats-list-shipper').is(':checked')) {
        var queryId = $("#Stats-list-shipper").data("queryid");
        window.location.href = SiteRoot + "BOL/ViewAuditingResultStats" + "?queryId=" + queryId;
    }
});
$("#GetResearchResults").click(function () {
    if ($('#ddlSelectCompany :selected').val() == '') {
        bootbox.alert("Please select a Client!");
        return false;
    }
});
$('#assignAppAction').on('click', function () {
    var items = $("#appActionsList .selected > td");
    $(items).each(function () {
        var appActionId = $(this).data("appactionid");
        var itemName = $(this).text();
        $.post(SiteRoot + "AppActions/AssignAppAction", {
            appActionId: appActionId,
            roleId: roleId
        }, function (result) {
            AddAppActionRow("assignedAppActionsList", itemName, appActionId);
            $("#appActionsList .selected:first").remove();
        });
    });
});
$('#removeAppAction').on('click', function () {
    var items = $("#assignedAppActionsList .selected > td");
    $(items).each(function () {
        var appActionId = $(this).data("appactionid");
        var itemName = $(this).text();
        console.log(itemName);
        console.log(appActionId);
        $.post(SiteRoot + "AppActions/RemoveAssignedAppAction", {
            appActionId: appActionId,
            roleId: roleId
        }, function (result) {
            AddAppActionRow("appActionsList", itemName, appActionId);
            $("#assignedAppActionsList .selected:first").remove();
        });
    });
});
//function LoadRegions() {
//    var companyId = $(companiesSel).val();
//    if (companyId != null && companyId != '') {
//        $.getJSON(SiteRoot + "BalanceDues/GetRegions?companyId=" + companyId, null, regions => {
//            var regionSel = $(regionsSel);
//            regionSel.empty();
//            $.each(regions, (index, region) => {
//                regionSel.append($('<option/>', {
//                    value: region.Region,
//                    text: region.Region
//                }));
//            });
//        });
//    }
//    regionsSel.enabled = true;
//}
//function LoadCustomers() {
//    var companyId = $(companiesSel).val();
//    if (companyId != null && companyId != '') {
//        $.getJSON(SiteRoot + "BalanceDues/GetCustomers?companyId=" + companyId, null, customers => {
//            var customerSel = $(customersSel);
//            customerSel.empty();
//            $.each(customers, (index, customer) => {
//                customerSel.append($('<option/>', {
//                    value: customer.CustomerId,
//                    text: customer.Customer
//                }));
//            });
//        });
//    }
//    customersSel.enabled = true;
//}
//function LoadBDErrorCodes() {
//    var companyId = $(companiesSel).val();
//    if (companyId != null && companyId != '') {
//        $.getJSON(SiteRoot + "BalanceDues/GetBDErrorCodes?companyId=" + companyId, null, bdErrorCodes => {
//            var bdErrorCodeSel = $(bdErrorCodesSel);
//            bdErrorCodeSel.empty();
//            $.each(bdErrorCodes, (index, bdErrorCode) => {
//                bdErrorCodeSel.append($('<option/>', {
//                    value: bdErrorCode.BdErrorCode,
//                    text: bdErrorCode.ErrorCodes
//                }));
//            });
//        });
//    }
//    bdErrorCodesSel.enabled = true;
//} 
$("body").on("change", ".clientfl", function () {
    var ClientID = $("#ddlClients.clientfl").val();
    //var Region = $("#ddlRegions.Regionfl").val();
    if (ClientID == "" || ClientID == undefined || ClientID.length <= 0) {
        ClientID = "";
    }
    //if (Region == "" || Region == undefined || Region.length <= 0) {
    //    Region = "";
    //}
    //$.get(SiteRoot + "Regions/LoadRegions?ClientID=" + ClientID + "&Region=" + Region, function (html) {
    $.get(SiteRoot + "Regions/LoadRegions?ClientID=" + ClientID, function (html) {

        $("#regions-list").html(html);
        if ($.fn.DataTable.isDataTable('#regions-list')) {
            $('#regions-list').DataTable().destroy();
        }
        $('#regions-list').DataTable({
            "order": [[0, 'asc'], [1, 'asc']],
            "pagingType": "full_numbers",
            "pageLength": 100,
            "bFilter": false,
            "bLengthChange": false,
            "ordering": true
        });
        $.get(SiteRoot + "Regions/LoadRegions?ClientID=" + ClientID + "&IsGetRegion=true", function (html) {

            $("#ddlRegions").html(html);
        });
    });
});
$("body").on("change", ".Regionfl", function () {
    var ClientID = $("#ddlClients.clientfl").val();
    var Region = $("#ddlRegions.Regionfl").val();
    if (ClientID == "" || ClientID == undefined || ClientID.length <= 0) {
        ClientID = "";
    }
    if (Region == "" || Region == undefined || Region.length <= 0) {
        Region = "";
    }
    $.get(SiteRoot + "Regions/LoadRegions?ClientID=" + ClientID + "&Region=" + Region, function (html) {

        $("#regions-list").html(html);
        if ($.fn.DataTable.isDataTable('#regions-list')) {
            $('#regions-list').DataTable().destroy();
        }
        $('#regions-list').DataTable({
            "order": [[0, 'asc'], [1, 'asc']],
            "pagingType": "full_numbers",
            "pageLength": 100,
            "bFilter": false,
            "bLengthChange": false,
            "ordering": true
        });
    });
});
$("#ddlClient").on("change", function () {
    var selectedClient = $("#ddlClient").find(":selected").val();
    if (selectedClient != undefined && selectedClient != "" && selectedClient.length > 0) {
        $("#ProcessReportsbtn").removeClass("hidden");
    } else {
        $("#RptlblMsg").empty();
        $("#ProcessReportsbtn").addClass("hidden");
    }
});
var Rptlbl = $("#RptlblMsg");
$("#ProcessReports").click(function () {
    var Reports = $("#ddlReports").find(":selected").val();
    if (Reports != undefined && Reports != "" && Reports.length > 0) {
        var Type = $('input[name="ReportType"]:checked').val();
        if (Reports == "ARG_Management_Snapshot") {
            var selectedClient = $("#ddlClient").find(":selected").val();
            if (selectedClient == '' || selectedClient == undefined || selectedClient.length <= 0) {
                return false;
            }
            console.log("Gen. Generate...");
            Rptlbl.html("Generating...");
            $.post(SiteRoot + "Reports/GenerateReport", { Client: selectedClient, ReportType: Reports, Type: Type }, function (data) {
                console.log("Returning GenerateBalDue...");
                console.log(data);
                if (data == "false") {
                    data = "You do not have access to this report.";
                    //$("#ddlReports").val("");
                    //$("#ProcessReportsbtn").addClass("hidden");
                    //$("#ClientDropdown").find($('option')).attr('selected', false);
                    //$("#ClientDropdown").addClass("hidden");
                    //$("#AnalystDiv").addClass("hidden");
                    Rptlbl.html(data);
                }
                else {
                    Rptlbl.html("<a href='" + data + "' target='_blank'>Download Report</a>");
                    window.open(data, '_blank');
                }
            });
        }
        else if (Reports == "ARG_Pending_Balance_Dues" || Reports == "RevenueByClientAnalyst" || Reports == "Revenue_Analyst_Productivity") {
            var Analyst = $("#Analyst").val();
            console.log("Gen2. Generate...");
            Rptlbl.html("Generating...");
            $.post(SiteRoot + "Reports/GenerateReport", { Analyst: Analyst, ReportType: Reports, Type: Type }, function (data) {
                console.log("Returning GenerateBalDue...");
                console.log(data);
                if (data == "false") {
                    data = "You do not have access to this report.";
                    Rptlbl.html(data);
                }
                else {
                    Rptlbl.html("<a href='" + data + "' target='_blank'>Download Report</a>");
                    window.open(data, '_blank');
                }
            });
        }
        else {
            return false;
        }
    }
    else {
        return false;
    }
});
$("input[name='ReportType']").on("change", function () {
    $("#RptlblMsg").empty();
});