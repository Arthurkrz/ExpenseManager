using ExpenseManager.Web.Mapping.Utilities;
using System;
using System.Reflection;
using Xunit;

namespace ExpenseManager.Tests.Mapping.TestModels
{
    public class MappingTypeHelperTests
    {
        [Fact]
        public void CreateTargetInstance_ShouldThrowInvalidOperationException_WhenInterfaceTargetType()
        {
            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => 
                MappingTypeHelper.CreateTargetInstance(typeof(InterfaceTarget)));

            Assert.Equal("Cannot create nested target " +
                "type 'ExpenseManager.Tests.Mapping.Test" +
                "Models.InterfaceTarget' because " +
                "it is an interface.", exception.Message);
        }

        [Fact]
        public void CreateTargetInstance_ShouldThrowInvalidOperationException_WhenAbstractTargetType()
        {
            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                MappingTypeHelper.CreateTargetInstance(typeof(AbstractTarget)));

            Assert.Equal("Cannot create nested target " +
                "type 'ExpenseManager.Tests.Mapping." +
                "TestModels.AbstractTarget' " +
                "because it is abstract.", exception.Message);
        }

        [Fact]
        public void CreateTargetInstance_ShouldThrowInvalidOperationException_WhenNestedTargetTypeConstructorThrows()
        {
            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() =>
                MappingTypeHelper.CreateTargetInstance(
                    typeof(NestedAddressTargetThrowException)));

            Assert.Equal("The constructor for nested " +
                "target type 'ExpenseManager.Tests." +
                "Mapping.TestModels.NestedAddressTargetThrowException' " +
                "threw an exception.", exception.Message);
        }
    }
}
