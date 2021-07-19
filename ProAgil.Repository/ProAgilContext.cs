using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Domain.Identity;

namespace ProAgil.Repository
{
    public class ProAgilContext : IdentityDbContext<User, Role, int, // contextos de autenticação e autorização que serão abordados na aplicação
                                                    IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                                                    IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ProAgilContext(DbContextOptions<ProAgilContext> options) : base(options)
        {
            
        }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<PalestranteEvento> PalestranteEventos { get; set; }
        public DbSet<RedeSocial> RedeSociais { get; set; }

    // relação NxN
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Relacionando os "papéis" dos identity
            modelBuilder.Entity<UserRole>(userRoles => 
                {
                    userRoles.HasKey(ur => new { ur.UserId, ur.RoleId });

                    userRoles.HasOne(ur => ur.Role)
                            .WithMany(r => r.UserRoles)
                            .HasForeignKey(ur => ur.RoleId)
                            .IsRequired();
                    
                    userRoles.HasOne(ur => ur.User)
                            .WithMany(r => r.UserRoles)
                            .HasForeignKey(ur => ur.UserId)
                            .IsRequired();
                }
            );

            modelBuilder.Entity<PalestranteEvento>()
                .HasKey(PE => new { PE.EventoId, PE.PalestranteId });
        }
    }
}