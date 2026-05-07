using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gado.Dominio.Entidades;
using Gado.Aplicacao.DTOs;
using DataAccess.Repositorios; 

namespace Gado.Aplicacao;

public class BaixaAnimalAplicacao : IBaixaAnimalAplicacao
{
    private readonly IBaixaAnimalRepositorio _baixaRepositorio;
    private readonly IAnimalRepositorio _animalRepositorio; 

    public BaixaAnimalAplicacao(
        IBaixaAnimalRepositorio baixaRepositorio,
        IAnimalRepositorio animalRepositorio)
    {
        _baixaRepositorio = baixaRepositorio;
        _animalRepositorio = animalRepositorio;
    }

    public async Task<BaixaAnimal> CriarAsync(RegistrarBaixaDTO dto) // <--- Padrão Criar
    {
        var animal = await _animalRepositorio.ObterPorIdAsync(dto.AnimalID);
        if (animal == null) throw new Exception("Animal não encontrado.");
        if (!animal.Ativo) throw new Exception("Este animal já está inativo.");

        var novaBaixa = new BaixaAnimal
        {
            AnimalID = dto.AnimalID,
            DataBaixa = dto.DataBaixa,
            Motivo = dto.Motivo,
            Observacao = dto.Observacao,
            Ativo = true
        };

        animal.Ativo = false; // Tira o boi do pasto
        await _animalRepositorio.AtualizarAsync(animal);

        return await _baixaRepositorio.SalvarAsync(novaBaixa);
    }

    public async Task AtualizarAsync(AtualizarBaixaDTO dto)
    {
        var baixa = await _baixaRepositorio.ObterPorIdAsync(dto.BaixaID);
        if (baixa == null) throw new Exception("Registro de baixa não encontrado.");

        baixa.DataBaixa = dto.DataBaixa;
        baixa.Motivo = dto.Motivo;
        baixa.Observacao = dto.Observacao;

        await _baixaRepositorio.AtualizarAsync(baixa);
    }

    public async Task ExcluirAsync(int id) // <--- Padrão Excluir
    {
        var baixa = await _baixaRepositorio.ObterPorIdAsync(id);
        if (baixa == null) throw new Exception("Registro de baixa não encontrado.");
        if (!baixa.Ativo) throw new Exception("Esta baixa já foi cancelada.");

        await _baixaRepositorio.DeletarAsync(baixa); 

        var animal = await _animalRepositorio.ObterPorIdAsync(baixa.AnimalID);
        if (animal != null)
        {
            animal.Ativo = true; // Devolve o boi pro pasto
            await _animalRepositorio.AtualizarAsync(animal);
        }
    }

    public async Task<BaixaAnimal> ObterAsync(int id) // <--- Padrão Obter
    {
        return await _baixaRepositorio.ObterPorIdAsync(id);
    }

    public async Task<IEnumerable<BaixaAnimal>> ListarAsync(bool ativo = true)
    {
        return await _baixaRepositorio.ListarAsync(ativo);
    }
}