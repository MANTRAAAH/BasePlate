# Script di generazione e sincronizzazione automatica del Report PDF
$ErrorActionPreference = "Stop"

$workspaceRoot = Split-Path -Parent $PSScriptRoot
if (-not $workspaceRoot) { $workspaceRoot = (Get-Location).Path }

$htmlPath = Join-Path $workspaceRoot "BasePlate_Report_Stato_Progetto.html"
$localPdfPath = Join-Path $workspaceRoot "BasePlate_Report_Stato_Progetto.pdf"
$drivePdfPath = "G:\Il mio Drive\BasePlate_Report_Stato_Progetto.pdf"

Write-Host "📄 Rigenerazione PDF in corso da $htmlPath..." -ForegroundColor Cyan

# 1. Compilazione del PDF tramite Edge headless
$edgeExe = "C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"
$htmlUri = "file:///" + $htmlPath.Replace("\", "/")

Start-Process -FilePath $edgeExe -ArgumentList @(
    "--headless=new",
    "--disable-gpu",
    "--run-all-compositor-stages-before-draw",
    "--print-to-pdf=`"$localPdfPath`"",
    "`"$htmlUri`""
) -Wait

if (Test-Path $localPdfPath) {
    Write-Host "✅ PDF generato localmente: $localPdfPath" -ForegroundColor Green

    # 2. Sincronizzazione automatica su Google Drive (G:\)
    if (Test-Path "G:\Il mio Drive") {
        Copy-Item -Path $localPdfPath -Destination $drivePdfPath -Force
        Write-Host "☁️ PDF sincronizzato con successo su Google Drive: $drivePdfPath" -ForegroundColor Green
    } else {
        Write-Warning "⚠️ Il percorso 'G:\Il mio Drive' non è al momento accessibile. Verifica che Google Drive for Desktop sia attivo."
    }
} else {
    Write-Error "❌ Errore durante la generazione del PDF locale."
}

