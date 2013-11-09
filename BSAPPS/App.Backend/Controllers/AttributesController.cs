using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Backend.Controllers
{
    public class AttributesController : BaseCrudController<Core.Domain.Attribute>
    {
        public AttributesController(IRepository<Core.Domain.Attribute> repo)
        :base(repo){
        
        }

        public AttributesController()
        :this(new TestRepo<Core.Domain.Attribute>(
            (x,y) => x.Id = y,
            (x, y) => { x.Image = y.Image; x.Name = y.Name; },
            (x) => x.Id
            ))
        {
 
        
        }

        
    }
}
