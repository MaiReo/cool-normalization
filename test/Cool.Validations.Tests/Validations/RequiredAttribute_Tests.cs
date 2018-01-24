using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cool.Validations.Tests.Validations
{
    public class RequiredAttribute_Tests
    {
        [Fact]
        public void Required_Test()
        {
            var attr = new RequiredAttribute();

            attr.IsValid( default( byte ) ).ShouldBeFalse();
            attr.IsValid( default( short ) ).ShouldBeFalse();
            attr.IsValid( default( int ) ).ShouldBeFalse();
            attr.IsValid( default( long ) ).ShouldBeFalse();

            attr.IsValid( default( float ) ).ShouldBeFalse();
            attr.IsValid( default( double ) ).ShouldBeFalse();
            attr.IsValid( default( decimal ) ).ShouldBeFalse();

            attr.IsValid( default( char ) ).ShouldBeFalse();
            attr.IsValid( default( string ) ).ShouldBeFalse();
            attr.IsValid( string.Empty ).ShouldBeFalse();

            attr.IsValid( default( object ) ).ShouldBeFalse();
        }

        [Fact]
        public void Required_AllowDefaultValue_Test()
        {
            var attr = new RequiredAttribute()
            {
                AllowDefaultValue = true
            };

            attr.IsValid( default( byte ) ).ShouldBeTrue();
            attr.IsValid( default( short ) ).ShouldBeTrue();
            attr.IsValid( default( int ) ).ShouldBeTrue();
            attr.IsValid( default( long ) ).ShouldBeTrue();

            attr.IsValid( default( float ) ).ShouldBeTrue();
            attr.IsValid( default( double ) ).ShouldBeTrue();
            attr.IsValid( default( decimal ) ).ShouldBeTrue();

            attr.IsValid( default( char ) ).ShouldBeTrue();
            attr.IsValid( default( string ) ).ShouldBeFalse();
            attr.IsValid( string.Empty ).ShouldBeFalse();

            attr.IsValid( default( object ) ).ShouldBeFalse();
        }
    }
}
