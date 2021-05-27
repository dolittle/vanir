#!/bin/bash
docker-compose restart sampleapp-runtime-aspnetcore
touch ./Source/Typescript/Backend/index.ts
