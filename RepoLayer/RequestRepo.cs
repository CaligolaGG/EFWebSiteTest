using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace RepoLayer
{
    /// <summary>
    /// Class to interact with the InfoRequest table in the db
    /// </summary>
    public class RequestRepo
    {
        private MyDbContext _ctx;

        public RequestRepo(MyDbContext ctx)
        {
            _ctx = ctx;
        }
        public IQueryable<InfoRequest> GetAll() => _ctx.InfoRequests;
        public int GetNumber() => _ctx.InfoRequests.Count();


    }
}

