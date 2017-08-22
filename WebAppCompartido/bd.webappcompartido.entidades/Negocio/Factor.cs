namespace bd.webappcompartido.entidades
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Factor
    {
        [Key]
        public int IdFactor { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Porciento:")]
        [Range(1, 100, ErrorMessage = "El {0} debe estar entre los valores {1} y {2} ")]
        public decimal? Porciento { get; set; }

        //Propiedades Virtuales Referencias a otras clases

      
    }
}
