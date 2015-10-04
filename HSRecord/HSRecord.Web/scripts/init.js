$('.flip').click(function () {
    $(this).find('.card').addClass('flipped').mouseleave(function () {
        $(this).removeClass('flipped');
    });
    return false;
});

var game = new Game();
game.init();

var friendlyMinions = [
        new Minion("GVG_051"),
        new Minion("GVG_048"),
        new Minion("GVG_093"),
        new Minion("GVG_037"),
        new Minion("GVG_046"),
        new Minion("GVG_039"),
        new Minion("GVG_040")
],
    enemyMinions = [
        new Minion("GVG_011"),
        new Minion("GVG_009"),
        new Minion("GVG_020"),
        new Minion("GVG_013"),
        new Minion("GVG_014"),
        new Minion("GVG_018"),
        new Minion("GVG_016")
    ];
for (var i = 0; i < friendlyMinions.length; i++) {
    friendlyMinions[i].play(Constant.Side.Friendly);
}
for (var i = 0; i < enemyMinions.length; i++) {
    enemyMinions[i].play(Constant.Side.Enemy);
}

game.drawCard("GVG_051");
game.drawCard("GVG_016");
game.drawCard("GVG_018");
game.drawCard("GVG_013");

game.drawCard("GVG_048");
game.drawCard("GVG_093");
game.drawCard("GVG_037");
game.drawCard("GVG_040");