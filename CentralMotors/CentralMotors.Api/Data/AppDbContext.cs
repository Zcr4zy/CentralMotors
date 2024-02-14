using CentralMotors.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CentralMotors.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Cor> Cores { get; set; }
        public DbSet<Fabricante> Fabricantes { get; set; }
        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<Tipo> Tipos { get; set; }
        public DbSet<TipoCombustivel> TipoCombustiveis { get; set; }
        public DbSet<TipoTransmissao> TipoTransmissoes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Seed Cor
            List<Cor> cores = [
                new()
                {
                    CorId = 1,
                    Nome = "Branco"
                },
                new()
                {
                    CorId = 2,
                    Nome = "Preto"
                },
                new()
                {
                    CorId = 3,
                    Nome = "Vermelho"
                },
                new()
                {
                    CorId = 4,
                    Nome = "Azul"
                }
            ];
            builder.Entity<Cor>().HasData(cores);
            #endregion

            #region Seed Fabricantes
            List<Fabricante> fabricantes = [
                new()
                {
                    FabricanteId = 1,
                    Nome = "Chevrolet"
                },
                new()
                {
                    FabricanteId = 2,
                    Nome = "Volkswagen"
                },
                new()
                {
                    FabricanteId = 3,
                    Nome = "Hyundai"
                }
            ];
            builder.Entity<Fabricante>().HasData(fabricantes);
            #endregion

            #region Seed Modelos
            List<Modelo> modelos = [
                new()
                {
                    ModeloId = 1,
                    Nome = "Onix",
                    FabricanteId = 1
                },
                new()
                {
                    ModeloId = 2,
                    Nome = "Cruze",
                    FabricanteId = 1
                },
                new()
                {
                    ModeloId = 3,
                    Nome = "Camaro",
                    FabricanteId = 1
                },
                new()
                {
                    ModeloId = 4,
                    Nome = "Polo",
                    FabricanteId = 2
                },
                new()
                {
                    ModeloId = 5,
                    Nome = "T-Cross",
                    FabricanteId = 2
                },
                new()
                {
                    ModeloId = 6,
                    Nome = "Fusca",
                    FabricanteId = 2
                },
                new()
                {
                    ModeloId = 7,
                    Nome = "HB20",
                    FabricanteId = 3
                },
                new()
                {
                    ModeloId = 8,
                    Nome = "Creta",
                    FabricanteId = 3
                }
            ];

            builder.Entity<Modelo>().HasData(modelos);
            #endregion

            #region Seed Tipos
            List<Tipo> tipos = [
                new()
                {
                    TipoId = 1,
                    Nome = "Moto"
                },
                new()
                {
                    TipoId = 2,
                    Nome = "Carro"
                },
                new()
                {
                    TipoId = 3,
                    Nome = "Caminhão"
                }
            ];
            builder.Entity<Tipo>().HasData(tipos);
            #endregion

            #region Seed TipoCombustivel
            List<TipoCombustivel> tipoCombustiveis = [
                new()
                {
                    TipoCombustivelId = 1,
                    Nome = "Gasolina"
                },
                new()
                {
                    TipoCombustivelId = 2,
                    Nome = "Etanol"
                },
                new()
                {
                    TipoCombustivelId = 3,
                    Nome = "Flex (Etanol ou Gasolina)"
                },
                new()
                {
                    TipoCombustivelId = 4,
                    Nome = "Eletricidade"
                },
                new()
                {
                    TipoCombustivelId = 5,
                    Nome = "Diesel"
                }
            ];
            builder.Entity<TipoCombustivel>().HasData(tipoCombustiveis);
            #endregion

            #region Seed TipoTransmissao
            List<TipoTransmissao> tipoTransmissoes = [
                new()
                {
                    TipoTransmissaoId = 1,
                    Nome = "Manual"
                },
                new()
                {
                    TipoTransmissaoId = 2,
                    Nome = "Automática"
                }
            ];
            builder.Entity<TipoTransmissao>().HasData(tipoTransmissoes);
            #endregion

            #region Seed Veiculo
            List<Veiculo> veiculos = [
                new()
                {
                    VeiculoId = 1,
                    AnoModelo = 2012,
                    AnoFabricacao = 2011,
                    Valor = 55000,
                    KmRodados = "198.000 Km",
                    Motor = "1.8",
                    Localizacao = "GuilhermeVeiculos, Jaú",
                    Foto1 = "https://boffmotors.com.br/carros/5b051b2f43b4e05390f59d5c7783057c-thumbjpeg-chevrolet-cruze-9430459-900-675-70.jpg",
                    Foto2 = "https://img.olx.com.br/images/94/941481244478696.webp",
                    CorId = 1,
                    ModeloId = 2,
                    TipoId = 2,
                    TipoCombustivelId = 3,
                    TipoTransmissaoId = 1
                }
            ];
            builder.Entity<Veiculo>().HasData(veiculos);
            #endregion
        }
    }



}

