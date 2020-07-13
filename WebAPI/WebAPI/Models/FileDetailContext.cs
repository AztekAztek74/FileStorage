using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class FileDetailContext: DbContext
    {
        public FileDetailContext(DbContextOptions<FileDetailContext> options) : base(options)
        {
        }
        public DbSet<FileDetail> FileDetails { get; set; }
        public DbSet<ShaPathDetail> ShaPathDetails { get; set; }
    }
}
