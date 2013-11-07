using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public interface IRecommendationEngine
    {
        Recommendation GetRecommendationFor(bool[] answers);
    }

    public class Recommendation {
        public int PlaceId { get; set; }
    }

    public class RecomendationHarcoded : IRecommendationEngine
    {
        private Recommendation zoologico;
        private Recommendation parque;
        private Recommendation antares;
        private Recommendation museo;
        public RecomendationHarcoded()
        {
            zoologico = new Recommendation { PlaceId = 1 };
            parque = new Recommendation { PlaceId = 2 };
            antares = new Recommendation { PlaceId = 3 };
            museo = new Recommendation { PlaceId = 4 };

        }


        public Recommendation GetRecommendationFor(bool[] answers)
        {
            if (answers[0] == true && answers[1] == true)
            {
                return zoologico;
            }

            if (answers[0] == true && answers[1] == false && answers[2] == true)
            {
                return zoologico;
            }

            if (answers[0] == true && answers[1] == false && answers[2] == false)
            {
                return parque;
            }
            if (answers[0] == false && answers[1] == true && answers[2] == true)
            {
                return antares;
            }

            if (answers[0] == false && answers[1] == true && answers[2] == false)
            {
                return antares;
            }
            if (answers[0] == false && answers[1] == false && answers[2] == true)
            {
                return antares;
            }

            if (answers[0] == false && answers[1] == false && answers[2] == false)
            {
                return museo;
            }
            return null;
        }
    }

    public class PlaceRepositoryHarcoded : IPlaceRepository
    {
        private List<Place> places;

        public PlaceRepositoryHarcoded()
        {
            places = new List<Place>();

            places.Add(new Place { Name = "Zoologico BS AS", Id = 1 });
            places.Add(new Place { Name = "Parque Lezama", Id = 2 });
            places.Add(new Place { Name = "Antares", Id = 3 });
            places.Add(new Place { Name = "Museo", Id = 4 });

        }

        public Place GetById(int id)
        {
            return places.FirstOrDefault(x => x.Id == id);
        }
    }
}
