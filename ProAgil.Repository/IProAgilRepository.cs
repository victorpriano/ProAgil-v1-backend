using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         void Delete<T> (T entity) where T : class;
         Task<bool> SaveChangesAsync();

         Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes);
         Task<Evento[]> GetAllEventoAsync(bool includePalestrantes);
         Task<Evento> GetAllEventoAsyncById(int EventoId, bool includePalestrantes);

         Task<Palestrante[]> GetAllPalestranteByName(string name, bool includeEventos);
         Task<Palestrante> GetPalestranteAsync(int PalestranteId, bool includeEventos);
         

    }
}