#!/usr/bin/env bash
# ─────────────────────────────────────────────────────────────────────────────
# Hotel Reservation System — Build & Run Script (Linux / macOS)
# Usage: ./build-and-run.sh
# ─────────────────────────────────────────────────────────────────────────────

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

# ── Resolve ports from .env (fallback to defaults) ───────────────────────────
if [ -f .env ]; then
  # shellcheck disable=SC1091
  set -a; source .env; set +a
fi

FRONTEND_PORT="${FRONTEND_PORT:-80}"
API_PORT="${API_PORT:-8080}"

# ── Banner ────────────────────────────────────────────────────────────────────
echo ""
echo "┌──────────────────────────────────────────────────────────┐"
echo "│        Hotel Reservation System — Docker Setup           │"
echo "└──────────────────────────────────────────────────────────┘"
echo ""

# ── Pre-flight check ──────────────────────────────────────────────────────────
if ! command -v docker &>/dev/null; then
  echo "❌  Docker is not installed or not in PATH. Please install Docker Desktop."
  exit 1
fi

if ! docker compose version &>/dev/null 2>&1 && ! docker-compose version &>/dev/null 2>&1; then
  echo "❌  Docker Compose is not available. Please install Docker Compose."
  exit 1
fi

# Prefer 'docker compose' (v2) over 'docker-compose' (v1)
if docker compose version &>/dev/null 2>&1; then
  COMPOSE="docker compose"
else
  COMPOSE="docker-compose"
fi

# ── Build images ──────────────────────────────────────────────────────────────
echo "▶  Building Docker images (this may take a few minutes on first run)..."
if ! $COMPOSE build --pull; then
  echo "❌  Image build failed. Check the output above."
  exit 1
fi

# ── Start containers ──────────────────────────────────────────────────────────
echo ""
echo "▶  Starting containers..."
if ! $COMPOSE up -d; then
  echo "❌  Failed to start containers. Check the output above."
  exit 1
fi

# ── Wait for frontend to be reachable ─────────────────────────────────────────
echo ""
echo "▶  Waiting for services to become ready..."
MAX_WAIT=90
ELAPSED=0
INTERVAL=3

until curl -sf "http://localhost:${FRONTEND_PORT}" > /dev/null 2>&1; do
  if [ "$ELAPSED" -ge "$MAX_WAIT" ]; then
    echo "⚠️  Frontend did not respond within ${MAX_WAIT}s — check: docker compose logs hotel-frontend"
    break
  fi
  sleep "$INTERVAL"
  ELAPSED=$(( ELAPSED + INTERVAL ))
done

# ── Success message ───────────────────────────────────────────────────────────
echo ""
echo "┌──────────────────────────────────────────────────────────┐"
echo "│  ✅  Hotel Reservation System is up and running!         │"
echo "│                                                          │"
echo "│  🌐  Frontend  →  http://localhost:${FRONTEND_PORT}              │"
echo "│  🔌  API       →  http://localhost:${API_PORT}              │"
echo "│  📖  Swagger   →  (available in Development mode only)   │"
echo "│                                                          │"
echo "│  Admin credentials:  admin / Admin@123                   │"
echo "│                                                          │"
echo "│  To stop:   docker compose down                         │"
echo "│  To rebuild: docker compose build --no-cache            │"
echo "└──────────────────────────────────────────────────────────┘"
echo ""
