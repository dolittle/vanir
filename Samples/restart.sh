#!/bin/bash
docker-compose restart sampleapp-runtime-aspnetcore
docker-compose restart sampleapp-runtime-typescript
touch ./Source/Typescript/Backend/index.ts
touch ./Source/Aspnetcore/Backend/Program.cs
