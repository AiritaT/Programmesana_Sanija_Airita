using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Programmesana_Sanija_Airita.Models;
namespace Programmesana_Sanija_Airita.Controllers.DataAccess
{
    public class FilesRepository: ConnectionClass
    {
        public FilesRepository() : base()
        {}
        public IQueryable<File> GetFiles()
        {
            return Entity.Files;
        }
        public void AddFiles (File f)
        {
            f.id = Guid.NewGuid();
            Entity.Files.Add(f);
            Entity.SaveChanges();
        }
        public void AddUpload(Upload u)
        {
            Entity.Uploads.Add(u);
            Entity.SaveChanges();
        }
        public void DeleteFile(File f)
        {
            Entity.Files.Remove(f);
            Entity.SaveChanges();
        }
        public IQueryable<Upload> GetUploadForFiles(Guid id)
        {
            return Entity.Uploads.Where(x => x.File_id == id);
        }

        public Upload GetUpload(int id)
        {
            return Entity.Uploads.SingleOrDefault(x => x.id == id);
        }
        public void DeleteUpload(Upload u)
        {
            Entity.Uploads.Remove(u);
            Entity.SaveChanges();
        }
       
    }
}