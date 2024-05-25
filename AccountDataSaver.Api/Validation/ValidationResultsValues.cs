using System.ComponentModel.DataAnnotations;

namespace AccountDataSaver.Api.Validation;

public record ValidationResultsValues(bool IsValid, List<ValidationResult>? Results)
{
    public static ValidationResultsValues Create(bool isValid, string errorMessage)
    {
        List<ValidationResult>? results = new System.Collections.Generic.List<ValidationResult>()
        {
            new ValidationResult(errorMessage)
        };

        return new ValidationResultsValues(false, results);
    }
    public List<string> GetModelValidationMistakes()
    {
        if (Results == null)
        {
            return Enumerable.Empty<string>().ToList();
        }
        
        return Results.Select(x => x.ErrorMessage).ToList()!;
    }
}