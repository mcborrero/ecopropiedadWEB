using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EcopropiedadWeb.Controllers
{
    public class GetFileController : ApiController
    {
		[HttpPost]
		public async Task<HttpResponseMessage> AddFile()
		{
			//var guiaid = db.GUIA_LOGISTIC.Where(q => q.tras_id == transid).Max(q => q.guia_id);

			if (!Request.Content.IsMimeMultipartContent())
			{
				this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
			}

			string root = HttpContext.Current.Server.MapPath("~/guiaspdf");
			//string root = "C:/root/JuntaFiles/";
			var provider = new MultipartFormDataStreamProvider(root);
			var result = await Request.Content.ReadAsMultipartAsync(provider);

			// On upload, files are given a generic name like "BodyPart_26d6abe1-3ae1-416a-9429-b35f15e6e5d5"
			// so this is how you can get the original file name
			var originalFileName = GetDeserializedFileName(result.FileData.First());

			//var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);
			string path = result.FileData.First().LocalFileName;
			Char delimiter = '_';
			String[] substrings = path.Split(delimiter);
			string newname = substrings[1];
			//guardar la imagen
			//var id = entity.docu_id;

			//Do whatever you want to do with your file here
			//var changname = db.cat_documentacion_reafiliacion.Where(q => q.docu_id == id).ToList();
			string originalname = originalFileName;
			string serialname = newname;
			int num = 0;
			try
			{

				File.Move(root + "/" + "BodyPart_" + serialname, root + "/" + originalname);
				System.IO.File.Delete(root + "/" + "BodyPart_" + serialname);

				//var entity = new SCAN_LOGISTIC
				//{
				//	guia_id = guiaid,
				//	guia_filename = "guaspdf/" + originalname

				//};

				//db.SCAN_LOGISTIC.Add(entity);
				//db.SaveChanges();

				//
				//int check = db.REGISTRO_PAGO.Max(q => q.regi_id);
				//var stud = (from s in db.REGISTRO_PAGO
				//			where s.regi_id == check
				//			select s).FirstOrDefault();
				////3 cancelado
				//stud.regi_unage_url = "http://189.236.99.252:8082/ms/guiaspdf/" + originalname;

				//num = db.SaveChanges();


				return this.Request.CreateResponse(HttpStatusCode.OK, "ok");
			}
			catch (Exception e)
			{
				return this.Request.CreateResponse(HttpStatusCode.OK, "nook");
			}

			//return this.Request.CreateResponse(HttpStatusCode.OK, "ok");
		}

		private string GetDeserializedFileName(MultipartFileData fileData)
		{
			var fileName = GetFileName(fileData);
			return JsonConvert.DeserializeObject(fileName).ToString();
			//return JsonConvert.SerializeObject(fileName).ToString();
		}

		public string GetFileName(MultipartFileData fileData)
		{
			return fileData.Headers.ContentDisposition.FileName;
		}
	}
}
