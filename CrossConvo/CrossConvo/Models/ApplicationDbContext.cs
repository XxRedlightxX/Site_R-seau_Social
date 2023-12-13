using CrossConvo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CrossConvo.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Groupe> Groupes { get; set; }
        public DbSet<GroupeUtilisateur> GroupesUtilisateurs { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Ami> Amis { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Commentaire> Commentaires { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupeUtilisateur>()
                .HasKey(gu => new { gu.GroupeId, gu.UtilisateurId });

            modelBuilder.Entity<GroupeUtilisateur>()
                .HasOne(gu => gu.Groupe)
                .WithMany(g => g.GroupesUtilisateurs)
                .HasForeignKey(gu => gu.GroupeId);

            modelBuilder.Entity<GroupeUtilisateur>()
                .HasOne(gu => gu.Utilisateur)
                .WithMany(u => u.GroupesUtilisateurs)
                .HasForeignKey(gu => gu.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Commentaire>()
                .HasOne(c => c.Utilisateur)
                .WithMany(u => u.Commentaires)
                .OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<Ami>()
                .HasOne(ami => ami.Utilisateur)
                .WithMany(u => u.Amis)
                .HasForeignKey(ami => ami.UtilisateurId)
                .OnDelete(DeleteBehavior.SetNull);


           

        }

        public void SeedData()
        {
            if (!Groupes.Any() && !Utilisateurs.Any())
            {
                Groupes.RemoveRange(Groupes);
                Utilisateurs.RemoveRange(Utilisateurs);
                SaveChanges();

                var SampleGroupes = SampleDonnees.GetGroupes();
                foreach (var groupe in SampleGroupes)
                {
                    Groupes.Add(groupe);
                }
                SaveChanges();
            }
        }
    }
}