# ─── Stage 1: Build Angular application ──────────────────────────────────────
FROM node:22-alpine AS build
WORKDIR /app

# Copy package files first for layer-cached npm install
COPY package.json package-lock.json* ./

# Install dependencies (use ci for reproducible builds)
RUN npm ci --legacy-peer-deps

# Copy the rest of the Angular source
COPY . .

# Build production bundle using the docker configuration
# Angular 22 with @angular/build:application outputs to dist/Presentation/browser
RUN npm run build -- --configuration docker

# ─── Stage 2: Serve with Nginx ───────────────────────────────────────────────
FROM nginx:1.27-alpine AS runtime

# Remove the default nginx config
RUN rm /etc/nginx/conf.d/default.conf

# Copy our custom nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy the compiled Angular app from the build stage
COPY --from=build /app/dist/Presentation/browser /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
