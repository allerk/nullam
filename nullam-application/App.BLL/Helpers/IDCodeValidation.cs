using System.Text.RegularExpressions;

namespace App.BLL.Helpers;

public class IDCodeValidation
{
    private static readonly Regex IDCodeRegex = 
        new Regex("^[1-6][0-9]{2}(0[1-9]|1[0-2])(0[1-9]|[1-2][0-9]|3[0-1])[0-9]{4}$");
    private static readonly int[] Weights = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 };
    
    public bool IsValid(string idCode)
    {
        if (string.IsNullOrEmpty(idCode))
        {
            return false;
        }

        idCode = idCode.Trim();

        if (!IDCodeRegex.IsMatch(idCode))
        {
            return false;
        }

        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            int digit = idCode[i] - '0';
            sum += Weights[i] * digit;
        }

        int control = (sum % 11) % 10;
        int lastDigit = idCode[10] - '0';

        return control == lastDigit;
    }
}