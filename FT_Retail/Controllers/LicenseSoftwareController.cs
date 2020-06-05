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
        public IActionResult Index(string licenseID)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            if (isoStore.FileExists("licenseKey.txt"))
            {
                //Console.WriteLine("The file already exists!");
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("licenseKey.txt", FileMode.Open, isoStore))
                {
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        //Console.WriteLine("Reading contents:");
                        ViewData["licenseKey"] = reader.ReadToEnd();
                    }
                }
            }

            var macAddr =
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();

            long.TryParse(Regex.Replace(macAddr, "[^0-9]", ""), out long machineID);
            machineID *= 1997 * 1997;

            ViewData["machineID"] = machineID.ToString("X");

            if (licenseID != null)
            {
                long license = machineID * 1997;
                long licenseDec = long.Parse(licenseID, System.Globalization.NumberStyles.HexNumber);

                if (license == licenseDec)
                {
                    if (isoStore.FileExists("licenseKey.txt"))
                    {
                        isoStore.DeleteFile("licenseKey.txt");
                    }
                        using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("licenseKey.txt", FileMode.CreateNew, isoStore))
                        {
                            using (StreamWriter writer = new StreamWriter(isoStream))
                            {
                                writer.WriteLine(licenseID);
                            }
                    }
                }
            }

            return View();
        }
    }
}