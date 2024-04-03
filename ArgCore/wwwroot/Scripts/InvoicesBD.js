//Should be able to view all invoices from all clients....when I attempt this it states to select a client..
//$("#GetBDResults").click(function () {
//    if ($("#ddlCompanies").val() <= 0) {
//        $("#lblInvoiceBDMsg").html("Please select client!");
//        return false;
//    }
//});
//$(document).ready(function () {
//    $("#invoicePaidTypeBlock").hide();
//});
var terms = 0;

$(document).ready(function () {
    loadInvoiceNos();
    $("#ddlRegions").change(function () {
        loadInvoiceNos();
    });

    $("#ddlInvoiceTypes").change(function () {
        loadInvoiceNos();
    });
});

$(".balDueSearchForm #GetBDResults").click(function () {
    if ($("#ddlClients").val().length <= 0) {
        $("#lblClientReportMsg").html("Please select client!");
        return false;
    }
});
$("#AddInvoicePopUp").on('click', function () {
    $(this).css("pointer-events", "none");
    var companyId = $("#ddlClients").val();
    if (companyId <= 0) {
        $("#lblInvoiceBDMsg").html("Please select client!");
        return false;
    }
    var url = SiteRoot + "Invoices/Save?invoiceId=0";
    console.log(url);
    $.get(url, function (html) {
        $("#AddInvoicePopUp").css("pointer-events", "auto");
        bootbox.dialog({
            message: html, size: "large"
        }).addClass('AddInvoiceDialog');
        $("#InvoiceDetail_InvoiceDate,#InvoiceDetail_DueDate").val('');
        $(".datepicker").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'mm-dd-yy',
            yearRange: "-0:+50"
        });

        $("#InvoiceDetail_InvoiceDate").on('change', function () {
            setInvoiceDueDate($(this).val());
        });

        $("#ddlInvoiceCompanies").change(function () {
            var companyId = $("#ddlInvoiceCompanies").val();
            var invoiceId = $("#InvoiceDetail_InvoiceId").val();

            $("#ddlInvoiceTerms").val(companyId);
            terms = Number($("#ddlInvoiceTerms option:selected").text());

            $("#ddlInvoiceRegions").empty();
            $("#ddlInvoiceType").empty();
            setInvoiceDueDate($("#InvoiceDetail_InvoiceDate").val());

            $.post(SiteRoot + "Invoices/LoadDataByCompany?companyId=" + companyId, function (html) {
                $("#InvoiceDetail_Invoice").val(html.invoice);
                $.each(html.regions, function (i, item) {
                    $("#ddlInvoiceRegions").append('<option value="' + item.value + '">' + item.text + '</option>');
                });
                $.each(html.invoiceTypes, function (i, item) {
                    $("#ddlInvoiceType").append('<option value="' + item.value + '">' + item.text + '</option>');
                });
            });
        });
    });
});
function OnNewInvoiceAdded(ajaxContext) {
    $("#lblAddedInvoiceMsg").html(ajaxContext.Message);
    loadInvoiceNos();
    $(".CancelButton").click();
}
function isEmpty(value) {
    return typeof value == 'string' && !value.trim() || typeof value == 'undefined' || value === null;
}
$("#SearchOptions_AddBDToInvoice").change(function () {
    if ($('#SearchOptions_AddBDToInvoice').is(":checked")) {
        var msg = "";
        var selectedCompany = $("#ddlArgInvoiceCompany").chosen().val();
        var selectedRegion = $("#ddlRegions").chosen().val();
        var selectedInvoice = $("#ddlInvoiceNo").chosen().val();
        var selectedInvoiceType = $("#ddlInvoiceTypes").chosen().val();

        if (selectedCompany == null) {
            msg = "Please select Client";
            $('#SearchOptions_AddBDToInvoice').prop('checked', false);
        }
        //if ($($("#ddlRegions").chosen().val()).length <= 0) {
        if (selectedRegion == null) {
            msg += "<br />Please select Region";
            $('#SearchOptions_AddBDToInvoice').prop('checked', false);
        }
        //if ($($("#ddlInvoiceNo").chosen().val()).length <= 0) {
        if (selectedInvoice == null) {
            msg += "<br />Please select Invoice#";
            $('#SearchOptions_AddBDToInvoice').prop('checked', false);
        }
        //var selectedCompany = $("#ddlClients").val();
        //var selectedRegion = $("#ddlRegions").val();
        //var selectedInvoice = $("#ddlInvoiceNo").val();
        //var InvoiceType = $("#ddlInvoiceTypes").chosen().val();
        //if (selectedCompany.length <= 0) {
        //    msg = "Please select Client";
        //    $('#SearchOptions_AddBDToInvoice').prop('checked', false);
        //}
        ////if ($($("#ddlRegions").chosen().val()).length <= 0) {
        //if (selectedRegion.length <= 0) {
        //    msg += "<br />Please select Region";
        //    $('#SearchOptions_AddBDToInvoice').prop('checked', false);
        //}
        ////if ($($("#ddlInvoiceNo").chosen().val()).length <= 0) {
        //if (selectedInvoice.length <= 0) {
        //    msg += "<br />Please select Invoice#";
        //    $('#SearchOptions_AddBDToInvoice').prop('checked', false);
        //}
        //if (InvoiceType == null) {
        //    msg += "<br />Please select Invoice Type";
        //    $('#SearchOptions_AddBDToInvoice').prop('checked', false);
        //}
        $("#lblInvoiceBDMsg").html(msg);
        if (selectedCompany != null && selectedRegion != null && selectedInvoice != null) {
            //if (selectedCompany.length > 0 && selectedRegion.length > 0 && selectedInvoice.length > 0) {
            //var companyId = $("#ddlCompanies").val();
            //var regions = $("#ddlRegions").val();
            //var invoiceNo = $("#ddlInvoiceNo").val();
            //var invoiceNos = $("#ddlInvoiceNo").chosen().val();
            // var InvoiceType = InvoieTypeDropDown(selectedCompany);
            //console.log("InvoieType:"+InvoiceType);
            $.post(SiteRoot + "ArgInvoicesBD/GetBDCountInfo?companyId=" + selectedCompany + "&region=" + selectedRegion + "&invoiceType=" + selectedInvoiceType + "&invoiceNo=" + selectedInvoice, null, function (data) {
                $("#StatusResults").show();
                var currency = '';
                if (data.Currency !== undefined && data.Currency !== null && data.Currency !== "") {
                    currency = "(" + data.Currency + ")";
                }
                console.log(data);
                console.log("Amount= " + JSON.stringify(data.TotalPaymentAmount));
                console.log("Invoice Type " + JSON.stringify(data.InvoiceTypes));
                var block = $("#StatusResults");

                if (data.TotalBol > 0 && data.ConversationRate === 0 && data.Currency !== undefined && data.Currency !== null && data.Currency !== "" && data.Currency !== "USD") {
                    $("#lblInvoiceConvMsg").html("Conversion rate not available for the " + data.Currency + ".");
                }
                else {
                    block.html("<div class='declaration'><p>The following balance dues will be added to invoice</p></div>");
                    block.append("<table id='results-list' class='table text-center'><thead><tr><th class='totalBol'>Total BOL</th><th class='payAmnt'>Total Payment Amount</th></tr></thead><tbody class='results-details'><tr><td>" + data.TotalBol + "</td><td>" + data.TotalPaymentAmount + currency + "</td></tr></tbody></table>");
                    if (data.TotalBol > 0) {
                        block.append("<h3>Do you wish to proceed?</h3>");
                        block.append("<input type='button' id='ProceedYes' class='btn btn-lg btn-info' value='Yes' />");
                        block.append("<input type='button' id='ProceedNo' class='btn btn-lg btn-danger' value='No' />");
                        //Populate Invoice Type
                        //$("#invoicePaidTypeBlock").show();
                        //var populateInvoiceTypePaid = function (selectbox, dataArray) {
                        //    dataArray.forEach(function (data) {
                        //        selectbox.append("<option>" + data.Text + "</option>");
                        //        $('#ddlInvoiceTypesPaid').trigger('chosen:updated');
                        //    });
                        //};
                        var dataArray = data.InvoiceTypes;
                        //var selectbox = $('#ddlInvoiceTypesPaid');
                        var selectbox = $('#ddlInvoiceTypes');

                        //populateInvoiceTypePaid(selectbox, dataArray);
                        // var combo = $("<select id='SELECTOR'></select>").addClass("form-control chzn-select");
                        $("#ProceedYes").click(function () {
                            //var InvoiceType = $("#ddlInvoiceTypesPaid").chosen().val();
                            var InvoiceType = $("#ddlInvoiceTypes").val();

                            if (InvoiceType == null) {
                                msg = "";
                                msg += "<br />Please select Invoice Type";
                                $("#lblInvoiceBDMsg").html(msg);
                            }
                            else {
                                block.html("Adding balance dues to invoice...");
                                $.post(SiteRoot + "ArgInvoicesBD/AddBalanceDuesToInvoice?bdIds=" + data.BdIds + "&invoiceNo=" + selectedInvoice + "&invoiceType=" + InvoiceType + "&companyId=" + selectedCompany + "&amount=" + data.TotalPaymentAmount, null, function (data) {
                                    console.log("Returning Updated results...");
                                    console.log(data);
                                    $("#StatusResults").html("");
                                    msg = "";
                                    if (isEmpty(data.Message)) {
                                        block.html("<div class='message' style='font-size: 18px;color: green;'>" + data.InvoicesBDCount + " Balance Dues added to Invoice<br/> " + data.InvoicesCommissionCount + " Invoices Commissions added <br/>" + data.UpdatedBDCount + " Balance Dues updated<br />" + data.CollectionCommentCount + " Records added in Collection Comments</div>");
                                    }
                                    else {
                                        block.html(data.Message);
                                    }
                                });
                            }
                        });
                        $("#ProceedNo").click(function () {
                            $("#StatusResults").html("");
                            msg = "";
                            $("#lblInvoiceBDMsg").html(msg);
                            $('#SearchOptions_AddBDToInvoice').prop('checked', false);
                        });
                    }
                }
            });
        }
    }
    else {
        $("#StatusResults").hide();
        $("#lblInvoiceConvMsg").html("");
    }
});
$("#GenerateClientInvoicePdf").click(function () {
    var invoiceNo = $("#ddlInvoiceNo").chosen().val();
    var CompanyId = $("#ddlArgInvoiceCompany").chosen().val();
    if (invoiceNo.length <= 0) {
        $("#lblInvoiceBDMsg").html("Please select Invoice#!");
        return false;
    }
    $("#message").html("Generating....");
    $.get(SiteRoot + "ArgInvoicesBD/GenerateInvoicePDF?invoiceNo=" + invoiceNo + "&companyId=" + CompanyId + "&generatePdf=True", null, function (data) {
        console.log(data);
        var html = "<h4><a target='_blank' class='viewInvoice' href='" + data.InvoiceUrl + "'>View Invoice</a></h4>";
        $("#message").html(html);
    });
});
$("#SearchOptions_DisplayDetails").change(function () {
    if ($('#SearchOptions_DisplayDetails').is(":checked")) {
        $("#lblInvoiceBDMsg").html("");
        var invoiceNo = $("#ddlInvoiceNo").chosen().val();
        var companyId = $("#ddlArgInvoiceCompany").chosen().val();
        if (invoiceNo == null) {
            //if (invoiceNo.length <= 0) {
            $("#lblInvoiceBDMsg").html("Please select Invoice#!");
            $('#SearchOptions_DisplayDetails').prop('checked', false);
            return false;
        }
        var url = SiteRoot + "ArgInvoicesBD/GenerateInvoicePDF?invoiceNo=" + invoiceNo + "&companyId=" + companyId + "&generatePdf=false" + "&displayDetails=true";
        console.log(url);
        $("#message").html("Loading...");
        //window.open(url, "_blank");
        $.get(url, null, function (data) {
            $("#message").html(data);
        });
    }
    else {
        $("#message").html("");
    }
});
function setInvoiceDueDate(invoiceDate) {
    if (invoiceDate !== "") {
        var invoiceDueDate = new Date(invoiceDate).addDays(terms);
        $("#InvoiceDetail_DueDate").datepicker('setDate', invoiceDueDate);
    }
}

function loadInvoiceNos() {
    var companyIds = $("#ddlArgInvoiceCompany").chosen().val();
    var regions = $("#ddlRegions").chosen().val();
    var invoiceTypes = $("#ddlInvoiceTypes").chosen().val();

    var data = {
        companyIds: companyIds,
        regions: regions,
        invoiceTypes: invoiceTypes,
    };
    $.post(SiteRoot + "ArgInvoicesBD/LoadInvoiceNos", data, function (html) {
        var ddlInvoiceNo = $("#ddlInvoiceNo");
        var selectedInvoiceNos = ddlInvoiceNo.val();
        ddlInvoiceNo.empty(); 
        $.each(html, function (i, item) {
            ddlInvoiceNo.append('<option value="' + item.value + '">' + item.text + '</option>');
        });
        ddlInvoiceNo.val(selectedInvoiceNos).trigger("chosen:updated");
    });
}