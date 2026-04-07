using API_LocadoraVeiculos.Data;
using API_LocadoraVeiculos.DTOs.Clientes;
using API_LocadoraVeiculos.DTOs.ClientesDtos;
using API_LocadoraVeiculos.Models.Clientes;
using API_LocadoraVeiculos.Models.Respostas;
using API_LocadoraVeiculos.Services.Interfaces;
using Azure;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_LocadoraVeiculos.Services
{
    public class ClienteService : IClienteInterface
    {
        private readonly AppDbContext _context;

        public ClienteService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<ClienteResponseDto>> BuscaClientePorId(int IdCLiente)
        {
            ResponseModel<ClienteResponseDto> resposta = new ResponseModel<ClienteResponseDto>();
            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(cliente => cliente.Id == IdCLiente);
                if (cliente == null)
                {
                    resposta.Mensagem = "Cliente não encontrado";
                    return resposta;
                }
                var clienteDto = new ClienteResponseDto
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Email = cliente.Email,
                    Status = cliente.Status,

                };
                resposta.Dados = clienteDto;
                resposta.Mensagem = "Cliente localizado!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Erros = new List<string>();
                resposta.Erros.Add("Erro interno ao buscar cliente - "+ex.Message);
          
                resposta.Sucesso = false;
                return resposta;
            }
        }
        public async Task<ResponseModel<ClienteResponseDto>> BuscarClientePorIdLocacao(int IdLocacao)
        {
            ResponseModel<ClienteResponseDto> resposta = new ResponseModel<ClienteResponseDto>();
            try
            {
                var loc = await _context.Locacoes.FirstOrDefaultAsync(loc => loc.Id == IdLocacao);
       
                if (loc == null) { 
                    resposta.Dados = null;
                    resposta.Mensagem = "Nenhum registro encontrado";
                    return resposta;
                }

                var clienteDto = new ClienteResponseDto
                {
                    Id = loc.Cliente.Id,
                    Nome = loc.Cliente.Nome,
                    Email = loc.Cliente.Email,
                    Status = loc.Cliente.Status,
                }; 

                resposta.Dados = clienteDto;
      
                resposta.Mensagem = "Cliente localizado!";
                return resposta;

            }catch(Exception ex)
            {
                resposta.Erros = new List<string>();
                resposta.Erros.Add("Erro interno ao buscar cliente - " + ex.Message);

                resposta.Sucesso = false;
                return resposta;
            }
        }
        public async Task<ResponseModel<List<ClienteResponseDto>>> ListarClientes()
        {
            ResponseModel<List<ClienteResponseDto>> resposta = new ResponseModel<List<ClienteResponseDto>>() ;
            try
            {
                var clientes = await _context.Clientes.ToListAsync();

                var clientesDto = clientes.Select(c => new ClienteResponseDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Email = c.Email,
                    Status = c.Status,

                }).ToList();

                resposta.Dados = clientesDto;
                resposta.Mensagem = "Todos os clientes foram coletados.";
                return resposta;
            }catch (Exception)
            {
                resposta.Erros = new List<string>(); 
                resposta.Erros.Add("Erro interno ao listar clientes");
                resposta.Sucesso = false;
                return resposta;

            }
        }

        public async Task<ResponseModel<ClienteResponseDto>> CriarCliente(ClienteRequestDto dto)
        {
            ResponseModel<ClienteResponseDto> resposta = new();
            try
            {
                //verifica cpf duplicado
                var cpfExiste = await _context.Clientes.AnyAsync(c => c.Cpf == dto.Cpf);
                if (cpfExiste)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Já existe um cliente cadastrado com este CPF.";
                    return resposta;
                }

                // Verifica Email duplicado
                var emailExiste = await _context.Clientes.AnyAsync(c => c.Email == dto.Email);
                if (emailExiste)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Já existe um cliente cadastrado com este e-mail.";
                    return resposta;
                }

                // Verifica Telefone duplicado
                var telefoneExiste = await _context.Clientes.AnyAsync(c => c.Telefone == dto.Telefone);
                if (telefoneExiste)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Já existe um cliente cadastrado com este telefone.";
                    return resposta;
                }

                var nCliente = new ClienteModel 
                {
                    Nome = dto.Nome,
                    Email = dto.Email,
                    Cpf = dto.Cpf,
                    DataNasc = dto.DataNasc,
                    Telefone = dto.Telefone,
                    NumCnh = dto.NumCnh,
                    ValidadeCnh = dto.ValidadeCnh,
                    Status = true,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };

                _context.Add(nCliente);
                await _context.SaveChangesAsync();

                var resDto = new ClienteResponseDto
                {
                    Id = nCliente.Id,
                    Nome = nCliente.Nome,
                    Email = nCliente.Email,
                    Status = nCliente.Status
                };

                resposta.Dados = resDto;
                resposta.Mensagem = "Cliente criado com sucesso";
                resposta.Sucesso = true;
                return resposta;

            }catch(Exception ex) { 
                resposta.Sucesso=false;
                resposta.Erros = [ex.Message];
                resposta.Mensagem = "Erro interno do servidor";
                return resposta;
            }
        }

        public async Task<ResponseModel<ClienteModel>> EditarCliente(int idCliente, ClienteUpdateDto dto)
        {
            ResponseModel<ClienteModel> resposta = new();
            try
            {
               

                var cliente = await _context.Clientes.FindAsync(idCliente);

                if (cliente == null)
                {
                    resposta.Mensagem = "Cliente não encontrado!";
                    return resposta;
                }

                // Verifica CPF duplicado
                var cpfExiste = await _context.Clientes.AnyAsync(c => c.Cpf == dto.Cpf && c.Id != idCliente);
                if (cpfExiste)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Já existe outro cliente cadastrado com este CPF.";
                    return resposta;
                }

                // Verifica Email duplicado 
                var emailExiste = await _context.Clientes.AnyAsync(c => c.Email == dto.Email && c.Id != idCliente);
                if (emailExiste)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Já existe outro cliente cadastrado com este e-mail.";
                    return resposta;
                }

                // Verifica Telefone duplicado 
                var telefoneExiste = await _context.Clientes.AnyAsync(c => c.Telefone == dto.Telefone && c.Id != idCliente);
                if (telefoneExiste)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Já existe outro cliente cadastrado com este telefone.";
                    return resposta;
                }


                cliente.Nome = dto.Nome;
                cliente.Email = dto.Email;
                cliente.Telefone = dto.Telefone;
                cliente.Cpf = dto.Cpf;
                cliente.NumCnh = dto.NumCnh;
                cliente.DataNasc = dto.DataNasc;
                cliente.ValidadeCnh = dto.ValidadeCnh;

                await _context.SaveChangesAsync();

                resposta.Mensagem = "Cliente atualizado com sucesso!";
                resposta.Sucesso = true;
                resposta.Dados = cliente;

                return resposta;

            }
            catch (Exception ex) {
                resposta.Sucesso = false;
                resposta.Erros = ["Erro interno do servidor - "+ex.Message];
                return resposta;
            }
        }

        public async Task<ResponseModel<ClienteResponseDto>> ExcluirCliente(int idCliente)
        {
            ResponseModel<ClienteResponseDto> resposta = new();
            try
            {
               
                var cliente = await _context.Clientes.FirstOrDefaultAsync(cliente => cliente.Id == idCliente);
                
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();

                resposta.Dados = new ClienteResponseDto { Id = cliente.Id, Nome = cliente.Nome, Email = cliente.Email, Status = false };
                resposta.Mensagem = "Cliente excluido com sucesso";
                resposta.Sucesso = true;
                return resposta;

            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Erros = [ex.Message];
                resposta.Mensagem = "Erro interno do servidor";
                return resposta;
            }
        }
    }
}
