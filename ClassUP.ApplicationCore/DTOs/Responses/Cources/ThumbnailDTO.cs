using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.Cources
{
    public class ThumbnailDTO
    {
        public Stream FileStream { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public long FileSize { get; set; }
    }
}
