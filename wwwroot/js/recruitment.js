var idUpdate = 0;
function deleteRecruitment(id) {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", '/Recruitment/Delete/' + id);
    // Set the request headers
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onload = function () {
        if (this.readyState == 4 && this.status == 200) {
            toastr.success("Xóa tin thành công.")
            document.getElementById('tblRecruitment').deleteRow(document.getElementById(id).rowIndex);
        } else {
            toastr.error("Có lỗi khi xóa. Mã lỗi: " + xhr.status);
        }
    };
    xhr.onerror = function () {
        toastr.error("Đã xảy ra lỗi khi xóa tin.");
    };

    // Show a confirmation dialog
    var confirmDelete = confirm("Bạn có chắc muốn xóa tin tuyển dụng này?");
    if (confirmDelete) {
        xhr.send();
    }
}

//function addRowToTable(id, name) {
//    var table = document.getElementById("tblIndustry");
//    var row = table.insertRow();
//    row.id = id;

//    var cell1 = row.insertCell(0);
//    cell1.innerHTML = table.rows.length - 1;

//    var cell2 = row.insertCell(1);
//    cell2.innerHTML = name;

//    var cell3 = row.insertCell(2);
//    cell3.className = "table-action";
//    cell3.innerHTML = `<a href="#"><i class="fal fa-pen" style="color: #000000; margin-right: 25px;"></i></a>
//        <a onclick="deleteIndustry(` + id + `)"><i class="fal fa-trash" style="color: #000000;"></i></a>`;
//}

//function resetText() {
//    document.getElementById("IndustryName").value = "";
//    document.getElementById("message").innerHTML = "";
//}

//function getIndustryById(id) {
//    var IndustryName = document.getElementById("IndustryName");

//    var request = new XMLHttpRequest();
//    request.onreadystatechange = function () {
//        if (this.readyState == 4 && this.status == 200) {
//            var res = JSON.parse(this.responseText);
//            idUpdate = res.industryId;
//            IndustryName.value = res.industryName;
//            showModal();
//        }
//    };
//    request.open("GET", `/Industry/GetById/${id}`, true);
//    request.send();
//}

//function createIndustry() {
//    var industryName = document.getElementById("IndustryName");
//    var message = document.getElementById("message");
//    if (industryName.value == "") {
//        message.innerHTML = "Vui lòng nhập tên ngành nghề !";
//        industryName.focus();
//        return;
//    }

//    var industryData = {
//        IndustryName: industryName.value
//    };
//    var request = new XMLHttpRequest();
//    request.onreadystatechange = function () {
//        if (this.readyState == 4 && this.status == 200) {
//            var res = JSON.parse(this.responseText);
//            if (res.message != null) {
//                message.innerHTML = res.message;
//                return;
//            }
//            toastr.success("Thêm ngành nghề mới thành công !");
//            resetText();
//            addRowToTable(res.industryId, res.industryName);
//            document.getElementById("btn-close").click();
//        }
//    };
//    request.open("POST", "/Industry/Create", true);
//    request.setRequestHeader("Content-type", "application/json");
//    request.send(JSON.stringify(industryData));
//}

//function updateIndustry() {
//    var industryName = document.getElementById("IndustryName");
//    var message = document.getElementById("message");
//    if (industryName.value == "") {
//        message.innerHTML = "Vui lòng nhập tên ngành nghề !";
//        industryName.focus();
//        return;
//    }

//    var industryData = {
//        IndustryName: industryName.value
//    }
//    var request = new XMLHttpRequest();
//    request.onreadystatechange = function () {
//        if (this.readyState == 4 && this.status == 200) {
//            var res = JSON.parse(this.responseText);
//            if (res.message != null) {
//                message.innerHTML = res.message;
//                return;
//            }
//            else {
//                toastr.error("Có lỗi khi sửa. Mã lỗi: " + request.status);
//            }
//            document.getElementById("industry-" + res.industryId).innerHTML = res.industryName;
//            toastr.success("Sửa ngành nghề thành công !");
//            resetText();
//            document.getElementById("btn-close").click();
//        }
//    };
//    request.open("POST", '/Industry/Update/' + idUpdate, true);
//    request.setRequestHeader("Content-type", "application/json");
//    request.send(JSON.stringify(industryData));
//}



//function createOrUpdate() {
//    if (idUpdate == 0) {
//        createIndustry();
//    } else {
//        updateIndustry();
//    }
//}

function searchRecruitment() {
    var name = document.getElementById("name").value;
    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var recruitments = JSON.parse(xhr.responseText);
            var html = "";
            for (var i = 0; i < recruitments.length; i++) {
                var recruitment = recruitments[i];
                var STT = 1;
                STT = i + 1;
                html += `<tr id="` + recruitment.recruitmentId + `">
                    <td>` + STT + `</td>
                    <td>`+ recruitment.recruitmentId + `</td>
                    <td id="recuitment-@item.RecruimentId">` + recruitment.recruitmentTitle + `</td>
                    <td class="table-action">
                    <a href ="Recruitment/Details/` + recruitment.recruitmentId + `" class="ms-1 me-2">
                                        <i class="fal fa-info text-black"></i>
                                    </a>
                                    <a href ="Recruitment/Edit/` + recruitment.recruitmentId + `" class="me-2">
                                        <i class="fal fa-pen text-black"></i>
                                    </a>
                                    <a onclick="deleteRecruitment('` + recruitment.recruitmentId + `')">
                                        <i class="fal fa-trash text-black"></i>
                                    </a>
                    </td>
                </tr>`
            }
            document.getElementById("tbody").innerHTML = html;
            toastr.info("Tìm thấy: " + recruitments.length + " kết quả.");
        }
    };
    xhr.open("GET", '/Recruitment/Search?search=' + encodeURIComponent(name), true);
    xhr.send();
}

document.getElementById("search").addEventListener("click", searchRecruitment);
//document.getElementById("name").addEventListener("input", searchRecruitment);
