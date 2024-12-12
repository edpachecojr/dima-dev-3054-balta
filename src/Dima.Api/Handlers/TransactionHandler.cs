using Dima.Api.Data;
using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        try
        {
            var transaction = new Transaction
            {
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                CreatedAt = DateTimeOffset.Now,
                Amount = request.Amount,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Title = request.Title,
                Type = request.Type,
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, 201, "Transação criada com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new Response<Transaction?>(null, 500, "Não foi possível criar a sua transação");
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            
            if (transaction == null)
                return new Response<Transaction?>(null, 404, "Transação não encontrada");
            
            transaction.Amount = request.Amount;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;
            transaction.Title = request.Title;
            transaction.Type = request.Type;
            transaction.CategoryId = request.CategoryId;

            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, 200, "Transação alterada com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new Response<Transaction?>(null, 500, "Não foi possível atualizar a sua transação");
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            
            if (transaction == null)
                return new Response<Transaction?>(null, 404, "Transação não encontrada");

            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, 204, "Transação removida com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new Response<Transaction?>(null, 500, "Não foi possível atualizar a sua transação");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await context.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            
            return transaction is null
                ? new Response<Transaction?>(null, 404, "Transação não encontrada")
                : new Response<Transaction?>(transaction);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new Response<Transaction?>(null, 500, "Não foi possível recuperar transação.");
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriod(GetTransactionsByPeriodRequest request)
    {
        try
        {
            request.StartDate ??= DateTime.Now.GetStartOfMonth();
            request.EndDate ??= DateTime.Now.GetEndOfMonth();
            var query = context
                .Transactions
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId && x.CreatedAt >= request.StartDate && x.CreatedAt <= request.EndDate)
                .OrderBy(x => x.CreatedAt);

            var transactions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Transaction>?>(transactions, count, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new PagedResponse<List<Transaction>?>(null, 500, "Falha ao recuperar transacoes.");
        }
    }
}