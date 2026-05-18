FROM node:20-alpine AS build
WORKDIR /app

ENV NODE_OPTIONS="--max-old-space-size=1024"
COPY package*.json ./
RUN npm install --no-fund --no-audit --maxsockets=1 --foreground-scripts
COPY . .

ARG VITE_API_URL
ARG VITE_TAXIMETRO_URL

ENV VITE_API_URL=$VITE_API_URL
ENV VITE_TAXIMETRO_URL=$VITE_TAXIMETRO_URL

RUN npx vite build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]