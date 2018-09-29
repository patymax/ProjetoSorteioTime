using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using Modelo;

namespace Entity
{

    public class Context : DbContext
    {
        public Context() : base("Projeto") { }

        public System.Data.Entity.DbSet<Modelo.Time> Times { get; set; }

        public System.Data.Entity.DbSet<Modelo.Pontuacao> Pontuacao { get; set; }

        public System.Data.Entity.DbSet<Modelo.Rodada> Rodada { get; set; }      



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Primeira geração cod fist, comentar linha para que as tabelas sejam criadas
            Database.SetInitializer<Context>(null);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
                      
        
    }
}