using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POOI_EF_TORRES_VENERO.Models
{
    public class Consumidor
    {
        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression(@"^[0-9]{8}$",
            ErrorMessage = "El DNI debe tener exactamente 8 digitos.")]
        [StringLength(8, MinimumLength = 8)]
        [Display(Name = "DNI")]
        public string Dnicont { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(40)]
        [Display(Name = "Nombre")]
        public string Nomcont { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(40)]
        [Display(Name = "Apellido")]
        public string Apecont { get; set; }

        [EmailAddress(ErrorMessage = "Ingrese un correo valido.")]
        [StringLength(250)]
        [Display(Name = "Correo electronico")]
        public string Emailcont { get; set; }

        [Required(ErrorMessage = "El pais es obligatorio.")]
        [StringLength(3, MinimumLength = 3,
            ErrorMessage = "El codigo del pais debe tener 3 caracteres.")]
        [Display(Name = "Codigo de pais")]
        public string Idpais { get; set; }

        public Consumidor()
        {

        }

        public Consumidor(string dnicont,string nomcont,string apecont,string emailcont,string idpais)
        {
            Dnicont = dnicont;
            Nomcont = nomcont;
            Apecont = apecont;
            Emailcont = emailcont;
            Idpais = idpais;
        }
    }
}