$(document).ready(function () {
    $("#SelectedColumnIndex option:selected").removeAttr('selected');
});
$("#SelectedTable").change(function () {
    var selectedTable = $("#SelectedTable option:selected").text();
    var file = $("#File").val();
    console.log(file);
    $.get(SiteRoot + "Import/GetTablescolumnList?tableName=" + selectedTable + "&fileName=" + file, null, function (data) {
        $("#SelectedColumn").empty();
        $("#SelectedColumn").append('<option>' + "-- select Column --" + '</option>');
        $.each(data, function (i, item) {
            $("#SelectedColumn").append('<option>' + item + '</option>');
        });
    });
});
$("#SelectedColumnIndex").change(function () {
    var name = $("#SelectedColumnIndex option:selected").text();
    console.log(name);
    $("#SelectedHeaderName").val(name);
});
