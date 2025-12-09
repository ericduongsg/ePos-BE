using API.BL;
using API.BO;
using Core_App.BL;
using Microsoft.ML;
using Models.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Web.BO;
using static System.Net.Mime.MediaTypeNames;


namespace WhiteCoat.API.Controllers
{
    public class DevController : ApiController
    {
        private Utilities until_bl = new Utilities();
        private Logger logger = LogManager.GetLogger("Write-Log");
        //private ApiResult apiResult;

        private readonly NLog.Logger _logger = NLog.LogManager.GetLogger("Log");
        private readonly ApiKey_BL apiKeyBL = new ApiKey_BL();
      

        [HttpPost]
        public ApiResult GenerateResult(GenerateContentModelRequest model)
        {

            //string appdatafolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data");
            //string fileName = "response.txt";

            //string prompt = File.ReadAllText(appdatafolder + "\\" + fileName);


            //List<ItemInfo> resultInfos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemInfo>>(prompt);

            //string apiResponse = "";
            //double totalAmount = 0;
            //foreach (var item in resultInfos)
            //{
            //    string name = item.name;
            //    string qty = item.qty;
            //    string price = item.price;
            //    double amount = double.Parse(qty) * double.Parse(price);
            //    totalAmount += amount;
            //    apiResponse += name  + " " + String.Format("{0:N2}", double.Parse(qty)) + " x " + String.Format("{0:N0}", double.Parse(price)) + " = " + String.Format("{0:N0}", amount) + "\n";
            //}
            //apiResponse = apiResponse + String.Format("{0:N0}", totalAmount);
            //return new ApiResult(ErrorCodes.OK, "success", apiResponse);

            string content = model.Content.Trim();

            string text = "";

            if (model.Language == "en_US")
            {
                text = System.Configuration.ConfigurationManager.AppSettings["GOOGLE_API_CONTENT_EN"].ToString();
            }
            else if (model.Language == "en_SG")
            {
                text = System.Configuration.ConfigurationManager.AppSettings["GOOGLE_API_CONTENT_EN"].ToString();
            }
            else
            {
                text = System.Configuration.ConfigurationManager.AppSettings["GOOGLE_API_CONTENT_VN"].ToString();
            }

            string appdatafolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data");
            string fileName = "prompt.txt";

            string prompt = File.ReadAllText(appdatafolder + "\\" + fileName);

            text = prompt.Replace("{{content}}", content);

            InfoGoogleRequest infoRequest = new InfoGoogleRequest
            {
                contents = new List<ContentGoogleRequest>()
                {
                    new ContentGoogleRequest
                    {
                        parts = new List<PartGoogleRequest>()
                        {
                            new PartGoogleRequest
                            {
                                text=text
                            }
                        }
                    }
                },
                generationConfig = new GenerationConfigGoogleRequest
                {
                    temperature = 0,
                    topK = 1,
                    topP = 1,
                    maxOutputTokens = 8192,
                    stopSequences = new List<object>()
                },
                safetySettings = new List<SafetySettingGoogleRequest>()
                {
                    new SafetySettingGoogleRequest{category = "HARM_CATEGORY_HARASSMENT", threshold = "BLOCK_MEDIUM_AND_ABOVE"},
                    new SafetySettingGoogleRequest{category = "HARM_CATEGORY_HATE_SPEECH", threshold = "BLOCK_MEDIUM_AND_ABOVE"},
                    new SafetySettingGoogleRequest{category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_MEDIUM_AND_ABOVE"},
                    new SafetySettingGoogleRequest{category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_MEDIUM_AND_ABOVE"}
                }
            };

            try
            {
                // Get the best available API key from database
                ApiKeyInfo apiKeyInfo = apiKeyBL.GetBestAvailableApiKey();
                if (apiKeyInfo == null || string.IsNullOrEmpty(apiKeyInfo.Api_Key))
                {
                    return new ApiResult(ErrorCodes.BAD_REQUEST, "Error", "No available API keys at the moment. Please try again later.");
                }

                var client = new RestClient(System.Configuration.ConfigurationManager.AppSettings["GOOGLE_API_URL"].ToString());
                var request = new RestRequest("?key={key}", Method.POST);
                request.AddUrlSegment("key", apiKeyInfo.Api_Key);
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddBody(infoRequest);
                IRestResponse restResponse = client.Execute(request);
                if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Update API key usage after successful request
                    apiKeyBL.UpdateApiKeyUsage(apiKeyInfo.Api_Key);

                    InfoGoogleResponse infoGoogleResponse = JsonConvert.DeserializeObject<InfoGoogleResponse>(restResponse.Content);
                    string textResponse = infoGoogleResponse.candidates[0].content.parts[0].text;
                    //models/gemini-2.5-flash changes
                    textResponse = textResponse.Replace("```json", "");
                    textResponse = textResponse.Replace("```", "");
                    var resultInfos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemInfo>>(textResponse);

                    _logger.Info("Dev-contrller GenerateResult responseMessage: " + textResponse);

                    string apiResponse = "";
                    double totalAmount = 0;
                    foreach (var item in resultInfos)
                    {
                        string name = item.name;
                        string qty = item.qty;
                        string price = item.price;
                        double amount = double.Parse(qty) * double.Parse(price);
                        totalAmount += amount;
                        apiResponse += name + " " + String.Format("{0:N2}", double.Parse(qty)) + " x " + String.Format("{0:N0}", double.Parse(price)) + " = " + String.Format("{0:N0}", amount) + "\n\n";
                    }
                    apiResponse = apiResponse + "Tổng tiền: " + String.Format("{0:N0}", totalAmount);
                    return new ApiResult(ErrorCodes.OK, "success", apiResponse);
                    

                    //#region Title
                    //if (resultInfo["title"] != null)
                    //{
                    //    promotionInfo.title = resultInfo["title"];
                    //}
                    //if (resultInfo["Title"] != null)
                    //{
                    //    promotionInfo.title = resultInfo["Title"];
                    //}
                    //else if (resultInfo["article_title"] != null)
                    //{
                    //    promotionInfo.title = resultInfo["article_title"];
                    //}
                    //else if (resultInfo["post_title"] != null)
                    //{
                    //    promotionInfo.title = resultInfo["post_title"];
                    //}
                    //else if (resultInfo["Headline"] != null)
                    //{
                    //    promotionInfo.title = resultInfo["Headline"];
                    //}
                    //else if (resultInfo["headline"] != null)
                    //{
                    //    promotionInfo.title = resultInfo["headline"];
                    //}
                    //#endregion

                    //_logger.Info("Dev-contrller GenerateResult responseMessage: " + textResponse);
                    //return new ApiResult(ErrorCodes.OK, "success", "\n" + textResponse.Replace("\\n", "\n"));
                }
                else
                {
                    var objSerialize = JsonConvert.SerializeObject(restResponse.Content, Formatting.Indented);
                    _logger.Error("Dev-contrller GenerateResult responseMessage: " + Convert.ToString(objSerialize));
                    //InfoFailedGoogleResponse infoFailedGoogleResponse = JsonConvert.DeserializeObject<InfoFailedGoogleResponse>(objSerialize);

                    return new ApiResult(ErrorCodes.BAD_REQUEST, "Error", "Đã xảy ra lỗi, vui lòng thử lại trong giây lát. Error: " + restResponse.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Dev-contrller GenerateResult: " + ex.ToString());

                return new ApiResult(ErrorCodes.BAD_REQUEST, "Error", "Đã xảy ra lỗi, vui lòng thử lại trong giây lát.");
            }

        }
        [HttpPost]
        public ApiResult GenerateResultJson(GenerateContentModelRequest model)
        {
            string content = model.Content.Trim();
            string requestPrompt = "";

            if (model.Language == "en_US")
            {
                requestPrompt = System.Configuration.ConfigurationManager.AppSettings["GOOGLE_API_CONTENT_EN"].ToString();
            }
            else if (model.Language == "en_SG")
            {
                requestPrompt = System.Configuration.ConfigurationManager.AppSettings["GOOGLE_API_CONTENT_EN"].ToString();
            }
            else
            {
                requestPrompt = System.Configuration.ConfigurationManager.AppSettings["GOOGLE_API_CONTENT_VN"].ToString();
            }

            string appdatafolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data");
            string fileName = "prompt.txt";
            string prompt = File.ReadAllText(appdatafolder + "\\" + fileName);
            requestPrompt = prompt.Replace("{{content}}", content);

            try
            {
                // Get the best available API key from database
                ApiKeyInfo apiKeyInfo = apiKeyBL.GetBestAvailableApiKey();
                if (apiKeyInfo == null || string.IsNullOrEmpty(apiKeyInfo.Api_Key))
                {
                    return new ApiResult(ErrorCodes.BAD_REQUEST, "Error", "No available API keys at the moment. Please try again later.");
                }

                Utilities utilities = new Utilities();
                string apiKey = utilities.getDecrypt(apiKeyInfo.Api_Key);
                if (apiKeyInfo.Type == "1")//Request Gemini
                {
                    return requestGemini(requestPrompt, apiKey, apiKeyInfo.Api_Key);
                    //return requestMegaLLM(requestPrompt, apiKey);
                }
                else
                {
                    return requestMegaLLM(requestPrompt, apiKey, apiKeyInfo.Api_Key);                    
                }
                
            }
            catch (Exception ex)
            {
                _logger.Error("Dev-contrller GenerateResult Error: " + ex.ToString());

                return new ApiResult(ErrorCodes.BAD_REQUEST, "Error", "Đã xảy ra lỗi, vui lòng thử lại trong giây lát.");
            }

        }
        private ApiResult requestGemini(string requestPrompt, string apiKey, string encryptedApiKey)
        {
            InfoGoogleRequest infoRequest = new InfoGoogleRequest
            {
                contents = new List<ContentGoogleRequest>()
                {
                    new ContentGoogleRequest
                    {
                        parts = new List<PartGoogleRequest>()
                        {
                            new PartGoogleRequest
                            {
                                text=requestPrompt
                            }
                        }
                    }
                },
                generationConfig = new GenerationConfigGoogleRequest
                {
                    temperature = 0,
                    topK = 1,
                    topP = 1,
                    maxOutputTokens = 8192,
                    stopSequences = new List<object>()
                },
                safetySettings = new List<SafetySettingGoogleRequest>()
                {
                    new SafetySettingGoogleRequest{category = "HARM_CATEGORY_HARASSMENT", threshold = "BLOCK_MEDIUM_AND_ABOVE"},
                    new SafetySettingGoogleRequest{category = "HARM_CATEGORY_HATE_SPEECH", threshold = "BLOCK_MEDIUM_AND_ABOVE"},
                    new SafetySettingGoogleRequest{category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_MEDIUM_AND_ABOVE"},
                    new SafetySettingGoogleRequest{category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_MEDIUM_AND_ABOVE"}
                }
            };

            var client = new RestClient(System.Configuration.ConfigurationManager.AppSettings["GOOGLE_API_URL"].ToString());
            var request = new RestRequest("?key={key}", Method.POST);
            request.AddUrlSegment("key", apiKey);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddBody(infoRequest);
            IRestResponse restResponse = client.Execute(request);

            if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Update API key usage after successful request
                apiKeyBL.UpdateApiKeyUsage(encryptedApiKey);

                InfoGoogleResponse infoGoogleResponse = JsonConvert.DeserializeObject<InfoGoogleResponse>(restResponse.Content);
                string textResponse = infoGoogleResponse.candidates[0].content.parts[0].text;
                textResponse = textResponse.Replace("```json", "").Replace("```", "");
                var itemInfos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemInfo>>(textResponse);

                //_logger.Info("Dev-contrller GenerateResult responseMessage: " + textResponse);

                //string apiResponse = "";
                double totalAmount = 0;
                foreach (var item in itemInfos)
                {
                    //string name = item.name;
                    string qty = item.qty;
                    string price = item.price;

                    double amount = double.Parse(qty) * double.Parse(price);
                    totalAmount += amount;

                    item.qty = String.Format("{0:N2}", double.Parse(qty));
                    item.price = String.Format("{0:N0}", double.Parse(price));
                    item.amount = String.Format("{0:N0}", amount);

                    //apiResponse += name + " " + String.Format("{0:N2}", double.Parse(qty)) + " x " + String.Format("{0:N0}", double.Parse(price)) + " = " + String.Format("{0:N0}", amount) + "\n\n";
                }
                //apiResponse = apiResponse + "Tổng tiền: " + String.Format("{0:N0}", totalAmount);


                OrderInfo result = new OrderInfo();
                result.total_amount = String.Format("{0:N0}", totalAmount);
                result.items = itemInfos;

                return new ApiResult(ErrorCodes.OK, "success", result);
            }
            else
            {
                var objSerialize = JsonConvert.SerializeObject(restResponse.Content, Formatting.Indented);
                _logger.Error("requestGemini Error responseMessage: " + Convert.ToString(objSerialize));
                //InfoFailedGoogleResponse infoFailedGoogleResponse = JsonConvert.DeserializeObject<InfoFailedGoogleResponse>(objSerialize);

                return new ApiResult(ErrorCodes.BAD_REQUEST, "Error", "Đã xảy ra lỗi, vui lòng thử lại trong giây lát. Error: " + restResponse.StatusCode);
            }
        }

        private ApiResult requestMegaLLM(string requestPrompt, string apiKey, string encryptedApiKey)
        {
            //var requestInfo = new ChatCompletionRequest("openai-gpt-oss-20b");
            //requestInfo.AddMessage("user", requestPrompt);

            var requestInfo = new
            {
                model = System.Configuration.ConfigurationManager.AppSettings["MEGALLM_MODEL"].ToString(),
                messages = new[]
                {
                    new { role = "user", content = requestPrompt }
                }
            };

            // Configure Newtonsoft.Json serializer with camelCase
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            string jsonBody = JsonConvert.SerializeObject(requestInfo, serializerSettings);



            var client = new RestClient(System.Configuration.ConfigurationManager.AppSettings["MEGALLM_API_URL"].ToString());
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + apiKey);
            request.RequestFormat = RestSharp.DataFormat.Json;
            //request.AddBody(requestInfo);
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

            IRestResponse restResponse = client.Execute(request);
            if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Update API key usage after successful request
                apiKeyBL.UpdateApiKeyUsage(encryptedApiKey);



                ChatCompletionResponse chatCompletionResponse = JsonConvert.DeserializeObject<ChatCompletionResponse>(restResponse.Content);
                string textResponse = chatCompletionResponse.Choices[0].Message.Content;

                textResponse = textResponse.Replace("```json", "").Replace("```", "");
                var itemInfos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemInfo>>(textResponse);

                //_logger.Info("Dev-contrller GenerateResult responseMessage: " + textResponse);

                //string apiResponse = "";
                double totalAmount = 0;
                foreach (var item in itemInfos)
                {
                    //string name = item.name;
                    string qty = item.qty;
                    string price = item.price;

                    double amount = double.Parse(qty) * double.Parse(price);
                    totalAmount += amount;

                    item.qty = String.Format("{0:N2}", double.Parse(qty));
                    item.price = String.Format("{0:N0}", double.Parse(price));
                    item.amount = String.Format("{0:N0}", amount);

                    //apiResponse += name + " " + String.Format("{0:N2}", double.Parse(qty)) + " x " + String.Format("{0:N0}", double.Parse(price)) + " = " + String.Format("{0:N0}", amount) + "\n\n";
                }
                //apiResponse = apiResponse + "Tổng tiền: " + String.Format("{0:N0}", totalAmount);


                OrderInfo result = new OrderInfo();
                result.total_amount = String.Format("{0:N0}", totalAmount);
                result.items = itemInfos;

                return new ApiResult(ErrorCodes.OK, "success", result);
            }
            else
            {
                var objSerialize = JsonConvert.SerializeObject(restResponse.Content, Formatting.Indented);
                _logger.Error("requestMegaLLM Error responseMessage: " + Convert.ToString(objSerialize));
                //InfoFailedGoogleResponse infoFailedGoogleResponse = JsonConvert.DeserializeObject<InfoFailedGoogleResponse>(objSerialize);

                return new ApiResult(ErrorCodes.BAD_REQUEST, "Error", "Đã xảy ra lỗi, vui lòng thử lại trong giây lát. Error: " + restResponse.StatusCode);
            }

            //return new ApiResult(ErrorCodes.OK, "success", apiKey);
        }
        [HttpGet]
        public ApiResult TextSource(string language)
        {
            //_logger.Info("Dev-contrller TextSource language= " + language);
            string appdatafolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data");
            string fileName = "";
            if (language == "en_US") {
                fileName = "text_source_en.json";
            }
            else if (language == "en_SG")
            {
                fileName = "text_source_en.json";
            }
            else
            {
                fileName = "text_source_vn.json";
            }
            //_logger.Info("Dev-contrller TextSource fileName= " + fileName);
            using (StreamReader file = File.OpenText(appdatafolder+"\\" + fileName))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject o2 = (JObject)JToken.ReadFrom(reader);
                return new ApiResult(ErrorCodes.OK, ErrorString.OK, o2);
            }
            
        }
        [HttpGet]
        public ApiResult TextCorrection(string input)
        {
            var trainingData = new List<CorrectionData>
            {
                new CorrectionData { Original = "Jhon", Corrected = "John" },
                new CorrectionData { Original = "exmaple", Corrected = "example" },
                new CorrectionData { Original = "teh", Corrected = "the" },
                new CorrectionData { Original = "recieve", Corrected = "receive" }
            };

            var mlContext = new MLContext();

            // Load data
            var data = mlContext.Data.LoadFromEnumerable(trainingData);

            // Define pipeline
            var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(CorrectionData.Original))
                .Append(mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(CorrectionData.Corrected)))
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("Corrected", "PredictedLabel"));

            // Train
            var model = pipeline.Fit(data);

            var predictionEngine = mlContext.Model.CreatePredictionEngine<CorrectionData, CorrectionPrediction>(model);

            var result = predictionEngine.Predict(new CorrectionData { Original = input });

            Console.WriteLine($"Predicted Correction: {result.Corrected}");

            return new ApiResult(ErrorCodes.OK, ErrorString.OK, result.Corrected);

        }
    }
}