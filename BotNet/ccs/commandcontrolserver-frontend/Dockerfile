# Use Node.js to build the Angular app
FROM node:18 AS build
WORKDIR /app
COPY commandcontrolserver-frontend/package.json commandcontrolserver-frontend/package-lock.json ./
RUN npm install
COPY commandcontrolserver-frontend/ .
RUN npm run build --prod

# Use an Nginx image to serve the Angular app
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
