using System.ComponentModel.DataAnnotations;
using AccountDataSaver.Api.Validation;

namespace AccountDataSaver.Api.Models;

public record AccountContractRequest : IValidatedModel
{
    [Required]
    [Length(4, 20)]
    public string ServiceUrl { get; set; }
    
    [Required(AllowEmptyStrings = true)]
    [MaxLength(50)]
    public string Description { get; set; }
    
    [Required]
    [Length(6, 15)]
    public string Login { get; set; }
    
    [Required]
    [Length(6, 30)]
    public string Password { get; set; }

    public ValidationResultsValues IsValid()
    {
        ValidationContext context = new ValidationContext(this);
        List<ValidationResult>? results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(this, context, results, true);
        
        return new ValidationResultsValues(isValid, results);
    }
}