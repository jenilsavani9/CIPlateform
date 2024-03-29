﻿
var userProfileName;

CKEDITOR.config.toolbar_MA = [
    { name: 'basicstyles', items: ['Bold', 'Italic', 'Strike', '-', 'Subscript', 'Superscript', 'RemoveFormat'] }
];

CKEDITOR.replace('cmspageeditor');
CKEDITOR.replace('cmspageeditor1', { toolbar: 'MA' });
CKEDITOR.replace('cmspageeditor2', { toolbar: 'MA' });
$(document).ready(function () {
    makeDivActive('User')
})

var currentdate = new Date();
var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
var datetime = currentdate.toLocaleDateString("en-In", { weekday: 'long' }).toString() + ", " +
    currentdate.toLocaleDateString("en-In", { month: 'long' }).toString() + " " +
    currentdate.getDate() + ", " + currentdate.getFullYear() + ", " +
    currentdate.getHours() + ":" +
    currentdate.getMinutes()

$('#dashboard-time').html(datetime)


$('.modal').on('hidden.bs.modal', function () {
    $(this)
        .find("input:not([type=hidden]),textarea,select, input[type=file]")
        .val('')
        .end()
        .find("input[type=checkbox], input[type=radio]")
        .prop("checked", "")
        .end();

    CKEDITOR.instances.cmspageeditor.setData("");
    CKEDITOR.instances.cmspageeditor1.setData("");
    CKEDITOR.instances.cmspageeditor2.setData("");

    $('#mission-media-edit-files').addClass('d-none');
    $('#mission-docs-edit-files').addClass('d-none');

    $('#banner-media-edit-files').html("");

    //$('#profile-user-img').html('');

    $('#user-crud-email').prop('disabled', false);

    // change banner btn from edit to add
    var element = document.getElementById('banner-crud-btn');
    element.innerHTML = `<button type="button" class="btn btn-secondary mission-theme-crud-close-btn" data-bs-dismiss="modal">Close</button>
    <button type="submit" class="btn btn-primary mission-theme-crud-save-btn" data-bs-dismiss="modal" id="banner-crud-save-btn" onclick="AddBanner('add')">Save changes</button>`

    // for clear image names from mission images
    ImagesList = [];
});

function makeDivActive(ID) {
    var allDivs = document.querySelector(".dashbord-sidebar-detail");
    allDivs.classList.remove("dashbord-sidebar-detail");

    var newActiveDiv = document.querySelector("#" + ID);
    newActiveDiv.classList.add("dashbord-sidebar-detail");

    var title = document.querySelector("#selected-option-title");

    var newTitle = document.getElementById(ID).querySelector(".title-name");
    const hr = document.createElement("hr");

    var addBtn = `<button type="button" id="user-add-btn" class="btn btn-outline-warning" data-bs-toggle="modal" data-bs-target="#${ID}-Modal" style="right: 32px;position: absolute;">Add +</button>`;

    title.innerHTML = newTitle.innerHTML;
    title.innerHTML += addBtn;
    title.appendChild(hr);

    $.ajax({
        url: "/api/admin",
        type: "GET",
        success: function (result) {
            var myJsonString = JSON.stringify(result);

            // mission datatable

            if (ID == "Mission") {
                var tableDiv = document.querySelector("#admin-data-table");
                var missionTable = `
    <table class="table container table-striped table-hover" id='mission-table'>
        <thead>

            <tr>
                <th>Title</th>
                <th>Mission Type</th>
                <th>Start Date</th>
                <th>End Date</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody id="table-data-mission">
        </tbody>

    </table>`;

                tableDiv.innerHTML = missionTable;

                $(function () {
                    $.each(result.missions, function (i, item) {
                        if (result.missions.length > 0) {
                            if ($(".dataTables_empty").hasClass("dataTables_empty")) {
                                $(".dataTables_empty").hide();
                            }
                        }

                        $("#table-data-mission").append(`
                                 <tr id='${item.missionId}'>
                                     <td id="title">${item.title}</td>
                                     <td id="type">${item.missionType}
                                     </td>
                                     <td id="sDate">${item.startDate.split("T")[0]}</td>
                                     <td id="eDate">${item.endDate.split("T")[0]}</td>
                                     <td id="action"><i class="fas fa-edit" data-bs-toggle="modal" data-bs-target="#${ID}-Modal" onclick="LoadMissionData(${item.missionId})"></i> | <i class="fa fa-trash" style="color:red" onclick="DeleteMission(${item.missionId})"></i></td>
                                 </tr>`);
                    });

                    $("#mission-table").DataTable({
                        language: {
                            searchPlaceholder: "Search records",
                            search: "",
                        }
                    });
                });
            }

            // User datatable
            else if (ID == "User") {
                var tableDiv = document.querySelector("#admin-data-table");
                var missionTable = `
    <table class="table container table-striped table-hover" id='mission-table'>
        <thead>

            <tr>
                <th>FirstName</th>
                <th>LastName</th>
                <th>Email</th>
                <th>Employee Id</th>
                <th>Department</th>
                <th>Status</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody id="table-data-mission" class="new-table">
        </tbody>

    </table>`;
                tableDiv.innerHTML = missionTable;
                $(function () {
                    $.each(result.users, function (i, item) {
                        if (result.users.length > 0) {
                            if ($(".dataTables_empty").hasClass("dataTables_empty")) {
                                $(".dataTables_empty").hide();
                            }
                        }

                        $("#table-data-mission").append(`
                                 <tr id='${item.id}'>
                                     <td id="fName">${item.firstName}</td>
                                     <td id="lName">${item.lastName}</td>
                                     <td id="email">${item.email}</td>
                                     <td id="empId">${item.employeeId}</td>
                                     <td id="dept">${item.department}</td>
                                     <td class="userStatus">${item.status == 1 ? "Active" : "In-Active"}</td>
                                     <td id="action"><i class="fas fa-edit" data-bs-toggle="modal" data-bs-target="#${ID}-Modal" onclick="editUserCrud(${item.id})"></i> | <i class="fa fa-trash" style="color:red" onclick="deleteUserCrud(${item.id})"></i></td>

                                 </tr>`);
                    });

                    $("#mission-table").DataTable({
                        language: {
                            searchPlaceholder: "Search records",
                            search: "",
                        }
                    });
                });
            }

            //CMS Pages datatable
            else if (ID == "CMS-Page") {
                var tableDiv = document.querySelector("#admin-data-table");
                var missionTable = `
    <table class="table container table-striped table-hover" id='mission-table'>
        <thead>

            <tr>
                <th>Title</th>
                <th>Status</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody id="table-data-mission">
        </tbody>

    </table>`;

                tableDiv.innerHTML = missionTable;

                $(function () {
                    $.each(result.cmsPages, function (i, item) {
                        if (result.cmsPages.length > 0) {
                            if ($(".dataTables_empty").hasClass("dataTables_empty")) {
                                $(".dataTables_empty").hide();
                            }
                        }

                        $("#table-data-mission").append(`
                 <tr id='${item.cmsPageId}'>
                     <td id="title">${item.title}</td>
                     <td id="sDate">${item.status}</td>
                     <td id="action"><i class="fas fa-edit" data-bs-toggle="modal" data-bs-target="#${ID}-Modal" onclick="editCMSPage(${item.cmsPageId})"></i> | <i class="fa fa-trash" style="color:red" onclick="deleteCMS(${item.cmsPageId})"></i></td>
                 </tr>`);
                    });

                    $("#mission-table").DataTable({
                        language: {
                            searchPlaceholder: "Search records",
                            search: "",
                        }
                    });
                });
            }

            //Mission Theme datatable
            else if (ID == "Mission-Theme") {
                var tableDiv = document.querySelector("#admin-data-table");
                var missionTable = `
    <table class="table container table-striped table-hover" id='mission-table'>
        <thead>

            <tr>
                <th>Title</th>
                <th>Status</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody id="table-data-mission">
        </tbody>

    </table>`;

                tableDiv.innerHTML = missionTable;

                $(function () {
                    $.each(result.missionthemes, function (i, item) {
                        if (result.missionthemes.length > 0) {
                            if ($(".dataTables_empty").hasClass("dataTables_empty")) {
                                $(".dataTables_empty").hide();
                            }
                        }

                        $("#table-data-mission").append(`
                                 <tr id='${item.cmsPageId}'>
                                     <td id="title">${item.title}</td>
                                     <td id="sDate">${item.status == 1 ? "Active" : "In-Active"}</td>
                                     <td id="action"><i class="fas fa-edit" data-bs-toggle="modal" data-bs-target="#${ID}-Modal" onclick="LoadMissionTheme(${item.themeId})"></i> | <i class="fa fa-trash" style="color:red" onclick="DeleteMission(${item.themeId})"></i></td>
                                 </tr>`);
                    });

                    $("#mission-table").DataTable({
                        language: {
                            searchPlaceholder: "Search records",
                            search: "",
                        }
                    });
                });
            }

            //Mission Skills datatable
            else if (ID == "Mission-SKills") {
                var tableDiv = document.querySelector("#admin-data-table");
                var missionTable = `
    <table class="table container table-striped table-hover" id='mission-table'>
        <thead>

            <tr>
                <th>Title</th>
                <th>Status</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody id="table-data-mission">
        </tbody>

    </table>`;
                tableDiv.innerHTML = missionTable;

                $(function () {
                    $.each(result.missionSkills, function (i, item) {
                        if (result.missionSkills.length > 0) {
                            if ($(".dataTables_empty").hasClass("dataTables_empty")) {
                                $(".dataTables_empty").hide();
                            }
                        }

                        $("#table-data-mission").append(`
                 <tr id='${item.cmsPageId}'>
                     <td id="title">${item.skillName}</td>
                     <td id="sDate">${item.status == 1 ? "Active" : "In-Active"}</td>
                     <td id="action"><i class="fas fa-edit" data-bs-toggle="modal" data-bs-target="#${ID}-Modal" onclick="LoadMissionSkill(${item.skillId})"></i> | <i class="fa fa-trash" style="color:red" onclick="DeleteSkill(${item.skillId})"></i></td>
                 </tr>`);
                    });

                    $("#mission-table").DataTable({
                        language: {
                            searchPlaceholder: "Search records",
                            search: "",
                        }
                    });
                });
            }

            // mission application datatable
            else if (ID == "Mission-Applications") {
                $("#user-add-btn").addClass('d-none');
                var tableDiv = document.querySelector("#admin-data-table");
                tableDiv.innerHTML = "";
                var missionTable = `
    <table class="table container table-striped table-hover" id='mission-table'>
        <thead>

            <tr>
                <th>Mission Title</th>
                <th>Misison Id</th>
                <th>User Id</th>
                <th>User Name</th>
                <th>Applied At</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody id="table-data-mission">
        </tbody>

    </table>`;

                tableDiv.innerHTML = missionTable;

                $(function () {
                    $.each(result.missionApplication, function (i, item) {
                        if (result.missionApplication.length > 0) {
                            if ($(".dataTables_empty").hasClass("dataTables_empty")) {
                                $(".dataTables_empty").hide();
                            }
                        }

                        $("#table-data-mission").append(`
                 <tr id='${i}'>
                     <td id="title">${item.title}</td>
                     <td id="mId">${item.missionId}</td>
                     <td id="uId">${item.userId}</td>
                     <td id="name">${item.username}</td>
                     <td id="aDate">${item.appliedAt.split("T")[0]}</td>
                     <td id="action">
                          <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="green" class="bi bi-check-circle" viewBox="0 0 16 16" style="cursor:pointer;" onclick="ApproveMissionApplication(${item.id})">
                           <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                           <path d="M10.97 4.97a.235.235 0 0 0-.02.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-1.071-1.05z"/>
                         </svg> | <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="red" class="bi bi-x-circle" viewBox="0 0 16 16" style="cursor:pointer;" onclick="RejectMissionApplication(${item.id})">
                           <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                           <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"/>
                         </svg>


                 </tr>`);
                    });

                    $("#mission-table").DataTable({
                        language: {
                            searchPlaceholder: "Search records",
                            search: "",
                        }
                    });
                });
            }

            // story datatable
            else if (ID == "Story") {
                $("#user-add-btn").addClass('d-none');
                var tableDiv = document.querySelector("#admin-data-table");
                tableDiv.innerHTML = "";
                var missionTable = `
    <table class="table container table-striped table-hover" id='mission-table'>
        <thead>

            <tr>
                <th>Story Title</th>
                <th>Full Name</th>
                <th>Mission Title</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody id="table-data-mission">
        </tbody>

    </table>`;

                tableDiv.innerHTML = missionTable;

                $(function () {
                    $.each(result.missionStory, function (i, item) {
                        if (result.missionStory.length > 0) {
                            if ($(".dataTables_empty").hasClass("dataTables_empty")) {
                                $(".dataTables_empty").hide();
                            }
                        }

                        $("#table-data-mission").append(`
                 <tr id='${item.storyId}'>
                     <td id="stitle">${item.storyTitle}</td>
                     <td id="uName">${item.username}</td>
                     <td id="mTitle">${item.missionTitle}</td>
                     <td id="action">
                     <a type="button" class="btn view-story-btn" href="/story/${item.storyId}">View</a>
                     |  <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="green" class="bi bi-check-circle" viewBox="0 0 16 16" style="cursor:pointer;" onclick="ApproveStory(${item.storyId})">
                           <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                           <path d="M10.97 4.97a.235.235 0 0 0-.02.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-1.071-1.05z"/>
                         </svg>  |  <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="red" class="bi bi-x-circle" style="cursor:pointer;" viewBox="0 0 16 16" onclick="RejectStory(${item.storyId})">
                           <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                           <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"/>
                         </svg>  |  <i class="fa fa-trash" style="color:red;cursor:pointer;" onclick="DeleteStory(${item.storyId})"></i>

                     </td>
                 </tr>`);
                    });

                    $("#mission-table").DataTable({
                        language: {
                            searchPlaceholder: "Search records",
                            search: "",
                        }
                    });
                });
            }

            // banner managment
            else if (ID == "Banner-Management") {

                var tableDiv = document.querySelector("#admin-data-table");
                tableDiv.innerHTML = "";
                var missionTable = `
    <table class="table container table-striped table-hover" id='mission-table'>
        <thead>

            <tr>
                <th>Image Path</th>
                <th>Sort Order</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody id="table-data-mission">
        </tbody>

    </table>`;

                tableDiv.innerHTML = missionTable;

                $(function () {
                    $.each(result.banner, function (i, item) {
                        if (result.banner.length > 0) {
                            if ($(".dataTables_empty").hasClass("dataTables_empty")) {
                                $(".dataTables_empty").hide();
                            }
                        }

                        $("#table-data-mission").append(`
                                 <tr id='${item.bannerId}'>
                                     <td id="stitle">${item.image}</td>
                                     <td id="stitle">${item.sortOrder}</td>
                                     <td id="action"><i class="fas fa-edit" data-bs-toggle="modal" data-bs-target="#${ID}-Modal" onclick="LoadBanner(${item.bannerId})"></i> | <i class="fa fa-trash" style="color:red" onclick="DeleteBanner(${item.bannerId})"></i></td>
                                 </tr>`);
                    });

                    $("#mission-table").DataTable({
                        language: {
                            searchPlaceholder: "Search records",
                            search: "",
                        }
                    });
                });
            }
        },
    });


}

function makeDivActiveMobile(ID) {
    var allDivs = document.querySelector(".dashbord-sidebar-detail-mobile");
    var removed = allDivs.classList.remove("dashbord-sidebar-detail-mobile");

    var newActiveDiv = document.querySelector("#" + ID);
    newActiveDiv.classList.add("dashbord-sidebar-detail-mobile");

    var title = document.querySelector("#selected-option-title");

    var newTitle = document.getElementById(ID).querySelector(".title-name");
    const hr = document.createElement("hr");




    title.innerHTML = newTitle.innerHTML;

    title.appendChild(hr);
}

// user crud start
function loadCountry(response) {

    var html = "";
    if (response.userCountry != null) {
        html = "<option disabled hidden>select your country</option>";
    } else {
        html = "<option selected disabled hidden>select your country</option>";
    }

    response.country.map(item => {
        if (response.userCountry != null && item.countryId == response.userCountry) {
            html += `<option value="${item.countryId}" selected>${item.name}</option>`
        } else {
            html += `<option value="${item.countryId}" >${item.name}</option>`
        }

    }).join(" ")
    return html;
}

//load countrys in drop-down
$.ajax({
    type: "GET",
    url: `/api/profile/country`,
    success: function (response) {

        var countryElements = document.getElementById("user-crud-country-dropdown");
        countryElements.innerHTML = loadCountry(response);

        var countryElements = document.getElementById("mission-country");
        countryElements.innerHTML = loadCountry(response);

    },
    failure: function (response) {
        alert(response.responseText);
    },
    error: function (response) {
        alert(response.responseText);
    }
});

// load country related city
function loadCity(response) {
    var html = "<option disabled hidden>select your city</option>";
    response.country.map(item => {
        html += `<option value="${item.cityId}" >${item.name}</option>`
    }).join(" ")
    return html;
}


function loadCityForCountry(countryId) {

    $.ajax({
        type: "GET",
        url: `/api/profile/country/${countryId}`,
        success: function (response) {

            var city = document.getElementById("user-crud-city-dropdown");
            city.innerHTML = loadCity(response);

            var city = document.getElementById("mission-city");
            city.innerHTML = loadCity(response);

        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

// save user img
$("#myFileInput").on('change', function () {
    var files = $('#myFileInput').prop("files");
    var url = "/api/static/save";
    formData = new FormData();
    formData.append("MyUploader", files[0]);

    jQuery.ajax({
        type: 'POST',
        url: url,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (response) {
            var avatar = document.getElementById("profile-user-img");
            avatar.innerHTML = `<img src="/mediaUpload/${files[0].name}" alt="" class="rounded-circle user-profile-img" onclick="document.getElementById('myFileInput').click()" style="cursor:pointer;width: 100px;height: 100px;">`;
            // assign filename to local variable
            userProfileName = files[0].name;
        },
        error: function (response) {
            alert(response);
        }
    });
});


function AddUserModal() {
    var error = 0;
    var firstname = $('#user-crud-first-name').val()
    if (firstname == "") {
        error = 1;
        $('#user-crud-first-name-error').removeClass("d-none")
        $('#user-crud-first-name-error').html("FirstName is required.")
    } else {
        $('#user-crud-first-name-error').addClass("d-none")
    }
    var lastname = $('#user-crud-last-name').val()
    if (lastname == "") {
        error = 1;
        $('#user-crud-last-name-error').removeClass("d-none")
        $('#user-crud-last-name-error').html("LastName is required.")
    } else {
        $('#user-crud-last-name-error').addClass("d-none")
    }
    var email = $('#user-crud-email').val()
    if (email == "") {
        error = 1;
        $('#user-crud-email-error').removeClass("d-none")
        $('#user-crud-email-error').html("Email is required.")
    } else {
        $('#user-crud-email-error').addClass("d-none")
    }
    var password = $('#user-crud-password').val()
    if (password != "" && password.length < 8) {
        error = 1;
        $('#user-crud-password-error').removeClass("d-none")
        $('#user-crud-password-error').html("password is required.")
    } else {
        $('#user-crud-password-error').addClass("d-none")
    }
    var avatar = $('#user-crud-avatar').val()
    var empId = $('#user-crud-employee-id').val()
    if (empId == "") {
        error = 1;
        $('#user-crud-employee-id-error').removeClass("d-none")
        $('#user-crud-employee-id-error').html("Emp Id is required.")
    } else {
        $('#user-crud-employee-id-error').addClass("d-none")
    }
    var department = $('#user-crud-department').val()
    if (department == "") {
        error = 1;
        $('#user-crud-department-error').removeClass("d-none")
        $('#user-crud-department-error').html("department is required.")
    } else {
        $('#user-crud-department-error').addClass("d-none")
    }
    var mobileNo = $('#user-crud-mobile-number').val()
    if (mobileNo == "") {
        error = 1;
        $('#user-crud-mobile-number-error').removeClass("d-none")
        $('#user-crud-mobile-number-error').html("mobile number is required.")
    } else {
        $('#user-crud-mobile-number-error').addClass("d-none")
    }
    var country = $('#user-crud-country-dropdown').val()
    if (country == "0") {
        error = 1;
        $('#user-crud-country-dropdown-error').removeClass("d-none")
        $('#user-crud-country-dropdown-error').html("Country is required.")
    } else {
        $('#user-crud-country-dropdown-error').addClass("d-none")
    }
    var city = $('#user-crud-city-dropdown').val()
    if (city == "0") {
        error = 1;
        $('#user-crud-city-dropdown-error').removeClass("d-none")
        $('#user-crud-city-dropdown-error').html("City is required.")
    } else {
        $('#user-crud-city-dropdown-error').addClass("d-none")
    }
    var pText = $('#user-crud-profile-text').val()
    if (pText == "") {
        error = 1;
        $('#user-crud-profile-text').removeClass("d-none")
        $('#user-crud-profile-text').html("profile text is required.")
    } else {
        $('#user-crud-profile-text-error').addClass("d-none")
    }
    var status = $('#user-crud-status-dropdown').val()

    if (error == 0) {
        $.ajax({
            type: "POST",
            url: `/api/admin/addUser`,
            data: {
                "firstName": firstname,
                "lastName": lastname,
                "phoneNumber": mobileNo,
                "avatar": userProfileName,
                "employeeId": empId,
                "department": department,
                "city": city,
                "country": country,
                "profileText": pText,
                "password": password,
                "email": email
            },
            success: function (response) {

                if (response.status == false) {
                    toastr.error("Email and Employee Id must be unique.");
                    makeDivActive("User");
                } else {
                    toastr.success("User Added.");
                    makeDivActive("User");
                }


            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    error = 0;

}

function EditUserModal(id) {

    //edit ajax call
    var error = 0;
    var firstname = $('#user-crud-first-name').val()
    if (firstname == "") {
        error = 1;
        $('#user-crud-first-name-error').removeClass("d-none")
        $('#user-crud-first-name-error').html("FirstName is required.")
    } else {
        $('#user-crud-first-name-error').addClass("d-none")
    }
    var lastname = $('#user-crud-last-name').val()
    if (lastname == "") {
        error = 1;
        $('#user-crud-last-name-error').removeClass("d-none")
        $('#user-crud-last-name-error').html("LastName is required.")
    } else {
        $('#user-crud-last-name-error').addClass("d-none")
    }
    var email = $('#user-crud-email').val()
    if (email == "") {
        error = 1;
        $('#user-crud-email-error').removeClass("d-none")
        $('#user-crud-email-error').html("Email is required.")
    } else {
        $('#user-crud-email-error').addClass("d-none")
    }
    var password = $('#user-crud-password').val()
    if (password != "" && password.length < 8) {
        error = 1;
        $('#user-crud-password-error').removeClass("d-none")
        $('#user-crud-password-error').html("password is required.")
    } else {
        $('#user-crud-password-error').addClass("d-none")
    }
    var avatar = $('#user-crud-avatar').val()
    var empId = $('#user-crud-employee-id').val()
    if (empId == "") {
        error = 1;
        $('#user-crud-employee-id-error').removeClass("d-none")
        $('#user-crud-employee-id-error').html("Emp Id is required.")
    } else {
        $('#user-crud-employee-id-error').addClass("d-none")
    }
    var department = $('#user-crud-department').val()
    if (department == "") {
        error = 1;
        $('#user-crud-department-error').removeClass("d-none")
        $('#user-crud-department-error').html("department is required.")
    } else {
        $('#user-crud-department-error').addClass("d-none")
    }
    var mobileNo = $('#user-crud-mobile-number').val()
    if (mobileNo == "") {
        error = 1;
        $('#user-crud-mobile-number-error').removeClass("d-none")
        $('#user-crud-mobile-number-error').html("mobile number is required.")
    } else {
        $('#user-crud-mobile-number-error').addClass("d-none")
    }
    var country = $('#user-crud-country-dropdown').val()
    if (country == "0") {
        error = 1;
        $('#user-crud-country-dropdown-error').removeClass("d-none")
        $('#user-crud-country-dropdown-error').html("Country is required.")
    } else {
        $('#user-crud-country-dropdown-error').addClass("d-none")
    }
    var city = $('#user-crud-city-dropdown').val()
    if (city == "0") {
        error = 1;
        $('#user-crud-city-dropdown-error').removeClass("d-none")
        $('#user-crud-city-dropdown-error').html("City is required.")
    } else {
        $('#user-crud-city-dropdown-error').addClass("d-none")
    }
    var pText = $('#user-crud-profile-text').val()
    if (pText == "") {
        error = 1;
        $('#user-crud-profile-text').removeClass("d-none")
        $('#user-crud-profile-text').html("profile text is required.")
    } else {
        $('#user-crud-profile-text-error').addClass("d-none")
    }
    var status = $('#user-crud-status-dropdown').val()

    if (error == 0) {
        $.ajax({
            type: "POST",
            url: `/api/admin/editUser`,
            data: {
                "Id": id,
                "firstName": firstname,
                "lastName": lastname,
                "phoneNumber": mobileNo,
                "avatar": userProfileName,
                "employeeId": empId,
                "department": department,
                "city": city,
                "country": country,
                "profileText": pText,
                "password": password,
                "email": email,
                "status": status
            },
            success: function (response) {

                if (response.status == false) {
                    toastr.error("Email and Employee Id must be unique.");
                    makeDivActive("User");
                } else {
                    toastr.success("User Changed Successfully.");
                    makeDivActive("User");
                }


            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    error = 0;

}

// edit user
function editUserCrud(id) {

    $('#user-crud-email').prop('disabled', true);

    var element = document.getElementById("user-crud-modal-btn");
    element.innerHTML = `<button type="button" class="btn btn-secondary user-crud-close-btn" data-bs-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary user-crud-save-btn" data-bs-dismiss="modal" id="user-crud-save-btn" onclick="EditUserModal(${id})">Save changes</button>`

    $.ajax({
        type: "GET",
        url: `/api/admin/getUserProfile`,
        data: {
            "id": id,
        },
        success: function (response) {
            $('#user-crud-first-name').val(response.result.firstName)
            $('#user-crud-last-name').val(response.result.lastName)
            $('#user-crud-email').val(response.result.email)
            $('#user-crud-password').val(response.result.password)
            $('#user-crud-employee-id').val(response.result.employeeId)
            $('#user-crud-department').val(response.result.department)
            $('#user-crud-mobile-number').val(response.result.phoneNumber)
            $('#user-crud-profile-text').val(response.result.profileText)

            // load country
            $(`#user-crud-country-dropdown option:eq(${response.result.countryId})`).prop('selected', true)

            // load city
            loadCityForCountry(response.result.countryId);
            $(`#user-crud-city-dropdown option:eq(${response.result.cityId})`).prop('selected', true)

            // for status
            if (response.result.status == "0") {
                $(`#user-crud-status-dropdown option:eq(1)`).prop('selected', true)
            } else {
                $(`#user-crud-status-dropdown option:eq(0)`).prop('selected', true)
            }

            // avatar
            var a = document.getElementById("profile-user-img");
            a.innerHTML = `<img src="/mediaUpload/${response.result.avatar}" alt="" class="rounded-circle user-profile-img" onclick="document.getElementById('myFileInput').click()" style="cursor:pointer;width: 100px;height: 100px;">`
            userProfileName = response.result.avatar;
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}

function deleteUserCrud(id) {
    $.ajax({
        type: "POST",
        url: `/api/admin/deleteUserProfile`,
        data: {
            "id": id,
        },
        success: function (response) {
            toastr.success("User Deleted.");
            makeDivActive("User");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

// user crud end

// cms crud start

function addCMS() {

    var error = 0;
    var title = $('#cms-crud-title').val()
    if (title == "") {
        error = 1;
        $('#cms-crud-title-error').removeClass("d-none")
        $('#cms-crud-title-error').html("Title is required.")
    } else {
        $('#cms-crud-title-error').addClass("d-none")
    }
    var editor = CKEDITOR.instances.cmspageeditor.getData();
    var slug = $('#cms-crud-Slug').val()
    if (slug == "") {
        error = 1;
        $('#cms-crud-Slug-error').removeClass("d-none")
        $('#cms-crud-Slug-error').html("Slug is required.")
    } else {
        $('#cms-crud-Slug-error').addClass("d-none")
    }
    var status = $('#cms-crud-status').val()

    if (error == 0) {
        $.ajax({
            type: "POST",
            url: `/api/admin/addCMS`,
            data: {
                "title": title,
                "slug": slug,
                "status": status,
                "description": editor
            },
            success: function (response) {

                toastr.success("CMS Added.");
                makeDivActive("CMS-Page");

            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    error = 0;
}

function EditCMS(id) {


    var error = 0;
    var title = $('#cms-crud-title').val()
    if (title == "") {
        error = 1;
        $('#cms-crud-title-error').removeClass("d-none")
        $('#cms-crud-title-error').html("Title is required.")
    } else {
        $('#cms-crud-title-error').addClass("d-none")
    }
    var editor = CKEDITOR.instances.cmspageeditor.getData();
    var slug = $('#cms-crud-Slug').val()
    if (slug == "") {
        error = 1;
        $('#cms-crud-Slug-error').removeClass("d-none")
        $('#cms-crud-Slug-error').html("Slug is required.")
    } else {
        $('#cms-crud-Slug-error').addClass("d-none")
    }
    var status = $('#cms-crud-status').val()

    if (error == 0) {
        $.ajax({
            type: "POST",
            url: `/api/admin/editCMS`,
            data: {
                "id": id,
                "title": title,
                "slug": slug,
                "status": status,
                "description": editor
            },
            success: function (response) {
                toastr.success("CMS Changed.");
                makeDivActive("CMS-Page");

            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    error = 0;


}

function editCMSPage(id) {

    var element = document.getElementById('cms-page-modal-btn');
    element.innerHTML = `<button type="button" class="btn btn-secondary cms-crud-close-btn" data-bs-dismiss="modal">Close</button>
            <button type="submit" class="btn btn-primary cms-crud-save-btn" data-bs-dismiss="modal" id="cms-crud-save-btn" onclick="EditCMS(${id})">Save changes</button>`

    $.ajax({
        type: "GET",
        url: `/api/admin/getCMS`,
        data: {
            "id": id,
        },
        success: function (response) {

            var title = $('#cms-crud-title').val(response.result.title)
            CKEDITOR.instances.cmspageeditor.setData(response.result.description);
            var slug = $('#cms-crud-Slug').val(response.result.slug)
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}

function deleteCMS(id) {

    $.ajax({
        type: "POST",
        url: `/api/admin/deleteCMS`,
        data: {
            "id": id,
        },
        success: function (response) {
            toastr.success("CMS Deleted.");
            makeDivActive("CMS-Page");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}

// cms crud end

// mission theme CRUD start

function AddMissionTheme() {

    var error = 0;
    var title = $('#mission-theme-crud-title').val();
    if (title == "") {
        error = 1;
        $('#mission-theme-crud-title-error').removeClass("d-none")
        $('#mission-theme-crud-title-error').html("Theme Title is required.")
    } else {
        $('#mission-theme-crud-title-error').addClass("d-none")
    }
    var status = $('#mission-theme-crud-status').val();

    var LongStatus = 0;
    if (status == "Pending") {
        LongStatus = 2
    } else {
        LongStatus = 1
    }

    if (error == 0) {
        $.ajax({
            type: "POST",
            url: `/api/admin/addTheme`,
            data: {
                "title": title,
                "status": LongStatus
            },
            success: function (response) {
                if (response.result == true) {
                    toastr.success("Theme Added.");
                    makeDivActive("Mission-Theme");
                } else {
                    toastr.error("Theme Title Must be unique.");
                    makeDivActive("Mission-Theme");
                }

            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    error = 0;

}

function EditMissionTheme(id) {

    var error = 0;
    var title = $('#mission-theme-crud-title').val();
    if (title == "") {
        error = 1;
        $('#mission-theme-crud-title-error').removeClass("d-none")
        $('#mission-theme-crud-title-error').html("Theme Title is required.")
    } else {
        $('#mission-theme-crud-title-error').addClass("d-none")
    }
    var status = $('#mission-theme-crud-status').val();

    var LongStatus = 0;
    if (status == "Pending") {
        LongStatus = 2
    } else {
        LongStatus = 1
    }

    if (error == 0) {
        $.ajax({
            type: "POST",
            url: `/api/admin/editTheme`,
            data: {
                "title": title,
                "status": LongStatus,
                "themeId": id
            },
            success: function (response) {

                if (response.result == true) {
                    toastr.success("Theme Changed.");
                    makeDivActive("Mission-Theme");
                } else {
                    toastr.error("Theme Title Must be unique.");
                    makeDivActive("Mission-Theme");
                }
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    error = 0;

}

function LoadMissionTheme(id) {

    var element = document.getElementById('mission-theme-crud-btn');
    element.innerHTML = `<button type="button" class="btn btn-secondary mission-theme-crud-close-btn" data-bs-dismiss="modal">Close</button>
            <button type="submit" class="btn btn-primary mission-theme-crud-save-btn" data-bs-dismiss="modal" id="mission-theme-crud-save-btn" onclick="EditMissionTheme(${id})">Save changes</button>`



    $.ajax({
        type: "GET",
        url: `/api/admin/getTheme`,
        data: {
            "id": id
        },
        success: function (response) {

            $('#mission-theme-crud-title').val(response.result.title);

            var themeStatus = document.getElementById('mission-theme-crud-status')
            if (response.result.status == 1) {
                themeStatus.innerHTML = `<option value="Pending" >In-Active</option>
                                                <option value="Published" selected>Active</option>`
            } else {
                themeStatus.innerHTML = `<option value="Pending" selected>In-Active</option>
                                                <option value="Published">Active</option>`
            }

        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}

function DeleteMission(id) {

    $.ajax({
        type: "POST",
        url: `/api/admin/deleteTheme`,
        data: {
            "id": id
        },
        success: function (response) {
            toastr.success("Theme Deleted.");
            makeDivActive("Mission-Theme");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}


// mission theme CRUD end



// mission skill CRUD end

function AddMissionSkill() {

    var error = 0;
    var title = $('#mission-skill-crud-title').val();
    if (title == "") {
        error = 1;
        $('#mission-skill-crud-title-error').removeClass("d-none")
        $('#mission-skill-crud-title-error').html("Skill Name is required.")
    } else {
        $('#mission-skill-crud-title-error').addClass("d-none")
    }
    var status = $('#mission-skill-crud-status').val();

    var LongStatus = 0;
    if (status == "Pending") {
        LongStatus = 2
    } else {
        LongStatus = 1
    }

    if (error == 0) {
        $.ajax({
            type: "POST",
            url: `/api/admin/addSkill`,
            data: {
                "skillName": title,
                "status": LongStatus
            },
            success: function (response) {
                if (response.result == true) {
                    toastr.success("Skill Added.");
                    makeDivActive("Mission-SKills");
                } else {
                    toastr.error("Skill Title must be unique.");
                    makeDivActive("Mission-SKills");
                }

            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    error = 0;

}

function EditMissionSkill(id) {

    var error = 0;
    var title = $('#mission-skill-crud-title').val();
    if (title == "") {
        error = 1;
        $('#mission-skill-crud-title-error').removeClass("d-none")
        $('#mission-skill-crud-title-error').html("Skill Name is required.")
    } else {
        $('#mission-skill-crud-title-error').addClass("d-none")
    }
    var status = $('#mission-skill-crud-status').val();

    var LongStatus = 0;
    if (status == "Pending") {
        LongStatus = 2
    } else {
        LongStatus = 1
    }

    if (error == 0) {
        $.ajax({
            type: "POST",
            url: `/api/admin/editSkill`,
            data: {
                "skillName": title,
                "status": LongStatus,
                "skillId": id
            },
            success: function (response) {

                if (response.result == true) {
                    toastr.success("Skill Changed.");
                    makeDivActive("Mission-SKills");
                } else {
                    toastr.error("Skill Title must be unique.");
                    makeDivActive("Mission-SKills");
                }
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    error = 0;

}

function LoadMissionSkill(id) {

    var element = document.getElementById('mission-skill-crud-btn');
    element.innerHTML = `<button type="button" class="btn btn-secondary mission-skill-crud-close-btn" data-bs-dismiss="modal">Close</button>
            <button type="submit" class="btn btn-primary mission-skill-crud-save-btn" data-bs-dismiss="modal" id="mission-skill-crud-save-btn" onclick="EditMissionSkill(${id})">Save changes</button>`



    $.ajax({
        type: "GET",
        url: `/api/admin/getSkill`,
        data: {
            "id": id
        },
        success: function (response) {
            $('#mission-skill-crud-title').val(response.result.skillName);
            //$('#mission-theme-crud-status').val();

            var themeStatus = document.getElementById('mission-skill-crud-status')
            if (response.result.status == 1) {
                themeStatus.innerHTML = `<option value="Pending" >In-Active</option>
                                                <option value="Published" selected>Active</option>`
            } else {
                themeStatus.innerHTML = `<option value="Pending" selected>In-Active</option>
                                                <option value="Published">Active</option>`
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}

function DeleteSkill(id) {

    $.ajax({
        type: "POST",
        url: `/api/admin/deleteSkill`,
        data: {
            "id": id
        },
        success: function (response) {
            toastr.success("Skill Deleted.");
            makeDivActive("Mission-SKills");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}


// mission skill CRUD end


// mission application approve start

function ApproveMissionApplication(id) {

    $.ajax({
        type: "POST",
        url: `/api/admin/approveMission`,
        data: {
            "id": id
        },
        success: function (response) {
            toastr.success("Application Approved.");
            makeDivActive("Mission-Applications");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}

function RejectMissionApplication(id) {

    $.ajax({
        type: "POST",
        url: `/api/admin/rejectMission`,
        data: {
            "id": id
        },
        success: function (response) {
            toastr.warning("Application Rejected.");
            makeDivActive("Mission-Applications");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}



// mission skill approve end



// story approve start

function ApproveStory(id) {

    $.ajax({
        type: "POST",
        url: `/api/admin/approveStory`,
        data: {
            "id": id
        },
        success: function (response) {
            toastr.success("Story Approved.");
            makeDivActive("Story");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}

function RejectStory(id) {

    $.ajax({
        type: "POST",
        url: `/api/admin/rejectStory`,
        data: {
            "id": id
        },
        success: function (response) {
            toastr.info("Story Rejected.");
            makeDivActive("Story");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}

function DeleteStory(id) {
    $.ajax({
        type: "POST",
        url: `/api/admin/deleteStory`,
        data: {
            "id": id
        },
        success: function (response) {
            toastr.warning("Story Deleted.");
            makeDivActive("Story");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}


// story approve end

// mission start

// var for image list
var ImagesList = [];

function AddMission() {

    ImagesList = [];

    var error = 0;
    var title = $('#mission-title').val();
    if (title == "") {
        error = 1;
        $('#mission-title-error').removeClass("d-none")
        $('#mission-title-error').html("Title is required.")
    } else {
        $('#mission-title-error').addClass("d-none")
    }
    var organization = $('#mission-organization').val();
    if (organization == "") {
        error = 1;
        $('#mission-organization-error').removeClass("d-none")
        $('#mission-organization-error').html("Organization is required.")
    } else {
        $('#mission-organization-error').addClass("d-none")
    }
    var description = CKEDITOR.instances.cmspageeditor1.getData();
    var organizationDetails = CKEDITOR.instances.cmspageeditor2.getData();
    var startDate = $('#mission-start-date').val();
    var endDate = $('#mission-end-date').val();

    if (Date.parse(startDate) > Date.parse(endDate)) {
        error = 1;
        $('#mission-start-date-error').removeClass("d-none")
        $('#mission-start-date-error').html("Enter Valid Date.")
    } else {
        $('#mission-start-date-error').addClass("d-none")
    }

    if (endDate == "") {
        error = 1;
        $('#mission-end-date-error').removeClass("d-none")
        $('#mission-end-date-error').html("End Date is required.")
    } else {
        $('#mission-end-date-error').addClass("d-none")
    }
    var countryId = $('#mission-country').val();
    if (countryId == null) {
        error = 1;
        $('#mission-country-error').removeClass("d-none")
        $('#mission-country-error').html("Country is required.")
    } else {
        $('#mission-country-error').addClass("d-none")
    }
    var cityId = $('#mission-city').val();

    if (cityId == null) {
        error = 1;
        $('#mission-city-error').removeClass("d-none")
        $('#mission-city-error').html("City is required.")
    } else {
        $('#mission-city-error').addClass("d-none")
    }
    var seats = $('#mission-seats').val();
    var deadline = $('#mission-registration-deadline').val();
    var availability = $('#mission-availability').val();
    if (availability == null) {
        error = 1;
        $('#mission-availability-error').removeClass("d-none")
        $('#mission-availability-error').html("Availability is required.")
    } else {
        $('#mission-availability-error').addClass("d-none")
    }
    var type = $('#mission-type').val();
    if (type == null) {
        error = 1;
        $('#mission-type-error').removeClass("d-none")
        $('#mission-type-error').html("Type is required.")
    } else {
        $('#mission-type-error').addClass("d-none")
    }
    var status = $('#mission-status').val();
    if (status == null) {
        error = 1;
        $('#mission-status-error').removeClass("d-none")
        $('#mission-status-error').html("Status is required.")
    } else {
        $('#mission-status-error').addClass("d-none")
    }
    var themeId = $('#mission-theme').val();
    if (themeId == null) {
        error = 1;
        $('#mission-theme-error').removeClass("d-none")
        $('#mission-theme-error').html("Theme is required.")
    } else {
        $('#mission-theme-error').addClass("d-none")
    }

    //for goal mission
    var missionObjective = $("#mission-objective").val();
    var missionTarget = $("#mission-target").val();
    console.log(missionObjective, missionTarget);
    // for images
    let file = $("#mission-images")[0].files;

    var images = [];
    for (var i = 0; i < file.length; i++) {
        images.push(file[i].name);
    }

    // for documents
    let Docfile = $("#mission-document")[0].files;

    var doc = [];
    for (var i = 0; i < Docfile.length; i++) {
        doc.push(Docfile[i].name);
    }


    if (error == 0) {
        $.ajax({
            type: "POST",
            url: `/api/admin/addMission`,
            data: {
                "title": title,
                "organization": organization,
                "description": description,
                "organizationDetails": organizationDetails,
                "startDate": startDate,
                "endDate": endDate,
                "countryId": countryId,
                "cityId": cityId,
                "seats": seats,
                "deadline": deadline,
                "availability": availability,
                "missionType": type,
                "status": status,
                "themeId": themeId,
                "images": images,
                "docs": doc,
                "missionObjective": missionObjective,
                "missionTarget": missionTarget


            },
            success: function (response) {
                toastr.success("Mission Added.");
                makeDivActive("Mission");
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    error = 0;
}



function RemoveImagesFromList(item) {
    ImagesList = ImagesList.filter(name => name !== item);
    var element = document.getElementById(item);
    element.classList.add('d-none');
}

function LoadImages(response) {
    html = '';
    if (response.result.images.length == 0) {
        html += "No image Selected";
        return html;
    }
    response.result.images.map(item => {
        html += `<div class="col-2" id="${item}"><img src="/mediaUpload/${item}"  width="100px" height="100px" style="position: relative;" /><i class="bi bi-x-square-fill" style="cursor:pointer;" onclick="RemoveImagesFromList('${item}')"></i></div>`
        ImagesList.push(item);
    }).join("");
    return html;
}

function LoadDocs(response) {
    html = '';
    if (response.result.docs.length == 0) {
        html += "No Document Found.";
        return html;
    }
    response.result.docs.map(item => {
        html += `<a href="/document/${item}" class="col-3 me-2 edit-mission-documents" >${item}</a>`

    }).join("");

    return html;
}

async function LoadMissionData(id) {

    var element = document.getElementById('mission-crud-btn');
    element.innerHTML = `<button type="button" class="btn btn-secondary mission-skill-crud-close-btn" data-bs-dismiss="modal">Close</button>
            <button type="submit" class="btn btn-primary mission-skill-crud-save-btn" id="mission-crud-save-btn" onclick="EditMission(${id})">Save changes</button>`

    $('#mission-media-edit-files').removeClass('d-none');
    $('#mission-docs-edit-files').removeClass('d-none');

    $.ajax({
        type: "GET",
        url: `/api/admin/loadMission`,
        data: {
            "id": id
        },
        success: function (response) {
            $('#mission-title').val(response.result.title);
            $('#mission-organization').val(response.result.organization);
            CKEDITOR.instances.cmspageeditor1.setData(response.result.description);
            CKEDITOR.instances.cmspageeditor2.setData(response.result.organizationDetails);
            $('#mission-start-date').val(response.result.startDate);
            $('#mission-end-date').val(response.result.endDate);
            //$('#mission-country').val(response.result.title);
            //$('#mission-city').val(response.result.title);
            $('#mission-seats').val(response.result.seats);
            $('#mission-registration-deadline').val(response.result.title);

            var loadImages = document.getElementById('mission-media-edit-files');
            loadImages.innerHTML = LoadImages(response);

            var loadDocs = document.getElementById('mission-docs-edit-files');
            loadDocs.innerHTML = LoadDocs(response);

            // mission type
            var themeStatus = document.getElementById('mission-type')
            if (response.result.missionType == "Time") {
                themeStatus.innerHTML = `<option disabled hidden>Open this select menu</option>
                                              <option value="Time" selected>Time</option>
                                              <option value="Goal">Goal</option>`
            } else {
                themeStatus.innerHTML = `<option disabled hidden>Open this select menu</option>
                                              <option value="Time" >Time</option>
                                              <option value="Goal" selected>Goal</option>`
            }

            // mission status
            var status = document.getElementById('mission-status')
            if (response.result.status == "1") {
                status.innerHTML = `<option  disabled hidden>Open this select menu</option>
                                              <option value="1" selected>Active</option>
                                              <option value="0">In-Active</option>`
            } else {
                status.innerHTML = `<option selected disabled hidden>Open this select menu</option>
                                              <option value="1">Active</option>
                                              <option value="0" selected>In-Active</option>`
            }

            // mission status
            var missionType = document.getElementById('mission-availability')
            if (response.result.availability == "weekend") {
                missionType.innerHTML = `<option selected disabled hidden>Open this select menu</option>
                                              <option value="daily">daily</option>
                                              <option value="weekly">weekly</option>
                                              <option value="weekend" selected>weekend</option>`
            } else if (response.result.availability == "weekly") {
                missionType.innerHTML = `<option selected disabled hidden>Open this select menu</option>
                                              <option value="daily">daily</option>
                                              <option value="weekly" selected>weekly</option>
                                              <option value="weekend">weekend</option>`
            } else {
                missionType.innerHTML = `<option selected disabled hidden>Open this select menu</option>
                                              <option value="daily" selected>daily</option>
                                              <option value="weekly">weekly</option>
                                              <option value="weekend">weekend</option>`
            }

            // select theme
            $(`#mission-theme option:eq(${response.result.themeId})`).prop('selected', true)

            // select Country
            $(`#mission-country option:eq(${response.result.countryId})`).prop('selected', true)

            // for loading city
            loadCityForCountry(response.result.countryId);
            $(`#mission-city option:eq(${response.result.cityId})`).prop('selected', true)

        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}

function EditMission(id) {

    var error = 0;
    var title = $('#mission-title').val();
    if (title == "") {
        error = 1;
        $('#mission-title-error').removeClass("d-none")
        $('#mission-title-error').html("Title is required.")
    } else {
        $('#mission-title-error').addClass("d-none")
    }
    var organization = $('#mission-organization').val();
    if (organization == "") {
        error = 1;
        $('#mission-organization-error').removeClass("d-none")
        $('#mission-organization-error').html("Organization is required.")
    } else {
        $('#mission-organization-error').addClass("d-none")
    }
    var description = CKEDITOR.instances.cmspageeditor1.getData();
    var organizationDetails = CKEDITOR.instances.cmspageeditor2.getData();
    var startDate = $('#mission-start-date').val();
    var endDate = $('#mission-end-date').val();
    if (startDate == "" || (startDate != "" && Date.parse(startDate) > Date.parse(endDate))) {
        error = 1;
        $('#mission-start-date-error').removeClass("d-none")
        $('#mission-start-date-error').html("Enter Valid Date.")
    } else {
        $('#mission-start-date-error').addClass("d-none")
    }
    if (endDate == "") {
        error = 1;
        $('#mission-end-date-error').removeClass("d-none")
        $('#mission-end-date-error').html("End Date is required.")
    } else {
        $('#mission-end-date-error').addClass("d-none")
    }
    var countryId = $('#mission-country').val();
    if (countryId == null) {
        error = 1;
        $('#mission-country-error').removeClass("d-none")
        $('#mission-country-error').html("Country is required.")
    } else {
        $('#mission-country-error').addClass("d-none")
    }
    var cityId = $('#mission-city').val();

    if (cityId == null) {
        error = 1;
        $('#mission-city-error').removeClass("d-none")
        $('#mission-city-error').html("City is required.")
    } else {
        $('#mission-city-error').addClass("d-none")
    }
    var seats = $('#mission-seats').val();
    var deadline = $('#mission-registration-deadline').val();
    var availability = $('#mission-availability').val();
    if (availability == null) {
        error = 1;
        $('#mission-availability-error').removeClass("d-none")
        $('#mission-availability-error').html("Availability is required.")
    } else {
        $('#mission-availability-error').addClass("d-none")
    }
    var type = $('#mission-type').val();
    if (type == null) {
        error = 1;
        $('#mission-type-error').removeClass("d-none")
        $('#mission-type-error').html("Type is required.")
    } else {
        $('#mission-type-error').addClass("d-none")
    }
    var status = $('#mission-status').val();
    if (status == null) {
        error = 1;
        $('#mission-status-error').removeClass("d-none")
        $('#mission-status-error').html("Status is required.")
    } else {
        $('#mission-status-error').addClass("d-none")
    }
    var themeId = $('#mission-theme').val();
    if (themeId == null) {
        error = 1;
        $('#mission-theme-error').removeClass("d-none")
        $('#mission-theme-error').html("Theme is required.")
    } else {
        $('#mission-theme-error').addClass("d-none")
    }

    // for images from file select
    let file = $("#mission-images")[0].files;


    for (var i = 0; i < file.length; i++) {
        ImagesList.push(file[i].name);
    }



    // for docs
    let Docfile = $("#mission-document")[0].files;

    var doc = [];
    for (var i = 0; i < Docfile.length; i++) {
        doc.push(Docfile[i].name);
    }


    if (error == 0) {
        $.ajax({
            type: "POST",
            url: `/api/admin/editMission`,
            data: {
                "id": id,
                "title": title,
                "organization": organization,
                "description": description,
                "organizationDetails": organizationDetails,
                "startDate": startDate,
                "endDate": endDate,
                "countryId": countryId,
                "cityId": cityId,
                "seats": seats,
                "deadline": deadline,
                "availability": availability,
                "missionType": type,
                "status": status,
                "themeId": themeId,
                "images": ImagesList,
                "docs": doc

            },
            success: function (response) {
                toastr.success("Mission Changed.");
                makeDivActive("Mission");
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    error = 0;
}

function DeleteMission(id) {
    $.ajax({
        type: "POST",
        url: `/api/admin/deleteMission`,
        data: {
            "id": id
        },
        success: function (response) {
            toastr.success("Mission Deleted.");
            makeDivActive("Mission");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function loadTheme(response) {
    var html = '<option selected disabled hidden>Select Theme</option>';
    response.result.map(item => {
        html += `<option value="${item.themeId}">${item.title}</option>`
    }).join("")

    return html;
}

//load mission theme
$.ajax({
    type: "GET",
    url: `/api/admin/getThemes`,
    success: function (response) {

        var element = document.getElementById('mission-theme');
        element.innerHTML = loadTheme(response);
    },
    failure: function (response) {
        alert(response.responseText);
    },
    error: function (response) {
        alert(response.responseText);
    }
});


// images save to db
function InsertMissionImage() {

    var files = $('#mission-images').prop("files");
    var url = "/api/admin/saveImg";


    var formData = new FormData();

    //formData.append("MyUploader", files);
    if (files.length > 0) {
        for (var x = 0; x < files.length; x++) {
            // the name has to be 'files' so that .NET could properly bind it
            formData.append('MyUploader', files.item(x));
        }
    }

    jQuery.ajax({
        type: 'POST',
        url: url,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (response) {

        },
        error: function (response) {
            alert(response);
        }
    });


}

// save document to root
function InsertMissionDocs() {

    var files = $('#mission-document').prop("files");
    var url = "/api/admin/saveDoc";


    var formData = new FormData();

    //formData.append("MyUploader", files);
    if (files.length > 0) {
        for (var x = 0; x < files.length; x++) {
            // the name has to be 'files' so that .NET could properly bind it
            formData.append('MyUploader', files.item(x));
        }
    }


    jQuery.ajax({
        type: 'POST',
        url: url,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (response) {

        },
        error: function (response) {
            alert(response);
        }
    });


}

// mission end



// banner start
var banner = "";
$('#banner-images').on('change', function () {

    let file = $("#banner-images")[0].files;
    banner = file[0].name;

    var url = "/api/static/save";
    formData = new FormData();
    var files = $('#banner-images').prop("files");
    formData.append("MyUploader", files[0]);

    jQuery.ajax({
        type: 'POST',
        url: url,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (response) {
            var element = document.getElementById('banner-media-edit-files');
            element.innerHTML = `<img src="/mediaUpload/${banner}" width="200px" height="200px" />`

        },
        error: function (response) {
            alert(response);
        }
    });

})


// load banner
function LoadBanner(id) {

    var element = document.getElementById('banner-crud-btn');
    element.innerHTML = `<button type="button" class="btn btn-secondary mission-theme-crud-close-btn" data-bs-dismiss="modal">Close</button>
            <button type="submit" class="btn btn-primary mission-theme-crud-save-btn" data-bs-dismiss="modal" id="banner-crud-save-btn" onclick="AddBanner('edit', ${id})">Save changes</button>`

    $.ajax({
        type: "GET",
        url: `/api/admin/bannerbyid`,
        data: {
            "id": id
        },
        success: function (response) {

            var bannerImage = document.getElementById('banner-media-edit-files');
            bannerImage.innerHTML = `<img src="/mediaUpload/${response.result.image}" width="200px" height="200px" />`;
            banner = response.result.image;
            $('#banner-text').val(response.result.text);
            $('#sort-order').val(response.result.sortOrder);

        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

async function AddBanner(flag, id) {


    var error = 0;

    var text = $('#banner-text').val();
    var sortOrder = $('#sort-order').val();
    // for checking sort order
    await $.ajax({
        type: "GET",
        url: `/api/admin/checkBannerSort`,
        data: {
            "id": sortOrder
        },
        success: function (response) {

            if (response.result != true) {
                error = 1;
                toastr.error("Sort Value Already Exists");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

    if (error == 0) {

        if (flag == "add") {
            $.ajax({
                type: "POST",
                url: `/api/admin/addBanner`,
                data: {
                    "image": banner,
                    "sortOrder": sortOrder,
                    "text": text
                },
                success: function (response) {
                    toastr.success("Banner Added.");
                    makeDivActive("Banner-Management");
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        } else {
            $.ajax({
                type: "POST",
                url: `/api/admin/addBanner`,
                data: {
                    "image": banner,
                    "sortOrder": sortOrder,
                    "text": text,
                    "BannerId": id
                },
                success: function (response) {
                    toastr.success("Banner Changed.");
                    makeDivActive("Banner-Management");
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        }

    }
    error = 0;
}

function DeleteBanner(id) {
    $.ajax({
        type: "POST",
        url: `/api/admin/deleteBanner`,
        data: {
            "BannerId": id
        },
        success: function (response) {
            toastr.warning("Banner Deleted.");
            makeDivActive("Banner-Management");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

// banner end


// for disabled the input field for change time and goal mission.
$('#mission-type').change(function () {
    var missionType = $('#mission-type').val();
    if (missionType == "Time") {
        $('#mission-objective').prop('disabled', true);
        $('#mission-target').prop('disabled', true);
        $('#mission-registration-deadline').prop('disabled', false);
    } else {
        $('#mission-objective').prop('disabled', false);
        $('#mission-target').prop('disabled', false);
        $('#mission-registration-deadline').prop('disabled', true);
    }
});
