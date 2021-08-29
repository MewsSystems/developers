# Mews SRE task

You are given a file `logs.txt` with JSON log items containing information about requests on production for some time interval. The item contains various data, but most importantly endpoint name and the request duration. Several deploys happened in the given interval with bugs that made some endpoints much slower (regressed).

Your task is to choose any language to write a program, that reads this file and prints out which requests regressed and also the approximate time of it with 5-minute tolerance. It is up to you to define what regression means.

**Input**: `logs.txt.` file.

**Output**: Name of endpoints that regressed and the time when it approximately happened.

Note that the quality of the code is not the most important aspect, since this is not a task for a backend engineer.

## Usage
Execute with default parameters: `.\AnalyzeRegression.ps1`

Execute with parameters:
`.\AnalyzeRegression.ps1 -FilePath .\logs.txt -FloatingAvgBatchSize 5000 -Threshold 5`

Output to both console and file (requires change in the script - Write-Host to Write-Output)
`.\AnalyzeRegression.ps1 | Tee-Object -FilePath "./out.txt"`

## Algorithm
With normal distribution there is approx. 16% chance that items will be **above** the average more that the standard deviation.
Using a floating window of measurements of size `FloatingAvgBatchSize`, the code is iterating over the collection and comparing when values got too slow when compared to the floating average. To flag the call as potential introduction of a bug, there need to be a number of consecutive slow calls higher than the `Threshold` parameter provided.

### Possible improvements
- Different formula for computing average of the floating window, but this way we lose the information about standard deviation of the floating window. We could also compute the standard deviation less frequently.
```
$newAvg = ($oldAvg * $FloatingAvgBatchSize - $oldestItem.Dutation + $newItem.Dutation) / $FloatingAvgBatchSize
```
- changing the threshold to avoid false positives
- not iterating over the number of measurements, but rather a timespan
- use a kind of score system, to determine the number of slow calls over a time period (so they wouldn't have to be consecutive calls, considered separate peaks if there is one normal call in between). If there was slowness reported within the last 5mins, it is likely still the same peak.
- output the results in a more readable format (table)
- reading the file as a stream
- processing measurements for each endpoint in parallel

## Interpreting the results
This script will output:
- name of the measured endpoint
- general statistics for all measurements for this endpoint
- list of peaks in `Duration`, 2 lines for each
   - start of the peak, `Duration` of the call that triggered the alert, floating average of `FloatingAvgBatchSize` number of measurements before, Standard deviation of the same floating window, index of the measurement
   - end of the peak, number of consecutive calls that were above the threshold
```
/api/rates/add

Count             : 57784
Average           : 199.414215007615
Sum               : 11522951
Maximum           : 249
Minimum           : 150
StandardDeviation : 28.8057222447386
Property          : Duration

08/26/2021 13:14:28 : Duration: 247, Avg: 199.031147540984, Dev: 29.0110984122763, index: 6952
08/26/2021 13:14:28, 5 slow measurements
08/26/2021 13:20:47 : Duration: 218, Avg: 178, Dev: 0, index: 10000
08/26/2021 13:20:47, 5 slow measurements
```
If there are multiple reported peaks that are very close to each other, chances are that a bug was introduced before the first one.