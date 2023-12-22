﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CrossConvo.Models
{
    public class Ami
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idAmi { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre nom")]
        [DataType(DataType.Text)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Display(Name = "Nom de l'utilisateur")]
        public string? Nom { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre prénom")]
        [DataType(DataType.Text)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Display(Name = "Préom de l'utilisateur")]
        public string? Prenom { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre nom d'utilisateur")]
        [DataType(DataType.Text)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Display(Name = "Username de l'utilisateur")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre email")]
        [DataType(DataType.Text)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Display(Name = "Email de l'utilisateur")]
        public string? Email { get; set; }

        public string? UtilisateurId { get; set; }

        public virtual Utilisateur? Utilisateur { get; set; }
    }
}
