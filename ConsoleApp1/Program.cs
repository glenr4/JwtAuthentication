using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using JwtAuthentication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;

namespace ConsoleApp1
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			// Create a random key using a random number generator. This would be the
			//  secret key shared by sender and receiver.
			byte[] privateKey = new Byte[64];
			//RNGCryptoServiceProvider is an implementation of a random number generator.
			using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
			{
				// The array is now filled with cryptographically strong random bytes.
				rng.GetBytes(privateKey);

				Debug.WriteLine($"Private key: {Base64Url.Encode(privateKey)}");

				var payload = new Dictionary<string, string>();
				payload.Add("uid", "Glen");

				// Use the secret key to sign the message file.
				var jwtFactory = new JwtFactory();
				var jwt = jwtFactory.Create(JwtAuthentication.HashAlgorithm.HS256, privateKey, payload);

				Console.WriteLine($"JWT: {jwt}");
				Debug.WriteLine($"JWT: {jwt}");

				OtherJwt(payload, privateKey);

				Console.ReadLine();
			}
		}

		private static void OtherJwt(Dictionary<string, string> payload, byte[] secret)
		{
			IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
			IJsonSerializer serializer = new JsonNetSerializer();
			IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
			IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

			var token = encoder.Encode(payload, secret);
			Console.WriteLine($"Other JWT: {token}");
			Debug.WriteLine($"Other JWT: {token}");
		}
	}
}