using System;
using System.Collections.Generic;
using System.Text;

namespace Queue.Model
{
    public class Emails
    {
        public string To { get; set; }
        public string Subject { get; set; }

        public string Content { get; set; }

        public byte[][] attachmentDatas { get; set; }

        public (byte[] data, string fileNameWithExtension)[] fileAttachments { get; set; }
    }
}
