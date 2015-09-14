function Card(cardId) {
    this.cardId = cardId;
    this.card = game.getCard(this.cardId);
}
Card.prototype.play = function (side) {
}
Card.prototype.buildCardElement = function () {
    this.cardElement = document.createElement("div");
    this.cardElement.className = "card-item animation-get-from-deck";

    return this.cardElement;
}