
function saveJob(id, buttonSave = null) {

    var saveJobData = {
        recruitmentId: id
    };

    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Job/Save?recruitmentId=' + id, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            //console.log(buttonSave.parentNode);
            //var parent = buttonSave.closest('.div-parent-save-job');
            var parent;
            var html = '';
            if (buttonSave != null) {
                parent = buttonSave.parentNode;
                html = `
                 <a href="javascript:void(0)" onclick="unSave('${id}', this)"
                    class="btn-unsave text-highlight" 
                    data-placement="top" data-original-title="Bỏ lưu">
                                                      
                    <i class="fa-solid fa-heart"></i>
                </a>
                `;
                parent.innerHTML = html;
            } else {
                parent = document.querySelectorAll('.div-parent-save-job');
                html = `
                <button id="un-save" class="btn btn-save save btn-topcv-text-normal btn-outline-hover" style="background:#00b14f; color: #fff" onclick="unSave('${id}')">
                    <i class="fa-regular fa-heart"></i> Đã lưu
                </button>
                `;
                for (var i = 0; i < parent.length; i++) {
                    parent[i].innerHTML = html;
                }
            }
        }
    };
    xhr.send(saveJobData);
}

function unSave(id, buttonUnSave = null) {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", '/Job/UnSave/' + id);
    // Set the request headers
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onload = function () {
        if (this.readyState == 4 && this.status == 200) {
            /*toastr.success("Bỏ lưu thành công")*/
            var parent;
            var html = "";
            if (buttonUnSave != null) {
                parent = buttonUnSave.parentNode;
                html = `
                <a href="javascript:void(0)" class="btn-save save" onclick="saveJob('${id}', this)">
                    <i class="fa-regular fa-heart"></i>
                </a>
                `;
                parent.innerHTML = html;
            } else {
                parent = document.querySelectorAll('.div-parent-save-job');
                html = `
                <button id="save" class="btn btn-save save btn-topcv-text-normal btn-outline-hover" onclick="saveJob('${id}')">
                    <i class="fa-regular fa-heart"></i> Lưu tin
                </button>
                `;
                for (var i = 0; i < parent.length; i++) {
                    parent[i].innerHTML = html;
                }
            }
            
        } else {
            toastr.error("Mã lỗi: " + xhr.status);
        }
    };
    xhr.onerror = function () {
        toastr.error("Đã xảy ra lỗi.");
    };

    xhr.send();
}



//    // Show a confirmation dialog
//    var confirmDelete = confirm("Bạn có chắc muốn xóa ngành nghề này?");
//    if (confirmDelete) {
//        xhr.send();
//    }
//}


//function resetText() {
//    document.getElementById("IndustryName").value = "";
//    document.getElementById("message").innerHTML = "";
//    document.getElementById("IndustryImage").value = "";
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

//function createOrUpdate() {
//    if (idUpdate == 0) {
//        createIndustry();
//    } else {
//        updateIndustry();
//    }
//}

//function searchIndustries() {
//    var name = document.getElementById("name").value;
//    console.log("Name: " + name);
//    var xhr = new XMLHttpRequest();
//    xhr.onreadystatechange = function () {
//        if (this.readyState == 4 && this.status == 200) {
//            var industries = JSON.parse(xhr.responseText);
//            var html = "";
//            for (var i = 0; i < industries.length; i++) {
//                var industry = industries[i];
//                var STT = 1;
//                STT = i + 1;
//                html += `<tr id="` + industry.industryId + `">
//                    <td class="col-1">` + STT + `</td>
//                    <td id="industry-@item.IndustryId" class="col-5">`+ industry.industryName + `</td>
//                     <td class="col-3">
//                         <img class="rounded" style="width: 250px;" src="/images/industry/`+ industry.industryImage + `" alt="` + industry.industryImage + `" />
//                    </td>
//                    <td class="table-action col-3">
//                        <a onclick="getIndustryById(` + industry.industryId + `)" data-bs-toggle="modal" data-bs-target="#myModal">
//                            <i class="fal fa-pen" style="color: #000000; margin-right: 25px;"></i>
//                        </a>
//                        <a onclick="deleteIndustry(` + industry.industryId + `)">
//                            <i class="fal fa-trash" style="color: #000000;"></i>
//                        </a>
//                    </td>
//                </tr>`
//            }
//            document.getElementById("tbody").innerHTML = html;
//            toastr.info("Tìm thấy: " + industries.length + " kết quả.");
//        }
//    };
//    xhr.open("GET", '/Industry/Search?name=' + encodeURIComponent(name), true);
//    xhr.send();
//}

//document.getElementById("search").addEventListener("click", searchIndustries);
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