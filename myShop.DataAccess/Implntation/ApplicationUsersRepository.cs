using myShop.Entity.Models;
using myShop.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DataAccess.Implntation
{
    public class ApplicationUsersRepository : GenericRepository<ApplicationUsers> , IApplicationUsersRepository
    {
        private readonly AppDbContext _context;

        public ApplicationUsersRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
