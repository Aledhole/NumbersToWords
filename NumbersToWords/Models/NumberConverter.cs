using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Numerics;

namespace NumbersToWords.Models

{
    public class NumberConverter
    {
        private static string[] unitsMap = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

        private static string[] tensMap = { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        private static string[] thousandsMap = {"", "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion", "Sextillion", "Septillion", "Octillion", "Nonillion", "Decillion",
                "Undecillion", "Duodecillion", "Tredecillion", "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septendecillion", "Octodecillion", "Novemdecillion", "Vigintillion" };
        public NumberConverter() { }

        public static string UnitsMapToWord(int digit)
        {
            return (unitsMap[digit]).ToUpper();
        }
        public static string TensMapToWord(int digit)
        {
            return (tensMap[digit]).ToUpper();
        }
        public static string ThousandsMapToWord(int digit)
        {
            return (thousandsMap[digit]).ToUpper();
        }

        public static string ParseNumberInput(string input)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(input))
                throw new FormatException("Input cannot be null or empty.");

            // Check if negative
            bool isNegative = input.StartsWith("-");
            if (isNegative)
                input = input.Substring(1);

            string[] parts = input.Split('.');

            // Parse whole part
            if (!BigInteger.TryParse(parts[0], out BigInteger wholePart))
                throw new FormatException("Invalid whole number format.");
           
            if (wholePart == 0 && (parts.Length == 1 && decimal.Parse("0." + parts[1]) == 0))
                return "ZERO";

            // Validate fractional part
            decimal fractionalPart = 0;
            if (parts.Length > 1)
            {
                string fractionalString = parts[1];
                if (fractionalString.Length > 2)
                    throw new FormatException("Too many decimal places."); // Explicit validation

                fractionalPart = decimal.Parse("0." + fractionalString);
            }
            if (wholePart == 0 && fractionalPart > 0)
            {
                string decimals = ConvertDecimalsToWords(fractionalPart);
                return isNegative ? $"NEGATIVE {decimals} CENTS" : $"{decimals} CENTS";
            }
            // Convert whole part to words
            string result = ConvertNumberToWords(wholePart);

            // Convert fractional part to words
            if (fractionalPart > 0)
            {
                string decimals = ConvertDecimalsToWords(fractionalPart);
                result += " AND " + decimals + " CENTS";
            }

            if (isNegative)
                result = "NEGATIVE " + result;

            return result.Trim();
        }

        private static string ConvertDecimalsToWords(decimal number)
        {
            int fractionalAsWholeNumber = (int)(number * 100);
            string decimals = ConvertNumberToWords(fractionalAsWholeNumber);
            return decimals;
        }

        private static string ConvertNumberToWords(BigInteger number)
        {
            

            if (number < 20)
                return UnitsMapToWord((int)number);

            if (number < 100)
                return TensMapToWord((int)number / 10) + (number % 10 > 0 ? "-" + UnitsMapToWord((int)number % 10) : "");

            if (number < 1000)
                return UnitsMapToWord((int)number / 100) + " HUNDRED AND" + (number % 100 > 0 ? " " + ConvertNumberToWords(number % 100) : "");

            for (int i = thousandsMap.Length - 1; i > 0; i--)
            {
                BigInteger divisor = BigInteger.Pow(1000, i);
                if (number >= divisor)
                {
                    BigInteger quotient = number / divisor;
                    BigInteger remainder = number % divisor;

                    string result = ConvertNumberToWords(quotient) + " " + thousandsMap[i].ToUpper();
                    if (remainder > 0)
                    {
                        result += " " + ConvertNumberToWords(remainder);
                    }
                    return result;
                }
            }
            return number.ToString();
        }
              
    }

}
