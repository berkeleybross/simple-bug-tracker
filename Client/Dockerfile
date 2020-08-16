# **********************************************
# npm temp image - use an alias so we only have to update version number once
# **********************************************
FROM node:12.16.2-alpine3.11 AS npm
WORKDIR /app

# **********************************************
# build temp image
# **********************************************
FROM npm AS build

COPY package*.json ./
RUN npm ci

COPY . ./
# RUN find . -path ./node_modules -prune -o -print # Uncomment and build docker to list everything in docker context except node_modules.
RUN npm run lint
RUN npm run build

# TODO: only include production NPM packages
RUN find . -path ./node_modules -prune -o -print # Uncomment and build docker to list everything in docker context except node_modules.

ENTRYPOINT []
CMD ["npm", "run", "start"]
