using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Data.VO;

namespace RestWithASPNETUdemy.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;

        //Declarando Serviço/Entidade que será utilizado - Books
        private IBooksBusiness _booksBusiness;

        public BooksController(ILogger<BooksController> logger, IBooksBusiness booksBusiness)
        {
            _logger = logger;
            _booksBusiness = booksBusiness;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_booksBusiness.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var bookVO = _booksBusiness.FindByID(id);
            if (bookVO == null)
            {
                return NotFound();
            }
            return Ok(bookVO);
        }

        [HttpPost]
        public IActionResult Post([FromBody] BooksVO bookVO)
        {
            if (bookVO == null)
            {
                return BadRequest();
            }
            return Ok(_booksBusiness.Create(bookVO));
        }

        [HttpPut]
        public IActionResult Put([FromBody] BooksVO bookVO)
        {
            if (bookVO == null)
            {
                return BadRequest();
            }
            return Ok(_booksBusiness.Update(bookVO));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _booksBusiness.Delete(id);
            return NoContent();
        }
    }
}
