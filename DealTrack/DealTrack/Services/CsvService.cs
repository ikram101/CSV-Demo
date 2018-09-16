using DealTrack.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DealTrack.Services
{
    public class CsvFileService : ICsvFileService
    {

        public DataTable ReadCsvFile(string filePath)
        {
            DataTable dt = new DataTable();
            return dt = DealTrackBLL.FileHelper.ConvertCSVtoDataTable(filePath);
        }
    }
}