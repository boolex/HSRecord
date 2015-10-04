function Hand(side) {
	this.side = side;
	this.htmlElement = document.getElementById((side == Constant.Side.Friendly)? "friendlyHand" : "enemyHand" );
	this.cards = [];
}
Hand.prototype.put = function(card, cardElement){
	this.htmlElement.appendChild(cardElement);
	this.cards.push(card);
	this.htmlElement.className = "hand " + "hand-cards-" + this.cards.length;	
}