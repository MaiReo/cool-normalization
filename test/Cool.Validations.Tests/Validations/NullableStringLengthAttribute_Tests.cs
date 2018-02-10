using Shouldly;
using Xunit;
using Cool.Validations;

namespace Cool.Validations.Tests
{
    public class NullableStringLengthAttribute_Tests
    {
        [Fact( DisplayName = "自定义入参验证模块可空字符串长度验证" )]
        public void IsValid_Test()
        {
            var attr = new NullableStringLengthAttribute( 5 )
            {
                MinimumLength = 2
            };
            attr.IsValid( default( object ) ).ShouldBeTrue();
            attr.IsValid( default( string ) ).ShouldBeTrue();
            attr.IsValid( "12" ).ShouldBeTrue();
            attr.IsValid( "12345" ).ShouldBeTrue();
            attr.IsValid( "" ).ShouldBeFalse();
            attr.IsValid( "1" ).ShouldBeFalse();
            attr.IsValid( "string" ).ShouldBeFalse();

        }
        [Fact]
        public void IsValid_DenyNull_Test()
        {
            var attr = new NullableStringLengthAttribute( 5 )
            {
                MinimumLength = 2,
                AllowNullString = false,
            };
           
            attr.IsValid( "" ).ShouldBeFalse();

            attr.IsValid( default( object ) ).ShouldBeFalse();
            attr.IsValid( default( string ) ).ShouldBeFalse();
        }

        [Fact]
        public void IsValid_AllowEmpty_Test()
        {
            var attr = new NullableStringLengthAttribute( 5 )
            {
                MinimumLength = 2,
                AllowNullString = false,
                AllowEmptyString = true,
            };

            attr.IsValid( "" ).ShouldBeTrue();


            attr.IsValid( default( object ) ).ShouldBeFalse();
            attr.IsValid( default( string ) ).ShouldBeFalse();
        }
    }
}
