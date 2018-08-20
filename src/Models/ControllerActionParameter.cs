﻿using System;
using System.Collections.Generic;
using System.Text;
using AspNetCore.IntegrationTesting.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCore.IntegrationTesting.Models
{
    internal class ControllerActionParameter : IControllerActionParameter
    {
        public string ParameterName { get; }
        public object ParameterValue { get; }
        public IBindingSourceMetadata BindingSourceMetadata { get; }

        public IControllerAction Action { get; }

        internal ControllerActionParameter(string parameterName, object parameterValue, IBindingSourceMetadata metadata, IControllerAction controllerAction)
        {
            ParameterName = parameterName;
            ParameterValue = parameterValue;
            BindingSourceMetadata = metadata;
            Action = controllerAction;
        }
    }
}
