# Use official Node.js image
FROM node:20-alpine

# Set working directory inside container
WORKDIR /app

# Copy package.json and package-lock.json first to leverage Docker cache
COPY ./package*.json ./

# Install dependencies
RUN npm install

# Copy all project files
COPY . .

# Expose Vue dev server port
EXPOSE 5173

# Run Vue dev server with host 0.0.0.0 for container access
CMD ["npm", "run", "dev", "--", "--host", "0.0.0.0"]
