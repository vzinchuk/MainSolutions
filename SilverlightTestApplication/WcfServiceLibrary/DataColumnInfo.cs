using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
    [DataContract]
    public class DataColumnInfo
    {
        [DataMember]
        public string ColumnName { get; set; }

        [DataMember]
        public string ColumnTitle { get; set; }

        [DataMember]
        public string DataTypeName { get; set; }

        [DataMember]
        public bool IsRequired { get; set; }

        [DataMember]
        public bool IsKey { get; set; }

        [DataMember]
        public bool IsReadOnly { get; set; }

        [DataMember]
        public int DisplayIndex { get; set; }

        [DataMember]
        public string EditControlType { get; set; }

        [DataMember]
        public int MaxLength { get; set; }
    }

}
