let SessionExist;


const Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
    }
})


function initFavBooks() {
    // Handle Favorites
    var items = document.getElementsByClassName('product_fav');
    for (var x = 0; x < items.length; x++) {
        var item = items[x];
        item.addEventListener('click', function (fn) {
            CheckIfSessionExists();
            if (SessionExist == true) {
                fn.target.classList.toggle('active');
                let state;
                var id = fn.target.dataset.id;
                let isActive = fn.target.classList.contains("active");
                if (isActive) {
                    state = "activate"
                } else {
                    state = "deactivate";
                }
                AddToWishList(id,state);
            }

        });
    }
}

function AddToWishList(id,state) {
    $.ajax({
        url: "/Home/AddToWishList?bookID=" + id + "&state="+state,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (response) {
            debugger
            switch (response) {
                case "Deactivated":
                    Toast.fire({
                        icon: 'success',
                        title: 'Book has been removed from wishlist'
                    })
                    break;
                case "Activated":
                    Toast.fire({
                        icon: 'success',
                        title: 'Book has been included to wishlist'
                    })
                    break;
                case "Added":
                    Toast.fire({
                        icon: 'success',
                        title: 'Book has been added to wishlist'
                    })
                    break;
            }
          
        },
        error: function () {
            Toast.fire({
                icon: 'error',
                title: 'Operation was not completed'
            })
        }
    })
}




function CheckIfSessionExists() {
    $.ajax({
        url:"/Account/CheckSessionExists",
        type: "Get",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            SessionExist = result;
            if (result == false) {
                var returnUrl = window.location.href.split('?')[0];

                url = "/Account/Login?returnUrl=" + returnUrl;
                SwalAlert(url, true);
            }
            else {
            //    window.location.href = "/Courses/CourseContentHeader?CourseID=" + id;
            }
        }
    })
}

function SwalAlert(optionalUrl, isToRedirect) {
    Swal.fire({
        text: "You can\'t access this feature because you ain\'t login yet ,Do you want to login now?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, go to login!'
    }).then((result) => {
        if (result.value) {
            if (isToRedirect == true) {
                window.location.href= optionalUrl;
            }
            else {

            }
        }
        else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'error'
            )
        }
    })
}

