using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuthentication
{
	public class JwtFactory
	{
		public HashAlgorithm Algorithm { get; set; }
		public string Header { get; set; }
		public string Payload { get; set; }
		public string Signature { get; set; }

		public string Create(HashAlgorithm algorithm, byte[] privatekey, Dictionary<string, string> payload)
		{
			var header = new Dictionary<string, string>();
			header.Add("alg", algorithm.ToString());
			header.Add("typ", "JWT");
			var encodedHeader = this.ToJsonBase64Url(header);

			var encodedPayload = this.ToJsonBase64Url(payload);

			// TODO: also need to do RS256
			var signature = Base64Url.Encode(EncodeHS256(privatekey, $"{encodedHeader}.{encodedPayload}"));

			return $"{encodedHeader}.{encodedPayload}.{signature}";
		}

		private byte[] EncodeHS256(byte[] privatekey, string data)
		{
			using (HMACSHA256 hs256 = new HMACSHA256(privatekey))
			{
				return hs256.ComputeHash(Encoding.UTF8.GetBytes(data));
			}
		}

		private string ToJsonBase64Url(Dictionary<string, string> dict)
		{
			return Base64Url.Encode(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dict)));
		}
	}
}