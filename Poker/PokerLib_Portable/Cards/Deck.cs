using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Cards
{
    public class Deck
    {
        private List<Card> _availableCards;
        private Stack<Card> _discard;
        private Random _random;

        public Card NextCard { get { return MoveRandomCardToDiscard(); } }

        public Deck(Random random)
        {
            _random = random;

            _availableCards = new List<Card>();
            foreach (CardRanks rank in Enum.GetValues(typeof(CardRanks)))
            {
                foreach (Suits suit in Enum.GetValues(typeof(Suits)))
                {
                    _availableCards.Add(new Card(rank, suit));
                }
            }

            _discard = new Stack<Card>();
        }

        public Deck() : this(new Random()) { }

        public void Reshuffle()
        {
            _availableCards.AddRange(_discard);
            _discard.Clear();
        }

        private Card MoveRandomCardToDiscard()
        {
            // Make sure we have cards left to move
            if (_availableCards.Count == 0)
            {
                throw new InvalidOperationException("Attempted to draw a card but there are no cards to draw");
            }

            // Between 0 and (_availableCards.Count - 1), inclusive
            int index = _random.Next(_availableCards.Count);
            Card chosenCard = _availableCards[index];
            _availableCards.RemoveAt(index);
            _discard.Push(chosenCard);
            return chosenCard;
        }
    }
}
