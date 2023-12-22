using CrossConvo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CrossConvo.Models
{
    public class ApplicationDbContext : IdentityDbContext<Utilisateur>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Groupe> Groupes { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Ami> Amis { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Commentaire> Commentaires { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Commentaire>()
                .HasOne(c => c.Utilisateur)
                .WithMany(u => u.Commentaires)
                .OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<Ami>()
                .HasOne(ami => ami.Utilisateur)
                .WithMany(u => u.Amis)
                .HasForeignKey(ami => ami.UtilisateurId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Utilisateur)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Utilisateur>()
                .HasOne(u => u.Groupe)
                .WithMany(g => g.Utilisateurs)
                .HasForeignKey(u => u.GroupeId)
                .OnDelete(DeleteBehavior.Restrict);
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
