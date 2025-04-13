FROM node:lts-alpine AS builder
EXPOSE 5173
WORKDIR /appSource
COPY package*.json ./
RUN npm install --force && chmod -R +x node_modules/.bin
RUN rm -rf node_modules/ && npm install && npm audit fix
COPY . .
CMD ["npm", "run", "dev", "--", "--host"]