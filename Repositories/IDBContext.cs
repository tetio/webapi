using MongoDB.Driver;
using webapi.Models;

namespace webapi.Repositories
{
    public interface IDBContext
    {
        IMongoClient Client { get; }
        IMongoCollection<Game> Games { get; }
    }
}