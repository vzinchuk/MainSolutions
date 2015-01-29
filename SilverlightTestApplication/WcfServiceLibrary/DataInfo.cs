using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
    [DataContract]
    public class DataInfo
    {
        [DataMember]
        public Dictionary<string,object> DataRow { get; set; }
    }
}
