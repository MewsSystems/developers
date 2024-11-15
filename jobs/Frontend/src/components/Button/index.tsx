import { ReactNode } from "react"
import { twMerge } from "tailwind-merge"

interface ButtonProps {
  type?: "button" | "submit" | "reset" | undefined
  icon?: ReactNode
  className?: string
  children?: ReactNode
  onClick?: () => void
}

export const Button = ({ type = "button", icon, className, children, onClick }: ButtonProps) => {
  return (
    <button
      type={type}
      className={twMerge(
        "bg-primary flex items-center rounded px-3 py-2 text-white hover:bg-sky-600",
        className,
      )}
      onClick={() => onClick?.()}
    >
      {icon && <span className={twMerge(children && "mr-2")}>{icon}</span>}
      {children}
    </button>
  )
}

export default Button
