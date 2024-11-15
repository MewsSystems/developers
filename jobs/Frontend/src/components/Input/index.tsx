import { ChangeEvent, Dispatch, ReactNode, SetStateAction } from "react"
import { twMerge } from "tailwind-merge"

interface InputProps {
  label?: string
  prefixIcon?: ReactNode
  suffixIcon?: ReactNode
  placeholder?: string
  className?: string
  required?: boolean
  value: string
  setValue: Dispatch<SetStateAction<string>>
}

export const Input = ({
  label,
  prefixIcon,
  suffixIcon,
  placeholder,
  className,
  required,
  value,
  setValue,
}: InputProps) => {
  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    e.preventDefault()

    setValue(e.target.value)
  }

  return (
    <div className={twMerge("w-full", className)}>
      <p className="mb-1">
        <span>{label}</span>
        {required && <span className="ml-1">*</span>}
      </p>
      <div className="flex w-full items-center rounded border px-3 py-2 text-gray-600">
        {prefixIcon && <span className="mr-2">{prefixIcon}</span>}
        <input
          type="text"
          placeholder={placeholder}
          className="w-full placeholder-gray-300 outline-none"
          value={value}
          onChange={handleChange}
        />
        {suffixIcon && <span className="ml-2">{suffixIcon}</span>}
      </div>
    </div>
  )
}

export default Input
