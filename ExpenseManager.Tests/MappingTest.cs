using ExpenseManager.Core.Entities;
using ExpenseManager.Tests.ObjectGenerators;
using ExpenseManager.Web.Mapping;
using ExpenseManager.Web.Mapping.Contracts;
using ExpenseManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExpenseManager.Tests
{
    public class MappingTest
    {
        private readonly IObjectMapper _sut;

        public MappingTest()
        {
            _sut = new ObjectMapper();
        }

        [Theory]
        [MemberData(nameof(MappingData.GetValidObjects), MemberType = typeof(MappingData))]
        public void Map_MustSuccesfullyConvert<TSource, TTarget>(TSource source, TTarget expectedTarget)
        {
            // Arrange
            var mappingProps = new Dictionary<string, string>
            {
                { "ValueString", "Value" },
                { "Value", "ValueString" },
                { "ValueRangeStart", "ValueStringRangeStart"},
                { "ValueRangeEnd", "ValueStringRangeEnd"},
                { "ValueStringRangeStart", "ValueRangeStart"},
                { "ValueStringRangeEnd", "ValueRangeEnd"},
            };

            var mapMethod = _sut.GetType().GetMethod("Map")
                .MakeGenericMethod(typeof(TSource), typeof(TTarget));

            // Act
            object result = mapMethod.Invoke(_sut, [source, mappingProps]);

            // Assert
            var expectedTargetProps = expectedTarget.GetType().GetProperties();
            var resultProps = result.GetType().GetProperties();
            
            foreach (var expectedProp in expectedTargetProps)
            {
                var resultName = resultProps.FirstOrDefault(x => x.Name == expectedProp.Name);
                var expectedValue = expectedProp.GetValue(expectedTarget);

                Assert.NotNull(resultName);
            }
        }

        [Fact]
        public void Map_MustReturnException_WhenNullSource()
        {
            // Arrange
            Expense nullExpense = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                _sut.Map<Expense, ExpenseViewModel>(nullExpense));
        }
    }
}
