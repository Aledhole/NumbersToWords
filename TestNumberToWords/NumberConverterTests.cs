using Microsoft.VisualStudio.TestPlatform.Utilities;
using NumbersToWords.Models;
using Xunit;
using Xunit.Abstractions;
namespace NumberToWordsTests
{

    public class NumberConverterTests
    {
        private readonly ITestOutputHelper _output;

        public NumberConverterTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("123.45", "ONE HUNDRED AND TWENTY-THREE AND FORTY-FIVE CENTS")]
        [InlineData("0.00", "ZERO")]
        [InlineData("-123", "NEGATIVE ONE HUNDRED AND TWENTY-THREE")]
        [InlineData("-0.99", "NEGATIVE NINETY-NINE CENTS")]
        public void InputCorrectNumber_ReturnCorrectWords(string input, string expected)
        {
            
            // Act
            string result = NumberConverter.ParseNumberInput(input);

            // Assert
            Assert.Equal(expected, result);
            _output.WriteLine(expected,result);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("/,>?`~@#$%^&*()_+=-.")]
        [InlineData("9.999")]
        public void InputNotCorrect_ThrowFormatException(string input)
        {

            _output.WriteLine($"Testing input: {input}");            
            Assert.Throws<FormatException>(() => NumberConverter.ParseNumberInput(input));
        }

        [Fact]
        public void InputLargeNumber_ReturnCorrectWords()
        {
            // Arrange
            string input = "123456789123456789";
            string expected = "ONE HUNDRED AND TWENTY-THREE QUADRILLION FOUR HUNDRED AND FIFTY-SIX TRILLION SEVEN HUNDRED AND EIGHTY-NINE BILLION ONE HUNDRED AND TWENTY-THREE MILLION FOUR HUNDRED AND FIFTY-SIX THOUSAND SEVEN HUNDRED AND EIGHTY-NINE";

           
            string result = NumberConverter.ParseNumberInput(input);
            _output.WriteLine(expected,result);
            Assert.Equal(expected, result);
        }
    }
}