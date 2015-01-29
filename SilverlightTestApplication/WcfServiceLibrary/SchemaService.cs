using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary
{   
    public class SchemaService : ISchemaService
    {
        public DataSetData GetDataUsingDataContract(List<string> guids)
        {
            DataSet set = ObjectLoader.GetDataSet(guids);
            DataSetData composite = DataSetData.FromDataSet(set);
            return composite;
        }


        public List<PairRelation> GetRelationUsingDataContract(List<string> guids)
        {
            return ObjectLoader.GetDataRelation(guids);
        }


        public List<PairRelation> LoadTypes()
        {
            return ObjectLoader.LoadTypes();
        }
    }
}
