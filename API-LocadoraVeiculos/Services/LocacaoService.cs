using API_LocadoraVeiculos.Data;
using API_LocadoraVeiculos.DTOs.LocacoesDtos;
using API_LocadoraVeiculos.DTOs.PagamentosDto;
using API_LocadoraVeiculos.Models.Locacoes;
using API_LocadoraVeiculos.Models.Pagamentos;
using API_LocadoraVeiculos.Models.Respostas;
using API_LocadoraVeiculos.Models.Veiculos;
using API_LocadoraVeiculos.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_LocadoraVeiculos.Services
{
    public class LocacaoService : ILocacaoInterface
    {
        private readonly AppDbContext _context;

        public LocacaoService(AppDbContext context)
        {
            _context = context;
        }

        // ─── LISTAR TODAS ─────────────────────────────────────────────────────────────

        public async Task<ResponseModel<List<LocacaoResponseDto>>> ListarLocacoes()
        {
            ResponseModel<List<LocacaoResponseDto>> resposta = new();
            try
            {
                var locacoes = await _context.Locacoes
                    .Include(l => l.Cliente)
                    .Include(l => l.Veiculo)
                    .ToListAsync();

                var dto = locacoes.Select(l => new LocacaoResponseDto
                {
                    Id = l.Id,
                    NomeCliente = l.Cliente.Nome,
                    NomeVeiculo = l.Veiculo.NomeModelo,
                    Placa = l.Veiculo.Placa,
                    DataInicio = l.DataInicio,
                    DataFimPrevista = l.DataFimPrevista,
                    DataFimReal = l.DataFimReal,
                    ValorTotal = l.ValorTotal,
                    Status = l.Status
                }).ToList();

                resposta.Dados = dto;
                resposta.Mensagem = "Todas as locações foram coletadas.";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Erros = ["Erro interno ao listar locações - " + ex.Message];
                return resposta;
            }
        }

        // ─── BUSCAR POR ID ────────────────────────────────────────────────────────────

        public async Task<ResponseModel<LocacaoResponseDto>> BuscarLocacaoPorId(int idLocacao)
        {
            ResponseModel<LocacaoResponseDto> resposta = new();
            try
            {
                var locacao = await _context.Locacoes
                    .Include(l => l.Cliente)
                    .Include(l => l.Veiculo)
                    .FirstOrDefaultAsync(l => l.Id == idLocacao);

                if (locacao == null)
                {
                    resposta.Mensagem = "Locação não encontrada.";
                    return resposta;
                }

                resposta.Dados = new LocacaoResponseDto
                {
                    Id = locacao.Id,
                    NomeCliente = locacao.Cliente.Nome,
                    NomeVeiculo = locacao.Veiculo.NomeModelo,
                    Placa = locacao.Veiculo.Placa,
                    DataInicio = locacao.DataInicio,
                    DataFimPrevista = locacao.DataFimPrevista,
                    DataFimReal = locacao.DataFimReal,
                    ValorTotal = locacao.ValorTotal,
                    Status = locacao.Status
                };

                resposta.Mensagem = "Locação localizada!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Erros = ["Erro interno ao buscar locação - " + ex.Message];
                return resposta;
            }
        }

        // ─── CRIAR ────────────────────────────────────────────────────────────────────

        public async Task<ResponseModel<LocacaoResponseDto>> CriarLocacao(LocacaoRequestDto dto)
        {
            ResponseModel<LocacaoResponseDto> resposta = new();
            try
            {
                var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
                if (cliente == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Cliente não encontrado.";
                    return resposta;
                }

                var veiculo = await _context.Veiculos.FindAsync(dto.VeiculoId);
                if (veiculo == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Veículo não encontrado.";
                    return resposta;
                }

                if (veiculo.Status != StatusVeiculo.Disponivel)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Veículo não está disponível para locação.";
                    return resposta;
                }

                int diarias = (dto.DataFimPrevista.Date - dto.DataInicio.Date).Days;
                if (diarias <= 0)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "A data de fim prevista deve ser posterior à data de início.";
                    return resposta;
                }

                var novaLocacao = new LocacaoModel
                {
                    Cliente = cliente,
                    Veiculo = veiculo,
                    DataInicio = dto.DataInicio,
                    DataFimPrevista = dto.DataFimPrevista,
                    ValorTotal = diarias * veiculo.ValorDiaria,
                    Status = StatusLocacao.Ativa,
                    created_up = DateTime.Now,
                    updated_up = DateTime.Now
                };

                veiculo.Status = StatusVeiculo.Alugado;

                _context.Locacoes.Add(novaLocacao);

     
                var novoPagamento = new PagamentoModel
                {
                    Locacao = novaLocacao,
                    Valor = novaLocacao.ValorTotal,
                    MetodoPag = dto.MetodoPag,
                    Status = StatusPagamento.Pendente, // sempre começa pendente
                    Data_pagamento = DateTime.MinValue,        // será preenchido ao realizar o pagamento
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };

                _context.Pagamentos.Add(novoPagamento);
                

                await _context.SaveChangesAsync();

                resposta.Dados = new LocacaoResponseDto
                {
                    Id = novaLocacao.Id,
                    NomeCliente = cliente.Nome,
                    NomeVeiculo = veiculo.NomeModelo,
                    Placa = veiculo.Placa,
                    DataInicio = novaLocacao.DataInicio,
                    DataFimPrevista = novaLocacao.DataFimPrevista,
                    ValorTotal = novaLocacao.ValorTotal,
                    Status = novaLocacao.Status
                };

                resposta.Mensagem = "Locação e pagamento criados com sucesso!";
                resposta.Sucesso = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Erros = [ex.Message];
                resposta.Mensagem = "Erro interno do servidor.";
                return resposta;
            }
        }

        

        public async Task<ResponseModel<LocacaoResponseDto>> EditarLocacao(int idLocacao, LocacaoUpdateDto dto)
        {
            ResponseModel<LocacaoResponseDto> resposta = new();
            try
            {
                var locacao = await _context.Locacoes
                    .Include(l => l.Cliente)
                    .Include(l => l.Veiculo)
                    .FirstOrDefaultAsync(l => l.Id == idLocacao);

                if (locacao == null)
                {
                    resposta.Mensagem = "Locação não encontrada.";
                    return resposta;
                }

                if (locacao.Status != StatusLocacao.Ativa)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Somente locações ativas podem ser editadas.";
                    return resposta;
                }

                int diarias = (dto.DataFimPrevista.Date - locacao.DataInicio.Date).Days;
                if (diarias <= 0)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "A data de fim prevista deve ser posterior à data de início.";
                    return resposta;
                }

                locacao.DataFimPrevista = dto.DataFimPrevista;
                locacao.ValorTotal = diarias * locacao.Veiculo.ValorDiaria;
                locacao.updated_up = DateTime.Now;

                await _context.SaveChangesAsync();

                resposta.Dados = new LocacaoResponseDto
                {
                    Id = locacao.Id,
                    NomeCliente = locacao.Cliente.Nome,
                    NomeVeiculo = locacao.Veiculo.NomeModelo,
                    Placa = locacao.Veiculo.Placa,
                    DataInicio = locacao.DataInicio,
                    DataFimPrevista = locacao.DataFimPrevista,
                    DataFimReal = locacao.DataFimReal,
                    ValorTotal = locacao.ValorTotal,
                    Status = locacao.Status
                };

                resposta.Mensagem = "Locação atualizada com sucesso!";
                resposta.Sucesso = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Erros = ["Erro interno do servidor - " + ex.Message];
                return resposta;
            }
        }


        public async Task<ResponseModel<LocacaoResponseDto>> EncerrarLocacao(int idLocacao)
        {
            ResponseModel<LocacaoResponseDto> resposta = new();
            try
            {
                var locacao = await _context.Locacoes
                    .Include(l => l.Cliente)
                    .Include(l => l.Veiculo)
                    .FirstOrDefaultAsync(l => l.Id == idLocacao);

                if (locacao == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Locação não encontrada.";
                    return resposta;
                }

                if (locacao.Status != StatusLocacao.Ativa)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Esta locação já foi encerrada.";
                    return resposta;
                }

               //so permite encerrar se o pagamento foi realizado
                var pagamento = await _context.Pagamentos
                    .FirstOrDefaultAsync(p => p.Locacao.Id == idLocacao);

                if (pagamento == null || pagamento.Status != StatusPagamento.Realizado)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Não é possível encerrar a locação. O pagamento ainda não foi confirmado.";
                    return resposta;
                }

                // Recalcula o valor real com base na devolução efetiva
                int diasReais = (DateTime.Now.Date - locacao.DataInicio.Date).Days;
                if (diasReais < 1) diasReais = 1; // mínimo 1 diária

                locacao.DataFimReal = DateTime.Now;
                locacao.ValorTotal = diasReais * locacao.Veiculo.ValorDiaria;
                locacao.Status = StatusLocacao.Finalizada;
                locacao.updated_up = DateTime.Now;

                locacao.Veiculo.Status = StatusVeiculo.Disponivel;

                await _context.SaveChangesAsync();

                resposta.Dados = new LocacaoResponseDto
                {
                    Id = locacao.Id,
                    NomeCliente = locacao.Cliente.Nome,
                    NomeVeiculo = locacao.Veiculo.NomeModelo,
                    Placa = locacao.Veiculo.Placa,
                    DataInicio = locacao.DataInicio,
                    DataFimPrevista = locacao.DataFimPrevista,
                    DataFimReal = locacao.DataFimReal,
                    ValorTotal = locacao.ValorTotal,
                    Status = locacao.Status
                };

                resposta.Mensagem = "Veículo devolvido e locação encerrada com sucesso!";
                resposta.Sucesso = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Erros = ["Erro interno do servidor - " + ex.Message];
                return resposta;
            }
        }


        public async Task<ResponseModel<LocacaoResponseDto>> ExcluirLocacao(int idLocacao)
        {
            ResponseModel<LocacaoResponseDto> resposta = new();
            try
            {
                var locacao = await _context.Locacoes
                    .Include(l => l.Cliente)
                    .Include(l => l.Veiculo)
                    .FirstOrDefaultAsync(l => l.Id == idLocacao);

                if (locacao == null)
                {
                    resposta.Mensagem = "Locação não encontrada.";
                    return resposta;
                }

                if (locacao.Status == StatusLocacao.Ativa)
                    locacao.Veiculo.Status = StatusVeiculo.Disponivel;

                var dtoRetorno = new LocacaoResponseDto
                {
                    Id = locacao.Id,
                    NomeCliente = locacao.Cliente.Nome,
                    NomeVeiculo = locacao.Veiculo.NomeModelo,
                    Placa = locacao.Veiculo.Placa,
                    DataInicio = locacao.DataInicio,
                    DataFimPrevista = locacao.DataFimPrevista,
                    DataFimReal = locacao.DataFimReal,
                    ValorTotal = locacao.ValorTotal,
                    Status = locacao.Status
                };

                _context.Locacoes.Remove(locacao);
                await _context.SaveChangesAsync();

                resposta.Dados = dtoRetorno;
                resposta.Mensagem = "Locação excluída com sucesso!";
                resposta.Sucesso = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Erros = [ex.Message];
                resposta.Mensagem = "Erro interno do servidor.";
                return resposta;
            }
        }

        public async Task<ResponseModel<PagamentoResponseDto>> ConfirmarPagamento(int idLocacao, ConfirmarPagamentoRequestDto request)
        {
            ResponseModel<PagamentoResponseDto> resposta = new();
            try
            {
                // Busca a locação com o pagamento relacionado
                var locacao = await _context.Locacoes
                    .Include(l => l.Cliente)
                    .Include(l => l.Veiculo)
                    .FirstOrDefaultAsync(l => l.Id == idLocacao);

                if (locacao == null)
                {
                    resposta.Sucesso = true;
                    resposta.Mensagem = "Locação não encontrada.";
                    return resposta;
                }

                // Busca o pagamento associado à locação
                var pagamento = await _context.Pagamentos
                    .FirstOrDefaultAsync(p => p.Locacao.Id == idLocacao);

                if (pagamento == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Pagamento não encontrado para esta locação.";
                    return resposta;
                }

                // Verifica se o pagamento já foi realizado
                if (pagamento.Status == StatusPagamento.Realizado)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Este pagamento já foi confirmado anteriormente.";
                    return resposta;
                }

                // Verifica se o pagamento foi cancelado
                if (pagamento.Status == StatusPagamento.Cancelado)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Este pagamento foi cancelado. Não é possível confirmá-lo.";
                    return resposta;
                }

                // Atualiza os dados do pagamento
                pagamento.MetodoPag = request.MetodoPagamento;
                pagamento.Status = StatusPagamento.Realizado;
                pagamento.Data_pagamento = DateTime.Now;
                pagamento.updated_at = DateTime.Now;

                await _context.SaveChangesAsync();

               
                resposta.Dados = new PagamentoResponseDto
                {
                    Id = pagamento.Id,
                    LocacaoId = locacao.Id,
                    Valor = pagamento.Valor,
                    MetodoPagamento = pagamento.MetodoPag.ToString(),
                    Status = pagamento.Status.ToString(),
                    DataPagamento = pagamento.Data_pagamento,
                    MensagemConfirmacao = $"Pagamento de R$ {pagamento.Valor:F2} confirmado com sucesso via {pagamento.MetodoPag}!"
                };

                resposta.Mensagem = "Pagamento confirmado com sucesso!";
                resposta.Sucesso = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Erros = ["Erro interno ao confirmar pagamento - " + ex.Message];
                resposta.Mensagem = "Erro interno do servidor.";
                return resposta;
            }
        }


    }
}