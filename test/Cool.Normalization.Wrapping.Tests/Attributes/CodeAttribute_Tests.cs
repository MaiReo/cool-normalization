using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Cool.Normalization.Tests
{
    public class CodeAttribute_Tests
    {
        [Fact( DisplayName = "Code特性性定义在类" )]
        public void Attribute_On_Class_Test()
        {
            typeof( TestAttributeClass ).GetCustomAttributes( true ).OfType<ICodeAttribute>()
            .First().Code.ShouldBe( "01" );
        }

        [Fact( DisplayName = "Code特性性定义在属性" )]
        public void Attribute_On_Property_Test()
        {
            typeof( TestAttributeClass )
                .GetProperty( nameof( TestAttributeClass.Property ) )
                .GetCustomAttributes( true ).OfType<ICodeAttribute>()
                .First().Code.ShouldBe( "02" );
        }

        [Fact( DisplayName = "Code特性性定义在方法" )]
        public void Attribute_On_Method_Test()
        {
            typeof( TestAttributeClass )
                .GetMethod( nameof( TestAttributeClass.Method ) )
                .GetCustomAttributes( true ).OfType<ICodeAttribute>()
                .First().Code.ShouldBe( "03" );
        }
    }


    [Code( "01" )]
    public class TestAttributeClass
    {
        [Code( "02" )]
        public string Property { get; set; }

        [Code( "03" )]
        public void Method()
        {

        }
    }


}
