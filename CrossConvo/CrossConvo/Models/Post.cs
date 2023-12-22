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

        [ForeignKey("Utilisateur")]
        public string UtilisateurId { get; set; }

        public virtual Utilisateur? Utilisateur { get; set; }

        public string? Contenu { get; set; }

        public int Likes { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        public ICollection<Commentaire>? Commentaires { get; set; }


        [NotMapped]
        public ICollection<string> LikedUserIds { get; set; } = new List<string>();

        public void IncrementLikes(string userId)
        {
            if (!LikedUserIds.Contains(userId))
            {
                Likes++;
                LikedUserIds.Add(userId);
            }
        }

        // Method to decrement likes for the current user
        public void DecrementLikes(string userId)
        {
            if (LikedUserIds.Contains(userId))
            {
                Likes--;
                LikedUserIds.Remove(userId);
            }
        }
    }
}
