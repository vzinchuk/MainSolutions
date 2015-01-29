using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
      [DataContract]
    public class PairRelation
    {
          [DataMember]
          public string Key1 { get; set; }
          [DataMember]
          public string Key2 { get; set; }
    }
}
