#!/bin/bash
set -e

dotnet publish ./Wrecept.csproj \
  -c Release \
  -r win-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeAllContentForSelfExtract=true \
  -p:EnableCompressionInSingleFile=true \
  -o ./publish

