$(document).ready(function () {
    $('#balanceSearch-list').DataTable({
        "pagingType": "full_numbers",
        "pageLength": 100,
        "bFilter": false,
        "bLengthChange": false
    });
    if ($("#SearchOptions_BolExecutionStartDate,#SearchOptions_BolExecutionEndDate,#SearchOptions_BalanceDueInvoiceStartDate,#SearchOptions_BalanceDueInvoiceEndDate,#SearchOptions_DateAddedStart,#SearchOptions_DateAddedEnd,#SearchOptions_BalanceDuePaymentEndDate,#SearchOptions_BalanceDuePaymentStartDate").val() == "1/1/0001 12:00:00 AM") {
        $("#SearchOptions_BolExecutionStartDate,#SearchOptions_BolExecutionEndDate,#SearchOptions_BalanceDueInvoiceStartDate,#SearchOptions_BalanceDueInvoiceEndDate,#SearchOptions_DateAddedStart,#SearchOptions_DateAddedEnd,#SearchOptions_BalanceDuePaymentEndDate,#SearchOptions_BalanceDuePaymentStartDate").val('');
    }
});
$("body").on("click", ".btnDelCustContact", function () {
    var contactId = $(this).data("contactid");
    var url = SiteRoot + "CustomerContacts/Delete";
    $.post(url, { contactId: contactId }, function (data) {
        if (data == "Deleted") {
            $(".CancelButton").click();
            bootbox.hideAll();
            $(".btnCancel").click();
        }
    }).fail(function (data) {
        console.error("Failed!");
    });
});
$("body").on("click", ".btnDelCustomer", function () {
    console.log("del");
    var customerId = $(this).data("customerid");
    var url = SiteRoot + "Customers/DeleteCustomer";
    console.log(url);
    $.post(url, { customerId: customerId }, function (data) {
        console.log(data);
        if (data == "Deleted") {
            $(".btnCancel").click();
            bootbox.hideAll();
        }
    }).fail(function (data) {
        console.error("Failed!");
    });
});
$("body").on("click", ".loadComments", function () {
    var balanceId = $(this).data("balanceid");
    $.get(SiteRoot + "BalanceDues/LoadCollectionComments?balanceId=" + balanceId, function (html) {
        $("#commentsBlock").html(html);
        $('.ckeditor').ckeditor();
    });
});
//$(".balanceSearch-details td.collStatus > a").click(function () {
//    var balanceId = $(this).data("balanceid");
//    $.get(SiteRoot + "BalanceDues/CollectionStatusDetails?balanceId=" + balanceId, function (html) {
//        //$(".modal-content").html(html);
//            bootbox.dialog({
//                message: html, size: "large"
//            }).addClass('CollectionStatusDialog');
//            $('.ckeditor').ckeditor();
//            $('.modal .modal-body').css('overflow-y', 'auto');
//            $('.modal .modal-body').css('height', $(window).height());
//            $('.modal .modal-body').css('border-radius', "10px");
//    });
//});
//$(".balanceSearch-details td.GLStatus > a").click(function () {
//    var balanceId = $(this).data("balanceid");
//    $.get(SiteRoot + "BalanceDues/ClientGLStatusDetails?balanceId=" + balanceId, function (html) {
//        //$(".modal-content").html(html);
//        bootbox.dialog({
//            message: html, size: "large"
//        }).addClass('ClientGLStatusDialog');
//        $('.ckeditor').ckeditor();
//        $('.modal .modal-body').css('overflow-y', 'auto');
//        $('.modal .modal-body').css('height', $(window).height());
//        $('.modal .modal-body').css('border-radius', "10px");
//    });
//});
//$(".balanceSearch-details td.status > a").click(function () {
//    var balanceId = $(this).data("balanceid");
//    $.get(SiteRoot + "BalanceDues/InvoiceStatusDetails?balanceId=" + balanceId, function (html) {
//        bootbox.dialog({
//            message: html, size: "large"
//        }).addClass('InvoiceStatusDialog');
//        $('.ckeditor').ckeditor();
//        $('.modal .modal-body').css('overflow-y', 'auto');
//        $('.modal .modal-body').css('height', $(window).height());
//        $('.modal .modal-body').css('border-radius', "10px");
//    });
//});
//$(".balanceSearch-details td.bdAmntPaid > a").click(function () {
//    var balanceId = $(this).data("balanceid");
//    $.get(SiteRoot + "BalanceDues/BDPaymentAmountDetails?balanceId=" + balanceId, function (html) {
//        bootbox.dialog({
//            message: html, size: "large"
//        }).addClass('PaymentAmountDialog');
//        $('.modal .modal-body').css('overflow-y', 'auto');
//        $('.modal .modal-body').css('height', $(window).height());
//        $('.modal .modal-body').css('border-radius', "10px");
//    });
//});
var lbl = $("#lblMsg");
function GetBdIds() {
    var bdIdList = "";
    var bdIdsCount = 0;
    var i;
    for (i = 0; i < bdIds.length; ++i) {
        var value = bdIds[i];
        bdIdList += (value + ",");
        bdIdsCount++;
    }
    ;
    console.log("Total BDIds: " + bdIdsCount);
    return bdIdList;
}
$("#GetBDResults").click(function () {
    if ($('#ddlCompanies :selected').val() == '') {
        bootbox.alert("Please select a Client!");
        return false;
    }
});
//If result has Collectionstatus value showing *Reject*, display entire row with light red background
$(".balanceSearch-details .collStatus > a").each(function () {
    var status = $(this).text();
    if (status == "Reject") {
        $(this).parent().parent().addClass("rejectedStaus");
    }
});
//When BOL hyperlink is clicked, please highligh row in a light orange...this helps keep track of what BOLs have been clicked
$("#balanceSearch-list td.bol > a").click(function () {
    $(this).parent().parent().addClass("highlight");
});
$("#balanceSearch-list td.bdAmntPaid > a").click(function () {
    $(this).parent().parent().addClass("viewingBDPaymentAmount");
});
$("#balanceSearch-list td.collStatus span > a").click(function () {
    $(this).parent().parent().parent().addClass("viewingCollStatus");
});
$("#balanceSearch-list td.status span > a").click(function () {
    $(this).parent().parent().parent().addClass("viewingInvoiceStatus");
});
$("#balanceSearch-list td.GLStatus span > a").click(function () {
    $(this).parent().parent().parent().addClass("viewingGLStatus");
});
$("#balanceSearch-list td.customer > a").click(function () {
    $(this).parent().parent().addClass("viewingCustomer");
});
$("#balanceSearch-list td.balDue > a").click(function () {
    $(this).parent().parent().addClass("viewingBDDetail");
});
$("#ProcessResults").click(function () {
    var bdIdList = GetBdIds();
    //var selectedStatuses = $("#ddlInvStatus").val();
    var invoicetypes = $("#ddlInvoiceTypes").chosen().val();
    console.log("Invoice type " + invoicetypes);
    var selectedStatuses = $("#ddlInvStatus").chosen().val();
    console.log(selectedStatuses);
    if ($('.changeBDStatus').is(":checked")) {
        var status = $("#ddlBDInvoiceStatus").val();
        if (status.length > 0) {
            if (status == "Invoiced_REC") {
                return GetResultsByInvoiced_RECStatus(status);
            }
            else {
                return GetResultsByBDInvoiceStatus(status);
            }
        }
    }
    if ($('.generateAuditReview').is(":checked")) {
        if ($('#hiddenPendingApproval').is(":visible") && $('.updatePendingApproval').is(":checked")) {
            UpdatePending();
        }
        return GenerateBalDue();
    }
    if ($('.outputToSpreadsheet').is(":checked")) {
        return GenerateSpreadsheet();
    }
    if ($('.generateInvoices').is(":checked")) {
        if ($.inArray("Invoiced_NR", selectedStatuses) || $.inArray("Invoiced_REC", selectedStatuses)) {
            lbl.html("Generating customer statements...");
            $.post(SiteRoot + "BalanceDues/InvoicePDF", { bdIds: bdIdList, invoiceNameType: 2 }, function (data) {
                console.log("List: ");
                console.log(data.InvoiceOutput);
                var html = "";
                html += "<table class='table table-bordered table-striped' style='width:100%; margin-top:30px'>";
                html += "<thead><tr><th style='width:30%'>Customer ID</th><th style='width:30%;'>Company</th><th style='width:20%; text-align:left'>Invoice#</th><th style='width:50%; text-align:right'>PDF Link</th></thead><tbody>";
                //html += "<tr><td>" + data.InvoiceOutput.CustomerId + "</td><td>" + data.InvoiceOutput.CompanyName + "</td><td style='text-align:right'><a target='_blank' href='" + data.InvoiceOutput.PdfLink + "'>View Invoice</a></td></tr>";
                $.each(data.InvoiceOutput, function (index, item) {
                    html += "<tr><td>" + item.CustomerId + "</td><td>" + item.CompanyName + "</td><td>" + item.BalanceDueInvoice + "</td><td style='text-align:right'><a target='_blank' href='" + item.PdfLink + "'>View Statement</a></td></tr>";
                });
                html += "</tbody></table>";
                lbl.html(html);
            });
        }
        else {
            lbl.html("Invoice Status should be Invoiced_NR or Invoiced_REC to generate Invoices");
            return;
        }
    }
    if ($('.generateEmailInvoices').is(":checked")) {
        if ($.inArray("Invoiced_NR", selectedStatuses) || $.inArray("Invoiced_REC", selectedStatuses)) {
            return GetEmailResultsByBDInvoiceStatus();
        }
        else {
            lbl.html("Invoice Status should be Invoiced_NR or Invoiced_REC to generate Email Invoices");
            return;
        }
    }
});
$(".changeBDStatus").change(function () {
    if ($('.changeBDStatus').is(":checked")) {
        $("#ddlBDInvoiceStatus").removeClass("hidden");
    }
    else {
        $("#ddlBDInvoiceStatus").addClass("hidden");
    }
});
$(".generateEmailInvoices").change(function () {
    if ($('.generateEmailInvoices').is(":checked")) {
        $("#ddlEmailTemplate").removeClass("hidden");
        $("#ddlSMTPUsers").removeClass("hidden");
    }
    else {
        $("#ddlEmailTemplate").addClass("hidden");
        $("#ddlSMTPUsers").addClass("hidden");
    }
});
function GenerateBalDue() {
    console.log("Gen. GenerateBalDue...");
    lbl.html("Generating...");
    $.post(SiteRoot + "BalanceDues/GenerateAuditReviews", { searchOptionsForExport: searchOptionsForExport }, function (data) {
        console.log("Returning GenerateBalDue...");
        console.log(data);
        if (data.indexOf("re-login") > 0) {
            lbl.html(data);
        }
        else {
            lbl.html("<a href='" + data + "' target='_blank'>View Spreadsheet</a>");
        }
    });
}

function UpdatePending() {
    var bdIdList = GetBdIds();
    $.post(SiteRoot + "BalanceDues/UpdatePendingStatus", { bdIds: bdIdList }, function (data) {
        if (data.indexOf("re-login") > 0) {
            lbl.html(data);
        }
    });
}

function GenerateSpreadsheet() {
    console.log("Gen. Spreadsheet...");
    lbl.html("Generating...");
    $.post(SiteRoot + "BalanceDues/GenerateSpreadsheet", { searchOptionsForExport: searchOptionsForExport }, function (data) {
        console.log("Returning Spreadsheet...");
        console.log(data);
        if (data.indexOf("re-login") > 0) {
            lbl.html(data);
        }
        else {
            lbl.html("<a href='" + data + "' target='_blank'>View Spreadsheet</a>");
        }
    });
}
function GetResultsByBDInvoiceStatus(status) {
    var block = $("#StatusResults");
    var customerCount = $(".totalCustomerCount").text();
    var bdAmount = $(".totalBDAmount").text();
    var bdCount = $(".totalBDCount").text();
    var bdPaidAmount = $(".totalBDPaidAmount").text();
    block.html("<div class='declaration'>The following balance dues will be set to an InvoiceStatus=" + status + "</p>");
    block.append("<table id='results-list' class='table text-center'><thead><tr><th class='customer'>Customer Count</th><th class='bdCount'>Balance Due Count</th><th class='bdAmnt'>Balance Due Amount</th></tr></thead><tbody class='results-details'><tr><td class='customer'>" + customerCount + "</td><td class='bdCount'>" + bdCount + "</td><td class='bdAmnt'>" + bdAmount + "</td></tr></tbody></table>");
    block.append("<h3>Do you wish to proceed?</h3>");
    block.append("<input type='button' id='ProceedYes' class='btn btn-lg btn-info' value='Yes' />");
    block.append("<input type='button' id='ProceedNo' class='btn btn-lg btn-danger' value='No' />");
    $("#ProceedYes").click(function () {
        block.html("Updating...");
        var bdIdList = GetBdIds();
        $.post(SiteRoot + "BalanceDues/UpdateInvoiceStatus", { bdIds: bdIdList, status: $("#ddlBDInvoiceStatus").val() }, function (data) {
            var htmlText = "";
            htmlText = "<div class='message'>" + data.UpdatedInvoiceCount + " Balance Dues Updated!</div><br />";
            console.log(data.MissingInvBOLsMsg);
            if (data.MissingInvBOLsMsg != null) {
                htmlText += "<div style='color:#c00000;'>" + data.MissingInvBOLsMsg + "</div>";
            }
            block.html(htmlText);
            //if (data.ChangesDone != null) {
            //    ShowNoty(data.ChangesDone);
            //}
        });
    });
    $("#ProceedNo").click(function () {
        $("#StatusResults").html("");
    });
}
function GetResultsByInvoiced_RECStatus(status) {
    var bdIds = GetBdIds();
    $.post(SiteRoot + "BalanceDues/GetBDByStatus", { bdIds: bdIds }, function (data) {
        var block = $("#StatusResults");
        block.html("<div class='declaration'>The following balance dues will be set to an InvoiceStatus=" + status + "</p>");
        block.append("<table id='results-list' class='table text-center'><thead><tr><th class='customer'>Customer Count</th><th class='bdCount'>Balance Due Count</th><th class='bdAmnt'>Balance Due Amount</th></tr></thead><tbody class='results-details'><tr><td class='customer'>" + data.CustomerCount + "</td><td class='bdCount'>" + data.BDCount + "</td><td class='bdAmnt'>" + data.BDAmount + "</td></tr></tbody></table>");
        if (data.BDCount > 0) {
            block.append("<h3>Do you wish to proceed?</h3>");
            block.append("<input type='button' id='ProceedYes' class='btn btn-lg btn-info' value='Yes' />");
            block.append("<input type='button' id='ProceedNo' class='btn btn-lg btn-danger' value='No' />");
            $("#ProceedYes").click(function () {
                block.html("Updating...");
                var bdIdList = data.BdIdsByStatus;
                $.post(SiteRoot + "BalanceDues/UpdateInvoiceStatusToInvoiced_REC", { bdIdsByStatus: bdIdList, status: $("#ddlBDInvoiceStatus").val() }, function (data) {
                    var htmlText = "";
                    htmlText = "<div class='message'>" + data.UpdatedInvoiceCount + " Balance Dues Updated!</div><br />";
                    console.log(data.MissingInvBOLsMsg);
                    if (data.MissingInvBOLsMsg != null) {
                        htmlText += "<div style='color:#c00000;'>" + data.MissingInvBOLsMsg + "</div>";
                    }
                    block.html(htmlText);
                    //if (data.ChangesDone != null) {
                    //    ShowNoty(data.ChangesDone);
                    //}
                });
            });
            $("#ProceedNo").click(function () {
                $("#StatusResults").html("");
            });
        }
    });
}
//var oldGLStatus = $("#BalanceDueInfo_ClientGlStatus").val();
//$("#BalanceDueInfo_ClientGlStatus").change(function () {
//    console.log("ClGLSta");
//    var balId = $("#BalanceDueInfo_BalanceId").val();
//    var newStatus = $("#BalanceDueInfo_ClientGlStatus").val();
//    $.post(SiteRoot + "BalanceDues/UpdateBDClientGLStatus?balanceId=" + balId + "&oldStatus=" + oldGLStatus + "&newStatus=" + newStatus, null, function (data) {
//        $("#lblClientGLStatusMsg").html(data.Message);
//    });
//});
function GetEmailResultsByBDInvoiceStatus() {
    var block = $("#EmailInvoiceResults");
    var customerCount = $(".totalCustomerCount").text();
    var bdAmount = $(".totalBDAmount").text();
    var bdCount = $(".totalBDCount").text();
    var bdPaidAmount = $(".totalBDPaidAmount").text();
    block.html("<div class='declaration'>The following balance dues will be emailed</p>");
    block.append("<table id='results-list' class='table text-center'><thead><tr><th class='customer'>Customer Count</th><th class='bdCount'>Balance Due Count</th><th class='bdAmnt'>Balance Due Amount</th></tr></thead><tbody class='results-details'><tr><td class='customer'>" + customerCount + "</td><td class='bdCount'>" + bdCount + "</td><td class='bdAmnt'>" + bdAmount + "</td></tr></tbody></table>");
    block.append("<div class='confirmationBlock'><h3>Do you wish to proceed?</h3><input type='button' id='SendEmail' class='btn btn-lg btn-info' value='Yes' /><input type='button' id='SkipEmail' class='btn btn-lg btn-danger' value='No' /></div>");
    $("#SendEmail").click(function () {
        var bdIdList = GetBdIds();
        var smtpAccountId = $("#ddlSMTPUsers").val();
        if (smtpAccountId.length <= 0) {
            console.log("Smtp User");
            lbl.html("Please select SMTP User");
            return;
        }
        var templateId = $("#ddlEmailTemplate").val();
        if (templateId.length <= 0) {
            console.log("Smtp Email");
            lbl.html("Please select Email Template");
            return;
        }
        $(".confirmationBlock").remove();
        lbl.html("Generating PDF's... and Creating Drafts...");
        $.post(SiteRoot + "BalanceDues/InvoicePDF?bdIds=" + bdIdList + "&invoiceNameType=1" + "&smtpAccountId=" + smtpAccountId + "&templateId=" + templateId + "&createDrafts=true", null, function (data) {
            console.log("Confirming...");
            console.log(data);
            if (data.Message != null) {
                lbl.html(data.Message);
            }
            else if (data.EmailResultHtml != null) {
                lbl.html(data.EmailResultHtml);
            }
            else {
                lbl.html(data);
            }
            //lbl.html(data);
            //var draftIdx = 0;
            //lbl.html("Creating Drafts...");
            //$.each(data, (index, item) => {
            //draftIdx++;
            //console.log(item.FileServerPath);
            //item.FileServerPath = encodeURIComponent(item.FileServerPath);
            //$.post(SiteRoot + "BalanceDues/GenerateEmailDraft?smtpAccountId=" + smtpAccountId + "&customerId=" + item.CustomerId + "&templateId=" + templateId + "&attachFileServerPath=" + item.FileServerPath + "&InvoiceNo=" + item.BalanceDue.BalanceDueInvoice + "&BalanceId=" + item.BalanceDue.BalanceId, null, function (data) {
            //    lbl.html("<div class='message'>" + draftIdx + " drafts generated</div>");
            //});
            //});
        }).fail(function (data) {
            console.log(data);
            console.log("Failed");
            lbl.html(data.responseText);
        }).always(function (data) {
            console.log(data);
        });
    });
    $("#SkipEmail").click(function () {
        $("#EmailInvoiceResults").html("");
    });
}