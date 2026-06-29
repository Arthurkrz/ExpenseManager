using ExpenseManager.Core.Contracts.Mapping;
using ExpenseManager.Core.Entities;
using ExpenseManager.Tests.ObjectGenerators;
using ExpenseManager.Web.Mapping;
using ExpenseManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace ExpenseManager.Tests
{
    public class MappingTest
    {
        private readonly IMap _mapper;

        public MappingTest()
        {
            _mapper = new MappingProfile();
        }

        [Theory]
        [MemberData(nameof(MappingData.GetValidObjects), MemberType = typeof(MappingData))]
        public void Map_MustSuccesfullyConvert<TSource, TTarget>
                    (TSource source, TTarget expectedTarget)
        {
            // Act
            var mappingProps = new Dictionary<string, string>
            {
                { "ValueString", "Value" },
                { "Value", "ValueString" },
                { "ValueRangeStart", "ValueStringRangeStart"},
                { "ValueRangeEnd", "ValueStringRangeEnd"},
                { "ValueStringRangeStart", "ValueRangeStart"},
                { "ValueStringRangeEnd", "ValueRangeEnd"},
            };

            var mapMethod = _mapper.GetType().GetMethod("Map").MakeGenericMethod(typeof(TSource), typeof(TTarget));
            object mapReturn = mapMethod.Invoke(_mapper, new object[] { source, mappingProps });

            // Assert
            PropertyInfo[] expectedTargetProps = expectedTarget.GetType().GetProperties();
            PropertyInfo[] mapReturnProps = mapReturn.GetType().GetProperties();
            
            foreach (var expectedProp in expectedTargetProps)
            {
                var mapReturnProp = mapReturnProps.FirstOrDefault(x => x.Name == expectedProp.Name);

                Assert.NotNull(mapReturnProp);

                var expectedValue = expectedProp.GetValue(expectedTarget);
                var actualValue = mapReturnProp.GetValue(mapReturn);

                Assert.Equal(expectedValue, actualValue);
            }
        }

        [Fact]
        public void Map_MustReturnException_WhenNullSource()
        {
            // Arrange
            Expense nullBill = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                _mapper.Map<Expense, ExpenseViewModel>(nullBill));
        }
    }
}
