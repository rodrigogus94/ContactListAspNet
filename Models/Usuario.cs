using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebMySQL.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Display(Name = "Id")]
        [Column("Id")]
        public int Id { get; set; }

        [Display(Name = "DTN_ID")]
        [Required(ErrorMessage = "O DTN_ID é obrigatório e tem que ser de 00 à 1000.")]
        [Range(0,1000)]
        [Column("DTN_ID")]
        [StringLength(20)]
        public string DTN_ID { get; set; }

        [Display(Name = "DTN_DESTINATION")]
        [Required(ErrorMessage = "O DTN_ID é obrigatório e tem que ser de 0 à 10000.")]
        [Range(00,10000)]
        [Column("DTN_DESTINATION")]
        [StringLength(10)]
        public string DTN_DESTINATION { get; set; }
        
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O nome obrigatório.")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage =
            "Números e caracteres especiais não são permitidos no nome.")]
        [Column("Nome")]
        public string Nome { get; set; }

        [Display(Name = "Celular")]
        [Required(ErrorMessage = "O celular é obrigatório.")]
        [RegularExpression(@"^\(?([0-9]{2})\)?[-. ]?([0-9]{5})[-. ]?([0-9]{4})$", 
            ErrorMessage = "O formato do núemro do celular tem que ser (xx)xxxxx-xxxx.")]
        [Column("Celular")]
        public string Celular { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "O Email é obrigatório.")]
        [RegularExpression(".+\\@.+\\..+",ErrorMessage = "Informe um email válido...")]
        [Column("Email")]
        public string Email { get; set; }


        
    }
    
    
}
