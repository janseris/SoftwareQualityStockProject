﻿using System.Globalization;

using Microsoft.VisualBasic.FileIO;

using Project.API.Models;
using Project.DataAccses.External.CSV.Interfaces;

namespace Project.DataAccses.External.CSV
{
    public class StockPositionsCSVParser : IStockPositionsCSVParser
    {
        public IList<StockPositionRecord> Parse(byte[] csv)
        {
            var lines = GetTextLines(csv);
            lines = GetCleanedCSVLines(lines);
            var items = new List<StockPositionRecord>();
            foreach (var line in lines)
            {
                var item = Parse(line);
                items.Add(item);
            }
            return items;
        }

        private List<string> GetTextLines(byte[] csv)
        {
            List<string> lines = new List<string>();
            using var ms = new MemoryStream(csv);
            using var reader = new StreamReader(ms);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
            return lines;
        }

        /// <summary>
        /// Removes first and last line which is some non-data from the stock positions CSV 
        /// <br>This cleaning process is only valid for the specific stock positions CSV file, does not apply to general CSV files.</br>
        /// </summary>
        private List<string> GetCleanedCSVLines(List<string> lines)
        {
            lines.RemoveAt(0); //remove first line - CSV headers
            lines.RemoveAt(lines.Count - 1); //remove last line - not data
            return lines;
        }

        //source: https://stackoverflow.com/questions/6542996/how-to-split-csv-whose-columns-may-contain-comma?noredirect=1&lq=1
        private List<string> ParseIntoFields(string line)
        {
            using var parser = new TextFieldParser(new StringReader(line));
            parser.HasFieldsEnclosedInQuotes = true;
            parser.SetDelimiters(",");
            var fields = parser.ReadFields();
            return fields.ToList();
        }

        private const int FieldsCount = 8;

        private StockPositionRecord Parse(string line)
        {
            var fields = GetFieldsSafe(line);
            if(fields.Count != FieldsCount)
            {
                throw new ArgumentException($"Incorrect number of fields when parsing {line} to delimited fields. Expected count: {FieldsCount}");
            }
            //date = [0]
            //company name = [2]
            //ticker = [3]
            //shares = [4]
            DateTime date = DateTime.ParseExact(fields[0], "dd/MM/yyyy", null);
            string companyName = fields[2];
            string ticker = fields[3];
            //https://stackoverflow.com/questions/59881624/c-sharp-parse-int-from-different-locale
            int shares = int.Parse(fields[4], NumberStyles.Integer | NumberStyles.AllowThousands, CultureInfo.InvariantCulture /* ',' thousands separator */);
            return new StockPositionRecord(date, companyName, ticker, shares);
        }

        /// <summary>
        /// Throws <see cref="ArgumentException"/> if csv line parsing into token fails.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private List<string> GetFieldsSafe(string line)
        {
            try
            {
                var fields = ParseIntoFields(line);
                return fields;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse line {line} to delimited fields. See inner exception for details.", ex);
            }
        }
    }
}
