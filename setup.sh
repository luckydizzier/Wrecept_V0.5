#!/bin/bash
set -e

SOLUTION="Wrecept.CoreOnly.sln"

dotnet restore "$SOLUTION"
dotnet build "$SOLUTION" -c Release

dotnet test "$SOLUTION"
