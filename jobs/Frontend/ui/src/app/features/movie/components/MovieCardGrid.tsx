import { cn } from "@/app/lib/utils"

export const MovieCardGrid = ({
  children,
  className,
}: {
  children: React.ReactNode
  className?: string
}) => {
  return (
    <section
      className={cn(
        "grid grid-cols-[repeat(auto-fill,minmax(200px,1fr))] gap-6",
        className,
      )}
    >
      {children}
    </section>
  )
}
