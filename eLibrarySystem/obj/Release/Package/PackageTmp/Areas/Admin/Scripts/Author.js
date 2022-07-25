function Edit(Id) {
    $.ajax({
        url: "/Author/GetAuthor/" + Id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#Id').val(result.Id);
            $('#Name').val(result.Name);
            $('#DateOfBirth').val(moment(result.DateOfBirth).format("LL"));
            $('#CountryOfOrigin').val(result.CountryOfOrigin);
            $('#StateOfOrigin').val(result.StateOfOrigin);
            $('#ContactAddress').val(result.ContactAddress);
            $('#Biography').val(result.Biography);
            $('#BackPage').attr("src", "data:image/gif;base64," + result.ImageString);
        },
        error: function (errormessage) {
            var message = errormessage.responseText;
            toastr.error(message, "Data not retrieved successfully", { showDuration: 500 })
        }
    });
}
const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
function Prompt(ID) {
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
                url: "/Author/DeleteAuthor/" + ID,
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
$(function () {
    $("#DateOfBirth").change(function () {
        var dob = $("#DateOfBirth").val();
        $("#DateOfBirth").val(moment(dob).format("LL"));
    })
})
