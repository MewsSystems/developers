import { ReactNode } from "react"

interface ModalFooterProps {
  className?: string
  children?: ReactNode
}

export const ModalFooter = ({ className, children }: ModalFooterProps) => {
  return <div className={className}>{children}</div>
}

export default ModalFooter
