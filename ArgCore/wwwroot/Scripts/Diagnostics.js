$(document).ready(function () {
    var url = SiteRoot + "Diagnostics/GetDiagnosticData";
    console.log(url);
    $.get(url, function (data) {
        console.log(data);
        if (data.Name != null) {
            var html = "<div class='diagnostics'>";
            //html += "<table class='table table-bordered table-striped' style='width:100%; margin-top:30px'>";
            //html += "<thead><tr><th style='width:30%'>Name</th><th style='width:30%;'>Address</th><th style='width:20%; text-align:left'>Location</th><th style='width:50%; text-align:right'>Contact</th></thead><tbody>";
            //html += "<tr><td>" + data.Name + "</td><td>" + data.Address+ "</td><td>" + data.Location + "</td><td style='text-align:right'>" + data.Contact+ "</td></tr>";
            html += "<span class='myLabel'>Name: </span>" + data.Name + "<br/><span class='myLabel'>Address: </span>" + data.Address1;
            html += "<br/><span class='myLabel'>City: </span>" + data.City + "<br/><span class='myLabel'>State: </span>" + data.State;
            html += "<br/><span class='myLabel'>DBName: </span>" + data.DBName;
            html += "</div>";
            $("#Diagnostics").html(html);
        }
    });
});
