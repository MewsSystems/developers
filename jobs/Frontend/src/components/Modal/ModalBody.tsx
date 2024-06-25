import { ReactNode } from "react"
import { twMerge } from "tailwind-merge"

interface ModalBodyProps {
  className?: string
  children?: ReactNode
}

export const ModalBody = ({ className, children }: ModalBodyProps) => {
  return <div className={twMerge("text-sm text-gray-900", className)}>{children}</div>
}

export default ModalBody
