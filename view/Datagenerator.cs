// Aimee


// using System;
// using System.IO;
// using System.Text.Json;
// using System.Text.RegularExpressions;

// public static class DataGenerator
// {
//     private static int _nextId = 1; // Centrale teller voor unieke ID's

//     public static void Run()
//     {
//         string folderName = "jsontasks";
        
//         // Begin met een schone lei: verwijder de map als die al bestaat en maak hem opnieuw
//         if (Directory.Exists(folderName)) Directory.Delete(folderName, true);
//         Directory.CreateDirectory(folderName);

//         Console.WriteLine("Generating 100 tasks...");

//         // 1. Maak twee complexe ketens (6 diep elk = 12 taken)
//         CreateAndSaveChain(folderName, "Software Launch", 
//             new[] { "Launch App on Store", "Final Marketing Push", "Write Documentation", "Fix Final Bugs", "Beta Testing Phase", "Setup Database Schema" });

//         CreateAndSaveChain(folderName, "Wedding Plan", 
//             new[] { "The Big Wedding Day", "Final Rehearsal", "Flower and Decor Setup", "Confirm Catering Menu", "Send Invitations", "Book Venue" });

//         // 2. De rest van de onderwerpen voor losse taken (tot we op 100 zitten)
//         string[] topics = {
//             "Clean washing machine", "Wash windows", "Degrease oven", "Unclog sink", "Flip mattresses",
//             "Update passwords", "Clean desktop", "Delete apps", "Backup photos", "Inbox Zero",
//             "Cancel subscriptions", "Clear history", "Update LinkedIn", "Organize cloud", "Archive mail",
//             "Install updates", "Download invoices", "Check spam", "Change profile pic", "Weekly schedule",
//             "Finalize minutes", "Prepare slides", "Market research", "Project deadline", "Networking call",
//             "Read chapter", "Update portfolio", "Brainstorm", "Send invoice", "Order supplies",
//             "Request leave", "Team outing", "Ergonomic check", "Feedback prep", "Meal plan",
//             "Farmers market", "Prep lunch", "Try recipe", "Buy coffee", "Refill spices",
//             "Clean bottle", "Take vitamins", "Sort baking", "Visit butcher", "30-min walk",
//             "Book dentist", "Pack gym bag", "Meditate", "Call hair", "Trim nails",
//             "Clean shoes", "Check first aid", "Sleep cycle", "Yoga", "Drink water",
//             "Eye test", "Face mask", "Stretch", "Fruit bowl", "Tire pressure",
//             "Vacuum car", "Oil bike", "Transit card", "Washer fluid", "Book hotel",
//             "Pack suitcase", "Check passport", "Renew permit", "Map route", "Mow lawn",
//             "Pull weeds", "Water plants", "Trim hedge", "Scrub patio", "Clean birdhouse"
//         };

//         int topicIndex = 0;
//         while (_nextId <= 100)
//         {
//             var task = new {
//                 Id = _nextId,
//                 Description = topics[topicIndex % topics.Length],
//                 Priority = (_nextId % 3 == 0) ? "must have" : "should have",
//                 dependant = (object)null,
//                 Status = "to do",
//                 Completed = false,
//                 CreationDate = DateTime.Now.AddDays(-_nextId).ToString("O")
//             };

//             SaveTaskToFile(folderName, task);
//             _nextId++;
//             topicIndex++;
//         }

//         Console.WriteLine($"Succes! 100 files created in /{folderName}");
//     }

//     private static void CreateAndSaveChain(string folder, string chainName, string[] chainTopics)
//     {
//         object currentDependant = null;

//         // We bouwen de keten van achter naar voren op (recursief object maken)
//         // Maar we moeten de ID's toewijzen zodat de root (de laatste taak) de hoogste ID heeft
//         for (int i = 5; i >= 0; i--)
//         {
//             var taskObj = new {
//                 Id = _nextId++,
//                 Description = chainTopics[i],
//                 Priority = "must have",
//                 dependant = currentDependant,
//                 Status = "to do",
//                 Completed = false,
//                 CreationDate = DateTime.Now.ToString("O")
//             };

//             // Alleen de 'root' van de keten slaan we op als bestand, 
//             // omdat jouw systeem de rest via de 'dependant' property inlaadt.
//             if (i == 0) 
//             {
//                 SaveTaskToFile(folder, taskObj);
//             }
//             else 
//             {
//                 // We houden het object vast om in de volgende (hogere) taak te stoppen
//                 currentDependant = taskObj;
//             }
//         }
//     }

//     private static void SaveTaskToFile(string folder, object task)
//     {
//         // Gebruik 'dynamic' om toegang te krijgen tot Id en Description van het anonieme object
//         dynamic t = task;
//         int id = t.Id;
//         string desc = t.Description;

//         // Maak de beschrijving veilig voor bestandsnamen (geen spaties of vreemde tekens)
//         string safeDesc = Regex.Replace(desc, @"[^a-zA-Z0-9]", "_");
//         string fileName = $"{id}_{safeDesc}.json";
        
//         string filePath = Path.Combine(folder, fileName);
//         string json = JsonSerializer.Serialize(task, new JsonSerializerOptions { WriteIndented = true });
        
//         File.WriteAllText(filePath, json);
//     }
// }