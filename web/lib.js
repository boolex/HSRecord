function Game(){
}
Game.prototype.loadCards = function(){
	var self = this;
	jQuery.ajax({
         url:    "http://hearthstonejson.com/json/AllSets.json",
         success: function(result) {
                      self.cards = result;
                  },
         async:   false
    });    
}
Game.prototype.getCard = function(cardId){
	for(var set in this.cards){
		for(var i = 0; i < this.cards[set].length; i++)
			if(this.cards[set][i]["id"] == cardId)
				return this.cards[set][i];
	}
}
function Card(cardId){
	this.cardId = cardId;
	this.card = game.getCard(this.cardId);		
}
Card.prototype.play = function(side){
}
function Minion(cardId){
	this.prototype = new Card(cardId);
}
Minion.prototype.play = function(side){
	this.prototype.play(side);
	this.dropOnBoard(side);
	
	if(this.prototype.card != null){
		this.setHealth(this.prototype.card.health);
		this.setAttack(this.prototype.card.attack);
	}
}	
Minion.prototype.dropOnBoard = function(side){
	var fiendlySideElement = document.getElementById("friendlySide"),
    enemySideElement = document.getElementById("enemySide");
	
	var container = (side == Constant.Side.Friendly) ? fiendlySideElement : enemySideElement;
	var element = this.buildCardElement();
	container.appendChild(element);
}
Minion.prototype.buildCardElement = function(){
	this.cardElement = document.createElement("div");
	this.cardElement.className = "card";
	
	this.attackInfoElement = document.createElement("div");
	this.attackInfoElement.className = "attack card-info";
	this.attackInfoElement.innerHTML = this.health || 0;
	this.cardElement.appendChild(this.attackInfoElement);
	
	this.healthInfoElement = document.createElement("div");
	this.healthInfoElement.className = "health card-info";
	this.healthInfoElement.innerHTML = this.health || 0;
	this.cardElement.appendChild(this.healthInfoElement);
	
	return this.cardElement;
}

Minion.prototype.setHealth = function(health){
	this.health = health;
	this.healthInfoElement.innerHTML = this.health || 0;
}
Minion.prototype.setAttack = function(attack){
	this.attack = attack;
	this.attackInfoElement.innerHTML = this.attack || 0;
}