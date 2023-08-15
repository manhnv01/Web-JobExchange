// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showMessage(text, type = "success", title = "Thông báo", icon = "glyphicon-ok", delay = 3000) {
   

    $.pnotify_remove_all();
    var notice = $.pnotify({
        pnotify_title: title,
        pnotify_text: text,
        type: type,
        icon: 'glyphicon ' + icon,
        addclass: 'snotify',
        sticker: false,
        pnotify_delay: delay,
        stack: {
            dir1: "down", // Hướng theo chiều dọc
            dir2: "left", // Hướng theo chiều ngang
            firstpos1: 90, // Vị trí đầu tiên theo chiều dọc
            firstpos2: 10, // Vị trí đầu tiên theo chiều ngang
            spacing1: 10, // Khoảng cách giữa các thông báo theo chiều dọc
            spacing2: 10, // Khoảng cách giữa các thông báo theo chiều ngang
            push: "bottom", // Đẩy các thông báo lên trên hoặc xuống dưới stack
            context: $("body")
        }
    }).click(function (event) {
        notice.pnotify_remove();
    });
}