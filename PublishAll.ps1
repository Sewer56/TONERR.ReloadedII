
# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

./Publish.ps1 -ProjectPath "sonicheroes.utils.toner/sonicheroes.utils.toner.csproj" `
              -PackageName "sonicheroes.utils.toner" `

Pop-Location