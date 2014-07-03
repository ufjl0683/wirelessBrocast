using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controller
{
   public interface IThreadTask
    {

       void Start();
       void Abort();
       void Join();
       int PlayCnt { get;   }
    }
}
