using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CrossConvo.Models
{
    public class Groupe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupeId { get; set; }

        [DataType(DataType.Text)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Display(Name = "Nom du groupe")]
        public string Nom { get; set; }

        public ICollection<Utilisateur>? Utilisateurs { get; set; }
    }
}
