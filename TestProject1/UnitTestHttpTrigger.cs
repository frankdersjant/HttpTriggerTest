using FunctionApp6;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Web.Http;

namespace TestProject1
{
    public class UnitTestHttpTrigger
    {
       
        [Theory]
        [InlineData("", typeof(BadRequestResult))]
        [InlineData("QueryParamValue", typeof(OkResult))]
        [InlineData("ThisStringCausesTheFunctionToThrowAnError", typeof(InternalServerErrorResult))]
        public async Task Function_Returns_Correct_StatusCode(string queryParam, Type expectedResult)
        {
            //Arrange
            var qc = new QueryCollection(new Dictionary<string, StringValues> { { "q", new StringValues(queryParam) } });
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Query)
                .Returns(() => qc);

            var logger = Mock.Of<ILogger>();

            //Act
            var response = await FunctionHttpTrigger.Run(request.Object, logger);

            //Assert
            Xunit.Assert.True(response.GetType() == expectedResult);
        }
    }
}