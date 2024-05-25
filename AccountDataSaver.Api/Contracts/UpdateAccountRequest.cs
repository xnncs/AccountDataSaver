using System.ComponentModel.DataAnnotations;
using AccountDataSaver.Api.Models;
using AccountDataSaver.Api.Validation;

namespace AccountDataSaver.Api.Contracts;

public record UpdateAccountRequest : IValidatedModel
{
    [Required]
    public int AccountId { get; set; }
    
    [Required]
    public AccountContractRequest Data { get; set; }
    
    public ValidationResultsValues IsValid()
    {
        ValidationContext context = new ValidationContext(this);
        List<ValidationResult?> results = new List<ValidationResult?>();

        bool isValid = Validator.TryValidateObject(this, context, results!, true);
        
        return new ValidationResultsValues(isValid, results);
    }
}