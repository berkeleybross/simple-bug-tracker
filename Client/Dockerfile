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

# **********************************************
# test runnable image - run unit tests and get output easily
# **********************************************
FROM build as test

ENTRYPOINT []
CMD []

# **********************************************
# publish temp image - move all files that are needed in production into a single location so that they can be copied in a single layer
# **********************************************
FROM build AS publish

RUN mkdir -p /app/artifacts/dist/.nuxt && \
  cp -r ./.nuxt/dist ./artifacts/dist/.nuxt && \
  cp ./nuxt.config.js ./artifacts/dist/

# **********************************************
# runtime runnable image
# **********************************************
FROM npm AS runtime
ENV NODE_ENV production

COPY package*.json ./
RUN npm ci --only=production

COPY --from=publish /app/artifacts/dist /app

# RUN find . -path ./node_modules -prune -o -print # Uncomment and build docker to list everything in docker context except node_modules.

ENV HOST 0.0.0.0

ENTRYPOINT []
CMD ["node", "server/index.js"]
