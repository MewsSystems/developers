#!/bin/bash
# Script to install and start Redis on Mac/Linux
# Supports both native installation and Docker

CONTAINER_NAME="redis-dev"
REDIS_PORT="6379"
REDIS_PASSWORD=""
WITH_PERSISTENCE=false
USE_DOCKER=false

# Parse command line arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        --docker)
            USE_DOCKER=true
            shift
            ;;
        --password)
            REDIS_PASSWORD="$2"
            shift 2
            ;;
        --port)
            REDIS_PORT="$2"
            shift 2
            ;;
        --persistence)
            WITH_PERSISTENCE=true
            shift
            ;;
        --container-name)
            CONTAINER_NAME="$2"
            shift 2
            ;;
        --help)
            echo "Usage: $0 [OPTIONS]"
            echo ""
            echo "Options:"
            echo "  --docker              Use Docker instead of native installation"
            echo "  --password PASSWORD   Set Redis password"
            echo "  --port PORT           Redis port (default: 6379)"
            echo "  --persistence         Enable data persistence"
            echo "  --container-name NAME Docker container name (default: redis-dev)"
            echo "  --help                Show this help message"
            echo ""
            exit 0
            ;;
        *)
            echo "Unknown option: $1"
            echo "Use --help for usage information"
            exit 1
            ;;
    esac
done

echo "=== Redis Setup Script (Mac/Linux) ==="
echo ""

# Function to setup Redis with Docker
setup_redis_docker() {
    echo "Using Docker to install Redis..."
    echo ""

    # Check if Docker is installed
    if ! command -v docker &> /dev/null; then
        echo "Error: Docker is not installed."
        echo ""
        if [[ "$OSTYPE" == "darwin"* ]]; then
            echo "Install Docker Desktop for Mac:"
            echo "  https://www.docker.com/products/docker-desktop"
        else
            echo "Install Docker:"
            echo "  https://docs.docker.com/engine/install/"
        fi
        exit 1
    fi

    # Check if Docker is running
    if ! docker info &> /dev/null; then
        echo "Error: Docker is not running."
        echo "Please start Docker and try again."
        exit 1
    fi

    echo "Docker is installed and running."
    echo ""

    # Check if port is already in use
    if lsof -Pi :$REDIS_PORT -sTCP:LISTEN -t >/dev/null 2>&1 || netstat -an 2>/dev/null | grep -q ":$REDIS_PORT.*LISTEN"; then
        echo "Warning: Port $REDIS_PORT is already in use."
        echo "This may cause issues if not used by an existing Redis container."
        echo ""
    fi

    # Check if container already exists
    if docker ps -a --format '{{.Names}}' | grep -q "^${CONTAINER_NAME}$"; then
        echo "Redis container already exists."

        # Check if it's running
        if docker ps --format '{{.Names}}' | grep -q "^${CONTAINER_NAME}$"; then
            echo "Container is already running."
        else
            echo "Starting existing container..."
            docker start $CONTAINER_NAME
            echo "Container started successfully."
        fi
    else
        echo "Creating and starting Redis container..."
        echo ""

        # Build docker run command
        DOCKER_CMD="docker run -d --name $CONTAINER_NAME -p $REDIS_PORT:6379"

        # Add persistence if requested
        if [ "$WITH_PERSISTENCE" = true ]; then
            echo "Enabling persistence (AOF + RDB)..."
            VOLUME_NAME="${CONTAINER_NAME}-data"
            docker volume create $VOLUME_NAME >/dev/null 2>&1
            DOCKER_CMD="$DOCKER_CMD -v ${VOLUME_NAME}:/data"
        fi

        # Build Redis command
        REDIS_CMD=""
        if [ -n "$REDIS_PASSWORD" ]; then
            echo "Setting password protection..."
            REDIS_CMD="redis-server --requirepass $REDIS_PASSWORD"
        else
            echo "Warning: No password set. Redis will accept connections without authentication."
            if [ "$WITH_PERSISTENCE" = true ]; then
                REDIS_CMD="redis-server --appendonly yes"
            fi
        fi

        if [ "$WITH_PERSISTENCE" = true ] && [ -n "$REDIS_PASSWORD" ]; then
            REDIS_CMD="$REDIS_CMD --appendonly yes"
        fi

        # Run the container
        if [ -n "$REDIS_CMD" ]; then
            eval "$DOCKER_CMD redis:7-alpine $REDIS_CMD"
        else
            eval "$DOCKER_CMD redis:7-alpine"
        fi

        if [ $? -eq 0 ]; then
            echo "Redis container created and started successfully."
        else
            echo "Error creating container."
            exit 1
        fi

        # Wait for Redis to start
        echo ""
        echo "Waiting for Redis to start..."
        sleep 3
    fi

    # Test connection
    echo ""
    echo "Testing Redis connection..."
    if [ -n "$REDIS_PASSWORD" ]; then
        PONG=$(docker exec $CONTAINER_NAME redis-cli -a "$REDIS_PASSWORD" PING 2>/dev/null)
    else
        PONG=$(docker exec $CONTAINER_NAME redis-cli PING 2>/dev/null)
    fi

    if [ "$PONG" = "PONG" ]; then
        echo "Redis is responding correctly!"
    else
        echo "Warning: Could not verify Redis is responding."
    fi

    echo ""
    echo "=== Setup Complete ==="
    echo ""
    echo "Connection details:"
    echo "  Host: localhost"
    echo "  Port: $REDIS_PORT"
    if [ -n "$REDIS_PASSWORD" ]; then
        echo "  Password: $REDIS_PASSWORD"
    else
        echo "  Password: (none - insecure!)"
    fi
    if [ "$WITH_PERSISTENCE" = true ]; then
        echo "  Persistence: Enabled (AOF + RDB)"
    else
        echo "  Persistence: Disabled (data will be lost on restart)"
    fi
    echo ""
    echo "Connection string:"
    if [ -n "$REDIS_PASSWORD" ]; then
        echo "  localhost:${REDIS_PORT},password=${REDIS_PASSWORD}"
    else
        echo "  localhost:${REDIS_PORT}"
    fi
    echo ""
    echo "Container management:"
    echo "  View logs:    docker logs $CONTAINER_NAME"
    echo "  Stop:         docker stop $CONTAINER_NAME"
    echo "  Start:        docker start $CONTAINER_NAME"
    echo "  Remove:       docker rm -f $CONTAINER_NAME"
    if [ "$WITH_PERSISTENCE" = true ]; then
        echo "  Remove data:  docker volume rm ${CONTAINER_NAME}-data"
    fi
    echo ""
    echo "Redis CLI:"
    if [ -n "$REDIS_PASSWORD" ]; then
        echo "  docker exec -it $CONTAINER_NAME redis-cli -a '$REDIS_PASSWORD'"
    else
        echo "  docker exec -it $CONTAINER_NAME redis-cli"
    fi
    echo ""
}

# Function to setup Redis natively
setup_redis_native() {
    echo "Installing Redis natively..."
    echo ""

    # Detect OS and install Redis
    if [[ "$OSTYPE" == "darwin"* ]]; then
        # macOS
        echo "Detected macOS"

        # Check if Homebrew is installed
        if ! command -v brew &> /dev/null; then
            echo "Error: Homebrew is not installed."
            echo "Install Homebrew from: https://brew.sh"
            echo ""
            echo "Or use Docker: $0 --docker"
            exit 1
        fi

        # Check if Redis is already installed
        if brew list redis &> /dev/null; then
            echo "Redis is already installed via Homebrew."
        else
            echo "Installing Redis via Homebrew..."
            brew install redis

            if [ $? -ne 0 ]; then
                echo "Error installing Redis."
                exit 1
            fi
            echo "Redis installed successfully."
        fi

        # Start Redis service
        echo ""
        echo "Starting Redis service..."
        brew services start redis

        echo ""
        echo "=== Setup Complete ==="
        echo ""
        echo "Connection details:"
        echo "  Host: localhost"
        echo "  Port: 6379"
        echo "  Password: (none by default)"
        echo ""
        echo "Service management:"
        echo "  Stop:    brew services stop redis"
        echo "  Start:   brew services start redis"
        echo "  Restart: brew services restart redis"
        echo ""
        echo "Redis CLI:"
        echo "  redis-cli"
        echo ""
        echo "Configuration file:"
        echo "  $(brew --prefix)/etc/redis.conf"
        echo ""

    elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
        # Linux
        echo "Detected Linux"

        # Detect package manager
        if command -v apt-get &> /dev/null; then
            # Debian/Ubuntu
            echo "Using apt package manager..."

            if dpkg -l | grep -q "^ii.*redis-server"; then
                echo "Redis is already installed."
            else
                echo "Installing Redis..."
                sudo apt-get update
                sudo apt-get install -y redis-server

                if [ $? -ne 0 ]; then
                    echo "Error installing Redis."
                    exit 1
                fi
                echo "Redis installed successfully."
            fi

            # Start Redis service
            echo ""
            echo "Starting Redis service..."
            sudo systemctl start redis-server
            sudo systemctl enable redis-server

            echo ""
            echo "=== Setup Complete ==="
            echo ""
            echo "Connection details:"
            echo "  Host: localhost"
            echo "  Port: 6379"
            echo "  Password: (none by default)"
            echo ""
            echo "Service management:"
            echo "  Stop:    sudo systemctl stop redis-server"
            echo "  Start:   sudo systemctl start redis-server"
            echo "  Restart: sudo systemctl restart redis-server"
            echo "  Status:  sudo systemctl status redis-server"
            echo ""
            echo "Redis CLI:"
            echo "  redis-cli"
            echo ""
            echo "Configuration file:"
            echo "  /etc/redis/redis.conf"
            echo ""

        elif command -v yum &> /dev/null; then
            # RHEL/CentOS/Fedora
            echo "Using yum package manager..."

            if rpm -qa | grep -q "redis"; then
                echo "Redis is already installed."
            else
                echo "Installing Redis..."
                sudo yum install -y redis

                if [ $? -ne 0 ]; then
                    echo "Error installing Redis."
                    exit 1
                fi
                echo "Redis installed successfully."
            fi

            # Start Redis service
            echo ""
            echo "Starting Redis service..."
            sudo systemctl start redis
            sudo systemctl enable redis

            echo ""
            echo "=== Setup Complete ==="
            echo ""
            echo "Connection details:"
            echo "  Host: localhost"
            echo "  Port: 6379"
            echo "  Password: (none by default)"
            echo ""
            echo "Service management:"
            echo "  Stop:    sudo systemctl stop redis"
            echo "  Start:   sudo systemctl start redis"
            echo "  Restart: sudo systemctl restart redis"
            echo "  Status:  sudo systemctl status redis"
            echo ""
            echo "Redis CLI:"
            echo "  redis-cli"
            echo ""
            echo "Configuration file:"
            echo "  /etc/redis.conf"
            echo ""
        else
            echo "Error: Unsupported package manager."
            echo "Please install Redis manually or use Docker: $0 --docker"
            exit 1
        fi
    else
        echo "Error: Unsupported operating system."
        echo "Use Docker: $0 --docker"
        exit 1
    fi
}

# Main execution
if [ "$USE_DOCKER" = true ]; then
    setup_redis_docker
else
    setup_redis_native
fi
