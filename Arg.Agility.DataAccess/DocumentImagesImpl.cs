﻿using Arg.Agility.DataModels;
using Arg.DataAccess;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Arg.Agility.DataAccess
{
    public class DocumentImagesImpl
    {
        private readonly SqlConnection _clientDbConnection;
        public DocumentImagesImpl() => _clientDbConnection = Common.ClientDatabase;

        public static string[] PriorityFiles = { "Shipping Instructions", "Dangerous Cargo Manifest", "Dangerous Cargo Paperwork" };

        public List<DocumentImages> GetDocumentImage(string jobNumber)
        {
            const string query = @"SELECT * FROM DocumentImages i
                                   WHERE i.fileName <> '' AND i.JobNumber=@JobNumber
                                   ORDER BY i.Type;";

            var documentImages = _clientDbConnection.Query<DocumentImages>(query, new { JobNumber = jobNumber }).ToList();
            var files = new List<DocumentImages>();
            var pf = documentImages.Where(x => x.Type.Contains(PriorityFiles[0]));
            files.AddRange(pf);
            pf = documentImages.Where(x => x.Type.Contains(PriorityFiles[1]));
            files.AddRange(pf);
            pf = documentImages.Where(x => x.Type.Contains(PriorityFiles[2]));
            files.AddRange(pf);
            var remaining = documentImages.Except(files);
            files.AddRange(remaining);
            return files;
        }
    }
}
