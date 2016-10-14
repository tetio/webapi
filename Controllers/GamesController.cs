using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Repositories;
using Bogus;
using System;
using System.IO;

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
        // GET api/games
        [HttpGet]
        public IEnumerable<Game> Get()
        {
            return repository.Get();
        }

        // GET api/games/open
        [HttpGet("Open")]
        public IEnumerable<Game> GetOpenGames()
        {
            return repository.GetOpenGames();
        }


        // GET api/games/5
        [HttpGet("{id}")]
        public Game Get(string id)
        {
            return repository.Get(id);
        }

        //POST api/games
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            var game = repository.gameGenerate();
            repository.InsertGame(game);
            return new JsonResult(game);
        }

        // POST api/games/open
        [HttpPost("Filter")]
        public IActionResult Filter(int? id)
        {

            string jsonData = new StreamReader(Request.Body).ReadToEnd();
            return new JsonResult(repository.Filter(jsonData));
        }

        // POST api/games/open
        [HttpPost("Join")]
        public IActionResult Join(int? id)
        {

            string jsonPlayer = new StreamReader(Request.Body).ReadToEnd();
            this.repository.FindCreatedAndBlock(jsonPlayer);

            //return new JsonResult(repository.Filter(jsonData));
            return null;
        }        

        // PUT api/games/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/games/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }



    }
}
