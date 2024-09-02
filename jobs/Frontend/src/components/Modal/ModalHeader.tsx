import { ReactNode } from "react"
import { DialogTitle } from "@headlessui/react"
import { twMerge } from "tailwind-merge"

interface ModalHeaderProps {
  className?: string
  children?: ReactNode
}

export const ModalHeader = ({ className, children }: ModalHeaderProps) => {
  return (
    <DialogTitle as="h3" className={twMerge("mb-2 text-2xl text-gray-900", className)}>
      {children}
    </DialogTitle>
  )
}

export default ModalHeader
