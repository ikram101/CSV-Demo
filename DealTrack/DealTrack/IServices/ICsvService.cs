using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealTrack.IServices
{
    public interface ICsvFileService
    {
       DataTable ReadCsvFile(string filePath);
    }


     

}
