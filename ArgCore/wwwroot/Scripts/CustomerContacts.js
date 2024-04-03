$("#AddCustomerContactPopUp").click(function () {
    var customerId = $(this).data("customerid");
    console.log(customerId);
    var url = SiteRoot + "CustomerContacts/Save?contactId=0&customerId=" + customerId;
    console.log(url);
    $.get(url, function (html) {
        bootbox.dialog({
            message: html, size: "large"
        }).addClass('AddCustContactDialog');
    });
});
