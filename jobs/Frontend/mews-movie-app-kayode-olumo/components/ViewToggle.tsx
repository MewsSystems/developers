"use client"

import { LayoutGrid, List, Rows3 } from "lucide-react"
import { Button } from "@/components/ui/button"

type ViewMode = "grid" | "list" | "compact"

interface ViewToggleProps {
  mode: ViewMode
  onModeChange: (mode: ViewMode) => void
}

export function ViewToggle({ mode, onModeChange }: ViewToggleProps) {
  return (
    <div className="flex gap-1 border border-[#8B7355] rounded-md p-1 bg-[#E8DCC8]">
      <Button
        variant="ghost"
        size="icon"
        className={`h-7 w-7 ${mode === "list" ? "bg-background" : ""}`}
        onClick={() => onModeChange("list")}
      >
        <Rows3 className="h-4 w-4" />
      </Button>
      <Button
        variant="ghost"
        size="icon"
        className={`h-7 w-7 ${mode === "compact" ? "bg-background" : ""}`}
        onClick={() => onModeChange("compact")}
      >
        <List className="h-4 w-4" />
      </Button>
      <Button
        variant="ghost"
        size="icon"
        className={`h-7 w-7 ${mode === "grid" ? "bg-background" : ""}`}
        onClick={() => onModeChange("grid")}
      >
        <LayoutGrid className="h-4 w-4" />
      </Button>
    </div>
  )
}
