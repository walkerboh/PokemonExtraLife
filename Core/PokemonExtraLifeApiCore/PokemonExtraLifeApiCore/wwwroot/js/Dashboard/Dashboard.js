$(function() {
    setInterval(reloadPage, 60000);
});

function reloadPage() {
    if($('#reloadToggle').is(':checked')) {
        location.reload(true)
    }
}