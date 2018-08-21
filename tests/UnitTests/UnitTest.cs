using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Newtonsoft.Json;
using URLinq;
using UnitTests.Controllers;
using UnitTests.Models;
using Xunit;

namespace UnitTests
{
    public class UnitTest
    {
        [Theory(DisplayName = "Tests A Simple Get")]
        [InlineData("1")]
        [InlineData("abc")]
        [InlineData("a4d3")]
        public void SimpleGet(string id)
        {
            var route = RouteHelper.GetRoute<UnitTestController, Task<string>>(controller => controller.GetString(id));
            route.Should().NotBeNull();
            var routeAsString = route.ToString();
            routeAsString.Should().Be($"/api/v1/UnitTest?id={id}");
        }

        [Theory(DisplayName = "Tests A Simple Get With A SubRoute")]
        [InlineData("1")]
        [InlineData("abc")]
        [InlineData("a4d3")]
        public void GetStringSubRoute(string id)
        {
            var route = RouteHelper.GetRoute<UnitTestController, Task<string>>(controller => controller.GetStringSubRoute(id));
            route.Should().NotBeNull();
            var routeAsString = route.ToString();
            routeAsString.Should().Be($"/api/v1/UnitTest/SubRoute?id={id}");
        }

        [Theory(DisplayName = "Tests A Simple Get With A Route Parameter")]
        [InlineData("1")]
        [InlineData("abc")]
        [InlineData("a4d3")]
        public void GetStringWithRouteParameter(string id)
        {
            var route = RouteHelper.GetRoute<UnitTestController, Task<string>>(controller => controller.GetStringWithRouteParameter(id));
            route.Should().NotBeNull();
            var routeAsString = route.ToString();
            routeAsString.Should().Be($"/api/v1/UnitTest/{id}");
        }

        [Theory(DisplayName = "Tests A Simple Post With A Complex Model")]
        [MemberData(nameof(GetComplexModels))]
        public async Task PostComplexModel(ComplexModel complexModel)
        {
            var message = RouteHelper.BuildRequestMessage<UnitTestController, Task<string>>(controller => controller.PostComplexModel(complexModel));
            message.Should().NotBeNull();
            message.Method.Should().Be(System.Net.Http.HttpMethod.Post);
            message.Content.Should().BeOfType<StringContent>();
            var content = await message.Content.ReadAsStringAsync();
            var ogContent = JsonConvert.SerializeObject(complexModel);
            content.Should().Be(ogContent);
        }

        /// <summary>
        /// Builds a route from a complex model using a custom binder.  The binder extracts data from the model to use as a route parameter.
        /// </summary>
        /// <param name="complexModel">The complex model.</param>
        /// <returns></returns>
        [Theory(DisplayName = "Tests A Simple Post With A Complex Model Containing A Route Parameter and Custom Binder")]
        [MemberData(nameof(GetComplexModels))]
        public async Task PostComplexModelWithRouteParameter(ComplexModel complexModel)
        {
            //this is how we add custom controller action parameter decomposers
            ControllerActionParameterBinders.AddBinders(new IdModelProvider());
            var message = RouteHelper.BuildRequestMessage<UnitTestController, Task<string>>(controller => controller.PostComplexModelWithId(complexModel));
            message.Should().NotBeNull();
            message.Method.Should().Be(System.Net.Http.HttpMethod.Post);
            message.Content.Should().BeOfType<StringContent>();
            message.RequestUri.ToString().Should().Be($"/api/v1/UnitTest/{complexModel.Id}");
            var content = await message.Content.ReadAsStringAsync();
            var ogContent = JsonConvert.SerializeObject(complexModel);
            content.Should().Be(ogContent);
        }

        public static TheoryData GetComplexModels
        {
            get
            {
                Bogus.Faker faker = new Faker();
                var data = new TheoryData<ComplexModel>();
                data.Add(new ComplexModel
                {
                    Id = faker.Random.AlphaNumeric(10),
                    Email = faker.Person.Email,
                    FullName = faker.Person.FullName,
                    Addresses = new System.Collections.Generic.List<Address>()
                       {
                            new Address
                            {
                                 Street = faker.Person.Address.Street
                            }
                       }
                });
                return data;
            }
        }
    }
}