using ast_visual_studio_extension.CxCLI;
using ast_visual_studio_extension.CxWrapper.Exceptions;
using ast_visual_studio_extension.CxWrapper.Models;
using Xunit;

namespace ast_visual_studio_extension_tests.cx_unit_tests.cx_wrapper_tests
{
    [Collection("Cx Collection")]
    public class AuthTest : BaseTest
    {
        [Fact]
        public void TestAuthValidate()
        {
            Assert.NotNull(cxWrapper.AuthValidate());
        }

        [Fact]
        public void TestAuthFailute()
        {
            CxConfig config = GetCxConfig();
            config.ApiKey = "invalidAPIKEY";
            
            Assert.Throws<CxException>(() =>
            {
                CxWrapper cxWrapper = new(config, GetType());
                cxWrapper.AuthValidate();
            });
        }
    }
}