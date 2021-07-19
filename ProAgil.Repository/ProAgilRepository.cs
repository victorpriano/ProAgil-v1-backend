using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;

        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
        
        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedeSociais);

            if(includePalestrantes)
            {
                query = query   
                    .Include(p => p.PalestranteEventos)
                    .ThenInclude(p => p.Palestrante);
            }
            query = query.OrderByDescending(p => p.DataEvento);

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetAllEventoAsyncById(int EventoId, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedeSociais);

            if(includePalestrantes)
            {
                query = query   
                    .Include(p => p.PalestranteEventos)
                    .ThenInclude(p => p.Palestrante);
            }
            query = query.OrderByDescending(p => p.DataEvento)
                .Where(p => p.Id == EventoId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedeSociais);

            if(includePalestrantes)
            {
                query = query   
                    .Include(p => p.PalestranteEventos)
                    .ThenInclude(p => p.Palestrante);
            }
            query = query.OrderByDescending(p => p.DataEvento)
                .Where(p => p.Tema.Contains(tema));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestranteByName(string name, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(c => c.RedeSociais);

            if(includeEventos)
            {
                query = query   
                    .Include(p => p.PalestranteEventos)
                    .ThenInclude(p => p.Evento);
            }
            query = query.OrderBy(p => p.Nome)
                .Where(p => p.Nome.Contains(name));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteAsync(int PalestranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(c => c.RedeSociais)
                .ThenInclude(c => c.Evento);

            if(includeEventos)
            {
                query = query   
                    .Include(p => p.PalestranteEventos)
                    .ThenInclude(p => p.Evento);
            }
            query = query.OrderBy(p => p.Nome)
                .Where(p => p.Id == PalestranteId);

            return await query.FirstOrDefaultAsync();
        }

    }
}