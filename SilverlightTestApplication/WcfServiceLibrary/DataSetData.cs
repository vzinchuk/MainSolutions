using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
    [DataContract]
    public class DataSetData
    {
        [DataMember]
        public ObservableCollection<DataTableInfo> Tables { get; set; }
        [DataMember]
        public string DataXML { get; set; }

        public static DataSetData FromDataSet(DataSet ds)
        {
            DataSetData dsd = new DataSetData();
            dsd.Tables = new ObservableCollection<DataTableInfo>();
            foreach (DataTable t in ds.Tables)
            {
                DataTableInfo tableInfo = new DataTableInfo { TableName = t.TableName };
                dsd.Tables.Add(tableInfo);
                tableInfo.Columns = new ObservableCollection<DataColumnInfo>();
                foreach (DataColumn c in t.Columns)
                {
                    DataColumnInfo col = new DataColumnInfo { ColumnName = c.ColumnName, ColumnTitle = c.ColumnName, DataTypeName = c.DataType.FullName, MaxLength = c.MaxLength, IsKey = c.Unique, IsReadOnly = (c.Unique || c.ReadOnly), IsRequired = !c.AllowDBNull };
                    if (c.DataType == typeof(System.Guid))
                    {
                        col.IsReadOnly = true;
                        col.DisplayIndex = -1;
                    }                    
                    tableInfo.Columns.Add(col);
                }
                tableInfo.Rows = new ObservableCollection<DataInfo>();
                foreach (DataRow row in t.Rows)
                {
                    DataInfo infoRow = new DataInfo();
                    infoRow.DataRow = new Dictionary<string, object>();
                    foreach (DataColumnInfo info in tableInfo.Columns)
                    {
                        if (info.ColumnName == "TypeName")
                            tableInfo.TableName = row["TypeName"].ToString();                        
                    }

                    if (!DBNull.Value.Equals(row["AttributeDisplayName"]))
                    {
                        if (!String.IsNullOrEmpty(row["AttributeDisplayName"].ToString()))
                        {
                            //if (info.ColumnName == "AttributeName")
                            infoRow.DataRow.Add("ClassName", row["ClassName"]);
                            //if (info.ColumnName == "ClassDisplayName")
                            infoRow.DataRow.Add("AttributeName", row["AttributeName"]);

                            tableInfo.Rows.Add(infoRow);
                        }
                    }
                   
                }
                
            }
            dsd.DataXML = ds.GetXml();
            return dsd;
        }
    }





}
