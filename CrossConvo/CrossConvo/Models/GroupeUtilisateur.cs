using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CrossConvo.Models
{
    public class GroupeUtilisateur
    {
        [Key]
        [Column(Order = 1)]
        public int GroupeId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int UtilisateurId { get; set; }

        
        public virtual Groupe? Groupe { get; set; }

        
        public virtual Utilisateur? Utilisateur { get; set; }
    }
}
