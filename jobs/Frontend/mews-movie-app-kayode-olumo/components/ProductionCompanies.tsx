interface ProductionCompany {
  id: number
  name: string
  logo_path: string | null
}

interface ProductionCompaniesProps {
  companies: ProductionCompany[]
}

export function ProductionCompanies({ companies }: ProductionCompaniesProps) {
  if (!companies || companies.length === 0) return null

  return (
    <div>
      <h3 className="text-lg sm:text-xl font-bold mb-3">Production Companies</h3>
      <div className="flex flex-wrap gap-3">
        {companies.map((company) => (
          <span key={company.id} className="text-sm bg-secondary px-3 py-1 rounded-lg">
            {company.name}
          </span>
        ))}
      </div>
    </div>
  )
}
