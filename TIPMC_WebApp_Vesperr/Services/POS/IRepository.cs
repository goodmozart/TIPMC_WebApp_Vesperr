using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TIPMC_WebApp_Vesperr.Services.POS
{
    public interface IRepository
    {
        string GeneratePONumber();
        string GenerateGRNumber();
        string GenerateSONumber();
        string GenerateInvenTranNumber();
    }
}
