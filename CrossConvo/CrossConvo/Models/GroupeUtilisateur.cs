using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CrossConvo.Models
{
    public class GroupeUtilisateur
    {
        [Key]
        [Column(Order = 1)]
        public string GroupeId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string UtilisateurId { get; set; }

        
        public virtual Groupe? Groupe { get; set; }

        
        public virtual Utilisateur? Utilisateur { get; set; }
    }
}
