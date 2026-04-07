using API_LocadoraVeiculos.Data;
using API_LocadoraVeiculos.DTOs.VeiculosDto;
using API_LocadoraVeiculos.Models.Respostas;
using API_LocadoraVeiculos.Models.Veiculos;
using API_LocadoraVeiculos.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_LocadoraVeiculos.Services
{
    public class VeiculoService : IVeiculoInterface
    {
        private readonly AppDbContext _context;

        public VeiculoService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ResponseModel<List<VeiculoResponseDto>>> ListarVeiculos()
        {
            ResponseModel<List<VeiculoResponseDto>> resposta = new();
            try
            {
                var veiculos = await _context.Veiculos
                    .Include(v => v.Categoria)
                    .Include(v => v.Cor)
                    .ToListAsync();

                var dto = veiculos.Select(v => new VeiculoResponseDto
                {
                    Id = v.Id,
                    NomeModelo = v.NomeModelo,
                    Placa = v.Placa,
                    CategoriaNome = v.Categoria.NomeCateg,
                    CorNome = v.Cor.cor,
                    CorHexa = v.Cor.HexaCor,
                    AnoVeiculo = v.AnoVeiculo,
                    ValorDiaria = v.ValorDiaria,
                    Status = v.Status.ToString()
                }).ToList();

                resposta.Dados = dto;
                resposta.Mensagem = "Todos os veículos foram listados.";
                resposta.Sucesso = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Erros = ["Erro interno ao listar veículos - " + ex.Message];
                return resposta;
            }
        }



        public async Task<ResponseModel<VeiculoResponseDto>> BuscarVeiculoPorId(int idVeiculo)
        {
            ResponseModel<VeiculoResponseDto> resposta = new();
            try
            {
                var veiculo = await _context.Veiculos
                    .Include(v => v.Categoria)
                    .Include(v => v.Cor)
                    .FirstOrDefaultAsync(v => v.Id == idVeiculo);

                if (veiculo == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Veículo não encontrado.";
                    return resposta;
                }

                resposta.Dados = new VeiculoResponseDto
                {
                    Id = veiculo.Id,
                    NomeModelo = veiculo.NomeModelo,
                    Placa = veiculo.Placa,
                    CategoriaNome = veiculo.Categoria.NomeCateg,
                    CorNome = veiculo.Cor.cor,
                    CorHexa = veiculo.Cor.HexaCor,
                    AnoVeiculo = veiculo.AnoVeiculo,
                    ValorDiaria = veiculo.ValorDiaria,
                    Status = veiculo.Status.ToString()
                };

                resposta.Mensagem = "Veículo localizado!";
                resposta.Sucesso = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Erros = ["Erro interno ao buscar veículo - " + ex.Message];
                return resposta;
            }
        }


        public async Task<ResponseModel<VeiculoResponseDto>> AdicionarVeiculo(VeiculoRequestDto dto)
        {
            ResponseModel<VeiculoResponseDto> resposta = new();
            try
            {
                // Verifica se a placa já existe
                var placaExiste = await _context.Veiculos.AnyAsync(v => v.Placa == dto.Placa);
                if (placaExiste)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Já existe um veículo com esta placa.";
                    return resposta;
                }

                // Busca a categoria
                var categoria = await _context.Categorias.FindAsync(dto.CategoriaId);
                if (categoria == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Categoria não encontrada.";
                    return resposta;
                }

                // Busca a cor
                var cor = await _context.Cores.FindAsync(dto.CorId);
                if (cor == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Cor não encontrada.";
                    return resposta;
                }

                var novoVeiculo = new VeiculoModel
                {
                    NomeModelo = dto.NomeModelo,
                    Placa = dto.Placa.ToUpper(),
                    Categoria = categoria,
                    Cor = cor,
                    AnoVeiculo = dto.AnoVeiculo,
                    ValorDiaria = dto.ValorDiaria,
                    Status = StatusVeiculo.Disponivel, // sempre começa disponível
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };

                _context.Veiculos.Add(novoVeiculo);
                await _context.SaveChangesAsync();

                resposta.Dados = new VeiculoResponseDto
                {
                    Id = novoVeiculo.Id,
                    NomeModelo = novoVeiculo.NomeModelo,
                    Placa = novoVeiculo.Placa,
                    CategoriaNome = categoria.NomeCateg,
                    CorNome = cor.cor,
                    CorHexa = cor.HexaCor,
                    AnoVeiculo = novoVeiculo.AnoVeiculo,
                    ValorDiaria = novoVeiculo.ValorDiaria,
                    Status = novoVeiculo.Status.ToString()
                };

                resposta.Mensagem = "Veículo adicionado com sucesso!";
                resposta.Sucesso = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Erros = ["Erro interno ao adicionar veículo - " + ex.Message];
                return resposta;
            }
        }

   

        public async Task<ResponseModel<VeiculoResponseDto>> EditarVeiculo(int idVeiculo, VeiculoUpdateDto dto)
        {
            ResponseModel<VeiculoResponseDto> resposta = new();
            try
            {
                var veiculo = await _context.Veiculos
                    .Include(v => v.Categoria)
                    .Include(v => v.Cor)
                    .FirstOrDefaultAsync(v => v.Id == idVeiculo);

                if (veiculo == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Veículo não encontrado.";
                    return resposta;
                }

                // Verifica se a placa já existe (excluindo o próprio veículo)
                var placaExiste = await _context.Veiculos
                    .AnyAsync(v => v.Placa == dto.Placa && v.Id != idVeiculo);
                if (placaExiste)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Já existe outro veículo com esta placa.";
                    return resposta;
                }

                // Busca a categoria
                var categoria = await _context.Categorias.FindAsync(dto.CategoriaId);
                if (categoria == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Categoria não encontrada.";
                    return resposta;
                }

                // Busca a cor
                var cor = await _context.Cores.FindAsync(dto.CorId);
                if (cor == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Cor não encontrada.";
                    return resposta;
                }

                veiculo.NomeModelo = dto.NomeModelo;
                veiculo.Placa = dto.Placa.ToUpper();
                veiculo.Categoria = categoria;
                veiculo.Cor = cor;
                veiculo.AnoVeiculo = dto.AnoVeiculo;
                veiculo.ValorDiaria = dto.ValorDiaria;
                veiculo.Status = dto.Status;
                veiculo.updated_at = DateTime.Now;

                await _context.SaveChangesAsync();

                resposta.Dados = new VeiculoResponseDto
                {
                    Id = veiculo.Id,
                    NomeModelo = veiculo.NomeModelo,
                    Placa = veiculo.Placa,
                    CategoriaNome = categoria.NomeCateg,
                    CorNome = cor.cor,
                    CorHexa = cor.HexaCor,
                    AnoVeiculo = veiculo.AnoVeiculo,
                    ValorDiaria = veiculo.ValorDiaria,
                    Status = veiculo.Status.ToString()
                };

                resposta.Mensagem = "Veículo atualizado com sucesso!";
                resposta.Sucesso = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Erros = ["Erro interno ao editar veículo - " + ex.Message];
                return resposta;
            }
        }



        public async Task<ResponseModel<VeiculoResponseDto>> RemoverVeiculo(int idVeiculo)
        {
            ResponseModel<VeiculoResponseDto> resposta = new();
            try
            {
                var veiculo = await _context.Veiculos
                    .Include(v => v.Categoria)
                    .Include(v => v.Cor)
                    .FirstOrDefaultAsync(v => v.Id == idVeiculo);

                if (veiculo == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Veículo não encontrado.";
                    return resposta;
                }

                // Verifica se o veículo está alugado
                if (veiculo.Status == StatusVeiculo.Alugado)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Não é possível remover um veículo que está alugado.";
                    return resposta;
                }

                var dtoRetorno = new VeiculoResponseDto
                {
                    Id = veiculo.Id,
                    NomeModelo = veiculo.NomeModelo,
                    Placa = veiculo.Placa,
                    CategoriaNome = veiculo.Categoria.NomeCateg,
                    CorNome = veiculo.Cor.cor,
                    CorHexa = veiculo.Cor.HexaCor,
                    AnoVeiculo = veiculo.AnoVeiculo,
                    ValorDiaria = veiculo.ValorDiaria,
                    Status = veiculo.Status.ToString()
                };

                _context.Veiculos.Remove(veiculo);
                await _context.SaveChangesAsync();

                resposta.Dados = dtoRetorno;
                resposta.Mensagem = "Veículo removido com sucesso!";
                resposta.Sucesso = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Erros = ["Erro interno ao remover veículo - " + ex.Message];
                return resposta;
            }
        }
    }
}