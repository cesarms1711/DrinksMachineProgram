$(function () {

    $("[data-field='true']").each(function () {

        if ($(this).find("span.field-validation-error").length > 0) {
            $(this).find("input, select, textarea").addClass("is-invalid");
        }

    });

    $("#ValidationSummary").children("ul").css("list-style-type", "none");
    $("#ValidationSummary").children("ul").addClass("p-0");

    $("#ValidationSummary li").each(function () {
        $(this).addClass("alert alert-danger alert-dismissible mb-2");
        $(this).prepend("<button class='btn-close' type='button' data-dismiss='alert'></button>");
    });

    $.fn.hasData = function (key) {
        return typeof $(this).data(key) !== "undefined";
    };

});