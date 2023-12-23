using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace CrossConvo.Models
{
    public class Utilisateur : IdentityUser
    {
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
        public Groupe Groupe { get; set; }

        public ICollection<Ami>? Amis { get; set; }
        public ICollection<Post>? Posts { get; set; }

        public ICollection<Commentaire>? Commentaires { get; set; }
    }
}