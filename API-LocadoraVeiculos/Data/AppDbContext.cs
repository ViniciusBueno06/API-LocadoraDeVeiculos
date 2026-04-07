using API_LocadoraVeiculos.Models.Clientes;
using API_LocadoraVeiculos.Models.Locacoes;
using API_LocadoraVeiculos.Models.Pagamentos;
using API_LocadoraVeiculos.Models.Veiculos;
using Microsoft.EntityFrameworkCore;

namespace API_LocadoraVeiculos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<VeiculoModel> Veiculos { get; set; }
        public DbSet<LocacaoModel> Locacoes { get; set; }
        public DbSet<PagamentoModel> Pagamentos { get; set; }
        public DbSet<CategoriaVeiculo> Categorias { get; set; }  // ← Nome simplificado
        public DbSet<CorVeiculo> Cores { get; set; }  // ← Nome simplificado

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Impede CPF duplicado
            modelBuilder.Entity<ClienteModel>()
                .HasIndex(c => c.Cpf)
                .IsUnique();

            // Impede Email duplicado
            modelBuilder.Entity<ClienteModel>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // Impede Telefone duplicado (opcional, pois telefones podem ser compartilhados)
            modelBuilder.Entity<ClienteModel>()
                .HasIndex(c => c.Telefone)
                .IsUnique();



            // Configurar relacionamentos (opcional, mas recomendado)
            modelBuilder.Entity<VeiculoModel>()
                .HasOne(v => v.Categoria)
                .WithMany(c => c.Veiculos)
                .HasForeignKey(v => v.CategoriaId);

            modelBuilder.Entity<VeiculoModel>()
                .HasOne(v => v.Cor)
                .WithMany(c => c.Veiculos)
                .HasForeignKey(v => v.CorId);

            // ─── SEED DE CORES ─────────────────────────────────────────
            modelBuilder.Entity<CorVeiculo>().HasData(
                new CorVeiculo { Id = 1, cor = "Branco", HexaCor = "#FFFFFF" },
                new CorVeiculo { Id = 2, cor = "Preto", HexaCor = "#000000" },
                new CorVeiculo { Id = 3, cor = "Prata", HexaCor = "#C0C0C0" },
                new CorVeiculo { Id = 4, cor = "Vermelho", HexaCor = "#FF0000" },
                new CorVeiculo { Id = 5, cor = "Azul", HexaCor = "#0000FF" },
                new CorVeiculo { Id = 6, cor = "Verde", HexaCor = "#008000" },
                new CorVeiculo { Id = 7, cor = "Amarelo", HexaCor = "#FFFF00" },
                new CorVeiculo { Id = 8, cor = "Cinza", HexaCor = "#808080" },
                new CorVeiculo { Id = 9, cor = "Marrom", HexaCor = "#8B4513" },
                new CorVeiculo { Id = 10, cor = "Bege", HexaCor = "#F5F5DC" }
            );

            // ─── SEED DE CATEGORIAS ─────────────────────────────────────
            modelBuilder.Entity<CategoriaVeiculo>().HasData(
                new CategoriaVeiculo { Id = 1, NomeCateg = "Hatch", Descricao = "Carros compactos e econômicos" },
                new CategoriaVeiculo { Id = 2, NomeCateg = "Sedan", Descricao = "Carros com porta-malas grande, ideais para viagens" },
                new CategoriaVeiculo { Id = 3, NomeCateg = "SUV", Descricao = "Utilitários esportivos, espaço e conforto" },
                new CategoriaVeiculo { Id = 4, NomeCateg = "Picape", Descricao = "Veículos para carga e trabalho" },
                new CategoriaVeiculo { Id = 5, NomeCateg = "Van", Descricao = "Veículos para transporte de grupos" },
                new CategoriaVeiculo { Id = 6, NomeCateg = "Esportivo", Descricao = "Alta performance e design arrojado" },
                new CategoriaVeiculo { Id = 7, NomeCateg = "Luxo", Descricao = "Conforto e sofisticação premium" }
            );
        }
    }
}