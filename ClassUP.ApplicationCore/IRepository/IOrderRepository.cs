using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.IRepository
{
     public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order?> GetByIdWithItemsAsync(int id);
    }
}
