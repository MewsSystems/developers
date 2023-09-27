import { useEffect } from "react"
import { toast } from "react-toastify"

export const useFailedRequest = (
  isError: boolean = false,
  message: string = "Something went wrong!",
) => {
  useEffect(() => {
    if (isError) {
      toast(message, { type: "error" })
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isError])
}
