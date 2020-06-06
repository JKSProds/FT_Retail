using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FT_Retail.Controllers
{
    public class LicenseSoftwareController : Controller
    {

        private const string LICENSEFILE = "licenseKey.txt";

        private string getMachineID ()
        {
            var macAddr =
           (
               from nic in NetworkInterface.GetAllNetworkInterfaces()
               where nic.OperationalStatus == OperationalStatus.Up
               select nic.GetPhysicalAddress().ToString()
           ).FirstOrDefault();

            long.TryParse(Regex.Replace(macAddr, "[^0-9]", ""), out long machineID);
            machineID *= 1997 * 1997;

            return machineID.ToString("X");
        }

        private bool checkLicense(string licenseID, string machineID)
        {
            long.TryParse(machineID, System.Globalization.NumberStyles.HexNumber, null, out long license);
            long.TryParse(licenseID, System.Globalization.NumberStyles.HexNumber, null, out long licenseDec);

            if (license * 1997 == licenseDec)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public string readLicense ()
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            if (isoStore.FileExists(LICENSEFILE))
            {
                //Console.WriteLine("The file already exists!");
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(LICENSEFILE, FileMode.Open, isoStore))
                {
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        string licenseID = reader.ReadToEnd();

                        if (checkLicense(licenseID, getMachineID()))
                        {
                            return licenseID;
                        }
                        else
                        {
                            isoStore.DeleteFile(LICENSEFILE);
                        }
                    }
                }
            }
            return "";
        }

        private void writeLicense (string licenseID)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            if (isoStore.FileExists(LICENSEFILE))
            {
                isoStore.DeleteFile(LICENSEFILE);
            }
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(LICENSEFILE, FileMode.CreateNew, isoStore))
            {
                using (StreamWriter writer = new StreamWriter(isoStream))
                {
                    writer.WriteLine(licenseID);
                }
            }
        }

        public IActionResult Index(string licenseID)
        {

            ViewData["licenseKey"] = readLicense();
            ViewData["machineID"] = getMachineID();

            if (licenseID != null)
            {
              if (checkLicense(licenseID, getMachineID()))
                {
                    writeLicense(licenseID);
                    return Redirect("/");
                }
                else
                {
                    ViewBag.Message = "Licença incorreta. Por favor contacte o administrador do seu sistema!";
                    ViewBag.Color = "ERROR";
                }
            }

            return View();
        }
    }
}