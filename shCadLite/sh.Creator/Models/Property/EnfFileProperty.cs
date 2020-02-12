using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.Models.Property
{
    class EnfFileProperty:ProperyModel
    {
        public EnfFileProperty(FileInfo file)
        {
            Title = file.FullName;




        }
    }
}
