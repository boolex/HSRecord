function Game() {
    this.board = new Board();
}
Game.prototype.init = function () {
    this.loadCards();
    this.loadCardId2ImageMap();
}
Game.prototype.loadCards = function () {
    var self = this;
    jQuery.ajax({
        url: "http://hearthstonejson.com/json/AllSets.json",
        success: function (result) {
            self.cards = result;
        },
        async: false
    });
}
Game.prototype.loadCardId2ImageMap = function () {
    //var self = this;
    //jQuery.ajax({
    //    url: "http://localhost:81/data/cardimagemap.js",
    //    success: function (result) {
    //        self.cardId2ImageMap = result;
    //    },
    //    async: false
    //});
}
Game.prototype.getCard = function (cardId) {
    for (var set in this.cards) {
        for (var i = 0; i < this.cards[set].length; i++)
            if (this.cards[set][i]["id"] == cardId)
                return this.cards[set][i];
    }
}
Game.prototype.getCardImageLink = function (cardId) {
    return cardId2ImageMap[cardId];
}
Game.prototype.drawCard=function(cardId, side) {
    var hand = this.getHand(side);
    var deck = this.getDeck(side);
    var card = new Card(cardId);
    var cardElement = card.buildCardElement();
    cardElement.style["background-image"] = "url(" + this.getCardImageLink(cardId) + ")";
    var board = this.getBoard();
    board.htmlElement.appendChild(cardElement);
    //showCardAnimation(deck);
    //putCarInHandAnimation(hand);
}
Game.prototype.getHand = function (side) {

}
Game.prototype.getBoard=function() {
    return this.board;
}
Game.prototype.getDeck = function (side) {

}