using Core.Domain;
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

        public Place GetPlace(bool[] answers)
        {
            var recomendation = engine.GetRecommendationFor(answers.ToArray());

            return new Place { Name = recomendation.Name };

            //var place = repository.GetById(recomendation.PlaceId);

            //return repository.GetById(recomendation.PlaceId);

        }
    }
}
