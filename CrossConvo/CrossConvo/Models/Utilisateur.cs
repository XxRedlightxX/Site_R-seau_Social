using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace CrossConvo.Models
{
    public class Utilisateur : IdentityUser
    {
        public int UtilisateurId { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre nom")]
        [DataType(DataType.Text)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Display(Name = "Nom de l'utilisateur")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre prénom")]
        [DataType(DataType.Text)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Display(Name = "Prénom de l'utilisateur")]
        public string Prenom { get; set; }

        public int? GroupeId { get; set; }
        public virtual Groupe Groupe { get; set; }

        public virtual ICollection<Ami>? Amis { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }

        public virtual ICollection<Commentaire>? Commentaires { get; set; }
    }
}