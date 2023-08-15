// Function to handle the 'Tôi đang làm việc ở đây' checkbox
$("#iam_work_here").on("change", function () {
    checkIamWorkingHere();
});

// Function to check if 'Tôi đang làm việc ở đây' checkbox is checked and show/hide the end date fields accordingly
function checkIamWorkingHere() {
    const isWorkingHere = $("#iam_work_here").is(":checked");
    (isWorkingHere) ? $('#EndDate').hide() : $('#EndDate').show();
}

// Function to clear the form fields
function clearFormExperience() {
    $("#CompanyName").val("");
    $("#Position").val("");
    $("#StartMonth").val("");
    $("#StartYear").val("");
    $("#EndMonth").val("");
    $("#EndYear").val("");
    $("#Description").val("");
    $("#iam_work_here").prop("checked", false);
    $("#form-update-experience .btn-red-outline").hide();
    checkIamWorkingHere();
}

$('#form-update-experience').on('submit', function (e) {
    e.preventDefault();

    if (checkValidExperience()) {
        const $formUpdateExperience = $("#form-update-experience");

        const isWorkingHere = $("#iam_work_here").is(":checked");

        const endMonth = isWorkingHere ? 0 : parseInt($formUpdateExperience.find("#EndMonth").val());
        const endYear = isWorkingHere ? 0 : parseInt($formUpdateExperience.find("#EndYear").val());

        let data = {
            CompanyName: $formUpdateExperience.find("#CompanyName").val(),
            Position: $formUpdateExperience.find("#Position").val(),
            StartMonth: parseInt($formUpdateExperience.find("#StartMonth").val()),
            StartYear: parseInt($formUpdateExperience.find("#StartYear").val()),
            EndMonth: endMonth,
            EndYear: endYear,
            Description: $formUpdateExperience.find("#Description").val(),
        };

        // Your AJAX request here
        // ...

        // After successful AJAX request, you can call the following functions:
        // GetExperience();
        // showMessage("Thêm kinh nghiệm thành công");
        // $('#modal-update-experience').modal('hide');
        // clearFormExperience();
    } else {
        console.log("nolooooo");
    }
    return false;
});

// Function to check if the experience form fields are valid
const checkValidExperience = () => {
    var isValid = true;

    // Clear old error messages
    $("#modal-update-experience .error-message").empty();

    // Check each field and display error messages if needed
    if ($("#modal-update-experience #CompanyName").val() === "") {
        $("#company-name-error").text("Company Name is required");
        isValid = false;
    }
    if ($("#modal-update-experience #Position").val() === "") {
        $("#position-error").text("Position is required");
        isValid = false;
    }
    if ($("modal-update-experience #StartMonth").val() === "") {
        $("#startMonth-error").text("Start Month is required");
        isValid = false;
    }
    if ($("modal-update-experience #StartYear").val() === "") {
        $("#startYear-error").text("Start Year is required");
        isValid = false;
    }

    return isValid;
};

// Function to handle the 'Tải ảnh' button click
$(".btn-svg-icon").on("click", function () {
    // Your code to handle image upload here
    // ...
});
