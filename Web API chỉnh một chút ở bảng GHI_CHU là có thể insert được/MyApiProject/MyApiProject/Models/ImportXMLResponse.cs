using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace MyApiProject.Models
{
    public class ImportXMLResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
