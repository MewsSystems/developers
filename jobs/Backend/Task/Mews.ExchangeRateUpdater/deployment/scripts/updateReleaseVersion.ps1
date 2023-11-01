$version = ""
Write-Output($version)
Write-Output("##vso[task.setvariable variable=releaseNumber;]$version")