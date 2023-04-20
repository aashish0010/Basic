using Basic.Domain.Entity;

namespace Basic.Domain.Interface
{
    public interface IImageHandlerInterface
    {
        CommonResponse UploadImage(string token, UserDocReq req);
        CommonResponse UploadProfileImage(string token, UserDocReq req);
        bool MakeProfilepic(Uri url, string token);
    }
}
