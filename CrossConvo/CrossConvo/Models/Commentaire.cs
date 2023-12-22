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

        public string? UtilisateurId { get; set; }

        public virtual Utilisateur? Utilisateur { get; set; }

        public int PostId { get; set; }

        public virtual Post? Post { get; set; }

        public int Likes { get; set; }
    }
}
