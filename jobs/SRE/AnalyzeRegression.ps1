[CmdletBinding()]
Param(
    [string]$FilePath           = './logs.txt', # log file containing json entries with Timestamp, Endpoint and Duration fields
    [int]$FloatingAvgBatchSize  = 5000,         # number of entries to be compared for floating average and deviation
    [int]$Threshold             = 5             # number of consequtive slow measurements to trigger an alert
)

# read log entries and group by endpoint
# todo: read the file as a stream to reduce memory usage in case of very big files, filelock
$enpointGroups = Get-Content $FilePath | ConvertFrom-Json | Group-Object -Property Endpoint

# todo: run in parallel
# todo: nicer output, perhaps format as table
foreach ($endpointStats in $enpointGroups) {
    $endpointStats.Name
    $endpointStats.Group | Measure-Object -Property Duration -AllStats 
    
    $entries = $endpointStats.Group
    $count = 0
    for ($i = 0; $i -lt $entries.Count - $FloatingAvgBatchSize; $i++) {
        $item = $entries[$FloatingAvgBatchSize+$i]

        # todo: very ineffective, change formula
        $measurement = $entries[$i..$FloatingAvgBatchSize+$i] | Measure-Object -Property Duration -Average -StandardDeviation
        if ($item.Duration - $measurement.Average -gt $measurement.StandardDeviation) {
            # flag as suspicious if it reaches threshold
            if (++$count -eq $Threshold) {
                Write-Host -ForegroundColor red "$($item.Timestamp) : Duration: $($item.Duration), Avg: $($measurement.Average), Dev: $($measurement.StandardDeviation), index: $($FloatingAvgBatchSize+$i)"
            }
        }
        else {
            if ($count -ge $Threshold) {
                # slowness spike is over
                Write-Host -ForegroundColor green "$($item.Timestamp), $count slow measurements"
            }
            $count = 0
        }
    }
}
