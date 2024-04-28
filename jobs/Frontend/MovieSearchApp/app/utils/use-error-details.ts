import { isRouteErrorResponse, useRouteError } from "@remix-run/react"

export const useErrorDetails = () => {
  const error = useRouteError()

  let statusCode: number | undefined = undefined
  let statusText: string | undefined = undefined
  let message: string | undefined = undefined

  if (error instanceof Error) {
    message = error.message
  }

  if (isRouteErrorResponse(error)) {
    statusCode = error.status
    statusText = error.statusText
    message = error.data
  }

  return {
    message,
    statusCode,
    statusText,
  }
}
