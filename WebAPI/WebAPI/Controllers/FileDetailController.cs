using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using WebAPI.Models;
using MimeMapping;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDetailController : ControllerBase
    {
        public IHttpContextAccessor _httpContextAccessor { get; set; }
        private readonly FileDetailContext _context;

        public FileDetailController(FileDetailContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        // GET: api/FileDetail
        [HttpGet]
        public IEnumerable<HttpGetData> GetFileDetails()
        {
            var currentUser = Environment.UserName;
            if (_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                currentUser = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            List<HttpGetData> Data = (
                from item in _context.FileDetails
                join inner in _context.ShaPathDetails on item.FileSha256 equals inner.FileSha256
                where item.User == currentUser && item.FileSha256 == inner.FileSha256
                select new HttpGetData()
                {
                    FileId = item.FileId,
                    FileName = item.FileName,
                    User = item.User,
                    Date = item.Date
                }).ToList();
            return Data;
            //return _context.FileDetails.Where(item => item.User == Environment.UserName).ToList();
        }

        // GET: api/FileDetail/5
        [HttpGet("{id}")]
        public async Task <ActionResult<IList<FileDetail>>> GetFileDetail(int id)
        {
            var fileDetail = await _context.FileDetails.FindAsync(id);
            var path = _context.ShaPathDetails.Where(item => fileDetail.FileSha256 == item.FileSha256).Select(inner => inner.FilePath).FirstOrDefault();


            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, MimeUtility.GetMimeMapping(fileDetail.FileName), fileDetail.FileName);

        }

        // PUT: api/FileDetail/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFileDetail(int id, FileDetail fileDetail)
        {
            if (id != fileDetail.FileId)
            {
                return BadRequest();
            }

            _context.Entry(fileDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FileDetail
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<FileDetail>> PostFileDetail()
        {
            try
            {
                var currentUser = Environment.UserName;
                if (_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    currentUser = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                }
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if(file.Length>0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Value.Trim('"');
                    var fullPath = Path.Combine(pathToSave, Path.GetRandomFileName());

                    var temporaryPath = "";

                    byte[] shaFile;

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                                             
                        await file.CopyToAsync(stream);
                    }

                    using(FileStream stream = System.IO.File.OpenRead(fullPath))
                    {
                        var sha = new SHA256Managed();
                        shaFile = sha.ComputeHash(stream);
                    }

                    temporaryPath = (from item in _context.FileDetails
                                     join inner in _context.ShaPathDetails on item.FileSha256 equals inner.FileSha256
                                     where item.FileSha256 == BitConverter.ToString(shaFile) && item.FileSha256 == inner.FileSha256
                                     select inner.FilePath ).FirstOrDefault();

                    if (!String.IsNullOrEmpty(temporaryPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    else
                    {
                        var newRecordPath = new ShaPathDetail { FileSha256 = BitConverter.ToString(shaFile), FilePath = fullPath };
                        _context.ShaPathDetails.Add(newRecordPath);
                    }

                    var newRecordFile = new FileDetail { FileName = fileName, User = currentUser, Date = DateTime.Now, FileSha256  = BitConverter.ToString(shaFile) };
                    
                    _context.FileDetails.Add(newRecordFile);
                    
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
            

        }

        // DELETE: api/FileDetail/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FileDetail>> DeleteFileDetail(int id)
        {
            var fileDetail = await _context.FileDetails.FindAsync(id);

            if (fileDetail == null)
            {
                return NotFound();
            }

            if (_context.FileDetails.Count(item =>  item.FileSha256 == fileDetail.FileSha256)==1)
            {
                var path = _context.ShaPathDetails.Where(item => fileDetail.FileSha256 == item.FileSha256).Select(inner => inner.FilePath).FirstOrDefault();
                var sha = _context.ShaPathDetails.Where(item => fileDetail.FileSha256 == item.FileSha256).Select(inner => inner.FileSha256).FirstOrDefault();
                var shaPathDetail = await _context.ShaPathDetails.FindAsync(sha);
                System.IO.File.Delete(path);
                _context.ShaPathDetails.Remove(shaPathDetail);
            }

            _context.FileDetails.Remove(fileDetail);
            await _context.SaveChangesAsync();

            return fileDetail;
        }

        private bool FileDetailExists(int id)
        {
            return _context.FileDetails.Any(e => e.FileId == id);
        }
    }
}
