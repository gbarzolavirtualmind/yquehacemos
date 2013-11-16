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
        public AttributesController(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public AttributesController()
            : this(new TestUnitOfWork())
        {


        }


    }
}
