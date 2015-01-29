using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



//https://
//social.msdn.microsoft.com/Forums/silverlight/en-US/75432eff-0227-4154-b232-15a5eec69461/how-to-display-dataset-in-silverlight-datagrid?forum=silverlightnet
namespace WcfServiceLibrary
{
    public class ObjectLoader
    {
        public static DataSet GetDataSet(List<string> guids)
        {
            string connectionString = GetConnectionString();
            DataSet dataSet = new DataSet("m42production");
            using (SqlConnection connection =
                       new SqlConnection(connectionString))
            {               
                SqlDataAdapter adapter = new SqlDataAdapter();
                connection.Open();             
                SqlCommand command = new SqlCommand(

                            String.Format(@"
							declare @typeId uniqueidentifier, @typeName varchar(250)

							declare typeTables cursor for 
							select top 10 ID, Name from SchemaObjectType where IsSchemaObject = 0 and ID in ('{0}')
							open typeTables
							fetch next from typeTables into @typeId, @typeName
							while(@@FETCH_STATUS = 0)
							begin
														
								select tp.Name as TypeName, cls.ID as ClassID, cls.Name as ClassName, cls.DisplayName as ClassDisplayName,attr.Name as AttributeName,  attr.DisplayName as AttributeDisplayName, attr.PrimaryKey,  cls.ClassType 
								from SchemaObjectType  as tp
								join SchemaObjectTypeToClass rel on rel.RelatedSchemaObjectType = tp.ID
								join SchemaObjectClass cls on cls.ID = rel.RelatedSchemaObjectClass
								join SchemaObjectAttribute attr on attr.BelongsToSchemaObjectClass = cls.ID
								where tp.ID = @typeId
								--and cls.ClassType = 3
								Order By tp.Name, cls.ID asc, attr.PrimaryKey desc
								fetch next from typeTables into @typeId, @typeName
							end

							close typeTables
							deallocate typeTables", String.Join("','", guids)),
                    connection);
                command.CommandType = CommandType.Text;                               
                adapter.SelectCommand = command;                            
               
                adapter.Fill(dataSet);               
                connection.Close();              
                       
            }
            return dataSet;
        }

        static private string GetConnectionString()
        {
            // To avoid storing the connection string in your code,  
            // you can retrieve it from a configuration file. 
            return "Data Source=10.6.101.113;Initial Catalog=m42production;"
                + "User Id=sa; password=matrix42";
        }


        public static List<PairRelation> GetDataRelation(List<string> guids)
        {
            string connectionString = GetConnectionString();          
            DataSet dataSet = new DataSet("m42production");
            using (SqlConnection connection =
                       new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                connection.Open();
                SqlCommand command = new SqlCommand(
                            String.Format(@"
                            ;with relations 
							as
							(
							select rel.RelatedSchemaObjectClass as ID, cls.Name as ClassName, otp.Name as TypeName, atr.Name as AttributeName, atr.ID as AttributeID from SchemaObjectType as otp
							join SchemaObjectTypeToClass rel on rel.RelatedSchemaObjectType = otp.ID
							join SchemaObjectClass cls on cls.ID = rel.RelatedSchemaObjectClass
							join SchemaObjectAttribute atr on atr.BelongsToSchemaObjectClass = cls.ID
							where otp.ID in (select top 10 ID from SchemaObjectType where IsSchemaObject = 0 and ID in ('{0}'))
							and atr.IsSchemaObject > 0 
							)

							select distinct atr1.TypeName as Key1, atr2.TypeName as Key2 from SchemaObjectRelation as rel
							join relations atr1 on rel.RightAttribute =atr1.AttributeID 
							join relations atr2 on rel.LeftAttribute = atr2.AttributeID
							where atr1.TypeName <> atr2.TypeName", String.Join("','", guids)),

                    connection);
                command.CommandType = CommandType.Text;
                adapter.SelectCommand = command;

                adapter.Fill(dataSet);
                connection.Close();

            }
            DataTable table = dataSet.Tables[0];
            List<PairRelation> pairCollection = new List<PairRelation>();
            foreach (DataRow row in table.Rows)
            {
                pairCollection.Add(new PairRelation() { Key1 = row["Key1"].ToString(), Key2 = row["Key2"].ToString() });
            }
            return pairCollection;
        }

        public static List<PairRelation> LoadTypes()
        {
            string connectionString = GetConnectionString();
            DataSet dataSet = new DataSet("m42production");
            using (SqlConnection connection =
                       new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                connection.Open();
                SqlCommand command = new SqlCommand(String.Format(@"select ID, Name from SchemaObjectType where IsSchemaObject = 0 order by Name"),connection);
                command.CommandType = CommandType.Text;
                adapter.SelectCommand = command;

                adapter.Fill(dataSet);
                connection.Close();

            }
            DataTable table = dataSet.Tables[0];
            List<PairRelation> pairCollection = new List<PairRelation>();
            foreach (DataRow row in table.Rows)
            {
                pairCollection.Add(new PairRelation() { Key1 = row["ID"].ToString(), Key2 = row["Name"].ToString() });
            }
            return pairCollection;
        }
    }
}
