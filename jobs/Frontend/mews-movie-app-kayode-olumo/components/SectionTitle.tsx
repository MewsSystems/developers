interface SectionTitleProps {
  children: React.ReactNode
  className?: string
}

export function SectionTitle({ children, className = "" }: SectionTitleProps) {
  return (
    <h2 className={`text-2xl sm:text-3xl lg:text-4xl font-bold tracking-tight ${className}`}>
      {children}
    </h2>
  )
}
