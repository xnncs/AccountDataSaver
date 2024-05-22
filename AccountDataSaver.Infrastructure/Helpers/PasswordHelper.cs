using System.Text;
using AccountDataSaver.Infrastructure.Abstract;
using AccountDataSaver.Infrastructure.Models;

namespace AccountDataSaver.Infrastructure.Helpers;

public class PasswordHelper : IPasswordHelper
{
    private readonly char[] englishAlphabet = [
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 
        's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
    ];

    private readonly int[] numbers = [
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9
    ];

    private readonly char[] specificSymbols = [
        '/', ':', ';', '_'
    ];
    

    private const int minimumPasswordLength = 6;
    
    private readonly Random random = new Random();
    
    public string GenerateRandomPassword(GenerateRandomPasswordOptions options)
    {
        if (options.PasswordLength < minimumPasswordLength)
        {
            throw new AggregateException($"Password length should be more then {minimumPasswordLength}");
        }
        
        // calculating counts of all symbol types based on its rate
        int numbersCount = (int)(options.PasswordLength * options.NumbersRate);
        int specificSymbolsCount = (int)(options.PasswordLength * options.SpecificSymbolsRate);
        int lettersCount = options.PasswordLength - numbersCount - specificSymbolsCount;

        // converts int array to char array
        char[] charNumbers = Array.ConvertAll(numbers, x => x.ToString()[0]);

        StringBuilder builder = new StringBuilder();

        builder.Append(GetRandomSymbols(lettersCount, englishAlphabet));
        builder.Append(GetRandomSymbols(numbersCount, charNumbers));
        builder.Append(GetRandomSymbols(specificSymbolsCount, specificSymbols));

        char[] passwordChars = builder.ToString().ToCharArray();
        random.Shuffle<char>(passwordChars);

        return new string(passwordChars);
    }
    private string GetRandomSymbols(int count, char[] alphabet)
    {
        StringBuilder builder = new StringBuilder();
        
        for (int i = 0; i < count; i++)
        {
            char element = alphabet[random.Next(0, alphabet.Length)];
            builder.Append(element.ToString());
        }

        return builder.ToString();
    }
}