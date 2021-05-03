using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Utilidades
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var nombrePropiedad = bindingContext.ModelName;
            var valor = bindingContext.ValueProvider.GetValue(nombrePropiedad);

            if (valor == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            try
            {
                var varlorDeserializado = JsonConvert.DeserializeObject<T>(valor.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(varlorDeserializado);
            }
            catch (Exception)
            {
                // Un error, por ejemplo, que es esté esperando un listado de números y  se reciba un de strings
                bindingContext.ModelState.TryAddModelError(nombrePropiedad, "El valor dado no es del tipo adecuado");
            }

            return Task.CompletedTask;


        }

    }
}
