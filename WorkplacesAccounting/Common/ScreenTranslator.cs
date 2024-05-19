using System.Net.Sockets;

namespace WorkplacesAccounting.Common
{
    public class ScreenTranslator
    {
        public string RemoteIPAdress { get; set; }
        public int RemotePort { get; set; }
        public Socket Socket { get; set; }
        public ScreenTranslator(string IPAdress,int Port) 
        {
            this.RemoteIPAdress = IPAdress;
            this.RemotePort = Port;
            
        }
        public async void StartTranslation()
        {
            try
            {
                
            }
            catch (Exception ex) 
            {
            
            }
        }
    }
}
