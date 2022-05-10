FROM nginx:alpine

WORKDIR /etc/nginx/conf.d
COPY docker/webgl.conf default.conf

WORKDIR /webgl
COPY builds/ .
COPY docker/style.css ./TemplateData/style.css