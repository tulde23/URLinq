# ulinq
ULinq is a .NET standard library that converts MVC/WebAPI controller actions into URLs. It is particularly useful for integration testing.


Take for example the following controller:

``` csharp
[Route("api/v1/[controller])]
public class WeatherController : Controller{
  
   [HttpGet]
   public Task<int> GetCurrentTemperatureByPostalCode(int postalCode){
      return Task.FromResult(85);
   }
}
```

Our goal is to refactor this integration test:
```csharp
  [Theory]
 [InlineData(19106,85)]
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
 [InlineData(19106,85)]
 public  async Task GetWeatherForPostalCode(int postalCode, int expectedResult){
    var route = RouteHelper.GetRoute<WeatherController, int>(controller => controller.GetCurrentTemperatureByPostalCode(postalCode));
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

Now let's register our binder. 
``` csharp

        ControllerActionParameterBinders.AddBinders(new CustomFromModelBinder());
       
```

That's it!  Enjoy.