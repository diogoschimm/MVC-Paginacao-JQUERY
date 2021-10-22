using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppPagination.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [HttpPost]
        public ActionResult AtualizarRegistro(Document documentDto)
        {
            var document = Db.Documents.FirstOrDefault(d => d.DocumentId == documentDto.DocumentId);
            if (document != null)
                document.DocumentTypeId = document.DocumentTypeId;

            return Json(document);
        }

        public ActionResult Dados(int? page = 1)
        {
            if (page <= 0)
                return JsonGet(new Data { Success = false, Message = "Informe uma pagina maior ou igual a 1" });

            var count = Db.Documents.Count(d => d.DocumentTypeId == 0);

            if (page > count)
                return JsonGet(new Data { Success = false, Message = $"Informe uma pagina menor que {count}" });

            var documents = Db.Documents.Where(d => d.DocumentTypeId == 0).Skip(page.Value - 1).Take(1);
            var dados = new Data
            {
                Success = true,
                Message = "Sucesso",
                Document = documents.FirstOrDefault(),
                Total = count,
                Page = page.Value
            };
            return JsonGet(dados);
        }
        public ActionResult Lista()
        {
            return JsonGet(Db.Documents);
        }

        private ActionResult JsonGet(object dados) => Json(dados, JsonRequestBehavior.AllowGet);

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class Data
    {
        public Document Document { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class Document
    {
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string UrlDocument { get; set; }
        public int DocumentTypeId { get; set; }
    }
    public class Db
    {
        public static List<Document> Documents
        {
            get
            {
                var documents = new List<Document>();
                for (int i = 1; i <= 15; i++)
                {
                    documents.Add(new Document
                    {
                        DocumentId = i,
                        DocumentName = $"Document{i}",
                        UrlDocument = $"https://localhost/Documents/document{i}.pdf",
                        DocumentTypeId = 0
                    });
                };
                return documents;
            }
        }
    }
}