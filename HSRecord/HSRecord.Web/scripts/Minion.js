function Minion(cardId) {
    this.prototype = new Card(cardId);
}
Minion.prototype.play = function (side) {
    this.prototype.play(side);
    this.dropOnBoard(side);

    if (this.prototype.card != null) {
        this.setHealth(this.prototype.card.health);
        this.setAttack(this.prototype.card.attack);
        this.setImage(game.getCardImageLink(this.prototype.card.id));
        if (this.isTaunt())
            this.setTaunt();
    }
}
Minion.prototype.dropOnBoard = function (side) {
    var fiendlySideElement = document.getElementById("friendlySide"),
    enemySideElement = document.getElementById("enemySide");

    var container = (side == Constant.Side.Friendly) ? fiendlySideElement : enemySideElement;
    var element = this.buildCardOnBoardElement();
    container.appendChild(element);
}
Minion.prototype.buildCardOnBoardElement = function () {
    this.cardElement = document.createElement("div");
    this.cardElement.className = "minion-on-board";

    this.tauntMaskElement = document.createElement("div");
    this.cardElement.appendChild(this.tauntMaskElement);

    this.iconElement = document.createElement("div");
    this.iconElement.className = "minion-on-board-icon";
    this.cardElement.appendChild(this.iconElement);

    this.attackInfoElement = document.createElement("div");
    this.attackInfoElement.className = "attack minion-on-board-info";
    this.attackInfoElement.innerHTML = this.health || 0;
    this.cardElement.appendChild(this.attackInfoElement);

    this.healthInfoElement = document.createElement("div");
    this.healthInfoElement.className = "health minion-on-board-info";
    this.healthInfoElement.innerHTML = this.health || 0;
    this.cardElement.appendChild(this.healthInfoElement);

    return this.cardElement;
}

Minion.prototype.setHealth = function (health) {
    this.health = health;
    this.healthInfoElement.innerHTML = this.health || 0;
}
Minion.prototype.setAttack = function (attack) {
    this.attack = attack;
    this.attackInfoElement.innerHTML = this.attack || 0;
}
Minion.prototype.setImage = function (link) {
    this.iconElement.style["background-image"] = "url(" + link + ")";
}
Minion.prototype.setTaunt = function () {
    this.tauntMaskElement.className = "taunt";
}
Minion.prototype.isTaunt = function () {
    if (this.prototype.card == null)
        return false;
    if (this.prototype.card.mechanics == null || this.prototype.card.mechanics.length == 0)
        return false;
    return this.prototype.card.mechanics.indexOf(Constant.Mechanics.Taunt) != -1;
}