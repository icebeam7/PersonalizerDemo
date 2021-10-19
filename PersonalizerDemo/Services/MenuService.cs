using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Personalizer.Models;

namespace PersonalizerDemo.Services
{
    public class MenuService
    {
        public static IList<RankableAction> GetActions()
        {
            return new List<RankableAction>
            {
                new RankableAction
                {
                    Id = "pasta",
                    Features = new List<object>() 
                    { 
                        new 
                        { 
                            taste = "salty", 
                            spiceLevel = "medium" 
                        }, 
                        new 
                        { 
                            nutritionLevel = 5, 
                            cuisine = "italian" 
                        } 
                    }
                },

                new RankableAction
                {
                    Id = "ice cream",
                    Features = new List<object>() 
                    { 
                        new 
                        { 
                            taste = "sweet", 
                            spiceLevel = "none" 
                        }, 
                        new 
                        { 
                            nutritionalLevel = 2 
                        } 
                    }
                },

                new RankableAction
                {
                    Id = "juice",
                    Features = new List<object>() 
                    { 
                        new 
                        { 
                            taste = "sweet", 
                            spiceLevel = "none" 
                        }, 
                        new 
                        { 
                            nutritionLevel = 5 
                        }, 
                        new 
                        { 
                            drink = true 
                        } 
                    }
                },

                new RankableAction
                {
                    Id = "salad",
                    Features = new List<object>() 
                    { 
                        new 
                        { 
                            taste = "salty", 
                            spiceLevel = "low" 
                        }, 
                        new 
                        { 
                            nutritionLevel = 8 
                        } 
                    }
                }
            };
        }
    }
}