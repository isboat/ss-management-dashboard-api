using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Management.Dashboard.Models.Settings;
using Management.Dashboard.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Management.Dashboard.Services
{
    public class S3UploadService : IUploadService
    {
        private readonly string? _bucketName;
        private readonly string? _accessKey;
        private readonly string? _secretKey;

        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.EUWest2;
        public S3UploadService(IOptions<S3Settings> settings)
        {
            _bucketName = settings.Value.BucketName;
            _accessKey = settings.Value.AccessKey;
            _secretKey = settings.Value.SecretKey;

        }

        public async Task<bool> RemoveAsync(string tenantId, string fileName)
        {
            try
            {
                // Set up your AWS credentials
                var credentials = new BasicAWSCredentials(_accessKey, _secretKey);
                using var s3Client = new AmazonS3Client(credentials, bucketRegion);

                var keyName = CreatePath(tenantId, fileName);

                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
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
                    var credentials = new BasicAWSCredentials(_accessKey, _secretKey);
                    using var s3Client = new AmazonS3Client(credentials, bucketRegion);
                    var fileTransferUtility = new TransferUtility(s3Client);

                    var key = CreatePath(tenantId, fileName);
                    await fileTransferUtility.UploadAsync(stream, _bucketName, key);
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

            return null!;
        }

        private string CreateS3Url(RegionEndpoint bucketRegion, string key)
        {
            var region = bucketRegion.SystemName;

            return $"https://{_bucketName}.s3.{region}.amazonaws.com/{key}";
        }

        private static string CreatePath(string tenantId, string filename)
        {
            return $"mediaasset/{tenantId}/{filename}";
        }
    }
}
