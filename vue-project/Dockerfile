#FROM node:22-alpine3.19

#WORKDIR /vue-app

#RUN npm install -g http-server
# make the 'app' folder the current working directory
#WORKDIR /app
# copy 'package.json' to install dependencies
#COPY vue-project/ ./
# install dependencies
#RUN npm install
# copy files and folders to the current working directory (i.e. 'app' folder)
#COPY . .
# build app for production with minification
#RUN npm run build

FROM node:lts-alpine as build-stage
RUN mkdir -p /app
WORKDIR /app
COPY vue-project/ ./
RUN npm install
COPY . .
RUN npm run build

FROM nginx:alpine
EXPOSE 8080
COPY vue-project/nginx.conf /etc/nginx/nginx.conf
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build-stage /app/dist /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]