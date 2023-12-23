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


        public string? Contenu { get; set; }

        public int Likes { get; set; }

  
        [DataType(DataType.Upload)]
        public byte[]? File { get; set; }

        public ICollection<Commentaire>? Commentaires { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre ID d'utilisateur")]
        [DataType(DataType.Text)]
        [Column(TypeName = "NVARCHAR(450)")]
        [StringLength(450)]
        [Display(Name = "ID de l'utilisateur")]
        public string? UtilisateurId { get; set; }

        [ForeignKey(nameof(UtilisateurId))]
        public Utilisateur Utilisateur { get; set; }

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
