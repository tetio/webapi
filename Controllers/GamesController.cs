using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Repositories;
using Bogus;
using System;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        protected GamesRepository repository;

        public GamesController(IDBContext dbContext)
        {
            repository = new GamesRepository(dbContext);
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<Game> Get()
        {
            return repository.Get();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Game Get(string id)
        {
            return repository.Get(id);
        }

        //POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            var game = this.gameGenerate();
            repository.InsertGame(game);
            return new JsonResult(game);
            //return new ObjectResult(game);§
        }

        // public void Post([FromBody]string value)
        // {
        //     var name = new Bogus.DataSets.Name();
        //     var game = new Game() {owner = name.FirstName(), maxPlayers = 3, type = "TEST"};
        //     repository.InsertGame(game);
        // }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        private Game gameGenerate()
        {
            var numPlayers = new Bogus.DataSets.Commerce().Random.Int(1, 3);
            var numMovements = new Bogus.DataSets.Commerce().Random.Int(1, 5);
            var name = new Bogus.DataSets.Name();
            var word = new Bogus.DataSets.Hacker();
            var players = new List<Player>();
            for (var i = 0; i < numPlayers; i++)
            {
                var movements = new List<Movement>();
                for (var j = 0; j < numMovements; j++)
                {
                    movements.Add(new Movement() { playedAt = DateTime.UtcNow, word = word.Noun() });
                }
                players.Add(new Player() { username = name.FirstName(), joinedAt = DateTime.UtcNow, movements = movements });
            }
            return  new Game() { owner = name.FirstName(), maxPlayers = 3, type = "TEST", createdAt = DateTime.UtcNow, players = players };
        }
    }
}
