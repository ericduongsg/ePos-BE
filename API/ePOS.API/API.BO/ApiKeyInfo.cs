using System;

namespace API.BO
{
    public class ApiKeyInfo
    {
        public int Id { get; set; }
        public string Api_Key { get; set; }
        public string Latest_Request_On { get; set; }
        public int Total_Request { get; set; }
        public string Type { get; set; }//1: gemini; 2: mega-llm
    }
}