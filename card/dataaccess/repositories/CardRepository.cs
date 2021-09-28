using card.dataaccess.entities;
using card.dto.requests;

namespace card.dataaccess.repositories
{
    public interface ICardRepository
    {
        CardEntity FindByCustomerId(Customer customer);
    }
    public class MockCardRepository : ICardRepository
    {
        public CardEntity FindByCustomerId(Customer customer)
        {
            return new CardEntity { CustomerId = "1" };
        }
    }
}