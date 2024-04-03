var BolDescItem = (function () {
    function BolDescItem() {
    }
    return BolDescItem;
}());
var BOLOtherChargesItem = (function () {
    function BOLOtherChargesItem() {
    }
    return BOLOtherChargesItem;
}());
var PaymentStats = (function () {
    function PaymentStats() {
    }
    return PaymentStats;
}());
//function GetBolCharges(oceanCharges) {
//    var bolNo = $("#BolHeaderDetails_BOLNo").val();
//    if (bolNo.length <= 0) {
//        $("#lblMessage").html("BolNo cannot be empty");
//        return false;
//    }
//    $.get(SiteRoot + "BOL/GetBolCharges?bolNo=" + bolNo + "&oceanCharges=" + oceanCharges, function (data) {
//        console.log(data);
//        return data;
//    });
//}
//TASK >> Prepopulate the form
//For the BOL Item, automatically add line and pre-populate container information, as if user clicked add new row.  But, also add AmountDue value from BOLCharges where ChargeDescription like 'Ocean%' using bolcharges.usamount value
//Prepopulate OtherCharge values by searching BOLCharges where chargedescription not like 'ocean%' and add lines on form:
//ChargeCode on form should = BOLCharges.ChargeCode
//Description on form should be blank
//AmountDue on form should = BOLCharges.USAmount
var bolNo = $("#BolHeaderDetails_BOLNo").val();
$(document).ready(function () {
    UpdateAmountPaid();
    if ($("#BolItemBlock .item_row").length == 1 || $("#BolChargesBlock .charge_row").length == 1) {
        if (bolNo.length <= 0) {
            $("#lblMessage").html("BolNo cannot be empty");
            return false;
        }
        //When pre-populating screen, make sure your adding multiple BOLItems.IF there ar multiple BOLItems, please add them when prepopulating form.Also, populate Amount Due for Item
        if ($("#BolItemBlock .item_row").length == 1) {
            $.get(SiteRoot + "BOL/GetBolItemCharges?bolNo=" + bolNo + "&oceanCharges=true", function (data) {
                if (data != '') {
                    $.each(data, function (index, item) {
                        if (data[index].BOLNo.length > 0) {
                            $("#add-bol-item-row").click();
                            $("#BolItemBlock .AddBolItem .itemsAmountDue:last").val(data[index].USAmount);
                        }
                    });
                }
            });
        }
        if ($("#BolChargesBlock .charge_row").length == 1) {
            $.get(SiteRoot + "BOL/GetBolItemCharges?bolNo=" + bolNo + "&oceanCharges=false", function (data) {
                if (data != '') {
                    $.each(data, function (index, item) {
                        if (data[index].BOLNo.length > 0) {
                            $("#add-bol-charge-row").click();
                            $("#BolChargesBlock .AddBolItem .bolOtherChargeCode:last").val(data[index].ChargeCode);
                            $("#BolChargesBlock .AddBolItem .chargesAmountDue:last").val(data[index].USAmount);
                        }
                    });
                }
            });
        }
    }

    $("#ddlBDDescs").chosen().change(function () {
        $("#ErrorDescription").val($(this).val());
    });
});

function UpdateAmountPaid() {
    if ($("#AmountPaid").val() == 0) {
        console.log("getting amount");
        $.get(SiteRoot + "BOL/GetAmountPaid?bolNo=" + bolNo + "&custId=" + $("#ddlparticipants").val(), function (data) {
            console.log("Amount Paid: " + data);
            $("#AmountPaid").val(data);
        });
    }
}
//AmountPaid: Prepopulate using sum(ARCash.AmountPaid), and PayorID matching CustomerID on form-- - so this will be updated after user selects Customer.If there's already an AmountPaid value in the form, don't change it.
$("#ddlparticipants").change(function () {
    UpdateAmountPaid();
});
$("#add-bol-item-row").click(function () {
    var idx = $("#BolItemBlock .item_row").length - 1;
    var src = $("#BolItemBlock .item_row:first");
    var html = $('<div>').append(src.clone()).html();
    html = html.replace(/\[-1\]/g, "[" + idx + "]");
    //html = html.replace(/-1/g, idx); showing wrong ItemValues which we are loading by default
    $(html).insertAfter("#BolItemBlock .item_row:last").removeClass("hidden");
    var ctrls = $("#BolItemBlock .item_row:last input:not('.hidden')");
    $.each(ctrls, function () {
        $(this).attr("required", "required");
    });
});
//>> Added "notRequired" due to below task
//Sometimes after Creating Description, the user clicks Add Balance Due, and nothing happens due to a validation error.
//This is fine, but occasionally no error is displayed, telling the user what field is causing the error.
//Please fix.I was able to determine the issue for this particular error was because the BOL Other Charges Description
// fields needed a value-- - please remove this validation as many times they don't need a description....
//and provide message to user for other validation errors.
$("#add-bol-charge-row").click(function () {
    var idx = $("#BolChargesBlock .charge_row").length - 1;
    var src = $("#BolChargesBlock .charge_row:first");
    var html = $('<div>').append(src.clone()).html();
    html = html.replace(/\[-1\]/g, "[" + idx + "]");
    $(html).insertAfter("#BolChargesBlock .charge_row:last").removeClass("hidden");
    var ctrls = $("#BolChargesBlock .charge_row:last input:not('.hidden')");
    console.log("Controls: " + ctrls.length);
    $.each(ctrls, function () {
        if ($(this).hasClass("notRequired")) {
            return;
        }
        else {
            $(this).attr("required", "required");
        }
    });
});
function roundNumber(num, decimals) {
    var newnumber = new Number(num + '').toFixed(parseInt(decimals));
    return parseFloat(newnumber);
}
$('body').on("click", "#AddBalDue", function () {
    console.log("AddBalDue clicked");
    //CHECK AMOUNT VALIDATIONS BASED ON INVOICE TYPE
    var msgCtrl = $("#lblMessage");
    var iType = $("#ddlInvoiceType").val().toLowerCase();
    var amt = $("#AmountDue").val();
    console.log("IsTouched: " + isTouched);
    if (isTouched) {
        msgCtrl.html("Please create description again, some data has been updated.");
        return false;
    }
    console.log(iType + " " + amt);
    if (iType.indexOf("under") > 0 && amt < 50) {
        msgCtrl.html("Amount must be at least 50 for underbilling");
        return false;
    }
    if (iType.indexOf("over") > 0 && amt > -50) {
        msgCtrl.html("Amount must be at least -50 for overbilling");
        return false;
    }
    return true;
});
$("#AddDes").click(function () {
    if ($("#ddlInvoiceType option:selected").val() == "") {
        $("#lblMessage").html("Please select Invoice Type");
        return;
    }
    if ($("#ddlErrorCodes option:selected").val() == "") {
        $("#lblMessage").html("Please select BDError Code");
        return;
    }
    if ($("#ddlparticipants option:selected").val() == "") {
        $("#lblMessage").html("Please select CustomerID");
        return;
    }
    $("#lblMessage").html("");
    var htmlContent = "";
    var invoiceType = $("#ddlInvoiceType option:selected").val();
    var additionalDescription = "";
    if ($("#AdditionalDescription").val() !== "") {
        additionalDescription = "<p>Additional note:<br />";
        additionalDescription += $("#AdditionalDescription").val() + "</p>";
    }

    htmlContent += "<p>" + $("#ErrorDescription").val() + additionalDescription + " The corrected charges are below.</p>";

    var rowCount = $(".item_row:not('.hidden')").length;
    console.log(rowCount);
    var stats = new PaymentStats();
    stats.AmountDue = 0;
    stats.TotalCharges = 0;
    stats.AmountPaid = 0;
    var items = new Array();
    for (var i = 0; i < rowCount; i++) {
        var item = new BolDescItem();
        item.ContainerID = $("[name='BalanceDuesItems[" + i + "].Container']:visible").val();
        item.Amount = $("[name='BalanceDuesItems[" + i + "].AmountDue']:visible").val();
        item.Commodity = $("[name='BalanceDuesItems[" + i + "].Commodity']:visible").val();
        item.CommodityDesc = $("[name='BalanceDuesItems[" + i + "].CommodityDesc']:visible").val();
        item.Due = $("[name='BalanceDuesItems[" + i + "].Due']:visible").val();
        item.Hazmat = $("[name='BalanceDuesItems[" + i + "].Hazmat']:visible").val();
        item.Measure = $("[name='BalanceDuesItems[" + i + "].Measure']:visible").val();
        item.RefNo = $("[name='BalanceDuesItems[" + i + "].RefNo']:visible").val();
        item.Size = $("[name='BalanceDuesItems[" + i + "].ContainerSize']:visible").val();
        item.TariffRef = $("[name='BalanceDuesItems[" + i + "].TariffRef']:visible").val();
        item.Type = $("[name='BalanceDuesItems[" + i + "].Type']:visible").val();
        item.Weight = $("[name='BalanceDuesItems[" + i + "].Weight']:visible").val();
        item.WeightIndicator = $("[name='BalanceDuesItems[" + i + "].WeightUnit']:visible").val();
        item.MeasureIndicator = $("[name='BalanceDuesItems[" + i + "].MeasureUnit']:visible").val();
        items.push(item);
    }
    var content = "";
    if ($(items).length > 0) {
        var htmlTable = "<table class='table table-desc'>";
        htmlTable += "<tr>";
        htmlTable += "<th>Type</th>";
        htmlTable += "<th>ContainerID</th>";
        htmlTable += "<th>Size</th>";
        htmlTable += "<th>Weight</th>";
        htmlTable += "<th>Measure</th>";
        htmlTable += "<th>Hazmat</th>";
        htmlTable += "<th>Commodity</th>";
        htmlTable += "<th>Tariff Ref#</th>";
        htmlTable += "<th>Amount Due</th>";
        htmlTable += "</tr>";

        $(items).each(function (idx, item) {
            if (typeof item.Hazmat != "undefined")
                item.Hazmat = item.Hazmat.toLowerCase() === "true" ? "Yes" : "No";

            htmlTable += AddCharges(item);
            stats.TotalCharges += roundNumber(item.Amount, 2);
            stats.AmountDue += roundNumber(item.Amount, 2);
        });

        htmlTable += "</table>";
        htmlContent += "\n\n" + htmlTable;
    }
    //LOAD CHARGES
    var charges = new Array();
    rowCount = $(".charge_row:not('.hidden')").length;
    for (var i = 0; i < rowCount; i++) {
        var charge = new BOLOtherChargesItem();
        charge.ItemId = $("[name='BalanceDuesOtherCharges[" + i + "].ItemId']:visible").val();
        charge.TariffRefNo = $("[name='BalanceDuesOtherCharges[" + i + "].TariffRefNo']:visible").val();
        charge.ChargeCode = $("[name='BalanceDuesOtherCharges[" + i + "].ChargeCode']:visible option:selected").text();
        charge.Description = $("[name='BalanceDuesOtherCharges[" + i + "].Description']:visible").val();
        charge.AmountDue = $("[name='BalanceDuesOtherCharges[" + i + "].AmountDue']:visible").val();
        charges.push(charge);
    }
    if ($(charges).length > 0) {
        htmlContent += "\n\nOther charges:\n";

        var htmlTable = "<table class='table table-desc'>";
        htmlTable += "<tr>";
        htmlTable += "<th>Charge Code</th>";
        htmlTable += "<th>Description</th>";
        htmlTable += "<th>Amount Due</th>";
        htmlTable += "</tr>";

        $(charges).each(function (idx, item) {
            htmlTable += AddOtherCharges(item);
            stats.TotalCharges += roundNumber(item.AmountDue, 2);
        });

        htmlTable += "</table>";
        htmlContent += "\n\n" + htmlTable;
    }
    stats.AmountPaid = $("#AmountPaid").val();
    stats.AmountDue = roundNumber((stats.TotalCharges - stats.AmountPaid), 2);
    htmlContent += "\n\n";

    if (invoiceType == "BOL Overcharge") {
        var htmlTable = "<table class='table table-desc'><tr><td>Total corrected charges: </td><td class='text-right' style='text-align:right'>" + roundNumber(stats.TotalCharges, 2).toLocaleString() + "</td></tr></table>";
        htmlContent += htmlTable;
    }
    else {
        var htmlTable = "<table class='table table-desc'>";
        htmlTable += "<tr><td>Total charges: </td><td class='text-right' style='text-align:right'>" + roundNumber(stats.TotalCharges, 2).toLocaleString() + "</td></tr>";
        htmlTable += "<tr><td>Amount Paid: </td><td class='text-right' style='text-align:right'>" + stats.AmountPaid.toLocaleString() + "</td></tr>";
        htmlTable += "<tr><td>Amount Due: </td><td class='text-right' style='text-align:right'>" + stats.AmountDue.toLocaleString() + "</td></tr>";
        htmlTable += "</table>";
        htmlContent += htmlTable;
    }

    $("#TotalCharges").val(stats.TotalCharges);
    $("#AmountPaid").val(stats.AmountPaid);
    $("#AmountDue").val(stats.AmountDue);
    $("#Description").val(htmlContent);
    $("#bdDescription").html(htmlContent);
    isTouched = false;
});

var isTouched = false;
$("body").on("change", ".AddBolHeader input,select, .AddBolItem input,select", function () {
    isTouched = true;
    console.log("IsTouched: " + isTouched);
});
$('body').on("change", "#ddlInvoiceType", function () {
    var invoiceType = $("#ddlInvoiceType").val();
    //LOAD ERROR CODES BASED ON INVOICE TYPE
    $.get(SiteRoot + "BalanceDues/GetBDErrorCodes?invoiceType=" + invoiceType, null, function (data) {
        var ctrl = $("#ddlErrorCodes");
        ctrl.html("");
        ctrl.append("<option value=\"\">--select--</option>");
        $.each(data, function (i, item) {
            ctrl.append("<option value=\"" + item.Description + "\">" + item.BdErrorCode + "</option>");
        });
        ctrl.trigger("chosen:updated");
    });
    //LOAD BD DESC CODES BASED ON INVOICE TYPE
    $.get(SiteRoot + "BalanceDues/GetBdDescriptions?invoiceType=" + invoiceType, null, function (data) {
        console.log(data);
        var ctrl = $("#ddlBDDescs");
        ctrl.html("");
        ctrl.append("<option value=\"\">--select--</option>");
        $.each(data, function (i, item) {
            ctrl.append("<option value=\"" + item.Description + "\">" + item.BDDescription + "</option>");
        });
        ctrl.trigger("chosen:updated");
    });
});
//Sometimes after Creating Description, the user clicks Add Balance Due, and nothing happens due to a validation error.
//This is fine, but occasionally no error is displayed, telling the user what field is causing the error.
//Please fix.I was able to determine the issue for this particular error was because the BOL Other Charges Description
// fields needed a value-- - please remove this validation as many times they don't need a description....
//and provide message to user for other validation errors.
$(document).on("change", ".bolOtherChargeCode", function () {
    var ctrl = $(this);
    console.log("Ctrl: >>");
    console.log(ctrl);
    var idx = ctrl.find('option:selected').index();
    var txtCtrl = ctrl.parent().parent().find(".otherChargeDesc");
    console.log(idx);
    if (idx <= 0) {
        txtCtrl.attr("required", "required");
        console.log("Added attr req");
    }
    else {
        txtCtrl.removeAttr("required");
        console.log("Removed attr req");
    }
});
//function MakeLengthOfString(str: string, reqlength: number, addendSpacesToEnd: boolean) {
//    var spacesToAdd = len
//    return AddSpaces(str, removeEventListener
//}
//function AddColumn(value: string, width: number) {
//    return AddSpaces(value + "\t", width);
//}
function AddSpaces(str, requiredLength, atEnd) {
    var spaces = "";
    var spaceCount = requiredLength - str.length;
    for (var i = 0; i < spaceCount; i++) {
        spaces += " ";
    }
    if (atEnd)
        str = str + spaces;
    else
        str = spaces + str;
    return str;
}
$('body').on("click", ".removeItem", function () {
    var tr = $(this).parent().parent();
    var itemId = $(this).attr("data-itemid");
    console.log($(this));
    //$.post(SiteRoot + "Bol/DeleteBdItem?itemId=" + itemId, null, function () {
    tr.remove();
    UpdateItemIndexes(".AddBolItem", "tr.item_row", "BalanceDuesItems");
    //});
});
$('body').on("click", ".removeChargeItem", function () {
    var tr = $(this).parent().parent();
    var itemId = $(this).attr("data-chargeitemid");
    console.log($(this));
    //$.post(SiteRoot + "Bol/DeleteBolOtherCharges?itemId=" + itemId, null, function () {
    tr.remove();
    UpdateItemIndexes("#BolChargesBlock", "tr.charge_row", "BalanceDuesOtherCharges");
    //});
});
$('body').on("change", "#BalanceDue_InvoiceType", function () {
    var invoiceType = $("#BalanceDue_InvoiceType").val();
    //LOAD ERROR CODES BASED ON INVOICE TYPE
    $.get(SiteRoot + "BalanceDues/GetBDErrorCodes?invoiceType=" + invoiceType, null, function (data) {
        var ctrl = $("#ddlErrorCodes");
        ctrl.html("");
        ctrl.append("<option value=\"\">--select--</option>");
        $.each(data, function (i, item) {
            ctrl.append("<option value=\"" + item.Description + "\">" + item.BdErrorCode + "</option>");
        });
        ctrl.trigger("chosen:updated");
    });
    //LOAD BD DESC CODES BASED ON INVOICE TYPE
    $.get(SiteRoot + "BalanceDues/GetBdDescriptions?invoiceType=" + invoiceType, null, function (data) {
        console.log(data);
        var ctrl = $("#ddlBDDescs");
        ctrl.html("");
        ctrl.append("<option value=\"\">--select--</option>");
        $.each(data, function (i, item) {
            ctrl.append("<option value=\"" + item.Description + "\">" + item.BDDescription + "</option>");
        });
        ctrl.trigger("chosen:updated");
    });
});
function UpdateItemIndexes(containerSelector, rowSelector, nameAttrPrefix) {
    //EXAMPLE: .AddBolItem tr.item_row:not('.hidden') - WILL BE THE OUTOUT OF FIRST LINE
    var rows = $(containerSelector + " " + rowSelector + ":not('.hidden')");
    console.log("Rows: " + rows.length);
    var idx = 0;
    $.each(rows, function () {
        var ctrls = $(this).find("[name*='" + nameAttrPrefix + "']"); //EXAMPLE VALUE: BalanceDuesItems
        console.log("Controls: " + rows.length);
        $.each(ctrls, function () {
            var item = $(this);
            var oldName = item.attr("name");
            var newName = oldName.replace(/\[[0-9]+\]/g, "[" + idx + "]");
            item.attr("name", newName);
        });
        idx++;
    });
}

function AddCharges(item) {
    var tableRow = "";
    tableRow += "<tr>";
    tableRow += "<td>" + item.Type + "</td>";
    tableRow += "<td>" + item.ContainerID + "</td>";
    tableRow += "<td>" + item.Size + "</td>";
    tableRow += "<td>" + item.Weight + " " + item.WeightIndicator + "</td>";
    tableRow += "<td>" + item.Measure + " " + item.MeasureIndicator + "</td>";
    tableRow += "<td>" + item.Hazmat + "</td>";
    tableRow += "<td>" + item.Commodity + ' ' + item.CommodityDesc + "</td>";
    tableRow += "<td>" + item.TariffRef + "</td>";
    tableRow += "<td class='text-right' style='text-align:right'>" + roundNumber(item.Amount, 2).toLocaleString() + "</td>";
    tableRow += "</tr>";
    return tableRow;
}

function AddOtherCharges(item) {
    var tableRow = "";
    tableRow += "<tr>";
    tableRow += "<td>" + item.ChargeCode + "</td>";
    tableRow += "<td>" + item.Description + "</td>";
    tableRow += "<td class='text-right' style='text-align:right'>" + roundNumber(item.AmountDue, 2).toLocaleString() + "</td>";
    tableRow += "</tr>";
    return tableRow;
}