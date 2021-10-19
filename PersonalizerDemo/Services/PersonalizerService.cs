using System;
using System.Collections.Generic;

using Microsoft.Azure.CognitiveServices.Personalizer;
using Microsoft.Azure.CognitiveServices.Personalizer.Models;

namespace PersonalizerDemo.Services
{
    public class PersonalizerService
    {
        private static readonly string apiKey = "";
        private static readonly string serviceEndpoint = "";

        // Initialize Personalizer client
        private static PersonalizerClient personalizerClient = new PersonalizerClient(
            new ApiKeyServiceClientCredentials(apiKey))
        {
            Endpoint = serviceEndpoint
        };

        public static RankResponse GetRecommendation(IList<RankableAction> actions,
            List<object> userContext,
            List<string> excludedActions)
        {
            // Generate an ID to associate with the request.
            var eventId = Guid.NewGuid().ToString();

            // Rank the actions
            var request = new RankRequest(actions, userContext, excludedActions, eventId);
            return personalizerClient.Rank(request);
        }

        public static void SendFeedback(RankResponse recommendation, float feedback)
        {
            personalizerClient.Reward(recommendation.EventId, new RewardRequest(feedback));
        }
    }
}
