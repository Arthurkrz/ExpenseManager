using ExpenseManager.Tests.Mapping.TestModels;
using ExpenseManager.Web.Mapping;
using System;
using System.Collections.Generic;
using Xunit;

namespace ExpenseManager.Tests.Mapping
{
    public class ObjectMapperTests
    {
        private readonly ObjectMapper _sut = new();

        [Fact]
        public void Map_ShouldThrowException_WhenNullSource()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                _sut.Map<Source, Target>(null));

            Assert.Equal("Value cannot be null. " +
                "(Parameter 'source')", exception.Message);
        }

        [Fact]
        public void MapCollection_ShouldThrowException_WhenNullSource()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                _sut.MapCollection<Source, Target>(null));

            Assert.Equal("Value cannot be null. " +
                "(Parameter 'source')", exception.Message);
        }

        [Fact]
        public void Map_ShouldMapDirectlyAssignableProperties_IgnoringNameCase()
        {
            // Arrange
            var source = new CaseInsensitiveSource
            {
                NAME = "NAME",
                Quantity = 2
            };

            // Act
            var result = _sut.Map<CaseInsensitiveSource, 
                CaseInsensitiveTarget>(source);

            // Assert
            Assert.Equal(source.NAME, result.Name);
            Assert.Equal(source.Quantity, result.Quantity);
        }

        [Fact]
        public void Map_ShouldIgnoreTargetNonExistantProperties()
        {
            // Arrange
            var source = new MissingPropertySource
            {
                Name = "Name",
                SourceOnlyProperty = "SourceOnlyProperty"
            };

            // Act
            var result = _sut.Map<MissingPropertySource, 
                MissingPropertyTarget>(source);

            // Assert
            Assert.Equal(source.Name, result.Name);
        }

        [Fact]
        public void Map_ShouldIgnoreTargetPrivateSetterProperties()
        {
            // Arrange
            var source = new Source { Name = "Name" };

            // Act
            var result = _sut.Map<Source, PrivateSetterTarget>(source);

            // Assert
            Assert.Empty(result.Name);
        }

        [Fact]
        public void Map_ShouldIgnoreIncompatibleTypeProperties()
        {
            // Arrange
            var source = new IncompatibleSource { Value = 25 };

            // Act
            var result = _sut.Map<IncompatibleSource, 
                IncompatibleTarget>(source);

            // Assert
            Assert.Empty(result.Value);
        }

        [Fact]
        public void Map_ShouldIgnoreIncompatibleNestedProperty()
        {
            // Arrange
            var source = new NestedIncompatibleSource
            {
                Value = new NestedIncompatibleValueSource
                {
                    Amount = "100.00"
                }
            };

            // Act
            var result = _sut.Map<NestedIncompatibleSource,
                NestedIncompatibleTarget>(source);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(0m, result.Value.Amount);
        }

        [Fact]
        public void Map_ShouldIgnoreNestedTargetPropertyWithPrivateSetter()
        {
            // Arrange
            var source = new NestedPrivateSetterSource
            {
                Value = new NestedPrivateSetterValueSource
                {
                    Description = "Description"
                }
            };

            // Act
            var result = _sut.Map<NestedPrivateSetterSource, 
                NestedPrivateSetterTarget>(source);

            // Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public void Map_ShouldUseConfiguredConverter()
        {
            // Arrange
            var source = new Source { Name = "Name" };

            // Act
            var result = _sut.Map<Source, Target>(source, 
                options =>  options.MapProperty(s => s.Name, 
                t => t.Name, v => v.ToUpperInvariant()));

            // Assert
            Assert.Equal("NAME", result.Name);
        }

        [Fact]
        public void Map_ShouldNotOverwriteConfiguredPropertyDuringConventionMapping()
        {
            // Arrange
            var source = new Source { Name = "Name" };

            // Act
            var result = _sut.Map<Source, Target>(source, 
                options => options.MapProperty(i => i.Name, 
                i => i.Name, _ => "Configured"));

            // Assert
            Assert.Equal("Configured", result.Name);
        }

        [Fact]
        public void Map_ShouldMapEnumByName()
        {
            // Arrange
            var source = new EnumSource 
            { Status = SourceStatus.Approved };

            // Act
            var result = _sut.Map<EnumSource, EnumTarget>(source);

            // Assert
            Assert.Equal(TargetStatus.Approved, result.Status);
        }

        [Fact]
        public void Map_ShouldMapNullableEnumByName()
        {
            // Arrange
            var source = new NullableEnumSource 
            { Status = SourceStatus.Approved };

            // Act
            var result = _sut.Map<NullableEnumSource, 
                NullableEnumTarget>(source);

            // Assert
            Assert.Equal(TargetStatus.Approved, result.Status);
        }

        [Fact]
        public void Map_ShouldMapNullNullableEnumAsNull()
        {
            var source = new NullableEnumSource 
            { Status = null };

            var result = _sut.Map<NullableEnumSource, 
                NullableEnumTarget>(source);

            Assert.Null(result.Status);
        }

        [Fact]
        public void Map_ShouldThrow_WhenInvalidEnumValue()
        {
            // Arrange
            var source = new NullableEnumSource 
            { Status = (SourceStatus)999 };

            // Act & Assert
            var exception = Assert.Throws<
                InvalidOperationException>(() =>
                _sut.Map<NullableEnumSource,
                NullableEnumTarget>(source));

            Assert.Equal("The value '999' is not " +
                "defined in enum 'SourceStatus'.", exception.Message);
        }

        [Fact]
        public void Map_ShouldThrowException_WhenNonExistantTargetEnumMember()
        {
            // Arrange
            var source = new MissingEnumMemberSource
            { Status = ExtendedSourceStatus.Archived };

            // Act & Assert
            var exception = Assert.Throws<
                InvalidOperationException>(() =>
                _sut.Map<MissingEnumMemberSource,
                MissingEnumMemberTarget>(source));

            Assert.Contains("Cannot map enum value " +
                "'ExtendedSourceStatus.Archived' to enum " +
                "'ReducedTargetStatus' because the target " +
                "enum does not contain a member " +
                "named 'Archived'.", exception.Message);
        }

        [Fact]
        public void Map_ShouldMapNestedClassRecursively()
        {
            // Arrange
            var source = new NestedSource 
            { 
                Name = "Name",
                Address = new NestedAddressSource 
                { Street = "Street", City = "City" } 
            };

            // Act
            var result = _sut.Map<NestedSource, 
                NestedTarget>(source);

            // Assert
            Assert.NotNull(result.Address);
            Assert.NotNull(result.Address.Street);
            Assert.NotNull(result.Address.City);
            Assert.Equal("Street", result.Address.Street);
            Assert.Equal("City", result.Address.City);
        }

        [Fact]
        public void Map_ShouldMapNullNestedClassAsNull()
        {
            // Arrange
            var source = new NestedSource 
            { Address = null };

            // Act
            var result = _sut.Map<NestedSource, 
                NestedTarget>(source);

            // Assert
            Assert.Null(result.Address);
        }

        [Fact]
        public void Map_ShouldThrowException_WhenObjectGraphContainsCycle()
        {
            // Arrange
            var source = new CyclicSource();
            source.Parent = source;

            // Act & Assert
            var exception = Assert.Throws<
                InvalidOperationException>(() =>
                _sut.Map<CyclicSource, CyclicTarget>(source));

            Assert.Equal("A circular reference was detected while " +
                "mapping 'CyclicSource' to 'CyclicTarget'.", exception.Message);
        }

        [Fact]
        public void Map_ShouldThrowException_WhenNestedTargetHasNoParameterlessConstructor()
        {
            // Arrange
            var source = new ConstructibleSource
            {
                Details = new ConstructibleDetailsSource
                {
                    Name = "Name"
                }
            };

            // Act & Assert
            var exception = Assert.Throws<
                InvalidOperationException>(() =>
                _sut.Map<ConstructibleSource,
                TargetWithoutDefaultConstructor>(source));

            Assert.Equal("Cannot create nested target type " +
                "'ExpenseManager.Tests.Mapping.TestModels." +
                "TargetDetailsWithoutDefaultConstructor' " +
                "because it does not have a public " +
                "parameterless constructor.", 
                exception.Message);
        }

        [Fact]
        public void Map_ShouldIgnoreNestedProperty_WhenTargetPropertyNotExist()
        {
            // Arrange
           var source = new NestedMissingPropertySource
           {
               Details = new NestedMissingPropertyDetailsSource
               {
                   Name = "Mapped",
                   SourceOnly = "Ignored",
               }
           };

            // Act
            var result = _sut.Map<
                NestedMissingPropertySource, 
                NestedMissingPropertyTarget>(source);

            // Assert
            Assert.NotNull(result.Details);
            Assert.Equal("Mapped", result.Details.Name);
        }

        [Fact]
        public void MapCollection_ShouldMapAllItems_UsingTheSameConfiguration()
        {
            // Arrange
            var source = new[]
            {
                new Source { Name = "Name1" },
                new Source { Name = "Name2" }
            };

            // Act
            var result = _sut.MapCollection<Source, Target>(source, 
                options => options.MapProperty(i => i.Name, 
                i => i.Name, v => $"Mapped: {v}"));

            // Assert
            Assert.Collection(result,
                first => Assert.Equal("Mapped: Name1", first.Name),
                second => Assert.Equal("Mapped: Name2", second.Name));
        }

        [Fact]
        public void MapCollection_ShouldThrowException_WhenNullItem()
        {
            // Arrange
            IEnumerable<Source> source = [new Source { Name = "Valid" }, null];

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                _sut.MapCollection<Source, Target>(source));

            Assert.Equal("Value cannot be null. " +
                "(Parameter 'item')", exception.Message);
        }

        [Fact]
        public void MapCollection_ShouldReturnEmptyList_WhenSourceIsEmpty()
        {
            // Act & Assert
            Assert.Empty(_sut.MapCollection<Source, Target>([]));
        }

        [Fact]
        public void Map_ShouldThrowInvalidOperationException_WhenNonNullableTargetEnum()
        {
            // Arrange
            var source = new NullableEnumSource { Status = null };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _sut.Map<NullableEnumSource, EnumTarget>(source));

            Assert.Equal("Cannot map a null value from " +
                "enum 'SourceStatus' to non-nullable " +
                "enum 'TargetStatus'.", exception.Message);
        }
    }
}
