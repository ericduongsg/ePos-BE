using Newtonsoft.Json;
using System.Collections.Generic;

namespace Web.BO
{
    #region Google
    #region Google Request
    public class ContentGoogleRequest
    {
        public List<PartGoogleRequest> parts { get; set; }
    }

    public class GenerationConfigGoogleRequest
    {
        public double temperature { get; set; }
        public int topK { get; set; }
        public int topP { get; set; }
        public int maxOutputTokens { get; set; }
        public List<object> stopSequences { get; set; }
    }

    public class PartGoogleRequest
    {
        public string text { get; set; }
    }

    public class InfoGoogleRequest
    {
        public List<ContentGoogleRequest> contents { get; set; }
        public GenerationConfigGoogleRequest generationConfig { get; set; }
        public List<SafetySettingGoogleRequest> safetySettings { get; set; }
    }

    public class SafetySettingGoogleRequest
    {
        public string category { get; set; }
        public string threshold { get; set; }
    }


    #endregion

    #region Google Response
    public class CandidateGoogleResponse
    {
        public ContentGoogleResponse content { get; set; }
        public string finishReason { get; set; }
        public int index { get; set; }
        public List<SafetyRatingGoogleResponse> safetyRatings { get; set; }
    }

    public class ContentGoogleResponse
    {
        public List<PartGoogleResponse> parts { get; set; }
        public string role { get; set; }
    }

    public class PartGoogleResponse
    {
        public string text { get; set; }
    }

    public class PromptFeedbackGoogleResponse
    {
        public List<SafetyRatingGoogleResponse> safetyRatings { get; set; }
    }

    public class InfoGoogleResponse
    {
        public List<CandidateGoogleResponse> candidates { get; set; }
        public PromptFeedbackGoogleResponse promptFeedback { get; set; }
    }

    public class SafetyRatingGoogleResponse
    {
        public string category { get; set; }
        public string probability { get; set; }
    }
    #endregion

    #region Google Response Failed
    public class DetailFailedGoogleResponse 
    {
        [JsonProperty("@type")]
        public string type { get; set; }
        public string reason { get; set; }
        public string domain { get; set; }
        public MetadataFailedGoogleResponse metadata { get; set; }
    }

    public class ErrorFailedGoogleResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public List<DetailFailedGoogleResponse> details { get; set; }
    }

    public class MetadataFailedGoogleResponse
    {
        public string service { get; set; }
    }

    public class InfoFailedGoogleResponse
    {
        public ErrorFailedGoogleResponse error { get; set; }
    }
    #endregion
    #endregion

    #region Ajax Promotion
    public class APIResult
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; }
    }
    public class PromotionInfo
    {
        public string title { get; set; }
        public string summary { get; set; }
    }
    public class ItemInfo
    {
        public string name { get; set; }
        public string qty { get; set; }
        public string price { get; set; }
        public string amount { get; set; }
    }
    public class OrderInfo
    {
        public string total_amount { get; set; }
        public List<ItemInfo> items { get; set; }
    }
    #endregion
}