using Microsoft.EntityFrameworkCore;
using ReserveiAPI.Objects.Models.Entities;
using ReserveiAPI.Context.Builders;

namespace ReserveiAPI.Context
{
    public class AppDBContext : DbContext
    {
        //Mapeando relacional dos objetos do banco de dados
        public AppDBContext(DbContextOptions<AppDBContext>options): base(options) { }
        //Conjunto: Usuário
        public DbSet<UserModel> Users { get; set; }

        //fluent APi    

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
        base.OnModelCreating(modelBuilder);

            //Entidades de Usuario
            UserBuilder.Build(modelBuilder);
        }
    }
}
