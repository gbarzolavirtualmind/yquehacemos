using Core.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Core
{
    public interface IRecommendationEngine
    {
        Recommendation GetRecommendationFor(bool[] answers);
    }

    public class Recommendation {
        public int PlaceId { get; set; }
        public string Name { get; set; }
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

    public class GoogleRecommendationEngine : IRecommendationEngine
    {
        public Recommendation GetRecommendationFor(bool[] answers)
        {
            string authorizationHeader = ConfigurationManager.AppSettings["AuthorizationHeader"];
            string urlPredict = ConfigurationManager.AppSettings["urlPredict"];

            var answersInt = answers.ToList().Select(x => (x ? 1: 0)).ToArray();

            var model = new PredictionRequest { input = new PredictionInput { csvInstance = answersInt} };
            DataContractJsonSerializer oSerializer = new DataContractJsonSerializer(typeof(PredictionRequest));

            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
		    {
			    oSerializer.WriteObject(ms, model);
                data = ms.ToArray();
                var json = Encoding.Default.GetString(ms.ToArray());
                System.Diagnostics.Debug.WriteLine(json);
		    }


            

            WebClient client = new WebClient();
            client.Headers.Add("Authorization", authorizationHeader);
            client.Headers.Add("Content-type", "application/json");
            var resultData = client.UploadData(urlPredict,"POST",data);

            PredictionResult result = null;

            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(PredictionResult));

            using (MemoryStream ms = new MemoryStream(resultData))
            {
                result = deserializer.ReadObject(ms) as PredictionResult;
            }

            return new Recommendation
            {
                Name = result.outputLabel,
                PlaceId = 1
            };
        }
    }

    public class PredictionRequest
    {
        [DataMember(Name = "input")]
        public PredictionInput input { get; set; }

    
    }

    [DataContract]
    public class PredictionResult
    {
        [DataMember]
        public string outputLabel { get; set; }

        [DataMember]
        public List<PredictionMulti> outputMulti { get; set; }

    }
    [DataContract]
    public class PredictionMulti
    {
        [DataMember]
        public string label { get; set; }

        [DataMember]
        public string score { get; set; }
    }

    public class PredictionInput
    {
        [DataMember(Name = "csvInstance")]
        public int[] csvInstance { get; set; }
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
