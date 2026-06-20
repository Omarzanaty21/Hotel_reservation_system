# Configuration Guide

## Backend CORS Configuration

### Development (Local)
CORS is configured in `appsettings.json` with allowed origins for local development:
```json
"Cors": {
  "AllowedOrigins": [
    "http://localhost:4200",
    "https://localhost:4200"
  ]
}
```

### Docker/Production
To change CORS origins for Docker or production deployments, set the configuration through environment variables:

**Docker Example:**
```bash
docker run -e Cors__AllowedOrigins__0=http://myapp.com \
           -e Cors__AllowedOrigins__1=https://myapp.com \
           -p 5026:5026 \
           hotel-api:latest
```

**Or using docker-compose.yml:**
```yaml
services:
  api:
    image: hotel-api:latest
    environment:
      - Cors__AllowedOrigins__0=http://myapp.com
      - Cors__AllowedOrigins__1=https://myapp.com
    ports:
      - "5026:5026"
```

---

## Frontend API Host Configuration

### Development (Local)
Angular reads the API host from three sources (in priority order):
1. **Global window object:** `window.APP_CONFIG.apiHost`
2. **LocalStorage:** `localStorage.getItem('apiHost')`
3. **Default config:** `http://localhost:5026`

### Method 1: Dynamic Script Injection (Recommended for Docker)
Create a `config.js` file served before the Angular app loads:

**File:** `public/config.js`
```javascript
window.APP_CONFIG = {
  apiHost: 'http://api.yourdomain.com'
};
```

**In `index.html`, add before `<script src="main.js">`:**
```html
<script src="config.js"></script>
```

### Method 2: Environment Files
Use Angular environment files for build-time configuration:

**Development:**
```bash
ng serve  # Uses src/environments/environment.ts (apiHost: http://localhost:5026)
```

**Production Build:**
```bash
ng build --configuration production  # Uses src/environments/environment.prod.ts
```

### Method 3: LocalStorage Runtime Override
Set the API host at runtime via browser console or by passing a query parameter:

```javascript
localStorage.setItem('apiHost', 'http://api.yourdomain.com');
location.reload();
```

### Docker Compose Example
```yaml
services:
  frontend:
    build: ./Presentation
    ports:
      - "4200:4200"
    environment:
      API_HOST: http://api:5026  # Resolved by Docker network

  api:
    build: ./API
    ports:
      - "5026:5026"
```

Then inject the `config.js` with the environment variable during container startup.

---

## Summary

- **Backend:** CORS origins are configured in `appsettings.json` and can be overridden via ASP.NET Core configuration providers (environment variables, environment-specific JSON files, Docker secrets, etc.)
- **Frontend:** API host is determined at runtime from `window.APP_CONFIG`, localStorage, or defaults, allowing maximum flexibility for external deployments
