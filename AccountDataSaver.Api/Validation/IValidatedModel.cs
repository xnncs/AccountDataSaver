using AccountDataSaver.Api.Models;

namespace AccountDataSaver.Api.Validation;

public interface IValidatedModel
{
    public ValidationResultsValues IsValid();
}