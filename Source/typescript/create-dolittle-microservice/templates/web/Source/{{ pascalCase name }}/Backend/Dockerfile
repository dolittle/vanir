FROM node:14.2.0 AS shared-build

WORKDIR /app

COPY .eslintrc.js ./
COPY tsconfig.json ./
COPY package.json ./

COPY Source/Shared/Backend/package.json ./Source/Shared/Backend/package.json
COPY Source/Shared/Components/package.json ./Source/Shared/Components/package.json
COPY Source/Shared/DependencyInversion/package.json ./Source/Shared/DependencyInversion/package.json
COPY Source/Shared/MVVM/package.json ./Source/Shared/MVVM/package.json
COPY Source/Shared/Platform/package.json ./Source/Shared/Platform/package.json
COPY Source/Shared/Portal/package.json ./Source/Shared/Portal/package.json
COPY Source/Shared/Styles/package.json ./Source/Shared/Styles/package.json
COPY Source/Shared/Web/package.json ./Source/Shared/Web/package.json
COPY Source/Shared/WebPack/package.json ./Source/Shared/WebPack/package.json
RUN yarn

COPY Source/{{name}}/version.json /app/Source/{{name}}/version.json

# Backend Build
FROM node:14.2.0 AS backend-build

WORKDIR /app

COPY --from=shared-build /app/ ./

WORKDIR /app/Source/{{name}}/Backend
COPY Source/{{name}}/Backend/package.json ./package.json
RUN yarn

COPY Source/Shared /app/Source/Shared/
COPY Source/{{name}}/Backend/ ./

RUN yarn build

# Web Build
FROM node:14.2.0 AS web-build
WORKDIR /app

COPY --from=shared-build /app/ ./

WORKDIR /app/Source/{{name}}/Web
COPY Source/{{name}}/Web/package.json ./package.json
RUN yarn

COPY Source/Shared /app/Source/Shared/
COPY Source/{{name}}/Web ./
RUN yarn build

# Resulting Image
FROM node:14.2.0-slim as base
ARG CONFIGURATION=Release

RUN echo Configuration = $CONFIGURATION

WORKDIR /app
COPY --from=backend-build /app/Source/{{name}}/Backend/dist/index.js ./
COPY --from=backend-build /app/Source/{{name}}/Backend/config.json ./
COPY --from=backend-build /app/node_modules/swagger-ui-dist/swagger-ui.css ./public/api/{{name}}/swagger/
COPY --from=backend-build /app/node_modules/swagger-ui-dist/swagger-ui-standalone-preset.js ./public/api/{{name}}/swagger/
COPY --from=backend-build /app/node_modules/swagger-ui-dist/swagger-ui-bundle.js ./public/api/{{name}}/swagger/
COPY --from=backend-build /app/node_modules/swagger-ui-dist/favicon-16x16.png ./public/api/{{name}}/swagger/
COPY --from=backend-build /app/node_modules/swagger-ui-dist/favicon-32x32.png ./public/api/{{name}}/swagger/
COPY --from=backend-build /app/Source/{{name}}/Backend/swagger.json ./
COPY --from=web-build /app/Source/{{name}}/Web/wwwroot ./public

ENTRYPOINT ["node", "index.js"]
