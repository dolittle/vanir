#!/bin/bash
docker-compose restart sampleapp-runtime-warehouse
docker-compose restart sampleapp-runtime-shop
touch ./Source/Shop/Backend/index.ts
touch ./Source/Warehouse/Backend/Program.cs
