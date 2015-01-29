﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary
{    
    [ServiceContract]
    public interface ISchemaService
    {    
        [OperationContract]
        DataSetData GetDataUsingDataContract(List<string> guids);

        [OperationContract]
        List<PairRelation> GetRelationUsingDataContract(List<string> guids);


        [OperationContract]
        List<PairRelation> LoadTypes();
    }

}
