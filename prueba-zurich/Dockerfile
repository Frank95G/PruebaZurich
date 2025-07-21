# Etapa de construcción
FROM node:22 AS builder
WORKDIR /app
COPY frontend/package.json frontend/package-lock.json ./
RUN npm install
COPY frontend .
RUN npm run build --prod

# Etapa de producción
FROM nginx:alpine
COPY --from=builder /app/dist/frontend /usr/share/nginx/html
COPY frontend/nginx-custom.conf /etc/nginx/conf.d/default.conf
EXPOSE 80