using System.ComponentModel.DataAnnotations;
using AccountDataSaver.Api.Models;
using AccountDataSaver.Api.Validation;

namespace AccountDataSaver.Api.Contracts;

public record RegisterUserRequest : IValidatedModel
{
    [Required]
    [Length(3, 15)]
    public string Name { get; set; }
    
    [Required]
    [Length(6, 30)]
    public string Email { get; set; }
    
    [Required]
    [Length(6, 30)]
    public string Password { get; set; }
    
    
    public ValidationResultsValues IsValid()
    {
        ValidationContext context = new ValidationContext(this);
        List<ValidationResult> results = new List<ValidationResult>();
        
        bool isEmailValid = IsEmailValid(Email);
        if (!isEmailValid)
        {
            results.Add(new ValidationResult("Email is not valid"));
        }

        bool isValid = Validator.TryValidateObject(this, context, results, true) && isEmailValid;
        
        return new ValidationResultsValues(isValid, results);
    }

    private bool IsEmailValid(string email)
    {
        EmailAddressAttribute emailAttribute = new EmailAddressAttribute();
        return emailAttribute.IsValid(email);
    }
}