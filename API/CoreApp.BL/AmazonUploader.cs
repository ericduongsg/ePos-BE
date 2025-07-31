using System;
using System.Collections.Generic;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using NLog;

namespace API.BL
{
    public class AmazonUploader
    {
        Logger log = LogManager.GetLogger("S3");

        public bool uploadFile(byte[] data, string subDirectoryInBucket, string fileNameInS3)
        {
            IAmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.APSoutheast1);

            TransferUtility utility = new TransferUtility(client);
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
            string bucket_name = System.Configuration.ConfigurationManager.AppSettings["S3_bucket"];

            request.BucketName = bucket_name + @"/" + subDirectoryInBucket;
            request.Key = fileNameInS3;
            request.InputStream = new MemoryStream(data);
            utility.Upload(request);
            client.Dispose();

            return true;
        }

        public bool checkFile(string folder, string file_name)
        {
            string bucket_name = System.Configuration.ConfigurationManager.AppSettings["S3_bucket"];
            IAmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.APSoutheast1);
            S3FileInfo s3FileInfo = new S3FileInfo(client, bucket_name, folder + "/" + file_name);
            bool result = s3FileInfo.Exists;
            client.Dispose();
            return result;
        }

        public void moveFolder(string folder_old, string file_old, string folder_new, string file_new)
        {
            string bucket_name = System.Configuration.ConfigurationManager.AppSettings["S3_bucket"];
            IAmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.APSoutheast1);
            try
            {
                CopyObjectRequest request = new CopyObjectRequest();
                request.SourceBucket = System.Configuration.ConfigurationManager.AppSettings["S3_bucket"];
                request.SourceKey = folder_old + "/" + file_old;
                request.DestinationBucket = bucket_name;
                request.DestinationKey = folder_new + "/" + file_new;
                client.CopyObject(request);

                deleteFile(folder_old, file_old);
                client.Dispose();
            }
            catch (Exception ex)
            {
                client.Dispose();
                log.Info(ex.Message);
            }

        }

        public bool deleteFile(string folder, string file_name)
        {
            string bucket_name = System.Configuration.ConfigurationManager.AppSettings["S3_bucket"];
            IAmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.APSoutheast1);
            if (checkFile(folder, file_name))
            {
                DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucket_name,
                    Key = folder + "/" + file_name
                    //Key = subDirectoryInBucket + "/" + fileNameInS3
                };
                try
                {
                    client.DeleteObject(deleteObjectRequest);
                    client.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    client.Dispose();
                    log.Info(ex.Message);
                    return false;
                }
            }
            else
            {
                client.Dispose();
                return false;
            }
        }

        public void deleteFolder(String folder_path)
        {
            string bucket_name = System.Configuration.ConfigurationManager.AppSettings["S3_bucket"];
            IAmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.APSoutheast1);
            ListObjectsRequest request = new ListObjectsRequest();
            request.BucketName = bucket_name;
            request.Prefix = folder_path;
            ListObjectsResponse response = client.ListObjects(request);
            foreach (S3Object o in response.S3Objects)
            {
                DeleteObjectRequest deleteRequest = new DeleteObjectRequest
                {
                    BucketName = bucket_name,
                    Key = o.Key
                };
                client.DeleteObject(deleteRequest);
                client.Dispose();
                // Console.WriteLine("Deleting an object " + o.Key);
            }
        }

        public void deleteObjectsInFolder(String folder_path, List<String> file_names)
        {
            string bucket_name = System.Configuration.ConfigurationManager.AppSettings["S3_bucket"];
            IAmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.APSoutheast1);
            DeleteObjectsRequest multiObjectDeleteRequest = new DeleteObjectsRequest();
            multiObjectDeleteRequest.BucketName = bucket_name;

            foreach (string file_name in file_names)
            {
                multiObjectDeleteRequest.AddKey(folder_path + "/" + file_name, null);
            }

            try
            {
                DeleteObjectsResponse response = client.DeleteObjects(multiObjectDeleteRequest);
                client.Dispose();

                //   Console.WriteLine("Successfully deleted all the {0} items", response.DeletedObjects.Count);
            }
            catch (DeleteObjectsException ex)
            {
                client.Dispose();
                log.Info(ex.Message);
                // Process exception.
            }
        }

        public void deleteFileInFolderTemps(String folder_path, float days)
        {
            try
            {
                string bucket_name = System.Configuration.ConfigurationManager.AppSettings["S3_bucket"];
                IAmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.APSoutheast1);
                DeleteObjectsRequest multiObjectDeleteRequest = new DeleteObjectsRequest();
                multiObjectDeleteRequest.BucketName = bucket_name;

                ListObjectsRequest image_list = new ListObjectsRequest()
                {
                    BucketName = bucket_name,
                    Prefix = folder_path,
                };
                ListObjectsResponse response = client.ListObjects(image_list);
                foreach (S3Object s3Object in response.S3Objects)
                {
                    if (s3Object.LastModified < DateTime.Now.AddDays(-days))
                    {
                        log.Info("Delete: " + s3Object.Key + ":" + s3Object.LastModified);
                        multiObjectDeleteRequest.AddKey(s3Object.Key, null);
                    }
                }
                if (multiObjectDeleteRequest.Objects.Count > 0)
                {
                    client.Dispose();
                    client = Amazon.AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.APSoutheast1);
                    DeleteObjectsResponse deleteObjectsResponse = client.DeleteObjects(multiObjectDeleteRequest);
                }
                client.Dispose();
            }
            catch (AmazonS3Exception ex)
            {
                log.Info("Error:" + ex.ToString());
            }
        }
    }
}