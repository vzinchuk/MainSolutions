using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightTestApplication.Core
{
    public class GetData
    {

        public static ServiceReference1.SchemaServiceClient GetService()
        {
            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            binding.MaxReceivedMessageSize = 2147483647; // int's max size
            binding.MaxBufferSize = 2147483647; // int's max size
            System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(new Uri(Application.Current.Host.Source, "../Service.svc"));
            try
            {
                return new ServiceReference1.SchemaServiceClient(binding, address);
            }
            catch (Exception e)
            {

            }
            return null;           
        }
    }
}
