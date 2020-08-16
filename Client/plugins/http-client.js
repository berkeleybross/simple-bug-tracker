import { jsonClient, transformMiddleware, baseUrl } from 'hippity'
import { progressMiddleware } from '~/app/http/progress-middleware.js'

export default (ctx, inject) => {
  const apiHost = process.env.PROXY_TO || 'http://localhost:5000/'

  const client = jsonClient
    .useIf(!process.server, progressMiddleware(getProgressBar))
    .useLast(
      transformMiddleware([baseUrl(process.server ? `${apiHost}` : '/')])
    )

  ctx.$http = client
  inject('http', client)
}

const noOp = {
  finish: () => {},
  start: () => {},
  fail: () => {},
  set: () => {},
}

function getProgressBar() {
  return window.$nuxt && window.$nuxt.$loading && window.$nuxt.$loading.set
    ? window.$nuxt.$loading
    : noOp
}
