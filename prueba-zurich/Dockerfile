# Etapa de construcción
FROM node:20 AS build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build

# Etapa final (servir la app)
FROM nginx:alpine
RUN rm -rf /usr/share/nginx/html/*FROM nginx:1.25-alpine
COPY --from=build /app/dist/prueba-zurich/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
RUN chmod -R 755 /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
