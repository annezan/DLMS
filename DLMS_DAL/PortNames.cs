using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace DLMS_DAL
{
    public class PortNames
    {
        public static List<string> GetPortNames() 
        {
            List<string> portsLibres = new List<string>();
            string[] ports = SerialPort.GetPortNames(); // Liste des ports COM disponibles

            foreach (string port in ports)
            {
                try
                {
                    using (SerialPort serialPort = new SerialPort(port, 19200)) // Configuration avec un baudrate par défaut
                    {
                        serialPort.Open(); // Tentative d'ouverture du port
                        serialPort.Close(); // Fermeture du port après test
                        portsLibres.Add(port);

                    }
                }
                catch (UnauthorizedAccessException)
                {
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return portsLibres;
        }

        public static string GetOpticalProbePort()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%(COM%'"))
            {
                foreach (var obj in searcher.Get())
                {
                    string name = obj["Name"].ToString();
                    if (name.Contains("Optical") || name.Contains("Probe") || name.Contains("Serial Port")) // Adapte selon ton matériel
                    {
                        return name.Substring(name.LastIndexOf("(COM") + 1).Replace("(", "").Replace(")", "");
                    }
                }
            }
            return null;
        }

        

    }
}
