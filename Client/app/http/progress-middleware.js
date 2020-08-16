function wrap(original, wrapper, name) {
  if (typeof original === 'function') {
    return (e) => {
      wrapper(e)
      original(e)
    }
  }

  if (original === null || typeof original === 'undefined') {
    return wrapper
  }

  throw new Error(`${name} must be a function`)
}

export function progressMiddleware(getProgressBar) {
  let currentRequests = 0

  const onProgress = (e) => {
    if (currentRequests > 0) {
      const progress = (e.loaded * 100) / (e.total * currentRequests)
      getProgressBar().set(Math.min(100, progress))
    }
  }

  return async function (request, next) {
    if (request && request.showProgress === false) {
      await next(request)
      return
    }

    currentRequests++

    const originalUpload = request.onUploadProgress
    const originalDownload = request.onDownloadProgress

    request.onUploadProgress = wrap(
      originalUpload,
      onProgress,
      'onUploadProgress'
    )
    request.onDownloadProgress = wrap(
      originalDownload,
      onProgress,
      'onDownloadProgress'
    )

    try {
      const result = await next(request)

      currentRequests--
      request.onUploadProgress = originalUpload
      request.onDownloadProgress = originalDownload

      if (currentRequests <= 0) {
        currentRequests = 0
        getProgressBar().finish()
      }

      return result
    } catch (e) {
      currentRequests--
      request.onUploadProgress = originalUpload
      request.onDownloadProgress = originalDownload

      getProgressBar().fail()
      getProgressBar().finish()

      throw e
    }
  }
}
