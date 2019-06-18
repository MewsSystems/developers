                {
                    // Internet connection/file availability
                    throw new Exception($"Check Internet connection or source file availability - {ex.Message}");
                }

                // Validate webData has content
                if(string.IsNullOrEmpty(webData))
                {
                    throw new Exception("Source data is empty!");
                }

                var currLines = webData.Split(new string[] { newLine }, StringSplitOptions.None);
                
                if(currLines.Length<3)
                {
                    throw new Exception("No data found!");
                }

                // ignore header information by starting at line 3
                for (int i = 2; i < currLines.Length; i++)
                {
                    var linecontent = currLines[i].Split(delimeter);
                    if (linecontent.Length != 5)
                    {
                        continue;
                    }

                    var amt = decimal.Parse(linecontent[2]);
                    var currCode = linecontent[3];
                    var rate = decimal.Parse(linecontent[4]);
                    if (currencies.Any(o => o.Code.Equals(currCode, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        result.Add(new ExchangeRate(new Currency(currCode), czkCurr, rate / amt));
                    }
                }

                return result;// Enumerable.Empty<ExchangeRate>();
           
        }

        public string GetAbbrMonthString(int monthIndex)
        {
            var months = new[]
            {
