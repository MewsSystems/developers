import { DollarSign } from "lucide-react"
import { formatCurrency } from "@/lib/utils/formatters"

interface FinancialInfoProps {
  budget?: number
  revenue?: number
}

export function FinancialInfo({ budget, revenue }: FinancialInfoProps) {
  if (!budget && !revenue) return null

  return (
    <div className="grid sm:grid-cols-2 gap-4">
      {budget && budget > 0 && (
        <div className="p-4 sm:p-6 border-2 rounded-xl overflow-hidden shadow-lg">
          <div className="flex items-center gap-2 mb-2">
            <DollarSign className="h-5 w-5 text-muted-foreground" />
            <span className="text-sm font-semibold uppercase tracking-wide">Budget</span>
          </div>
          <p className="text-xl sm:text-2xl font-bold">{formatCurrency(budget)}</p>
        </div>
      )}

      {revenue && revenue > 0 && (
        <div className="p-4 sm:p-6 border-2 rounded-xl overflow-hidden shadow-lg">
          <div className="flex items-center gap-2 mb-2">
            <DollarSign className="h-5 w-5 text-muted-foreground" />
            <span className="text-sm font-semibold uppercase tracking-wide">Revenue</span>
          </div>
          <p className="text-xl sm:text-2xl font-bold">{formatCurrency(revenue)}</p>
        </div>
      )}
    </div>
  )
}
