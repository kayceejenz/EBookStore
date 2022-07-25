// Add the following code if you want the name of the file appear on select
$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});
var url = window.location.href.split('?')[0];

for (var i = 0; i < document.links.length; i++) {
    if (document.links[i].href == url) {
        document.links[i].className = 'mm-active';
        document.links[i].parentNode.className = 'mm-active';
    }
}

$(function () {
    $(".appcontainter").addClass("closed-sidebar");
})
