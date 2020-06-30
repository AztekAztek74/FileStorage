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

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDetailController : ControllerBase
    {
        private readonly FileDetailContext _context;

        public FileDetailController(FileDetailContext context)
        {
            _context = context;
        }

        // GET: api/FileDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileDetail>>> GetFileDetails()
        {
            var listDB = await _context.FileDetails.ToListAsync();
            var listReady = listDB.FindAll(item => item.User == Environment.UserDomainName);
            return listReady;
        }

        // GET: api/FileDetail/5
        [HttpGet("{id}")]
        public async Task <ActionResult<IList<FileDetail>>> GetFileDetail(int id)
        {
            var fileDetail = await _context.FileDetails.FindAsync(id);

            var memory = new MemoryStream();
            using (var stream = new FileStream(fileDetail.FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, MimeUtility.GetMimeMapping(fileDetail.FileName), fileDetail.FileName);

        }


        //System.Net.Http.Headers.

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
            
            await _context.SaveChangesAsync();
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if(file.Length>0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Value.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    //var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var newRecord = new FileDetail { FileName = fileName, FilePath = fullPath, User = Environment.UserDomainName, Date = DateTime.Now};
                    _context.FileDetails.Add(newRecord);
                    await _context.SaveChangesAsync();
                    return Ok();
                    //return CreatedAtAction("GetFileDetail", new { id = fileDetail.FileId, FileName = fileName, FilePath = fullPath, fileDetail.User, Date = DateTime.Now });
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

            System.IO.File.Delete(fileDetail.FilePath);

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
