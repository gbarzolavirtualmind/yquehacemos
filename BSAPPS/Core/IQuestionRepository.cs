using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IQuestionRepository : IRepository<Question>
    {
        

    }

    public interface IRepository<T> where T: class
    {
        List<T> GetAll();
        void Add(T newQuestion);
        void Update(T question);
        void Delete(int id);
        T GetById(int id);
    }
}
