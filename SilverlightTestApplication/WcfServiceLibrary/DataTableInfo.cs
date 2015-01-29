using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
    [DataContract]
    public class DataTableInfo
    {
        [DataMember]
        public string TableName { get; set; }

        [DataMember]
        public ObservableCollection<DataColumnInfo> Columns { get; set; }

        [DataMember]
        public ObservableCollection<DataInfo> Rows { get; set; }
    }
}
