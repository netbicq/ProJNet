﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProJ.Model;
using ProJ.Model.DB;

namespace ProJ.IBll
{
    public interface IOwner
    {

        ActionResult<bool> ApplyOwner(Model.Para.OwnerNew owner);

        ActionResult<bool> AddOwner(Model.Para.OwnerEdit owner);

        ActionResult<bool> DelOwner(Guid id);

        ActionResult<Model.DB.Basic_Owner> OwnerSelector();

        ActionResult<Pager<Model.DB.Basic_Owner>> GetOwnerList(PagerQuery<string> para);


    }
}
