using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gurux.Common;
using Gurux.Serial;
using Gurux.Net;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Secure;
using System.Diagnostics;
using System.IO.Ports;
using Gurux.DLMS.Objects.Enums;
using Gurux.DLMS;
using System.Net.Mime;
using System.Text;
using System;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Authorization;
namespace API_DLMS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MainController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetReadAll(string? port, string? serialport, string? AddressIp,string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey)
        {
            var read=Connexion.Read(port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey);
            var status = read.Substring(read.Length - 3);
            if (status=="200" )
            {
                var jsonResponse = read.Substring(0,read.Length - 3);
                return Ok(jsonResponse);
            }
            else
            {
                return StatusCode(500, new { error = read.Substring(0, read.Length - 3) });
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> GetReadObjectsProfile(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects)
        {
            var read = await Connexion.ReadObjectsProfile(port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey, Objects);
            var status = read.ToString().Substring(read.ToString().Length - 3);
            if (status == "200")
            {
                var jsonResponse = read.ToString().Substring(0, read.ToString().Length - 3);
                return Ok(jsonResponse);
            }
            else
            {
                return StatusCode(500, new { error = read.ToString().Substring(0, read.ToString().Length - 3) });
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetReadObjectsMajCompteur(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects)
        {
            var read = await Connexion.ReadObjectsMajCompteur(port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey, Objects);
            var status = read.ToString().Substring(read.ToString().Length - 3);
            if (status == "200")
            {
                var jsonResponse = read.ToString().Substring(0, read.ToString().Length - 3);
                return Ok(jsonResponse);
            }
            else
            {
                return StatusCode(500, new { error = read.ToString().Substring(0, read.ToString().Length - 3) });
            }

        }
        [HttpGet]
        public async Task<IActionResult> GetReadObjectsCommande(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects,int? Idcommande, int? Nombreentree, DateTime? DateDebut, DateTime? DateFin)
        {
            var read = await Connexion.ReadObjectsCommande(port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey, Objects,Idcommande, Nombreentree,  DateDebut, DateFin);
            var status = read.ToString().Substring(read.ToString().Length - 3);
            if (status == "200")
            {
                var jsonResponse = read.ToString().Substring(0, read.ToString().Length - 3);
                return Ok(jsonResponse);
            }
            else
            {
                return StatusCode(500, new { error = read.ToString().Substring(0, read.ToString().Length - 3) });
            }

        }

    }
}
