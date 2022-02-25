using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer
{
    /// <summary>
    /// Class to interact with the InfoRequest table in the db
    /// </summary>
    public class InfoRequestRepository : IInfoRequestRepository
    {
        private MyDbContext _ctx;

        public InfoRequestRepository(MyDbContext ctx)
        {
            _ctx = ctx;
        }
        public IQueryable<InfoRequest> GetAll() => _ctx.InfoRequests;
        public int GetNumber() => _ctx.InfoRequests.Count();


    }
}

