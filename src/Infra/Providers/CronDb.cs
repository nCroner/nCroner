using LiteDB;
using nCroner.Core.Tools;

namespace Infra.Providers;

    public class CronDb : IDb
    {
        private LiteDatabase? _db;

        public void Dispose()
        {
            _db?.Dispose();
            _db = null;
            GC.SuppressFinalize(this);
        }

        public ILiteCollection<T> GetCollection<T>(Guid id, string collectionName)
        {
            _db = new LiteDatabase($"data\\{id}.db");
            return _db.GetCollection<T>(collectionName);
        }

        public ILiteCollection<T> GetGlobalCollection<T>(string name, string collectionName)
        {
            collectionName = collectionName
                .Replace(" ", "")
                .Replace("?", "")
                .Replace("*", "")
                .Replace("\\", "")
                .Replace("/", "");
            _db = new LiteDatabase($"data\\{name}.db");
            return _db.GetCollection<T>(collectionName);
        }
    }
