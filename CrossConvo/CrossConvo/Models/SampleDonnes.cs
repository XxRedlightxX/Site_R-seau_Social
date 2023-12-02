using System;
using System.Collections.Generic;
using CrossConvo.Models;

namespace CrossConvo.Models
{
    public class SampleDonnees
    {
        public static List<Utilisateur> Utilisateurs = new List<Utilisateur>();
        public static List<Groupe> Groupes = new List<Groupe>();

        public static Groupe[] GetGroupes()
        {
            DateTime datePublication = DateTime.Now;

            // Create users
            Utilisateur utilisateur1 = new Utilisateur
            {
                Nom = "LaBaie",
                Prenom = "Ariel",
                Username = "ariel1234",
                Email = "ariel1234@gmail.com",
                Password = "ariel1234",
                Amis = new List<Ami>(),
                Posts = new List<Post>()
            };

            Utilisateur utilisateur2 = new Utilisateur
            {
                Nom = "LaRouge",
                Prenom = "Amanda",
                Username = "amanda1234",
                Email = "amanda1234@gmail.com",
                Password = "amandal1234",
                Amis = new List<Ami>(),
                Posts = new List<Post>()
            };

            // Add friends to users
            utilisateur1.Amis.Add(new Ami
            {
                Nom = "LaRouge",
                Prenom = "Amanda",
                Username = "amanda1234",
                Email = "amanda1234@gmail.com"
            });

            // Create posts
            Post post1 = new Post
            {
                Title = "Post 1",
                PublicationDate = datePublication,
                Contenu = "Salut 1",
                Likes = 5,
                Commentaires = new List<Commentaire>
                {
                    new Commentaire
                    {
                        PublicationDate = datePublication,
                        Contenu = "Oui",
                        Likes = 1
                    },
                    new Commentaire
                    {
                        PublicationDate = datePublication,
                        Contenu = "Salut ariel",
                        Likes = 0
                    }
                }
            };

            Post post2 = new Post
            {
                Title = "Post 2",
                PublicationDate = datePublication,
                Contenu = "Salut 2",
                Likes = 5,
                Commentaires = new List<Commentaire>
                {
                    new Commentaire
                    {
                        PublicationDate = datePublication,
                        Contenu = "Non",
                        Likes = 0
                    }
                }
            };

            utilisateur1.Posts.Add(post1);
            utilisateur1.Posts.Add(post2);
            utilisateur2.Posts.Add(post1);

            // Create groups
            Groupe groupe1 = new Groupe
            {
                Nom = "Groupe 1",
                GroupesUtilisateurs = new List<GroupeUtilisateur>
                {
                    new GroupeUtilisateur
                    {
                        Utilisateur = utilisateur1
                    },
                    new GroupeUtilisateur
                    {
                        Utilisateur = utilisateur2
                    }
                }
            };

            // Add users and groups to the lists
            Utilisateurs.Add(utilisateur1);
            Utilisateurs.Add(utilisateur2);
            Groupes.Add(groupe1);

            return Groupes.ToArray();
        }
    }
}