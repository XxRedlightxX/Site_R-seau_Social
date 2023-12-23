using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CrossConvo.Models
{
    public class Commentaire
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentaireId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date de publication")]
        public DateTime? PublicationDate { get; set; }

        public string? Contenu { get; set; }


        public int PostId { get; set; }

        public Post? Post { get; set; }

        public int Likes { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre ID d'utilisateur")]
        [DataType(DataType.Text)]
        [Column(TypeName = "NVARCHAR(450)")]
        [StringLength(450)]
        [Display(Name = "ID de l'utilisateur")]
        public string? UtilisateurId { get; set; }

        [ForeignKey(nameof(UtilisateurId))]
        public Utilisateur Utilisateur { get; set; }
    }
}
