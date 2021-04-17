using back_end.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entidades
{
    public class Genero: IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 10)]
        // [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        [Range(18, 20)]
        public int Edad { get; set; }

        [CreditCard]
        public string TargetaDeCredito { get; set; }

        [Url]
        public string URL { get; set; }

        // Validación por modelo
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();
                if (primeraLetra != primeraLetra.ToUpper())
                {
                    // yield indica que el método, operador o le descriptor de acceso get en el que aparece es un iterador.
                    yield return new ValidationResult("La primera letra debe ser mayúscula", 
                                        new string[] { nameof(Nombre) }); // --> aquí le estamos indicando, en 2do parámetro, que el error le corresponde al campo Nombre
                }
            }
        }
    }
}
