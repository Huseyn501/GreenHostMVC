using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GreenHost.Helpers.Extentions
{
    public static class CreatingFileExtention
    {
        public static string CreatingFile(this IFormFile formFile,string root,string folderName)
        {
            string fileName = "";
            if (formFile.FileName.Length > 100)
            {
                fileName = Guid.NewGuid() + formFile.FileName.Substring(formFile.FileName.Length - 64);
            }
            else
            {
                fileName = Guid.NewGuid() + formFile.FileName;
            }
            string path = Path.Combine(root, folderName, fileName);
            using (FileStream stream = new FileStream(path,FileMode.Create))
            {
                formFile.CopyTo(stream);
            }
            return fileName;
        }
        public static void DeletingFile(this string fileName, string root, string folderName)
        {
            string path = Path.Combine(root, folderName, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
