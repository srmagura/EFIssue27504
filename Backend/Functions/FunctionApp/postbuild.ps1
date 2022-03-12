param([string]$projectConfiguration)

if ($projectConfiguration -eq 'Release') {
	exit 0
}

# Starts in project root
cp pre-commit ../../../.git/hooks
"Copied pre-commit to .git/hooks."

& "../GenerateTypeScript/bin/$projectConfiguration/net6.0/GenerateTypeScript"
