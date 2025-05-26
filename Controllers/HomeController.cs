using Microsoft.AspNetCore.Mvc;
using NetMutillidae.Models;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetMutillidae.Controllers;

public class HomeController : Controller
{

    public HomeController()
    {
    }

    /// <summary>
    /// Metodo vulnerabile a XSS-Injection
    /// Attraverso la manipolazione di onerror in img o del parametro userName
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public IActionResult Index(string userName)
    {
        var model = new HomeViewModel
        {
            UserName = userName
        };
        return View(model);
    }

    /// <summary>
    /// Metodo vulnerabile ad attacchi di tipo "path traversal (CWE-22)
    /// Creando il file richiesto nel modo descritto è possibile risalire le cartelle e ottenere un file riservato (come ad esempio appsettings.json)
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public IActionResult GetFile(string fileName)
    {

        //Exploit payload: fileName = "../appsettings.json"

        string basePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        string fullPath = Path.Combine(basePath, fileName);
        if (System.IO.File.Exists(fullPath))
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, "application/octet-stream", fileName);
        }
        return NotFound();
    }

}
