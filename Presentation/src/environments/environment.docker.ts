// Docker environment — API calls go through the Nginx reverse proxy on the same host.
// No explicit apiUrl is needed because all /api/* requests are proxied by nginx.conf.
export const environment = {
  production: true,
};
