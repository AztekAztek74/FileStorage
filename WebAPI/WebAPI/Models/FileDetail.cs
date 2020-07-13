using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class FileDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileId { get; set; }
        public string FileName { get; set; }
        //public string FilePath { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }
        public string FileSha256 { get; set; }
        public ShaPathDetail ShaPathDetail { get; set; }
    }
}


