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
        IRepository<T> _crudRepository;
        public BaseCrudController(IRepository<T> crudRepository)
        {
            _crudRepository = crudRepository;
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View(_crudRepository.GetAll());
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            return View(_crudRepository.GetById(id));
        }

        [HttpPost]
        public ActionResult Edit(T item)
        {
            if (ModelState.IsValid)
            {
                _crudRepository.Update(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        public ActionResult Create(T newItem)
        {
            if (ModelState.IsValid)
            {
                _crudRepository.Add(newItem);
                return RedirectToAction("Index");
            }
            return View(newItem);

        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            _crudRepository.Delete(id);
            return Json(true);
        }
    }

    public class QuestionsController : BaseCrudController<Question>
    {
        public QuestionsController(IRepository<Question> repository)
        :base(repository)
        {
            
        }

        public QuestionsController()
        :this(new TestRepo<Question>(
            (x,y) => x.Id = y,
            (x,y) => { x.Content = y.Content;},
            (x) => x.Id
            ))
        { 
            
        }

        

        

    }

    public class PlacesController : BaseCrudController<Place> {
       public PlacesController(IRepository<Place> repository)
        :base(repository)
        {
            
        }

       public PlacesController()
        :this(new TestRepo<Place>(
            (x,y) => x.Id = y,
            (x, y) => { x.Address = y.Address; x.Description = y.Description; x.Name = y.Name; },
            (x) => x.Id
            ))
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
}
