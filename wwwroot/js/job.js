function formatDateToDdMmYyyy(date) {
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();

    return `${day}/${month}/${year}`;
}

$(document).ready(function () {
    //hideAddFilter();
    getFilterJob("City");

    loadingJob(false);
    loadingCompany(false);
    getRecruitments();
    getCompanyTop();
    function getRecruitments(filter = null, value1 = null, value2 = null) {

        var xhr = new XMLHttpRequest();

        xhr.open("POST", "./Job/GetRecruitments", true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4) {
                if (xhr.status === 200) {
                    var response = JSON.parse(xhr.responseText);
                    let listJob = document.getElementById("list-job");
                    let featureJobPage = document.getElementById("feature-job-page");
                    let paginationJob = document.getElementById("pagination-job");
                    console.log(response);
                    if (response.length <= 0) {
                        let html = `<h4 class="text-center">Không tìm thấy công việc ở điều kiện này. Vui lòng thử lại</h4>`;
                        listJob.innerHTML = html;
                        featureJobPage.hidden = true;
                        loadingJob(true);
                        return;
                    }
                    featureJobPage.hidden = false;

                    let quantityJob = response.length;
                    let number = 9;
                    let paging = Math.ceil(quantityJob / number)

                    listJob.innerHTML = "";


                    for (let i = 0; i < paging; i++) {
                        let startIndex = i * number;
                        let endIndex = Math.min(startIndex + number, quantityJob);

                        let html = `<div class="item ${(i == 0) ? 'active' : ''}">`;

                        for (let j = startIndex; j < endIndex; j++) {
                            // Thêm các bản ghi vào HTML của mỗi trang ở đây
                            let item = response[j];
                            html += `
                                <div class="col-md-4 col-sm-6 feature-job job-ta">
                                    <div class="feature-job-item">
                                        <div class="cvo-flex">
                                            <a href="../Job/DefaultJob/${item.Recruitment.RecruitmentId}"
                                                target="_blank" tabindex="0">
                                                <div class="box-company-logo">
                                                    <div class="avatar">
                                                        <img src="../images/companies/${item.Recruitment.Company.Avatar}"
                                                                alt="${item.Recruitment.Company.CompanyName}"
                                                                class="img-responsive">
                                                    </div>
                                                </div>
                                            </a>
                                            <div class="col-title cvo-flex-grow">
                                                <a href="../Job/DefaultJob/${item.Recruitment.RecruitmentId}"
                                                    target="_blank" data-toggle="tooltip" title=""
                                                    data-placement="top" data-container="body"
                                                    class="title text_ellipsis"
                                                    data-original-title="${item.Recruitment.RecruitmentTitle}"
                                                    tabindex="0">
                                                    <strong class="job_title">
                                                        ${item.Recruitment.RecruitmentTitle}
                                                    </strong>
                                                </a>
                                                <a href="../Company/Detail/${item.Recruitment.CompanyId}"
                                                    target="_blank" data-toggle="tooltip" title=""
                                                    data-placement="top" data-container="body"
                                                    class="text-silver company text_ellipsis company_name"
                                                    data-original-title="${item.Recruitment.Company.CompanyName}"
                                                    tabindex="0">
                                                    ${item.Recruitment.Company.CompanyName}
                                                </a>
                                                <div class="salary">
                                                    <span style="font-size: 12px;">Ngày đăng: ${item.Recruitment.CreatedAt}</span>
                                                </div>
                                                <div class="salary">
                                                    <span  style="font-size: 12px;">Ngày hết hạn: ${item.Recruitment.ExpirationDate}</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="box-footer">
                                            <div class="col-job-info">
                                                <div class="salary">
                                                    <span class="text_ellipsis">${formatSalary(item.Recruitment.Salary)}</span>
                                                </div>
                                                <div data-html="true" data-toggle="tooltip" title=""
                                                        data-placement="top" data-container="body" class="address"
                                                        data-original-title="">
                                                        ${item.Recruitment.City}
                                                    <span class="text_ellipsis"></span>
                                                </div>
                                            </div>
                                            <div class="col-like">`;
                            if ($("#isLogin").val() == "abc") {
                                if (item.SaveRecruitment == null) {

                                    html += `<a href="javascript:void(0)" class="btn-save save" onclick="saveJob('${item.Recruitment.RecruitmentId}', this)"> <i class="fa-regular fa-heart"></i> </a>`;
                                } else {
                                    html += ` <a href="javascript:void(0)" onclick="unSave('${item.Recruitment.RecruitmentId}', this)" class="btn-unsave text-highlight" ><i class="fa-solid fa-heart"></i> </a>`;

                                }
                            }
                                       
                                        html += `</div>
                                        </div>

                                    </div>
                                </div>`
                        }
                        html += `</div>`;
                        listJob.insertAdjacentHTML("beforeend", html);

                    }
                    loadingJob(true);

                    //pagination
                    let html = `<span class="hight-light" id="current-job-page">1</span> / ${paging} trang`;
                    //console.log(paging);
                    paginationJob.innerHTML = html;

                    //"beforebegin": Trước thẻ element.
                    //"afterbegin": Bên trong thẻ element, trước tất cả các thẻ con của nó.
                    //"beforeend": Bên trong thẻ element, sau tất cả các thẻ con của nó.
                    //"afterend": Sau thẻ element.

                } else {
                    console.error("Request error:", xhr.status, xhr.statusText);
                    showMessage("Có lỗi sảy ra!", "error", "Thông báo", "glyphicon-remove", 3000);
                }
            }
        };
        var formData = new FormData();
        if (filter != null) {
            formData.append('filter', filter);
            formData.append('value1', value1);
            formData.append('value2', value2);
        }
        xhr.send(formData);
    }
    $('#carousel-job').on('slide.bs.carousel', function onSlide(event) {
        let activeSlide = event.relatedTarget; // Lấy slide hiện tại
        let activeIndex = Array.from(activeSlide.parentElement.children).indexOf(activeSlide);
        updatePagination(activeIndex);
    })
    function updatePagination(activeIndex) {
        let currentJobPage = document.getElementById("current-job-page");
        currentJobPage.innerText = activeIndex + 1;
    }

    let filterFeatureJob = document.getElementById("filter-feature-job");
    filterFeatureJob.addEventListener("change", function (e) {
        let filter = e.target.value;
        getFilterJob(filter);
    });

    let filterDistricts = document.getElementById("districts");
    let filterSalarys = document.getElementById("salaries");
    let filterExperiences = document.getElementById("experiences");
    let filterIndustryes = document.getElementById("industryes");

    filterDistricts.addEventListener("change", function (e) {
        //console.log(e.target.value);
        getRecruitments("districts", e.target.value);
    });
    filterSalarys.addEventListener("change", function (e) {
        let value = e.target.value;
        let result = value.split("-");
        getRecruitments("salaries", result[0], result[1]);
    });
    filterExperiences.addEventListener("change", function (e) {
        let value = e.target.value;
        let result = value.split("-");
        getRecruitments("experiences", result[0], result[1]);
    });
    filterIndustryes.addEventListener("change", function (e) {
        getRecruitments("industryes", e.target.value);
    });



    function getFilterJob(filter) {
        hideAddFilter();
        $(`#${filter}`).show();
    }
    function hideAddFilter() {
        $(`#City`).hide();
        $(`#Salary`).hide();
        $(`#Experience`).hide();
        $(`#Industry`).hide();
    }
    function loadingJob(status) {
        $(".loadingg").hide();
        $("#carousel-job").hide();
        if (status) {
            $("#carousel-job").show();
        } else {
            $(".loadingg").show();
        }
    }
    function loadingCompany(status) {
        $("#loading-company").hide();
        $("#carousel-company").hide();
        if (status) {
            $("#carousel-company").show();
        } else {
            $("#loading-company").show();
        }
    }
    function formatSalary(salary) {
        if (isNaN(salary)) {
            return "0 triệu";
        }

        const million = 1000000;

        if (salary >= million) {
            const millionPart = Math.floor(salary / million);
            const thousandPart = Math.floor((salary % million) / 1000);
            const thousandString = thousandPart > 0 ? `,${thousandPart}` : "";
            return `${millionPart}${thousandString} triệu`;
        } else {
            const thousandPart = Math.floor(salary / 1000);
            const thousandString = thousandPart > 0 ? `,${thousandPart}` : "";
            return `${thousandString} nghìn`;
        }
    }

    ///company
    function getCompanyTop() {
        var xhr = new XMLHttpRequest();
        loadingCompany(false);
        xhr.open("POST", "./Job/GetTopCompaniesWithRecruitmentCount", true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4) {
                if (xhr.status === 200) {
                    var response = JSON.parse(xhr.responseText);
                    let listCompany = document.getElementById("list-company");
                    console.log(response);
                    if (response.length <= 0) {
                        let html = `<h4 class="text-center">Không có công ty nào trên hệ thống</h4>`;
                        listJob.innerHTML = html;
                        featureJobPage.hidden = true;
                        loadingCompany(true);
                        return;
                    }

                    let quantityCompany = response.length;
                    let number = 4;
                    let paging = Math.ceil(quantityCompany / number)

                    listCompany.innerHTML = "";


                    for (let i = 0; i < paging; i++) {
                        let startIndex = i * number;
                        let endIndex = Math.min(startIndex + number, quantityCompany);

                        let html = `<div class="row item ${(i == 0) ? 'active' : ''}" >`;

                        for (let j = startIndex; j < endIndex; j++) {
                            let item = response[j];
                            html += `
                                <div class="col-md-3 col-sm-6" style="width: 270px; margin-right: 20px;">
                                    <div class="top-company--item">
                                        <a href="../Company/Detail/${item.Company.CompanyId}">
                                            <img src="../Images/companies/${item.Company.Avatar}" alt="Logo">
                                        </a>
                                        <a href="../Company/Detail/${item.Company.CompanyId}"
                                           data-toggle="tooltip" title="${item.Company.CompanyName}">
                                            <h4 class="top-company__title">${item.Company.CompanyName}</h4>
                                        </a>
                                        <label for="" class="top-company__badge">${item.RecruitmentCount} việc làm</label>
                                        <div data-id="31515" class="top-company__wrap-btn follow">
                                            
                                        </div>
                                    </div>
                                </div>`
                        }
                        html += `</div>`;
                        listCompany.insertAdjacentHTML("beforeend", html);

                    }
                    loadingCompany(true);

                } else {
                    console.error("Request error:", xhr.status, xhr.statusText);
                    showMessage("Có lỗi sảy ra!", "error", "Thông báo", "glyphicon-remove", 3000);
                }
            }
        };

        xhr.send();
    }
    $('#frm-search-job').on("submit", function () {
        var inputSearchText = $('#frm-search-job #keyword').val();
        getRecruitments("title", inputSearchText);

        var targetElement = document.getElementById('box-feature-jobs');
        if (targetElement) {
            targetElement.scrollIntoView({ behavior: 'smooth' });
        }
    });
});