$(document).ready(function () {
    $(".floating-button").mouseenter(function () {
        showNotification();
    });

    function showNotification() {
        var notification = $("#floating-notification");
        notification.fadeIn().delay(3000).fadeOut();
    }
});

