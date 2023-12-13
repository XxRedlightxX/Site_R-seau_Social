using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CrossConvo.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Le champ Titre est requis.")]
        [Display(Name = "Titre de l'article")]
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date de publication")]
        public DateTime? PublicationDate { get; set; }

        public int UtilisateurId { get; set; }
        
        public virtual Utilisateur? Utilisateur { get; set; }

        public string? Contenu { get; set; }

        public int Likes { get; set; }

        public virtual ICollection<Commentaire>? Commentaires { get; set; }
        [NotMapped]
        public virtual ICollection<Utilisateur>? users { get; set; }






        public void IncrementLikes() 
        {
            Likes++; // Incrémente le nombre de likes de 1
        }
    }
}
