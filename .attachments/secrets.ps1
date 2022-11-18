$id = "f86e1b81-dae7-4210-a0d6-a7498ac3e5df"
$secrets = "$env:AppData\Microsoft\UserSecrets\$id\secrets.json"
if (Test-Path $secrets) {
    Write-Host "secrets.json already exists"
} else {
    $json = @"
{
  "AZDO_PAT": "",
  "personaOptions": {
    "andieUser": "auto.andie@wtwco.com",
    "andiePswd": "",
    "ginaUser": "auto.gina@wtwco.com",
    "ginaPswd": "",
    "connieUser": "auto.connie@wtwco.com",
    "conniePswd": ""
  }
}
"@
    New-Item -Path $secrets -ItemType "file" -Force -Value $json
    Write-Host "secrets.json created"
}
notepad $secrets