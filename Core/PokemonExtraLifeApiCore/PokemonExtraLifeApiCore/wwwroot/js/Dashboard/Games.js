function ShiftGames() {
    var currGame = $('#GamesModel_CurrentGame');
    var nextGame = $('#GamesModel_NextGame');
    var folGame = $('#GamesModel_FollowingGame');
    
    currGame.val(nextGame.val());
    nextGame.val(folGame.val());
    folGame.val('');
}