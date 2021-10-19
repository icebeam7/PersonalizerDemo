using System;
using System.Linq;
using System.Collections.Generic;

using PersonalizerDemo.Services;

namespace PersonalizerDemo
{
    class Program
    {
        private static string ReadCharacter()
        {
            return Console.ReadKey().Key.ToString().Last().ToString().ToUpper();
        }

        private static string GetUsersTimeOfDay()
        {
            var timeOfDayFeatures = new string[] { "morning", "afternoon", "evening", "night" };

            Console.WriteLine("\nWhat time of day is it (enter number)? 1. morning 2. afternoon 3. evening 4. night");
            
            if (!int.TryParse(ReadCharacter(), out int index) || index < 1 || index > timeOfDayFeatures.Length)
            {
                Console.WriteLine($"\nEntered value is invalid. Setting feature value to {timeOfDayFeatures[0]}.");
                index = 1;
            }

            return timeOfDayFeatures[index - 1];
        }

        private static string GetUsersTastePreference()
        {
            var tasteFeatures = new string[] { "salty", "sweet" };

            Console.WriteLine("\nWhat type of food would you prefer (enter number)? 1. salty 2. sweet");

            if (!int.TryParse(ReadCharacter(), out int tasteIndex) || tasteIndex < 1 || tasteIndex > tasteFeatures.Length)
            {
                Console.WriteLine("\nEntered value is invalid. Setting feature value to " + tasteFeatures[0] + ".");
                tasteIndex = 1;
            }

            return tasteFeatures[tasteIndex - 1];
        }

        static void Main(string[] args)
        {
            var iteration = 1;
            var runLoop = true;

            // Get the actions list to choose from personalizer with their features.
            var actions = MenuService.GetActions();

            do
            {
                Console.WriteLine($"\nIteration: {iteration++}");

                // Get context information from the user.
                var timeOfDayFeature = GetUsersTimeOfDay();
                var tasteFeature = GetUsersTastePreference();

                // Create current context from user specified data.
                var currentContext = new List<object>() {
                    new { time = timeOfDayFeature },
                    new { taste = tasteFeature }
                };

                // Exclude an action for personalizer ranking. This action will be held at its current position.
                // This simulates a business rule to force the action "juice" to be ignored in the ranking.
                // As juice is excluded, the return of the API will always be with a probability of 0.
                var excludeActions = timeOfDayFeature != "morning" ? new List<string> { "juice" } : new List<string>();

                // Get recommendation from Personalizer based on context
                var recommendation = PersonalizerService.GetRecommendation(actions, currentContext, excludeActions);
                Console.WriteLine($"\nPersonalizer service thinks you would like to have: {recommendation.RewardActionId}.");

                Console.WriteLine("\nAdditional data: Personalizer Service ranked actions as follows:");

                foreach (var rec in recommendation.Ranking)
                {
                    Console.WriteLine(rec.Id + " " + rec.Probability);
                }

                Console.WriteLine("Did you like the recommendation (Y/N)?");
                var answer = ReadCharacter();

                var reward = 0.0f;

                switch (answer)
                {
                    case "Y":
                        reward = 1;
                        Console.WriteLine("\nGreat! Enjoy your food.");
                        break;
                    case "N":
                        reward = 0;
                        Console.WriteLine("\nYou didn't like the recommended food choice.");
                        break;
                    default:
                        Console.WriteLine("\nEl valor ingresado no es válido. Asumiremos que no te agradó la recomendación de comida.");
                        break;
                }

                // Send the reward for the action based on user response.
                PersonalizerService.SendFeedback(recommendation, reward);

                Console.WriteLine("\nPress q to break, any other key to continue:");
                runLoop = !(ReadCharacter() == "Q");
            } while (runLoop);
        }
    }
}