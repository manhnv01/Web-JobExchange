
function chooseFile(fileInput) {
    // const AVATAR = document.getElementById("image");
    if (fileInput.files && fileInput.files[0]) {
        const reader = new FileReader();
        reader.readAsDataURL(fileInput.files[0]);

        reader.onload = function (e) {
            $('#change-avatar').attr('src', e.target.result);
            // AVATAR.style.background = `url(${reader.result}) center center/cover`;
        }

    }
}

$("#change-avatar").on('click', () => {
    $("#avatar_file").trigger("click");
});
$("#avatar_file").on('change', function () {
    var fileInput = this;
    if (fileInput.files.length > 0) {
        var file = fileInput.files[0];

        var formData = new FormData();
        formData.append('avatar_file', file);

        var xhr = new XMLHttpRequest();
        xhr.open('POST', './UploadAvatar', true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                var response = xhr.responseText;
                if (response == "error") {
                    toastr.error("Có lỗi sảy ra!");
                } else if (response == "empty") {
                    toastr.error("Không có file avatar!");
                } else {
                    toastr.success("Cập nhật avatar thành công.");
                }
            }
        };
        xhr.send(formData);
    }
});


function chooseCoverFile(fileInput) {
    // const AVATAR = document.getElementById("image");
    if (fileInput.files && fileInput.files[0]) {
        const reader = new FileReader();
        reader.readAsDataURL(fileInput.files[0]);

        reader.onload = function (e) {
            $('#change-cover').attr('src', e.target.result);
            // AVATAR.style.background = `url(${reader.result}) center center/cover`;
        }

    }
}

$("#change-cover").on('click', () => {
    $("#cover_file").trigger("click");
});
$("#cover_file").on('change', function () {
    var fileInput = this;
    if (fileInput.files.length > 0) {
        var file = fileInput.files[0];

        var formData = new FormData();
        formData.append('cover_file', file);

        var xhr = new XMLHttpRequest();
        xhr.open('POST', './UploadCover', true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                var response = xhr.responseText;
                if (response == "error") {
                    toastr.error("Có lỗi sảy ra!");
                } else if (response == "empty") {
                    toastr.error("Không có file cover!");
                } else {
                    toastr.success("Cập nhật cover thành công.");
                }
            }
        };
        xhr.send(formData);
    }
});