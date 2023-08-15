$(document).ready(function () {

    // Profile
    var birthday = new Date($("#text-birthday").text());
    var formattedBirthday = birthday.toLocaleDateString("en-GB");
    $("#text-birthday").text(formattedBirthday);
    $('#form-update-info-personal').on('submit', function (e) {
        e.preventDefault();
        if (checkValid()) {

            var data = {
                CandidateId: $("#form-update-info-personal #CandidateId").val(),
                AccountId: $("#form-update-info-personal #AccountId").val(),
                FullName: $("#form-update-info-personal #FullName").val(),
                Gender: $("#form-update-info-personal #Gender:checked").val(),
                Birthday: $("#form-update-info-personal #Birthday").val(),
                Phone: $("#form-update-info-personal #Phone").val(),
                Avatar: $("#form-update-info-personal #Avatar").val(),
            };

            var xhr = new XMLHttpRequest();
            xhr.open("POST", "./candidate/UpdateInfoPersonal", true);
            xhr.setRequestHeader("Content-Type", "application/json");
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    var response = JSON.parse(xhr.responseText);
                    // Xử lý phản hồi từ server sau khi gửi dữ liệu thành công
                    console.log(response);

                    $("#profile-name #text-fullname").text(response.fullName);

                    var dateParts = response.birthday.split("T")[0].split("-");
                    var birthday = dateParts[2] + "/" + dateParts[1] + "/" + dateParts[0];
                    $("#profile-name #text-birthday").text(birthday);
                    $("#profile-name #text-gender").text(response.gender);
                    $("#profile-name #text-phone").text(response.phone);
                    $('#modal-update-info-personal').modal('hide');
                    showMessage("Cập nhật thông tin cá nhân thành công");
                } else {
                    showMessage("Có lỗi sảy ra!", "error", "Thông báo", "glyphicon-remove", 3000);
                }
            };

            xhr.send(JSON.stringify(data));


        } else {
            console.log("nolooooo");
        }
        return false;
    });
    const checkValid = () => {
        let FullName = $('[data-valmsg-for="FullName"]').text();
        let Birthday = $('[data-valmsg-for="Birthday"]').text();
        let Phone = $('[data-valmsg-for="Phone"]').text();

        if (FullName == "" && Birthday == "" && Phone == "") {
            return true;
        } else {
            return false;
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
            xhr.open('POST', './candidate/UploadAvatar', true);
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    var response = xhr.responseText;
                    if (response == "error") {
                        showMessage("Có lỗi sảy ra!", "error", "Thông báo", "glyphicon-remove", 3000);
                    } else if (response == "empty") {
                        showMessage("Không có file avatar!", "error", "Thông báo", "glyphicon-remove", 3000);
                    } else {
                        $(".avatar").css("background-image", `url("./images/avatar/${response}?v=${Math.floor(Math.random() * (99999 - 1 + 1)) + 1}")`);
                        $("#form-update-info-personal #Avatar").val(response);
                        showMessage("Cập nhật avatar thành công");
                    }
                }
            };
            xhr.send(formData);
        }
    });

    //Education

    //Get All
    GetEducation();
    function GetEducation() {
        Loading("education", false);

        let candidateId = $("#text-candidateId").val();

        var xhr = new XMLHttpRequest();
        xhr.open("POST", "./candidate/GetAllEducation?candidateId=" + candidateId, true);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                var response = JSON.parse(xhr.responseText);
                // Xử lý phản hồi từ server sau khi gửi dữ liệu thành công
                console.log(response);

                if (response != "") {
                    $("#education .style1").hide();
                    $("#education .style2").show();

                    let html = "";
                    response.forEach(function (item) {
                        let date = (item.endMonth == 0 || item.endYear == 0) ? "Hiện tại" : item.endMonth + "/" + item.endYear;
                        html += `
                        <li class="list-item">
                            <div class="item">
                                <img src="./images/profile-icon-svg/icon-education.svg" alt="${item.schoolName}" class="item-img"> 
                                <div class="item-content">
                                    <div class="item-title cur-poiter">${item.schoolName}</div> 
                                    <div class="item-sub-title">${item.major}</div> 
                                    <div class="item-date">
                                        <span>${item.startMonth}/${item.startYear} - ${date}</span>
                                    </div> 
                                    <div class="item-description">${item.description} </div>
                                </div>
                            </div> 
                            <div class="item-action">
                                <button name="updateEducation" data-education-id="${item.educationId}" data-school-name="${item.schoolName}" 
                                        data-major="${item.major}" data-start-month="${item.startMonth}" data-start-year="${item.startYear}"
                                        data-end-month="${item.endMonth}" data-end-year="${item.endYear}" data-description="${item.description}"
                                class="btn" style="min-width: 0">
                                    <i class="fa-solid fa-pencil" style="color: #00b14f;"></i>
                                </button>
                            </div>
                        </li>
                        `;
                    });
                    $("#education .list").html(html);
                    Loading("education", true);
                } else {
                    $("#education .style1").show();
                    $("#education .style2").hide();
                    Loading("education", true);
                }
            }
        };

        xhr.send();
    }
    $("#iam_studying_here").on("change", function () {
        checkIamStudyingHere();
    });
    $("#add-education").on('click', function () {
        clearFormEducation();
    });
    function checkIamStudyingHere() {
        const isStudyingHere = $("#iam_studying_here").is(":checked");
        (isStudyingHere) ? $('#form-update-education #EndDate').hide() : $('#form-update-education #EndDate').show();
    }
    $('#form-update-education').on('submit', function (e) {

        e.preventDefault();

        if (checkValidEducation()) {
            const $formUpdateEducation = $("#form-update-education");

            const isStudyingHere = $("#iam_studying_here").is(":checked");

            const endMonth = isStudyingHere ? 0 : parseInt($formUpdateEducation.find("#EndMonth").val());
            const endYear = isStudyingHere ? 0 : parseInt($formUpdateEducation.find("#EndYear").val());
            const educationId = $formUpdateEducation.find("#EducationId").val();

            let data = {
                SchoolName: $formUpdateEducation.find("#SchoolName").val(),
                Major: $formUpdateEducation.find("#Major").val(),
                StartMonth: parseInt($formUpdateEducation.find("#StartMonth").val()),
                StartYear: parseInt($formUpdateEducation.find("#StartYear").val()),
                EndMonth: endMonth,
                EndYear: endYear,
                Description: $formUpdateEducation.find("#Description").val(),
                CandidateId: $formUpdateEducation.find("#CandidateId").val(),
            };
            
            

            var xhr = new XMLHttpRequest();
            if (educationId == "") {
                xhr.open("POST", "./candidate/AddEdudation", true);
            } else {
                data.EducationId = educationId;
                xhr.open("POST", "./candidate/UpdateEdudation", true);
            }
            xhr.setRequestHeader("Content-Type", "application/json");
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    var response = JSON.parse(xhr.responseText);
                    // Xử lý phản hồi từ server sau khi gửi dữ liệu thành công
                    GetEducation();
                    (educationId == "") ? showMessage("Thêm học vấn thành công") : showMessage("Cập nhật học vấn thành công");
                    
                    $('#modal-update-education').modal('hide');
                    clearFormEducation();
                } else {
                    showMessage("Có lỗi sảy ra!", "error", "Thông báo", "glyphicon-remove", 3000);
                }
            };

            xhr.send(JSON.stringify(data));


        } else {
            console.log("nolooooo");
        }
        return false;
    });

    //update education
    $('#education').on('click', 'button[name=updateEducation]', function () {
        clearFormEducation();
        $('#modal-update-education').modal('show');
        $("#form-update-education #EducationId").val($(this).data("education-id"));
        $("#form-update-education #SchoolName").val($(this).data("school-name"));
        $("#form-update-education #Major").val($(this).data("major"));
        $("#form-update-education #StartMonth").val($(this).data("start-month"));
        $("#form-update-education #StartYear").val($(this).data("start-year"));
        if ($(this).data("end-month") != 0) {
            $("#form-update-education #EndMonth").val($(this).data("end-month"));
            $("#form-update-education #EndYear").val($(this).data("end-year"));
        } else {
            $("#form-update-education #iam_studying_here").prop("checked", true);
            checkIamStudyingHere();
        }
        $("#form-update-education #Description").val($(this).data("description"));

        $("#form-update-education .btn-red-outline").show();
    });

    //delete education
    $("#form-update-education .btn-red-outline").on("click", function () {
        if (confirm("Bạn có chắc muốn xóa học vấn này?")) {
            const $formUpdateEducation = $("#form-update-education");
            const educationId = $formUpdateEducation.find("#EducationId").val();
            data = {
                educationId: parseInt(educationId)
            }
            var xhr = new XMLHttpRequest();
            xhr.open("POST", "./candidate/DeleteEducation/"+educationId, true);
            xhr.setRequestHeader("Content-Type", "application/json");
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    var response = JSON.parse(xhr.responseText);
                    // Xử lý phản hồi từ server sau khi gửi dữ liệu thành công
                    GetEducation();
                    showMessage("Xóa học vấn thành công");

                    $('#modal-update-education').modal('hide');
                    clearFormEducation();
                } else {
                    showMessage("Có lỗi sảy ra!", "error", "Thông báo", "glyphicon-remove", 3000);
                }
            };

            xhr.send(JSON.stringify(data));
          
        }
    });

    const checkValidEducation = () => {
        var isValid = true;

        // Xóa thông báo lỗi cũ
        $("#modal-update-education .error-message").empty();

        // Kiểm tra từng trường và hiển thị thông báo lỗi nếu cần
        if ($("#modal-update-education #SchoolName").val() === "") {
            $("#schoolName-error").text("School Name is required");
            isValid = false;
        }
        if ($("#modal-update-education #Major").val() === "") {
            $("#major-error").text("Major is required");
            isValid = false;
        }
        if ($("modal-update-education #StartMonth").val() === "") {
            $("#startMonth-error").text("Start Month is required");
            isValid = false;
        }
        if ($("modal-update-education #StartYear").val() === "") {
            $("#startYear-error").text("Start Year is required");
            isValid = false;
        }

        return isValid;
    }
    function clearFormEducation() {
        $("#form-update-education #SchoolName").val("");
        $("#form-update-education #Major").val("");
        $("#form-update-education #StartMonth").val("");
        $("#form-update-education #StartYear").val("");
        $("#form-update-education #EndMonth").val("");
        $("#form-update-education #EndYear").val("");
        $("#form-update-education #Description").val("");
        $("#form-update-education #iam_studying_here").prop("checked", false);
        $("#form-update-education .btn-red-outline").hide();
        checkIamStudyingHere();
    }

    function Loading(personal, status) {
        if (!status) {
            $(`#${personal}_default`).show();
            $(`#${personal}`).hide();
        } else {
            $(`#${personal}_default`).hide();
            $(`#${personal}`).show();
        }
        
    }
});