using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class CSVRecords<T, U> where U : CsvClassMap
    {
        public List<T> Records { get; set; }

        private string _FileName;

        public CSVRecords(string fileName)
        {
            _FileName = fileName;

            Load();
        }

        public void Load()
        {
            try
            {
                using (var sr = new StreamReader(_FileName, new UTF8Encoding(false)))
                using (var parser = new CsvParser(sr))
                {
                    parser.Configuration.HasHeaderRecord = true;
                    parser.Configuration.RegisterClassMap<U>();

                    using (var reader = new CsvReader(parser))
                    {
                        Records = reader.GetRecords<T>().ToList();
                    }
                }
            }
            catch (Exception)
            {
                Records = new List<T>();
            }
        }

        public void Save()
        {
            using (var fs = new FileStream(_FileName, FileMode.Truncate, FileAccess.Write))
            using (var sw = new StreamWriter(fs))
            using (var csvhelper = new CsvWriter(sw))
            {
                csvhelper.Configuration.RegisterClassMap<U>();
                csvhelper.WriteHeader<T>();

                foreach (var item in Records)
                {
                    csvhelper.WriteRecord(item);
                }
            }
        }
    }
}
