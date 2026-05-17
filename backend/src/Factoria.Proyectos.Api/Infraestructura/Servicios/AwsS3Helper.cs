using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace Factoria.Proyectos.Api.Infraestructura.Servicios
{
    public class AwsS3Helper
    {
        private readonly IConfiguration _config;

        public AwsS3Helper(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> UploadBase64(string base64, string tipoArchivo, string folder, int empresaid)
        {
            var bucket = _config["_AWSBucket"];
            var roleArn = _config["_AWSRoleArn"];
            var accessKey = _config["_AWSAccessKey"];
            var secretKey = _config["_AWSSecretKey"];
            var region = _config["_AWSRegion"];

            string data;
            if (base64.Contains(","))
                data = base64.Split(',')[1];
            else
                data = base64;

            var bytes = Convert.FromBase64String(data);

            var mimeMap = new Dictionary<string, string>
                {
                    {"image/png","png"},
                    {"image/jpeg","jpg"},
                    {"image/jpg","jpg"},
                    {"image/gif","gif"},
                    {"image/webp","webp"},
                    {"image/x-icon","ico"},
                    {"image/svg+xml","svg"},
                    {"application/pdf","pdf"}
                };
            var extension = mimeMap.ContainsKey(tipoArchivo) ? mimeMap[tipoArchivo] : "bin";
            var nombreArchivo = $"{Guid.NewGuid()}.{extension}";

            var baseCredentials = new BasicAWSCredentials(accessKey, secretKey);
            var assumedCredentials = new AssumeRoleAWSCredentials(baseCredentials, roleArn, "FactoriaSession");

            using var client = new AmazonS3Client(assumedCredentials, RegionEndpoint.GetBySystemName(region));

            var putRequest = new PutObjectRequest
            {
                BucketName = bucket,
                Key = $"{empresaid}/{folder}/{nombreArchivo}",
                InputStream = new MemoryStream(bytes),
                ContentType = tipoArchivo
            };

            await client.PutObjectAsync(putRequest);

            return $"https://{bucket}.s3.amazonaws.com/{empresaid}/{folder}/{nombreArchivo}";
        }
    }
}