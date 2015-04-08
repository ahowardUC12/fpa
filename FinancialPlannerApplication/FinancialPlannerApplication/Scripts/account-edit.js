$(document).ready(function () {
    $(".form-control").on("keyup", function () {
        $("#saveAccountButton").prop("disabled", false);
    });

    $("#addAccount #editAccountForm #saveAccountButton").on("click", function () {
        debugger;
        var form = $("#editAccountForm");

        $.ajax(
        {
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#addAccount").html(response);
                $("#saveAccountButton").prop("disabled", true);

            }
        });
    });

    $("#editAccount #editAccountForm #saveAccountButton").on("click", function () {
        debugger;
        var form = $("#editAccountForm");

        $.ajax(
        {
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#editAccount").html(response);
                $("#saveAccountButton").prop("disabled", true);
            }
        });
    });

});