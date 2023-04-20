using Basic.Application.Function;
using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Basic.Infrastracture.Entity;

namespace Basic.Application.Service
{
    public class ImageHandlerService : IImageHandlerInterface
    {
        private readonly ApplicationDbContext _context;
        public ImageHandlerService(ApplicationDbContext context)
        {
            _context = context;
        }
        public CommonResponse UploadProfileImage(string token, UserDocReq req)
        {



            dynamic imgdetails = NormalFunctions.CloudImageUpload(req.Image);


            _context.UserDoc.Add(new Domain.Entity.UserDoc
            {
                UserName = NormalFunctions.GetUserClaimsData(token).UserName,
                ImageName = Convert.ToString(imgdetails.PublicId),
                ImageUrl = Convert.ToString(imgdetails.Uri),
                ImageDetails = NormalFunctions.RandomString(10),
                IsProfilePic = "y",
                ImageCategory = Convert.ToString("ProfilePic...!!!"),
                IsDelete = "n",
                IsActive = "y",
                CreateDate = DateTime.UtcNow,

            });
            _context.SaveChanges();
            return new CommonResponse
            {
                Code = 200,
                Message = "Successfully Updated"
            };
        }
        public CommonResponse UploadImage(string token, UserDocReq req)
        {
            Path.Combine(req.Image.FileName, NormalFunctions.RandomString(10));
            var isprofilepic = _context.UserDoc.Select(x => x.IsProfilePic == "y" && x.IsDelete != "n" && x.UserName == NormalFunctions.GetUserClaimsData(token).UserName).Count();
            if (isprofilepic == 0)
            {
                UploadProfileImage(token, req);
                return new CommonResponse
                {
                    Code = 200,
                    Message = "Successfully Updated"
                };
            }
            dynamic imgdetails = NormalFunctions.CloudImageUpload(req.Image);
            //if (req.IsProfilePic == "y")
            //{
            //    var count = _context.UserDoc.Where(x => x.IsProfilePic == "y" && x.UserName == NormalFunctions.GetUserClaimsData(token).UserName);
            //    foreach (var data in count)
            //    {
            //        data.IsActive = "n";
            //        _context.Update(data);
            //    }
            //}

            _context.UserDoc.Add(new Domain.Entity.UserDoc
            {
                UserName = NormalFunctions.GetUserClaimsData(token).UserName,
                ImageName = Convert.ToString(imgdetails.PublicId),
                ImageUrl = Convert.ToString(imgdetails.Uri),
                ImageDetails = NormalFunctions.RandomString(10),
                IsProfilePic = "n",
                ImageCategory = Convert.ToString("Gallery Photo..!!!"),
                IsDelete = "n",
                IsActive = "y",
                CreateDate = DateTime.UtcNow,

            });
            _context.SaveChanges();
            return new CommonResponse
            {
                Code = 200,
                Message = "Successfully Updated"
            };
        }

        public bool MakeProfilepic(Uri Uri, string token)
        {
            if (string.IsNullOrEmpty(Uri.ToString()))
            {
                return false;
            }
            var username = NormalFunctions.GetUserClaimsData(token).UserName;
            var ppupload = _context.UserDoc.Where(x => x.IsProfilePic == "y" && x.UserName == username).FirstOrDefault();
            if (ppupload != null)
            {
                ppupload.IsProfilePic = "n";
                //ppupload.UpdateDate = DateTime.UtcNow;
                ppupload.ImageCategory = "Gallery Photo..!!!";
                _context.UserDoc.Update(ppupload);
                _context.SaveChanges();
            }

            var data = _context.UserDoc.Where(x => x.ImageUrl == Uri.ToString() && x.UserName == username).FirstOrDefault();

            data.IsProfilePic = "y";
            // ppupload.UpdateDate = DateTime.Now;
            data.ImageCategory = "ProfilePic...!!!";
            _context.UserDoc.Update(data);
            _context.SaveChanges();
            return true;

        }
    }
}
