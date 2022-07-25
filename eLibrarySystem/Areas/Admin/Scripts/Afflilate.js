$(function () {
    // local search
    $("#localSearchSimple").jsLocalSearch({
        "searchinput": "#gsearchsimple",
        "container": "contsearch",
        "containersearch": "gsearch",
        "action": "Show",
        "html_search": true,
        "mark_text": "si"
    });
})