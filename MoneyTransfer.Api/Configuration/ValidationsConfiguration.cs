using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;

using MoneyTransfer.Api.Filters;

namespace MoneyTransfer.Api.Configuration
{
    public static class ValidationsConfiguration
    {
        public static IMvcBuilder AddValidations(this IMvcBuilder mvcBuilder)
        {
            if (mvcBuilder == null)
            {
                throw new ArgumentNullException(nameof(mvcBuilder), $"{nameof(mvcBuilder)} should not be null.");
            }

            mvcBuilder.AddMvcOptions(options => options.Filters.Add(typeof(ValidationFilter)));

            mvcBuilder.AddFluentValidation(
                configuration =>
                {
                    configuration.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
                    configuration.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

            return mvcBuilder;
        }
    }
}
