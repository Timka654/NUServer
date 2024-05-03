using NU.Core.Models;

namespace NUServer.Shared.DB
{
    public class ResourceModel : IndexResourceModel
    {
        public int Id { get; set; }

        public bool Active { get; set; }
    }
}
