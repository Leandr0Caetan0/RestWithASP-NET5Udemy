using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Hypermedia.Filters;

namespace RestWithASPNETUdemy.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;

        //Declarando Serviço/Entidade que será utilizado - Books
        private IBookBusiness _bookBusiness;

        public BookController(ILogger<BookController> logger, IBookBusiness booksBusiness)
        {
            _logger = logger;
            _bookBusiness = booksBusiness;
        }

        [HttpGet]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            return Ok(_bookBusiness.FindAll());
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(long id)
        {
            var bookVO = _bookBusiness.FindByID(id);
            if (bookVO == null)
            {
                return NotFound();
            }
            return Ok(bookVO);
        }

        [HttpPost]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] BookVO bookVO)
        {
            if (bookVO == null)
            {
                return BadRequest();
            }
            return Ok(_bookBusiness.Create(bookVO));
        }

        [HttpPut]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] BookVO bookVO)
        {
            if (bookVO == null)
            {
                return BadRequest();
            }
            return Ok(_bookBusiness.Update(bookVO));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _bookBusiness.Delete(id);
            return NoContent();
        }
    }
}
