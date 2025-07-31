using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Models.Input
{
    
    public class GenerateContentModelRequest
    {
        public string Content { get; set; }
        public string Language { get; set; }
    }
}