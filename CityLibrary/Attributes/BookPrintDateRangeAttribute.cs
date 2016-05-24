using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityLibrary.Attributes
{
    public class BookPrintDateRangeAttribute : RangeAttribute, IClientValidatable
    {
        public BookPrintDateRangeAttribute() : base(typeof(int), DateTime.Parse("1900-01-01").Year.ToString(), DateTime.Now.Year.ToString())
        {
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {

            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "range"
            };

            rule.ValidationParameters.Add("min", Minimum);
            rule.ValidationParameters.Add("max", Maximum);

            yield return rule;
        }
    }
}