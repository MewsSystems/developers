import { FetchBaseQueryError } from "@reduxjs/toolkit/query/react"
import { SerializedError } from "@reduxjs/toolkit"

interface ErrorData {
  success: boolean
  status_code: number
  status_message: string
}

function ErrorCard({
  error,
}: {
  error: FetchBaseQueryError | SerializedError
}) {
  let errorMessage: string
  if ("status" in error) {
    errorMessage =
      "error" in error ? error.error : (error.data as ErrorData).status_message
  } else {
    errorMessage = error.message || "Unknown error"
  }

  return (
    <div>
      <p>An error has occurred: {errorMessage}</p>
    </div>
  )
}

export default ErrorCard
