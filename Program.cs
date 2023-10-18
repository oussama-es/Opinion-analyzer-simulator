using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace OpinionAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ouverture du fichier texte
            string fileName = "E:\\3eme_Annee\\S6\\DotNet\\Simulateur d'analyseur d'opinitions\\sport.txt";
            string text = File.ReadAllText(fileName);

            // Découpage du contenu en mots
            string[] words = text.Split(' ', '\n', '\r', '\t');

            // Suppression des mots sans intérêt
            List<string> stopWords = new List<string> { "le", "la", "les", "de", "du", "des", "un", "avoir", "être", "faire", "malgré", "mais", "même", "si", "pour", "quel", "quelle", "que", "qui", "à", "au", "et", "je", "tu", "il", "ils", "elle", "elles", "nous", "vous", "on", "mon", "ma", "mes", "son", "sa", "ses", "ce", "ces", "dans", "dont", "etc" , ""};

            List<string> meaningfulWords = new List<string>();
            foreach (string word in words)
            {
                string trimmedWord = word.Trim(new char[] { '.', ',', ':', ';', '!', '?', '(', ')', '[', ']', '{', '}', '-', '_', '/', '\\' });
                if (trimmedWord.All(Char.IsLetter) && !stopWords.Contains(trimmedWord))
                {
                    meaningfulWords.Add(trimmedWord.ToLower());
                }
            }

            // Création du dictionnaire d'occurrences de chaque mot
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            foreach (string word in meaningfulWords)
            {
                if (wordCount.ContainsKey(word))
                {
                    wordCount[word]++;
                }
                else
                {
                    wordCount.Add(word, 1);
                }
            }

            // Détermination de la catégorie du text
            Dictionary<string, string[]> categoryKeywords = new Dictionary<string, string[]>
            {
                { "sport", new string[] { "match", "score", "joueur", "victoire", "perte", "football", "basketball", "volleyball", "handball", "tennis", "natation", "équipes", "équipe", "musculation", "corps", "calories", "poids", "santé", "cardio" } },
                { "cinéma", new string[] { "film", "acteurs", "acteur", "actrice", "scénario", "réalisateur", "oscar", "cinéma" } },
                { "cuisine", new string[] { "recette", "ingrédient", "plat", "repas", "nourriture", "chef", "restaurant", "cuisine", "gastronomie" } }
            };

            const int minOccurrences = 4;
            string category = "inconnue";
            foreach (KeyValuePair<string, string[]> kvp in categoryKeywords)
            {
                int occurrences = 0;
                foreach (string keyword in kvp.Value)
                {
                    if (wordCount.ContainsKey(keyword) /*&& wordCount[keyword] >= minOccurrences*/)
                    {
                        occurrences++;
                    }
                }
                if (occurrences >= minOccurrences)
                {
                    category = kvp.Key;
                    break;
                }
            }

            // Détermination de l'opinion du rédacteur
            Dictionary<string, string[]> opinionKeywords = new Dictionary<string, string[]>
            {
                { "neutre", new string[] { "tolérable", "égal", "correcte", "normal", "moyen", "moyenne", "acceptable" } },
                { "positif", new string[] { "excellents", "bon", "apprécié", "extraordinaire", "meilleur", "meilleure", "meilleures", "bénéfique" } },
                { "négatif", new string[] { "ennuyeux", "médiocre", "déçu", "fade", "décevant", "décevante" } }
            };
            bool ok = false;
            string opinion = "inconnue";
            foreach (KeyValuePair<string, string[]> kvp in opinionKeywords)
            {
                foreach (string keyword in kvp.Value)
                {
                    if (wordCount.ContainsKey(keyword))
                    {
                        opinion = kvp.Key;
                        ok = true;
                    }
                    if (ok) break;
                }
                if (ok) break;
            }
            Console.WriteLine("Catégorie : " + category);
            Console.WriteLine("Opinion : " + opinion);
        }
    }
}
