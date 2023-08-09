using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.DataAccess.Entities
{
    public class ImageModel
    {
        public Guid ImageId { get; set; }
        public Guid UserId { get; set; }
        public byte[] ImageBytes { get; set; } = Array.Empty<byte>();
    }
}
