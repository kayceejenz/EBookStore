$(function () {


    $('input:radio[name="QuestionType"]').change(
        function () {
            if (this.checked && this.value == 'Multi_Choice') {
                $("#OptionDIV").slideDown();
                $("#CorrectAnswerDIV").slideDown();
            } else if (this.checked && this.value == 'Fill_in_the_blank') {
                $("#OptionDIV").slideUp();
                $("#CorrectAnswerDIV").slideDown();
            } else {
                $("#OptionDIV").slideUp();
                $("#CorrectAnswerDIV").slideUp();
            }
        });

    var choice = $("input:radio[name='QuestionType']");
    $.each(choice,
        function (i, item) {
            if (item.checked && item.id == 'Multi_Choice') {
                $("#OptionDIV").slideDown();
                $("#CorrectAnswerDIV").slideDown();
            }
            else if (item.checked && item.id == 'Fill_in_the_blank') {
                $("#OptionDIV").slideUp();
                $("#CorrectAnswerDIV").slideDown();
            }
        });

});
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
                url: "/QuestionBank/DeleteQuestion/" + ID,
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
