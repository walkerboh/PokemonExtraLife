// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function ShiftGames() {
    var currGame = $('#GamesModel_CurrentGame');
    var nextGame = $('#GamesModel_NextGame');
    var folGame = $('#GamesModel_FollowingGame');
    
    currGame.val(nextGame.val());
    nextGame.val(folGame.val());
    folGame.val('');
}

$(function() {
    setInterval(reloadPage, 60000);
});

function reloadPage() {
    if($('#reloadToggle').is(':checked')) {
        location.reload(true)
    }
}