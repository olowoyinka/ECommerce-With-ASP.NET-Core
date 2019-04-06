using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWeb.Respository
{
    public interface ICartRespository
    {
        bool IsExistingCheck(int? Id);
    }
}
