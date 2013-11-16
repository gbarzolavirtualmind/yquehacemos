using Core;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Backend.Controllers
{
    public abstract class BaseCrudController<T> : Controller where T: class
    {
        private IUnitOfWork _unitOfWork;
        public BaseCrudController(IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View(_unitOfWork.Repository<T>().GetAll());
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            return View(_unitOfWork.Repository<T>().GetById(id));
        }

        [HttpPost]
        public ActionResult Edit(T item)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Repository<T>().Update(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        public ActionResult Create(T newItem)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Repository<T>().Add(newItem);
                return RedirectToAction("Index");
            }
            return View(newItem);

        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            _unitOfWork.Repository<T>().Delete(id);
            return Json(true);
        }
    }

    public class QuestionsController : BaseCrudController<Question>
    {
        public QuestionsController(IUnitOfWork UnitOfWork)
        :base(UnitOfWork)
        {
            
        }

        public QuestionsController()
        :this(new TestUnitOfWork())
        { 
            
        }
    }

    public class PlacesController : BaseCrudController<Place>
    {
        public PlacesController(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public PlacesController()
            : this(new TestUnitOfWork())
        {

        }



    }

    public class TestRepo<T> : IRepository<T> where T: class
    {
        private static  readonly List<T> _items = new List<T>();
        private static int idNext = 0;
        private Action<T, int> _updateId;
        private Action<T, T> _updateItem;
        private Func<T, int> _getIdProperty;
        public TestRepo(Action<T,int> updateId, Action<T,T> update, Func<T,int> getIdProperty)
        {
            _updateId = updateId;
            _updateItem = update;
            _getIdProperty = getIdProperty;
        }

        public List<T> GetAll()
        {
            return _items;
        }

        public void Add(T newItem)
        {
            idNext++;
            _updateId(newItem,idNext);
            _items.Add(newItem);
        }

        public void Update(T item)
        {
            int id = _getIdProperty(item);
            T q = this.GetById(id);
            _updateItem(q, item);
        }

        public void Delete(int id)
        {
            T q = this.GetById(id);
            _items.Remove(q);
        }

        public T GetById(int id)
        {
            return _items.FirstOrDefault(x => _getIdProperty(x) == id);
        }
    }

    public class TestUnitOfWork : IUnitOfWork
    {
        private static Dictionary<Type,object> _repositories = new Dictionary<Type,object>();

        private object CreateRepository(Type type)
        {
            if(type == typeof(Question))
            {
                return new TestRepo<Question>((x,y) => x.Id =y,(x,y) => x.Content = y.Content,(x) => x.Id);
            }
            if(type == typeof(Core.Domain.Attribute))
            {
                return new TestRepo<Core.Domain.Attribute>((x,y) => x.Id =y,(x,y) => {x.Name = y.Name; x.Image = y.Image;},(x) => x.Id);
            }
            if(type == typeof(Place))
            {
                return new TestRepo<Place>((x,y) => x.Id =y,(x,y) => {x.Name = y.Name; x.Address = y.Address; x.Description = y.Description;},(x) => x.Id);
            }
            return null;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if(!_repositories.ContainsKey(type))
            {
                _repositories[type] = CreateRepository(type);
            }
            return _repositories[type] as IRepository<T>;
        }

        public void Commit()
        {
            
        }
    }

}
