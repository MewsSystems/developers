[CmdletBinding()]
Param(
    [string]$FilePath  = './logs.txt',
    [int]$FloatingAvgBatchSize = 5000,
    [int]$DeviationThresholdPercentage = 0.1
)

$FilePath = './logs_short.txt'
$FilePath = './logs.txt'

# read log entries and group by endpoint
# todo: read the file as a stream to reduce memory usage in case of very big files, filelock
$enpointGroups = Get-Content $FilePath | ConvertFrom-Json | Group-Object -Property Endpoint

foreach ($endpointStats in $enpointGroups) {
    $endpointStats.Name
    $endpointStats.Group | Measure-Object -Property Duration -AllStats 
    
    $entries = $endpointStats.Group
    $count = 0
    $threshold = 5
    for ($i = 0; $i -lt $entries.Count - $FloatingAvgBatchSize; $i++) {
        $item = $entries[$FloatingAvgBatchSize+$i]
        $measurement = $entries[$i..$FloatingAvgBatchSize+$i] | Measure-Object -Property Duration -Average -StandardDeviation
        if ($item.Duration - $measurement.Average -gt $measurement.StandardDeviation) {
            #flag as suspicious if it's the first time encountering worsened performance
            if (++$count -eq $threshold) {              
                Write-Host -ForegroundColor red "$($item.Timestamp) : Duration: $($item.Duration), Avg: $($measurement.Average), Dev: $($measurement.StandardDeviation), index: $($FloatingAvgBatchSize+$i)"
            }
        }
        else {
            if ($count -ge $threshold) {
                Write-Host -ForegroundColor green "$($item.Timestamp), $count slow measurements)"
            }
            $count = 0
        }
    }
}
