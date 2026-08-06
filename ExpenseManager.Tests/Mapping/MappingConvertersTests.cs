using ExpenseManager.Web.Mapping.CustomMappings;
using System;
using Xunit;

namespace ExpenseManager.Tests.Mapping
{
    public class MappingConvertersTests
    {
        [Theory]
        [InlineData(0, "0.00")]
        [InlineData(10, "10.00")]
        [InlineData(1234.5, "1234.50")]
        [InlineData(-9.99, "-9.99")]
        public void MoneyToString_ShouldUseTwoDecimalPlacesAndInvariantFormat(decimal value, string expected)
        {
            // Act & Assert
            Assert.Equal(expected, MappingConverters.MoneyToString(value));
        }

        [Theory]
        [InlineData("1234.56", 1234.56)]
        [InlineData("1234,56", 1234.56)]
        [InlineData("-1234,56", -1234.56)]
        public void StringToMoney_ShouldParseDotAndCommaDecimalSeparators(string value, decimal expected)
        {
            // Act & Assert
            Assert.Equal(expected, MappingConverters.StringToMoney(value));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void StringToMoney_ShouldThrowException_WhenWhiteSpaceValue(string value)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                MappingConverters.StringToMoney(value));

            Assert.Equal("The value cannot be an empty string or " +
                "composed entirely of whitespace. (Parameter " +
                "'value')", exception.Message);
        }

        [Fact]
        public void StringToMoney_ShouldThrowException_WhenNullValue()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                MappingConverters.StringToMoney(null));

            Assert.Equal("Value cannot be null. (Parameter 'value')", exception.Message);
        }

        [Fact]
        public void StringToMoney_ShouldThrowFormatException_WhenInvalidTypeValue()
        {
            // Act & Assert
            var exception = Assert.Throws<FormatException>(() =>
                MappingConverters.StringToMoney("value"));

            Assert.Equal("The value 'value' is not a valid " +
                "monetary value.", exception.Message);
        }

        [Theory]
        [InlineData("2500.75", 2500.75)]
        [InlineData("2500,75", 2500.75)]
        public void StringToNullableMoney_ShouldParseDotAndCommaDecimalSeparators(string value, decimal expected)
        {
            // Act & Assert
            Assert.Equal(expected, MappingConverters.StringToNullableMoney(value));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void StringToNullableMoney_ShouldReturnNull_WhenValueIsNullOrWhiteSpace(string value)
        {
            // Act & Assert
            Assert.Null(MappingConverters.StringToNullableMoney(value));
        }

        [Fact]
        public void StringToNullableMoney_ShouldThrowException_WhenInvalidValue()
        {
            // Act & Assert
            var exception = Assert.Throws<FormatException>(() =>
                MappingConverters.StringToNullableMoney("value"));

            Assert.Equal("The value 'value' is not a valid " +
                "monetary value.", exception.Message);
        }

        [Theory]
        [InlineData("10.00.00")]
        [InlineData("10,00,00")]
        public void TryParseMoney_ShouldReturnFalse_WhenMultipleDecimalSeparators(string value)
        {
            // Act & Assert
            var exception = Assert.Throws<FormatException>(() =>
                MappingConverters.StringToMoney(value));

            Assert.Equal($"The value '{value}' is not a valid " +
                "monetary value.", exception.Message);
        }
    }
}
