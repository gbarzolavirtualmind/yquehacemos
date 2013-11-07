using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class Service
    {
        private IRecommendationEngine engine;
        private IPlaceRepository repository;
        public Service(IRecommendationEngine Engine, IPlaceRepository Repository)
        {
            engine = Engine;
            repository = Repository;
        }

        public Place GetPlace(bool answer1, bool answer2, bool answer3)
        {
            List<bool> answers = new List<bool>{answer1,answer2,answer3};

            var recomendation = engine.GetRecommendationFor(answers.ToArray());

            return repository.GetById(recomendation.PlaceId);

        }
    }
}
