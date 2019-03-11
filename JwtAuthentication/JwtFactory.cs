using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuthentication
{
	public class JwtFactory
	{
		public string Create(HashAlgorithm algorithm, byte[] privateKey, Dictionary<string, string> payload)
		{
			var header = new Dictionary<string, string>();
			header.Add("alg", algorithm.ToString());
			header.Add("typ", "JWT");
			var encodedHeader = this.ToJsonBase64Url(header);

			var encodedPayload = this.ToJsonBase64Url(payload);

			string signature = string.Empty;
			if (algorithm == HashAlgorithm.RS256)
			{
				signature = Base64Url.Encode(EncodeHS256(privateKey, $"{encodedHeader}.{encodedPayload}"));
			}
			else if (algorithm == HashAlgorithm.RS256)
			{
				// TODO: also need to do RS256
				throw new NotImplementedException();
			}

			return $"{encodedHeader}.{encodedPayload}.{signature}";
		}

		public void Decode(string jwt)
		{
			throw new NotImplementedException();
		}

		private byte[] EncodeHS256(byte[] privateKey, string data)
		{
			using (HMACSHA256 hs256 = new HMACSHA256(privateKey))
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