using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CrossConvo.Models
{
    public class Utilisateur : IdentityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UtilisateurId { get; set; }

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

        [DataType(DataType.ImageUrl)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        [Display(Name = "Image")]
        public byte[]? ProfilePicture { get; set; }


        [Required(ErrorMessage = "Veuillez entrer votre email")]
        [DataType(DataType.Text)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Display(Name = "Email de l'utilisateur")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre password")]
        [DataType(DataType.Password)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Display(Name = "Password de l'utilisateur")]
        public string? Password { get; set; }

       
        public virtual ICollection<GroupeUtilisateur>? GroupesUtilisateurs { get; set; }

        public virtual ICollection<Ami>? Amis { get; set; }

        public virtual ICollection<Post>? Posts { get; set; }

        public virtual ICollection<Commentaire>? Commentaires { get; set; }

      
    }
}

