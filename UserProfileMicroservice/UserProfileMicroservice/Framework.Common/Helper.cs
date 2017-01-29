using System;
using System.Security.Cryptography;
using System.Text;
using VDS.RDF.Query;

namespace Framework.Common
{
    public class Helper
    {
        public static UserModellResponse MapResponse(SparqlResult result)
        {
            return new UserModellResponse
            {
                Resource = result.Value("s").ToString(),
                Email = result.Value("email").ToString(),
                Name = result.Value("name").ToString(),
                Id = new Guid(result.Value("id").ToString())
            };
        }

        public static string GenerateToken(string name)
        {
            return Helper.GetMd5Hash(MD5.Create(), name);
        }

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
