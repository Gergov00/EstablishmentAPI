using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace EstablishmentAPI.Extensions
{
    public static class ModelStateExtensions
    {
        public static string Errors(this ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage);
            return string.Join("; ", errors);
        }
    }
}
