using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IOrder
    {
        NotetiModel CreateOrder(CheckOutRequest input);

        List<OrderModel.Output.GetOrder> GetAll();

        List<OrderModel.Output.GetOrder> GetById(OrderModel.Input.GetOrder request);

        NotetiModel DeleteOrder(OrderModel.Input.DeteleOrder request);
    }
}