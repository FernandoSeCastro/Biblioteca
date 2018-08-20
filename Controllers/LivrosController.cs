using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LivrosController : Controller
    {
       private readonly DataContext _context;
       public LivrosController(DataContext context)
        {
            _context = context;

            if (_context.Livros.Count() == 0)
            {
                _context.Livros.Add(new Livro { Nome = "Livro 1", Autor ="Autor 1",DataFabricacao="10/05/1994",Ano = 1994 });
                _context.Livros.Add(new Livro { Nome = "Livro 2", Autor ="Autor 2",DataFabricacao="10/05/1994",Ano = 1994 });
                _context.Livros.Add(new Livro { Nome = "Livro 3", Autor ="Autor 3",DataFabricacao="10/05/1994",Ano = 1994 });
                _context.Livros.Add(new Livro { Nome = "Livro 4", Autor ="Autor 4",DataFabricacao="10/05/1994",Ano = 1994 });
                _context.SaveChanges();
            }
        }    

        [HttpGet]
        public List<Livro> GetAll()
        {
            return _context.Livros.OrderBy(i=>i.Nome).ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public Livro GetById(long id)
        {
            var item = _context.Livros.Find(id);
            if (item == null)
            {
                return new Livro();
            }
            return item;
        }   
        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody]Livro item)
        {            
            if(string.IsNullOrEmpty(item.Nome))
                return null;
            _context.Livros.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(long id,[FromBody]Livro item)
        {
            var todo = _context.Livros.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.Nome = item.Nome;
            todo.Autor = item.Autor;

            _context.Livros.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var livro = _context.Livros.Find(id);
            if (livro == null)
            {
                return NotFound();
            }

            _context.Livros.Remove(livro);
            _context.SaveChanges();
            return NoContent();
        }
    }
}