using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class HttpGetData
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }
    }
}
