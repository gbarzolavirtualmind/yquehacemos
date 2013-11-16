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
        private Moq.Mock<IUnitOfWork> _unitOfWork;
        private Moq.Mock<IRepository<T>> _crudRepository;
        private BaseCrudController<T> _sut;
        private T _editItem;
        private T _invalidItem;
        protected abstract List<T> CreateListItems();
        protected abstract BaseCrudController<T> CreateController(Moq.Mock<IUnitOfWork> unitOfWork);
        protected abstract T NewItem();
        protected abstract T EditItem(int id);
        protected abstract T InvalidItem();

        [TestInitialize]
        public void Setup()
        {
            _editItem = EditItem(1);
            _invalidItem = InvalidItem();

            _crudRepository = new Moq.Mock<IRepository<T>>();

            _unitOfWork = new Moq.Mock<IUnitOfWork>();
            _unitOfWork.Setup(x => x.Repository<T>()).Returns(_crudRepository.Object);
            _unitOfWork.Setup(x => x.Repository<T>().GetAll()).Returns(CreateListItems());
            _unitOfWork.Setup(x => x.Repository<T>().Add(NewItem()));
            _unitOfWork.Setup(x => x.Repository<T>().Update(_editItem));
            _unitOfWork.Setup(x => x.Repository<T>().Delete(1));
            _unitOfWork.Setup(x => x.Repository<T>().GetById(1)).Returns(_editItem);
            

            _sut = CreateController(_unitOfWork);
                
        }

        [TestMethod]
        public void Test_When_List_Items_Then_Load_And_Return_View()
        {
            var view = _sut.Index() as ViewResult;
            var model = view.Model as List<T>;

            Assert.AreEqual(3, model.Count);

            _unitOfWork.Verify(x => x.Repository<T>().GetAll());
        }

        [TestMethod]
        public void Test_When_Create_Item_Return_View()
        {
            var view = _sut.Create();
            Assert.IsInstanceOfType(view, typeof(ViewResult));

        }

        [TestMethod]
        public void Test_When_Create_Item_Success_Return_To_Index()
        {
            var newItem = NewItem();
            var view = _sut.Create(newItem) as RedirectToRouteResult;

            Assert.AreEqual<string>("Index", view.RouteValues["action"].ToString());
            _unitOfWork.Verify(x => x.Repository<T>().Add(newItem));
        }

        [TestMethod]
        public void Test_When_Create_Invalid_Item_Return_To_Create_View()
        {
            var invalidItem = InvalidItem();
            _sut.ModelState.AddModelError("*", "mock error message");

            var view = _sut.Create(invalidItem) as ViewResult;
            Assert.AreEqual(invalidItem, view.Model);
        }

        [TestMethod]
        public void Test_When_Edit_Item_Return_View()
        {
            var view = _sut.Edit(1) as ViewResult;
            Assert.AreSame(_editItem, view.Model);

            _unitOfWork.Verify(x => x.Repository<T>().GetById(1));
        }

        [TestMethod]
        public void Test_When_Edit_Item_Success_Return_To_Index()
        {

            var view = _sut.Edit(_editItem) as RedirectToRouteResult;

            Assert.AreEqual<string>("Index", view.RouteValues["action"].ToString());
            _unitOfWork.Verify(x => x.Repository<T>().Update(_editItem));
            
        }

        [TestMethod]
        public void Test_When_Edit_Invalid_Item_Return_To_View()
        {

            _sut.ModelState.AddModelError("*", "mock error message");

            var view = _sut.Edit(_invalidItem) as ViewResult;
            Assert.AreSame(_invalidItem, view.Model);
        }

        [TestMethod]
        public void Test_When_Delete_Item_Success_Return_Json()
        {
            var view = _sut.Delete(1);

            Assert.IsInstanceOfType(view, typeof(JsonResult));
            _unitOfWork.Verify(x => x.Repository<T>().Delete(1));

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

        protected override BaseCrudController<Question> CreateController(Moq.Mock<IUnitOfWork> unitOfWork)
        {
            return new QuestionsController(unitOfWork.Object);
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
    public class AttributesControllerTest : CrudTest<Core.Domain.Attribute>
    {

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
                },
                new Core.Domain.Attribute{
                    Id = 3,
                    Name = "Algo"
                }
            };
        }

        protected override BaseCrudController<Core.Domain.Attribute> CreateController(Moq.Mock<IUnitOfWork> unitOfWork)
        {
            return new AttributesController(unitOfWork.Object);
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

        protected override BaseCrudController<Place> CreateController(Moq.Mock<IUnitOfWork> unitOfWork)
        {
            return new PlacesController(unitOfWork.Object);
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
                Id = 1,
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
