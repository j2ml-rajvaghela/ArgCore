
//#region View Model
var viewModel = {
    items: ko.observableArray([]),
    filter: ko.observable(""),
    filter2: ko.observable(""),
    filter3: ko.observable(""),
    filter4: ko.observable(""),
    filter5: ko.observable(""),
    filter6: ko.observable(""), // status
    search: ko.observable(""),
    sortcol: ko.observable(""),
    selecteditem: ko.observable("")
};
// use this to collect records to update
var updateModel = {
    items: ko.observableArray([])
    //items: []
}

function dbresult(rec) {
    var self = this;
    self.action = rec.action;
    self.message = rec.message;
    self.issuccessful - rec.issuccessful;
}


function getData(path, p1, p2, p3) {
    log.info("getData");
    var req = "/api/" + path;
    if (p1 != null) { req += "/" + p1; }
    if (p2 != null) {
        log.info("...p2=" + p2);
        req += "/" + p2;
    }
    if (p3 != null) { req += "/" + p3; }
    log.info("api req=" + req);
    var jqxhr = $.get(req, "", function (response) {
        getDataResponse(response);
    }, "json")
    .fail(function (response) {
        RequestFailed(response);
    });
}

function getDataX(path, callback, p1, p2, p3) {
    try {
        log.info("getDataX");
        var req = "/api/" + path;
        if (p1 != null) { req += "/" + p1; }
        if (p2 != null) { req += "/" + p2; }
        if (p3 != null) { req += "/" + p3; }
        log.info("api req=" + req);
        var jqxhr = $.get(req, "", function (response) {
            if (typeof callback == "function") callback(response);
        }, "json")
        .fail(function (response) {
            RequestFailed(response);
        });
    } catch (e) { log.info("getDataX error: " + e.message);}
}


function getAdminData(path, p1, p2, p3) {
    //log.info("getData");
    var req = "/dapi/" + path;
    if (p1 != null) { req += "/" + p1; }
    if (p2 != null) { req += "/" + p2; }
    if (p3 != null) { req += "/" + p3; }
    log.info("api req=" + req);
    var jqxhr = $.get(req, "", function (response) {
        getDataResponse(response);
    }, "json")
    .fail(function (response) {
        RequestFailed(response);
    });
}
//#endregion

//#region Data Model

// generic data model
function dataModel(n) {
    var self = this;
    self.items = ko.observableArray([]);
    self.pageSize = ko.observable(1);
    self.columns = ko.observableArray([]);
    self.count = 0;

    this.dirtyItems = ko.computed(function () {
        return ko.utils.arrayFilter(this.items(), function (item) {
            return item.dirtyFlag.isDirty();
        });
    }, this);

    this.isDirty = ko.computed(function () {
        return this.dirtyItems().length > 0;
    }, this);

    self.removeItem = function (item) {
        log.info("removeItem");
        self.items.remove(item);
    }

//    self.refresh = function () {
//        self.items.valueHasMutated();
//    }




}

// http://jsfiddle.net/rniemeyer/dtpfv/
ko.dirtyFlag = function (root, isInitiallyDirty) {
    var result = function () { },
        _initialState = ko.observable(ko.toJSON(root)),
        _isInitiallyDirty = ko.observable(isInitiallyDirty);

    result.isDirty = ko.computed(function () {
        return _isInitiallyDirty() || _initialState() !== ko.toJSON(root);
    });

    result.reset = function () {
        _initialState(ko.toJSON(root));
        _isInitiallyDirty(false);
    };
    result.resetDirty = function () {
        _initialState(ko.toJSON(root));
        _isInitiallyDirty(true);
    };

    return result;
};
//#endregion

//#region Cookies
function clear(x) {
    //var ele = $(event.target)[0].id;
    $(this).prev("input").val("").keypress();                   // clear input box
    $(this).prev("input").trigger("keydown", { which: 50 });    // trigger refilter
}

function setCookie(key, value) {
    var expires = new Date();
    expires.setTime(expires.getTime() + (1 * 24 * 60 * 60 * 1000));
    document.cookie = key + '=' + value + ';expires=' + expires.toUTCString() + "; path=/";
}

function getCookie(key) {
    var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
    return keyValue ? keyValue[2] : null;
}
//#endregion

//#region Common Function
function getToday() {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    return today;
}
function checkRequiredField(fld, mark) {
    var isValid = 0;
    if ($("#" + fld).val().trim().length == 0) {
        isValid = 1;
        if (mark != null && mark == true) {
            $("#" + fld).addClass("dataentryerror");
            }
            }
    return isValid;
    }
var stringStartsWith = function (string, startsWith) {
    string = string || "";
    if (startsWith.length > string.length)
        return false;
    return string.substring(0, startsWith.length) === startsWith;
};
var stringContains = function (string, stringVal) {
    string = string || "";
    log.info(string + "|" + stringVal);
    if (stringVal.length > string.length) { return false; }
    if (string.indexOf(stringVal) > -1) { return true; }
    return false;
};
//#endregion

//#region Filtering

// common filter
viewModel.filteredItems = ko.dependentObservable(function () {
    var filter = this.filter().toLowerCase();       // first name
    var filter2 = this.filter2().toLowerCase();     // last name
    var filter3 = this.filter3().toLowerCase();     // date
    var filter4 = this.filter4(); //.toLowerCase();;    // locations DDL
    var filter5 = this.filter5().toLowerCase();     // response/msgbody
    var filter6 = this.filter6().toLowerCase();     // status
    if (filter) { // firstname
        return ko.utils.arrayFilter(this.items(), function (item) {
            return stringStartsWith(item.patfirstname.toLowerCase(), filter);
        });
    } else if (filter2) { // last name
        return ko.utils.arrayFilter(this.items(), function (item) {
            return stringStartsWith(item.patlastname.toLowerCase(), filter2);
        });
    } else if (filter3) { // date
        log.info("f3=" + filter3);
        return ko.utils.arrayFilter(this.items(), function (item) {
            return stringStartsWith(item.apptdate.toLowerCase(), filter3);
        });
    } else if (filter4 && filter4.length > 0) { // location
        log.info("f4=" + filter4 + "; len=" + filter4.length);
        return ko.utils.arrayFilter(this.items(), function (item) {
            return item.locationname == filter4;
        });
    } else if (filter5) { // response
        log.info("f5=" + filter5);
        return ko.utils.arrayFilter(this.items(), function (item) {
            return stringContains(item.msgbody.toLowerCase(), filter5);
        });
    } else if (filter6) { // status
        log.info("f6=" + filter6);
        return ko.utils.arrayFilter(this.items(), function (item) {
            return item.apptstatus == filter6;
        });
    } else {
        return this.items();
    }
}, viewModel);

viewModel.uniqueLocations = ko.dependentObservable(function () {
    var locations = ko.utils.arrayMap(this.items(), function (item) { return item.locationname })
    return ko.utils.arrayGetDistinctValues(locations).sort();
}, viewModel);

viewModel.uniqueDates = ko.dependentObservable(function () {
    var dates = ko.utils.arrayMap(this.items(), function (item) { return item.apptdate })
    return ko.utils.arrayGetDistinctValues(dates).sort();
}, viewModel);

viewModel.firstMatch = ko.dependentObservable(function () {
    var search = this.search().toLowerCase();

    if (!search) {
        return null;
    } else {
    }
}, viewModel);
//#endregion

//#region MessageHandling

function getDataFailed(r) {
    log.error("data request failed for " + r);
    setMessage("data request failed for " + r);
}

function RequestFailed(r) {
    log.error("data request failed");
    setMessage("data request failed");
}

function addMessage(msgs, newmsg) {
    if (msgs.length > 0) { msgs += ";<br/>" }
    msgs += newmsg;
    return msgs;
}


// clear page message
function clearMessage(fadetime) {
    $("#divmessage").removeClass("error");
    $("#divmessage").html("");
    if (fadetime != null) {
        $('#divmessagerow').fadeOut(fadetime);
    } else {
        $("#divmessagerow").hide();
    }
}
// set page message/error
function setMessage(msg, typ) {
    log.info("msg=" + msg);
    $("#divmessage").html(msg);
    $("#divmessagerow").removeClass("hide").show();
    if (typ != null) {
        //log.info("set msg class:" + typ)
        if (typ == "error") {
            $("#divmessage").addClass("error alert-danger").show();
        }
    }
}
// set page message/error
function setModalMessage(msg, typ) {
    log.info("modal error:" + msg);
    $("#modal-error").html(msg);
    $("#modal-error").removeClass("hide").show();
    $("#modal-error").show();
    if (typ == "error") {
        $("#modal-error").removeClass("alert-success").addClass("alert-danger");
    } else {
        $("#modal-error").removeClass("alert_danger").addClass("alert-success");
    }
}

function clearModalMessage(msg, typ) {
    $("#modal-error").html("");
    $("#modal-error").hide();
    $("#modal-error").removeClass("error");
}

// set page message/error
function setDBMessage(result, div, fadetime) {
    var x = "#modal-error";
    if (div != null) { x = div;}
    log.info("msgtype=" + result.messagetype + "; fade=" + fadetime);
    log.info("msg=" + result.message);

    if (result.message != null) { $(x).html(result.message); }
    $(x).removeClass("hide").show();
    if (result.messagetype == "ok") {
        $(x).removeClass("alert-danger").addClass("alert-success");
    } else {
        $(x).removeClass("alert-success").addClass("alert-danger");
    }
    if (fadetime != null) {
        $(x).fadeOut(fadetime);
    }
}
//#endregion

//#region Program Bar
function setProgressBar(barname) {
    var bn = "progress";
    if (barname != null) { bn = barname; }
    setProgressBarStart(bn);
}

function setProgressBarStart(bn) {
    $("#" + bn + "outer").show();
    $("#" + bn).width(0).text("start");
    $("#" + bn).addClass('active').removeClass("hide");
    //    $("#" + bn).width(400);
    progress = setInterval(function () {
        var $bar = $("#" + bn);
        if ($bar.width() == 400) {
            clearInterval(progress);
            log.info("bar " + "#" + bn + " done1");
            $("#" + bn).removeClass('active hide');
            $("#" + bn + "outer").fadeOut(3000);
        } else {
            $bar.width($bar.width() + 20);
        }
        $bar.text(($bar.width() / 4).toFixed(1) + "%");
    }, 100);
}

function hideProgressBar() {
    $('#progressouter').hide();
}

function finishProgressBar(barname) {
    var bn = "progress";
    if (barname != null) { bn = barname; }
    stopProgressBar(bn);
    //clearInterval(progress);
    //$("#progressouter").removeClass("active hide");
    //$('#progress').width(400);
    //$('#progress').text("100% Done");
    //$('#progressouter').fadeOut(3000);
}
function stopProgressBar(bn) {
    clearInterval(progress);
    $("#" + bn + "outer").removeClass("active hide");
    $("#" + bn).width(400);
    $("#" + bn).text("100% Done");
    $("#" + bn + "outer").fadeOut(3000);
}
/*
function setProgressBar() {
    $('#progressouter').show();
    $('#progress').width(400).text("start");
    $('#progress').addClass('active').removeClass("hide");
    progress = setInterval(function () {
        var $bar = $('#progress');
        if ($bar.width() == 400) {
            clearInterval(progress);
            log.info("bar done1");
            $('#progress').removeClass('active hide');
            $('#progressouter').fadeOut(3000);
        } else {
            $bar.width($bar.width() + 20);
        }
        $bar.text(($bar.width() / 4).toFixed(1) + "%");
    }, 100);
}

function hideProgressBar() {
    $('#progressouter').hide();
}

function finishProgressBar(hideMessage) {
    clearInterval(progress);
    $("#progressouter").removeClass("active hide");
    $('#progress').width(400);
    $('#progress').text("100% Done");
    $('#progressouter').fadeOut(3000);
}
*/
//#endregion
