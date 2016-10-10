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

        public GamesController() {
            repository = new GamesRepository();
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<GameModel> Get()
        {
            return repository.Get();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public GameModel Get(string id)
        {
            return repository.Get(id);
        }

        //POST api/values
        [HttpPost]
         public IActionResult Post([FromBody]string value)        
         {
             var name = new Bogus.DataSets.Name();
             var game = new GameModel() {owner = name.FirstName(), maxPlayers = 3, type = "TEST", createdAt = DateTime.UtcNow};
             repository.InsertGame(game);
             return new JsonResult(game);
            //return new ObjectResult(game);
         }

        // public void Post([FromBody]string value)
        // {
        //     var name = new Bogus.DataSets.Name();
        //     var game = new GameModel() {owner = name.FirstName(), maxPlayers = 3, type = "TEST"};
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
    }
}
