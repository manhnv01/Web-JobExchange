var idUpdate = 0;

//var imageData;
//function chooseFile(fileInput) {
//    // const AVATAR = document.getElementById("image");
//    if (fileInput.files && fileInput.files[0]) {
//        const reader = new FileReader();
//        reader.readAsDataURL(fileInput.files[0]);

//        reader.onload = function (e) {
//            imageData = e.target.result;
//           /* $('#thumbnail').attr('src', e.target.result);*/
//            // AVATAR.style.background = `url(${reader.result}) center center/cover`;
//        }

//    }
//}
function showModal() {
    var title = idUpdate == 0 ? 'Thêm mới ngành nghề' : 'Cập nhật ngành nghề';
    var btnConfirm = idUpdate == 0 ? 'Thêm mới' : 'Cập nhật';
    document.getElementById('myModalLabel').innerHTML = title;
    document.getElementById('btnConfirm').innerHTML = btnConfirm;
    $('#myModal').modal('show');
}

function createIndustry() {
    var industryName = document.getElementById("IndustryName");

    var message = document.getElementById("message");
    if (industryName.value == "") {
        message.innerHTML = "Vui lòng nhập tên ngành nghề !";
        industryName.focus();
        return;
    }
    var IndustryImage = document.getElementById("IndustryImage");
    if (IndustryImage.files.length > 0) {
        var file = IndustryImage.files[0];

        var formData = new FormData();
        formData.append('IndustryImage', file);
        formData.append('IndustryName', industryName.value);

        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Industry/Create', true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                var res = JSON.parse(this.responseText);
                if (res.message != null) {
                    message.innerHTML = res.message;
                    return;
                }
                toastr.success("Thêm ngành nghề mới thành công !");
                resetText();
                addRowToTable(res.industryId, res.industryName, res.industryImage);
                document.getElementById("btn-close").click();
            }
        };
        xhr.send(formData);
    }

    //var industryData = {
    //    IndustryName: industryName.value
    //};
    //var request = new XMLHttpRequest();
    //request.onreadystatechange = function () {
    //    if (this.readyState == 4 && this.status == 200) {
    //        var res = JSON.parse(this.responseText);
    //        if (res.message != null) {
    //            message.innerHTML = res.message;
    //            return;
    //        }
    //        toastr.success("Thêm ngành nghề mới thành công !");
    //        resetText();
    //        addRowToTable(res.industryId, res.industryName);
    //        document.getElementById("btn-close").click();
    //    }
    //};
    //request.open("POST", "/Industry/Create", true);
    //request.setRequestHeader("Content-type", "application/json");
    //request.send(JSON.stringify(industryData));
}

function updateIndustry() {
    var industryName = document.getElementById("IndustryName");
    var message = document.getElementById("message");
    if (industryName.value == "") {
        message.innerHTML = "Vui lòng nhập tên ngành nghề !";
        industryName.focus();
        return;
    }
    var IndustryImage = document.getElementById("IndustryImage");
    if (IndustryImage.files.length > 0) {
        var file = IndustryImage.files[0];

        var formData = new FormData();
        formData.append('IndustryImage', file);
        formData.append('IndustryName', industryName.value);
        formData.append('IndustryId', idUpdate);

        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Industry/Update/' + idUpdate, true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                var res = JSON.parse(this.responseText);
                if (res.message != null) {
                    message.innerHTML = res.message;
                    return;
                }
                toastr.success("Thêm ngành nghề mới thành công !");
                resetText();
                //$("tbody #2 td").eq(1).val(res.value.industryName);
                //$("tbody #2 td img").attr("src", "./images/industry/" + res.value.industryImage);
                document.getElementById(idUpdate).querySelector("td:nth-child(2)").textContent = res.value.industryName;
                document.getElementById(idUpdate).querySelector("td:nth-child(3) img").src = "/images/industry/" + res.value.industryImage;

                document.getElementById("btn-close").click();
            }
        };
        xhr.send(formData);
    }
}


function deleteIndustry(id) {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", '/Industry/Delete/' + id);
    // Set the request headers
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onload = function () {
        if (this.readyState == 4 && this.status == 200) {
            toastr.success("Xóa ngành nghề thành công.")
            document.getElementById('tblIndustry').deleteRow(document.getElementById(id).rowIndex);
        } else {
            toastr.error("Có lỗi khi xóa. Mã lỗi: " + xhr.status);
        }
    };
    xhr.onerror = function () {
        toastr.error("Đã xảy ra lỗi khi xóa bản ghi.");
    };

    // Show a confirmation dialog
    var confirmDelete = confirm("Bạn có chắc muốn xóa ngành nghề này?");
    if (confirmDelete) {
        xhr.send();
    }
}

function addRowToTable(id, name, image) {
    var table = document.getElementById("tblIndustry");
    var row = table.insertRow();
    row.id = id;

    var cell1 = row.insertCell(0);
    cell1.innerHTML = table.rows.length - 1;
    cell1.className = "col-1";

    var cell2 = row.insertCell(1);
    cell2.innerHTML = name;
    cell2.className = "col-5";

    var cell3 = row.insertCell(2);
    cell3.className = "col-3";
    var imgElement = document.createElement("img");
    imgElement.classList.add("rounded");
    imgElement.style.width = "250px";

    // Đặt thuộc tính src cho thẻ <img>
    imgElement.src = "/images/industry/" + image;

    // Thêm thẻ <img> vào cell3
    cell3.appendChild(imgElement);

    var cell4 = row.insertCell(3);
    cell4.classList.add("table-action");
    cell4.classList.add("col-3");
    cell4.innerHTML = `<a onclick="getIndustryById(` + id + `)"><i class="fal fa-pen" style="color: #000000; margin-right: 25px;"></i></a>
        <a onclick="deleteIndustry(` + id + `)"><i class="fal fa-trash" style="color: #000000;"></i></a>`;
}

function resetText() {
    document.getElementById("IndustryName").value = "";
    document.getElementById("message").innerHTML = "";
    document.getElementById("IndustryImage").value = "";
}

function getIndustryById(id) {
    var IndustryName = document.getElementById("IndustryName");

    var request = new XMLHttpRequest();
    request.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var res = JSON.parse(this.responseText);
            idUpdate = res.industryId;
            IndustryName.value = res.industryName;
            showModal();
        }
    };
    request.open("GET", `/Industry/GetById/${id}`, true);
    request.send();

}

function createOrUpdate() {
    if (idUpdate == 0) {
        createIndustry();
    } else {
        updateIndustry();
    }
}

function searchIndustries() {
    var name = document.getElementById("name").value;
    console.log("Name: " + name);
    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var industries = JSON.parse(xhr.responseText);
            var html = "";
            for (var i = 0; i < industries.length; i++) {
                var industry = industries[i];
                var STT = 1;
                STT = i + 1;
                html += `<tr id="` + industry.industryId + `">
                    <td class="col-1">` + STT + `</td>
                    <td id="industry-@item.IndustryId" class="col-5">`+ industry.industryName + `</td>
                     <td class="col-3">
                         <img class="rounded" style="width: 250px;" src="/images/industry/`+ industry.industryImage + `" alt="` + industry.industryImage + `" />
                    </td>
                    <td class="table-action col-3">
                        <a onclick="getIndustryById(` + industry.industryId + `)" data-bs-toggle="modal" data-bs-target="#myModal">
                            <i class="fal fa-pen" style="color: #000000; margin-right: 25px;"></i>
                        </a>
                        <a onclick="deleteIndustry(` + industry.industryId + `)">
                            <i class="fal fa-trash" style="color: #000000;"></i>
                        </a>
                    </td>
                </tr>`
            }
            document.getElementById("tbody").innerHTML = html;
            toastr.info("Tìm thấy: " + industries.length + " kết quả.");
        }
    };
    xhr.open("GET", '/Industry/Search?name=' + encodeURIComponent(name), true);
    xhr.send();
}

document.getElementById("search").addEventListener("click", searchIndustries);
//document.getElementById("name").addEventListener("input", searchIndustries);


// Hiển thị kết quả tìm kiếm
//function displaySearchResults(results) {
//    var searchResultsDiv = document.getElementById('searchResults');

//    // Xóa nội dung cũ
//    searchResultsDiv.innerHTML = '';

//    // Thêm kết quả mới vào div
//    for (var i = 0; i < results.length; i++) {
//        var result = results[i];
//        var resultItem = document.createElement('div');
//        resultItem.textContent = result;
//        searchResultsDiv.appendChild(resultItem);
//    }
//}
