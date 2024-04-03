var BolDescItemCeva = (function () {
    function BolDescItemCeva() {
    }
    return BolDescItemCeva;
}());
var BOLOtherChargesItemCeva = (function () {
    function BOLOtherChargesItemCeva() {
    }
    return BOLOtherChargesItemCeva;
}());
var PaymentStatsCeva = (function () {
    function PaymentStatsCeva() {
    }
    return PaymentStatsCeva;
}());
//TASK >> Prepopulate the form
//For the BOL Item, automatically add line and pre-populate container information, as if user clicked add new row.  But, also add AmountDue value from BOLCharges where ChargeDescription like 'Ocean%' using bolcharges.usamount value
//Prepopulate OtherCharge values by searching BOLCharges where chargedescription not like 'ocean%' and add lines on form:
//ChargeCode on form should = BOLCharges.ChargeCode
//Description on form should be blank
//AmountDue on form should = BOLCharges.USAmount
var bolNo = $("#BookingHeaderDetails_BOLNo").val();
console.log(bolNo);
$(document).ready(function () {
    UpdateAmountPaid();
    if ($("#BolItemBlock .item_row").length == 1 || $("#BolChargesBlock .charge_row").length == 1) {
        if (bolNo.length <= 0) {
            $("#lblMessage").html("BolNo cannot be empty");
            return false;
        }
    }
});
function UpdateAmountPaid() {
    if ($("#AmountPaid").val() == 0) {
        console.log("getting amount");
        $.get(SiteRoot + "Booking/GetAmountPaid?bolNo=" + bolNo + "&custId=" + $("#ddlparticipants").val(), function (data) {
            console.log("Amount Paid: " + data);
            $("#AmountPaid").val(data);
        });
    }
}
//AmountPaid: Prepopulate using sum(ARCash.AmountPaid), and PayorID matching CustomerID on form-- - so this will be updated after user selects Customer.If there's already an AmountPaid value in the form, don't change it.
$("#ddlparticipants").change(function () {
    UpdateAmountPaid();
});
//Adding Item row
//optional Class is added to stop required validation
$('body').on("click", "#add-bol-item-row", function () {
    var idx = $("#BolItemBlock .item_row").length - 1;
    var src = $("#BolItemBlock .item_row:first");
    var html = $('<div>').append(src.clone()).html();
    html = html.replace(/\[-1\]/g, "[" + idx + "]");
    $(html).insertAfter("#BolItemBlock .item_row:last").removeClass("hidden");
    var ctrls = $("#BolItemBlock .item_row:last input:not('.hidden, .optional')");
    $.each(ctrls, function () {
        $(this).attr("required", "required");
    });
});
//>> Added "notRequired" due to below task
//Sometimes after Creating Description, the user clicks Add Balance Due, and nothing happens due to a validation error.
//This is fine, but occassionally no error is displayed, telling the user what field is causing the error. 
//Please fix.I was able to determine the issue for this particular error was because the BOL Other Charges Description
// fields needed a value-- - please remove this validation as many times they don't need a description....
//and provide message to user for other validation errors.
//Adding Item row
$('body').on("click", "#add-bol-charge-row", function () {
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
$("#AddBalDue").click(function () {
    console.log("Add Bal. Due is clicked");
    console.log("IsTouched: " + isTouched);
    if (isTouched) {
        $("#lblMessage").html("Please create description again, some data has been updated.");
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
    var bdDesc = "";
    if ($("#ddlBDDescs").val().length > 0) {
        bdDesc = $("#ddlBDDescs").val();
    }
    var errCode = $("#ddlErrorCodes").val();
    var fullContent = "";
    if (bdDesc.length > 0) {
        fullContent = bdDesc + "\n\n";
    }
    fullContent += errCode + ".  Corrected charges are below.\n\n";
    var rowCount = $(".item_row").length - 1;
    console.log(rowCount);
    var stats = new PaymentStatsCeva();
    stats.AmountDue = 0;
    stats.TotalCharges = 0;
    stats.AmountPaid = 0;
    var items = new Array();
    for (var i = 0; i < rowCount; i++) {
        var item = new BolDescItemCeva();
        item.ContainerID = $("[name='BalanceDuesItems[" + i + "].Container']").val();
        item.Type = $("[name='BalanceDuesItems[" + i + "].Type']").val();
        console.log(item.Type);
        item.Amount = $("[name='BalanceDuesItems[" + i + "].AmountDue']").val();
        item.Commodity = $("[name='BalanceDuesItems[" + i + "].Commodity']").val();
        item.CommodityDesc = $("[name='BalanceDuesItems[" + i + "].CommodityDesc']").val();
        item.Due = $("[name='BalanceDuesItems[" + i + "].Due']").val();
        item.Hazmat = $("[name='BalanceDuesItems[" + i + "].Hazmat']").val();
        item.Measure = $("[name='BalanceDuesItems[" + i + "].Measure']").val();
        //item.RefNo = $("[name='BalanceDuesItems[" + i + "].RefNo']").val();
        item.Size = $("[name='BalanceDuesItems[" + i + "].ContainerSize']").val();
        item.TariffRef = $("[name='BalanceDuesItems[" + i + "].TariffRef']").val();
        //item.Type = $("[name='BalanceDuesItems[" + i + "].ContainerType']").val();
        item.Weight = $("[name='BalanceDuesItems[" + i + "].Weight']").val();
        item.WeightIndicator = $("[name='BalanceDuesItems[" + i + "].WeightUnit']").val();
        item.MeasureIndicator = $("[name='BalanceDuesItems[" + i + "].MeasureUnit']").val();
        item.Quantity = $("[name='BalanceDuesItems[" + i + "].Quantity']").val();
        item.GrossWeight = $("[name='BalanceDuesItems[" + i + "].GrossWeight']").val();
        item.Rate = $("[name='BalanceDuesItems[" + i + "].Rate']").val();
        items.push(item);
    }
    var content = "";
    if ($(items).length > 0) {
        var table = new AsciiTable();
        window.abc = items;
        //Checking for table heading columns whether data exists in colums or not
        var noCommodity = _.filter(items, { 'CommodityDesc': '' }).length == items.length;
        var noContainer = _.filter(items, { 'ContainerID': '' }).length == items.length;
        var noSize = _.filter(items, { 'Size': '' }).length == items.length;
        var noMeasure = _.filter(items, { 'Measure': '' }).length == items.length;
        if (!noMeasure)
            noMeasure = _.filter(items, { 'Measure': '0' }).length == items.length;
        if (!noMeasure)
            noMeasure = _.filter(items, { 'Measure': '0.00' }).length == items.length;
        var noRate = _.filter(items, { 'Rate': '' }).length == items.length;
        if (!noRate)
            noRate = _.filter(items, { 'Rate': '0' }).length == items.length;
        if (!noRate)
            noRate = _.filter(items, { 'Rate': '0.00' }).length == items.length;
        var tariffRef = _.filter(items, { 'TariffRef': '' }).length == items.length;
        var commodity = _.filter(items, { 'Commodity': '' }).length == items.length;
        var columns = [
            "Quantity", "Type", "ContainerID", "Size", "Hazmat Indicator", "Tariff Ref#", "Commodity", "Item Description", "Gross Weight", "Chargeable Weight", "Measure", "Rate", "Amount Due"];
        //getting index of empty column and deleting  that column if column is empty
        if (noContainer) {
            var idx = columns.indexOf("ContainerID");
            columns.splice(idx, 1);
        }
        if (noSize) {
            var idx = columns.indexOf("Size");
            columns.splice(idx, 1);
        }
        if (tariffRef) {
            var idx = columns.indexOf("Tariff Ref#");
            columns.splice(idx, 1);
        }
        if (commodity) {
            var idx = columns.indexOf("Commodity");
            columns.splice(idx, 1);
        }
        if (noCommodity) {
            var idx = columns.indexOf("Item Description");
            columns.splice(idx, 1);
        }
        if (noMeasure) {
            var idx = columns.indexOf("Measure");
            columns.splice(idx, 1);
        }
        if (noRate) {
            var idx = columns.indexOf("Rate");
            columns.splice(idx, 1);
        }
        //"Quantity", "Type", "ContainerID", "Size", "Hazmat Indicator", "Tariff Ref#", "Commodity", "Item Description", 
        //"Gross Weight", "Chargeable Weight", "Measure", "Rate", "Amount Due"];
        ///Alignmnent
        var idxQuantity = columns.indexOf("Quantity");
        table.setAlign(idxQuantity, AsciiTable.CENTER);
        var idxType = columns.indexOf("Type");
        table.setAlign(idxType, AsciiTable.CENTER);
        var idxContainerID = columns.indexOf("ContainerID");
        table.setAlign(idxContainerID, AsciiTable.CENTER);
        var idxSize = columns.indexOf("Size");
        table.setAlign(idxSize, AsciiTable.CENTER);
        var idxHaz = columns.indexOf("Hazmat Indicator");
        table.setAlign(idxHaz, AsciiTable.CENTER);
        var idxRate = columns.indexOf("Rate");
        table.setAlign(idxRate, AsciiTable.RIGHT);
        var idxAmount = columns.indexOf("Amount Due");
        table.setAlign(idxAmount, AsciiTable.RIGHT);
        //END Alignment
        //Filling Columns in table(Heading)
        table.setHeading(columns);
        $(items).each(function (idx, item) {
            if (typeof item.Hazmat != "undefined")
                item.Hazmat = item.Hazmat.toLowerCase() == "true" ? "Yes" : "No";
            //Pushing elements by Checking Header
            console.log(item.Type);
            var row = [];
            row.push(item.Quantity);
            row.push(item.Type);
            if (!noContainer)
                row.push(item.ContainerID);
            if (!noSize)
                row.push(item.Size);
            row.push(item.Hazmat);
            if (!tariffRef)
                row.push(item.TariffRef);
            if (!commodity)
                row.push(item.Commodity);
            if (!noCommodity)
                row.push(item.CommodityDesc);
            row.push(item.GrossWeight + ' ' + item.WeightIndicator);
            row.push(item.Weight + ' ' + item.WeightIndicator);
            //row.push(item.WeightIndicator);
            if (!noMeasure) {
                if (item.Measure != "0")
                    row.push(item.Measure + ' ' + item.MeasureIndicator);
            }
            if (!noRate) {
                if (item.Rate == "0")
                    row.push("");
                else
                    row.push(item.Rate);
            }
            row.push(Number(item.Amount).toFixed(2));
            //[item.Quantity, item.Type, item.ContainerID,
            //    item.Size,
            //    item.Hazmat,
            //    item.TariffRef,
            //    item.Commodity, item.CommodityDesc,
            //    item.WeightIndicator, item.Measure, item.MeasureIndicator,
            //    item.GrossWeight, item.Weight, item.Rate,
            //    item.Amount]
            //Check for table row columns
            //var remainingrows = [];
            //if (noCommodity) {
            //    var idxrow = columns.indexOf("Item Description");
            //    row.splice(idxrow, 1);
            //    //remainingrows.push(row[idxrow]);
            //}
            //if (noContainer) {
            //    var idxrow = columns.indexOf("ContainerID");
            //    row.splice(idxrow, 1);
            //    //remainingrows.push(row[idxrow]);
            //}
            //if (noSize) {
            //    var idxrow = removerow.indexOf("Size");
            //    row.splice(idxrow, 1);
            //    //remainingrows.push(row[idxrow]);
            //}
            //if (noMeasure) {
            //    var idxrow = removerow.indexOf("Measure");
            //    row.splice(idxrow, 1);
            //    idxrow = removerow.indexOf("Measure Indicator");
            //    row.splice(idxrow, 1);
            //    //remainingrows.push(row[idxrow]);
            //}
            //if (noRate) {
            //    var idxrow = removerow.indexOf("Rate");
            //    row.splice(idxrow, 1);
            //    //remainingrows.push(row[idxrow]);
            //}
            //if (tariffRef) {
            //    var idxrow = removerow.indexOf("Tariff Ref#");
            //    row.splice(idxrow, 1);
            //    //remainingrows.push(row[idxrow]);
            //}
            //if (commodity) {
            //    var idxrow = removerow.indexOf("Commodity");
            //    row.splice(idxrow, 1);
            //    //remainingrows.push(row[idxrow]);
            //}
            //row = row.filter(function (val) {
            //    return remainingrows.indexOf(val) == -1;
            //});
            table.addRow(row);
            stats.TotalCharges += roundNumber(item.Amount, 2);
            stats.AmountDue += roundNumber(item.Amount, 2);
            console.log("Stats: >>> ");
            console.log(stats);
        });
        //fullContent += content;
        // table.setAlign(8, AsciiTable.RIGHT);
        //table.setAlign(4, AsciiTable.RIGHT);
        //fullContent += CreateTable(content);
        fullContent += table.toString();
    }
    //LOAD CHARGES
    var charges = new Array();
    rowCount = $(".charge_row").length - 1;
    for (var i = 0; i < rowCount; i++) {
        var charge = new BOLOtherChargesItemCeva();
        charge.ItemId = $("[name='BalanceDuesOtherCharges[" + i + "].ItemId']").val();
        charge.TariffRefNo = $("[name='BalanceDuesOtherCharges[" + i + "].TariffRefNo']").val();
        charge.ChargeCode = $("[name='BalanceDuesOtherCharges[" + i + "].ChargeCode']").val();
        charge.Description = $("[name='BalanceDuesOtherCharges[" + i + "].Description']").val();
        charge.AmountDue = $("[name='BalanceDuesOtherCharges[" + i + "].AmountDue']").val();
        charge.Currency = $("[name='BalanceDuesOtherCharges[" + i + "].Currency']").val();
        charge.AmountPaid = $("[name='BalanceDuesOtherCharges[" + i + "].AmountPaid']").val();
        charges.push(charge);
        console.log(charge);
    }
    if ($(charges).length > 0) {
        fullContent += "\n\nOther charges:\n";
        content = "";
        //Check for table heading columns whether data exists in colums or not
        var notariffRefNo = _.filter(charges, { 'TariffRefNo': '' }).length == charges.length;
        var nodescription = _.filter(charges, { 'Description': '' }).length == charges.length;
        var chargecolumns = [
            "TariffRefNo", "Charge Code", "Description", "Amount Due"];
        //getting index of empty column and deleting  that column if column is empty
        if (notariffRefNo) {
            var idx = chargecolumns.indexOf("TariffRefNo");
            chargecolumns.splice(idx, 1);
        }
        if (nodescription) {
            var idx = chargecolumns.indexOf("Description");
            chargecolumns.splice(idx, 1);
        }
        table = new AsciiTable();
        //Filling Columns in table(Heading)
        table.setHeading(chargecolumns);
        //content += AddSpaces("Charge Code", 50, true) + "\t";
        //content += AddSpaces("Description", 30, true) + "\t";
        //content += AddSpaces("Amount Due", 20, false) + "\n";
        $(charges).each(function (idx, item) {
            var chargerow = [];
            //item.TariffRefNo, item.ChargeCode,
            //item.Description,
            //item.AmountDue
            //chargerow.push(item.Quantity);
            //chargerow.push(item.Type);
            //Pushing elements by Checking Header
            if (!notariffRefNo)
                chargerow.push(item.TariffRefNo);
            chargerow.push(item.ChargeCode);
            if (!nodescription)
                chargerow.push(item.Description);
            chargerow.push(Number(item.AmountDue).toFixed(2));
            table.addRow(chargerow);
            //content += AddSpaces(item.ChargeCode, 50, true) + "\t";
            //content += AddSpaces(item.Description, 30, true) + "\t";
            //content += AddSpaces(item.AmountDue.toString(), 20, false) + "\n";
            stats.TotalCharges += roundNumber(item.AmountDue, 2);
            //console.log("Stats: >>> ");
            console.log("TC: " + stats.TotalCharges);
        });
        table.setAlign(1, AsciiTable.RIGHT);
        table.setAlign(2, AsciiTable.RIGHT);
        fullContent += table.toString();
        //fullContent += content;
        content = "";
    }
    stats.AmountPaid = $("#AmountPaid").val();
    stats.AmountDue = roundNumber((stats.TotalCharges - stats.AmountPaid), 2);
    console.log("Amnt Due: " + stats.AmountDue);
    fullContent += "\n\n";
    table = new AsciiTable();
    table.addRow("Total charges: ", roundNumber(stats.TotalCharges, 2));
    table.addRow("Amount paid: ", stats.AmountPaid);
    table.addRow("Amount due: ", stats.AmountDue);
    table.setAlign(1, AsciiTable.RIGHT);
    fullContent += table.toString();
    //content += AddSpaces("Total charges:", 15, true) + "\t" + AddSpaces(roundNumber(stats.TotalCharges, 2).toString(), 10, false) + "\n";
    //content += AddSpaces("Amount paid:", 15, true) + "\t" + AddSpaces(roundNumber(stats.AmountPaid, 2).toString(), 10, false) + "\n";
    //content += AddSpaces("Amount due:", 15, true) + "\t" + AddSpaces(roundNumber(stats.AmountDue, 2).toString(), 10, false) + "\n";
    //fullContent += content;
    $("#TotalCharges").val(stats.TotalCharges);
    $("#AmountPaid").val(stats.AmountPaid);
    $("#AmountDue").val(stats.AmountDue);
    $("#Description").val(fullContent);
    isTouched = false;
});
var isTouched = false;
$("body").on("change", ".AddBolHeader input,select, .AddBolItem input,select", function () {
    isTouched = true;
    console.log("IsTouched: " + isTouched);
});
//Sometimes after Creating Description, the user clicks Add Balance Due, and nothing happens due to a validation error.
//This is fine, but occassionally no error is displayed, telling the user what field is causing the error. 
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
// Removing Item Row
$('body').on("click", ".removeItem", function () {
    debugger;
    var tr = $(this).parent().parent();
    var itemId = $(this).attr("data-itemid");
    console.log($(this));
    $.post(SiteRoot + "Bol/DeleteBdItem?itemId=" + itemId, null, function () {
        tr.remove();
        UpdateItemIndexes(".AddBolItem", "tr.item_row", "BalanceDuesItems");
    });
});
// Removing Charge Row
$('body').on("click", ".removeChargeItem", function () {
    debugger;
    var tr = $(this).parent().parent();
    var itemId = $(this).attr("data-chargeitemid");
    console.log($(this));
    $.post(SiteRoot + "Bol/DeleteBolOtherCharges?itemId=" + itemId, null, function () {
        tr.remove();
        UpdateItemIndexes("#BolChargesBlock", "tr.charge_row", "BalanceDuesOtherCharges");
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
