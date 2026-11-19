using ExpenseManager.Tests.Mapping.TestModels;
using ExpenseManager.Web.Mapping.Configurations;
using System;
using Xunit;

namespace ExpenseManager.Tests.Mapping
{
    public class MappingOptionsTests
    {
        [Fact]
        public void MapProperty_ShouldRegisterSourceTargetAndConverter()
        {
            // Arrange
            var options = new MappingOptions<Source, Target>();

            // Act
            var returnedOptions = options.MapProperty(
                s => s.Amount, t => t.ConvertedAmount, v => v * 2);

            // Assert
            var mapping = Assert.Single(options.PropertyMappings);
            Assert.Same(options, returnedOptions);
            Assert.Equal(nameof(Source.Amount), mapping.SourceProperty.Name);
            Assert.Equal(nameof(Target.ConvertedAmount), mapping.TargetProperty.Name);
            Assert.Equal(25m, mapping.Converter(12.5m));
        }

        [Fact]
        public void MapProperty_ShouldAllowMultipleMappings_InRegistrationOrder()
        {
            // Act
            var options = new MappingOptions<Source, Target>()
                .MapProperty(s => s.Amount, t => t.FormattedAmount, v => v.ToString())
                .MapProperty(s => s.Name, t => t.Name, v => v);

            // Assert
            Assert.Equal(2, options.PropertyMappings.Count);
            Assert.Equal(nameof(Source.Amount), options.PropertyMappings[0].SourceProperty.Name);
            Assert.Equal(nameof(Source.Name), options.PropertyMappings[1].SourceProperty.Name);
        }

        [Fact]
        public void MapProperty_ShouldThrowArgumentException_WhenSourceExpressionNotProperty()
        {
            // Arrange
            var options = new MappingOptions<Source, Target>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                options.MapProperty(s => s.GetAmount(), 
                t => t.FormattedAmount, v => v.ToString()));

            Assert.Equal("Expression must point to a property.", exception.Message);
        }

        [Fact]
        public void MapProperty_ShouldThrowArgumentException_WhenTargetExpressionNotProperty()
        {
            // Arrange
            var options = new MappingOptions<Source, Target>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            options.MapProperty(s => s.Amount, t => t.GetName(),
                v => v.ToString()));

            Assert.Equal("Expression must point to a property.", exception.Message);
        }

        [Fact]
        public void MapProperty_ShouldThrowInvalidOperationException_WhenTargetSetterPrivate()
        {
            // Arrange
            var options = new MappingOptions<PrivateSetterSource, PrivateSetterTarget>();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                options.MapProperty(options => options.Name, 
                t => t.Name, v => v));

            Assert.Equal("Target property 'Name' " +
                    $"must have a public setter.", exception.Message);
        }

        [Fact]
        public void MapProperty_ShouldThrowInvalidOperationException_WhenNoSetter()
        {
            // Arrange
            var options = new MappingOptions<NoSetterSource, NoSetterTarget>();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                options.MapProperty(options => options.Name,
                t => t.Name, v => v));

            Assert.Equal("Target property 'Name' " +
                    $"must have a public setter.", exception.Message);
        }

        [Fact]
        public void MapProperty_ShouldAcceptConvertedPropertyExpressions()
        {
            // Arrange
            var options = new MappingOptions<Source, Target>();

            // Act
            options.MapProperty(
                source => (object)source.Amount,
                target => (object)target.ConvertedAmount,
                value => value);

            // Assert
            var mapping = Assert.Single(options.PropertyMappings);

            Assert.Equal(nameof(Source.Amount), mapping.SourceProperty.Name);
            Assert.Equal(nameof(Target.ConvertedAmount), mapping.TargetProperty.Name);
        }
    }
}
