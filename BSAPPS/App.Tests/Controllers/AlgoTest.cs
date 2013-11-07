using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App;
using App.Controllers;
using Core;
using Moq;

namespace App.Tests
{
    [TestClass]
    public class ServiceTest
    {
        [TestMethod]
        public void Test0CallRecomendationEngine()
        {
            var PlaceRepository = new Moq.Mock<IPlaceRepository>();
            PlaceRepository.Setup(x => x.GetById(2)).Returns(new Place { Name = "Parque Lezama" });

            var Engine = new Moq.Mock<IRecommendationEngine>();
            var answers = new List<bool> { true, false, true }.ToArray();

            Engine.Setup(x => x.GetRecommendationFor(answers)).Returns(new Recommendation { PlaceId = 2 });

            var expectedPlace = new Place { Name = "Parque Lezama" };

            var sut = new Service(Engine.Object,PlaceRepository.Object);
            var place = sut.GetPlace(true, false, true);

            Assert.AreEqual(expectedPlace.Name, place.Name);

            Engine.VerifyAll();

            PlaceRepository.VerifyAll();

        }

        [TestMethod]
        public void Test1()
        {
            var expectedPlace = new Place { Name = "Zoologico BS AS" };

            var sut = new Service(new RecomendationHarcoded(), new PlaceRepositoryHarcoded());
            var place = sut.GetPlace(true, true, true);

            Assert.AreEqual(expectedPlace.Name, place.Name);
        }

        [TestMethod]
        public void Test2()
        {
            var expectedPlace = new Place { Name = "Zoologico BS AS" };

            var sut = new Service(new RecomendationHarcoded(), new PlaceRepositoryHarcoded());
            var place = sut.GetPlace(true, true, false);

            Assert.AreEqual(expectedPlace.Name, place.Name);
        }

        [TestMethod]
        public void Test3()
        {
            var expectedPlace = new Place { Name = "Zoologico BS AS" };

            var sut = new Service(new RecomendationHarcoded(), new PlaceRepositoryHarcoded());
            var place = sut.GetPlace(true, false, true);

            Assert.AreEqual(expectedPlace.Name, place.Name);
        }

        [TestMethod]
        public void Test4()
        {
            var engine = new RecomendationHarcoded();

            var expectedPlace = new Place { Name = "Parque Lezama" };

            var sut = new Service(new RecomendationHarcoded(), new PlaceRepositoryHarcoded());
            var place = sut.GetPlace(true, false, false);

            Assert.AreEqual(expectedPlace.Name, place.Name);
        }

        [TestMethod]
        public void Test5()
        {
            var engine = new RecomendationHarcoded();

            var expectedPlace = new Place { Name = "Antares" };

            var sut = new Service(new RecomendationHarcoded(), new PlaceRepositoryHarcoded());
            var place = sut.GetPlace(false, true, true);

            Assert.AreEqual(expectedPlace.Name, place.Name);
        }

        [TestMethod]
        public void Test6()
        {
            var engine = new RecomendationHarcoded();

            var expectedPlace = new Place { Name = "Antares" };

            var sut = new Service(new RecomendationHarcoded(), new PlaceRepositoryHarcoded());
            var place = sut.GetPlace(false, true, false);

            Assert.AreEqual(expectedPlace.Name, place.Name);
        }



        [TestMethod]
        public void Test7()
        {
            var engine = new RecomendationHarcoded();

            var expectedPlace = new Place { Name = "Antares" };

            var sut = new Service(new RecomendationHarcoded(), new PlaceRepositoryHarcoded());
            var place = sut.GetPlace(false, false, true);

            Assert.AreEqual(expectedPlace.Name, place.Name);
        }

        [TestMethod]
        public void Test8()
        {
            var engine = new RecomendationHarcoded();

            var expectedPlace = new Place { Name = "Museo" };

            var sut = new Service(new RecomendationHarcoded(), new PlaceRepositoryHarcoded());
            var place = sut.GetPlace(false, false, false);

            Assert.AreEqual(expectedPlace.Name, place.Name);
        }

    }

    [TestClass]
    public class RecomendationEngineTest
    {
        [TestMethod]
        public void GetRecommendationPlaceId()
        {
            var sut = new RecomendationHarcoded();
            var answers = new List<bool>{true,true,true};
            var result = sut.GetRecommendationFor(answers.ToArray());

            Assert.AreEqual(result.PlaceId, 1);
        }
    
    }

    

}
