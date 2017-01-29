﻿using System;
using System.Security.Cryptography;
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
            return MD5.Create(name).ToString();
        }
    }
}
