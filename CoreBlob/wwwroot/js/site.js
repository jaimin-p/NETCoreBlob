// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {

    $.LoadingOverlaySetup({
        background: "rgba(0, 0, 0, 0.5)",
        imageAnimation: "2s rotate_right",
        imageColor: "#169ddb"
    });

    $(document).ajaxStart(function () {
        $.LoadingOverlay("show");
    });
    $(document).ajaxStop(function () {
        $.LoadingOverlay("hide");
    });

    $("#createForm").submit(function (e) {
        $.LoadingOverlay("show");
    });

    $("#editForm").submit(function (e) {
        $.LoadingOverlay("show");
    });
});