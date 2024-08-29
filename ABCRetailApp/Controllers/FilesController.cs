using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ABCRetailApp.Services;
using ABCRetailApp.Models;
using System.Threading.Tasks;

public class FilesController : Controller
{
    private readonly FileStorageService _fileStorageService;

    public FilesController(FileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            using (var stream = file.OpenReadStream())
            {
                await _fileStorageService.UploadFileAsync(file.FileName, stream);
            }
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Download(string fileName)
    {
        var fileStream = await _fileStorageService.DownloadFileAsync(fileName);
        return File(fileStream, "application/octet-stream", fileName);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var files = await _fileStorageService.ListFilesAsync();
        return View(files);
    }
}
