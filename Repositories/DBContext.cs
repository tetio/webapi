﻿using MongoDB.Driver;
using webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Repositories
{
    public class DBContext : IDBContext
    {
        public const string CONNECTION_STRING_NAME = "lw";
        public const string DATABASE_NAME = "lw";
        public const string GAMES_COLLECTION_NAME = "games";

        // This is ok... Normally, these or the entire BlogContext
        // would be put into an IoC container. We aren't using one,
        // so we'll just keep them statically here as they are 
        // thread-safe.
        private static readonly IMongoClient client;
        private static readonly IMongoDatabase database;

        static DBContext()
        {
            var env = Environment.GetEnvironmentVariable("NODE_ENV");
            Console.WriteLine("NODE_ENV:   " + env);

            // TODO get db connection string for each environment            
            if (env.Equals(@"development"))
            {
                Console.WriteLine("NODE_ENV:   1");
                client = new MongoClient("mongodb://172.16.19.17:30481");
            } else if (env.Equals(@"local"))
            {
                Console.WriteLine("NODE_ENV:   2");
                client = new MongoClient("mongodb://localhost:27017");
            }
            else
            {
                Console.WriteLine("NODE_ENV:   3");
                client = new MongoClient("mongodb://localhost:27017");
            }
            database = client.GetDatabase(DATABASE_NAME);
        }

        public IMongoClient Client
        {
            get { return client; }
        }

        public IMongoCollection<Game> Games
        {
            get { return database.GetCollection<Game>(GAMES_COLLECTION_NAME); }
        }
    }
}
