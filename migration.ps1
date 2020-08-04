Write-Host "
1. Database migration
2. Database update
"

$option = Read-Host -Prompt "Select option"

if ($option -eq '0') {
    Write-Host $option
}

if ($option -eq '1') {
    $migration_name = Read-Host -Prompt "Write migration name: "
    cd ./Data
    dotnet ef --startup-project ../Web migrations add $migration_name --context WebQuizDbContext
    cd ../
}

if ($option -eq '2') {
    cd ./Data 
    dotnet ef --startup-project ../Web database update --context WebQuizDbContext
    cd ../
}

Read-Host -Prompt "Press 'Enter' to exit"