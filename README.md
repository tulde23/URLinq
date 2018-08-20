# ulinq
ULinq is a .NET standard library that converts MVC/WebAPI controller actions into URLs. It is particularly useful for integration testing.  Think of it as the inverse of model binding.

Our goal is to refactor this:
```csharp
  [Theory]
 [MemberData(nameof(CommonData))]
 public async Task GetWeatherForPostalCode(int postalCode, int expectedResult){
  
    var response = await Client.GetAsync($"/api/v1/Weather?postalCode={postalCode}");
    var json = await response.Content.ReadAsStringAsync();
    var data = JsonConvert.DeserializeObject<int>(json);
    data.Should().Equal(expectedResult);
   
  } 
```
Into this:
``` csharp
 [Theory]
 [MemberData(nameof(CommonData))]
 public  async Task GetWeatherForPostalCode(int postalCode, int expectedResult){
    var route = RouteHelper.GetRoute<WeatherController, int>(controller => controller.GetByPostalCode(postalCode));
    var response = await Client.GetAsync(route.ToString());
    var json = await response.Content.ReadAsStringAsync();
    var data = JsonConvert.DeserializeObject<int>(json);
    data.Should().Equal(expectedResult);
   
}
```
### Customization

For most scenarios, the default model binders should be able to map your action parameters back to a URI.  However, in some cases, like custom model binders, you will need to write a bit of code to help out.

First create the route binder:
``` csharp
/// <summary>
    /// Pulls  an id from an inbound model and sets it on the route
    /// </summary>
    /// <seealso cref="AbstractRouteBinder" />
    public class CustomFromModelRouteBinder : AbstractRouteBinder
    {
        /// <summary>
        /// Determines whether this instance can bind the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// <c>true</c> if this instance can decompose the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanBind(IControllerActionParameter parameter)
        {
            var model = parameter.ParameterValue as MyModel;
            return model != null;
        }

        /// <summary>
        /// Binds the parameter to the route.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="controllerActionRoute">The controller action route.</param>
        protected override void BindParameter(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute)
        {
            var model = parameter.ParameterValue as MyModel;
            controllerActionRoute.SetRouteValue("id", model.PersonId);
        }
    } 
```

Now let's register our binder. You do this in our IntegrationTestFixture constructor.
``` csharp

        ControllerActionParameterBinders.AddBinders(new CustomFromModelBinder());
       
```

That's it!  Enjoy.