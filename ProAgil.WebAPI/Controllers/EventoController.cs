using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.WebAPI.Dtos;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repo;
        private IMapper _mapper;

        public EventoController(IProAgilRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetEventos()
        {
            try
            {
                var eventos = await _repo.GetAllEventoAsync(true);

                var results = _mapper.Map<EventoDto[]>(eventos);
                
                return Ok(results);
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventosById(int id)
        {
            try
            {
                var evento = await _repo.GetAllEventoAsyncById(id, true);

                var results = _mapper.Map<EventoDto>(evento);
                
                return Ok(results);
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
            }
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> GetEventoByTema(string tema)
        {
            try
            {
                var eventos = await _repo.GetAllEventoAsyncByTema(tema, true);

                var results = _mapper.Map<EventoDto[]>(eventos);
                
                return Ok(results);
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostEventos(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);

                _repo.Add(evento);
                
                if(await _repo.SaveChangesAsync()) // salva apenas quando o estado for assincrono
                    return Created($"api/evento{evento.Id}", _mapper.Map<Evento>(model)); // retorno do que foi inserido
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Banco Dados Falhou");
            }

            return BadRequest();
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if(file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, fileName.Replace("\"", " ").Trim());

                    using(var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Ok();
            }
            catch(System.Exception ex )
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados falhou {ex.Message}");
            }

            return BadRequest("Erro ao tentar realizar o upload");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventos(int id, EventoDto model)
        {
            try
            {
                var evento = await _repo.GetAllEventoAsyncById(id, false); // verifica e nao retorna

                if(evento == null)
                    return NotFound();
                
                _mapper.Map(model, evento);
                
                _repo.Update(evento);

                if(await _repo.SaveChangesAsync()) // salva apenas quando o estado for assincrono
                    return Created($"api/evento{evento.Id}", evento);
                
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Banco Dados Falhou");
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventos(int id)
        {
            try
            {
                var eventos = await _repo.GetAllEventoAsyncById(id, false); // verifica e nao retorna

                if(eventos == null)
                    return NotFound();
                
                _repo.Delete(eventos);

                if(await _repo.SaveChangesAsync()) // salva apenas quando o estado for assincrono
                    return Ok();
                
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Banco Dados Falhou");
            }

            return BadRequest();
        }

    }
}