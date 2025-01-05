using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Dima.Core.Enums;

namespace Dima.Core.Requests.Transactions;

public class CreateTransactionRequest : Request
{
    [Required(ErrorMessage = "Título inválido")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Tipo inválido")]
    public ETransactionType Type { get; set; }
    
    [Required(ErrorMessage = "Amount inválido")]
    public decimal Amount { get; set; }
    
    [Required(ErrorMessage = "Categoria inválida")]
    public long CategoryId { get; set; }
    
    [Required(ErrorMessage = "Data inválido")]
    public DateTimeOffset? PaidOrReceivedAt { get; set; }

    [JsonIgnore]
    public DateTime? PaidOrReceivedDate
    {
        get => PaidOrReceivedAt?.DateTime;
        set
        {
            if (value == null)
            {
                PaidOrReceivedAt = null;
                return;
            }
            PaidOrReceivedAt = new DateTimeOffset(value.Value);
        }
    }
}