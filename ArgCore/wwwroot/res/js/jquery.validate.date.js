$(function () {
    $.validator.methods.date = function (value, element) {
        //if ($.browser.webkit) {
        //    //IE ONLY
        //    console.log("Checking for IE");
        //    var d = new Date();
        //    return this.optional(element) || !/Invalid|NaN/.test(new Date(d.toLocaleDateString(value)));
        //}
        //else {
            console.log("Checking for Chrome/Firefox");
            return this.optional(element) || !/Invalid|NaN/.test(new Date(new Date().toLocaleDateString(value)));
        //}
    };
});