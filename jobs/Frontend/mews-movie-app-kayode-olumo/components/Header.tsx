import Link from "next/link"
import { Button } from "@/components/ui/button"
import { ArrowLeft } from "lucide-react"

interface HeaderProps {
  variant?: "default" | "back"
  title?: string
  subtitle?: string
  backHref?: string
  backLabel?: string
}

export function Header({ 
  variant = "default", 
  title, 
  subtitle, 
  backHref = "/", 
  backLabel = "Back to Movies" 
}: HeaderProps) {
  if (variant === "back") {
    return (
      <header className="border-b bg-background sticky top-0 z-10 backdrop-blur-sm bg-background/95">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
          <Link href={backHref}>
            <Button variant="ghost" className="gap-2">
              <ArrowLeft className="h-4 w-4" />
              <span className="hidden sm:inline">{backLabel}</span>
              <span className="sm:hidden">Back</span>
            </Button>
          </Link>
        </div>
      </header>
    )
  }

  return (
    <header className="mb-8 sm:mb-12">
      {title && (
        <h1 className="text-4xl sm:text-5xl lg:text-6xl font-bold mb-4 sm:mb-6 tracking-tight">
          {title}
        </h1>
      )}
      {subtitle && (
        <p className="text-base sm:text-lg text-muted-foreground max-w-2xl leading-relaxed">
          {subtitle}
        </p>
      )}
    </header>
  )
}
