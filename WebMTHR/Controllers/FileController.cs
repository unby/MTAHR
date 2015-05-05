using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BaseType;
using Microsoft.AspNet.Identity;
using WebMTHR.Models;

namespace WebMTHR.Controllers
{
    public class FileController : Controller
    {
        ApplicationDbContext _dbContext = WebDbHelper.GetDbContext();
        // GET: File
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var idFile = Guid.Parse(Request.QueryString["idFile"]);
            if (idFile != Guid.Empty)
            {
                var file =await _dbContext.WorkFiles.FirstAsync(f => f.FileId == idFile);
                return File(file.GetByte(WebDbHelper.MthrConnectionString), System.Net.Mime.MediaTypeNames.Application.Octet, file.FileName);
            }
            return new HttpStatusCodeResult(404);
        }
    }
}