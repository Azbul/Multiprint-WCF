using System.ServiceModel;
using PrintService.Model.Interfaces;

namespace WebPrint
{
    public class PrintServiceClient
    {
        private BasicHttpBinding _binding;
        private EndpointAddress _endpoint;

        public PrintServiceClient()
        {
            _binding = new BasicHttpBinding();
            _endpoint = new EndpointAddress("http://localhost:53432/PrintService");
        }

        public IPrintService GetPrintService()
        {
            IPrintService service = null;

            using (var channelFactory = new ChannelFactory<IPrintService>(_binding, _endpoint))
            {
                service = channelFactory.CreateChannel();
            }

            return service;
        }
    }
}