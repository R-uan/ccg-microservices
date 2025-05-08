using MongoDB.Driver;
using CardCatalog.Interface;

namespace CardCatalog.Core
{
    public class CardCollectionRepository(IMongoCollection<Card> cardCollection) : ICardCatalogRepository
    {
        public async Task<Card?> FindCard(Guid id)
        {
            var query = await cardCollection.FindAsync(card => card.Id == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Card>> FindCards()
        {
            var query = await cardCollection.FindAsync(x => true);
            return await query.ToListAsync();
        }

        public async Task<List<Card>> FindCards(List<Guid> cardIds)
        {
            var filter = Builders<Card>.Filter.In(c => c.Id, cardIds);
            var result = await cardCollection.FindAsync(filter);
            return await result.ToListAsync();
        }

        public async Task<Card?> SaveCard(Card card)
        {
            await cardCollection.InsertOneAsync(card);
            var s = await cardCollection.FindAsync(cc => cc.Id == card.Id);
            return await s.FirstOrDefaultAsync();
        }

        public async Task<int> SaveMultipleCards(List<Card> cards)
        {
            var before = await cardCollection.CountDocumentsAsync(_ => true);
            await cardCollection.InsertManyAsync(cards);
            var after = await cardCollection.CountDocumentsAsync(_ => true);
            return (int)(after - before);
        }
    }
}