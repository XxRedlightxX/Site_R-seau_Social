using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CrossConvo.Models
{
    public class Utilisateur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        [Required(ErrorMessage = "Veuillez entrer votre nom d'utilisateur")]
        [DataType(DataType.Text)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Display(Name = "Username de l'utilisateur")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre email")]
        [DataType(DataType.Text)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Display(Name = "Email de l'utilisateur")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre password")]
        [DataType(DataType.Password)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Display(Name = "Password de l'utilisateur")]
        public string Password { get; set; }


        public virtual ICollection<GroupeUtilisateur>? GroupesUtilisateurs { get; set; }
        
        public virtual ICollection<Ami>? Amis { get; set; }
        
        public virtual ICollection<Post>? Posts { get; set; }

        public virtual ICollection<Commentaire>? Commentaires { get; set; }
    }
}