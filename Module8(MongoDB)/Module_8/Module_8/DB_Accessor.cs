using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module_8
{
    public class DB_Accessor
    {
        static IMongoDatabase db;
        string tableName;
      
        public DB_Accessor(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);
        }
        public void setTableName(string tableName)
        {
            this.tableName = tableName;
        }
        public void Add<T>(T document)
        {
            var collection = db.GetCollection<T>(tableName);
            collection.InsertOne(document);
        }
        public  void AddRecords<T>(IEnumerable<T> documents)
        {    
           var collection = db.GetCollection<T>(tableName);
           collection.InsertMany(documents);
        }
        public IEnumerable<Book> GetBooks()
        {
            var filter = new BsonDocument();
            var collection = db.GetCollection<Book>(tableName);
            return collection.Find(filter).ToList();
        }
        public IEnumerable<Book> GetNameBook(int limit)
        {
            var collection = db.GetCollection<Book>(tableName);
            var filter = Builders<Book>.Filter.Gt("Count", 1);
            return collection.Find(filter).Sort("{Name: 1}").Limit(limit).ToList();
        }

        public Book GetBookMinCount()
        {
            var collection = db.GetCollection<Book>(tableName);
            return collection.Aggregate().SortBy((a) => a.Count).FirstOrDefault();
        }
        public Book GetBookMaxCount()
        {
            var collection = db.GetCollection<Book>(tableName);
            return collection.Aggregate().SortByDescending((a) => a.Count).FirstOrDefault();
        }

        public IQueryable<String> GetAllAuthor()
        {
            var collection = db.GetCollection<Book>(tableName);
            return collection.AsQueryable<Book>().Where(x => x.Author != null).Select(e => e.Author).Distinct();
        }

        public IEnumerable<Book> GetBookWithoutAuthor()
        {
            var collection = db.GetCollection<Book>(tableName);
            var filter = Builders<Book>.Filter.Eq<Book>("Author", null);
            return collection.Find(filter).ToList();
        }

        public void AddOneCountEachBook()
        {
            var collection = db.GetCollection<Book>(tableName);
            var result = collection.Find(x => true).ToList();

            foreach (var item in result)
            {
                var filter = Builders<Book>.Filter.Eq("_id", item.ID);
                var update = Builders<Book>.Update.Set("Count", ++item.Count);
                collection.UpdateOne(filter, update);
            }
        }

        public void AddAdditionalGenge(string mainGenre, string additionalGenre)
        {
            var collection = db.GetCollection<Book>(tableName);
            var result = collection.Find(x => true).ToList().Where(x => x.Genre.Contains(mainGenre) && !x.Genre.Contains(additionalGenre));

            foreach (var item in result)
            {
                var newArrayGenre = item.Genre.ToList();
                newArrayGenre.Add(additionalGenre);
                var filter = Builders<Book>.Filter.Eq("_id", item.ID);
                var update = Builders<Book>.Update.Set("Genre", newArrayGenre.ToArray());
                collection.UpdateOne(filter, update);
            }

        }

        public void DeleteBookWhereCountLess(int count)
        {
            var collection = db.GetCollection<Book>(tableName);
            var result = collection.DeleteMany<Book>(p => p.Count < 3);
        }

        public void DeleteAll()
        {
            var collection = db.GetCollection<Book>(tableName);
            var result = collection.DeleteMany<Book>(p => true);
        }


    }
}
