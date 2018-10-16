function ShiftGames() {
    var currGame = $('#CurrentGame');
    var nextGame = $('#NextGame');
    var folGame = $('#FollowingGame');
    
    currGame.val(nextGame.val());
    nextGame.val(folGame.val());
    folGame.val('');
}