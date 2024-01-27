using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Management.Dashboard.Services.Interfaces;
using SharpCompress.Common;
using System.IO;

namespace Management.Dashboard.Services
{
    public class S3UploadService : IUploadService
    {
        private const string bucketName = "isboatscreenservice";
        private const string accessKey = "AKIA5SW2KVX4TFYB2K6K";
        private const string secretKey = "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";

        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.EUWest2;
        public S3UploadService()
        {
        }

        public async Task<bool> RemoveAsync(string tenantId, string fileName)
        {
            try
            {
                // Set up your AWS credentials
                var credentials = new BasicAWSCredentials(accessKey, secretKey);
                using var s3Client = new AmazonS3Client(credentials, bucketRegion);

                var keyName = CreatePath(tenantId, fileName);

                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };

                var deleteObjectResponse = await s3Client.DeleteObjectAsync(deleteObjectRequest);

                return deleteObjectResponse.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }
            return false;
        }

        public async Task<string> UploadAsync(string tenantId, string fileName, Stream stream)
        {
            try
            {
                using (stream)
                {
                    // Set up your AWS credentials
                    var credentials = new BasicAWSCredentials(accessKey, secretKey);
                    using var s3Client = new AmazonS3Client(credentials, bucketRegion);
                    var fileTransferUtility = new TransferUtility(s3Client);

                    var key = CreatePath(tenantId, fileName);
                    await fileTransferUtility.UploadAsync(stream, bucketName, key);
                    return CreateS3Url(bucketRegion, key);
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }

            return null;
        }

        private static string CreateS3Url(RegionEndpoint bucketRegion, string key)
        {
            var region = bucketRegion.SystemName;

            return $"https://{bucketName}.s3.{region}.amazonaws.com/{key}";
        }

        private static string CreatePath(string tenantId, string filename)
        {
            return $"mediaasset/{tenantId}/{filename}";
        }
    }
}
