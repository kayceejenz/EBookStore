﻿function EditCategory(Id) {
    $.ajax({
        url: "/ApplicationSettings/GetCategory/" + Id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#IdEdit').val(result.Id);
            $('#DescriptionEdit').val(result.Description);
            $('#ContentInformationEdit').val(result.ContentInformation);
        },
        error: function (errormessage) {
            var message = errormessage.responseText;
            toastr.error(message, "Data not retrieved successfully", { showDuration: 500 })
        }
    });
}
function DeleteCategory(ID) {
    Swal.fire({
        title: 'Confirmation',
        text: "Are you sure, you want to delete this?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/APplicationSettings/DeleteCategory/" + ID,
                type: "Post",
                contentType: "application/json;charset=UTF-8",
                dataType: "json",
                success: function () {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Deleted successfully',
                        showConfirmButton: false,
                        timer: 1500
                    })
                    window.location.reload(true);
                },
                error: function () {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'Delete Failed',
                        showConfirmButton: false,
                        timer: 1500
                    })
                }
            })
        }
        else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Deactivation cancelled :)',
                'error'
            )
        }
    })
}

function EditSubCategory(Id) {
    $.ajax({
        url: "/ApplicationSettings/GetSubCategory/" + Id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#IdSEdit').val(result.Id);
            $('#DescriptionSEdit').val(result.Description);
            $('#ContentInformationSEdit').val(result.ContentInformation);
            $('#ParentIDSEdit').val(result.ParentID);
        },
        error: function (errormessage) {
            var message = errormessage.responseText;
            toastr.error(message, "Data not retrieved successfully", { showDuration: 500 })
        }
    });
}
function DeleteSubCategory(ID) {
    Swal.fire({
        title: 'Confirmation',
        text: "Are you sure, you want to delete this?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/APplicationSettings/DeleteSubCategory/" + ID,
                type: "Post",
                contentType: "application/json;charset=UTF-8",
                dataType: "json",
                success: function () {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Deleted successfully',
                        showConfirmButton: false,
                        timer: 1500
                    })
                    window.location.reload(true);
                },
                error: function () {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'Delete Failed',
                        showConfirmButton: false,
                        timer: 1500
                    })
                }
            })
        }
        else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Deactivation cancelled :)',
                'error'
            )
        }
    })
}
const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
