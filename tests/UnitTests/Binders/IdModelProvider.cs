using System;
using System.Collections.Generic;
using System.Text;
using URLinq.Binders;
using URLinq.Contracts;

namespace UnitTests.Models
{
    public class IdModelProvider : AbstractRouteBinder
    {
        public override bool CanBind(IControllerActionParameter parameter)
        {
            return parameter.ParameterValue is ComplexModel;
        }

        protected override void BindParameter(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute)
        {
            var complexModel = parameter.ParameterValue as ComplexModel;
            if (complexModel.Id == null)
            {
                throw new System.ArgumentException("complexModel.Id can not be null.  Did you forget to set it on your source model?");
            }
            controllerActionRoute.SetRouteValue("myid", complexModel.Id);
        }
    }
}
