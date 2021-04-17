using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Validaciones
{
    public class PrimeraLetraMayusculaAttribute: ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((value == null) || string.IsNullOrEmpty(value.ToString()))
            {
                // Devuelve Success porque ya hay una regla (atributo que controla esto: [Required]
                // No queremos que aquí se valide lo que otro debe validar (aquí se ignora esa regla de validación y se espera que lo haga [Required])
                return ValidationResult.Success; 
            }

            var primeraLetra = value.ToString()[0].ToString();
            if (primeraLetra != primeraLetra.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayúscula");
            }

            return ValidationResult.Success;
        }

    }
}
