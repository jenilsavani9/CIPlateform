// story listing function
function loadStory(response) {

    return response.storys.map(item => {
        var dots = "...";
        var limit = 150;
        var desc = "No Description."
        if (item.story.description != null && item.story.description.length > limit) {
            // you can also use substr instead of substring
            desc = item.story.description.substring(0, limit) + dots;
        }
        var storyPath = "/img/404-Page-image.png";
        if (item.storyMedia != null && item.storyMedia.storyPath != null) {
            storyPath = "/mediaUpload/" + item.storyMedia.storyPath;
        }
        
        return `<div class="item col-12 col-md-6 col-xl-4 mt-4">
                    <div class="thumbnail card">
                        <div class="img-event story-page-card-top">
                            <img class="group list-group-image img-fluid" style="width:100%;"
                                src="${storyPath}" alt="" />
                            <div class="story-page-card-top-btn d-none">
                                <a class="story-page-btn-img" href="/story/${item.story.storyId}">View Details<img class="ms-2"
                                        src="./img/right-arrow2.png" alt=""></a>
                            </div>

                            <div class="story-page-bottom-center">${item.theme.title}</div>
                        </div>
                        <div class="caption card-body">
                            <h4 class="group card-title inner list-group-item-heading">
                                ${item.story.title}</h4>
                            <p class="group inner landing-page-list-group-item-text">
                                ${desc}
                            </p>
                            <div class="d-flex align-items-center">
                                <img class="card-body-user-img" src="./img/${item.user.avatar}" alt="">
                                <div class="ms-2">${item.user.firstName} ${item.user.lastName}</div>
                            </div>
                        </div>
                    </div>
                </div>`
    }).join("")
}


// function for story pagination
function loadStoryForPagination(response) {

    var previous = `<li class="page-item story-paginaition-item" onclick="onClickStoryPagination(${response.currentStoryPage <= 0 ? response.currentStoryPage : response.currentStoryPage - 1})" style="cursor:pointer;">
                    <a class="page-link" aria-label="Previous">
                        <span aria-hidden="true">&laquo;</span>
                    </a>
                </li>`

    var forword = `<li class="page-item story-paginaition-item" onclick="onClickStoryPagination(${(response.currentStoryPage + 1) >= response.totalStoryPage ? response.currentStoryPage : response.currentStoryPage + 1})" style="cursor:pointer;" >
                        <a class="page-link" aria-label="Next">
                            <span aria-hidden="true" >&raquo;</span>
                        </a>
                    </li>`
    
    var pages = ""
    for (var i = 0; i < response.totalStoryPage ; i++) {
        pages += `<li class="page-item story-paginaition-item ${response.currentStoryPage == i ? "active" : ""}" onclick="onClickStoryPagination(${i})"><a class="page-link" style="cursor:pointer;">${i + 1}</a></li> `
    }

    return previous + pages + forword;
    
}

//onclick for pagination
function onClickStoryPagination(page) {

    $.ajax({
        type: "GET",
        url: `/api/story?page=${page}`,
        success: function (response) {

            var element = document.getElementById("story-page-story-cards");
            element.innerHTML = loadStory(response)

            var element2 = document.getElementById("story-page-pagination");
            element2.innerHTML = loadStoryForPagination(response);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}


$.ajax({
    type: "GET",
    url: `/api/story?page=0`,
    success: function (response) {
        
        var element = document.getElementById("story-page-story-cards");
        element.innerHTML = loadStory(response)

        var element2 = document.getElementById("story-page-pagination");
        element2.innerHTML = loadStoryForPagination(response);
    },
    failure: function (response) {
        alert(response.responseText);
    },
    error: function (response) {
        alert(response.responseText);
    }
});