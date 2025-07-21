# Etapa de construcción
FROM node:20 AS build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build --prod

# Etapa final
FROM nginx:alpine
COPY --from=build /app/dist/prueba-zurich /usr/share/nginx/html 
COPY nginx.conf /etc/nginx/conf.d/default.conf