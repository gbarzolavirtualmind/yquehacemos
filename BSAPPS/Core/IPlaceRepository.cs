﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public interface IPlaceRepository
    {
        Place GetById(int id);
    }
}
