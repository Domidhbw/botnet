# Use Node.js to build the Angular app
FROM node:18 AS build
WORKDIR /app
COPY commandcontrolserver-frontend/package.json commandcontrolserver-frontend/package-lock.json ./
RUN npm install
COPY commandcontrolserver-frontend/ ./
RUN npm run build:ssr -- --configuration=production

# Use Node.js to serve SSR Angular App
FROM node:18-alpine AS server
WORKDIR /app
COPY --from=build /app/dist /app/dist
COPY --from=build /app/package.json /app/package.json
RUN npm install --omit=dev

EXPOSE 4000
CMD ["npm", "run", "serve:ssr"]
