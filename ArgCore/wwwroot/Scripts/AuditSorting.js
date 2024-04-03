function ShowIconLoader(element) {
    var loadHtml = '<div class="imgloader"><img src="/images/busy.gif" style="left: 94%; top: 10px;width: 25px;" class=""></img></div>';
    $(loadHtml).insertAfter(element);
}
function HideIconLoader() {
    $('.imgloader').remove();
}
function Applyarrangement(element) {
    ShowIconLoader(element);
    setTimeout(function () {
        var parent = $("#Soringparentdiv");
        var Sortblocks = $(parent).find('.sortablediv');
        if (Sortblocks !== null && Sortblocks !== undefined && Sortblocks.length > 0) {
            var ob = new Object();
            ob.values = new Array();
            $(Sortblocks).each(function () {
                var value = new Object();
                value.fieldName = $(this).attr('data-div');
                value.fieldValue = $(this).attr('data-id');
                var findwidget = $(".leftBlock #"+value.fieldName+"");
                var height = 0;
                if (findwidget !== "" && findwidget.length > 0) {
                    height = findwidget[0].clientHeight;
                }
                value.height = height + "px";
                ob.values.push(value);
            });
            $.ajax({
                url: '/BOL/SaveArrangement',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(ob),
                success: function (result) {
                    HideIconLoader();
                    location.reload(true);
                },
                error: function (error) {
                    HideIconLoader();
                }
            });
        }
    }, 10);
}