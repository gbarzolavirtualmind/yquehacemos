using App.Backend.Controllers;
using Core;
using Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Backend.Tests
{
    public abstract class CrudTest<T> where T: class
    {
        private BaseCrudController<T> _sut;
        private Moq.Mock<IRepository<T>> _crudRepository;

        protected abstract List<T> CreateListItems();
        protected abstract BaseCrudController<T> CreateController(IRepository<T> crudRepository);
        protected abstract T NewItem();
        protected abstract T EditItem(int id);
        protected abstract T InvalidItem();

        [TestInitialize]
        public void Setup()
        { 
            _crudRepository = new Moq.Mock<IRepository<T>>();
            _sut = CreateController(_crudRepository.Object);
                
        }

        [TestMethod]
        public void ShouldListQuestions()
        {
            var items = CreateListItems();
            _crudRepository.Setup(x => x.GetAll()).Returns(items);

            var view = _sut.Index() as ViewResult;

            Assert.IsInstanceOfType(view.Model, typeof(List<T>));

            var model = view.Model as List<T>;

            Assert.AreEqual(items.Count, model.Count);

            _crudRepository.VerifyAll();
        }

        [TestMethod]
        public void ShouldAddNew()
        {
            var newQuestion = new Question { Content = "New Question" };
            var newItem = NewItem();

            _crudRepository.Setup(x => x.Add(newItem));

            var view = _sut.Create(newItem) as RedirectToRouteResult;

            Assert.IsInstanceOfType(view, typeof(RedirectToRouteResult));

            _crudRepository.VerifyAll();


        }

        [TestMethod]
        public void NotShouldAddNewQuestionWithEmptyContent()
        {
            var invalidItem = InvalidItem();
            _sut.ModelState.AddModelError("*", "mock error message");

            var view = _sut.Create(invalidItem) as ViewResult;
            Assert.AreEqual(invalidItem, view.Model);
        }

        [TestMethod]
        public void ShouldShowEdit()
        {
            var editItem = EditItem(1);

            _crudRepository.Setup(x => x.GetById(1)).Returns(editItem);

            var view = _sut.Edit(1) as ViewResult;
            Assert.AreEqual(editItem, view.Model);

            _crudRepository.VerifyAll();
        }

        [TestMethod]
        public void ShouldEditQuestion()
        {
            var editItem = EditItem(1);

            _crudRepository.Setup(x => x.Update(editItem));

            var view = _sut.Edit(editItem) as RedirectToRouteResult;

            Assert.IsInstanceOfType(view, typeof(RedirectToRouteResult));

            _crudRepository.VerifyAll();
        }

        [TestMethod]
        public void NotShouldEditQuestionWithEmptyContent()
        {

            var invalidItem = InvalidItem();

            _sut.ModelState.AddModelError("*", "mock error message");

            var view = _sut.Edit(invalidItem) as ViewResult;
            Assert.AreEqual(invalidItem, view.Model);
        }

        public void ShouldDeleteQuestion()
        {
            var item = EditItem(1);
            _crudRepository.Setup(x => x.Delete(1));

            var view = _sut.Delete(1);

            Assert.IsInstanceOfType(view, typeof(JsonResult));
            _crudRepository.VerifyAll();
        }
    }

    [TestClass]
    public class QuestionControllerTest : CrudTest<Question>
    {

        protected override List<Question> CreateListItems()
        {
            return new List<Question>{
                new Question{ Id = 1},
                new Question{ Id = 2 },
                new Question{ Id = 3 },
            };
        }

        protected override BaseCrudController<Question> CreateController(IRepository<Question> crudRepository)
        {

            return new QuestionsController(crudRepository);
        }

        protected override Question NewItem()
        {
            return new Question { Id = 1 };
        }

        protected override Question EditItem(int id)
        {
            return new Question { Id = id, Content = "asdasd" };
        }

        protected override Question InvalidItem()
        {
            return new Question {  };
        }
    }

    [TestClass]
    public class AttributesControllerTest : CrudTest<Core.Domain.Attribute> {

        protected override List<Core.Domain.Attribute> CreateListItems()
        {
            return new List<Core.Domain.Attribute>
            {
                new Core.Domain.Attribute{
                    Id = 1,
                    Name = "Musica"
                },
                new Core.Domain.Attribute{
                    Id = 2,
                    Name = "Ruido"
                }
            };
        }

        protected override BaseCrudController<Core.Domain.Attribute> CreateController(IRepository<Core.Domain.Attribute> crudRepository)
        {
            return new AttributesController(crudRepository);
        }

        protected override Core.Domain.Attribute NewItem()
        {
            return new Core.Domain.Attribute
            {
                Id = 1
            };
        }

        protected override Core.Domain.Attribute EditItem(int id)
        {
            return new Core.Domain.Attribute
            {
                Id = 1,
                Image = "lala",
                Name = "Ruido"
            };
        }

        protected override Core.Domain.Attribute InvalidItem()
        {
            return new Core.Domain.Attribute
            {
              
            };
        }
    }

    [TestClass]
    public class PlacesControllerTest : CrudTest<Place>
    {

        protected override List<Place> CreateListItems()
        {
            return new List<Place>
            {
                new Place{
                    Id = 1,
                    Name = "test"
                },
                new Place{
                    Id = 2,
                    Name = "test 2"
                },
                new Place{
                    Id = 3,
                    Name = "test 3"
                }
            };
        }

        protected override BaseCrudController<Place> CreateController(IRepository<Place> crudRepository)
        {
            return new PlacesController(crudRepository);
        }

        protected override Place NewItem()
        {
            return new Place
            {
                Name = "test"
            };
        }

        protected override Place EditItem(int id)
        {
            return new Place
            {
                Id  = 1,
                Name = "test"
            };
        }

        protected override Place InvalidItem()
        {
            return new Place
            {
              
            };
        }
    }
}
